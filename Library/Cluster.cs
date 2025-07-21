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
			Members.Add(instance);
			instance.Cluster = this;
		}

		public void RemoveMember(Instance instance)
		{
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
	}
}
