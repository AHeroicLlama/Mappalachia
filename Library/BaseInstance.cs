namespace Library
{
	public enum LockLevel
	{
		// Order is used to define percieved relevance and thus sort in the UI
		None,
		Level0,
		Level1,
		Level2,
		Level3,
		RequiresTerminal,
		RequiresKey,
		Chained,
		Barred,
		Inaccessible,
		Unknown,
	}

	// Abstract of the shared properties of Instance or GroupedInstance
	public abstract class BaseInstance(Entity entity, Space space, string label, LockLevel lockLevel, double spawnWeight, bool inContainer = false)
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
		// EG 0.5 for a 50% chance NPC, 2 for junk containing 2 scrap, 1 for a statically placed object
		public virtual double SpawnWeight { get; } = spawnWeight;

		// If this instance actually exists within a container
		public virtual bool InContainer { get; } = inContainer;

		public override bool Equals(object? obj)
		{
			if (obj is null)
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (obj.GetType() != GetType())
			{
				return false;
			}

			if (obj is not BaseInstance other)
			{
				return false;
			}

			return Entity.Equals(other.Entity) &&
				Space.Equals(other.Space) &&
				Label.Equals(other.Label) &&
				LockLevel.Equals(other.LockLevel) &&
				SpawnWeight.Equals(other.SpawnWeight) &&
				InContainer.Equals(other.InContainer);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Entity, Space, Label, LockLevel, InContainer);
		}
	}
}
