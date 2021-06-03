using System;
using System.Collections.Generic;
using System.Linq;
using Mappalachia.Class;
using Microsoft.Data.Sqlite;

namespace Mappalachia
{
	// Interfaces with the database and provides helper methods, data translation and sorting
	static class DataHelper
	{
		static List<string> permittedLockTypes;
		static List<string> permittedSignatures;

		static readonly Dictionary<string, string> signatureDescription = new Dictionary<string, string>
		{
			{ "STAT", "Environmental scenery which does not move and cannot be interacted with." },
			{ "SCOL", "A grouped set of static objects." },
			{ "ACTI", "Defined trigger volume, invisible in-game.\n" +
				"Activators can mark out designated areas, trigger events, or act as 'hit-boxes' for interaction." },
			{ "LIGH", string.Empty },
			{ "NPC_", "Non-player character." },
			{ "MISC", "Junk, Scrap or Mod." },
			{ "MSTT", "Environmental scenery which animates or responds to physics." },
			{ "BOOK", "Note, Plan or Recipe." },
			{ "CONT", "Anything which can hold items." },
			{ "FURN", "Object or position which a character can use to enter into an animation.\n" +
				"Includes workbenches and instruments, but also NPC ambush positions such as scorchbeast spawns." },
			{ "LVLI", "Lootable object which has some odds of spawning (up to and including 100% chances)\n" +
				"Many items of different categories are in fact included under this one." },
			{ "TERM", string.Empty },
			{ "TXST", "A decal applied to a surface such as paint or dirt." },
			{ "DOOR", string.Empty },
			{ "ALCH", "Food, drink, chems etc." },
			{ "SOUN", string.Empty },
			{ "NOTE", string.Empty },
			{ "FLOR", "Collectable natural resource." },
			{ "ARMO", string.Empty },
			{ "ASPC", "Applies settings for specific environmental acoustics." },
			{ "WEAP", string.Empty },
			{ "KEYM", string.Empty },
			{ "TACT", "A trigger/activator which causes a voice line to be played." },
			{ "HAZD", "Area of space which can harm the player (fire/radiation)." },
			{ "AMMO", string.Empty },
			{ "IDLM", "Allows an npc to enter an idle animation." },
			{ "BNDS", "A curved line shape. Usually used for power lines." },
			{ "SECH", "Trigger for echo sound effect." },
			{ "PROJ", "An 'armed' bullet/missile or thrown weapon." },
			{ "CNCY", string.Empty },
		};

		// Provide a user-friendly name for each signature which best represents what a typical player would know them as
		static readonly Dictionary<string, string> signatureToFriendlyName = new Dictionary<string, string>
		{
			{ "STAT", "Static object" },
			{ "SCOL", "Static collection" },
			{ "ACTI", "Activator" },
			{ "LIGH", "Light" },
			{ "NPC_", "NPC" },
			{ "MISC", "Junk/Scrap" },
			{ "MSTT", "Moveable static" },
			{ "BOOK", "Note/Plan" },
			{ "CONT", "Container" },
			{ "FURN", "Furniture" },
			{ "LVLI", "Loot" },
			{ "TERM", "Terminal" },
			{ "TXST", "Texture set" },
			{ "DOOR", "Door" },
			{ "ALCH", "Aid" },
			{ "SOUN", "Sound" },
			{ "NOTE", "Holotape" },
			{ "FLOR", "Flora" },
			{ "ARMO", "Armor/Apparel" },
			{ "ASPC", "Acoustic space" },
			{ "WEAP", "Weapon" },
			{ "KEYM", "Key" },
			{ "TACT", "Talking activator" },
			{ "HAZD", "Hazard" },
			{ "AMMO", "Ammo" },
			{ "IDLM", "Idle marker" },
			{ "BNDS", "Bendable spline" },
			{ "SECH", "Echo marker" },
			{ "PROJ", "Projectile" },
			{ "CNCY", "Currency" },
		};

