using Microsoft.Data.Sqlite;

namespace Library
{
	// Extension methods for SqliteDataReader
	public static class ReaderExtensions
	{
		public static double GetDouble(this SqliteDataReader reader, string columnName)
		{
			return reader.GetDouble(reader.GetOrdinal(columnName));
		}

		public static float GetFloat(this SqliteDataReader reader, string columnName)
		{
			return reader.GetFloat(reader.GetOrdinal(columnName));
		}

		public static int GetInt(this SqliteDataReader reader, string columnName)
		{
			return reader.GetInt32(reader.GetOrdinal(columnName));
		}

		public static uint GetUInt(this SqliteDataReader reader, string columnName)
		{
			return (uint)reader.GetInt32(reader.GetOrdinal(columnName));
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
				reader.GetFloat(reader.GetOrdinal("x")),
				reader.GetFloat(reader.GetOrdinal("y")),
				reader.GetFloat(reader.GetOrdinal("z")));
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
				reader.GetFloat("boundX"),
				reader.GetFloat("boundY"),
				reader.GetFloat("boundZ"),
				reader.GetFloat("rotZ"));
		}

		public static Signature GetSignature(this SqliteDataReader reader)
		{
			string signature = reader.GetString("signature");

			return Enum.Parse<Signature>(signature);
		}
	}
}
