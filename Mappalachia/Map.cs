using Library;
using static Library.Common;

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
		public static double MapMarkerIconScale { get; } = 2;

		static double MapMarkerLabelTextOffset { get; } = 50; // Offset in Y Px where the label text is drawn from the mapmarker, if icons are on too

		static int MapMarkerLabelSize { get; } = 36;

		static Brush MapMarkerLabelBrush { get; } = Brushes.White;

		public static Image Draw(MapSettings settings)
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
			using Graphics graphics = Graphics.FromImage(mapImage);

			// Apply the brightness and grayscale if selected
			mapImage.AdjustBrightnessOrGrayscale(settings.Brightness, settings.GrayscaleBackground);

			// Overlay the water mask
			if (settings.HighlightWater && settings.Space.IsWorldspace)
			{
				graphics.DrawImage(settings.Space.GetWaterMask(), 0, 0);
			}

			// Handle drawing map marker icons and/or labels
			if (settings.MapMarkerIcons || settings.MapMarkerLabels)
			{
				float labelOffset = (float)(settings.MapMarkerIcons ? MapMarkerLabelTextOffset : 0);
				Font font = GetFont(MapMarkerLabelSize);

				foreach (MapMarker marker in Database.AllMapMarkers)
				{
					PointF coord = marker.Coord.AsScaledPoint(settings.Space);

					if (settings.MapMarkerIcons)
					{
						graphics.DrawImage(marker.GetMapMarkerImage(), coord);
					}

					if (settings.MapMarkerLabels)
					{
						graphics.DrawString(marker.Label, font, MapMarkerLabelBrush, new PointF(coord.X, coord.Y + labelOffset));
					}
				}
			}

			GC.Collect();

			return mapImage;
		}

		// Returns the X/Y of a Coord, scaled from world to image coordinates (given the scaling of the space), as a PointF
		public static PointF AsScaledPoint(this Coord coord, Space space)
		{
			return new PointF(
				(MapImageResolution / 2) * (float)(1 + ((coord.X - space.CenterX) / space.GetRadius())),
				(MapImageResolution / 2) * (float)(1 + (((coord.Y * -1) - space.CenterY) / space.GetRadius())));
		}

		// Returns the application font in the given pixel size
		static Font GetFont(int size)
		{
			return new Font(FileIO.GetFontFamily(), size, GraphicsUnit.Pixel);
		}
	}
}
