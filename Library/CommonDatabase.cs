using Microsoft.Data.Sqlite;

namespace Library
{
	public static class CommonDatabase
	{
		public static SqliteConnection GetNewConnection(string path, bool accessReadonly = true)
		{
			string connectionString = $"Data Source={path};Pooling=false";

			if (accessReadonly)
			{
				connectionString += ";Mode=readonly";
			}

			SqliteConnection connection = new SqliteConnection(connectionString);
			connection.Open();
			return connection;
		}

		// Shortcut to return a reader with the given query already executed
		public static async Task<SqliteDataReader> GetReader(SqliteConnection connection, string queryText)
		{
			SqliteCommand query = connection.CreateCommand();
			query.CommandText = queryText;
			return await query.ExecuteReaderAsync();
		}

		public static async Task<string> GetGameVersion(SqliteConnection connection)
		{
			using SqliteDataReader reader = await GetReader(connection, "SELECT value FROM Meta WHERE Key = 'GameVersion';");
			reader.Read();
			return reader.GetString(0);
		}

		// Returns the results of the given query as a collection of Space
		// All spaces if queryText is null
		public static async Task<List<Space>> GetSpaces(SqliteConnection connection, string? queryText = null)
		{
			queryText ??= "SELECT * FROM Space ORDER BY isWorldspace DESC, spaceEditorID ASC;";

			using SqliteDataReader reader = await GetReader(connection, queryText);
			List<Space> spaces = new List<Space>();

			while (reader.Read())
			{
				spaces.Add(new Space(
					reader.GetUInt("spaceFormID"),
					reader.GetString("spaceEditorID"),
					reader.GetString("spaceDisplayName"),
					reader.GetBool("isWorldspace"),
					reader.GetDouble("centerX"),
					reader.GetDouble("centerY"),
					reader.GetDouble("maxRange")));
			}

			return spaces;
		}

		// Returns the Space instance with the editorID from the database
		public static async Task<Space?> GetSpaceByEditorID(SqliteConnection connection, string editorID)
		{
			string queryText = $"SELECT * FROM Space WHERE spaceEditorID = '{editorID}'";

			using SqliteDataReader reader = await GetReader(connection, queryText);
			reader.Read();

			if (!reader.HasRows)
			{
				return null;
			}

			return new Space(
				reader.GetUInt("spaceFormID"),
				reader.GetString("spaceEditorID"),
				reader.GetString("spaceDisplayName"),
				reader.GetBool("isWorldspace"),
				reader.GetDouble("centerX"),
				reader.GetDouble("centerY"),
				reader.GetDouble("maxRange"));
		}

		// Returns a collection of all X/Y coordinates of the given Space
		public static async Task<List<Coord>> GetCoords(SqliteConnection connection, Space space)
		{
			string queryText = $"SELECT x, y FROM Position WHERE spaceFormID = {space.FormID};";
			return await GetCoords(connection, queryText);
		}

		// Returns the results of the given query as a collection of Coord
		public static async Task<List<Coord>> GetCoords(SqliteConnection connection, string queryText)
		{
			using SqliteDataReader reader = await GetReader(connection, queryText);
			List<Coord> coordinates = new List<Coord>();

			while (reader.Read())
			{
				coordinates.Add(new Coord(
					reader.GetFloat("x"),
					reader.GetFloat("y")));
			}

			return coordinates;
		}

		// Returns the results of the given query as a collection of MapMarker
		public static async Task<List<MapMarker>> GetMapMarkers(SqliteConnection connection, string? queryText = null)
		{
			queryText ??= "SELECT * FROM MapMarker GROUP BY icon ORDER BY icon ASC;";

			using SqliteDataReader reader = await GetReader(connection, queryText);
			List<MapMarker> mapMarkers = new List<MapMarker>();

			while (reader.Read())
			{
				mapMarkers.Add(new MapMarker(
					reader.GetString("icon"),
					reader.GetString("label"),
					reader.GetUInt("spaceFormID"),
					new Coord(
						reader.GetFloat("x"),
						reader.GetFloat("y"))));
			}

			return mapMarkers;
		}

		// Returns the Regions in the given Space, optionally with the specific editorID
		public static async Task<List<Region>> GetRegionsFromSpace(SqliteConnection connection, Space space, string? regionEditorID = null)
		{
			string regionQuery = $"SELECT * FROM Region WHERE SpaceFormID = {space.FormID}{(regionEditorID == null ? string.Empty : $" AND regionEditorID = '{regionEditorID}'")};";

			using SqliteDataReader regionReader = await GetReader(connection, regionQuery);
			List<Region> regions = new List<Region>();

			while (regionReader.Read())
			{
				Region region = new Region(
					regionReader.GetUInt("regionFormID"),
					regionReader.GetString("regionEditorID"),
					regionReader.GetUInt("minLevel"),
					regionReader.GetUInt("maxLevel"));

				regions.Add(region);
			}

			// Populate the region points
			foreach (Region region in regions)
			{
				string pointQuery = $"SELECT x, y, regionIndex, coordIndex FROM RegionPoints WHERE regionFormID = {region.FormID};";
				using SqliteDataReader pointReader = await GetReader(connection, pointQuery);

				while (pointReader.Read())
				{
					region.AddPoint(new RegionPoint(
					region,
					space,
					new Coord(
						pointReader.GetFloat("x"),
						pointReader.GetFloat("y")),
					pointReader.GetUInt("regionIndex"),
					pointReader.GetUInt("coordIndex")));
				}
			}

			return regions;
		}

		// Converts a collection of strings to an SQLite list string suitable for an IN clause
		// EG ["a", "b"] -> "('a','b')"
		public static string ToSqliteCollection<T>(this IEnumerable<T> elements)
		{
			if (!typeof(T).IsEnum && typeof(T) != typeof(string))
			{
				throw new ArgumentException("Only string or enum collections are valid.");
			}

			return $"({string.Join(",", elements.Select(e => "\'" + e?.ToString() + "\'"))})";
		}
	}
}
