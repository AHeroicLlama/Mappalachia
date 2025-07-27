using System.Text.Json.Serialization;

namespace Mappalachia
{
	public class ClusterSettings
	{
		// Range in game units
		public int Range { get; set; } = 10000;

		public int MinWeight { get; set; } = 3;

		public bool LiveUpdate { get; set; } = true;

		public bool ClusterPerLegendGroup { get; set; } = false;

		[JsonIgnore]
		public int MaxRange { get; } = 50000;
	}
}
