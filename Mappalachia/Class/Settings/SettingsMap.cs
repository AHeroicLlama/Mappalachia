using Mappalachia.Class;
using System;

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
			None,
		}

		public enum LegendMode
		{
			Compact,
			Extended,
			Hidden,
		}

		// Min/maxes
		public const int brightnessMin = 5;
		public const int brightnessMax = 300;

		// Defaults
		public const int brightnessDefault = 50;
		public const Background backgroundDefault = Background.Normal;
		public const bool grayScaleDefault = false;
		public const bool showMapLabelsDefault = false;
		public const bool showMapIconsDefault = false;
		public const bool grayScaleMapIconsDefault = false;
		public const LegendMode legendModeDefault = LegendMode.Compact;
		public const bool highlightWaterDefault = false;
		public static readonly string titleDefault = string.Empty;

		// User-definable settings
		public static int brightness = brightnessDefault;
		public static Background background = backgroundDefault;
		public static bool grayScale = grayScaleDefault;
		public static bool showMapLabels = showMapLabelsDefault;
		public static bool showMapIcons = showMapIconsDefault;
		public static bool grayScaleMapIcons = grayScaleMapIconsDefault;
		public static LegendMode legendMode = legendModeDefault;
		public static bool highlightWater = highlightWaterDefault;
		public static string title = titleDefault;

		// Nuke zone flux weights
		public static bool drawOptimalNukeZone = false;
		public static float fluxWeightCrimson = 1.0f;
		public static float fluxWeightCobalt = 0.0f;
		public static float fluxWeightFluorescent = 0.0f;
		public static float fluxWeightViolet = 0.0f;
		public static float fluxWeightYellowcake = 0.0f;

		// Returns the normalized weighting (0-1f) of the given flux color, given the weights of all fluxes
		public static float GetNormalizedFluxWeight(NukeZone.FluxColor color)
		{
			float total = fluxWeightCrimson + fluxWeightCobalt + fluxWeightFluorescent + fluxWeightViolet + fluxWeightYellowcake;

			switch (color)
			{
				case NukeZone.FluxColor.Crimson:
					return fluxWeightCrimson / total;

				case NukeZone.FluxColor.Cobalt:
					return fluxWeightCobalt / total;

				case NukeZone.FluxColor.Fluorescent:
					return fluxWeightFluorescent / total;

				case NukeZone.FluxColor.Yellowcake:
					return fluxWeightYellowcake / total;

				case NukeZone.FluxColor.Violet:
					return fluxWeightViolet / total;

				default:
					throw new ArgumentException("Unknown Flux color " + color.ToString());
			}
		}

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
