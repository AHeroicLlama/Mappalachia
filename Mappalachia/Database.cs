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

		public static List<Space> CachedSpaces { get; } = GetSpaces(Connection).Result;

		public static List<MapMarker> CachedMapMarkers { get; } = GetMapMarkers(Connection).Result;

		static string ToStringForQuery(this LockLevel lockLevel)
		{
			if (lockLevel == LockLevel.None)
			{
				return string.Empty;
			}

			return lockLevel.ToString();
		}

		public static async Task<List<GroupedInstance>> Search(string searchTerm, Space? selectedSpace = null, List<Signature>? selectedSignatures = null, List<LockLevel>? selectedLockLevels = null)
		{
			selectedSignatures ??= Enum.GetValues<Signature>().ToList();
			selectedLockLevels ??= Enum.GetValues<LockLevel>().ToList();

			searchTerm = ProcessSearchString(searchTerm);
			List<GroupedInstance> results = new List<GroupedInstance>();

			// If the search term is a FormID, we perform further specific searches against this
			bool searchIsFormID = ExactFormID.IsMatch(searchTerm.ToUpper());

			// 'Standard'
			string optionalExactFormIDTerm = searchIsFormID ? $"referenceFormID = '{searchTerm}' OR " : string.Empty;
			string optionalSpaceTerm = selectedSpace != null ? $"AND spaceFormID = {selectedSpace.FormID} " : string.Empty;

			string query = "SELECT referenceFormID, editorID, displayName, signature, spaceFormID, count, label, lockLevel, percChanceNone FROM Position_PreGrouped " +
				"JOIN Entity ON Entity.entityFormID = Position_PreGrouped.referenceFormID " +
				$"WHERE ({optionalExactFormIDTerm} label LIKE '%{searchTerm}%' OR editorID LIKE '%{searchTerm}%' or displayName LIKE '%{searchTerm}%') " +
				optionalSpaceTerm +
				$"AND Position_PreGrouped.lockLevel IN {selectedLockLevels.Select(l => l.ToStringForQuery()).ToSqliteCollection()} " +
				$"AND Entity.signature IN {selectedSignatures.ToSqliteCollection()};";

			SqliteDataReader reader = await GetReader(Connection, query);

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
				"SELECT npcName, spaceFormID, spawnWeight, count(*) as count FROM NPC " + // TODO which columns
				$"WHERE npcName LIKE '%{searchTerm}%' " +
				optionalSpaceTerm +
				"GROUP BY NPC.spaceFormID, npcName, spawnWeight;";

			reader = await GetReader(Connection, query);

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
			query = "SELECT component, spaceFormID, componentQuantity, sum(count) as properCount FROM Scrap " +
				"JOIN Position_PreGrouped ON Position_PreGrouped.referenceFormID = Scrap.junkFormID " +
				$"WHERE component LIKE '%{searchTerm}%' " +
				optionalSpaceTerm +
				"GROUP BY component, spaceFormID, componentQuantity;";

			reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				results.Add(new GroupedInstance(
					new DerivedScrap(reader.GetString("component")),
					selectedSpace ?? GetSpaceByFormID(reader.GetUInt("spaceFormID")),
					reader.GetInt("properCount"),
					0,
					string.Empty,
					LockLevel.None,
					reader.GetFloat("componentQuantity")));
			}

			// Region
			query =
				"SELECT regionFormID, regionEditorID, spaceFormID FROM Region " +
				$"WHERE (regionEditorID LIKE '%{searchTerm}%' OR regionFormID = '{searchTerm}') " +
				optionalSpaceTerm +
				"GROUP BY regionFormID;";

			reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				results.Add(new GroupedInstance(
					new Library.Region(
						reader.GetUInt("regionFormID"),
						reader.GetString("regionEditorID")),
					selectedSpace ?? GetSpaceByFormID(reader.GetUInt("spaceFormID")),
					1,
					0,
					string.Empty,
					LockLevel.None));
			}

			// FormID-specific
			if (searchIsFormID)
			{
				query =
					"SELECT referenceFormID, editorID, displayName, signature, spaceFormID, label, lockLevel, percChanceNone, count(*) as count FROM Position " +
					"JOIN Entity ON Entity.entityFormID = Position.referenceFormID " +
					"WHERE " +
					optionalSpaceTerm +
					$"(instanceFormID = '{searchTerm}' OR teleportsToFormID = '{searchTerm}') " +
					"GROUP BY spaceFormID, Position.referenceFormID, teleportsToFormID, lockLevel, label;";

				reader = await GetReader(Connection, query);

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
			}

			reader.Dispose();
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
			return CachedSpaces.Where(space => space.FormID == formID).FirstOrDefault() ?? throw new Exception($"No Space with formID {formID} was found");
		}
	}
}
