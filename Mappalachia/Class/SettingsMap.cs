namespace Mappalachia
{
	// Settings for the map image
	class SettingsMap
	{
		public enum Mode
		{
			Worldspace,
			Cell,
		}

		public static Mode mode = Mode.Worldspace;

		public static bool IsCellModeActive()
		{
			return mode == Mode.Cell;
		}

		// Min/maxes - these control the min/maxes on the form
		public static readonly int brightnessMin = 5;
		public static readonly int brightnessMax = 300;

		public static readonly int brightnessDefault = 50;

		// User-definable settings
		public static int brightness = brightnessDefault;
		public static bool layerMilitary = false;
		public static bool layerNWMorgantown = false;
		public static bool layerNWFlatwoods = false;
		public static bool grayScale = false;
	}
}
