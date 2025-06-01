using Library;
using Microsoft.Data.Sqlite;
using static Library.Common;
using static Library.CommonDatabase;

namespace Mappalachia
{
	static class Database
	{
		public static SqliteConnection Connection { get; } = GetNewConnection(Paths.DatabasePath);

		public static List<Space> AllSpaces { get; } = GetSpaces(Connection, "SELECT * FROM Space ORDER BY isWorldspace DESC, spaceDisplayName ASC").Result;

		public static List<MapMarker> AllMapMarkers { get; } = GetMapMarkers(Connection, "SELECT * FROM MapMarker").Result;

		static char EscapeChar { get; } = '`';

		// The core database search function - returns a collection of GroupedInstance from the given search params
		public static async Task<List<GroupedInstance>> Search(Settings settings)
		{
			string searchTerm = ProcessSearchString(settings.SearchSettings.SearchTerm);
			List<GroupedInstance> results = new List<GroupedInstance>();

			// If the search term is a FormID, we perform further specific searches against this
			bool searchIsFormID = searchTerm.IsHexFormID();

			// 'Standard'
			string optionalExactFormIDTerm = searchIsFormID ? $"referenceFormID = '{HexToInt(searchTerm)}' OR " : string.Empty;
			string optionalSpaceTerm = settings.SearchSettings.SearchInAllSpaces ? string.Empty : $"AND spaceFormID = {settings.Space.FormID} ";

			string query = "SELECT referenceFormID, editorID, displayName, signature, spaceFormID, count, label, lockLevel, percChanceNone FROM Position_PreGrouped " +
				"JOIN Entity ON Entity.entityFormID = Position_PreGrouped.referenceFormID " +
				$"WHERE ({optionalExactFormIDTerm} label LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' OR editorID LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' or displayName LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}') " +
				optionalSpaceTerm +
				$"AND Position_PreGrouped.lockLevel IN {settings.SearchSettings.SelectedLockLevels.Select(l => l.ToStringForQuery()).ToSqliteCollection()} " +
				$"AND Entity.signature IN {settings.SearchSettings.SelectedSignatures.ToSqliteCollection()};";

			SqliteDataReader reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				results.Add(new GroupedInstance(
					new Entity(
						reader.GetUInt("referenceFormID"),
						reader.GetString("editorID"),
						reader.GetString("displayName"),
						reader.GetSignature()),
					GetSpaceByFormID(reader.GetUInt("spaceFormID")),
					reader.GetInt("count"),
					0,
					reader.GetString("label"),
					reader.GetLockLevel(),
					(100 - reader.GetInt("percChanceNone")) / 100f));
			}

			// NPC
			query =
				"SELECT npcName, spaceFormID, spawnWeight, count(*) as count FROM NPC " +
				$"WHERE npcName LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' " +
				optionalSpaceTerm +
				"GROUP BY NPC.spaceFormID, npcName, spawnWeight;";

			reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				results.Add(new GroupedInstance(
					new DerivedNPC(reader.GetString("npcName")),
					GetSpaceByFormID(reader.GetUInt("spaceFormID")),
					reader.GetInt("count"),
					0,
					string.Empty,
					LockLevel.None,
					reader.GetFloat("spawnWeight")));
			}

			// Scrap
			query = "SELECT component, spaceFormID, componentQuantity, sum(count) as properCount FROM Scrap " +
				"JOIN Position_PreGrouped ON Position_PreGrouped.referenceFormID = Scrap.junkFormID " +
				$"WHERE component LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' " +
				optionalSpaceTerm +
				"GROUP BY component, spaceFormID, componentQuantity;";

			reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				results.Add(new GroupedInstance(
					new DerivedScrap(reader.GetString("component")),
					GetSpaceByFormID(reader.GetUInt("spaceFormID")),
					reader.GetInt("properCount"),
					0,
					string.Empty,
					LockLevel.None,
					reader.GetFloat("componentQuantity")));
			}

			// Region
			query =
				"SELECT regionFormID, regionEditorID, spaceFormID FROM Region " +
				$"WHERE (regionEditorID LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' OR regionFormID = '{searchTerm}') " +
				optionalSpaceTerm +
				"GROUP BY regionFormID;";

			reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				Space space = GetSpaceByFormID(reader.GetUInt("spaceFormID"));

				results.Add(new GroupedInstance(
					new Library.Region(
						reader.GetUInt("regionFormID"),
						reader.GetString("regionEditorID"),
						space),
					space,
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
					optionalExactFormIDTerm +
					$"(instanceFormID = '{HexToInt(searchTerm)}' OR teleportsToFormID = '{HexToInt(searchTerm)}') " +
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
						GetSpaceByFormID(reader.GetUInt("spaceFormID")),
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
				.Replace("_", $"{EscapeChar}_")
				.Replace("%", $"{EscapeChar}%")
				.Replace(" ", "%");
		}

		static string ToStringForQuery(this LockLevel lockLevel)
		{
			if (lockLevel == LockLevel.None)
			{
				return string.Empty;
			}

			return lockLevel.ToString();
		}

		static Space GetSpaceByFormID(uint formID)
		{
			return AllSpaces.Where(space => space.FormID == formID).FirstOrDefault() ?? throw new Exception($"No Space with formID {formID} was found");
		}
	}
}
