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

			// 'Standard' search for entities on the Position(preGrouped) table, joined with Entity
			string optionalSpaceTerm = settings.SearchSettings.SearchInAllSpaces ? string.Empty : $"AND spaceFormID = {settings.Space.FormID} ";
			bool searchForFormID = searchTerm.IsHexFormID() && settings.SearchSettings.Advanced;

			List<GroupedSearchResult> results = new List<GroupedSearchResult>();

			results.AddRange(await StandardSearch(settings, searchTerm, optionalSpaceTerm, searchForFormID));
			results.AddRange(await ContainerSearch(settings, searchTerm, optionalSpaceTerm, searchForFormID));
			results.AddRange(await ScrapSearch(settings, searchTerm, optionalSpaceTerm));
			results.AddRange(await NPCSearch(settings, searchTerm, optionalSpaceTerm));
			results.AddRange(await RegionSearch(settings, searchTerm, optionalSpaceTerm, searchForFormID));
			results.AddRange(await TeleportsToSearch(settings, searchTerm, optionalSpaceTerm, searchForFormID));
			results.AddRange(await InstanceSearch(searchTerm, searchForFormID));

			return results;
		}

		static async Task<List<GroupedSearchResult>> StandardSearch(Settings settings, string searchTerm, string optionalSpaceTerm, bool searchForFormID)
		{
			List<GroupedSearchResult> results = new List<GroupedSearchResult>();

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

			using SqliteDataReader reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				results.Add(new GroupedSearchResult(
					new Entity(
						reader.GetUInt("referenceFormID"),
						reader.GetString("editorID"),
						reader.GetString("displayName"),
						reader.GetSignature()),
					GetSpaceByFormID(reader.GetUInt("spaceFormID")) !,
					reader.GetInt("count"),
					1,
					reader.GetString("label"),
					reader.GetLockLevel()));
			}

			return results;
		}

		static async Task<List<GroupedSearchResult>> ContainerSearch(Settings settings, string searchTerm, string optionalSpaceTerm, bool searchForFormID)
		{
			List<GroupedSearchResult> results = new List<GroupedSearchResult>();

			string query = "SELECT contentFormID, editorID, displayName, signature, spaceFormID, SUM(count) as count, lockLevel, quantity " +
				"FROM Container " +
				"JOIN Entity ON Container.contentFormID = Entity.entityFormID " +
				"JOIN Position_PreGrouped ON Position_PreGrouped.referenceFormID = Container.containerFormID " +
				$"WHERE (label LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' OR editorID LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' OR displayName LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' {(searchForFormID ? $"OR contentFormID = '{HexToInt(searchTerm)}'" : string.Empty)}) " +
				optionalSpaceTerm +
				$"AND Position_PreGrouped.lockLevel IN {settings.SearchSettings.SelectedLockLevels.Select(l => l.ToStringForQuery()).ToSqliteCollection()} " +
				$"AND Entity.signature IN {settings.SearchSettings.SelectedSignatures.ToSqliteCollection()} " +
				$"GROUP BY editorID, contentFormID, lockLevel, quantity, spaceFormID;";

			using SqliteDataReader reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				results.Add(new GroupedSearchResult(
					new Entity(
						reader.GetUInt("contentFormID"),
						reader.GetString("editorID"),
						reader.GetString("displayName"),
						reader.GetSignature()),
					GetSpaceByFormID(reader.GetUInt("spaceFormID")) !,
					reader.GetInt("count"),
					reader.GetInt("quantity"),
					string.Empty,
					reader.GetLockLevel(),
					true));
			}

			return results;
		}

		static async Task<List<GroupedSearchResult>> ScrapSearch(Settings settings, string searchTerm, string optionalSpaceTerm)
		{
			List<GroupedSearchResult> results = new List<GroupedSearchResult>();

			if (!settings.SearchSettings.ShouldSearchForScrap())
			{
				return results;
			}

			// Note: We are not looking inside of containers here
			string query = "SELECT component, spaceFormID, componentQuantity, SUM(count) AS properCount " +
				"FROM Scrap " +
				"JOIN Position_PreGrouped ON Position_PreGrouped.referenceFormID = Scrap.junkFormID " +
				$"WHERE component LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' " +
				optionalSpaceTerm +
				"GROUP BY component, spaceFormID, componentQuantity;";

			using SqliteDataReader reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				results.Add(new GroupedSearchResult(
					new DerivedScrap(reader.GetString("component")),
					GetSpaceByFormID(reader.GetUInt("spaceFormID")) !,
					reader.GetInt("properCount"),
					reader.GetDouble("componentQuantity")));
			}

			return results;
		}

		static async Task<List<GroupedSearchResult>> NPCSearch(Settings settings, string searchTerm, string optionalSpaceTerm)
		{
			List<GroupedSearchResult> results = new List<GroupedSearchResult>();

			if (!settings.SearchSettings.ShouldSearchForNPC())
			{
				return results;
			}

			string query =
				"SELECT npcName, spaceFormID, spawnWeight, count(*) AS count " +
				"FROM NPC " +
				$"WHERE npcName LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}'" +
				optionalSpaceTerm +
				"GROUP BY NPC.spaceFormID, npcName, spawnWeight;";

			using SqliteDataReader reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				results.Add(new GroupedSearchResult(
					new DerivedNPC(reader.GetString("npcName")),
					GetSpaceByFormID(reader.GetUInt("spaceFormID")) !,
					reader.GetInt("count"),
					reader.GetDouble("spawnWeight")));
			}

			return results;
		}

		static async Task<List<GroupedSearchResult>> RegionSearch(Settings settings, string searchTerm, string optionalSpaceTerm, bool searchForFormID)
		{
			List<GroupedSearchResult> results = new List<GroupedSearchResult>();

			if (!settings.SearchSettings.ShouldSearchForRegion())
			{
				return results;
			}

			string query =
				"SELECT regionFormID, regionEditorID, spaceFormID " +
				"FROM Region " +
				$"WHERE (regionEditorID LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' " +
				$"OR regionFormID = '{searchTerm}' " +
				$"{(searchForFormID ? $"OR regionFormID = '{HexToInt(searchTerm)}'" : string.Empty)}) " +
				optionalSpaceTerm +
				"GROUP BY regionFormID;";

			using SqliteDataReader reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				Space space = GetSpaceByFormID(reader.GetUInt("spaceFormID")) !;

				results.Add(new GroupedSearchResult(
					new Library.Region(
						reader.GetUInt("regionFormID"),
						reader.GetString("regionEditorID"),
						space),
					space));
			}

			return results;
		}

		static async Task<List<GroupedSearchResult>> TeleportsToSearch(Settings settings, string searchTerm, string optionalSpaceTerm, bool searchForFormID)
		{
			List<GroupedSearchResult> results = new List<GroupedSearchResult>();

			if (!searchForFormID)
			{
				return results;
			}

			string query =
				"SELECT referenceFormID, editorID, displayName, signature, spaceFormID, label, lockLevel, count(*) AS count " +
				"FROM Position " +
				"JOIN Entity ON Entity.entityFormID = Position.referenceFormID " +
				$"WHERE ((teleportsToFormID = '{HexToInt(searchTerm)}') " +
				optionalSpaceTerm +
				$"AND lockLevel IN {settings.SearchSettings.SelectedLockLevels.Select(l => l.ToStringForQuery()).ToSqliteCollection()} " +
				$"AND Entity.signature IN {settings.SearchSettings.SelectedSignatures.ToSqliteCollection()}) " +
				"GROUP BY spaceFormID, Position.referenceFormID, teleportsToFormID, lockLevel, label;";

			using SqliteDataReader reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				results.Add(new GroupedSearchResult(
					new Entity(
						reader.GetUInt("referenceFormID"),
						reader.GetString("editorID"),
						reader.GetString("displayName"),
						reader.GetSignature()),
					GetSpaceByFormID(reader.GetUInt("spaceFormID")) !,
					reader.GetInt("count"),
					1,
					reader.GetString("label"),
					reader.GetLockLevel()));
			}

			return results;
		}

		static async Task<List<SingularSearchResult>> InstanceSearch(string searchTerm, bool searchForFormID)
		{
			List<SingularSearchResult> results = new List<SingularSearchResult>();

			if (!searchForFormID)
			{
				return results;
			}

			string query =
				"SELECT instanceFormID, referenceFormID, editorID, displayName, signature, spaceFormID, label, lockLevel " +
				"FROM Position " +
				"JOIN Entity ON Entity.entityFormID = Position.referenceFormID " +
				$"WHERE instanceFormID = '{HexToInt(searchTerm)}';";

			using SqliteDataReader reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				results.Add(new SingularSearchResult(
					reader.GetUInt("instanceFormID"),
					new Entity(
						reader.GetUInt("referenceFormID"),
						reader.GetString("editorID"),
						reader.GetString("displayName"),
						reader.GetSignature()),
					GetSpaceByFormID(reader.GetUInt("spaceFormID")) !,
					reader.GetString("label"),
					reader.GetLockLevel()));
			}

			return results;
		}

		// Returns all Instances of the given GroupedSearchResult
		// TODO WIP
		public static async Task<List<Instance>> GetInstances(GroupedSearchResult searchResult, Space? space = null)
		{
			space ??= searchResult.Space;

			List<Instance> instances = new List<Instance>();

			if (searchResult is SingularSearchResult singular)
			{
				Instance? instance = await GetSingularInstance(singular, space);

				if (instance != null)
				{
					instances.Add(instance);
				}
			}
			else if (searchResult.InContainer)
			{
				// TODO
			}
			else if (searchResult.Entity is Library.Region)
			{
				// TODO
			}
			else if (searchResult.Entity.GetType() == typeof(Entity))
			{
				instances.AddRange(await GetStandardInstances(searchResult, space));
			}
			else if (searchResult.Entity is DerivedScrap)
			{
				// TODO
			}
			else if (searchResult.Entity is DerivedNPC)
			{
				// TODO
			}

			return instances;
		}

		// Returns all instances in the given space which teleport to another space
		public static async Task<List<Instance>> GetTeleporters(Space space)
		{
			List<Instance> teleporters = new List<Instance>();

			string query = "SELECT x, y, z, instanceFormID, teleportsToFormID, primitiveShape, boundX, boundY, boundZ, rotZ FROM Position " +
				$"WHERE spaceFormID = {space.FormID} AND teleportsToFormID IS NOT NULL AND teleportsToFormID != {space.FormID}";

			using SqliteDataReader reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				// TODO a few fields are irrelevant here
				teleporters.Add(new Instance(
					null,
					space,
					reader.GetCoord(),
					reader.GetUInt("instanceFormID"),
					string.Empty,
					GetSpaceByFormID(reader.GetUInt("teleportsToFormID")),
					LockLevel.None,
					reader.GetShape()));
			}

			return teleporters;
		}

		// Return all 'standard' instances of the given GroupedSearchResult in the given space
		static async Task<List<Instance>> GetStandardInstances(GroupedSearchResult searchResult, Space? space = null)
		{
			space ??= searchResult.Space;

			List<Instance> instances = new List<Instance>();

			string query = "SELECT x, y, z, instanceFormID, teleportsToFormID, primitiveShape, boundX, boundY, boundZ, rotZ FROM Position " +
				$"WHERE referenceFormID = {searchResult.Entity.FormID} AND spaceFormID = {space.FormID} AND lockLevel = '{searchResult.LockLevel.ToStringForQuery()}' AND label = '{searchResult.Label}'";

			using SqliteDataReader reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				instances.Add(new Instance(
					searchResult.Entity,
					space,
					reader.GetCoord(),
					reader.GetUInt("instanceFormID"),
					searchResult.Label,
					GetSpaceByFormID(reader.GetUInt("teleportsToFormID")),
					searchResult.LockLevel,
					reader.GetShape()));
			}

			return instances;
		}

		// Returns the instance of a SingularSearchResult
		static async Task<Instance?> GetSingularInstance(SingularSearchResult searchResult, Space? space = null)
		{
			space ??= searchResult.Space;

			Instance? instance = null;

			string query = "SELECT x, y, z, teleportsToFormID, primitiveShape, boundX, boundY, boundZ, rotZ, spaceFormID FROM Position " +
				$"WHERE instanceFormID = {searchResult.InstanceFormID} AND spaceFormID = {space.FormID}";

			using SqliteDataReader reader = await GetReader(Connection, query);

			if (reader.Read())
			{
				instance = new Instance(
					searchResult.Entity,
					space,
					reader.GetCoord(),
					searchResult.InstanceFormID,
					searchResult.Label,
					GetSpaceByFormID(reader.GetUInt("teleportsToFormID")),
					searchResult.LockLevel,
					reader.GetShape());
			}

			return instance;
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

		static Space? GetSpaceByFormID(uint formID)
		{
			return AllSpaces.Where(space => space.FormID == formID).FirstOrDefault();
		}
	}
}
