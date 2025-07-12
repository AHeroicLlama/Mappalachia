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

		static int FontSizeItemsInOtherSpaces { get; } = 20;

		static int FontSizeMapMarkerLabel { get; } = 20;

		static int FontSizeWaterMark { get; } = 60;

		static int FontSizeTitle { get; } = 72;

		static Brush BrushGeneric { get; } = Brushes.White;

		static Brush BrushGenericTransparent { get; } = new SolidBrush(Color.FromArgb(128, 255, 255, 255));

		static Brush BrushDropShadow { get; } = new SolidBrush(Color.FromArgb(128, 0, 0, 0));

		static StringFormat Center { get; } = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

		static StringFormat TopRight { get; } = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near };

		static StringFormat BottomRight { get; } = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far };

		// The primary map draw function
		public static Image Draw(List<GroupedSearchResult> itemsToPlot, Settings settings, Progress<ProgressInfo>? progressInfo = null)
		{
			UpdateProgress(progressInfo, 10, "Draw started");

			// Gather the base background image
			Image mapImage = new Bitmap(FileIO.EmptyMapImage);
			using Graphics graphics = Graphics.FromImage(mapImage);
			graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

			RectangleF backgroundRectangle = GetScaledMapBackgroundRect(settings);

			// Draw the main background image
			graphics.DrawImage(settings.Space.GetBackgroundImage(settings.MapSettings.BackgroundImage), backgroundRectangle);

			DrawSpotlightTiles(settings, graphics, progressInfo);

			// Apply the brightness and grayscale if selected
			mapImage.AdjustBrightnessOrGrayscale(settings.MapSettings.Brightness, settings.MapSettings.GrayscaleBackground);

			// Overlay the water mask
			if (settings.MapSettings.HighlightWater && settings.Space.IsWorldspace)
			{
				graphics.DrawImage(settings.Space.GetWaterMask(), backgroundRectangle);
			}

			DrawMapMarkerIconsAndLabels(settings, graphics, progressInfo);

			// Call the relevant plot function
			switch (settings.PlotSettings.Mode)
			{
				case PlotMode.Standard:
					DrawStandardPlots(itemsToPlot, settings, graphics, false, progressInfo);
					break;

				case PlotMode.Topographic:
					DrawStandardPlots(itemsToPlot, settings, graphics, true, progressInfo);
					DrawTopographicLegend(settings, graphics);
					break;

				case PlotMode.Cluster:
					DrawClusterPlots(itemsToPlot, settings, graphics, progressInfo);
					break;

				default:
					throw new Exception("Unexpected PlotMode: " + settings.PlotSettings.Mode);
			}

			DrawConnectingSpacePlots(itemsToPlot, settings, graphics, progressInfo);
			DrawInstanceFormIDs(itemsToPlot, settings, graphics);
			DrawWaterMark(settings, graphics);
			DrawTitle(settings, graphics);
			mapImage = DrawLegend(itemsToPlot, settings, mapImage);

			GC.Collect();

			UpdateProgress(progressInfo, 0, "Done");

			return mapImage;
		}

		static void UpdateProgress(IProgress<ProgressInfo>? progressInfo, int percent, string status)
		{
			if (progressInfo == null)
			{
				return;
			}

			progressInfo.Report(new ProgressInfo(percent, status));
		}

		// Overload to easily calculate percentages give x/y progress
		static void UpdateProgress(IProgress<ProgressInfo>? progressInfo, int current, int total, string status)
		{
			if (progressInfo == null)
			{
				return;
			}

			if (current == 0)
			{
				progressInfo.Report(new ProgressInfo(0, status));
			}


			progressInfo.Report(new ProgressInfo((int)Math.Round((current / (double)total) * 100), status));
		}

		static void DrawSpotlightTiles(Settings settings, Graphics graphics, Progress<ProgressInfo>? progressInfo = null)
		{
			if (!settings.MapSettings.SpotlightEnabled ||
				settings.MapSettings.BackgroundImage != BackgroundImageType.Render)
			{
				return;
			}

			List<SpotlightTile> spotlightTiles = SpotlightTile.GetTilesInRect(new RectangleF(0, 0, MapImageResolution, MapImageResolution).AsWorldRectangle(settings), settings.Space);

			// Pre-cache the tile images in parallel
			spotlightTiles.AsParallel().ForAll(tile =>
			{
				tile.GetSpotlightTileImage();
			});

			int i = 0;
			foreach (SpotlightTile tile in spotlightTiles)
			{
				Image? image = tile.GetSpotlightTileImage();

				if (image == null)
				{
					continue;
				}

				i++;
				int percent = (int)Math.Round(i / (double)spotlightTiles.Count * 100);
				UpdateProgress(progressInfo, percent, $"Drawing tile {i} of {spotlightTiles.Count}");

				graphics.DrawImage(image, tile.GetRectangle().AsImageRectangle(settings));
			}
		}

		// TODO temp disable warning
