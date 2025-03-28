namespace Library
{
	// Abstract of the shared properties of Instance or GroupedInstance
	public abstract class BaseInstance(Entity entity, Space space, string label, LockLevel lockLevel, double spawnWeight)
	{
		// The Entity which this is an instance of
		public Entity Entity { get; } = entity;

		// The Space which this exists in
		public Space Space { get; } = space;

		// The Level of the lock on this instance
		public LockLevel LockLevel { get; } = lockLevel;

		// The text 'label' of this instance
		public string Label { get; } = label;

		// The expected quantity of the spawn
		// EG 0.5 for a 50% chance NPC, 2 for junk containing 2 scrap, 1 (Except rarely when Entity.percChanceNone has a value) for a statically placed object
		public double SpawnWeight { get; } = spawnWeight;
	}
}
