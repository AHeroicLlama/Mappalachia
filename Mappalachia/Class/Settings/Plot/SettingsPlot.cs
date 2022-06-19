// Generic plot settings
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

	public static bool IsCluster()
	{
		return mode == Mode.Cluster;
	}
}
