using System.Collections.Generic;
using System.Text;

namespace CommonwealthCartography
{
	// Functionality for dealing with Location and NPC Spawns
	class NPCSpawnHelper
	{
		// Using the actual locations file, and the pre-summed total odds for each location, calculate each spawn chance into an absolute chance
		public static CSVFile ProcessNPCSpawns(CSVFile locationFile)
		{
			List<string> npcSpawnHeader = new List<string> { "npc", "locationFormID", "chance" };
			List<CSVRow> rows = new List<CSVRow>();
			foreach (CSVRow row in locationFile.rows)
			{
				string rowNPC = row.GetCellFromColumn("property");
				string rowFormID = row.GetCellFromColumn("locationFormID");

				rowNPC = TrimNPCName(rowNPC);

				// The NPC spawn chance is its individual chance over the sum spawn chances of the given location
				double rowSpawnChance = 100;

				rows.Add(new CSVRow(rowNPC + "," + rowFormID + "," + rowSpawnChance, npcSpawnHeader));
			}

			return new CSVFile("NPC_Spawn.csv", npcSpawnHeader, rows);
		}

		// Reduce an NPC name to one familiar to a user/player
		public static string TrimNPCName(string name)
		{
			// Drop the plurals
			if (name.EndsWith("s"))
			{
				name = name.Remove(name.Length - 1);
			}

			// Drop the LocEnc prefix
			name = new StringBuilder(name)
			.Replace("LocEnc", string.Empty)

			// Convert names
			.Replace("SuperMutant", "Super Mutant")
			.Replace("ChildrenOfAtom", "Children Of Atom")
			.Replace("BrotherhoodOfSteel", "Brotherhood Of Steel")
			.ToString();

			return name;
		}
	}
}
