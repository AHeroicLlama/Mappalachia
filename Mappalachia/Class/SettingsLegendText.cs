using System.Collections.Generic;

namespace Mappalachia.Class
{
	//Stores settings for overridden legend text by legend group.
	static class SettingsLegendText
	{
		public static Dictionary<int, string> legendTexts = new Dictionary<int, string>();

		//Merges existing override texts with new groups added/modified in the legend list
		public static void UpdateGroups()
		{
			Dictionary<int, string> newLegendTexts = new Dictionary<int, string>();
			
			foreach (MapItem mapItem in FormMaster.legendItems)
			{
				int legendGroup = mapItem.legendGroup;

				//If we're finding repeat entries for the same legend group
				if (newLegendTexts.ContainsKey(legendGroup))
				{
					continue;
				}

				//Keep the old overriding text if this group still exists, otherwise add an empty entry
				string text = legendTexts.ContainsKey(legendGroup) ?
					legendTexts[legendGroup] :
					string.Empty;

				newLegendTexts.Add(legendGroup, text);
			}

			legendTexts = newLegendTexts;
		}

		//Add or update a record for an overriding legend text
		public static void UpdateLegend(string groupInt, string text)
		{
			//groupInt is programatically always an int
			int group = int.Parse(groupInt);

			if (legendTexts.ContainsKey(group))
			{
				legendTexts[group] = text;
			}
			else
			{
				legendTexts.Add(group, text);
			}
		}

		//Resets all overriding text
		public static void ResetAll()
		{
			legendTexts = new Dictionary<int, string>();
			UpdateGroups();
		}
	}
}
