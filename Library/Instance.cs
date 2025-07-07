namespace Library
{
	public class Instance(Entity entity, Space space, Coord coord, uint instanceFormID, string label, Space? teleportsTo, LockLevel lockLevel, Shape? primitiveShape, double spawnWeight = 1, bool inContainer = false)
		: BaseInstance(entity, space, label, lockLevel, spawnWeight, inContainer)
	{
		// The coordinates of this instance
		public Coord Coord { get; } = coord;

		// The Instance Form ID of this entity, in base-10. This should be unique
		public uint InstanceFormID { get; } = instanceFormID;

		// (Where applicable, eg a loading trigger door) The Space which this entity teleports to
		public Space? TeleportsTo { get; } = teleportsTo;

		// The Primitive Shape, if exists
		public Shape? PrimitiveShape { get; } = primitiveShape;
	}
}
