using System.Collections.Generic;
using System.Drawing;

namespace Mappalachia.Class
{
	// Generic plot settings
	static class SettingsPlot
	{
		public enum Mode
		{
			Icon,
			Heatmap,
			Topography,
		}

		public static Mode mode = Mode.Icon;
		public static bool drawVolumes = true; // Draw volumes when in icon mode

		public static bool IsHeatmap()
		{
			return mode == Mode.Heatmap;
		}

		public static bool IsIconOrTopography()
		{
			return (IsIcon() || IsTopography());
		}

		public static bool IsIcon()
		{
			return mode == Mode.Icon;
		}

		public static bool IsTopography()
		{
			return mode == Mode.Topography;
		}
	}

	static class SettingsPlotTopography
	{
		public static Color legendColor = Color.Purple;
	}

	// Plot Icon Settings and their defaults, used in FormPlotIconSettings
	static class SettingsPlotIcon
	{
		// Defaults
		static readonly int iconSizeDefault = 40;
		static readonly int lineWidthDefault = 3;
		static readonly int iconOpacityPercentDefault = 100;
		static readonly int shadowOpacityPercentDefault = 40;

		public static readonly List<PlotIconShape> paletteShapeDefault = new List<PlotIconShape>
		{
			//				Diamond square circle inner outer  fill
			new PlotIconShape(false, false, true, false, true, false),
			new PlotIconShape(true, false, false, false, false, false),
			new PlotIconShape(false, false, false, true, false, false),
			new PlotIconShape(false, true, true, false, false, false),
		};

		public static readonly List<Color> paletteColorDefault = new List<Color>
		{
			Color.Red, Color.Cyan, Color.Yellow, Color.Magenta, Color.Lime, Color.RoyalBlue, Color.Coral, Color.DarkGreen,
		};

		// Colorblind palettes credit: IBM, Bang Wong, and Paul Tol. From https://davidmathlogic.com/colorblind/
		public static readonly List<Color> paletteColorBlindIBM = new List<Color>
		{
			Color.FromArgb(100, 143, 255),
			Color.FromArgb(120, 94, 120),
			Color.FromArgb(220, 38, 127),
			Color.FromArgb(254, 97, 0),
			Color.FromArgb(255, 176, 0),
		};

		// Excluded black due to visibilty against dark coloured map
		public static readonly List<Color> paletteColorBlindWong = new List<Color>
		{
			Color.FromArgb(230, 159, 0),
			Color.FromArgb(86, 180, 233),
			Color.FromArgb(0, 158, 115),
			Color.FromArgb(240, 228, 66),
			Color.FromArgb(0, 114, 178),
			Color.FromArgb(213, 94, 0),
			Color.FromArgb(204, 121, 167),
		};

		public static readonly List<Color> paletteColorBlindTol = new List<Color>
		{
			Color.FromArgb(51, 34, 136),
			Color.FromArgb(17, 119, 51),
			Color.FromArgb(68, 170, 153),
			Color.FromArgb(136, 204, 238),
			Color.FromArgb(221, 204, 119),
			Color.FromArgb(204, 102, 119),
			Color.FromArgb(170, 68, 153),
			Color.FromArgb(136, 34, 85),
		};

		// Min/Maxes - these control the min/maxes on the form.
		public static readonly int iconSizeMin = 10;
		public static readonly int iconSizeMax = 100;
		public static readonly int lineWidthMin = 1;
		public static readonly int lineWidthMax = 8;
		public static readonly int iconOpacityPercentMin = 10;
		public static readonly int iconOpacityPercentMax = 100;
		public static readonly int shadowOpacityPercentMin = 0;
		public static readonly int shadowOpacityPercentMax = 100;

		// Settings variables;
		public static int iconSize;
		public static int lineWidth;
		public static int iconOpacityPercent;
		public static int shadowOpacityPercent;
		public static List<Color> paletteColor;
		public static List<PlotIconShape> paletteShape;

		// Constructor
		static SettingsPlotIcon()
		{
			Initialize();
		}

		// Assign settings values their defaults
		public static void Initialize()
		{
			iconSize = iconSizeDefault;
			lineWidth = lineWidthDefault;
			iconOpacityPercent = iconOpacityPercentDefault;
			shadowOpacityPercent = shadowOpacityPercentDefault;
			paletteColor = new List<Color>(paletteColorDefault);
			paletteShape = new List<PlotIconShape>(paletteShapeDefault);
		}
	}

	// Settings for heatmap generation
	static class SettingsPlotHeatmap
	{
		public enum ColorMode
		{
			Mono,
			Duo,
		}

		public static bool IsMono()
		{
			return colorMode == ColorMode.Mono;
		}

		public static bool IsDuo()
		{
			return colorMode == ColorMode.Duo;
		}

		// Not user-definable
		public static readonly int blendDistance = 10;

		// Updating this? Check FormMaster.UpdateHeatMapResolution()
		public static readonly List<int> validResolutions = new List<int> { 128, 256, 512, 1024 };

		public static int resolution = validResolutions[1];
		public static ColorMode colorMode = ColorMode.Mono;
	}
}
