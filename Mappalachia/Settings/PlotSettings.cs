namespace Mappalachia
{
	public enum PlotMode
	{
		Standard,
		Topographic,
		Heatmap,
		Cluster,
	}

	// Controls REGN and any Instances with associated Shapes
	public enum VolumeDrawMode
	{
		Fill,
		Border,
		Both,
	}

	public class PlotSettings
	{
		public PlotMode Mode { get; set; } = PlotMode.Standard;

		public bool DrawInstanceFormID { get; set; } = false;

		public VolumeDrawMode VolumeDrawMode { get; set; } = VolumeDrawMode.Both;

		public ClusterSettings ClusterSettings { get; set; } = new ClusterSettings();

		public PlotIconSettings PlotIconSettings { get; set; } = new PlotIconSettings();

		public bool AutoFindPlotsInConnectedSpaces { get; set; } = false;

		public bool ShowRegionLevels { get; set; } = true;
	}
}
