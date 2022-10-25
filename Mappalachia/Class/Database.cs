using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace Mappalachia
{
	// Direct SQL queries and their execution
	static class Database
	{
		static readonly SqliteConnection connection;

		static List<string> lockTypes;
		static List<string> signatures;

		// Instantiate the connection to the database
		static Database()
		{
			connection = IOManager.OpenDatabase();
		}

		// Return the game version associated to the database
		public static string GetGameVersion()
		{
			SqliteCommand query = connection.CreateCommand();
			query.CommandText = Properties.Resources.getGameVersion;
			SqliteDataReader reader = query.ExecuteReader();

			while (reader.Read())
			{
				return reader.GetString(0);
			}

			return "unknown";
		}

		// Find all the unique signatures across the data
		public static List<string> GetSignatures()
		{
			// Singleton
			if (Database.signatures != null)
			{
				return Database.signatures;
			}

			List<string> signatures = new List<string>();

			SqliteCommand query = connection.CreateCommand();
			query.CommandText = Properties.Resources.getSignatures;
			SqliteDataReader reader = query.ExecuteReader();

			while (reader.Read())
			{
				signatures.Add(reader.GetString(0));
			}

			Database.signatures = signatures;
			return signatures;
		}

		// Find all the unique lock levels across the data
		public static List<string> GetLockTypes()
		{
			// Singleton
			if (Database.lockTypes != null)
			{
				return Database.lockTypes;
			}

			List<string> lockTypes = new List<string>();

			SqliteCommand query = connection.CreateCommand();
			query.CommandText = Properties.Resources.getLockLevels;
			SqliteDataReader reader = query.ExecuteReader();

			while (reader.Read())
			{
				lockTypes.Add(reader.GetString(0));
			}

			Database.lockTypes = lockTypes;
			return lockTypes;
		}

		// Find all the unique NPC types under from NPC_Search
		public static List<string> GetVariableNPCTypes()
		{
			List<string> npcTypes = new List<string>();

			SqliteCommand query = connection.CreateCommand();
			query.CommandText = Properties.Resources.getNPCTypes;
			SqliteDataReader reader = query.ExecuteReader();

			while (reader.Read())
			{
				npcTypes.Add(reader.GetString(0));
			}

			return npcTypes;
		}

		// Find all the unique scrap types in the Scrap_Search table
		public static List<string> GetScrapTypes()
		{
			List<string> scrapTypes = new List<string>();

			SqliteCommand query = connection.CreateCommand();
			query.CommandText = Properties.Resources.getScrapTypes;
			SqliteDataReader reader = query.ExecuteReader();

			while (reader.Read())
			{
				scrapTypes.Add(reader.GetString(0));
			}

			return scrapTypes;
		}

		// Returns a list of all Spaces in the database, as Space objects
		public static List<Space> GetAllSpaces()
		{
			List<Space> spaces = new List<Space>();

			SqliteCommand query = connection.CreateCommand();
			query.CommandText = Properties.Resources.getSpaces;
			SqliteDataReader reader = query.ExecuteReader();

			while (reader.Read())
			{
				spaces.Add(new Space(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3) == 1, reader.GetInt32(4), reader.GetInt32(5),
						reader.GetInt32(6), reader.GetInt32(7), reader.GetInt32(8), reader.GetInt32(9), reader.GetInt32(10), reader.GetInt32(11), reader.GetFloat(12)));
			}

			return spaces;
		}

		// Gets the z coordinate of everything within a space
		public static List<int> GetAllSpaceZCoords(string spaceFormID)
		{
			List<int> coordinates = new List<int>();

			SqliteCommand query = connection.CreateCommand();
			query.CommandText = Properties.Resources.getAllZCoordsSpace;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$spaceFormID", spaceFormID);
			SqliteDataReader reader = query.ExecuteReader();

			while (reader.Read())
			{
				coordinates.Add(reader.GetInt32(0));
			}

			return coordinates;
		}

		// Conducts the standard search and returns the found items
		public static List<MapItem> SearchStandard(string searchTerm, List<string> allowedSignatures, List<string> allowedLockTypes, string spaceFormID, bool allSpaces)
		{
			try
			{
				List<MapItem> results = new List<MapItem>();

				searchTerm = DataHelper.ProcessSearchString(searchTerm);
				string queryString = allSpaces ? Properties.Resources.searchStandardEverywhere : Properties.Resources.searchStandard;
				SqliteCommand query = connection.CreateCommand();

				// SQlite doesn't seem to support using variable length lists as parameters, but we can directly edit the query instead.
				queryString = queryString.Replace("$allowedSignatures", string.Join(",", allowedSignatures.Select(s => '\'' + s + '\'')));
				queryString = queryString.Replace("$allowedLockTypes", string.Join(",", allowedLockTypes.Select(s => '\'' + s + '\'')));

				query.CommandText = queryString;
				query.Parameters.Clear();
				query.Parameters.AddWithValue("$searchTerm", searchTerm);
				query.Parameters.AddWithValue("$spaceFormID", spaceFormID);

				SqliteDataReader reader = query.ExecuteReader();

				while (reader.Read())
				{
					string editorID = reader.GetString(1);
					string signature = reader.GetString(3);

					results.Add(new MapItem(
						Type.Standard,
						reader.GetString(0), // FormID
						editorID, // Editor ID
						reader.GetString(2), // Display Name
						signature, // Signature
						allowedLockTypes, // The Lock Types filtered for this set of items.
						DataHelper.GetSpawnChance(signature, editorID), // Spawn chance
						reader.GetInt32(5), // Count
						reader.GetString(6), // Space EditorID
						reader.GetString(7), // Space Display Name/location
						reader.GetString(8))); // Entity reference "label"
				}

				return results;
			}
			catch (Exception e)
			{
				Notify.Error("Mappalachia encountered an error while searching the database:\n" +
				IOManager.genericExceptionHelpText +
				e);

				return new List<MapItem>();
			}
		}

		// Conducts the NPC search and returns the found items.
		// Also merges results with standard search results for the same name, then drops items containing "Corpse"
		public static List<MapItem> SearchNPC(string searchTerm, int minChance, string spaceFormID, bool allSpaces)
		{
			try
			{
				List<MapItem> results = new List<MapItem>();

				SqliteCommand query = connection.CreateCommand();
				query.CommandText = allSpaces ? Properties.Resources.searchNPCEverywhere : Properties.Resources.searchNPC;
				query.Parameters.Clear();
				query.Parameters.AddWithValue("$npc", searchTerm);
				query.Parameters.AddWithValue("$chance", minChance / 100.00);
				query.Parameters.AddWithValue("$spaceFormID", spaceFormID);

				SqliteDataReader reader = query.ExecuteReader();

				// Collect some variables which will always be the same for every result and are required for an instance of MapItem
				string signature = DataHelper.ConvertSignature("NPC_", false);
				List<string> lockTypes = GetLockTypes();

				while (reader.Read())
				{
					// Sub-query for cell can return null
					if (reader.IsDBNull(0))
					{
						continue;
					}

					string name = reader.GetString(0);
					double spawnChance = Math.Round(reader.GetDouble(1) * 100, 2);

					results.Add(new MapItem(
						Type.NPC,
						name, // FormID
						name + " [Min " + spawnChance + "%]", // Editor ID
						name, // Display Name
						signature,
						lockTypes, // The Lock Types filtered for this set of items.
						spawnChance,
						reader.GetInt32(2), // Count
						reader.GetString(3), // Space editorID
						reader.GetString(4), // Space Display Name/location
						string.Empty)); // label
				}

				// Expand the NPC search, by also conducting a standard search of only NPC_, ignorant of lock filter
				results.AddRange(SearchStandard(searchTerm, new List<string> { "NPC_" }, GetLockTypes(), spaceFormID, allSpaces));

				/*Copy out search results not containing "corpse", therefore dropping the dead "NPCs"
				This isn't perfect and won't catch ALL dead NPCs.
				A common pattern seems to be that things prefixed with 'Enc' are dead,
				but this isn't a global truth and filtering these out would cause too many false positives*/
				List<MapItem> resultsWithoutCorpse = new List<MapItem>();
				foreach (MapItem item in results)
				{
					if (item.editorID.Contains("corpse") || item.editorID.Contains("Corpse"))
					{
						continue;
					}
					else
					{
						resultsWithoutCorpse.Add(item);
					}
				}

				return resultsWithoutCorpse;
			}
			catch (Exception e)
			{
				Notify.Error("Mappalachia encountered an error while searching the database:\n" +
				IOManager.genericExceptionHelpText +
				e);

				return new List<MapItem>();
			}
		}

		// Conducts the scrap search and returns the found items
		public static List<MapItem> SearchScrap(string searchTerm, string spaceFormID, bool allSpaces)
		{
			try
			{
				List<MapItem> results = new List<MapItem>();
				SqliteCommand query = connection.CreateCommand();

				query.CommandText = allSpaces ? Properties.Resources.searchScrapEverywhere : Properties.Resources.searchScrap;
				query.Parameters.Clear();
				query.Parameters.AddWithValue("$scrap", searchTerm);
				query.Parameters.AddWithValue("$spaceFormID", spaceFormID);

				SqliteDataReader reader = query.ExecuteReader();

				// Collect some variables which will always be the same for every result and are required for an instance of MapItem
				string signature = DataHelper.ConvertSignature("MISC", false);
				List<string> lockTypes = GetLockTypes();
				double spawnChance = DataHelper.GetSpawnChance("MISC", string.Empty);

				while (reader.Read())
				{
					// Sub-query for cell can return null
					if (reader.IsDBNull(0))
					{
						continue;
					}

					string name = reader.GetString(0);

					results.Add(new MapItem(
						Type.Scrap,
						name, // FormID
						name + " scraps from junk", // Editor ID
						name, // Display Name
						signature,
						lockTypes, // The Lock Types filtered for this set of items.
						spawnChance,
						reader.GetInt32(2), // Count
						reader.GetString(3), // Space editorID
						reader.GetString(4), // Space Display Name/location
						string.Empty)); // label
				}

				return results;
			}
			catch (Exception e)
			{
				Notify.Error("Mappalachia encountered an error while searching the database:\n" +
				IOManager.genericExceptionHelpText +
				e);

				return new List<MapItem>();
			}
		}

		// Conducts the region search and returns the found items
		public static List<MapItem> SearchRegion(string searchTerm, string spaceFormID, bool allSpaces)
		{
			try
			{
				List<MapItem> results = new List<MapItem>();

				searchTerm = DataHelper.ProcessSearchString(searchTerm);
				SqliteCommand query = connection.CreateCommand();

				query.CommandText = allSpaces ? Properties.Resources.searchRegionEverywhere : Properties.Resources.searchRegion;
				query.Parameters.Clear();
				query.Parameters.AddWithValue("$searchTerm", searchTerm);
				query.Parameters.AddWithValue("$spaceFormID", spaceFormID);

				SqliteDataReader reader = query.ExecuteReader();

				// Collect some variables which will always be the same for every result and are required for an instance of MapItem
				string signature = DataHelper.ConvertSignature("REGN", false);
				List<string> lockTypes = GetLockTypes();
				double spawnChance = DataHelper.GetSpawnChance("REGN", string.Empty);

				while (reader.Read())
				{
					// Sub-query for cell can return null
					if (reader.IsDBNull(0))
					{
						continue;
					}

					results.Add(new MapItem(
						Type.Region,
						reader.GetString(0), // FormID
						reader.GetString(1), // Editor ID
						string.Empty, // Display Name
						signature,
						lockTypes, // The Lock Types filtered for this set of items.
						spawnChance,
						1, // Count
						reader.GetString(2), // Space editorID
						reader.GetString(3), // Space Display Name/location
						string.Empty)); // label
				}

				return results;
			}
			catch (Exception e)
			{
				Notify.Error("Mappalachia encountered an error while searching the database:\n" +
				IOManager.genericExceptionHelpText +
				e);

				return new List<MapItem>();
			}
		}

		// Return the coordinate locations and boundaries of instances of a FormID
		public static List<MapDataPoint> GetStandardCoords(string formID, string spaceFormID, List<string> filteredLockTypes, string label)
		{
			List<MapDataPoint> coordinates = new List<MapDataPoint>();

			SqliteCommand query = connection.CreateCommand();
			string queryString = Properties.Resources.getCoordsStandard;

			// SQlite doesn't seem to support using variable length lists as parameters, but we can directly edit the query instead.
			queryString = queryString.Replace("$allowedLockTypes", string.Join(",", filteredLockTypes.Select(s => '\'' + s + '\'')));

			query.CommandText = queryString;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$formID", formID);
			query.Parameters.AddWithValue("$spaceFormID", spaceFormID);
			query.Parameters.AddWithValue("$label", label);

			SqliteDataReader reader = query.ExecuteReader();

			while (reader.Read())
			{
				string primitiveShape = reader.GetString(3);

				// Identify if this item has a primitive shape and use the appropriate constructor
				if (primitiveShape == string.Empty)
				{
					coordinates.Add(new MapDataPoint(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2)));
				}
				else
				{
					coordinates.Add(new MapDataPoint(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), primitiveShape, reader.GetInt32(4), reader.GetInt32(5), reader.GetInt32(6), reader.GetInt32(7)));
				}
			}

			return coordinates;
		}

		// Return the coordinate locations of instances of an NPC above given min spawn chance
		public static List<MapDataPoint> GetNPCCoords(string npc, string spaceFormID, double minChance)
		{
			List<MapDataPoint> coordinates = new List<MapDataPoint>();

			SqliteCommand query = connection.CreateCommand();

			query.CommandText = Properties.Resources.getCoordsNPC;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$npc", npc);
			query.Parameters.AddWithValue("$chance", minChance / 100.00);
			query.Parameters.AddWithValue("$spaceFormID", spaceFormID);

			SqliteDataReader reader = query.ExecuteReader();

			while (reader.Read())
			{
				coordinates.Add(new MapDataPoint(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2))
				{
					weight = reader.GetDouble(3),
				});
			}

			return coordinates;
		}

		// Return the coordinate locations of instances of Scrap contained within Junk
		public static List<MapDataPoint> GetScrapCoords(string scrap, string spaceFormID)
		{
			List<MapDataPoint> coordinates = new List<MapDataPoint>();

			SqliteCommand query = connection.CreateCommand();

			query.CommandText = Properties.Resources.getCoordsScrap;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$scrap", scrap);
			query.Parameters.AddWithValue("$spaceFormID", spaceFormID);

			SqliteDataReader reader = query.ExecuteReader();

			while (reader.Read())
			{
				coordinates.Add(new MapDataPoint(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2))
				{
					weight = reader.GetInt32(3),
				});
			}

			return coordinates;
		}

		// Return the basic list of coordinates of all verts of a region - only suitable for basic plotting not full region plotting
		public static List<MapDataPoint> GetRegionCoords(string regionFormID, string spaceFormID)
		{
			List<RegionPoint> regionPoints = GetRegionPoints(regionFormID, spaceFormID);
			List<MapDataPoint> points = new List<MapDataPoint>();

			foreach (RegionPoint regionPoint in regionPoints)
			{
				points.Add(new MapDataPoint(regionPoint.x, regionPoint.y, 0));
			}

			return points;
		}

		// Return the Region Points of the given region
		public static List<RegionPoint> GetRegionPoints(string regionFormID, string spaceFormID)
		{
			List<RegionPoint> regions = new List<RegionPoint>();

			SqliteCommand query = connection.CreateCommand();

			query.CommandText = Properties.Resources.getCoordsRegion;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$regionFormID", regionFormID);
			query.Parameters.AddWithValue("$spaceFormID", spaceFormID);

			SqliteDataReader reader = query.ExecuteReader();

			while (reader.Read())
			{
				regions.Add(new RegionPoint(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3)));
			}

			return regions;
		}

		public static List<MapMarker> GetMapMarkers(string spaceFormID)
		{
			List<MapMarker> mapMarkers = new List<MapMarker>();
			SqliteCommand query = connection.CreateCommand();

			query.CommandText = Properties.Resources.getMapMarkers;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$spaceFormID", spaceFormID);

			SqliteDataReader reader = query.ExecuteReader();

			while (reader.Read())
			{
				mapMarkers.Add(new MapMarker(reader.GetString(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3)));
			}

			return mapMarkers;
		}

		public static List<string> GetUniqueMarkerNames()
		{
			List<string> markerNames = new List<string>();
			SqliteCommand query = connection.CreateCommand();

			query.CommandText = Properties.Resources.getAllUniqueMapMarkers;
			query.Parameters.Clear();

			SqliteDataReader reader = query.ExecuteReader();

			while (reader.Read())
			{
				markerNames.Add(reader.GetString(0));
			}

			return markerNames;
		}
	}
}
