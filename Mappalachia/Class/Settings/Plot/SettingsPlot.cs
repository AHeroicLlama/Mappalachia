// Generic plot settings
using Mappalachia;

static class SettingsPlot
{
	public enum Mode
	{
		Icon,
		Heatmap,
		Topography,
		Cluster,
	}

	public static Mode mode = Mode.Icon;
	public static bool drawVolumes = true; // Draw volumes when in icon/topograph mode
	public static bool fillRegions = true; // Fill regions with transparent color
	public static bool labelInstanceIDs = false; // Label each MapDataPoint with its FormID

	public static bool IsHeatmap()
	{
		return mode == Mode.Heatmap;
	}

	public static bool IsIconOrTopographic()
	{
		return IsIcon() || IsTopographic();
	}

	public static bool IsIcon()
	{
		return mode == Mode.Icon;
	}

	public static bool IsTopographic()
	{
		return mode == Mode.Topography;
	}

	// Given the current state, should we use the single topograph legend color?
	public static bool ShouldUseSingleTopographColor(MapItem mapItem)
	{
		return IsTopographic() && !mapItem.IsRegion();
	}

	public static bool IsCluster()
	{
		return mode == Mode.Cluster;
	}
}
