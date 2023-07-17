using System;

namespace CommonwealthCartography
{
	// A representation of a given LCTN and its total ESSChance property values, which control the odds of mobs spawning
	class Location
	{
		public readonly string locationFormID;

		public Location(string locationFormID)
		{
			this.locationFormID = locationFormID;
		}
	}
}
