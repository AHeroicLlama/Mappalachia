namespace Library
{
	// Defines a collection of Instances which form a cluster
	public class Cluster
	{
		public Cluster(Instance origin)
		{
			Origin = origin;
			AddMember(Origin);
		}

		public List<Instance> Members { get; } = new List<Instance>();

		public Instance Origin { get; }

		public void AddMember(Instance instance)
		{
			if (Members.Contains(instance) || instance.Cluster != null)
			{
				throw new Exception("Instance is already a member of a cluster");
			}

			Members.Add(instance);
			instance.Cluster = this;
		}

		public void RemoveMember(Instance instance)
		{
			if (!Members.Contains(instance))
			{
				throw new Exception("Tried to remove an instance, but was not a member");
			}

			Members.Remove(instance);
			instance.Cluster = null;
		}

		public bool ContainsInstance(Instance instance)
		{
			return Members.Contains(instance);
		}

		public double GetWeight()
		{
			return Members.Sum(instance => instance.SpawnWeight);
		}

		public Coord GetCentroid()
		{
			return new Coord(Members.Average(instance => instance.Coord.X), Members.Average(instance => instance.Coord.Y));
		}
	}
}
