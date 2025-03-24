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
			SqliteDataReader reader = await GetReader(connection, "SELECT value FROM Meta WHERE Key = 'GameVersion';");
			reader.Read();
			return reader.GetString(0);
		}

		// Returns the results of the given query as a collection of Space
		// All spaces if queryText is null
		public static async Task<List<Space>> GetSpaces(SqliteConnection connection, string? queryText = null)
		{
			queryText ??= "SELECT * FROM Space ORDER BY isWorldspace DESC, spaceEditorID ASC;";

			SqliteDataReader reader = await GetReader(connection, queryText);
			List<Space> spaces = new List<Space>();

			while (reader.Read())
			{
				spaces.Add(new Space(
					Convert.ToUInt32(reader["spaceFormID"]),
					(string)reader["spaceEditorID"],
					(string)reader["spaceDisplayName"],
					Convert.ToBoolean(reader["isWorldspace"]),
					Convert.ToDouble(reader["centerX"]),
					Convert.ToDouble(reader["centerY"]),
					Convert.ToDouble(reader["maxRange"])));
			}

			return spaces;
		}

		// Returns the Space instance with the editorID from the database
		public static async Task<Space?> GetSpaceByEditorID(SqliteConnection connection, string editorID)
		{
			string queryText = $"SELECT * FROM Space WHERE spaceEditorID = '{editorID}'";

			SqliteDataReader reader = await GetReader(connection, queryText);
			reader.Read();

			if (!reader.HasRows)
			{
				return null;
			}

			return new Space(
				Convert.ToUInt32(reader["spaceFormID"]),
				(string)reader["spaceEditorID"],
				(string)reader["spaceDisplayName"],
				Convert.ToBoolean(reader["isWorldspace"]),
				Convert.ToDouble(reader["centerX"]),
				Convert.ToDouble(reader["centerY"]),
				Convert.ToDouble(reader["maxRange"]));
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
			SqliteDataReader reader = await GetReader(connection, queryText);
			List<Coord> coordinates = new List<Coord>();

			while (reader.Read())
			{
				coordinates.Add(new Coord(
					Convert.ToSingle(reader["x"]),
					Convert.ToSingle(reader["y"])));
			}

			return coordinates;
		}

		// Returns the results of the given query as a collection of MapMarker
		public static async Task<List<MapMarker>> GetMapMarkers(SqliteConnection connection, string? queryText = null)
		{
			queryText ??= "SELECT * FROM MapMarker GROUP BY icon ORDER BY icon ASC;";

			SqliteDataReader reader = await GetReader(connection, queryText);
			List<MapMarker> mapMarkers = new List<MapMarker>();

			while (reader.Read())
			{
				mapMarkers.Add(new MapMarker(
					(string)reader["icon"],
					(string)reader["label"],
					Convert.ToUInt32(reader["spaceFormID"]),
					(float)Convert.ToDouble(reader["x"]),
					(float)Convert.ToDouble(reader["y"])));
			}

			return mapMarkers;
		}

		// Returns the Regions in the given Space, optionally with the specific editorID
		public static async Task<List<Region>> GetRegions(SqliteConnection connection, Space space, string? regionEditorID = null)
		{
			string queryText = $"SELECT * FROM Region WHERE SpaceFormID = {space.FormID}{(regionEditorID == null ? string.Empty : $" AND regionEditorID = '{regionEditorID}'")};";

			SqliteDataReader reader = await GetReader(connection, queryText);
			List<Region> regions = new List<Region>();

			while (reader.Read())
			{
				uint formID = Convert.ToUInt32(reader["regionFormID"]);
				Region? region = regions.Where(r => r.FormID == formID).FirstOrDefault();

				if (region == null)
				{
					region = new Region(
						formID,
						(string)reader["regionEditorID"]);

					regions.Add(region);
				}
				else
				{
					region.AddPoint(new RegionPoint(
						region,
						space,
						new Coord(
							(float)Convert.ToDouble(reader["x"]),
							(float)Convert.ToDouble(reader["y"])),
						Convert.ToUInt32(reader["regionIndex"]),
						Convert.ToUInt32(reader["coordIndex"])));
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
