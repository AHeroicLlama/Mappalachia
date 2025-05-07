using Library;

namespace Mappalachia
{
	public enum BackgroundImageType
	{
		Render,
		Menu,
		Military,
		None,
	}

	public enum LegendStyle
	{
		Normal,
		Extended,
		None,
	}

	public static class Map
	{
		public static async Task<Image> Draw(MapSettings settings)
		{
			// Filter for preferred bg images which are not applicable to the selected Space
			if (settings.BackgroundImage == BackgroundImageType.Military && !settings.Space.IsAppalachia())
			{
				settings.BackgroundImage = BackgroundImageType.Menu;
			}

			if (settings.BackgroundImage == BackgroundImageType.Menu && !settings.Space.IsWorldspace)
			{
				settings.BackgroundImage = BackgroundImageType.Render;
			}

			// Load in the chosen base background image
			Image mapImage = new Bitmap(settings.Space.GetBackgroundImage(settings.BackgroundImage));

			// Apply the brightness and grayscale if selected
			mapImage.AdjustBrightnessOrGrayscale(settings.Brightness, settings.Grayscale);

			// Overlay the water mask
			if (settings.HighlightWater && settings.Space.IsWorldspace)
			{
				using Graphics graphics = Graphics.FromImage(mapImage);
				graphics.DrawImage(settings.Space.GetWaterMask(), 0, 0);
			}

			GC.Collect();

			return mapImage;
		}
	}
}
