using Library;

namespace Mappalachia
{
	public enum LockLevel
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

	public class Instance(Entity entity, Space space, Coord point, uint instanceFormID, string label, Space? teleportsTo, LockLevel lockLevel, Shape? primitiveShape)
		: BaseInstance(entity, space, label, lockLevel)
	{
		// The coordinates of this instance
		public Coord Point { get; } = point;

		// The Instance Form ID of this entity, in base-10. This should be unique
		public uint InstanceFormID { get; } = instanceFormID;

		// (Where applicable, eg a loading trigger door) The Space which this entity teleports to
		public Space? TeleportsTo { get; } = teleportsTo;

		// The Primitive Shape
		public Shape? PrimitiveShape { get; } = primitiveShape;
	}
}
