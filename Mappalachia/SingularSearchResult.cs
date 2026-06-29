using Library;

namespace Mappalachia
{
	// Represents a GroupedSearchResult which is actually just 1 unique instance, due to being the result of an instanceFormID search.
	public class SingularSearchResult(uint instanceFormID, Entity entity, Space space, string label, LockLevel lockLevel)
		: GroupedSearchResult(entity, space, 1, 1, label, lockLevel)
	{
		public uint InstanceFormID { get; } = instanceFormID;

		public override string DataValueFormID => $"{Entity.GetFriendlyFormID()} (Instance {InstanceFormID.ToHex()})";

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

			if (obj is not SingularSearchResult other)
			{
				return false;
			}

			return InstanceFormID.Equals(other.InstanceFormID);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(InstanceFormID);
		}
	}
}
