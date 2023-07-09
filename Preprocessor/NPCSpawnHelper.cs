using System;
using System.Collections.Generic;
using System.Text;

namespace Mappalachia
{
	// Functionality for dealing with Location and NPC Spawns
	class NPCSpawnHelper
	{
		// Using the actual locations file, and the pre-summed total odds for each location, calculate each spawn chance into an absolute chance
		public static CSVFile ProcessNPCSpawns(CSVFile locationFile, List<Location> spawnChances)
		{
			List<string> npcSpawnHeader = new List<string> { "npc", "spawnClass", "locationFormID", "chance" };
			List<CSVRow> rows = new List<CSVRow>();
			foreach (CSVRow row in locationFile.rows)
			{
				string rowNPC = row.GetCellFromColumn("property");
				string rowFormID = row.GetCellFromColumn("locationFormID");

				// Calculate the 'class' (Main or Sub)
				string rowClass;
				if (rowNPC.Contains("Main"))
				{
					rowClass = "Main";
				}
				else if (rowNPC.Contains("Sub"))
				{
					rowClass = "Sub";
				}
				else
				{
					continue;
				}

				rowNPC = TrimNPCName(rowNPC);

				// Get the overall spawning odds for this location
				double locationOverallOdds = 0;
				foreach (Location spawnChance in spawnChances)
				{
					if (spawnChance.locationFormID == rowFormID)
					{
						locationOverallOdds = spawnChance.GetOdds(rowClass);
					}
				}

				// The NPC spawn chance is its individual chance over the sum spawn chances of the given location
				double rowSpawnChance = Math.Round((double.Parse(row.GetCellFromColumn("value")) / locationOverallOdds) * 100, 2);

				rows.Add(new CSVRow(rowNPC + "," + rowClass + "," + rowFormID + "," + rowSpawnChance, npcSpawnHeader));
			}

			return new CSVFile("NPC_Spawn.csv", npcSpawnHeader, rows);
		}

		// Sum the different spawn odds for each location
		public static List<Location> SumLocationSpawnChances(CSVFile file)
		{
			List<Location> spawnChances = new List<Location>();
			foreach (CSVRow row in file.rows)
			{
				// skip the entries that are not ESSChance AKA monster spawns
				if (!row.GetCellFromColumn("property").Contains("ESSChance"))
				{
					continue;
				}

				string locationFormID = row.GetCellFromColumn("locationFormID");

				// verify if the location already has an entry
				bool newEntry = true;
				foreach (Location locationSpawnChance in spawnChances)
				{
					// Use the existing location as already on record
					if (locationSpawnChance.locationFormID == locationFormID)
					{
						locationSpawnChance.AddOdds(row.GetCellFromColumn("value"), row.GetCellFromColumn("property"));
						newEntry = false;
						break;
					}
				}

				if (newEntry)
				{
					// The location is new to us, so create a new entry
					Location locChance = new Location(locationFormID);
					locChance.AddOdds(row.GetCellFromColumn("value"), row.GetCellFromColumn("property"));
					spawnChances.Add(locChance);
				}
			}

			return spawnChances;
		}

		// Reduce an NPC name to one familiar to a user/player
		public static string TrimNPCName(string name)
		{
			// Drop the plurals
			if (name.EndsWith("s"))
			{
				name = name.Remove(name.Length - 1);
			}

			// Drop the ESSChance prefix
			name = new StringBuilder(name)
			.Replace("ESSChanceSub", string.Empty)
			.Replace("ESSChanceMain", string.Empty)
			.Replace("ESSChanceCritterA", string.Empty)
			.Replace("ESSChanceCritterB", string.Empty)

			// Drop the editor labels
			.Replace("LARGE", string.Empty)
			.Replace("GIANTONLY", string.Empty)

			// Convert names
			.Replace("Rat", "Rad Rat")
			.Replace("Toad", "Rad Toad")
			.Replace("RadAnt", "Rad Ant")
			.Replace("Scorpions", "Rad Scorpion")
			.Replace("MoleMiner", "Mole Miner")
			.Replace("ViciousDog", "Vicious Dog")
			.Replace("SuperMutant", "Super Mutant")
			.Replace("Megasloth", "Mega Sloth")
			.Replace("Molerat", "Mole Rat")
			.Replace("YaoGuai", "Yao Guai")
			.Replace("FogCrawler", "Fog Crawler")
			.Replace("HoneyBeast", "Honey Beast")
			.Replace("CaveCricket", "Cave Cricket")
			.Replace("Mutations", "Snallygaster")
			.Replace("GraftonMonster", "Grafton Monster")
			.Replace("Swamp", "Gulper")
			.Replace("RadFrog", "Frog")
			.Replace("RadStag", "Radstag")
			.ToString();

			return name;
		}

		// For the Worldspace and Interior files, where monsters spawn, we should record the 'class' of monster spawn in a new column
		public static CSVFile AddMonsterClassColumn(CSVFile inputFile)
		{
			List<string> newFileHeader = inputFile.header;
			newFileHeader.Add("spawnClass");
			inputFile.header = newFileHeader;
			List<CSVRow> newFileRows = new List<CSVRow>();

			// Add a new column for monster spawn class, or blank if not found
			foreach (CSVRow row in inputFile.rows)
			{
				string monsterClass = GetClassFromName(row.GetCellFromColumn("referenceFormID"));

				string newRow = row.ToString() + "," + monsterClass;
				newFileRows.Add(new CSVRow(newRow, newFileHeader));
			}

			return new CSVFile(inputFile.fileName, newFileHeader, newFileRows);
		}

		// Find the monster 'class' given the full name of the entity reference
		public static string GetClassFromName(string fullName)
		{
			return fullName.Contains("Main") ? "Main" : (fullName.Contains("Sub") ? "Sub" : string.Empty);
		}
	}
}
