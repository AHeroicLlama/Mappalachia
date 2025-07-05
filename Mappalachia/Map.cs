using System.Drawing.Drawing2D;
using System.Drawing.Text;
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
		public static double IconScale { get; } = 1.5;

		static int MapMarkerLabelTextMaxWidth { get; } = 150; // Max width of the label text, before it attempts to wrap

		static int DropShadowOffset { get; } = 2;

		static int FontSizeMapMarkerLabel { get; } = 20;

		static int FontSizeWaterMark { get; } = 60;

		static int FontSizeTitle { get; } = 72;

		static Brush BrushGeneric { get; } = Brushes.White;

		static Brush BrushDropShadow { get; } = new SolidBrush(Color.FromArgb(128, 0, 0, 0));

		static StringFormat Center { get; } = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

		static StringFormat TopRight { get; } = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near };

		static StringFormat BottomRight { get; } = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far };

		// The primary map draw function
		public static Image Draw(List<Instance> instances, Settings settings)
		{
			// Gather the base background image
			Image mapImage = new Bitmap(FileIO.EmptyMapImage);
			using Graphics graphics = Graphics.FromImage(mapImage);
			graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;

			RectangleF backgroundRectangle = GetScaledMapBackgroundRect(settings);

			graphics.DrawImage(settings.Space.GetBackgroundImage(settings.MapSettings.BackgroundImage), backgroundRectangle);

			// TODO debug spotlight
			if ((settings.Space.IsWorldspace || SpotlightInCells) && settings.MapSettings.SpotlightEnabled)
			{
				List<SpotlightTile> spotlightTiles = SpotlightTile.GetTilesInRect(new RectangleF(0, 0, MapImageResolution, MapImageResolution).AsWorldRectangle(settings), settings.Space);

				foreach (SpotlightTile tile in spotlightTiles)
				{
					graphics.DrawImage(tile.GetImage(), tile.GetRectangle().AsImageRectangle(settings));
				}
			}

			// Apply the brightness and grayscale if selected
			mapImage.AdjustBrightnessOrGrayscale(settings.MapSettings.Brightness, settings.MapSettings.GrayscaleBackground);

			// Overlay the water mask
			if (settings.MapSettings.HighlightWater && settings.Space.IsWorldspace)
			{
				graphics.DrawImage(settings.Space.GetWaterMask(), backgroundRectangle);
			}

			// Call the relevant plot function
			switch (settings.PlotSettings.Mode)
			{
				case PlotMode.Standard:
					DrawStandardPlots(instances, settings, graphics);
					break;

				case PlotMode.Topographic:
					DrawStandardPlots(instances, settings, graphics, true);
					DrawTopographicLegend(settings, graphics);
					break;

				case PlotMode.Cluster:
					DrawClusterPlots(instances, settings, graphics);
					break;

				default:
					throw new Exception("Unexpected PlotMode: " + settings.PlotSettings.Mode);
			}

			if (settings.PlotSettings.ShowPlotsInOtherSpaces)
			{
				DrawConnectingSpacePlots(instances, settings, graphics);
			}

			if (settings.PlotSettings.DrawInstanceFormID)
			{
				DrawInstanceFormIDs(instances, settings, graphics);
			}

			DrawWaterMark(settings, graphics);
			DrawTitle(settings, graphics);
			DrawMapMarkerIconsAndLabels(settings, graphics);
			mapImage = DrawLegend(instances, settings, mapImage);

			GC.Collect();

			return mapImage;
		}

		// TODO temp disable warning
#pragma warning disable IDE0060 // Remove unused parameter
		static Bitmap GetSpotlightBackground(Settings settings, RectangleF spotlightCrop)
		{
			// TODO
			return new Bitmap(MapImageResolution, MapImageResolution);
		}

		// Standard including topographic plots
		static void DrawStandardPlots(List<Instance> instances, Settings settings, Graphics graphics, bool topographic = false)
		{
			// TODO
		}

		static void DrawClusterPlots(List<Instance> instances, Settings settings, Graphics graphics)
		{
			// TODO
		}

		// Draw plots where plotted entities exist in a space reachable by this one
		static void DrawConnectingSpacePlots(List<Instance> instances, Settings settings, Graphics graphics)
		{
			// TODO
		}

		// Draw the FormID of the instance of the plot
		static void DrawInstanceFormIDs(List<Instance> instances, Settings settings, Graphics graphics)
		{
			// TODO
		}

		// Draw the color scale demonstrating the topographic color/height mapping
		static void DrawTopographicLegend(Settings settings, Graphics graphics)
		{
			// TODO
		}

		// Draws the region or shape belonging to the given instance
		static void DrawRegionOrShape(Settings settings, Graphics graphics, Instance instance)
		{
			// TODO
		}
