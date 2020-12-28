using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace Mappalachia
{
	//Interfaces with the database and provides helper methods and data translation
	static class DataHelper
	{
		static List<string> permittedLockTypes;
		static List<string> permittedSignatures;

		static readonly Dictionary<string, string> signatureDescription = new Dictionary<string, string>
		{
			{
				"LVLI", "Lootable object which has some odds of spawning (up to and including 100% chances)\n" +
				"Many items of different categories are in fact included under this one."
			},
			{ "FLOR", "Collectable natural resource." },
			{ "MISC", "Junk, Scrap or Mod." },
			{
				"ACTI", "Defined volume of space, invisible in-game.\n" +
				"Activators can mark out designated areas, trigger events, or act as 'hit-boxes' for interaction."
			},
			{
				"FURN", "Object or position which a character can use to enter into an animation.\n" +
				"Includes workbenches and instruments, but also NPC ambush positions such as scorchbeast spawns."
			},
			{ "CONT", string.Empty },
			{ "NPC_", string.Empty },
			{ "DOOR", string.Empty },
			{ "HAZD", "Area of space which can harm the player (fire/radiation)." },
			{ "BOOK", "Note, Plan or Recipe." },
			{ "ALCH", "Food, Drink, Chems etc." },
			{ "TERM", string.Empty },
			{ "NOTE", string.Empty },
			{ "WEAP", string.Empty },
			{ "ARMO", "Apparel including Armor." },
			{ "AMMO", string.Empty },
			{ "TACT", "A trigger/activator which causes a voice line to be played." },
			{ "KEYM", string.Empty },
		};

		//Provide a user-friendly name for each signature which best represents what a typical player would know them as
		static readonly Dictionary<string, string> signatureToFriendlyName = new Dictionary<string, string>
		{
			{ "LVLI", "Loot" },
			{ "FLOR", "Flora" },
			{ "MISC", "Junk/Scrap" },
			{ "ACTI", "Activator" },
			{ "FURN", "Furniture" },
			{ "CONT", "Container" },
			{ "NPC_", "NPC" },
			{ "DOOR", "Door" },
			{ "HAZD", "Hazard" },
			{ "BOOK", "Note" },
			{ "ALCH", "Aid" },
			{ "TERM", "Terminal" },
			{ "NOTE", "Holotape" },
			{ "WEAP", "Weapon" },
			{ "ARMO", "Apparel" },
			{ "AMMO", "Ammo" },
			{ "TACT", "Voice trigger" },
			{ "KEYM", "Key" },
		};

		//Inverse the user friendly signature names so we can use the proper signatures in queries
		static readonly Dictionary<string, string> signatureToProperName = signatureToFriendlyName.ToDictionary(x => x.Value, x => x.Key);

		//Provide a user-friendly name to the lock level
		static readonly Dictionary<string, string> lockLevelToFriendlyName = new Dictionary<string, string>
		{
			{ string.Empty,			"Not locked" },
			{ "Novice (Level 0)",	"Level 0" },
			{ "Advanced (Level 1)", "Level 1" },
			{ "Expert (Level 2)",	"Level 2" },
			{ "Master (Level 3)",	"Level 3" },
		};

		//Inverse the user friendly lock names so we can use the proper lock levels in queries
		static readonly Dictionary<string, string> lockLevelToProperName = lockLevelToFriendlyName.ToDictionary(x => x.Value, x => x.Key);

		//Convert a signature to the proper or user-friendly version of itself
		//Works regardless of the current state of the given signature
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
				return "Unsupported signature!";
			}
		}

		//Convert a lockLevel to the proper or user-friendly version of itself
		//Works regardless of the current state of the given lock level
		public static string ConvertLockLevel(string lockLevel, bool properName)
		{
			try
			{
				//We don't convert all lock levels
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
				return "Unsupported lock level!";
			}
		}

		//runs ConvertLockLevel against an entire collection of lockLevel
		public static List<string> ConvertLockLevelCollection(List<string> lockLevels, bool properName)
		{
			List<string> result = new List<string>();

			foreach (string lockLevel in lockLevels)
			{
				result.Add(ConvertLockLevel(lockLevel, properName));
			}

			return result;
		}

		//Find the description of a given signature, in either long or short form
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

		//Escape functional SQL characters and wildcard on space
		public static string ProcessSearchString(string input)
		{
			return input.Trim()
				.Replace("_", "\\_")
				.Replace("%", "\\%")
				.Replace(" ", "%");
		}

		//Find all the unique signatures across the data
		public static List<string> GetPermittedSignatures()
		{
			//Singleton
			if (DataHelper.permittedSignatures != null)
			{
				return DataHelper.permittedSignatures;
			}

			List<string> permittedSignatures = new List<string>();
			SqliteDataReader reader = Queries.ExecuteQuerySignatures();
			while (reader.Read())
			{
				permittedSignatures.Add(reader.GetString(0));
			}

			DataHelper.permittedSignatures = permittedSignatures;
			return permittedSignatures;
		}

		//Find all the unique lock levels across the data
		public static List<string> GetPermittedLockTypes()
		{
			//Singleton
			if (DataHelper.permittedLockTypes != null)
			{
				return DataHelper.permittedLockTypes;
			}

			List<string> permittedLockTypes = new List<string>();
			SqliteDataReader reader = Queries.ExecuteQueryLockLevels();
			while (reader.Read())
			{
				permittedLockTypes.Add(reader.GetString(0));
			}

			DataHelper.permittedLockTypes = permittedLockTypes;
			return permittedLockTypes;
		}

		//Find all the unique NPC types under the variable spawns
		public static List<string> GetVariableNPCTypes()
		{
			List<string> npcTypes = new List<string>();
			SqliteDataReader reader = Queries.ExecuteQueryNPCTypes();
			while (reader.Read())
			{
				npcTypes.Add(reader.GetString(0));
			}

			return npcTypes;
		}

		//Find all the unique scrap types in the quantified junk table
		public static List<string> GetVariableScrapTypes()
		{
			List<string> scrapTypes = new List<string>();
			SqliteDataReader reader = Queries.ExecuteQueryScrapTypes();
			while (reader.Read())
			{
				scrapTypes.Add(reader.GetString(0));
			}

			return scrapTypes;
		}

		//Indicate the spawn chance based on understandings of LVLI
		public static double GetSpawnChance(string signature)
		{
			if (signature == "LVLI")
			{
				return -1;
			}
			else
			{
				return 100;
			}
		}

		//Return the coordinate locations and boundaries of instances of a FormID
		public static List<MapDataPoint> GetSimpleCoords(string formID, List<string> filteredLockTypes)
		{
			List<MapDataPoint> coordinates = new List<MapDataPoint>();

			using (SqliteDataReader reader = Queries.ExecuteQueryFindCoordinatesSimple(formID, filteredLockTypes))
			{
				while (reader.Read())
				{
					coordinates.Add(new MapDataPoint(reader.GetInt32(0), -reader.GetInt32(1), 1d, reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4)));
				}
			}

			return coordinates;
		}

		//Return the coordinate locations of instances of an NPC above given min spawn chance
		public static List<MapDataPoint> GetNPCCoords(string npc, double minChance)
		{
			List<MapDataPoint> coordinates = new List<MapDataPoint>();

			using (SqliteDataReader reader = Queries.ExecuteQueryFindCoordinatesNPC(npc, minChance))
			{
				while (reader.Read())
				{
					//Divide weighting by 100 as npc weighting is a percentage and we need 1=100%
					coordinates.Add(new MapDataPoint(reader.GetInt32(0), -reader.GetInt32(1), reader.GetInt32(2) / 100d, null, null, null));
				}
			}

			return coordinates;
		}

		//Return the coordinate locations of instances of Scrap contained within Junk
		public static List<MapDataPoint> GetScrapCoords(string scrap)
		{
			List<MapDataPoint> coordinates = new List<MapDataPoint>();

			using (SqliteDataReader reader = Queries.ExecuteQueryFindCoordinatesJunkScrap(scrap))
			{
				while (reader.Read())
				{
					coordinates.Add(new MapDataPoint(reader.GetInt32(0), -reader.GetInt32(1), reader.GetInt32(2), null, null, null));
				}
			}

			return coordinates;
		}
	}
}