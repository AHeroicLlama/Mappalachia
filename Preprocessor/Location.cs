using System;

namespace CommonwealthCartography
{
	// A representation of a given LCTN and its total ESSChance property values, which control the odds of mobs spawning
	class Location
	{
		public readonly string locationFormID;
		public double oddsMain;
		public double oddsSub;

		public Location(string locationFormID)
		{
			this.locationFormID = locationFormID;
			oddsMain = 0;
			oddsSub = 0;
		}

		public double GetOdds(string npcClass)
		{
			switch (NPCSpawnHelper.GetClassFromName(npcClass))
			{
				case "Main":
					return oddsMain;
				case "Sub":
					return oddsSub;
				default:
					throw new Exception("NPC Class " + npcClass + " is not recognised");
			}
		}

		// Increment the odds for a given spawn class at a given location
		public void AddOdds(string odds, string npcClass)
		{
			switch (NPCSpawnHelper.GetClassFromName(npcClass))
			{
				case "Main":
					oddsMain += double.Parse(odds);
					return;
				case "Sub":
					oddsSub += double.Parse(odds);
					return;
				default:
					return; // Do nothing - we're only interested in Main and Sub
			}
		}
	}
}
