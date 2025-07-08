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

		public void SetSpotlightToMapCenter()
		{
			if (RootSettings == null)
			{
				return;
			}

			SpotlightLocation = new Coord(RootSettings.Space.CenterX, RootSettings.Space.CenterY);
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

		// The size of the spotlight in tiles
		public int SpotlightSize { get; set; } = 2;

		[JsonIgnore]
		public Coord SpotlightLocation { get; set; } = new Coord(0, 0);

		[JsonIgnore]
		public bool SpotlightEnabled { get; set; } = false;
	}
}
