using Library;

namespace Mappalachia
{
	// Represents an amount of BaseInstances which share the same basic properties
	// This is used to represent a row on the search results
	public class GroupedInstance(Entity entity, Space space, int count, int legendGroup, string label, LockLevel lockLevel, double spawnWeight = 1, bool inContainer = false)
		: BaseInstance(entity, space, label, lockLevel, spawnWeight, inContainer)
	{
		public int Count { get; } = count;

		public int LegendGroup { get; } = legendGroup;

		public PlotIcon? PlotIcon { get; set; } = null;
	}
}
