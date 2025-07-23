using System.Drawing.Drawing2D;
using Library;
using static Library.Common;
using static Mappalachia.ImageHelper;

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

		static int LegendXMax { get; } = 650;

		static int LegendXPadding { get; } = 5;

		static int LegendYPadding { get; } = 5;

		static int MapMarkerLabelTextMaxWidth { get; } = 150; // Max width of the label text, before it attempts to wrap

		static Font FontLegend { get; } = GetFont(40);

		static Font FontItemsInOtherSpaces { get; } = GetFont(20);

		static Font FontMapMarkerLabel { get; } = GetFont(20);

		static Font FontWatermark { get; } = GetFont(60);

		static Font FontClusterLabel { get; } = GetFont(26);

		static Font FontInstanceFormID { get; } = GetFont(32);

		static Font FontRegionLevel { get; } = GetFont(32);

		static int VolumeEdgeThickness { get; } = 5;

		static int VolumeFillAlpha { get; } = 64;

		static int TopographLegendAlpha { get; } = 128;

		static int ClusterLabelAlpha { get; } = 200;

		static int ClusterLineThickness { get; } = 3;

		public static int DropShadowOffset { get; } = 2;

		public static Color DropShadowColor { get; } = Color.FromArgb(128, 0, 0, 0);

		static Brush BrushDropShadow { get; } = new SolidBrush(DropShadowColor);

		static Brush BrushGeneric { get; } = Brushes.White;

		static Brush BrushGenericTransparent { get; } = new SolidBrush(Color.FromArgb(128, 255, 255, 255));

		static StringFormat Center { get; } = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

		static StringFormat TopRight { get; } = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near };

		static StringFormat BottomRight { get; } = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far };

		static StringFormat CenterLeft { get; } = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };

		static int TopographLegendRectHeight { get; } = 1600;

		static RectangleF TopographLegendRect { get; } = new RectangleF(
			MapImageResolution - 150,
			(MapImageResolution - TopographLegendRectHeight) / 2,
			100,
			TopographLegendRectHeight);

		static float TopographLegendDivisions { get; } = 100;

		// The primary map draw function
		public static Image Draw(List<GroupedSearchResult> itemsToPlot, Settings settings, Progress<ProgressInfo>? progressInfo = null)
		{
			UpdateProgress(progressInfo, 10, "Draw started");

			// Set up the original image and graphics objects
			Image mapImage = new Bitmap(FileIO.EmptyMapImage);
			using Graphics graphics = GraphicsFromImageHQ(mapImage);

			// Draw the initial background image
			RectangleF backgroundRectangle = GetScaledMapBackgroundRect(settings);
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

			if (itemsToPlot.Count > 0)
			{
				// Call the relevant plot function
				switch (settings.PlotSettings.Mode)
				{
					case PlotMode.Standard:
						DrawStandardPlots(itemsToPlot, settings, graphics, false, progressInfo);
						break;

					case PlotMode.Topographic:
						DrawStandardPlots(itemsToPlot, settings, graphics, true, progressInfo);
						DrawTopographicLegend(settings, graphics, TopographLegendRect, TopographLegendDivisions);
						break;

					case PlotMode.Cluster:
						DrawClusterPlots(itemsToPlot, settings, graphics, progressInfo);
						break;

					default:
						throw new Exception("Unexpected PlotMode: " + settings.PlotSettings.Mode);
				}

				DrawConnectingSpacePlots(itemsToPlot, settings, graphics, progressInfo);
				DrawInstanceFormIDs(itemsToPlot, settings, graphics);
				mapImage = DrawLegend(itemsToPlot, settings, mapImage);
			}

			DrawWaterMark(settings, graphics);
			DrawTitle(settings, graphics);

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

		// Overload to automatically calculate percentage from x out of y
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

		// Standard including topographic plots
		static async void DrawStandardPlots(List<GroupedSearchResult> itemsToPlot, Settings settings, Graphics graphics, bool topographic = false, Progress<ProgressInfo>? progressInfo = null)
		{
			(double, double) topographRange = topographic ? await Database.GetZRange(itemsToPlot, settings) : (-1, -1);

			int i = 0;
			foreach (GroupedSearchResult item in itemsToPlot)
			{
				UpdateProgress(progressInfo, ++i, itemsToPlot.Count, $"Plotting standard coordinates and volumes");

				if (item.Space != settings.Space)
				{
					continue;
				}

				List<Instance> instances = await Database.GetInstances(item, item.Space);

				foreach (Instance instance in instances)
				{
					Image iconImage = item.PlotIcon.GetImage();
					Color color = item.PlotIcon.Color;

					if (topographic)
					{
						double range;

						if (topographRange.Item1 == topographRange.Item2)
						{
							range = 0.5;
						}
						else
						{
							range = (instance.HeightForTopograph - topographRange.Item1) / (topographRange.Item2 - topographRange.Item1);
						}

						// Overrides the image and color
						color = LerpColors(settings.PlotSettings.TopographicPalette.ToArray(), range);
						iconImage = item.PlotIcon.GetImage(color);
					}

					if (instance.Entity is Library.Region region)
					{
						DrawRegion(settings, graphics, region, color);
					}
					else if (instance.PrimitiveShape != null)
					{
						DrawPrimitiveShape(settings, graphics, instance, color);
					}
					else
					{
						graphics.DrawImageCentered(iconImage, instance.Coord.AsImagePoint(settings));
					}
				}
			}
		}

		static async void DrawClusterPlots(List<GroupedSearchResult> itemsToPlot, Settings settings, Graphics graphics, Progress<ProgressInfo>? progressInfo = null)
		{
			foreach (GroupedSearchResult item in itemsToPlot)
			{
				if (item.Space != settings.Space)
				{
					continue;
				}

				List<Instance> instances = await Database.GetInstances(item, item.Space);
				List<Cluster> clusters = new List<Cluster>();

				// Split out volumes - draw them normally, outside of the cluster calculation + draw
				List<Instance> regions = instances.Where(instance => instance.Entity is Library.Region).ToList();
				List<Instance> shapes = instances.Where(instance => instance.PrimitiveShape != null).ToList();

				instances = instances.Where(instance => instance.Entity is not Library.Region && instance.PrimitiveShape == null).ToList();

				foreach (Instance regionInstance in regions)
				{
					DrawRegion(settings, graphics, (Library.Region)regionInstance.Entity, item.PlotIcon.Color);
				}

				foreach (Instance shapeInstance in shapes)
				{
					DrawPrimitiveShape(settings, graphics, shapeInstance, item.PlotIcon.Color);
				}

				int i = 0;
				foreach (Instance outerInstance in instances)
				{
					UpdateProgress(progressInfo, ++i, instances.Count, "Finding clusters");

					if (outerInstance.IsMemberOfCluster)
					{
						continue;
					}

					clusters.Add(new Cluster(outerInstance));

					foreach (Instance innerInstance in instances)
					{
						if (innerInstance.IsMemberOfCluster)
						{
							continue;
						}

						if (GeometryHelper.Pythagoras(innerInstance.Coord, outerInstance.Coord) < settings.PlotSettings.ClusterSettings.Range)
						{
							outerInstance.Cluster!.AddMember(innerInstance);
						}
					}
				}

				i = 0;
				foreach (Instance instance in instances)
				{
					UpdateProgress(progressInfo, ++i, instances.Count, "Optimizing clusters");

					Cluster closestCluster = clusters.MinBy(cluster => GeometryHelper.Pythagoras(cluster.Origin.Coord, instance.Coord)) ?? throw new Exception("Failed to find a closest cluster");

					if (instance.Cluster == closestCluster)
					{
						continue;
					}

					instance.Cluster!.RemoveMember(instance);
					closestCluster.AddMember(instance);
				}

				i = 0;
				Color color = item.PlotIcon.Color;
				Pen pen = new Pen(color, ClusterLineThickness);
				Brush brush = new SolidBrush(color.WithAlpha(ClusterLabelAlpha));

				// Draw all qualifying clusters
				foreach (Cluster cluster in clusters)
				{
					UpdateProgress(progressInfo, ++i, clusters.Count, "Drawing clusters");

					if (cluster.GetWeight() < settings.PlotSettings.ClusterSettings.MinWeight)
					{
						continue;
					}

					if (cluster.Members.Count > 1)
					{
						graphics.DrawPolygon(pen, cluster.Members.Select(m => m.Coord.AsImagePoint(settings)).ToList().GetConvexHull());
					}

					graphics.DrawStringCentered(Math.Round(cluster.GetWeight(), 2).ToString(), FontClusterLabel, brush, cluster.Members.GetCentroid().AsImagePoint(settings));
				}
			}
		}

		// Draw doors where plotted entities exist in a space reachable by this one
		static async void DrawConnectingSpacePlots(List<GroupedSearchResult> itemsToPlot, Settings settings, Graphics graphics, Progress<ProgressInfo>? progressInfo = null)
		{
			Dictionary<Space, int> connectionsToSpace = new Dictionary<Space, int>();

			List<Instance> teleportersInSelectedSpace = await Database.GetTeleporters(settings.Space);

			int i = 0;
			foreach (GroupedSearchResult item in itemsToPlot)
			{
				UpdateProgress(progressInfo, ++i, itemsToPlot.Count, "Finding instances in other spaces");

				// We're purely plotting this whole search result in the selected space - skip
				if (item.Space == settings.Space && !settings.PlotSettings.AutoFindPlotsInConnectedSpaces)
				{
					continue;
				}

				// Find all 'teleporters' which exit this space or the space which this item is in
				// Take only one result per the source and destination of the teleporter
				List<Instance> teleporters =
					(await Database.GetTeleporters(item.Space))
					.Concat(teleportersInSelectedSpace)
					.DistinctBy(i => (i.Space, i.TeleportsTo)).ToList();

				foreach (Instance teleporter in teleporters)
				{
					// If autofind is off, and the door doesn't go this this space, nor the items space - skip
					if (!settings.PlotSettings.AutoFindPlotsInConnectedSpaces && teleporter.TeleportsTo != settings.Space && teleporter.TeleportsTo != item.Space)
					{
						continue;
					}

					// If the item cannot be reached by this door - skip
					if (item.Space != teleporter.Space && item.Space != teleporter.TeleportsTo)
					{
						continue;
					}

					// If the door doesn't exist on the curent map - skip
					if (teleporter.Space != settings.Space)
					{
						continue;
					}

					List<Instance> instances = await Database.GetInstances(item, teleporter.TeleportsTo);

					if (instances.Count == 0)
					{
						continue;
					}

					// We use this Dict to simply track how many items have been attributed to each space
					// in order that we can Y-offset the label to prevent overtyping
					if (!connectionsToSpace.TryGetValue(teleporter.TeleportsTo!, out int count))
					{
						connectionsToSpace.Add(teleporter.TeleportsTo!, 1);
					}
					else
					{
						connectionsToSpace[teleporter.TeleportsTo!] = ++count;
					}

					PointF point = teleporter.Coord.AsImagePoint(settings);
					graphics.DrawImageCentered(FileIO.DoorMarker, teleporter.Coord.AsImagePoint(settings));

					graphics.DrawStringCentered(
						$"{teleporter.TeleportsTo!.DisplayName}: {item.Entity.EditorID} ({instances.Count})",
						FontItemsInOtherSpaces,
						new SolidBrush(item.PlotIcon.Color),
						new PointF(point.X, point.Y + 20 + (25 * connectionsToSpace[teleporter.TeleportsTo])));
				}
			}
		}

		// Draw the FormID of the instance of the plot
		static async void DrawInstanceFormIDs(List<GroupedSearchResult> itemsToPlot, Settings settings, Graphics graphics)
		{
			if (!settings.PlotSettings.DrawInstanceFormID)
			{
				return;
			}

			foreach (GroupedSearchResult searchResult in itemsToPlot)
			{
				foreach (Instance instance in await Database.GetInstances(searchResult, settings.Space))
				{
					if (instance.InstanceFormID == 0)
					{
						continue;
					}

					PointF point = instance.Coord.AsImagePoint(settings);
					graphics.DrawStringCentered(instance.InstanceFormID.ToHex(), FontInstanceFormID, new SolidBrush(searchResult.PlotIcon.Color), new PointF(point.X, point.Y + (settings.PlotSettings.PlotIconSize / 2)), false);
				}
			}
		}

		// Draw the color scale demonstrating the topographic color/height mapping
		static void DrawTopographicLegend(Settings settings, Graphics graphics, RectangleF legendRect, float divisions)
		{
			float height = legendRect.Height;
			float step = height / divisions;

			for (float y = 0; y < height; y += step)
			{
				RectangleF sliceRect = new RectangleF(legendRect.X, legendRect.Y + y, legendRect.Width, step);
				graphics.FillRectangle(new SolidBrush(LerpColors(settings.PlotSettings.TopographicPalette.ToArray(), Math.Abs(y - height) / (double)height).WithAlpha(TopographLegendAlpha)), sliceRect);
			}
		}

		// Draws the region instance
		static void DrawRegion(Settings settings, Graphics graphics, Library.Region region, Color color)
		{
			Pen pen = new Pen(color, VolumeEdgeThickness);
			Brush brush = new SolidBrush(color.WithAlpha(VolumeFillAlpha));

			foreach (List<RegionPoint> subregion in region.GetAllSubRegions())
			{
				PointF[] points = subregion.Select(s => s.Coord.AsImagePoint(settings)).ToArray();

				if (settings.PlotSettings.VolumeDrawMode == VolumeDrawMode.Fill || settings.PlotSettings.VolumeDrawMode == VolumeDrawMode.Both)
				{
					graphics.FillPolygon(brush, points, FillMode.Winding);
				}

				if (settings.PlotSettings.VolumeDrawMode == VolumeDrawMode.Border || settings.PlotSettings.VolumeDrawMode == VolumeDrawMode.Both)
				{
					graphics.DrawPolygon(pen, points);
				}

				if (settings.PlotSettings.ShowRegionLevels)
				{
					string levelString = string.Empty;

					if (region.MinLevel == 0 && region.MaxLevel == 0)
					{
						continue;
					}

					if (region.MinLevel <= 1 && region.MaxLevel != 0)
					{
						levelString = $"\u2264{region.MaxLevel}";
					}
					else if (region.MinLevel != 0 && region.MaxLevel == 0)
					{
						levelString = $"\u2265{region.MinLevel}";
					}
					else
					{
						levelString = $"{region.MinLevel}-{region.MaxLevel}";
					}

					graphics.DrawStringCentered(levelString, FontRegionLevel, new SolidBrush(color), subregion.GetCentroid().AsImagePoint(settings));
				}
			}
		}

		// Draws the Shape belonging to the given instance
		static void DrawPrimitiveShape(Settings settings, Graphics graphics, Instance instance, Color color)
		{
			if (instance.PrimitiveShape == null)
			{
				throw new Exception("Instance has no shape");
			}

			Shape shape = (Shape)instance.PrimitiveShape;

			Pen pen = new Pen(color, VolumeEdgeThickness);
			Brush brush = new SolidBrush(color.WithAlpha(VolumeFillAlpha));

			PointF topLeft = new Coord(instance.Coord.X - (shape.BoundX / 2), instance.Coord.Y + (shape.BoundY / 2)).AsImagePoint(settings);
			PointF center = instance.Coord.AsImagePoint(settings);
			RectangleF rectangle = new RectangleF(topLeft.X, topLeft.Y, shape.BoundX.AsImageLength(settings), shape.BoundY.AsImageLength(settings));

			// Rotate the graphics 'canvas' around the center of the shape
			graphics.TranslateTransform(center.X, center.Y);
			graphics.RotateTransform((float)shape.RotZ);
			graphics.TranslateTransform(-center.X, -center.Y);

			VolumeDrawMode drawMode = settings.PlotSettings.VolumeDrawMode;

			switch (shape.ShapeType)
			{
				case ShapeType.Box:
				case ShapeType.Line:
				case ShapeType.Plane:
					if (drawMode == VolumeDrawMode.Fill || drawMode == VolumeDrawMode.Both)
					{
						graphics.FillRectangle(brush, rectangle);
					}

					if (drawMode == VolumeDrawMode.Border || drawMode == VolumeDrawMode.Both)
					{
						graphics.DrawRectangle(pen, rectangle);
					}

					break;

				case ShapeType.Sphere:
				case ShapeType.Ellipsoid:
					if (drawMode == VolumeDrawMode.Fill || drawMode == VolumeDrawMode.Both)
					{
						graphics.FillEllipse(brush, rectangle);
					}

					if (drawMode == VolumeDrawMode.Border || drawMode == VolumeDrawMode.Both)
					{
						graphics.DrawEllipse(pen, rectangle);
					}

					break;

				default:
					graphics.ResetTransform();
					throw new Exception($"Unexpected shape type {shape.ShapeType}");
			}

			graphics.ResetTransform();
		}

		static void DrawTitle(Settings settings, Graphics graphics)
		{
			string titleText = settings.MapSettings.Title.Trim();

			if (titleText.IsNullOrWhiteSpace())
			{
				return;
			}

			Font font = GetFont(settings.MapSettings.TitleFontSize);
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
			string text = $"{settings.Space.DisplayName} ({settings.Space.EditorID})";

			if (settings.Space.IsInstanceable)
			{
				text += " (Instanced)";
			}

			text += "\n";

			if (settings.MapSettings.SpotlightEnabled)
			{
				text += $"{Math.Round(settings.MapSettings.SpotlightLocation.X)}, {Math.Round(settings.MapSettings.SpotlightLocation.Y)} " +
					$"{Math.Round(settings.MapSettings.SpotlightSize * TileWidth / settings.Space.MaxRange, 3)}:1\n";
			}

			text += $"Game Version {await Database.GetGameVersion()} | Made with Mappalachia: github.com/AHeroicLlama/Mappalachia";
			RectangleF textBounds = new RectangleF(0, 0, MapImageResolution, MapImageResolution);

			DrawStringWithDropShadow(graphics, text, FontWatermark, BrushGenericTransparent, textBounds, BottomRight);
		}

		static void DrawMapMarkerIconsAndLabels(Settings settings, Graphics graphics, Progress<ProgressInfo>? progressInfo = null)
		{
			if (!settings.MapSettings.MapMarkerIcons && !settings.MapSettings.MapMarkerLabels)
			{
				return;
			}

			UpdateProgress(progressInfo, 50, "Drawing map markers");

			List<MapMarker> mapMarkers = Database.AllMapMarkers
				.Where(mapMarker => mapMarker.SpaceFormID == settings.Space.FormID)
				.OrderBy(mapMarker => mapMarker.Coord.Y).ToList();

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
					graphics.DrawStringCentered(marker.Label, FontMapMarkerLabel, BrushGeneric, new PointF(coord.X, coord.Y + labelOffset), !settings.MapSettings.MapMarkerIcons, MapMarkerLabelTextMaxWidth);
				}
			}
		}

		// May return an image of different dimensions if extended legend is used
		static Image DrawLegend(List<GroupedSearchResult> itemsToPlot, Settings settings, Image image)
		{
			itemsToPlot = itemsToPlot.DistinctBy(item => (item.LegendGroup, item.LegendText, item.PlotIcon)).ToList();

			if (itemsToPlot.Count == 0)
			{
				return image;
			}

			if (settings.MapSettings.LegendStyle == LegendStyle.None)
			{
				return image;
			}

			if (settings.MapSettings.LegendStyle == LegendStyle.Extended)
			{
				Bitmap resizedImage = new Bitmap(MapImageResolution + LegendXMax, MapImageResolution);

				using Graphics resizeGraphics = GraphicsFromImageHQ(resizedImage);
				resizeGraphics.DrawImage(image, LegendXMax, 0);
				image = resizedImage;
			}

			using Graphics graphics = GraphicsFromImageHQ(image);

			// TODO BUG - height of varying height plot icons not correct
			// Find the starting Y pos of the legend: sum the heights of the legend text line or icon (whichever is largest)
			// Then half it, flip it, and offset by the midpoint of the image
			float height = (MapImageResolution / 2) + (itemsToPlot.Sum(item => Math.Max(graphics.MeasureString(item.LegendText, FontLegend, new SizeF(LegendXMax, MapImageResolution)).Height, item.PlotIcon.Size)) / -2);

			foreach (GroupedSearchResult item in itemsToPlot)
			{
				Color legendColor = settings.PlotSettings.Mode == PlotMode.Topographic ?
					LerpColors(settings.PlotSettings.TopographicPalette.ToArray(), 0.5) :
					item.PlotIcon.Color;

				SizeF bounds = graphics.MeasureString(item.LegendText, FontLegend, new SizeF(LegendXMax - item.PlotIcon.Size - (LegendXPadding * 2), MapImageResolution));

				graphics.DrawStringWithDropShadow(
					item.LegendText,
					FontLegend,
					new SolidBrush(legendColor),
					new RectangleF((LegendXPadding * 2) + item.PlotIcon.Size, height, bounds.Width, bounds.Height),
					CenterLeft);

				if (item.Entity is not Library.Region)
				{
					Image legendIcon = settings.PlotSettings.Mode == PlotMode.Topographic ?
					item.PlotIcon.GetImage(legendColor) :
					item.PlotIcon.GetImage();

					graphics.DrawImageCentered(
						legendIcon,
						new PointF(LegendXPadding + (item.PlotIcon.Size / 2), height + (bounds.Height / 2)));
				}

				height += Math.Max(bounds.Height, item.PlotIcon.Size + LegendYPadding);
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

		// Return the effective center of the map image in world coords
		static Coord GetWorldCenter(Settings settings)
		{
			return settings.MapSettings.SpotlightEnabled ? settings.MapSettings.SpotlightLocation : settings.Space.GetCenter();
		}

		// Return the value which defines the scaling factor from world to image coords
		static double GetWorldToImageFactor(Settings settings)
		{
			// Get the effective range of the image in world coordinates
			double plotRange = settings.MapSettings.SpotlightEnabled ? settings.MapSettings.SpotlightSize * TileWidth : settings.Space.MaxRange;

			// Find the factor between world and image coordinate
			return plotRange / MapImageResolution;
		}

		// Returns a single length/size value scaled from world scale to image scale
		static float AsImageLength(this double value, Settings settings)
		{
			return (float)(value / GetWorldToImageFactor(settings));
		}

		// Returns the given World Coord as an Image PointF, considering spotlight scaling
		public static PointF AsImagePoint(this Coord coord, Settings settings)
		{
			Coord center = GetWorldCenter(settings);
			double factor = GetWorldToImageFactor(settings);

			// Apply the scaling factor, offset by the center, and flip the Y axis
			return new PointF(
				(float)(((coord.X - center.X) / factor) + (MapImageResolution / 2)),
				(float)((((coord.Y * -1) + center.Y) / factor) + (MapImageResolution / 2)));
		}

		// Returns the given Image PointF as World coordinates, considering spotlight scaling
		public static Coord AsWorldCoord(this PointF point, Settings settings)
		{
			Coord center = GetWorldCenter(settings);
			double factor = GetWorldToImageFactor(settings);

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