#pragma warning disable IDE0060 // Remove unused parameter

		// Standard including topographic plots
		static async void DrawStandardPlots(List<GroupedSearchResult> itemsToPlot, Settings settings, Graphics graphics, bool topographic = false, Progress<ProgressInfo>? progressInfo = null)
		{
			int i = 0;
			foreach (GroupedSearchResult item in itemsToPlot)
			{
				UpdateProgress(progressInfo, ++i, itemsToPlot.Count, $"Plotting standard coordinates");

				if (item.Space != settings.Space)
				{
					continue;
				}

				List<Instance> instances = await Database.GetInstances(item, item.Space);

				foreach (Instance instance in instances)
				{
					graphics.DrawImageCentered(item.PlotIcon.Image, instance.Coord.AsImagePoint(settings));
				}
			}
		}

		static async void DrawClusterPlots(List<GroupedSearchResult> itemsToPlot, Settings settings, Graphics graphics, Progress<ProgressInfo>? progressInfo = null)
		{
			// TODO
		}

		// Draw plots where plotted entities exist in a space reachable by this one
		static async void DrawConnectingSpacePlots(List<GroupedSearchResult> itemsToPlot, Settings settings, Graphics graphics, Progress<ProgressInfo>? progressInfo = null)
		{
			if (!settings.PlotSettings.ShowPlotsInOtherSpaces)
			{
				return;
			}

			int i = 0;
			foreach (GroupedSearchResult item in itemsToPlot)
			{
				UpdateProgress(progressInfo, ++i, itemsToPlot.Count, $"Plotting instances in other spaces");

				// Draw doors where the space of the selected item is the selected space, but more exist in connecting spaces
				foreach (Instance teleporter in (await Database.GetTeleporters(item.Space)).DistinctBy(i => i.TeleportsTo))
				{
					if (teleporter.Space != settings.Space)
					{
						continue;
					}

					DrawFinalConnectingSpacePlot(item, teleporter, settings, graphics);
				}

				// If the item's space is the selected space, then we already did all we need to above
				if (item.Space == settings.Space)
				{
					continue;
				}

				// Draw doors where the space of the selected item is not the selected space
				foreach (Instance teleporter in (await Database.GetTeleporters(settings.Space)).DistinctBy(i => i.TeleportsTo))
				{
					if (item.Space != teleporter.TeleportsTo)
					{
						continue;
					}

					DrawFinalConnectingSpacePlot(item, teleporter, settings, graphics);
				}
			}
		}

		static async void DrawFinalConnectingSpacePlot(GroupedSearchResult item, Instance teleporter, Settings settings, Graphics graphics)
		{
			List<Instance> instances = await Database.GetInstances(item, teleporter.TeleportsTo!);

			if (instances.Count == 0)
			{
				return;
			}

			// TODO polish
			graphics.DrawImageCentered(FileIO.DoorMarker, teleporter.Coord.AsImagePoint(settings));
			PointF point = teleporter.Coord.AsImagePoint(settings);
			graphics.DrawStringCentered($"{teleporter.TeleportsTo.DisplayName}: {instances.Count} {item.Entity.EditorID}", GetFont(FontSizeItemsInOtherSpaces), new SolidBrush(item.PlotIcon.Color), new PointF(point.X, point.Y + 50), true);
		}

		// Draw the FormID of the instance of the plot
		static async void DrawInstanceFormIDs(List<GroupedSearchResult> itemsToPlot, Settings settings, Graphics graphics)
		{
			if (!settings.PlotSettings.DrawInstanceFormID)
			{
				return;
			}

			// TODO
		}

		// Draw the color scale demonstrating the topographic color/height mapping
		static async void DrawTopographicLegend(Settings settings, Graphics graphics)
		{
			// TODO
		}

		// Draws the region or shape belonging to the given instance
		static async void DrawRegionOrShape(Settings settings, Graphics graphics, Instance instance)
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
			string text = $"{settings.Space.DisplayName} ({settings.Space.EditorID})\n";

			if (settings.MapSettings.SpotlightEnabled)
			{
				text += $"{Math.Round(settings.MapSettings.SpotlightLocation.X)}, {Math.Round(settings.MapSettings.SpotlightLocation.Y)} " +
					$"{Math.Round(settings.MapSettings.SpotlightSize * TileWidth / settings.Space.MaxRange, 3)}:1\n";
			}

			text += $"Game Version {await Database.GetGameVersion()} | Made with Mappalachia: github.com/AHeroicLlama/Mappalachia";

			Font font = GetFont(FontSizeWaterMark);
			RectangleF textBounds = new RectangleF(0, 0, MapImageResolution, MapImageResolution);

			DrawStringWithDropShadow(graphics, text, font, BrushGenericTransparent, textBounds, BottomRight);
		}

		static void DrawMapMarkerIconsAndLabels(Settings settings, Graphics graphics, Progress<ProgressInfo>? progressInfo = null)
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
		static Image DrawLegend(List<GroupedSearchResult> itemsToPlot, Settings settings, Image image)
		{
			// TODO
			switch (settings.MapSettings.LegendStyle)
			{
				case LegendStyle.Normal:
					break;

				case LegendStyle.Extended:
					int additionalWidth = 600;
					Bitmap resizedImage = new Bitmap(MapImageResolution + additionalWidth, MapImageResolution);

					Graphics graphics = Graphics.FromImage(resizedImage);
					graphics.DrawImage(image, additionalWidth, 0);
					return resizedImage;

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

		// Draw the string at the given coordinates, centered on the coord. Text wrap, and vertically centered or top-aligned to the coord
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

		// Returns the given World Coord as an Image PointF, considering spotlight scaling
		public static PointF AsImagePoint(this Coord coord, Settings settings)
		{
			// Find the center of the map in world coordinates
			Coord center = settings.MapSettings.SpotlightEnabled ? settings.MapSettings.SpotlightLocation : settings.Space.GetCenter();

			// Get the effective range of the image in world coordinates
			double plotRange = settings.MapSettings.SpotlightEnabled ? settings.MapSettings.SpotlightSize * TileWidth : settings.Space.MaxRange;

			// Find the factor between world and image coordinate
			double factor = plotRange / MapImageResolution;

			// Apply the scaling factor, offset by the center, and flip the Y axis
			return new PointF(
				(float)(((coord.X - center.X) / factor) + (MapImageResolution / 2)),
				(float)((((coord.Y * -1) + center.Y) / factor) + (MapImageResolution / 2)));
		}

		// Returns the given Image PointF as World coordinates, considering spotlight scaling
		public static Coord AsWorldCoord(this PointF point, Settings settings)
		{
			// Find the center of the map in world coordinates
			Coord center = settings.MapSettings.SpotlightEnabled ? settings.MapSettings.SpotlightLocation : settings.Space.GetCenter();

			// Get the effective range of the image in world coordinates
			double plotRange = settings.MapSettings.SpotlightEnabled ? settings.MapSettings.SpotlightSize * TileWidth : settings.Space.MaxRange;

			// Find the factor between world and image coordinate
			double factor = plotRange / MapImageResolution;

			// Inverse of the transformation made in AsImagePoint
			return new Coord(
				((point.X - (MapImageResolution / 2)) * factor) + center.X,
				(((point.Y - (MapImageResolution / 2)) * factor) - center.Y) * -1);
		}

		// Returns the given RectangleF of World coordinates to one in Image coordinates, considering spotlight scaling
		public static RectangleF AsImageRectangle(this RectangleF rect, Settings settings)
		{
			PointF topLeft = new Coord(rect.Left, rect.Top).AsImagePoint(settings);
			PointF bottomRight = new Coord(rect.Right, rect.Bottom).AsImagePoint(settings);

			return new RectangleF(topLeft.X, topLeft.Y, Math.Abs(bottomRight.X - topLeft.X), Math.Abs(bottomRight.Y - topLeft.Y));
		}

		// Returns the given RectangleF of image coordinates to one in World coordinates, considering spotlight scaling
		public static RectangleF AsWorldRectangle(this RectangleF rect, Settings settings)
		{
			Coord topLeft = new PointF(rect.Left, rect.Top).AsWorldCoord(settings);
			Coord bottomRight = new PointF(rect.Right, rect.Bottom).AsWorldCoord(settings);

			return new RectangleF((float)topLeft.X, (float)topLeft.Y, (float)Math.Abs(bottomRight.X - topLeft.X), (float)Math.Abs(topLeft.Y - bottomRight.Y));
		}

		// Returns the RectangleF in image coordinates which contains the map background when spotlight scaling is considered
		public static RectangleF GetScaledMapBackgroundRect(Settings settings)
		{
			if (!settings.MapSettings.SpotlightEnabled)
			{
				return new RectangleF(0, 0, MapImageResolution, MapImageResolution);
			}

			return settings.Space.GetRectangle().AsImageRectangle(settings);
		}

		// Returns the application font in the given pixel size
		static Font GetFont(int size)
		{
			return new Font(FileIO.GetFontFamily(), size, GraphicsUnit.Pixel);
		}
	}
}
