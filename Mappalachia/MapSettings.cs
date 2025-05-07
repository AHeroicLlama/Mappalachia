using Library;

namespace Mappalachia
{
	public class MapSettings()
	{
		public Space Space { get; set; } = Database.AllSpaces.First(); // TODO placeholder

		public float Brightness { get; set; } = 1.0f;

		public bool GrayscaleBackground { get; set; } = false;

		public bool HighlightWater { get; set; } = false;

		public bool MapMarkerIcons { get; set; } = false;

		public bool MapMarkerLabels { get; set; } = false;

		public BackgroundImageType BackgroundImage { get; set; } = BackgroundImageType.Menu;

		public LegendStyle LegendStyle { get; set; } = LegendStyle.Normal;
	}
}
