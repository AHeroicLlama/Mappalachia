using System;
using System.Collections.Generic;
using System.Linq;

namespace Mappalachia
{
	// Provides data helper methods, data translation and sorting
	public static class DataHelper
	{
		// A list of entities which are often (mis)represented instead by LVLI in the data
		public static readonly List<string> typicalLVLIItems = new List<string> { "FLOR", "ALCH", "WEAP", "ARMO", "BOOK", "AMMO" };

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
			{ "SOUN", "Trigger for sound effect." },
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
			{ "PROJ", "An 'armed' weapon such as a mine." },
			{ "CNCY", string.Empty },
			{ "REGN", "Defines the edges of map regions." },
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
			{ "TXST", "Decal" },
			{ "DOOR", "Door" },
			{ "ALCH", "Aid" },
			{ "SOUN", "Sound" },
			{ "NOTE", "Holotape" },
			{ "FLOR", "Flora" },
			{ "ARMO", "Armor/Apparel" },
			{ "ASPC", "Acoustics" },
			{ "WEAP", "Weapon" },
			{ "KEYM", "Key" },
			{ "TACT", "Voice activator" },
			{ "HAZD", "Hazard" },
			{ "AMMO", "Ammo" },
			{ "IDLM", "Idle marker" },
			{ "BNDS", "Spline" },
			{ "SECH", "Echo" },
			{ "PROJ", "Projectile" },
			{ "CNCY", "Currency" },
			{ "REGN", "Region" },
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
			{ string.Empty,         "Not locked" },
			{ "Novice (Level 0)",   "Level 0" },
			{ "Advanced (Level 1)", "Level 1" },
			{ "Expert (Level 2)",   "Level 2" },
			{ "Master (Level 3)",   "Level 3" },
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
			"Inaccessible",
			"Unknown",
		};

		// Signatures which could be affected by a lock level
		public static readonly List<string> lockableTypes = new List<string> { "CONT", "DOOR", "TERM" };

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

		// Convert a List of lockLevel to the proper or user-friendly version of itself
		// Works regardless of the current state of the given lock level
		public static List<string> ConvertLockLevel(List<string> lockLevels, bool properName)
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
			return "%" + input.Trim()
				.Replace("_", "\\_")
				.Replace("%", "\\%")
				.Replace(" ", "%") + "%";
		}

		// Indicate the spawn chance of a standard item based on understandings of LVLI
		public static float GetSpawnChance(string signature, string editorID)
		{
			return (signature == "LVLI" || editorID.Contains("ChanceNone")) ? -1 : 100;
		}

		// Returns the nth item in a list as if it were cyclic (supports <0 or >n)
		public static T GetCyclicItem<T>(List<T> collection, int n)
		{
			if (collection.Count == 0)
			{
				return default;
			}

			n %= collection.Count;

			if (n < 0)
			{
				n = collection.Count + n;
			}

			return collection[n];
		}
	}
}
