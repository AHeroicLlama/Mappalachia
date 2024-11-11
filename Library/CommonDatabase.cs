using Microsoft.Data.Sqlite;

namespace Library
{
	public static class CommonDatabase
	{
		static SqliteDataReader GetReader(SqliteConnection connection, string queryText)
		{
			SqliteCommand query = connection.CreateCommand();
			query.CommandText = queryText;
			return query.ExecuteReader();
		}

		public static string GetGameVersion(SqliteConnection connection)
		{
			SqliteDataReader reader = GetReader(connection, "SELECT value FROM Meta WHERE Key = 'GameVersion';");
			reader.Read();
			return reader.GetString(0);
		}

		// Returns the results of the given query as a collection of Space
		// All spaces if queryText is null
		public static List<Space> GetSpaces(SqliteConnection connection, string? queryText = null)
		{
			queryText ??= "SELECT * FROM Space ORDER BY isWorldspace DESC, spaceEditorID ASC";

			SqliteDataReader reader = GetReader(connection, queryText);
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

		// Returns a collection of all X/Y coordinates of the given Space
		// TODO: Should we convert this to return MapDataPoint, or similar?
		public static List<Instance> GetSpaceInstances(SqliteConnection connection, Space space)
		{
			SqliteDataReader reader = GetReader(connection, $"SELECT x, y FROM Position WHERE spaceFormID = {space.FormID}");
			List<Instance> coordinates = new List<Instance>();

			while (reader.Read())
			{
				coordinates.Add(new Instance(
					Convert.ToSingle(reader["x"]),
					Convert.ToSingle(reader["y"])));
			}

			return coordinates;
		}

		// Returns the results of the given query as a collection of MapMarker
		public static List<MapMarker> GetMapMarkers(SqliteConnection connection, string queryText)
		{
			SqliteDataReader reader = GetReader(connection, queryText);
			List<MapMarker> mapMarkers = new List<MapMarker>();

			while (reader.Read())
			{
				mapMarkers.Add(new MapMarker(
					(string)reader["icon"],
					(string)reader["label"],
					Convert.ToUInt32(reader["spaceFormID"]),
					Convert.ToDouble(reader["x"]),
					Convert.ToDouble(reader["y"])));
			}

			return mapMarkers;
		}

		// Converts a collection of strings to an SQLite list string suitable for an IN clause
		// EG ["a", "b"] -> "('a','b')"
		public static string ToSqliteCollection(this IEnumerable<string> elements)
		{
			return $"({string.Join(",", elements.Select(e => "\'" + e + "\'"))})";
		}
	}
}
