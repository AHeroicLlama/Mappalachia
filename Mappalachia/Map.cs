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

		static double MapMarkerLabelTextOffset { get; } = 32; // Offset in Y Px where the label text is drawn from the mapmarker, if icons are on too

		static int MapMarkerLabelFontSize { get; } = 21;

		static int MapMarkerLabelTextMaxWidth { get; } = 150; // Max width of the label text, before it attempts to wrap

		static Brush MapMarkerLabelBrush { get; } = Brushes.White;

		static Brush DropShadowBrush { get; } = new SolidBrush(Color.FromArgb(128, 0, 0, 0));

		static int DropShadowOffset { get; } = 5;

		static StringFormat CenterString { get; } = new StringFormat() { Alignment = StringAlignment.Center };

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
				Font font = GetFont(MapMarkerLabelFontSize);

				foreach (MapMarker marker in Database.AllMapMarkers.OrderBy(m => m.Coord.Y))
				{
					PointF coord = marker.Coord.AsScaledPoint(settings.Space);

					if (settings.MapMarkerIcons)
					{
						graphics.DrawImageCentered(marker.GetMapMarkerImage(), coord);
					}

					if (settings.MapMarkerLabels)
					{
						graphics.DrawStringCentered(marker.Label, font, MapMarkerLabelBrush, new PointF(coord.X, coord.Y + labelOffset), true, MapMarkerLabelTextMaxWidth);
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

		// Draw the string at the given coordinates, centered on the coord. Optional drop shadow and text wrap
		static void DrawStringCentered(this Graphics graphics, string text, Font font, Brush brush, PointF coord, bool dropShadow = false, int wrapWidth = -1)
		{
			// Calculate a rectangle which holds the text, when wrapped.
			SizeF stringBounds = wrapWidth == -1 ? graphics.MeasureString(text, font) : graphics.MeasureString(text, font, new SizeF(wrapWidth, MapImageResolution));
			RectangleF textBounds = new RectangleF(
				coord.X - (stringBounds.Width / 2),
				coord.Y - (stringBounds.Height / 2),
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
