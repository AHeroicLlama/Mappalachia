using Library;

namespace Mappalachia
{
	// Abstract of the shared properties of Instance or GroupedInstance
	public abstract class BaseInstance(Entity entity, Space space, string label, LockLevel lockLevel)
	{
		// The Entity which this is an instance of
		public Entity Entity { get; } = entity;

		// The Space which this exists in
		public Space Space { get; } = space;

		// The Level of the lock on this instance
		public LockLevel LockLevel { get; } = lockLevel;

		// The text 'label' of this instance
		public string Label { get; } = label;
	}
}
