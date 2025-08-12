using Library;

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

		static int MaxRange { get; } = 25000;

		// Range in game units
		public int Range { get; set; } = 10000;

		public int MinWeight { get; set; } = 3;

		public bool LiveUpdate { get; set; } = true;

		public bool ClusterPerLegendGroup { get; set; } = false;

		public static int GetMaxRangeForSpace(Space space)
		{
			return (int)Math.Min(space.MaxRange / 2, MaxRange);
		}

		public void CapToSpace(Space space)
		{
			Range = Math.Min(GetMaxRangeForSpace(space), Range);
		}
	}
}
