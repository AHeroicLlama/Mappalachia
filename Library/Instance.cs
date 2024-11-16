namespace Library
{
	// TODO - Flesh this out as needed - substitutes MapDataPoint
	public class Instance
	{
		public enum Shape
		{
			None,
			Box,
			Sphere,
			Line,
			Plane,
			Ellipsoid,
		}

		public enum Lock
		{
			None,
			Level0,
			Level1,
			Level2,
			Level3,
			RequiresKey,
			Inaccessible,
			RequiresTerminal,
			Chained,
			Unknown,
			Barred,
		}

		// The Entity which this is an instance of
		public Entity? Entity { get; } = null;

		// The X coordinate
		public float X { get; }

		// The Y coordinate
		public float Y { get; }

		// The Z coordinate
		public float Z { get; }

		// The Rotation of the instance in degrees
		public float Rotation { get; }

		// The Instance Form ID of this entity, in base-10. This should be unique
		public uint InstanceFormID { get; }

		// The Space which this exists in
		public Space? Space { get; }

		// The text 'label' of this instance
		public string Label { get; } = string.Empty;

		// (Where applicable, eg a loading trigger door) The Space which this entity teleports to
		public Space? TeleportsTo { get; }

		// The Level of the lock on this instance
		public Lock LockLevel { get; }

		// The Shape of the primitive
		public Shape PrimitiveShape { get; }

		public Instance(Entity entity, float x, float y, float z, float rotation, uint instanceFormID, Space space, string label, Space teleportsTo, Lock lockLevel, Shape primitiveShape)
		{
			Entity = entity;
			X = x;
			Y = y;
			Z = z;
			Rotation = rotation;
			InstanceFormID = instanceFormID;
			Space = space;
			Label = label;
			TeleportsTo = teleportsTo;
			LockLevel = lockLevel;
			PrimitiveShape = primitiveShape;
		}

		public Instance(float x, float y)
		{
			X = x;
			Y = y;
		}
	}
}
