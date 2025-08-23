using System.Drawing.Drawing2D;
using Library;
using static Library.Common;
using static Mappalachia.FormsHelper;
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

	public enum CompassStyle
	{
		Off,
		WhenUseful,
		Always,
	}

	public static class Map
	{
		public static int BlastRadius { get; } = 20460; // 0x002D1160

		public static int CompassSize { get; } = MapImageResolution / 8;

		public static double IconScale { get; } = 1.5;

		static int TitlePadding { get; } = 30;

		static int LegendXMax { get; } = (int)(MapImageResolution / 6.3);

		static int LegendXPadding { get; } = 5;

		static int LegendYPadding { get; } = 5;

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

		static StringFormat CenterLeft { get; } = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };

		static StringFormat TopRight { get; } = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near };

		static StringFormat BottomRight { get; } = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far };

		static StringFormat BottomLeft { get; } = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Far };

		static int TopographLegendRectHeight { get; } = MapImageResolution / 3;

		static int TopographLegendRectWidth { get; } = MapImageResolution / 40;

		static RectangleF TopographLegendRect { get; } = new RectangleF(
			MapImageResolution - TopographLegendRectWidth,
			(MapImageResolution - TopographLegendRectHeight) / 2,
			TopographLegendRectWidth,
			TopographLegendRectHeight);

		static float TopographLegendDivisions { get; } = 100;

		// The primary map draw function
		public static async Task<Image?> Draw(List<GroupedSearchResult> itemsToPlot, Settings settings, Progress<ProgressInfo>? progressInfo, CancellationToken cancellationToken)
		{
			UpdateProgress(progressInfo, 10, "Draw started");

			// Set up the original image and graphics objects
			Image mapImage = new Bitmap(FileIO.EmptyMapImage);
			using Graphics graphics = GraphicsFromImageHQ(mapImage);

			// Draw the initial background image
			RectangleF backgroundRectangle = GetScaledMapBackgroundRect(settings);
			graphics.DrawImage(settings.Space.GetBackgroundImage(settings.MapSettings.BackgroundImage), backgroundRectangle);

			DrawSpotlightTiles(settings, graphics, progressInfo, cancellationToken);

			if (cancellationToken.IsCancellationRequested)
			{
				return null;
			}

			DrawCompassRose(settings, graphics);

			// Apply the brightness and grayscale if selected
			mapImage.AdjustBrightnessOrGrayscale(settings.MapSettings.Brightness, settings.MapSettings.GrayscaleBackground);

			// Overlay the water mask
			if (settings.MapSettings.HighlightWater && settings.Space.IsWorldspace)
			{
				graphics.DrawImage(settings.Space.GetWaterMask(), backgroundRectangle);
			}

			DrawMapMarkerIconsAndLabels(settings, graphics, progressInfo);

			itemsToPlot = itemsToPlot.OrderBy(i => i.LegendGroup).ToList();

			if (itemsToPlot.Count > 0)
			{
				// Call the relevant plot function
				switch (settings.PlotSettings.Mode)
				{
					case PlotMode.Standard:
						await DrawStandardPlots(itemsToPlot, settings, graphics, false, progressInfo, cancellationToken);
						break;

					case PlotMode.Topographic:
						await DrawStandardPlots(itemsToPlot, settings, graphics, true, progressInfo, cancellationToken);
						DrawTopographicLegend(settings, graphics, TopographLegendRect, TopographLegendDivisions);
						break;

					case PlotMode.Heatmap:
						await DrawHeatmapPlots(itemsToPlot, settings, graphics, progressInfo, cancellationToken);
						break;

					case PlotMode.Cluster:
						await DrawClusterPlots(itemsToPlot, settings, graphics, progressInfo, cancellationToken);
						break;

					default:
						throw new Exception("Unexpected PlotMode: " + settings.PlotSettings.Mode);
				}

				if (cancellationToken.IsCancellationRequested)
				{
					return null;
				}

				await DrawConnectingSpacePlots(itemsToPlot, settings, graphics, progressInfo);
				mapImage = DrawLegend(itemsToPlot, settings, mapImage, progressInfo);
			}

			await DrawWaterMark(settings, graphics);
			DrawTitle(settings, graphics);

			GC.Collect();

			UpdateProgress(progressInfo, 0, "Done");

			return mapImage;
		}

		static void DrawCompassRose(Settings settings, Graphics graphics)
		{
			if (settings.MapSettings.CompassStyle == CompassStyle.Off)
			{
				return;
			}

			if (settings.MapSettings.CompassStyle == CompassStyle.WhenUseful && settings.Space.NorthAngle <= 0.01)
			{
				return;
			}

			using Image canvas = new Bitmap(CompassSize, CompassSize);
			using Graphics compassGraphic = GraphicsFromImageHQ(canvas);

			// Rotate the graphic around its center by the north angle, then draw the compass onto it
			compassGraphic.TranslateTransform(canvas.Width / 2, canvas.Height / 2);
			compassGraphic.RotateTransform((float)settings.Space.NorthAngle);
			compassGraphic.TranslateTransform(canvas.Width / -2, canvas.Height / -2);
			compassGraphic.DrawImage(FileIO.CompassRose, new RectangleF(0, 0, canvas.Width, canvas.Height));

			// Draw the rotated compass onto the map
			graphics.DrawImage(canvas, new RectangleF(MapImageResolution - canvas.Width, 0, canvas.Width, canvas.Height));
		}

		static void DrawSpotlightTiles(Settings settings, Graphics graphics, Progress<ProgressInfo>? progressInfo, CancellationToken cancellationToken)
		{
			if (!settings.MapSettings.SpotlightEnabled ||
				settings.MapSettings.BackgroundImage != BackgroundImageType.Render)
			{
				return;
			}

			List<SpotlightTile> spotlightTiles = SpotlightTile.GetTilesInRect(new RectangleF(0, 0, MapImageResolution, MapImageResolution).AsWorldRectangle(settings), settings.Space);

			if (cancellationToken.IsCancellationRequested)
			{
				return;
			}

			// Pre-cache the tile images in parallel
			spotlightTiles.AsParallel().ForAll(tile =>
			{
				tile.GetSpotlightTileImage();
			});

			int i = 0;
			foreach (SpotlightTile tile in spotlightTiles)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return;
				}

				Image? image = tile.GetSpotlightTileImage();

				if (image is not null)
				{
					i++;
					int percent = (int)Math.Round(i / (double)spotlightTiles.Count * 100);
					UpdateProgress(progressInfo, percent, $"Drawing tile {i} of {spotlightTiles.Count}");

					graphics.DrawImage(image, tile.GetRectangle().AsImageRectangle(settings));
				}

