using System.Text.RegularExpressions;
using Library;
using Microsoft.Data.Sqlite;
using static Library.CommonDatabase;

namespace Mappalachia
{
	static class Database
	{
		static Regex ExactFormID { get; } = new Regex("^[0-9A-F]{8}$");

		static SqliteConnection Connection { get; } = GetNewConnection(Paths.DatabasePath);

		public static List<Space> Spaces { get; } = GetSpaces(Connection);

		public static List<MapMarker> MapMarkers { get; } = GetMapMarkers(Connection);

		public static List<GroupedInstance> Search(string searchTerm, Space selectedSpace, List<Signature> selectedSignatures, List<LockLevel> selectedLockLevels)
		{
			searchTerm = ProcessSearchString(searchTerm);
			List<GroupedInstance> results = new List<GroupedInstance>();

			// If the search term is a FormID, we perform further specific searches against this
			bool searchIsFormID = ExactFormID.IsMatch(searchTerm);

			// 'Standard'
			string optionalExactFormIDTerm = searchIsFormID ? $"referenceFormID = '{searchTerm}' OR " : string.Empty;
			string optionalSpaceTerm = selectedSpace != null ? $"AND Space.spaceFormID = {selectedSpace.FormID} " : string.Empty;

			string query =
				"SELECT * FROM Position_PreGrouped " + // TODO which columns
				"JOIN Entity ON Entity.entityFormID = Position_PreGrouped.referenceFormID " +
				"JOIN Space on Space.spaceFormID = Position_PreGrouped.spaceFormID " +
				$"WHERE ({optionalExactFormIDTerm} label LIKE '%{searchTerm}%' OR editorID LIKE '%{searchTerm}%' or displayName LIKE '%{searchTerm}%') " +
				$"{optionalSpaceTerm} AND signature IN {selectedSignatures.ToSqliteCollection()} AND lockLevel IN {selectedLockLevels.ToSqliteCollection()} " +
				"ORDER BY Space.spaceDisplayName, count DESC"; // TODO, do we bother sorting? If we're aggregating anyway

			SqliteDataReader reader = GetReader(Connection, query);

			while (reader.Read())
			{
				results.Add(new GroupedInstance(
					new Entity(
						reader.GetUInt("referenceFormID"),
						reader.GetString("editorID"),
						reader.GetString("displayName"),
						reader.GetSignature()),
					selectedSpace ?? GetSpaceByFormID(reader.GetUInt("spaceFormID")),
					reader.GetInt("count"),
					0,
					reader.GetString("label"),
					reader.GetLockLevel(),
					(100 - reader.GetInt("percChanceNone")) / 100f));
			}

			// NPC
			query =
				"SELECT *, count(*) as count FROM NPC " + // TODO which columns
				"JOIN Space ON Space.spaceFormID = NPC.spaceFormID " +
				$"WHERE npcName LIKE '%{searchTerm}%' " +
				optionalSpaceTerm +
				"GROUP BY NPC.spaceFormID, npcName, spawnWeight " +
				"ORDER BY spaceDisplayName, npcName, spawnWeight DESC, count DESC"; // TODO, do we bother sorting? If we're aggregating anyway

			reader = GetReader(Connection, query);

			while (reader.Read())
			{
				results.Add(new GroupedInstance(
					new DerivedNPC(reader.GetString("npcName")),
					selectedSpace ?? GetSpaceByFormID(reader.GetUInt("spaceFormID")),
					reader.GetInt("count"),
					0,
					string.Empty,
					LockLevel.None,
					reader.GetFloat("spawnWeight")));
			}

			// Scrap
			query =
				"SELECT *, componentQuantity * sum(count) as weight FROM Scrap " + // TODO which columns
				"JOIN Position_PreGrouped ON Position_PreGrouped.referenceFormID = Scrap.junkFormID " +
				$"WHERE component LIKE '%{searchTerm}%' " +
				optionalSpaceTerm +
				"GROUP BY component, spaceFormID, componentQuantity" +
				"ORDER BY component, weight DESC, componentQuantity DESC"; // TODO, do we bother sorting? If we're aggregating anyway

			reader = GetReader(Connection, query);

			while (reader.Read())
			{
				results.Add(new GroupedInstance(
					new DerivedScrap(reader.GetString("component")),
					selectedSpace ?? GetSpaceByFormID(reader.GetUInt("spaceFormID")),
					reader.GetInt("weight"), // TODO, scrap or junk count - interacts with spawnWeight-componentQuantity
					0,
					string.Empty,
					LockLevel.None,
					reader.GetFloat("componentQuantity")));
			}

			// FormID-specific
			if (searchIsFormID)
			{
				query =
					"SELECT * FROM Position " + // TODO which columns
					"JOIN Entity ON Entity.entityFormID = Position.referenceFormID " +
					"JOIN Space ON Space.spaceFormID = Position.spaceFormID " +
					$"WHERE instanceFormID = '{searchTerm}' OR teleportsToFormID = '{searchTerm}'" +
					optionalSpaceTerm;

				reader = GetReader(Connection, query);

				//TODO
				//while (reader.Read())
				//{
				//	results.Add(new GroupedInstance(
				//		new Entity(
				//			),
				//		selectedSpace ?? GetSpaceByFormID(reader.GetUInt("spaceFormID")),
				//		reader.GetInt("count"),
				//		0,
				//		string.Empty,
				//		LockLevel.None,
				//		reader.GetFloat("componentQuantity")));
				//}
			}

			//TODO instanceFormID

			return results;
		}

		// Escape functional SQL characters and wildcard on space
		static string ProcessSearchString(string input)
		{
			return input.Trim()
				.Replace("'", "''")
				.Replace("_", "\\_")
				.Replace("%", "\\%")
				.Replace(" ", "%");
		}

		static Space GetSpaceByFormID(uint formID)
		{
			return Spaces.Where(space => space.FormID == formID).FirstOrDefault() ?? throw new Exception($"No Space with formID {formID} was found");
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
