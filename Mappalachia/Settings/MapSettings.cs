using System.Text.Json.Serialization;
using Library;

namespace Mappalachia
{
	public class MapSettings(Settings rootSettings)
	{
		public FontSettings FontSettings { get; set; } = new FontSettings();

		BackgroundImageType backgroundImage = BackgroundImageType.Menu;

		// A record of the background image before spotlight was enabled
		BackgroundImageType backgroundImagePreSpotlight = BackgroundImageType.Menu;

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
				backgroundImagePreSpotlight = value;
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
				// If we try to turn it on, but it's not installed, prompt on installation
				if (value && !FileIO.IsSpotlightInstalled())
				{
					Notify.SpotlightInstallationPrompt();
				}

				// Store the background image setting before spotlight is turned on
				if (value && !SpotlightEnabled)
				{
					backgroundImagePreSpotlight = backgroundImage;
				}

				// Restore the background image setting once spotlight is turned off
				if (!value && SpotlightEnabled)
				{
					backgroundImage = backgroundImagePreSpotlight;
				}

				// We assume for the majority of cases, turning on spotlight implies wanting the high res render too
				// But if it's already on or not installed, we leave as-is
				if (value && !SpotlightEnabled && FileIO.IsSpotlightInstalled())
				{
					backgroundImage = BackgroundImageType.Render;
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

		public bool MapMarkerGrayscale { get; set; } = false; // Not accessible via UI

		public LegendStyle LegendStyle { get; set; } = LegendStyle.Extended;

		public string Title { get; set; } = string.Empty;

		public CompassStyle CompassStyle { get; set; } = CompassStyle.WhenUseful;

		[JsonIgnore]
		public Settings RootSettings { get; set; } = rootSettings;

		public void SetSpotlightToMapCenter()
		{
			if (RootSettings is null)
			{
				return;
			}

			SpotlightLocation = RootSettings.Space.GetCenter();
		}

		public void CapSpotlightSizeToSpace()
		{
			// May have not finished initializing yet
			if (RootSettings is null)
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
