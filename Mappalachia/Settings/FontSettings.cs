using System.Text.Json.Serialization;

namespace Mappalachia
{
	public class FontSettings
	{
		[JsonIgnore]
		public int MinSizePlotted { get; } = 16;

		[JsonIgnore]
		public int MaxSizePlotted { get; } = 100;

		[JsonIgnore]
		public int MinSizeStatic { get; } = 32;

		[JsonIgnore]
		public int MaxSizeStatic { get; } = 200;

		public int SizeTitle { get; set; } = 72;

		public int SizeLegend { get; set; } = 40;

		public int SizeItemsInOtherSpaces { get; set; } = 20;

		public int SizeMapMarkerLabel { get; set; } = 20;

		public int SizeWatermark { get; set; } = 60;

		public int SizeClusterLabel { get; set; } = 32;

		public int SizeInstanceFormID { get; set; } = 32;

		public int SizeRegionLevel { get; set; } = 32;
	}
}
