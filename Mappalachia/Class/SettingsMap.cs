namespace Mappalachia
{
	//Settings for the map image
	class SettingsMap
	{
		//Min/maxes - these control the min/maxes on the form
		public static readonly int minBrightness = 5;
		public static readonly int maxBrightness = 300;

		//User-definable settings
		public static int brightness = Map.defaultBrightness;
		public static bool layerMilitary = false;
		public static bool layerNWMorgantown = false;
		public static bool layerNWFlatwoods = false;
		public static bool grayScale = false;
	}
}
