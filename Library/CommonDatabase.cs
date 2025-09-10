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
		public static async Task<SqliteDataReader> GetReader(SqliteConnection connection, string queryText, Dictionary<string, object>? parameters = null)
		{
			SqliteCommand query = connection.CreateCommand();
			query.CommandText = queryText;

			if (parameters != null)
			{
				foreach (KeyValuePair<string, object> param in parameters)
				{
					query.Parameters.AddWithValue(param.Key, param.Value);
				}
			}

			return await query.ExecuteReaderAsync().ConfigureAwait(false);
		}

		public static async Task<string> GetGameVersion(SqliteConnection connection)
		{
			using SqliteDataReader reader = await GetReader(connection, "SELECT value FROM Meta WHERE Key = 'GameVersion';");
			reader.Read();
			return reader.GetString(0);
		}

		// Returns the results of the given query as a collection of MapMarker
		public static async Task<List<MapMarker>> GetMapMarkers(SqliteConnection connection, string queryText)
		{
			using SqliteDataReader reader = await GetReader(connection, queryText);
			List<MapMarker> mapMarkers = new List<MapMarker>();

			while (reader.Read())
			{
				mapMarkers.Add(new MapMarker(
					reader.GetString("icon"),
					reader.GetString("label"),
					reader.GetUInt("spaceFormID"),
					new Coord(
						reader.GetDouble("x"),
						reader.GetDouble("y"))));
			}

			return mapMarkers;
		}

		// Returns the results of the given query as a collection of Space
		public static async Task<List<Space>> GetSpaces(SqliteConnection connection, string queryText)
		{
			using SqliteDataReader reader = await GetReader(connection, queryText);
			List<Space> spaces = new List<Space>();

			while (reader.Read())
			{
				spaces.Add(new Space(
					reader.GetUInt("spaceFormID"),
					reader.GetString("spaceEditorID"),
					reader.GetString("spaceDisplayName"),
					reader.GetBool("isWorldspace"),
					reader.GetBool("isInstanceable"),
					reader.GetDouble("centerX"),
					reader.GetDouble("centerY"),
					reader.GetDouble("maxRange"),
					reader.GetDouble("northAngle")));
			}

			return spaces;
		}

		// Returns a new List of all spaces, ordered by worlspace and editorID
		public static async Task<List<Space>> GetAllSpaces(SqliteConnection connection)
		{
			return await GetSpaces(connection, "SELECT * FROM Space ORDER BY isWorldspace DESC, spaceEditorID ASC");
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
				reader.GetBool("isInstanceable"),
				reader.GetDouble("centerX"),
				reader.GetDouble("centerY"),
				reader.GetDouble("maxRange"),
				0);
		}

		// Returns a collection of all coordinates of the given Space
		public static async Task<List<Coord>> GetAllCoords(SqliteConnection connection, Space space)
		{
			string queryText = $"SELECT x, y, z FROM Position WHERE spaceFormID = {space.FormID};";
			using SqliteDataReader reader = await GetReader(connection, queryText);
			List<Coord> coordinates = new List<Coord>();

			while (reader.Read())
			{
				coordinates.Add(reader.GetCoord());
			}

			return coordinates;
		}

		// Returns the regions in the given space where the editorIDs match the like term
		public static async Task<List<Region>> GetRegionsByLikeTerm(SqliteConnection connection, Space space, string likeTerm)
		{
			string queryText = $"SELECT * FROM Region WHERE regionEditorID LIKE {likeTerm} AND spaceFormID = {space.FormID};";

			using SqliteDataReader regionReader = await GetReader(connection, queryText);
			List<Region> regions = new List<Region>();

			while (regionReader.Read())
			{
				Region region = new Region(
					regionReader.GetUInt("regionFormID"),
					regionReader.GetString("regionEditorID"),
					space,
					regionReader.GetUInt("minLevel"),
					regionReader.GetUInt("maxLevel"));

				regions.Add(region);
			}

			// Populate the region points
			foreach (Region region in regions)
			{
				string pointQuery = $"SELECT x, y, subRegionIndex, coordIndex FROM RegionPoints WHERE regionFormID = {region.FormID};";
				using SqliteDataReader pointReader = await GetReader(connection, pointQuery);

				while (pointReader.Read())
				{
					region.AddPoint(new RegionPoint(
					region,
					new Coord(
						pointReader.GetDouble("x"),
						pointReader.GetDouble("y")),
					pointReader.GetUInt("subRegionIndex"),
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