#if DEBUG_SPOTLIGHT
				graphics.DrawStringCentered(
						$"{tile.XId}, {tile.YId}",
						GetFont(72),
						BrushGeneric,
						tile.GetCenter().AsImagePoint(settings));

				graphics.DrawRectangle(new Pen(Color.Orange, 5), tile.GetRectangle().AsImageRectangle(settings));
#endif
			}
		}

		// Standard including topographic plots
		static async Task DrawStandardPlots(List<GroupedSearchResult> itemsToPlot, Settings settings, Graphics graphics, bool topographic, Progress<ProgressInfo>? progressInfo, CancellationToken cancellationToken)
		{
			(double, double) topographRange = topographic ? await Database.GetZRange(itemsToPlot, settings) : (-1, -1);

			int i = 0;
			foreach (GroupedSearchResult item in itemsToPlot)
			{
				UpdateProgress(progressInfo, ++i, itemsToPlot.Count, $"Plotting {(topographic ? "topographic" : "standard")} coordinates and volumes");

				if (cancellationToken.IsCancellationRequested)
				{
					return;
				}

				if (!item.Space.Equals(settings.Space))
				{
					continue;
				}

				Font instanceFormIDFont = GetFont(settings.MapSettings.FontSettings.SizeInstanceFormID);
				List<Instance> instances = await Database.GetInstances(item, item.Space);

				foreach (Instance instance in instances)
				{
					Image? iconImage = null;
					Color color = item.PlotIcon.Color;

					// If this is topographic plot mode, and this is not a volume
					if (topographic && item.Entity is not Library.Region)
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
						color = LerpColors(settings.PlotSettings.PlotStyleSettings.SecondaryPalette.ToArray(), range);

						// If this is not a shape (therefore a normal topograph plot)
						if (instance.PrimitiveShape is null)
						{
							iconImage = item.PlotIcon.GetImage(color);
						}
					}

					if (instance.Entity is Library.Region region)
					{
						DrawRegion(settings, graphics, region, color);
					}
					else if (instance.PrimitiveShape is not null)
					{
						DrawPrimitiveShape(settings, graphics, instance, color);
					}
					else
					{
						graphics.DrawImageCentered(iconImage ?? item.PlotIcon.GetImage(), instance.Coord.AsImagePoint(settings));
					}

					if (settings.PlotSettings.DrawInstanceFormID)
					{
						PointF point = instance.Coord.AsImagePoint(settings);
						graphics.DrawStringCentered(instance.InstanceFormID.ToHex(), instanceFormIDFont, new SolidBrush(item.PlotIcon.Color), new PointF(point.X, point.Y + (item.PlotIcon.Size / 2)), false);
					}
				}
			}
		}

		static async Task DrawHeatmapPlots(List<GroupedSearchResult> itemsToPlot, Settings settings, Graphics graphics, Progress<ProgressInfo>? progressInfo, CancellationToken cancellationToken)
		{
			HeatmapSettings heatmapSettings = settings.PlotSettings.HeatmapSettings;

			using Bitmap heatmap = new Bitmap(MapImageResolution, MapImageResolution);
			using Graphics heatmapOverlay = Graphics.FromImage(heatmap);
			using Bitmap heatBlobDefaultWeight = CreateRadialGradientHeatSpot(heatmapSettings.Range, heatmapSettings.Intensity);

			int i = 0;
			foreach (GroupedSearchResult item in itemsToPlot)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return;
				}

				if (item.Space != settings.Space)
				{
					continue;
				}

				UpdateProgress(progressInfo, ++i, itemsToPlot.Count, "Plotting heatmap");

				Bitmap heatBlob = item.SpawnWeight == 1 ? heatBlobDefaultWeight : CreateRadialGradientHeatSpot(heatmapSettings.Range, (int)(heatmapSettings.Intensity * item.SpawnWeight));

				foreach (Instance instance in await GetInstancesDrawingVolumes(item, settings, graphics))
				{
					if (cancellationToken.IsCancellationRequested)
					{
						return;
					}

					heatmapOverlay.DrawImageCentered(heatBlob, instance.Coord.AsImagePoint(settings));
				}
			}

			if (cancellationToken.IsCancellationRequested)
			{
				return;
			}

			UpdateProgress(progressInfo, 90, "Colorizing heatmap");
			graphics.DrawImage(heatmap.InterpolateAgainstAlpha(settings.PlotSettings.PlotStyleSettings.SecondaryPalette.ToArray()), new PointF(0, 0));
		}

		// Return a bitmap of given size containing a black circular/radial gradient of linearly decreasing alpha
		static Bitmap CreateRadialGradientHeatSpot(int size, int intensity)
		{
			Bitmap bitmap = new Bitmap(size, size);
			double radius = size / 2;

			using Graphics graphics = Graphics.FromImage(bitmap);

			for (int x = 0; x < size; x++)
			{
				for (int y = 0; y < size; y++)
				{
					double xDist = x - radius;
					double yDist = y - radius;
					double distanceSquared = (xDist * xDist) + (yDist * yDist);
					double gradient = 1 - (distanceSquared / (radius * radius));

					if (gradient <= 0)
					{
						continue;
					}

					int alpha = Math.Min(255, (int)(gradient * intensity));
					bitmap.SetPixel(x, y, Color.Black.WithAlpha(alpha));
				}
			}

			return bitmap;
		}

		// Returns the non-shape non-region instances of the given item, drawing the volumes of the shapes and regions
		static async Task<List<Instance>> GetInstancesDrawingVolumes(GroupedSearchResult parentItem, Settings settings, Graphics graphics)
		{
			List<Instance> instances = await Database.GetInstances(parentItem, parentItem.Space);
			List<Instance> regions = instances.Where(instance => instance.Entity is Library.Region).ToList();
			List<Instance> shapes = instances.Where(instance => instance.PrimitiveShape is not null).ToList();

			foreach (Instance regionInstance in regions)
			{
				DrawRegion(settings, graphics, (Library.Region)regionInstance.Entity, parentItem.PlotIcon.Color);
			}

			foreach (Instance shapeInstance in shapes)
			{
				DrawPrimitiveShape(settings, graphics, shapeInstance, parentItem.PlotIcon.Color);
			}

			return instances.Where(instance => instance.Entity is not Library.Region && instance.PrimitiveShape is null).ToList();
		}

		static async Task DrawClusterPlots(List<GroupedSearchResult> itemsToPlot, Settings settings, Graphics graphics, Progress<ProgressInfo>? progressInfo, CancellationToken cancellationToken)
		{
			Font font = GetFont(settings.MapSettings.FontSettings.SizeClusterLabel);

			List<List<GroupedSearchResult>> clusterGroups = settings.PlotSettings.ClusterSettings.ClusterPerLegendGroup ?
				itemsToPlot.GroupBy(i => i.LegendGroup).Select(g => g.ToList()).ToList() :
				new List<List<GroupedSearchResult>>() { itemsToPlot };

			foreach (List<GroupedSearchResult> group in clusterGroups)
			{
				List<Instance> instances = new List<Instance>();

				GroupedSearchResult leadItem = group.First();

				UpdateProgress(progressInfo, 20, "Arranging cluster instances");

				foreach (GroupedSearchResult item in group)
				{
					if (!item.Space.Equals(settings.Space))
					{
						continue;
					}

					instances.AddRange(await GetInstancesDrawingVolumes(item, settings, graphics));
				}

				List<Cluster> clusters = new List<Cluster>();

				UpdateProgress(progressInfo, 40, "Forming clusters");

				foreach (Instance outerInstance in instances)
				{
					if (cancellationToken.IsCancellationRequested)
					{
						return;
					}

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

				UpdateProgress(progressInfo, 60, "Optimizing clusters");

				foreach (Instance instance in instances)
				{
					if (cancellationToken.IsCancellationRequested)
					{
						return;
					}

					Cluster closestCluster = clusters.MinBy(cluster => GeometryHelper.Pythagoras(cluster.Origin.Coord, instance.Coord)) ?? throw new Exception("Failed to find a closest cluster");

					if (instance.Cluster == closestCluster)
					{
						continue;
					}

					instance.Cluster!.RemoveMember(instance);
					closestCluster.AddMember(instance);
				}

				if (clusters.Count == 0)
				{
					continue;
				}

				Color color = leadItem.PlotIcon.Color;
				Pen pen = new Pen(color, ClusterLineThickness);
				Brush brush = new SolidBrush(color.WithAlpha(ClusterLabelAlpha));

				UpdateProgress(progressInfo, 80, "Drawing clusters");

				List<Cluster> clustersForDraw = clusters.Where(c => c.GetWeight() >= settings.PlotSettings.ClusterSettings.MinWeight).ToList();

				if (clustersForDraw.Count == 0)
				{
					Cluster? largest = clusters.MaxBy(c => c.GetWeight());

					if (largest is not null)
					{
						clustersForDraw.Add(largest);
					}
				}

				// Draw all qualifying clusters
				foreach (Cluster cluster in clustersForDraw)
				{
					if (cluster.Members.Count == 0)
					{
						continue;
					}

					// If the cluster is actually a polygon and not a line nor a point, draw the polygon, else, just use normal icons
					if (cluster.Members.Count > 2)
					{
						graphics.DrawPolygon(pen, cluster.Members.Select(m => m.Coord.AsImagePoint(settings)).ToList().GetConvexHull());
						graphics.DrawStringCentered(Math.Round(cluster.GetWeight(), 2).ToString(), font, brush, cluster.Members.GetCentroid().AsImagePoint(settings));
					}
					else
					{
						foreach (Instance instance in cluster.Members)
						{
							graphics.DrawImageCentered(leadItem.PlotIcon.GetImage(), instance.Coord.AsImagePoint(settings));
						}
					}
				}
			}
		}

		// Draw doors where plotted entities exist in a space reachable by this one
		static async Task DrawConnectingSpacePlots(List<GroupedSearchResult> itemsToPlot, Settings settings, Graphics graphics, Progress<ProgressInfo>? progressInfo = null)
		{
			Dictionary<Space, int> connectionsToSpace = new Dictionary<Space, int>();

			List<Instance> teleportersInSelectedSpace = await Database.GetTeleporters(settings.Space);

			Font font = GetFont(settings.MapSettings.FontSettings.SizeItemsInOtherSpaces);

			int i = 0;
			foreach (GroupedSearchResult item in itemsToPlot)
			{
				UpdateProgress(progressInfo, ++i, itemsToPlot.Count, "Finding instances in other spaces");

				// We're purely plotting this whole search result in the selected space - skip
				if (item.Space.Equals(settings.Space) && !settings.PlotSettings.AutoFindPlotsInConnectedSpaces)
				{
					continue;
				}

				// Find all 'teleporters' which exit this space or the space which this item is in
				// Take only one result per the source and destination of the teleporter
				List<Instance> teleporters =
					(await Database.GetTeleporters(item.Space))
					.Concat(teleportersInSelectedSpace)
					.DistinctBy(i => (i.Space, i.TeleportsTo))
					.ToList();

				foreach (Instance teleporter in teleporters)
				{
					// If autofind is off, and the door doesn't go this this space, nor the items space - skip
					if (!settings.PlotSettings.AutoFindPlotsInConnectedSpaces && !settings.Space.Equals(teleporter.TeleportsTo) && !item.Space.Equals(teleporter.TeleportsTo))
					{
						continue;
					}

					// If the item cannot be reached by this door - skip
					if (!item.Space.Equals(teleporter.Space) && !item.Space.Equals(teleporter.TeleportsTo))
					{
						continue;
					}

					// If the door doesn't exist on the current map - skip
					if (!teleporter.Space.Equals(settings.Space))
					{
						continue;
					}

					// If the plot is flux but the target space isn't nukable - skip
					if (item.Entity is DerivedRawFlux && !teleporter.TeleportsTo!.IsNukable())
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
						font,
						new SolidBrush(item.PlotIcon.Color),
						new PointF(point.X, point.Y + 20 + (25 * connectionsToSpace[teleporter.TeleportsTo])));
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
				graphics.FillRectangle(new SolidBrush(LerpColors(settings.PlotSettings.PlotStyleSettings.SecondaryPalette.ToArray(), Math.Abs(y - height) / (double)height).WithAlpha(TopographLegendAlpha)), sliceRect);
			}
		}

		// Draws the region instance
		static void DrawRegion(Settings settings, Graphics graphics, Library.Region region, Color color)
		{
			Pen pen = new Pen(color, VolumeEdgeThickness);
			Brush brush = new SolidBrush(color.WithAlpha(VolumeFillAlpha));
			Font font = GetFont(settings.MapSettings.FontSettings.SizeRegionLevel);

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

					graphics.DrawStringCentered(levelString, font, new SolidBrush(color), subregion.GetCentroid().AsImagePoint(settings));
				}
			}
		}

		// Draws the Shape belonging to the given instance
		static void DrawPrimitiveShape(Settings settings, Graphics graphics, Instance instance, Color color)
		{
			if (instance.PrimitiveShape is null)
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
				case ShapeType.Cylinder:
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
			string titleText = settings.MapSettings.Title;

			if (titleText.IsNullOrWhiteSpace())
			{
				return;
			}

			Font font = GetFont(settings.MapSettings.FontSettings.SizeTitle);
			SizeF stringBounds = graphics.MeasureString(titleText, font, new SizeF(MapImageResolution, MapImageResolution));

			RectangleF textBounds = new RectangleF(
				MapImageResolution - stringBounds.Width - TitlePadding,
				0,
				stringBounds.Width + TitlePadding,
				stringBounds.Height);

			DrawStringWithDropShadow(graphics, titleText, font, BrushGeneric, textBounds, TopRight);
		}

		static async Task DrawWaterMark(Settings settings, Graphics graphics)
		{
			Font font = GetFont(settings.MapSettings.FontSettings.SizeWatermark);

			string text = $"{settings.Space.DisplayName} ({settings.Space.EditorID})";

			if (settings.Space.IsInstanceable)
			{
				text += " (Instanced)";
			}

			text += $"\nGame Version {await Database.GetGameVersion()} | Made with Mappalachia: github.com/AHeroicLlama/Mappalachia";
			RectangleF textBounds = new RectangleF(0, 0, MapImageResolution, MapImageResolution);

			DrawStringWithDropShadow(graphics, text, font, BrushGenericTransparent, textBounds, BottomRight);
		}

		static void DrawMapMarkerIconsAndLabels(Settings settings, Graphics graphics, Progress<ProgressInfo>? progressInfo = null)
		{
			if (!settings.MapSettings.MapMarkerIcons && !settings.MapSettings.MapMarkerLabels)
			{
				return;
			}

			UpdateProgress(progressInfo, 50, "Drawing map markers");

			Font font = GetFont(settings.MapSettings.FontSettings.SizeMapMarkerLabel);

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

					// Hidden option - convert a new copy to grayscale
					if (settings.MapSettings.MapMarkerGrayscale)
					{
						image = new Bitmap(image).AdjustBrightnessOrGrayscale(1, true);
					}

					graphics.DrawImageCentered(image, coord);

					labelOffset = image.Height / 2;
				}

				if (settings.MapSettings.MapMarkerLabels)
				{
					graphics.DrawStringCentered(marker.Label, font, BrushGeneric, new PointF(coord.X, coord.Y + labelOffset), !settings.MapSettings.MapMarkerIcons);
				}
			}
		}

		// May return an image of different dimensions if extended legend is used
		static Image DrawLegend(List<GroupedSearchResult> itemsToPlot, Settings settings, Image image, Progress<ProgressInfo>? progressInfo)
		{
			itemsToPlot = itemsToPlot.DistinctBy(item => (item.LegendGroup, item.LegendText)).ToList();

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
			Font font = GetFont(settings.MapSettings.FontSettings.SizeLegend);

			// Find the starting Y pos of the legend: sum the heights of the legend text line or icon (whichever is largest)
			// Then half it, flip it, and offset by the midpoint of the image
			float totalHeight = itemsToPlot.Sum(item => Math.Max(graphics.MeasureString(item.LegendText, font, new SizeF(LegendXMax - item.PlotIcon.Size - (LegendXPadding * 2), MapImageResolution)).Height, item.PlotIcon.Size));
			float yPos = (MapImageResolution / 2) + (totalHeight / -2);

			foreach (GroupedSearchResult item in itemsToPlot)
			{
				UpdateProgress(progressInfo, 95, $"Drawing legend");

				SizeF bounds = graphics.MeasureString(item.LegendText, font, new SizeF(LegendXMax - item.PlotIcon.Size - (LegendXPadding * 2), MapImageResolution));

				float halfRowHeight = Math.Max(bounds.Height, item.PlotIcon.Size + LegendYPadding) / 2;
				float midPointRowPos = yPos + halfRowHeight;
				float iconXMid = LegendXPadding + (item.PlotIcon.Size / 2);

				graphics.DrawStringWithDropShadow(
					item.LegendText,
					font,
					new SolidBrush(item.PlotIcon.Color),
					new RectangleF(iconXMid * 2, midPointRowPos - (bounds.Height / 2), bounds.Width, bounds.Height),
					CenterLeft);

				if (item.Entity is not Library.Region)
				{
					Image legendIcon = settings.PlotSettings.Mode == PlotMode.Topographic ?
						item.PlotIcon.GetImage(item.PlotIcon.Color) :
						item.PlotIcon.GetImage();

					graphics.DrawImageCentered(
						legendIcon,
						new PointF(iconXMid, midPointRowPos));
				}

				yPos += halfRowHeight * 2;
			}

			if (totalHeight > MapImageResolution)
			{
				string text = "(continued)";
				SizeF size = graphics.MeasureString(text, font);

				graphics.DrawStringWithDropShadow(
					text,
					font,
					BrushGeneric,
					new RectangleF(LegendXMax, MapImageResolution - size.Height, MapImageResolution - size.Width, size.Height),
					BottomLeft);
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

		static int AsImageLength(this int value, Settings settings)
		{
			return (int)(value / GetWorldToImageFactor(settings));
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
