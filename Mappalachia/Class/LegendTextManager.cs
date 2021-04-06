using System.Collections.Generic;

namespace Mappalachia.Class
{
	//Stores settings for overridden legend text by legend group.
	static class LegendTextManager
	{
		//All texts including blank ones not yet overridden
		public static Dictionary<int, string> allLegendTexts = new Dictionary<int, string>();

		//Just overridden texts
		static Dictionary<int, string> overriddenTexts;

		//Resizes the list of overriding legends to account for groups added or removed on the main form
		public static void IncludeNewGroups()
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
				string text = allLegendTexts.ContainsKey(legendGroup) ?
					allLegendTexts[legendGroup] :
					string.Empty;

				newLegendTexts.Add(legendGroup, text);
			}

			allLegendTexts = newLegendTexts;
		}

		//Add or update a record for an overriding legend text
		public static void UpdateLegend(int group, string text)
		{
			if (allLegendTexts.ContainsKey(group))
			{
				allLegendTexts[group] = text;
			}
			else
			{
				allLegendTexts.Add(group, text);
			}

			//Reset the cache of overridden texts, since they just changed.
			overriddenTexts = null;
		}

		//Resets all overriding text
		public static void ResetAll()
		{
			allLegendTexts = new Dictionary<int, string>();
			overriddenTexts = null;
			IncludeNewGroups();
		}

		//Returns just the modified legend groups with their text
		public static Dictionary<int, string> GetOverriddenTexts()
		{
			if (overriddenTexts != null)
			{
				return overriddenTexts;
			}

			overriddenTexts = new Dictionary<int, string>();

			foreach (KeyValuePair<int, string> legendGroup in allLegendTexts)
			{
				if (legendGroup.Value != string.Empty)
				{
					overriddenTexts.Add(legendGroup.Key, legendGroup.Value);
				}
			}

			return overriddenTexts;
		}
	}
}
