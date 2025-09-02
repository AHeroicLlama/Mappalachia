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

		// Return the height as Topograph plots would see it - the Z coord, or the top edge of the volume
		public double HeightForTopograph => PrimitiveShape is not null ? Coord.Z + (((Shape)PrimitiveShape).BoundZ / 2) : Coord.Z;

		public Cluster? Cluster { get; set; } = null;

		public bool IsMemberOfCluster => Cluster is not null;

		public override bool Equals(object? obj)
		{
			if (obj is null)
			{
				return false;
			}

			if (obj is not Instance other)
			{
				return false;
			}

			return InstanceFormID == other.InstanceFormID;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(InstanceFormID);
		}
	}
}
