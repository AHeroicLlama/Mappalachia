namespace Library
{
	// Defines a collection of Instances which form a map cluster
	public class Cluster
	{
		public List<Instance> Members { get; } = new List<Instance>();

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
	}
}
