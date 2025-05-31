namespace Library
{
	// Abstract of the shared properties of Instance or GroupedInstance
	public abstract class BaseInstance(Entity entity, Space space, string label, LockLevel lockLevel, double spawnWeight)
	{
		// The Entity which this is an instance of
		public virtual Entity Entity { get; } = entity;

		// The Space which this exists in
		public virtual Space Space { get; } = space;

		// The Level of the lock on this instance
		public virtual LockLevel LockLevel { get; } = lockLevel;

		// The text 'label' of this instance
		public virtual string Label { get; } = label;

		// The expected quantity of the spawn
		// EG 0.5 for a 50% chance NPC, 2 for junk containing 2 scrap, 1 (Except rarely when Entity.percChanceNone has a value) for a statically placed object
		public virtual double SpawnWeight { get; } = spawnWeight;

		public override bool Equals(object? obj)
		{
			if (obj is not BaseInstance other || obj is null)
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			return Entity.Equals(other.Entity) &&
				Space.Equals(other.Space) &&
				Label.Equals(other.Label) &&
				LockLevel.Equals(other.LockLevel);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Entity, Space, Label, LockLevel);
		}
	}
}
