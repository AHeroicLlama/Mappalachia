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
		public static Image Draw(List<Instance> instances, Settings settings, RectangleF? superResCrop = null)
		{
			// Gather the base background image
			Image mapImage = superResCrop != null ?
				GetSuperResBackground(settings, (RectangleF)superResCrop) :
				new Bitmap(settings.Space.GetBackgroundImage(settings.MapSettings.BackgroundImage));

			using Graphics graphics = Graphics.FromImage(mapImage);
			graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

			// Apply the brightness and grayscale if selected
			mapImage.AdjustBrightnessOrGrayscale(settings.MapSettings.Brightness, settings.MapSettings.GrayscaleBackground);

			// Overlay the water mask
			if (settings.MapSettings.HighlightWater && settings.Space.IsWorldspace)
			{
				graphics.DrawImage(settings.Space.GetWaterMask(), 0, 0);
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
		static Bitmap GetSuperResBackground(Settings settings, RectangleF superResCrop)
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
#pragma warning restore IDE0060 // Remove unused parameter

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
