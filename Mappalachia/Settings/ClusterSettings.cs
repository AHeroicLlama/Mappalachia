namespace Mappalachia
{
	public class ClusterSettings
	{
		// Range in game units
		public int Range { get; set; } = 10000;

		public int MinWeight { get; set; } = 3;

		public bool LiveUpdate { get; set; } = true;
	}
}
