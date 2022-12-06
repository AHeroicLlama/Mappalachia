using System.Collections.Generic;

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
	public const int blendDistance = 10;

	// Updating this? Check FormMaster.UpdateHeatMapResolution()
	public static readonly List<int> validResolutions = new List<int> { 128, 256, 512, 1024 };

	public static int resolution = validResolutions[1];
	public static ColorMode colorMode = ColorMode.Mono;
}