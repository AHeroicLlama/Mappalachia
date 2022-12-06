using System.Collections.Generic;
using System.Drawing;
using Mappalachia;

// Plot Icon Settings and their defaults, used in FormPlotIconSettings
static class SettingsPlotStyle
{
	// Defaults
	const int iconSizeDefault = 40;
	const int lineWidthDefault = 3;
	const int iconOpacityPercentDefault = 100;
	const int shadowOpacityPercentDefault = 40;

	public static readonly List<PlotIconShape> paletteShapeDefault = new List<PlotIconShape>
		{
			// 				Diamond square circle inner outer  frame  marker  fill
			new PlotIconShape(false, false, true, false, true, false, false, false),
			new PlotIconShape(true, false, false, false, false, false, false, false),
			new PlotIconShape(false, false, false, false, false, false, true, true),
			new PlotIconShape(false, true, false, false, false, false, false, false),
			new PlotIconShape(false, false, false, false, false, true, false, false),
		};

	public static readonly List<Color> paletteColorDefault = new List<Color>
		{
			Color.Red, Color.Cyan, Color.Yellow, Color.Magenta, Color.Lime, Color.RoyalBlue, Color.Coral, Color.Green,
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
	public const int iconSizeMin = 10;
	public const int iconSizeMax = 100;
	public const int lineWidthMin = 1;
	public const int lineWidthMax = 8;
	public const int iconOpacityPercentMin = 10;
	public const int iconOpacityPercentMax = 100;
	public const int shadowOpacityPercentMin = 0;
	public const int shadowOpacityPercentMax = 100;

	// Settings variables;
	public static int iconSize;
	public static int lineWidth;
	public static int iconOpacityPercent;
	public static int shadowOpacityPercent;
	public static List<Color> paletteColor;
	public static List<PlotIconShape> paletteShape;

	// Constructor
	static SettingsPlotStyle()
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

	public static Color GetFirstColor()
	{
		return paletteColor[0];
	}

	// Return the second color, although just return the first if there is only one color in the palette
	public static Color GetSecondColor()
	{
		return paletteColor.Count == 1 ? GetFirstColor() : paletteColor[1];
	}
}