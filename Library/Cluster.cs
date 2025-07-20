namespace Library
{
	// Defines a collection of Instances which form a cluster
	public class Cluster
	{
		public Cluster(Instance origin)
		{
			AddMember(origin);
			Origin = origin;
		}

		public List<Instance> Members { get; } = new List<Instance>();

		public Instance Origin { get; }

		public void AddMember(Instance instance)
		{
			Members.Add(instance);
			instance.Cluster = this;
		}

		public void RemoveMember(Instance instance)
		{
			if (instance == Origin)
			{
				throw new InvalidOperationException("Cannot remove the origin point instance member of a cluster");
			}

			Members.Remove(instance);
			instance.Cluster = null;
		}

		public double GetWeight()
		{
			return Members.Sum(instance => instance.SpawnWeight);
		}
	}
}
