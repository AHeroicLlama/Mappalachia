using System.Text.Json.Serialization;
using Library;

namespace Mappalachia
{
	public class MapSettings(Settings rootSettings)
	{
		public FontSettings FontSettings { get; set; } = new FontSettings();

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

		float brightness = 0.5f;

		public float Brightness
		{
			get => brightness;

			set
			{
				brightness = Math.Clamp(value, 0.1f, 2.0f);
			}
		}

		Coord spotlightLocation = new Coord(0, 0);

		[JsonIgnore]
		public Coord SpotlightLocation
		{
			get => spotlightLocation;
			set
			{
				spotlightLocation = value;
				FileIO.FlushSpotlightTileImageCache();
			}
		}

		bool spotlightEnabled = false;

		public bool SpotlightEnabled
		{
			get => spotlightEnabled;
			set
			{
				// We assume for the majority of cases, turning on spotlight implies wanting the high res render too
				// But if it's already on, we leave as-is
				if (value && !SpotlightEnabled)
				{
					backgroundImage = BackgroundImageType.Render;
				}

				// If we try to turn it on, but it's not installed, prompt on installation
				if (value && !FileIO.IsSpotlightInstalled())
				{
					Notify.SpotlightInstallationPrompt();
					return;
				}

				spotlightEnabled = value;

				if (!SpotlightEnabled)
				{
					FileIO.FlushSpotlightTileImageCache();
				}
			}
		}

		// The size of the spotlight in tiles
		public double SpotlightSize { get; set; } = 2;

		public bool GrayscaleBackground { get; set; } = false;

		public bool HighlightWater { get; set; } = false;

		public bool MapMarkerIcons { get; set; } = false;

		public bool MapMarkerLabels { get; set; } = false;

		public LegendStyle LegendStyle { get; set; } = LegendStyle.Normal;

		public string Title { get; set; } = string.Empty;

		public CompassStyle CompassStyle { get; set; } = CompassStyle.WhenUseful;

		[JsonIgnore]
		public Settings RootSettings { get; set; } = rootSettings;

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
			// May have not finihsed initializing yet
			if (RootSettings == null)
			{
				return;
			}

			int maxSizeThisSpace = (int)Math.Min(RootSettings.Space.GetMaxSpotlightBenefit(), Common.SpotlightMaxSize);

			if (maxSizeThisSpace <= 0)
			{
				SpotlightSize = 1;
				return;
			}

			SpotlightSize = Math.Clamp(SpotlightSize, 1, maxSizeThisSpace);
		}
	}
}
