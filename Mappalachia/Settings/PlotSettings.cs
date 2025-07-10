namespace Mappalachia
{
	public enum PlotMode
	{
		Standard,
		Topographic,
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

		public bool ShowPlotsInOtherSpaces { get; set; } = true;

		public bool ShowRegionLevels { get; set; } = false;
	}
}
