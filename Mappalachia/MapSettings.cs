using System.Text.Json.Serialization;
using Library;

namespace Mappalachia
{
	public class MapSettings(Settings rootSettings)
	{
		BackgroundImageType backgroundImage = BackgroundImageType.Menu;

		public BackgroundImageType BackgroundImage
		{
			get => backgroundImage;

			set
			{
				if (backgroundImage == value)
				{
					return;
				}

				backgroundImage = value;
				RootSettings?.ResolveConflictingSettings();
			}
		}

		[JsonIgnore]
		public Settings RootSettings { get; set; } = rootSettings;

		public float Brightness { get; set; } = 1.0f;

		public bool GrayscaleBackground { get; set; } = false;

		public bool HighlightWater { get; set; } = false;

		public bool MapMarkerIcons { get; set; } = false;

		public bool MapMarkerLabels { get; set; } = false;

		public LegendStyle LegendStyle { get; set; } = LegendStyle.Normal;

		public string Title { get; set; } = string.Empty;

		public int SpotlightTileRange { get; set; } = 3;

		[JsonIgnore]
		public Coord SpotlightLocation { get; set; } = new Coord(8192, 8192);

		[JsonIgnore]
		public bool SpotlightEnabled { get; set; } = false;
	}
}
