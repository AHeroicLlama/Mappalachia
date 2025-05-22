namespace Mappalachia
{
	public class MapSettings(Settings rootSettings)
	{
		Settings RootSettings { get; } = rootSettings;

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
				RootSettings.ResolveConflictingSettings();
			}
		}

		public float Brightness { get; set; } = 1.0f;

		public bool GrayscaleBackground { get; set; } = false;

		public bool HighlightWater { get; set; } = false;

		public bool MapMarkerIcons { get; set; } = false;

		public bool MapMarkerLabels { get; set; } = false;

		public LegendStyle LegendStyle { get; set; } = LegendStyle.Normal;
	}
}
