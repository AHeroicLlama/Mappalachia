﻿using System.Drawing;

// Internal settings/defaults for topographic plotting
static class SettingsPlotTopograph
{
	public static readonly int defaultColorBands = 3; // The default number of colors from the color palette to insert as interpolated 'milestone' colors on the topograph range
	public static int colorBands = defaultColorBands;
	public static readonly Color legendColor = Color.Orange;
	public static readonly int heightKeyIndicators = 20; // How many different lines should show on the key for height/color
	public static readonly string heightKeyString = "^^^^";
	public static readonly int zThreshUpper = 45000; // Thresholds for heights considered an outlier
	public static readonly int zThreshLower = -8000;
}