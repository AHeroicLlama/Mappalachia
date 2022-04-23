﻿namespace Mappalachia
{
	// Settings for the map image
	class SettingsMap
	{
		// Min/maxes and defaults
		public static readonly int brightnessMin = 5;
		public static readonly int brightnessMax = 300;

		public static readonly int brightnessDefault = 50;
		public static readonly bool layerMilitaryDefault = false;
		public static readonly bool grayScaleDefault = false;
		public static readonly bool showMapLabelsDefault = false;
		public static readonly bool showMapIconsDefault = false;
		public static readonly bool hideLegendDefault = false;

		// User-definable settings
		public static int brightness = brightnessDefault;
		public static bool layerMilitary = layerMilitaryDefault;
		public static bool grayScale = grayScaleDefault;
		public static bool showMapLabels = showMapLabelsDefault;
		public static bool showMapIcons = showMapIconsDefault;
		public static bool hideLegend = hideLegendDefault;
	}
}
