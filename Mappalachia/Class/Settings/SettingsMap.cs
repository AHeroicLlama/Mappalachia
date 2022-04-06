namespace Mappalachia
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
		public static readonly bool showMapMarkersDefault = false;

		// User-definable settings
		public static int brightness = brightnessDefault;
		public static bool layerMilitary = layerMilitaryDefault;
		public static bool grayScale = grayScaleDefault;
		public static bool showMapMarkers = showMapMarkersDefault;
	}
}
