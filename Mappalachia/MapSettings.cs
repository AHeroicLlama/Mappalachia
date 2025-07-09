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

			SpotlightLocation = RootSettings.Space.GetCenter();
		}

		public void CapSpotlightSizeToSpace()
		{
			if (RootSettings == null)
			{
				return;
			}

			int maxSizeThisSpace = (int)Math.Min(RootSettings.Space.GetMaxSpotlightBenefit(), 8);
			SpotlightSize = Math.Clamp(SpotlightSize, 1, maxSizeThisSpace);
		}

		[JsonIgnore]
		public Settings RootSettings { get; set; } = rootSettings;

		float brightness = 1.0f;

		public float Brightness
		{
			get => brightness;

			set
			{
				brightness = Math.Clamp(value, 0.1f, 2.0f);
			}
		}

		public bool GrayscaleBackground { get; set; } = false;

		public bool HighlightWater { get; set; } = false;

		public bool MapMarkerIcons { get; set; } = false;

		public bool MapMarkerLabels { get; set; } = false;

		public LegendStyle LegendStyle { get; set; } = LegendStyle.Normal;

		public string Title { get; set; } = string.Empty;

		// The size of the spotlight in tiles
		public double SpotlightSize { get; set; } = 1;

		Coord spotlightLocation = new Coord(0, 0);

		[JsonIgnore]
		public Coord SpotlightLocation
		{
			get => spotlightLocation;
			set
			{
				spotlightLocation = value;
				FileIO.ClearSpotlightTileImageCache();
			}
		}

		bool spotlightEnabled = false;

		[JsonIgnore]
		public bool SpotlightEnabled
		{
			get => spotlightEnabled;
			set
			{
				spotlightEnabled = value;

				if (!SpotlightEnabled)
				{
					FileIO.ClearSpotlightTileImageCache();
				}
			}
		}
	}
}
