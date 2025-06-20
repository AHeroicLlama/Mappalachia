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
		public static async Task<List<GroupedSearchResult>> Search(Settings settings)
		{
			string searchTerm = ProcessSearchString(settings.SearchSettings.SearchTerm);
			List<GroupedSearchResult> results = new List<GroupedSearchResult>();

			// 'Standard' search for entities on the Position(preGrouped) table, joined with Entity
			string optionalSpaceTerm = settings.SearchSettings.SearchInAllSpaces ? string.Empty : $"AND spaceFormID = {settings.Space.FormID} ";
			bool searchForFormID = searchTerm.IsHexFormID() && settings.SearchSettings.Advanced;

			string query = "SELECT referenceFormID, editorID, displayName, signature, spaceFormID, count, label, lockLevel " +
				"FROM Position_PreGrouped " +
				"JOIN Entity ON Entity.entityFormID = Position_PreGrouped.referenceFormID " +
				$"WHERE (label LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' " +
				$"OR editorID LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' " +
				$"OR displayName LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' " +
				$"{(searchForFormID ? $"OR referenceFormID = '{HexToInt(searchTerm)}'" : string.Empty)}) " +
				optionalSpaceTerm +
				$"AND Position_PreGrouped.lockLevel IN {settings.SearchSettings.SelectedLockLevels.Select(l => l.ToStringForQuery()).ToSqliteCollection()} " +
				$"AND Entity.signature IN {settings.SearchSettings.SelectedSignatures.ToSqliteCollection()};";

			SqliteDataReader reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				results.Add(new GroupedSearchResult(
					new Entity(
						reader.GetUInt("referenceFormID"),
						reader.GetString("editorID"),
						reader.GetString("displayName"),
						reader.GetSignature()),
					GetSpaceByFormID(reader.GetUInt("spaceFormID")),
					reader.GetInt("count"),
					1,
					reader.GetString("label"),
					reader.GetLockLevel()));
			}

			// Container contents search
			query = "SELECT contentFormID, editorID, displayName, signature, spaceFormID, SUM(count) as count, lockLevel, quantity " +
				"FROM Container " +
				"JOIN Entity ON Container.contentFormID = Entity.entityFormID " +
				"JOIN Position_PreGrouped ON Position_PreGrouped.referenceFormID = Container.containerFormID " +
				$"WHERE (label LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' OR editorID LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' OR displayName LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' {(searchForFormID ? $"OR contentFormID = '{HexToInt(searchTerm)}'" : string.Empty)}) " +
				optionalSpaceTerm +
				$"AND Position_PreGrouped.lockLevel IN {settings.SearchSettings.SelectedLockLevels.Select(l => l.ToStringForQuery()).ToSqliteCollection()} " +
				$"AND Entity.signature IN {settings.SearchSettings.SelectedSignatures.ToSqliteCollection()} " +
				$"GROUP BY editorID, contentFormID, lockLevel, quantity, spaceFormID;";

			reader.Dispose();
			reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				results.Add(new GroupedSearchResult(
					new Entity(
						reader.GetUInt("contentFormID"),
						reader.GetString("editorID"),
						reader.GetString("displayName"),
						reader.GetSignature()),
					GetSpaceByFormID(reader.GetUInt("spaceFormID")),
					reader.GetInt("count"),
					reader.GetInt("quantity"),
					string.Empty,
					reader.GetLockLevel(),
					true));
			}

			reader.Dispose();

			if (settings.SearchSettings.ShouldSearchForNPC())
			{
				// NPC search
				query =
					"SELECT npcName, spaceFormID, spawnWeight, count(*) AS count " +
					"FROM NPC " +
					$"WHERE npcName LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}'" +
					optionalSpaceTerm +
					"GROUP BY NPC.spaceFormID, npcName, spawnWeight;";

				reader.Dispose();
				reader = await GetReader(Connection, query);

				while (reader.Read())
				{
					results.Add(new GroupedSearchResult(
						new DerivedNPC(reader.GetString("npcName")),
						GetSpaceByFormID(reader.GetUInt("spaceFormID")),
						reader.GetInt("count"),
						reader.GetFloat("spawnWeight")));
				}
			}

			if (settings.SearchSettings.ShouldSearchForScrap())
			{
				// Scrap search
				query = "SELECT component, spaceFormID, componentQuantity, SUM(count) AS properCount " +
					"FROM Scrap " +
					"JOIN Position_PreGrouped ON Position_PreGrouped.referenceFormID = Scrap.junkFormID " +
					$"WHERE component LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' " +
					optionalSpaceTerm +
					"GROUP BY component, spaceFormID, componentQuantity;";

				reader.Dispose();
				reader = await GetReader(Connection, query);

				while (reader.Read())
				{
					results.Add(new GroupedSearchResult(
						new DerivedScrap(reader.GetString("component")),
						GetSpaceByFormID(reader.GetUInt("spaceFormID")),
						reader.GetInt("properCount"),
						reader.GetFloat("componentQuantity")));
				}
			}

			if (settings.SearchSettings.ShouldSearchForRegion())
			{
				// Region search
				query =
					"SELECT regionFormID, regionEditorID, spaceFormID " +
					"FROM Region " +
					$"WHERE (regionEditorID LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' " +
					$"OR regionFormID = '{searchTerm}' " +
					$"{(searchForFormID ? $"OR regionFormID = '{HexToInt(searchTerm)}'" : string.Empty)}) " +
					optionalSpaceTerm +
					"GROUP BY regionFormID;";

				reader.Dispose();
				reader = await GetReader(Connection, query);

				while (reader.Read())
				{
					Space space = GetSpaceByFormID(reader.GetUInt("spaceFormID"));

					results.Add(new GroupedSearchResult(
						new Library.Region(
							reader.GetUInt("regionFormID"),
							reader.GetString("regionEditorID"),
							space),
						space));
				}
			}

			// Instance or TeleportsTo FormID-specific
			// Instance FormID ignores all filters as it is entirely unique
			if (searchForFormID)
			{
				query =
					"SELECT referenceFormID, editorID, displayName, signature, spaceFormID, label, lockLevel, count(*) AS count " +
					"FROM Position " +
					"JOIN Entity ON Entity.entityFormID = Position.referenceFormID " +
					$"WHERE ((teleportsToFormID = '{HexToInt(searchTerm)}') " +
					optionalSpaceTerm +
					$"AND lockLevel IN {settings.SearchSettings.SelectedLockLevels.Select(l => l.ToStringForQuery()).ToSqliteCollection()} " +
					$"AND Entity.signature IN {settings.SearchSettings.SelectedSignatures.ToSqliteCollection()}) " +
					"GROUP BY spaceFormID, Position.referenceFormID, teleportsToFormID, lockLevel, label;";

				reader = await GetReader(Connection, query);

				while (reader.Read())
				{
					results.Add(new GroupedSearchResult(
						new Entity(
							reader.GetUInt("referenceFormID"),
							reader.GetString("editorID"),
							reader.GetString("displayName"),
							reader.GetSignature()),
						GetSpaceByFormID(reader.GetUInt("spaceFormID")),
						reader.GetInt("count"),
						1,
						reader.GetString("label"),
						reader.GetLockLevel()));
				}

				reader.Dispose();

				query =
					"SELECT instanceFormID, referenceFormID, editorID, displayName, signature, spaceFormID, label, lockLevel " +
					"FROM Position " +
					"JOIN Entity ON Entity.entityFormID = Position.referenceFormID " +
					$"WHERE instanceFormID = '{HexToInt(searchTerm)}';";

				reader = await GetReader(Connection, query);

				while (reader.Read())
				{
					results.Add(new SingularSearchResult(
						reader.GetUInt("instanceFormID"),
						new Entity(
							reader.GetUInt("referenceFormID"),
							reader.GetString("editorID"),
							reader.GetString("displayName"),
							reader.GetSignature()),
						GetSpaceByFormID(reader.GetUInt("spaceFormID")),
						reader.GetString("label"),
						reader.GetLockLevel()));
				}

				reader.Dispose();
			}

			return results;
		}

		public static async Task<string> GetGameVersion()
		{
			return await CommonDatabase.GetGameVersion(Connection);
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
