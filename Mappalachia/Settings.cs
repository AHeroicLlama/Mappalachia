using Library;

namespace Mappalachia
{
	static class Settings
	{
		public static Space CurrentSpace { get; set; } = Database.CachedSpaces.First();

		public static bool Grayscale { get; set; } = false;

		public static bool HighlightWater { get; set; } = false;

		public static bool MapMarkerIcons { get; set; } = false;

		public static bool MapMarkerLabels { get; set; } = false;

		public static BackgroundImageType BackgroundImage { get; set; } = BackgroundImageType.Menu;

		public static LegendStyle LegendStyle { get; set; } = LegendStyle.Normal;
	}
}