		// Provides a pre-ordered list of each signature in a suggested sort order for the UI
		// This groups often-used items towards the top, and similar items together
		// Items not on this list are added to the bottom
		public static readonly List<string> suggestedSignatureSort = new List<string>
		{
			"LVLI",
			"FLOR",
			"NPC_",
			"MISC",
			"BOOK",
			"ALCH",
			"CONT",
			"STAT",
			"SCOL",
			"MSTT",
			"FURN",
			"NOTE",
			"TERM",
			"ARMO",
			"WEAP",
			"AMMO",
			"PROJ",
			"CNCY",
			"KEYM",
			"ACTI",
			"TACT",
			"DOOR",
			"HAZD",
			"IDLM",
			"LIGH",
			"SOUN",
			"SECH",
			"ASPC",
			"TXST",
			"BNDS",
		};

		// Provides a list of the recommended signatures to be selected by the filter by default
		// This helps prevent new users being flooded with less relevant or more technical results.
		public static readonly List<string> recommendedSignatures = new List<string>
		{
			"LVLI",
			"FLOR",
			"NPC_",
			"MISC",
			"BOOK",
			"ALCH",
			"CONT",
			/*"STAT",
			"SCOL",
			"MSTT",*/
			"FURN",
			"NOTE",
			"TERM",
			"ARMO",
			"WEAP",
			"AMMO",
			"PROJ",
			"CNCY",
			"KEYM",
			"ACTI",
			/*"TACT",
			"DOOR",
			"HAZD",
			"IDLM",
			"LIGH",
			"SOUN",
			"SECH",
			"ASPC",
			"TXST",
			"BNDS",*/
		};

		// Inverse the user friendly signature names so we can use the proper signatures in queries
		static readonly Dictionary<string, string> signatureToProperName = signatureToFriendlyName.ToDictionary(x => x.Value, x => x.Key);

		// Provide a user-friendly name to the lock level
		static readonly Dictionary<string, string> lockLevelToFriendlyName = new Dictionary<string, string>
		{
			{ string.Empty,			"Not locked" },
			{ "Novice (Level 0)",	"Level 0" },
			{ "Advanced (Level 1)", "Level 1" },
			{ "Expert (Level 2)",	"Level 2" },
			{ "Master (Level 3)",	"Level 3" },
		};

		// Inverse the user friendly lock names so we can use the proper lock levels in queries
		static readonly Dictionary<string, string> lockLevelToProperName = lockLevelToFriendlyName.ToDictionary(x => x.Value, x => x.Key);

		// Provides a pre-ordered list of each lock level in a suggested sort order
		// This groups often-used items towards the top, and similar items together
		public static readonly List<string> suggestedLockLevelSort = new List<string>
		{
			string.Empty,
			"Novice (Level 0)",
			"Advanced (Level 1)",
			"Expert (Level 2)",
			"Master (Level 3)",
			"Requires Terminal",
			"Requires Key",
			"Chained",
			"Barred",
			"Opens Door",
			"Inaccessible",
			"Unknown",
		};

		// Convert a signature to the proper or user-friendly version of itself
		// Works regardless of the current state of the given signature
		public static string ConvertSignature(string signature, bool properName)
		{
			try
			{
				if (properName)
				{
					return signatureToFriendlyName.ContainsKey(signature) ? signature : signatureToProperName[signature];
				}
				else
				{
					return signatureToProperName.ContainsKey(signature) ? signature : signatureToFriendlyName[signature];
				}
			}
			catch (Exception)
			{
				// Unable to convert - just return it unconverted
				return signature;
			}
		}

