using System.Text.RegularExpressions;
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

		static Regex SubstituteScrap { get; } = new Regex("derived|scrap", RegexOptions.IgnoreCase);

		static Regex SubstituteNPC { get; } = new Regex("derived|spawn", RegexOptions.IgnoreCase);

		static Regex SubstituteFlux { get; } = new Regex("derived|raw|flux", RegexOptions.IgnoreCase);

		static char EscapeChar { get; } = '`';

		// The core database search function - returns a collection of GroupedInstance from the given search params
		public static async Task<List<GroupedSearchResult>> Search(Settings settings)
		{
			string searchTerm = ProcessSearchString(settings.SearchSettings.SearchTerm);
			string allSpacesClause = settings.SearchSettings.SearchInAllSpaces ? string.Empty : $"AND spaceFormID = {settings.Space.FormID} ";
			bool searchForFormID = searchTerm.IsHexFormID() && settings.SearchSettings.Advanced;

			List<GroupedSearchResult> results = new List<GroupedSearchResult>();

			results.AddRange(await StandardSearch(settings, searchTerm, allSpacesClause, searchForFormID));
			results.AddRange(await ContainerSearch(settings, searchTerm, allSpacesClause, searchForFormID));
			results.AddRange(await ScrapSearch(settings, searchTerm, allSpacesClause));
			results.AddRange(await NPCSearch(settings, searchTerm, allSpacesClause));
			results.AddRange(await FluxSearch(settings, searchTerm, allSpacesClause));
			results.AddRange(await RegionSearch(settings, searchTerm, allSpacesClause, searchForFormID));
			results.AddRange(await TeleportsToSearch(settings, searchTerm, allSpacesClause, searchForFormID));
			results.AddRange(await InstanceSearch(searchTerm, searchForFormID));

			// We use LINQ to filter out results here, to avoid joining on the Space table in each search function
			if (settings.SearchSettings.SearchInInstancesOnly)
			{
				return results.Where(
					r => r is SingularSearchResult ||
					r.Space.IsInstanceable)
					.ToList();
			}

			return results;
		}

		static async Task<List<GroupedSearchResult>> StandardSearch(Settings settings, string searchTerm, string optionalSpaceClause, bool searchForFormID)
		{
			List<GroupedSearchResult> results = new List<GroupedSearchResult>();

			string query = "SELECT referenceFormID, editorID, displayName, signature, spaceFormID, count, label, lockLevel " +
				"FROM Position_PreGrouped " +
				"JOIN Entity ON Entity.entityFormID = Position_PreGrouped.referenceFormID " +
				$"WHERE (label LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' " +
				$"OR editorID LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' " +
				$"OR displayName LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' " +
				$"{(searchForFormID ? $"OR referenceFormID = '{HexToInt(searchTerm)}'" : string.Empty)}) " +
				optionalSpaceClause +
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
					GetSpaceByFormID(reader.GetUInt("spaceFormID")),
					reader.GetInt("count"),
					1,
					reader.GetString("label"),
					reader.GetLockLevel()));
			}

			return results;
		}

		static async Task<List<GroupedSearchResult>> ContainerSearch(Settings settings, string searchTerm, string optionalSpaceClause, bool searchForFormID)
		{
			List<GroupedSearchResult> results = new List<GroupedSearchResult>();

			string query = "SELECT contentFormID, editorID, displayName, signature, spaceFormID, SUM(count) as count, lockLevel, quantity " +
				"FROM Container " +
				"JOIN Entity ON Container.contentFormID = Entity.entityFormID " +
				"JOIN Position_PreGrouped ON Position_PreGrouped.referenceFormID = Container.containerFormID " +
				$"WHERE (label LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' OR editorID LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' OR displayName LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' {(searchForFormID ? $"OR contentFormID = '{HexToInt(searchTerm)}'" : string.Empty)}) " +
				optionalSpaceClause +
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
					GetSpaceByFormID(reader.GetUInt("spaceFormID")),
					reader.GetInt("count"),
					reader.GetInt("quantity"),
					string.Empty,
					reader.GetLockLevel(),
					true));
			}

			return results;
		}

		static async Task<List<GroupedSearchResult>> ScrapSearch(Settings settings, string searchTerm, string optionalSpaceClause)
		{
			List<GroupedSearchResult> results = new List<GroupedSearchResult>();

			if (!settings.SearchSettings.ShouldSearchForScrap())
			{
				return results;
			}

			string substituteSearch = SubstituteScrap.Replace(searchTerm, string.Empty).Trim();

			// Note: We are not looking inside of containers here
			string query = "SELECT component, spaceFormID, componentQuantity, SUM(count) AS properCount " +
				"FROM Scrap " +
				"JOIN Position_PreGrouped ON Position_PreGrouped.referenceFormID = Scrap.junkFormID " +
				$"WHERE (component LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' " +
				$"OR component LIKE '%{substituteSearch}%' ESCAPE '{EscapeChar}') " +
				optionalSpaceClause +
				"GROUP BY component, spaceFormID, componentQuantity;";

			using SqliteDataReader reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				results.Add(new GroupedSearchResult(
					new DerivedScrap(reader.GetString("component")),
					GetSpaceByFormID(reader.GetUInt("spaceFormID")),
					reader.GetInt("properCount"),
					reader.GetDouble("componentQuantity")));
			}

			return results;
		}

		static async Task<List<GroupedSearchResult>> NPCSearch(Settings settings, string searchTerm, string optionalSpaceClause)
		{
			List<GroupedSearchResult> results = new List<GroupedSearchResult>();

			if (!settings.SearchSettings.ShouldSearchForNPC())
			{
				return results;
			}

			string substituteSearch = SubstituteNPC.Replace(searchTerm, string.Empty).Trim();

			string query = "SELECT npcName, spaceFormID, spawnWeight, count(*) AS count " +
				"FROM NPC " +
				$"WHERE (npcName LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' " +
				$"OR npcName LIKE '%{substituteSearch}%' ESCAPE '{EscapeChar}') " +
				optionalSpaceClause +
				"GROUP BY NPC.spaceFormID, npcName, spawnWeight;";

			using SqliteDataReader reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				results.Add(new GroupedSearchResult(
					new DerivedNPC(reader.GetString("npcName")),
					GetSpaceByFormID(reader.GetUInt("spaceFormID")),
					reader.GetInt("count"),
					reader.GetDouble("spawnWeight")));
			}

			return results;
		}

		static async Task<List<GroupedSearchResult>> FluxSearch(Settings settings, string searchTerm, string optionalSpaceClause)
		{
			List<GroupedSearchResult> results = new List<GroupedSearchResult>();

			if (!settings.SearchSettings.ShouldSearchForRawFlux())
			{
				return results;
			}

			string substituteSearch = SubstituteFlux.Replace(searchTerm, string.Empty).Trim();

			string query = "SELECT color, spaceFormID, SUM(count) AS properCount " +
				"FROM Flux " +
				"JOIN Position_PreGrouped ON Position_PreGrouped.referenceFormID = Flux.referenceFormID " +
				$"WHERE (color LIKE '%{searchTerm}%' ESCAPE '{EscapeChar}' " +
				$"OR color LIKE '%{substituteSearch}%' ESCAPE '{EscapeChar}') " +
				optionalSpaceClause +
				"GROUP BY spaceFormID, color;";

			using SqliteDataReader reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				results.Add(new GroupedSearchResult(
					new DerivedRawFlux(reader.GetFluxColor()),
					GetSpaceByFormID(reader.GetUInt("spaceFormID")),
					reader.GetInt("properCount")));
			}

			results = results.Where(r => r.Space.IsNukable()).ToList();

			return results;
		}

		static async Task<List<GroupedSearchResult>> RegionSearch(Settings settings, string searchTerm, string optionalSpaceClause, bool searchForFormID)
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
				optionalSpaceClause +
				"GROUP BY regionFormID;";

			using SqliteDataReader reader = await GetReader(Connection, query);

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

			return results;
		}

		static async Task<List<GroupedSearchResult>> TeleportsToSearch(Settings settings, string searchTerm, string optionalSpaceClause, bool searchForFormID)
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
				optionalSpaceClause +
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
					GetSpaceByFormID(reader.GetUInt("spaceFormID")),
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
					GetSpaceByFormID(reader.GetUInt("spaceFormID")),
					reader.GetString("label"),
					reader.GetLockLevel()));
			}

			return results;
		}

		// Returns all Instances of the given GroupedSearchResult
		public static async Task<List<Instance>> GetInstances(GroupedSearchResult searchResult, Space? space = null)
		{
			space ??= searchResult.Space;

			List<Instance> instances = new List<Instance>();

			if (searchResult is SingularSearchResult singular)
			{
				Instance? instance = await GetSingularInstance(singular, space);

				if (instance is not null)
				{
					instances.Add(instance);
				}
			}
			else if (searchResult.InContainer)
			{
				instances.AddRange(await GetInContainerInstances(searchResult, space));
			}
			else if (searchResult.Entity is Library.Region)
			{
				Instance? instance = await GetRegionInstance(searchResult, space);

				if (instance is not null)
				{
					instances.Add(instance);
				}
			}
			else if (searchResult.Entity.GetType() == typeof(Entity))
			{
				instances.AddRange(await GetStandardInstances(searchResult, space));
			}
			else if (searchResult.Entity is DerivedScrap)
			{
				instances.AddRange(await GetScrapInstances(searchResult, space));
			}
			else if (searchResult.Entity is DerivedNPC)
			{
				instances.AddRange(await GetNPCInstances(searchResult, space));
			}
			else if (searchResult.Entity is DerivedRawFlux)
			{
				instances.AddRange(await GetFluxInstances(searchResult, space));
			}

			return instances;
		}

		// Returns all instances in the given space which teleport to another space
		public static async Task<List<Instance>> GetTeleporters(Space space)
		{
			List<Instance> teleporters = new List<Instance>();

			string query = "SELECT x, y, z, instanceFormID, teleportsToFormID, primitiveShape, boundX, boundY, boundZ, rotZ, " +
				"referenceFormID, editorID, displayName, signature FROM Position " +
				$"JOIN Entity ON Entity.entityFormID = Position.referenceFormID " +
				$"WHERE spaceFormID = {space.FormID} AND teleportsToFormID IS NOT NULL AND teleportsToFormID != {space.FormID}";

			using SqliteDataReader reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				teleporters.Add(new Instance(
					new Entity(
						reader.GetUInt("referenceFormID"),
						reader.GetString("editorID"),
						reader.GetString("displayName"),
						reader.GetSignature()),
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
		static async Task<List<Instance>> GetStandardInstances(GroupedSearchResult searchResult, Space space)
		{
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
		static async Task<Instance?> GetSingularInstance(SingularSearchResult searchResult, Space space)
		{
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

		// Returns the instances of a GroupedSearchResult which is inContainer
		static async Task<List<Instance>> GetInContainerInstances(GroupedSearchResult searchResult, Space space)
		{
			List<Instance> instances = new List<Instance>();

			string query = "SELECT x, y, z, instanceFormID FROM Position " +
				"JOIN Container ON Container.containerFormID = Position.referenceFormID " +
				$"WHERE contentFormID = {searchResult.Entity.FormID} AND quantity = {searchResult.SpawnWeight} AND lockLevel = '{searchResult.LockLevel.ToStringForQuery()}' AND spaceFormID = {space.FormID};";

			using SqliteDataReader reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				instances.Add(new Instance(
					searchResult.Entity,
					space,
					reader.GetCoord(),
					reader.GetUInt("instanceFormID"),
					searchResult.Label,
					null,
					searchResult.LockLevel,
					null,
					searchResult.SpawnWeight,
					true));
			}

			return instances;
		}

		// Returns the instance of a GroupedSearchResult which is a Region
		static async Task<Instance?> GetRegionInstance(GroupedSearchResult searchResult, Space space)
		{
			return await GetRegion(searchResult.Entity.EditorID, space);
		}

		// Return the region with the given EditorID from the given Space
		static async Task<Instance?> GetRegion(string editorID, Space space)
		{
			string regionQuery = "SELECT minLevel, maxLevel, regionFormID FROM Region " +
				$"WHERE regionEditorID = '{editorID}' AND spaceFormID = {space.FormID};";

			using SqliteDataReader regionReader = await GetReader(Connection, regionQuery);

			if (!regionReader.Read())
			{
				return null;
			}

			Library.Region region = new Library.Region(
				regionReader.GetUInt("regionFormID"),
				editorID,
				space,
				regionReader.GetUInt("minLevel"),
				regionReader.GetUInt("maxLevel"));

			string pointQuery = "SELECT x, y, subRegionIndex, coordIndex FROM RegionPoints " +
				$"WHERE regionFormID = {region.FormID};";

			using SqliteDataReader pointReader = await GetReader(Connection, pointQuery);

			while (pointReader.Read())
			{
				region.AddPoint(
					new RegionPoint(
						region,
						new Coord(
							pointReader.GetDouble("x"),
							pointReader.GetDouble("y")),
						pointReader.GetUInt("subRegionIndex"),
						pointReader.GetUInt("coordIndex")));
			}

			Instance instance = new Instance(
				region,
				space,
				region.GetAllSubRegions().First().GetCentroid(),
				0,
				string.Empty,
				null,
				LockLevel.None,
				null);

			return instance;
		}

		// Returns the instances of a GroupedSearchResult which is an NPC
		static async Task<List<Instance>> GetNPCInstances(GroupedSearchResult searchResult, Space space)
		{
			List<Instance> instances = new List<Instance>();

			string query = "SELECT x, y, z, Position.instanceFormID FROM Position " +
				"JOIN NPC ON npc.instanceFormID = Position.instanceFormID " +
				$"WHERE npcName = '{searchResult.Entity.EditorID}' AND npc.spawnWeight = {searchResult.SpawnWeight} AND npc.spaceFormID = {space.FormID}";

			using SqliteDataReader reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				instances.Add(new Instance(
					searchResult.Entity,
					space,
					reader.GetCoord(),
					reader.GetUInt("instanceFormID"),
					searchResult.Label,
					null,
					LockLevel.None,
					null,
					searchResult.SpawnWeight));
			}

			return instances;
		}

		// Returns the instances of a GroupedSearchResult which is scrap
		static async Task<List<Instance>> GetScrapInstances(GroupedSearchResult searchResult, Space space)
		{
			List<Instance> instances = new List<Instance>();

			string query = "SELECT x, y, z, Position.instanceFormID FROM Position " +
				"JOIN Scrap ON Scrap.junkFormID = Position.referenceFormID " +
				$"WHERE component = '{searchResult.Entity.EditorID}' AND Scrap.componentQuantity = {searchResult.SpawnWeight} AND Position.spaceFormID = {space.FormID}";

			using SqliteDataReader reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				instances.Add(new Instance(
					searchResult.Entity,
					space,
					reader.GetCoord(),
					reader.GetUInt("instanceFormID"),
					searchResult.Label,
					null,
					LockLevel.None,
					null,
					searchResult.SpawnWeight));
			}

			return instances;
		}

		// Returns the instances of a GroupedSearchResult which has an Entity of DerivedRawFlux
		static async Task<List<Instance>> GetFluxInstances(GroupedSearchResult searchResult, Space space)
		{
			List<Instance> instances = new List<Instance>();

			string query = "SELECT x, y, z, Position.instanceFormID FROM Position " +
				$"JOIN Flux ON Flux.referenceFormID = Position.referenceFormID " +
				$"WHERE Flux.Color = '{((DerivedRawFlux)searchResult.Entity).Color}' AND spaceFormID = {space.FormID}";

			using SqliteDataReader reader = await GetReader(Connection, query);

			while (reader.Read())
			{
				instances.Add(new Instance(
					searchResult.Entity,
					space,
					reader.GetCoord(),
					reader.GetUInt("instanceFormID"),
					searchResult.Label,
					null,
					LockLevel.None,
					null,
					searchResult.SpawnWeight));
			}

			// Find the Non-nukable zone(s), and exclude instances which lie within them
			foreach (string regionEditorID in space.GetNonNukableZoneEditorIds())
			{
				Instance regionInstance = (await GetRegion(regionEditorID, space)) ?? throw new Exception($"No Region with editorID {regionEditorID} found");
				Library.Region nonNukableZone = (Library.Region)regionInstance.Entity;

				instances = instances.Where(i => !nonNukableZone.ContainsPoint(i.Coord)).ToList();
			}

			return instances;
		}

		// Returns the min (item1) and max (item2) z coordinate of all instances of the itemsToPlot in the current space
		public static async Task<(double, double)> GetZRange(List<GroupedSearchResult> itemsToPlot, Settings settings)
		{
			if (itemsToPlot.Count == 0)
			{
				return (0, 0);
			}

			List<Instance> allInstances = (await Task.WhenAll(itemsToPlot.Select(async item => await GetInstances(item, settings.Space))))
				.SelectMany(instance => instance).ToList();

			if (allInstances.Count == 0)
			{
				return (0, 0);
			}

			return (allInstances.Min(i => i.HeightForTopograph), allInstances.Max(i => i.HeightForTopograph));
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
			if (formID == 0)
			{
				return null!;
			}

			return AllSpaces.FirstOrDefault(space => space.FormID == formID) ?? throw new Exception($"Space with formID {formID.ToHex()} not found!");
		}
	}
}
