using Microsoft.Data.Sqlite;

namespace Library
{
	public static class CommonDatabase
	{
		public static SqliteConnection GetNewConnection()
		{
			//TODO
			throw new NotImplementedException();
		}

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
		public static List<Space> GetSpaces(SqliteConnection connection, string queryText)
		{
			SqliteDataReader reader = GetReader(connection, queryText);
			List<Space> spaces = new List<Space>();

			while (reader.Read())
			{
				//TODO use the full ctor - once we've worked out how to do scaling
				spaces.Add(new Space(Convert.ToUInt32(reader["spaceFormID"]), (string)reader["spaceEditorID"], (string)reader["spaceDisplayName"], Convert.ToInt16(reader["isWorldspace"]) == 1));
			}

			return spaces;
		}

		// Returns the results of the given query as a collection of MapMarker
		public static List<MapMarker> GetMapMarkers(SqliteConnection connection, string queryText)
		{
			SqliteDataReader reader = GetReader(connection, queryText);
			List<MapMarker> mapMarkers = new List<MapMarker>();

			while (reader.Read())
			{
				//TODO use the full ctor
				mapMarkers.Add(new MapMarker((string)reader["icon"], (string)reader["label"], Convert.ToUInt32(reader["spaceFormID"])));
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
