using System.Collections.Generic;
using System.Drawing;

namespace Mappalachia.Class
{
	//Stores and caches legend icons against their legend groups
	static class PlotIconCache
	{
		static Dictionary<int, PlotIcon> plotIcons = new Dictionary<int, PlotIcon>();

		//Gets a PlotIcon for a given legend group. Returns cached version if available
		public static PlotIcon GetIconForGroup(int group)
		{
			int colorTotal = SettingsPlotIcon.paletteColor.Count;
			int shapeTotal = SettingsPlotIcon.paletteShape.Count;

			//Reduce the group number to find repeating icons
			//For example, 3 shapes and 5 colors makes 30 icons. Group 30 is therefore the same as group 0.
			group = group % (colorTotal * shapeTotal);

			//Return this ploticon if it has already been generated.
			if (plotIcons.ContainsKey(group))
			{
				return plotIcons[group];
			}

			//Identify which item from each palette should be used.
			//First iterate through every color, then every palette, and repeat.
			int colorIndex = group % colorTotal;
			int shapeIndex = (group / colorTotal) % shapeTotal;

			//Generate the PlotIcon
			Color color = SettingsPlotIcon.paletteColor[colorIndex];
			PlotIconShape shape = SettingsPlotIcon.paletteShape[shapeIndex];
			PlotIcon plotIcon = new PlotIcon(color, shape);

			//Register the icon in the cache and return it
			plotIcons.Add(group, plotIcon);
			return plotIcon;
		}

		//Clear the cache - used when plot icon settings are changed
		public static void ResetCache()
		{
			plotIcons = new Dictionary<int, PlotIcon>();
		}
	}
}