#pragma warning restore IDE0060 // Remove unused parameter

		static void DrawTitle(Settings settings, Graphics graphics)
		{
			string titleText = settings.MapSettings.Title.Trim();

			if (titleText.IsNullOrWhiteSpace())
			{
				return;
			}

			Font font = GetFont(FontSizeTitle);
			SizeF stringBounds = graphics.MeasureString(titleText, font, new SizeF(MapImageResolution, MapImageResolution));

			RectangleF textBounds = new RectangleF(
				MapImageResolution - stringBounds.Width,
				0,
				stringBounds.Width,
				stringBounds.Height);

			DrawStringWithDropShadow(graphics, titleText, font, BrushGeneric, textBounds, TopRight);
		}

		static async void DrawWaterMark(Settings settings, Graphics graphics)
		{
			string text = settings.Space.IsAppalachia() ? string.Empty : $"{settings.Space.DisplayName} ({settings.Space.EditorID})\n";
			text += $"Game Version {await Database.GetGameVersion()} | Made with Mappalachia: github.com/AHeroicLlama/Mappalachia";

			Font font = GetFont(FontSizeWaterMark);
			RectangleF textBounds = new RectangleF(0, 0, MapImageResolution, MapImageResolution);

			DrawStringWithDropShadow(graphics, text, font, BrushGeneric, textBounds, BottomRight);
		}

		static void DrawMapMarkerIconsAndLabels(Settings settings, Graphics graphics)
		{
			if (!settings.MapSettings.MapMarkerIcons && !settings.MapSettings.MapMarkerLabels)
			{
				return;
			}

			List<MapMarker> mapMarkers = Database.AllMapMarkers
				.Where(mapMarker => mapMarker.SpaceFormID == settings.Space.FormID)
				.OrderBy(mapMarker => mapMarker.Coord.Y).ToList();

			Font font = GetFont(FontSizeMapMarkerLabel);

			foreach (MapMarker marker in mapMarkers)
			{
				PointF coord = marker.Coord.AsImagePoint(settings);
				int labelOffset = 0;

				if (settings.MapSettings.MapMarkerIcons)
				{
					Image image = marker.GetMapMarkerImage();
					graphics.DrawImageCentered(image, coord);

					labelOffset = image.Height / 2;
				}

				if (settings.MapSettings.MapMarkerLabels)
				{
					graphics.DrawStringCentered(marker.Label, font, BrushGeneric, new PointF(coord.X, coord.Y + labelOffset), !settings.MapSettings.MapMarkerIcons, MapMarkerLabelTextMaxWidth);
				}
			}
		}

		// Draw the legend
		// May return an image of different dimensions if extended legend is used
		static Image DrawLegend(List<Instance> instances, Settings settings, Image image)
		{
			// TODO
			switch (settings.MapSettings.LegendStyle)
			{
				case LegendStyle.Normal:
					break;
				case LegendStyle.Extended:
					break;
				case LegendStyle.None:
					break;
			}

			return image;
		}

		// Draw the image at the given coordinates, centered on the coord
		static void DrawImageCentered(this Graphics graphics, Image image, PointF coord)
		{
			graphics.DrawImage(image, coord.X - (image.Width / 2), coord.Y - (image.Height / 2));
		}

		// Draw the string at the given coordinates, centered on the coord. Optional drop shadow, text wrap, and vertically centered or top-aligned to the coord
		static void DrawStringCentered(this Graphics graphics, string text, Font font, Brush brush, PointF coord, bool centerVert = true, int wrapWidth = -1)
		{
			// Calculate a rectangle which holds the text, when wrapped.
			SizeF stringBounds = wrapWidth == -1 ? graphics.MeasureString(text, font) : graphics.MeasureString(text, font, new SizeF(wrapWidth, MapImageResolution));

			RectangleF textBounds = new RectangleF(
				coord.X - (stringBounds.Width / 2),
				coord.Y - (centerVert ? (stringBounds.Height / 2) : 0),
				stringBounds.Width,
				stringBounds.Height);

			DrawStringWithDropShadow(graphics, text, font, brush, textBounds, Center);
		}

		// Draw the string with drop-shadow
		static void DrawStringWithDropShadow(this Graphics graphics, string text, Font font, Brush brush, RectangleF textBounds, StringFormat stringFormat)
		{
			RectangleF textBoundsShadow = new RectangleF(
				textBounds.X + DropShadowOffset,
				textBounds.Y + DropShadowOffset,
				textBounds.Width,
				textBounds.Height);

			graphics.DrawString(text, font, BrushDropShadow, textBoundsShadow, stringFormat);
			graphics.DrawString(text, font, brush, textBounds, stringFormat);
		}

		// Returns the X/Y of a Coord, scaled from world to image coordinates (given the scaling of the space), as a PointF
		// Inverse of AsWorldCoord
		public static PointF AsImagePoint(this Coord coord, Settings settings, bool ignoreSpotlight = false)
		{
			if (settings.MapSettings.SpotlightEnabled && !ignoreSpotlight)
			{
				double factor = settings.Space.Radius / (TileWidth * settings.MapSettings.SpotlightTileRange / 2);

				coord = new Coord(
					(coord.X - settings.MapSettings.SpotlightLocation.X) * factor,
					(coord.Y - settings.MapSettings.SpotlightLocation.Y) * factor);
			}

			float halfRes = MapImageResolution / 2f;

			PointF point = new PointF(
				(float)(halfRes * (1 + ((coord.X - settings.Space.CenterX) / settings.Space.Radius))),
				(float)(halfRes * (1 + (((coord.Y * -1) - settings.Space.CenterY) / settings.Space.Radius))));

			return point;
		}

		// Returns the game world coordinate of a point on the map image
		// Inverse of AsImagePoint
		public static Coord AsWorldCoord(this PointF point, Settings settings, bool ignoreSpotlight = false)
		{
			double halfRes = MapImageResolution / 2d;

			Coord coord = new Coord(
				(((point.X / halfRes) - 1) * settings.Space.Radius) + settings.Space.CenterX,
				-1 * ((((point.Y / halfRes) - 1) * settings.Space.Radius) + settings.Space.CenterY));

			if (!settings.MapSettings.SpotlightEnabled || ignoreSpotlight)
			{
				return coord;
			}

			double factor = settings.Space.Radius / (TileWidth * settings.MapSettings.SpotlightTileRange);

			return new Coord(
				(coord.X / factor) + settings.MapSettings.SpotlightLocation.X,
				(coord.Y / factor) + settings.MapSettings.SpotlightLocation.Y);
		}

		// Returns a RectangleF in image coordinates which represents the full size of the core map background images, with spotlight scaling considered
		// Can be used to apply the background images (and water mask) with respect to spotlight settings
		public static RectangleF GetScaledMapBackgroundRect(Settings settings)
		{
			if (!settings.MapSettings.SpotlightEnabled)
			{
				return new RectangleF(0, 0, MapImageResolution, MapImageResolution);
			}

			PointF topLeft = new PointF(0, 0).AsWorldCoord(settings, true).AsImagePoint(settings);
			PointF bottomRight = new PointF(MapImageResolution, MapImageResolution).AsWorldCoord(settings, true).AsImagePoint(settings);

			return new RectangleF(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
		}

		public static RectangleF AsWorldRectangle(this RectangleF rect, Settings settings)
		{
			Coord topLeft = new PointF(rect.Left, rect.Top).AsWorldCoord(settings);
			Coord bottomRight = new PointF(rect.Right, rect.Bottom).AsWorldCoord(settings);

			return new RectangleF(
				(float)topLeft.X,
				(float)topLeft.Y,
				(float)(bottomRight.X - topLeft.X),
				(float)(bottomRight.Y - topLeft.Y));
		}

		// Returns the rectangle in image coordinates, given a rectangle in world coordinates
		static RectangleF AsImageRectangle(this RectangleF rect, Settings settings)
		{
			Coord topLeftWorld = new Coord(rect.Left, rect.Top);
			Coord bottomRightWorld = new Coord(rect.Right, rect.Bottom);

			PointF topLeft = topLeftWorld.AsImagePoint(settings);
			PointF bottomRight = bottomRightWorld.AsImagePoint(settings);

			return new RectangleF(topLeft.X, topLeft.Y, Math.Abs(bottomRight.X - topLeft.X), Math.Abs(bottomRight.Y - topLeft.Y));
		}

		// Returns the application font in the given pixel size
		static Font GetFont(int size)
		{
			return new Font(FileIO.GetFontFamily(), size, GraphicsUnit.Pixel);
		}
	}
}