		// Convert a lockLevel to the proper or user-friendly version of itself
		// Works regardless of the current state of the given lock level
		public static string ConvertLockLevel(string lockLevel, bool properName)
		{
			try
			{
				// We don't convert all lock levels
				if (!lockLevelToFriendlyName.ContainsKey(lockLevel) &&
					!lockLevelToProperName.ContainsKey(lockLevel))
				{
					return lockLevel;
				}

				if (properName)
				{
					return lockLevelToFriendlyName.ContainsKey(lockLevel) ? lockLevel : lockLevelToProperName[lockLevel];
				}
				else
				{
					return lockLevelToProperName.ContainsKey(lockLevel) ? lockLevel : lockLevelToFriendlyName[lockLevel];
				}
			}
			catch (Exception)
			{
				// Unable to convert - just return it unconverted
				return lockLevel;
			}
		}

		// runs ConvertLockLevel against an entire collection of lockLevel
		public static List<string> ConvertLockLevelCollection(List<string> lockLevels, bool properName)
		{
			List<string> result = new List<string>();

			foreach (string lockLevel in lockLevels)
			{
				result.Add(ConvertLockLevel(lockLevel, properName));
			}

			return result;
		}

		// Find the description of a given signature, in either long or short form
		public static string GetSignatureDescription(string signature)
		{
			try
			{
				return signatureDescription[ConvertSignature(signature, true)];
			}
			catch (Exception)
			{
				return "Unsupported signature!";
			}
		}

		// Escape functional SQL characters and wildcard on space
		public static string ProcessSearchString(string input)
		{
			return input.Trim()
				.Replace("_", "\\_")
				.Replace("%", "\\%")
				.Replace(" ", "%");
		}

		// Find all the unique signatures across the data
		public static List<string> GetPermittedSignatures()
		{
			// Singleton
			if (DataHelper.permittedSignatures != null)
			{
				return DataHelper.permittedSignatures;
			}

			List<string> permittedSignatures = new List<string>();
			SqliteDataReader reader = Database.ExecuteQuerySignatures();
			while (reader.Read())
			{
				permittedSignatures.Add(reader.GetString(0));
			}

			DataHelper.permittedSignatures = permittedSignatures;
			return permittedSignatures;
		}

		// Find all the unique lock levels across the data
		public static List<string> GetPermittedLockTypes()
		{
			// Singleton
			if (DataHelper.permittedLockTypes != null)
			{
				return DataHelper.permittedLockTypes;
			}

			List<string> permittedLockTypes = new List<string>();
			SqliteDataReader reader = Database.ExecuteQueryLockLevels();
			while (reader.Read())
			{
				permittedLockTypes.Add(reader.GetString(0));
			}

			DataHelper.permittedLockTypes = permittedLockTypes;
			return permittedLockTypes;
		}

		// Find all the unique NPC types under the variable spawns
		public static List<string> GetVariableNPCTypes()
		{
			List<string> npcTypes = new List<string>();
			SqliteDataReader reader = Database.ExecuteQueryNPCTypes();
			while (reader.Read())
			{
				npcTypes.Add(reader.GetString(0));
			}

			return npcTypes;
		}

		// Find all the unique scrap types in the quantified junk table
		public static List<string> GetVariableScrapTypes()
		{
			List<string> scrapTypes = new List<string>();
			SqliteDataReader reader = Database.ExecuteQueryScrapTypes();
			while (reader.Read())
			{
				scrapTypes.Add(reader.GetString(0));
			}

			return scrapTypes;
		}

		// Returns a list of all cells in the database, as Cell objects
		public static List<Cell> GetAllCells()
		{
			List<Cell> cells = new List<Cell>();
			SqliteDataReader reader = Database.ExecuteQueryCells();
			while (reader.Read())
			{
				cells.Add(new Cell(reader.GetString(0), reader.GetString(1), reader.GetString(2)));
			}

			return cells;
		}

		// Indicate the spawn chance of a standard item based on understandings of LVLI
		public static double GetSpawnChance(string signature, string editorID)
		{
			return (signature == "LVLI" || editorID.Contains("ChanceNone")) ? -1 : 100;
		}

