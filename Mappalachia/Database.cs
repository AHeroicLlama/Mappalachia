using System.Text.RegularExpressions;
using Library;
using Microsoft.Data.Sqlite;
using static Library.CommonDatabase;

namespace Mappalachia
{
	static class Database
	{
		static Regex ExactFormID { get; } = new Regex("^[0-9A-F]{8}$");

		public static SqliteConnection Connection { get; } = GetNewConnection(Paths.DatabasePath);

		public static List<Space> Spaces { get; } = GetSpaces(Connection);

		public static List<MapMarker> MapMarkers { get; } = GetMapMarkers(Connection);

		public static List<GroupedInstance> Search(string searchTerm, Space selectedSpace, List<Signature> selectedSignatures, List<LockLevel> selectedLockLevels)
		{
			searchTerm = ProcessSearchString(searchTerm);
			List<GroupedInstance> results = new List<GroupedInstance>();

			// If the search term is a FormID, we perform further specific searches against this
			bool searchIsFormID = ExactFormID.IsMatch(searchTerm);

			string optionalExactFormIDTerm = searchIsFormID ? $"referenceFormID = '{searchTerm}' OR " : string.Empty;
			string optionalSpaceTerm = selectedSpace != null ? $"AND Position_PreGrouped.spaceFormID = {selectedSpace.FormID} " : string.Empty;

			string queryStandard =
				"SELECT * FROM Position_PreGrouped " +
				"JOIN Entity ON Entity.entityFormID = Position_PreGrouped.referenceFormID " +
				"JOIN Space on Space.spaceFormID = Position_PreGrouped.spaceFormID " +
				$"WHERE ({optionalExactFormIDTerm} label LIKE '{searchTerm}' OR editorID LIKE '{searchTerm}' or displayName LIKE '{searchTerm}') " +
				$"{optionalSpaceTerm} AND signature IN {selectedSignatures.ToSqliteCollection()} AND lockLevel IN {selectedLockLevels.ToSqliteCollection()} " +
				"ORDER BY Space.spaceDisplayName, count DESC";

			//TODO repeat for npc, scrap, instanceFormID

			return results;
		}

		// Escape functional SQL characters and wildcard on space
		static string ProcessSearchString(string input)
		{
			return "%" + input.Trim()
				.Replace("'", "''")
				.Replace("_", "\\_")
				.Replace("%", "\\%")
				.Replace(" ", "%") + "%";
		}

		// WIP
		//public static List<Instance> GetInstances(Entity entity, Space space)
		//{
		//	string query = $"SELECT * FROM Position WHERE spaceFormID = {space.FormID} AND referenceFormID = {entity.FormID};";

		//	SqliteDataReader reader = GetReader(Connection, query);
		//	List<Instance> instances = new List<Instance>();

		//	while (reader.Read())
		//	{
		//		instances.Add(new Instance(
		//			entity,
		//			space,
		//			reader.GetCoord(),
		//			reader.GetUInt("instanceFormID"),
		//			reader.GetString("label"),
		//			reader.GetSpace("teleportsToFormID"),
		//			reader.GetLockLevel(),
		//			reader.GetShape()));
		//	}

		//	return instances;
		//}
	}
}
