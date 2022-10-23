namespace Mappalachia
{
	// Settings for the map image
	class SettingsMap
	{
		public enum Background
		{
			Normal,
			Military,
			Satellite,
		}

		public enum LegendMode
		{
			Compact,
			Extended,
			Hidden,
		}

		// Min/maxes and defaults
		public static readonly int brightnessMin = 5;
		public static readonly int brightnessMax = 300;

		public static readonly int brightnessDefault = 75;
		public static readonly Background backgroundDefault = Background.Normal;
		public static readonly bool grayScaleDefault = false;
		public static readonly bool showMapLabelsDefault = false;
		public static readonly bool showMapIconsDefault = false;
		public static readonly LegendMode legendModeDefault = LegendMode.Compact;

		// User-definable settings
		public static int brightness = brightnessDefault;
		public static Background background = backgroundDefault;
		public static bool grayScale = grayScaleDefault;
		public static bool showMapLabels = showMapLabelsDefault;
		public static bool showMapIcons = showMapIconsDefault;
		public static LegendMode legendMode = legendModeDefault;

		public static bool ExtendedMargin()
		{
			return legendMode == LegendMode.Extended;
		}

		public static bool HiddenMargin()
		{
			return legendMode == LegendMode.Hidden;
		}
	}
}