		// Performs similar functionality to standard search but constrained to a specific cell, denoted by cellFormID
		public static List<MapItem> SearchCell(string searchTerm, Cell cell, List<string> allowedSignatures, List<string> allowedLockTypes)
		{
			try
			{
				List<MapItem> results = new List<MapItem>();

				// Run the standard simple search but for interiors and uniquely against the cellFormID
				using (SqliteDataReader reader = Database.ExecuteQuerySearchCell(cell.formID, searchTerm, allowedSignatures, allowedLockTypes))
				{
					while (reader.Read())
					{
						string signature = reader.GetString(2);
						string editorID = reader.GetString(1);

						results.Add(new MapItem(
							Type.Standard,
							reader.GetString(5), // FormID
							editorID, // Editor ID
							reader.GetString(0), // Display Name
							signature, // Signature
							allowedLockTypes, // The Lock Types filtered for this set of items.
							GetSpawnChance(signature, editorID), // Spawn chance
							reader.GetInt32(3), // Count
							cell.displayName, // Cell Display Name/location
							cell.editorID)); // Cell EditorID
					}
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

		// Conducts the standard search and returns the found items
		public static List<MapItem> SearchStandard(string searchTerm, bool searchInteriors, List<string> allowedSignatures, List<string> allowedLockTypes)
		{
			try
			{
				List<MapItem> results = new List<MapItem>();

				using (SqliteDataReader reader = Database.ExecuteQueryStandardSearch(searchInteriors, searchTerm, allowedSignatures, allowedLockTypes))
				{
					while (reader.Read())
					{
						string signature = reader.GetString(2);
						string editorID = reader.GetString(1);

						results.Add(new MapItem(
							Type.Standard,
							reader.GetString(5), // FormID
							editorID, // Editor ID
							reader.GetString(0), // Display Name
							signature, // Signature
							allowedLockTypes, // The Lock Types filtered for this set of items.
							GetSpawnChance(signature, editorID), // Spawn chance
							reader.GetInt32(3), // Count
							reader.GetString(6), // Cell Display Name/location
							reader.GetString(7))); // Cell EditorID
					}
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

		// Conducts the scrap search and returns the found items
		public static List<MapItem> SearchScrap(string searchTerm)
		{
			try
			{
				List<MapItem> results = new List<MapItem>();

				using (SqliteDataReader reader = Database.ExecuteQueryScrapSearch(searchTerm, SettingsSearch.searchInterior))
				{
					// Collect some variables which will always be the same for every result and are required for an instance of MapItem
					string signature = ConvertSignature("MISC", false);
					List<string> lockTypes = GetPermittedLockTypes();
					double spawnChance = GetSpawnChance("MISC", string.Empty);

					while (reader.Read())
					{
						// Sub-query for interior can return null
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
							reader.GetInt32(1), // Count
							reader.GetString(2), // Cell Display Name/location
							reader.GetString(3))); // Cell editorID
					}
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
		public static List<MapItem> SearchNPC(string searchTerm, int minChance)
		{
			try
			{
				List<MapItem> results = new List<MapItem>();

				using (SqliteDataReader reader = Database.ExecuteQueryNPCSearch(searchTerm, minChance / 100.00, SettingsSearch.searchInterior))
				{
					// Collect some variables which will always be the same for every result and are required for an instance of MapItem
					string signature = ConvertSignature("NPC_", false);
					List<string> lockTypes = GetPermittedLockTypes();

					while (reader.Read())
					{
						// Sub-query for interior can return null
						if (reader.IsDBNull(0))
						{
							continue;
						}

						double spawnChance = Math.Round(reader.GetDouble(2), 2);
						string name = reader.GetString(0);

						results.Add(new MapItem(
							Type.NPC,
							name, // FormID
							name + " [Min " + spawnChance + "%]", // Editor ID
							name, // Display Name
							signature,
							lockTypes, // The Lock Types filtered for this set of items.
							spawnChance,
							reader.GetInt32(1), // Count
							reader.GetString(3), // Cell Display Name/location
							reader.GetString(4))); // Cell editorID
					}
				}

				// Expand the NPC search, by also conducting a standard search of only NPC_, ignorant of lock filter
				results.AddRange(SearchStandard(searchTerm, SettingsSearch.searchInterior, new List<string> { "NPC_" }, GetPermittedLockTypes()));

				/*Copy out search results not containing "corpse", therefore dropping the dead "NPCs"
				This isn't perfect and won't catch ALL dead NPCs.
				A common pattern seems to be that things prefixed with 'Enc' are dead,
				but this isn't a global truth and filtering these out would cause too many false positives*/
				List<MapItem> itemsWithoutCorpse = new List<MapItem>();
				foreach (MapItem item in results)
				{
					if (item.editorID.Contains("corpse") || item.editorID.Contains("Corpse"))
					{
						continue;
					}
					else
					{
						itemsWithoutCorpse.Add(item);
					}
				}

				return itemsWithoutCorpse;
			}
			catch (Exception e)
			{
				Notify.Error("Mappalachia encountered an error while searching the database:\n" +
				IOManager.genericExceptionHelpText +
				e);

				return new List<MapItem>();
			}
		}

		// Gets the coordinate locations of everything within a cell, no filters
		public static List<MapDataPoint> GetAllCellCoords(string cellFormID)
		{
			List<MapDataPoint> coordinates = new List<MapDataPoint>();

			using (SqliteDataReader reader = Database.ExecuteQueryFindAllCoordinatesCell(cellFormID))
			{
				while (reader.Read())
				{
					coordinates.Add(new MapDataPoint(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2)));
				}
			}

			return coordinates;
		}

		// Return the coordinate locations and boundaries of instances of a FormID of an interior cell with given cellFormID
		public static List<MapDataPoint> GetCellCoords(string formID, string cellFormID, List<string> filteredLockTypes)
		{
			List<MapDataPoint> coordinates = new List<MapDataPoint>();

			using (SqliteDataReader reader = Database.ExecuteQueryFindCoordinatesCell(formID, cellFormID, filteredLockTypes))
			{
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
			}

			return coordinates;
		}

		// Return the coordinate locations and boundaries of instances of a FormID
		public static List<MapDataPoint> GetStandardCoords(string formID, List<string> filteredLockTypes)
		{
			List<MapDataPoint> coordinates = new List<MapDataPoint>();

			using (SqliteDataReader reader = Database.ExecuteQueryFindCoordinatesStandard(formID, filteredLockTypes))
			{
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
			}

			return coordinates;
		}

		// Return the coordinate locations of instances of an NPC above given min spawn chance
		public static List<MapDataPoint> GetNPCCoords(string npc, double minChance)
		{
			List<MapDataPoint> coordinates = new List<MapDataPoint>();

			using (SqliteDataReader reader = Database.ExecuteQueryFindCoordinatesNPC(npc, minChance))
			{
				while (reader.Read())
				{
					coordinates.Add(new MapDataPoint(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2))
					{
						weight = reader.GetInt32(3) / 100d, // Divide weighting by 100 as npc weighting is a percentage and we need 1=100%
					});
				}
			}

			return coordinates;
		}

		// Return the coordinate locations of instances of Scrap contained within Junk
		public static List<MapDataPoint> GetScrapCoords(string scrap)
		{
			List<MapDataPoint> coordinates = new List<MapDataPoint>();

			using (SqliteDataReader reader = Database.ExecuteQueryFindCoordinatesJunkScrap(scrap))
			{
				while (reader.Read())
				{
					coordinates.Add(new MapDataPoint(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2))
					{
						weight = reader.GetInt32(3),
					});
				}
			}

			return coordinates;
		}
	}
}
