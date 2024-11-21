using System.Data;
using Library;
using Microsoft.Data.Sqlite;
using static Library.CommonDatabase;

namespace Mappalachia
{
	static class Database
	{
		public static SqliteConnection Connection { get; } = GetNewConnection(Paths.DatabasePath);

		public static List<Space> Spaces { get; } = GetSpaces(Connection); // TODO - somewhere to store/cache these

		public static List<MapMarker> MapMarkers { get; } = GetMapMarkers(Connection);

		// WIP
		public static List<Instance> GetInstances(Entity entity, Space space)
		{
			// TODO this risks "cross-talk" where we don't respect the getSpawnWeight() implementation of the child class of Entity
			// Do we remove the spawn weight from the entity and make it the responsibility of Instance, or do we fully commit?
			string query = $"SELECT * FROM Position WHERE spaceFormID = {space.FormID} AND referenceFormID = {entity.FormID};";

			SqliteDataReader reader = GetReader(Connection, query);
			List<Instance> instances = new List<Instance>();

			while (reader.Read())
			{
				instances.Add(new Instance(
					entity,
					space,
					reader.GetCoord(),
					reader.GetUInt("instanceFormID"),
					reader.GetString("label"),
					reader.GetSpace("teleportsToFormID"),
					reader.GetLockLevel(),
					reader.GetShape()));
			}

			return instances;
		}

		// TODO move reader extensions
		static Coord GetCoord(this SqliteDataReader reader)
		{
			return new Coord(
				reader.GetFloat(reader.GetOrdinal("x")),
				reader.GetFloat(reader.GetOrdinal("y")),
				reader.GetFloat(reader.GetOrdinal("z")));
		}

		// Returns an optional float from the column name of the reader.
		// Given a null or empty, returns 0
		static float GetFloat(this SqliteDataReader reader, string columnName)
		{
			int ordinal = reader.GetOrdinal(columnName);

			if (reader.IsDBNull(ordinal) || string.IsNullOrEmpty(reader.GetString(ordinal)))
			{
				return 0f;
			}

			return reader.GetFloat(ordinal);
		}

		static uint GetUInt(this SqliteDataReader reader, string columnName)
		{
			return (uint)reader.GetInt32(reader.GetOrdinal(columnName));
		}

		static string GetString(this SqliteDataReader reader, string columnName)
		{
			return reader.GetString(reader.GetOrdinal(columnName));
		}

		// Returns the Space with the FormID present in the reader with the given column
		static Space? GetSpace(this SqliteDataReader reader, string columnName)
		{
			int ordinal = reader.GetOrdinal(columnName);

			if (string.IsNullOrEmpty(reader.GetString(ordinal)))
			{
				return null;
			}

			return Spaces.Where(space => space.FormID == reader.GetUInt(columnName)).FirstOrDefault();
		}

		static LockLevel GetLockLevel(this SqliteDataReader reader)
		{
			int ordinal = reader.GetOrdinal("lockLevel");

			if (reader.IsDBNull(ordinal) || string.IsNullOrEmpty(reader.GetString(ordinal)))
			{
				return LockLevel.None;
			}

			return Enum.Parse<LockLevel>(reader.GetString(ordinal).WithoutWhitespace());
		}

		static Shape? GetShape(this SqliteDataReader reader)
		{
			int shapeOrdinal = reader.GetOrdinal("primitiveShape");

			if (reader.IsDBNull(shapeOrdinal) || string.IsNullOrEmpty(reader.GetString(shapeOrdinal)))
			{
				return null;
			}

			return new Shape(Enum.Parse<ShapeType>(reader.GetString(shapeOrdinal)), reader.GetFloat("boundX"), reader.GetFloat("boundY"), reader.GetFloat("boundZ"), reader.GetFloat("rotZ"));
		}
	}
}
