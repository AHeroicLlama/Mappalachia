using Library;
using Microsoft.Data.Sqlite;
using static Library.CommonDatabase;

namespace Mappalachia
{
	static class Database
	{
		public static SqliteConnection Connection { get; } = GetNewConnection(Paths.DatabasePath);

		public static List<Space> Spaces { get; } = GetSpaces(Connection);

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
	}
}
