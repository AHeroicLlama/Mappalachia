namespace Mappalachia
{
	public class ClusterSettings
	{
		public ClusterSettings(int range, int minWeight, bool liveUpdate, bool clusterPerLegendGroup)
		{
			Range = range;
			MinWeight = minWeight;
			LiveUpdate = liveUpdate;
			ClusterPerLegendGroup = clusterPerLegendGroup;
		}

		public ClusterSettings()
		{
		}

		public static int MaxRange { get; } = 50000;

		// Range in game units
		public int Range { get; set; } = 10000;

		public int MinWeight { get; set; } = 3;

		public bool LiveUpdate { get; set; } = true;

		public bool ClusterPerLegendGroup { get; set; } = false;
	}
}
