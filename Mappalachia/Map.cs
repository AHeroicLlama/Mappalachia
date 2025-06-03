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
		public static double MapMarkerIconScale { get; } = 1.5;

		static int MapMarkerLabelFontSize { get; } = 21;

		static int MapMarkerLabelTextMaxWidth { get; } = 150; // Max width of the label text, before it attempts to wrap

		static Brush MapMarkerLabelBrush { get; } = Brushes.White;

		static Brush DropShadowBrush { get; } = new SolidBrush(Color.FromArgb(128, 0, 0, 0));

		static int DropShadowOffset { get; } = 2;

		static StringFormat CenterString { get; } = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

		public static Image Draw(Settings settings)
		{
			// Load in the chosen base background image
			Image mapImage = new Bitmap(settings.Space.GetBackgroundImage(settings.MapSettings.BackgroundImage));
			using Graphics graphics = Graphics.FromImage(mapImage);

			// Apply the brightness and grayscale if selected
			mapImage.AdjustBrightnessOrGrayscale(settings.MapSettings.Brightness, settings.MapSettings.GrayscaleBackground);

			// Overlay the water mask
			if (settings.MapSettings.HighlightWater && settings.Space.IsWorldspace)
			{
				graphics.DrawImage(settings.Space.GetWaterMask(), 0, 0);
			}

			// Handle drawing map marker icons and/or labels
			if (settings.MapSettings.MapMarkerIcons || settings.MapSettings.MapMarkerLabels)
			{
				List<MapMarker> mapMarkers = Database.AllMapMarkers
					.Where(mapMarker => mapMarker.SpaceFormID == settings.Space.FormID)
					.OrderBy(mapMarker => mapMarker.Coord.Y).ToList();

				Font font = GetFont(MapMarkerLabelFontSize);

				foreach (MapMarker marker in mapMarkers)
				{
					PointF coord = marker.Coord.AsScaledPoint(settings.Space);
					int labelOffset = 0;

					if (settings.MapSettings.MapMarkerIcons)
					{
						Image image = marker.GetMapMarkerImage();
						graphics.DrawImageCentered(image, coord);

						labelOffset = image.Height / 2;
					}

					if (settings.MapSettings.MapMarkerLabels)
					{
						graphics.DrawStringCentered(marker.Label, font, MapMarkerLabelBrush, new PointF(coord.X, coord.Y + labelOffset), true, !settings.MapSettings.MapMarkerIcons, MapMarkerLabelTextMaxWidth);
					}
				}
			}

			GC.Collect();

			return mapImage;
		}

		// Draw the image at the given coordinates, centered on the coord
		static void DrawImageCentered(this Graphics graphics, Image image, PointF coord)
		{
			graphics.DrawImage(image, coord.X - (image.Width / 2), coord.Y - (image.Height / 2));
		}

		// Draw the string at the given coordinates, centered on the coord. Optional drop shadow, text wrap, and vertically centered or top-aligned to the coord
		static void DrawStringCentered(this Graphics graphics, string text, Font font, Brush brush, PointF coord, bool dropShadow = false, bool centerVert = true, int wrapWidth = -1)
		{
			// Calculate a rectangle which holds the text, when wrapped.
			SizeF stringBounds = wrapWidth == -1 ? graphics.MeasureString(text, font) : graphics.MeasureString(text, font, new SizeF(wrapWidth, MapImageResolution));

			RectangleF textBounds = new RectangleF(
				coord.X - (stringBounds.Width / 2),
				coord.Y - (centerVert ? (stringBounds.Height / 2) : 0),
				stringBounds.Width,
				stringBounds.Height);

			// Drop the shadow text first
			if (dropShadow)
			{
				RectangleF textBoundsShadow = new RectangleF(
					textBounds.X + DropShadowOffset,
					textBounds.Y + DropShadowOffset,
					textBounds.Width,
					textBounds.Height);

				graphics.DrawString(text, font, DropShadowBrush, textBoundsShadow, CenterString);
			}

			graphics.DrawString(text, font, brush, textBounds, CenterString);
		}

		// Returns the X/Y of a Coord, scaled from world to image coordinates (given the scaling of the space), as a PointF
		static PointF AsScaledPoint(this Coord coord, Space space)
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
