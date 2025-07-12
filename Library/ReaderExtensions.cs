using Microsoft.Data.Sqlite;

namespace Library
{
	// Extension methods for SqliteDataReader
	public static class ReaderExtensions
	{
		public static double GetDouble(this SqliteDataReader reader, string columnName)
		{
			int ordinal = reader.GetOrdinal(columnName);

			if (reader.IsDBNull(ordinal))
			{
				return 0;
			}

			return reader.GetDouble(ordinal);
		}

		public static int GetInt(this SqliteDataReader reader, string columnName)
		{
			return reader.GetInt32(reader.GetOrdinal(columnName));
		}

		public static uint GetUInt(this SqliteDataReader reader, string columnName)
		{
			int ordinal = reader.GetOrdinal(columnName);

			if (reader.IsDBNull(ordinal))
			{
				return 0;
			}

			return (uint)reader.GetInt32(ordinal);
		}

		public static string GetString(this SqliteDataReader reader, string columnName)
		{
			return reader.GetString(reader.GetOrdinal(columnName));
		}

		public static bool GetBool(this SqliteDataReader reader, string columnName)
		{
			return reader.GetBoolean(reader.GetOrdinal(columnName));
		}

		public static Coord GetCoord(this SqliteDataReader reader)
		{
			return new Coord(
				reader.GetDouble("x"),
				reader.GetDouble("y"),
				reader.GetDouble("z"));
		}

		public static LockLevel GetLockLevel(this SqliteDataReader reader)
		{
			int ordinal = reader.GetOrdinal("lockLevel");

			if (reader.IsDBNull(ordinal) || string.IsNullOrEmpty(reader.GetString(ordinal)))
			{
				return LockLevel.None;
			}

			return Enum.Parse<LockLevel>(reader.GetString(ordinal).WithoutWhitespace());
		}

		public static Shape? GetShape(this SqliteDataReader reader)
		{
			int shapeOrdinal = reader.GetOrdinal("primitiveShape");

			if (reader.IsDBNull(shapeOrdinal) || string.IsNullOrEmpty(reader.GetString(shapeOrdinal)))
			{
				return null;
			}

			return new Shape(
				Enum.Parse<ShapeType>(reader.GetString(shapeOrdinal)),
				reader.GetDouble("boundX"),
				reader.GetDouble("boundY"),
				reader.GetDouble("boundZ"),
				reader.GetDouble("rotZ"));
		}

		public static Signature GetSignature(this SqliteDataReader reader)
		{
			string signature = reader.GetString("signature");

			return Enum.Parse<Signature>(signature);
		}
	}
}
