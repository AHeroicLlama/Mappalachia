using Library;

namespace Mappalachia
{
	// Represents an amount of BaseInstances which share the same basic properties
	// This is used to represent a row on the search results
	// Note that this is *not* a _collection_ of Instance
	public class GroupedInstance(Entity entity, Space space, int count, int legendGroup, string label, LockLevel lockLevel)
		: BaseInstance(entity, space, label, lockLevel)
	{
		public int Count { get; } = count;

		public int LegendGroup { get; } = legendGroup;
	}
}
