using Library;

namespace Mappalachia
{
	// Represents an amount of BaseInstances which share the same basic properties
	// This is used to represent a row on the search results
	public class GroupedInstance(Entity entity, Space space, int count, int legendGroup, string label, LockLevel lockLevel, float spawnWeight = 1)
		: BaseInstance(entity, space, label, lockLevel, spawnWeight)
	{
		public int Count { get; } = count;

		public int LegendGroup { get; } = legendGroup;
	}
}
