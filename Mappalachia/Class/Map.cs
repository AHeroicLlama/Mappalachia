using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using Mappalachia.Class;

namespace Mappalachia
{
	// The Map image, adjusting it, drawing it and plotting points onto it
	public static class Map
	{
		// Map-image coordinate scaling. Gathered manually by eye with reference points from in-game
		public static float scaling = 142;
		static readonly float xOffset = 1.7f;
		static readonly float yOffset = 5.2f;

		// Hidden settings
		public static readonly int mapDimension = 4096; // All background images should be this^2
		public static readonly int maxZoom = (int)(mapDimension * 2.0);
		public static readonly int minZoom = (int)(mapDimension * 0.05);
		public static readonly double markerIconScale = 2.5; // The scaling applied to map marker icons

		// Cluster mode tuning
		static readonly float clusteringRange = 100; // Max distance between plots for them to belong to the same cluster
		static readonly int clusterRefinementMaxIterations = 100; // Hard cap to stop clustering iterations after this many times
		static readonly float clusterMinPolygonArea = 250; // The minimum area px^2 that a cluster polygon must fill or else uses a centroid-centered bounding circle
		static readonly int clusterPolygonLineThickness = 4;
		static readonly int clusterBoundingCircleMinRadius = 15; // Clusters given a bounding circle are rendered at least this big
		static readonly int clusterPolygonPointReductionRange = 5; // Points in the cluster convex hull this close together are merged
		static readonly int clusterMinimumAngle = 10; // A cluster convex hull with an interior angle tighter than this are dropped as too 'pointy' and fall back to the circle
		static readonly Brush clusterWeightBrush = new SolidBrush(Color.FromArgb(200, Color.White));

		// Legend text positioning
		static readonly int legendIconX = 59; // The X Coord of the plot icon that is drawn next to each legend string
		public static readonly int plotXMin = 650; // Number of pixels in from the left of the map image where the player cannot reach
		static readonly int plotXMax = 3610;
		static readonly int plotYMin = 508;
		static readonly int plotYMax = 3382;
		static readonly int legendXMin = 116; // The padding in the from the left where legend text begins
		static readonly int legendWidth = plotXMin - legendXMin; // The resultant width (or length) of legend text rows in pixels
		static readonly int legendYPadding = 80; // Vertical space at top/bottom of image where legend text will not be drawn
		static readonly SizeF legendBounds = new SizeF(legendWidth, mapDimension - (legendYPadding * 2)); // Used for MeasureString to calculate legend string dimensions

		// Font and text
		public static readonly int legendFontSize = 48;
		public static readonly int mapLabelFontSize = 21;
		static readonly int fontDropShadowOffset = 3;
		static readonly int mapLabelMaxWidth = 180; // Maximum width before a map marker label will enter a new line
		static readonly Brush dropShadowBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 0));
		static readonly Brush brushWhite = new SolidBrush(Color.White);
		static readonly PrivateFontCollection fontCollection = IOManager.LoadFont();
		static readonly Font mapLabelFont = new Font(fontCollection.Families[0], mapLabelFontSize, GraphicsUnit.Pixel);
		static readonly Font legendFont = new Font(fontCollection.Families[0], legendFontSize, GraphicsUnit.Pixel);
		static readonly RectangleF infoTextBounds = new RectangleF(plotXMin, 0, mapDimension - plotXMin, mapDimension);
		static readonly StringFormat stringFormatBottomRight = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far }; // Align the text bottom-right
		static readonly StringFormat stringFormatBottomLeft = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Far }; // Align the text bottom-left
		static readonly StringFormat stringFormatCenter = new StringFormat() { Alignment = StringAlignment.Center }; // Align the text centrally

		// Volume plots
		public static readonly int volumeOpacity = 128;
		public static readonly uint minVolumeDimension = 8; // Minimum X or Y dimension in pixels below which a volume will use a plot icon instead

		// References to FormMaster elements
		static PictureBox mapFrame;
		public static ProgressBar progressBarMain;

		static Image finalImage;
		static Image backgroundLayer;

		public static Image GetImage()
		{
			return finalImage;
		}

		// Scale a coordinate from game coordinates down to map coordinates.
		public static float ScaleCoordinate(int coord, bool isYAxis)
		{
			if (isYAxis)
			{
				coord *= -1;
			}

			return (coord / scaling) + (mapDimension / 2f) + (isYAxis ? yOffset : xOffset);
		}

		// Attach the map image to the PictureBox on the master form
		public static void SetOutput(PictureBox pictureBox)
		{
			mapFrame = pictureBox;
		}

		// Construct the map background layer, without plotted points
		public static void DrawBaseLayer()
		{
			// Start with the basic image
			backgroundLayer = IOManager.GetImageForSpace(SettingsSpace.GetSpace());
			if (SettingsSpace.drawOutline)
			{
				Graphics backgroundGraphics = Graphics.FromImage(backgroundLayer);
				DrawCellBackground(backgroundGraphics);
			}

			Graphics graphic = Graphics.FromImage(backgroundLayer);
			float b = SettingsMap.brightness / 100f;

			// Apply grayscale color matrix, or just apply brightness adjustments
			ColorMatrix matrix = SettingsMap.grayScale ?
				new ColorMatrix(new float[][]
					{
						new float[] { 0.299f * b, 0.299f * b, 0.299f * b, 0, 0 },
						new float[] { 0.587f * b, 0.587f * b, 0.587f * b, 0, 0 },
						new float[] { 0.114f * b, 0.114f * b, 0.114f * b, 0, 0 },
						new float[] { 0, 0, 0, 1, 0 },
						new float[] { 0, 0, 0, 0, 1 },
					}) :
				new ColorMatrix(new float[][]
					{
						new float[] { b, 0, 0, 0, 0 },
						new float[] { 0, b, 0, 0, 0 },
						new float[] { 0, 0, b, 0, 0 },
						new float[] { 0, 0, 0, 1, 0 },
						new float[] { 0, 0, 0, 0, 1 },
					});

			ImageAttributes attributes = new ImageAttributes();
			attributes.SetColorMatrix(matrix);

			Point[] points =
			{
				new Point(0, 0),
				new Point(mapDimension, 0),
				new Point(0, mapDimension),
			};
			Rectangle rect = new Rectangle(0, 0, mapDimension, mapDimension);

			DrawMapMarkers(graphic);

			graphic.DrawImage(backgroundLayer, points, rect, GraphicsUnit.Pixel, attributes);

			Draw(); // Redraw the whole map since we updated the base layer
		}

		// Construct the final map by drawing plots over the background layer
		public static void Draw()
		{
			// Reset the current image to the background layer
			finalImage = new Bitmap(backgroundLayer);

			// Start progress bar off at 0
			progressBarMain.Value = progressBarMain.Minimum;
			float progress = 0;

			Graphics imageGraphic = Graphics.FromImage(finalImage);
			imageGraphic.SmoothingMode = SmoothingMode.AntiAlias;
			imageGraphic.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

			// Draw the bottom-right info/watermark paragraph
			DrawInfoWatermark(imageGraphic);

			// Draw all legend text for every MapItem
			int skippedLegends = DrawLegend(legendFont, imageGraphic);

			// Adds additional text if some items were missed from legend
			if (skippedLegends > 0)
			{
				string extraLegendText = "+" + skippedLegends + " more item" + (skippedLegends == 1 ? string.Empty : "s") + "...";
				imageGraphic.DrawString(extraLegendText, legendFont, brushWhite, infoTextBounds, stringFormatBottomLeft);
			}

			// Nothing else to plot - ensure we update for the background layer but then return
			if (FormMaster.legendItems.Count == 0)
			{
				GC.Collect();
				mapFrame.Image = finalImage;
				return;
			}

			SpaceScaling spaceScaling = SettingsSpace.GetSpace().GetScaling();

			// Draw a height-color key in Topography mode
			if (SettingsPlot.IsTopographic())
			{
				// Identify the sizing and locations for drawing the height-color-key strings
				double numHeightKeys = SettingsPlotTopograph.heightKeyIndicators;
				Font topographFont = new Font(fontCollection.Families[0], 62, GraphicsUnit.Pixel);
				float singleLineHeight = imageGraphic.MeasureString(SettingsPlotTopograph.heightKeyString, topographFont, new SizeF(infoTextBounds.Width, infoTextBounds.Height)).Height;

				// Identify the lower limit to start printing the key so that it ends up centered
				double baseHeight = (mapDimension / 2) - (singleLineHeight * (numHeightKeys / 2d));

				for (int i = 0; i <= numHeightKeys - 1; i++)
				{
					Brush brush = new SolidBrush(GetTopographColor(i / (numHeightKeys - 1)));
					imageGraphic.DrawString(SettingsPlotTopograph.heightKeyString, topographFont, brush, new RectangleF(plotXMax, 0, mapDimension - plotXMax, (float)(mapDimension - baseHeight)), stringFormatBottomRight);
					baseHeight += singleLineHeight;
				}
			}

			// Count how many Map Data Points are due to be mapped
			int totalMapDataPoints = 0;
			foreach (MapItem mapItem in FormMaster.legendItems)
			{
				totalMapDataPoints += mapItem.count;
			}

			// Loop through every MapDataPoint represented by all the MapItems to find the min/max z coord in the dataset
			bool first = true;
			int zMin = 0;
			int zMax = 0;
			double zRange = 0;
			if (SettingsPlot.IsTopographic())
			{
				foreach (MapItem mapItem in FormMaster.legendItems)
				{
					foreach (MapDataPoint point in mapItem.GetPlots())
					{
						if (first)
						{
							zMin = point.z - (point.boundZ / 2);
							zMax = point.z + (point.boundZ / 2);
							first = false;
							continue;
						}

						// Do not contribute outlier values to the min/max range - this ensures they have the same
						// color as the min/max *legitimate* item and they do not skew the color ranges
						if (point.z > SettingsPlotTopograph.zThreshUpper || point.z < SettingsPlotTopograph.zThreshLower)
						{
							continue;
						}

						if (point.z - (point.boundZ / 2) < zMin)
						{
							zMin = point.z - (point.boundZ / 2);
						}

						if (point.z + (point.boundZ / 2) > zMax)
						{
							zMax = point.z + (point.boundZ / 2);
						}
					}
				}

				zRange = Math.Abs(zMax - zMin);

				if (zRange == 0)
				{
					zRange = 1;
				}
			}

			if (SettingsPlot.IsIconOrTopographic())
			{
				// Processing each MapItem in serial, draw plots for every matching valid MapDataPoint
				foreach (MapItem mapItem in FormMaster.legendItems)
				{
					// Generate a Plot Icon and colours/brushes to be used for all instances of the MapItem
					PlotIcon plotIcon = mapItem.GetIcon();
					Image plotIconImg = SettingsPlot.IsIcon() ? plotIcon.GetIconImage() : null; // Icon mode has icon per MapItem, Topography needs icons per MapDataPoint and will be generated later
					Color volumeColor = Color.FromArgb(volumeOpacity, plotIcon.color);
					Brush volumeBrush = new SolidBrush(volumeColor);

					// Iterate over every data point and draw it
					foreach (MapDataPoint point in mapItem.GetPlots())
					{
						// Skip the point if its origin is outside the surface world
						if (SettingsSpace.CurrentSpaceIsWorld() &&
							(point.x < plotXMin || point.x >= plotXMax ||
							point.y < plotYMin || point.y >= plotYMax))
						{
							continue;
						}

						// If this coordinate exceeds the user-selected space mapping height bounds, skip it
						// (Also accounts for the z-height of volumes)
						if (!SettingsSpace.CurrentSpaceIsWorld() &&
							(point.z + (point.boundZ / 2d) < SettingsSpace.GetMinHeightCoordBound() ||
							point.z - (point.boundZ / 2d) > SettingsSpace.GetMaxHeightCoordBound()))
						{
							continue;
						}

						// Override colors in Topographic mode
						if (SettingsPlot.IsTopographic())
						{
							// Clamp the z values to the percieved outlier threshold
							double z = point.z + (point.boundZ / 2);
							z = Math.Max(Math.Min(z, SettingsPlotTopograph.zThreshUpper), SettingsPlotTopograph.zThreshLower);

							// Normalize the height of this item between the min/max z of the whole set
							double colorValue = (z - zMin) / zRange;

							// Override the plot icon color
							plotIcon = mapItem.GetIcon();
							plotIcon.color = GetTopographColor(colorValue);
							plotIconImg = plotIcon.GetIconImage(); // Generate a new icon with a unique color for this height color

							// Apply the color to volume plotting too
							volumeColor = Color.FromArgb(volumeOpacity, plotIcon.color);
							volumeBrush = new SolidBrush(volumeColor);
						}

						// If this meets all the criteria to be suitable to be drawn as a volume
						if (point.primitiveShape != string.Empty && // This is a primitive shape at all
							SettingsPlot.drawVolumes && // Volume drawing is enabled
							point.boundX >= minVolumeDimension && point.boundY >= minVolumeDimension) // This is large enough to be visible if drawn as a volume
						{
							Image volumeImage = new Bitmap((int)point.boundX, (int)point.boundY);
							Graphics volumeGraphic = Graphics.FromImage(volumeImage);
							volumeGraphic.SmoothingMode = SmoothingMode.AntiAlias;

							switch (point.primitiveShape)
							{
								case "Box":
								case "Line":
								case "Plane":
									volumeGraphic.FillRectangle(volumeBrush, new Rectangle(0, 0, (int)point.boundX, (int)point.boundY));
									break;
								case "Sphere":
								case "Ellipsoid":
									volumeGraphic.FillEllipse(volumeBrush, new Rectangle(0, 0, (int)point.boundX, (int)point.boundY));
									break;
								default:
									continue; // If we reach this, we dropped the drawing of a volume. Verify we've covered all shapes via the database summary.txt
							}

							volumeImage = ImageHelper.RotateImage(volumeImage, point.rotationZ);
							imageGraphic.DrawImage(volumeImage, (float)(point.x - (volumeImage.Width / 2)), (float)(point.y - (volumeImage.Height / 2)));
						}

						// This MapDataPoint is not suitable to be drawn as a volume - draw a normal plot icon, or topographic plot
						else
						{
							imageGraphic.DrawImage(plotIconImg, (float)(point.x - (plotIconImg.Width / 2d)), (float)(point.y - (plotIconImg.Height / 2d)));
						}
					}

					// Increment the progress bar per MapItem
					progress += mapItem.count;
					progressBarMain.Value = (int)((progress / totalMapDataPoints) * progressBarMain.Maximum);
					Application.DoEvents();
				}
			}
			else if (SettingsPlot.IsHeatmap())
			{
				int resolution = SettingsPlotHeatmap.resolution;
				int blendRange = SettingsPlotHeatmap.blendDistance;

				// Create a 2D Array of HeatMapGridSquare
				HeatMapGridSquare[,] squares = new HeatMapGridSquare[resolution, resolution];
				for (int x = 0; x < resolution; x++)
				{
					for (int y = 0; y < resolution; y++)
					{
						squares[x, y] = new HeatMapGridSquare();
					}
				}

				int pixelsPerSquare = mapDimension / resolution;

				foreach (MapItem mapItem in FormMaster.legendItems)
				{
					int heatmapLegendGroup = SettingsPlotHeatmap.IsDuo() ? mapItem.legendGroup % 2 : 0;

					foreach (MapDataPoint point in mapItem.GetPlots())
					{
						// Skip the point if its origin is outside the surface world
						if (SettingsSpace.CurrentSpaceIsWorld() &&
							(point.x < plotXMin || point.x >= plotXMax ||
							point.y < plotYMin || point.y >= plotYMax))
						{
							continue;
						}

						// If this coordinate exceeds the user-selected space mapping height bounds, skip it
						// (Also accounts for the z-height of volumes)
						if (!SettingsSpace.CurrentSpaceIsWorld() &&
							(point.z + (point.boundZ / 2d) < SettingsSpace.GetMinHeightCoordBound() ||
							point.z - (point.boundZ / 2d) > SettingsSpace.GetMaxHeightCoordBound()))
						{
							continue;
						}

						// Identify which grid square this MapDataPoint falls within
						int squareX = (int)Math.Floor(point.x / pixelsPerSquare);
						int squareY = (int)Math.Floor(point.y / pixelsPerSquare);

						// Loop over every grid square within range, and increment by the weight proportional to the distance
						for (int x = squareX - blendRange; x < squareX + blendRange; x++)
						{
							for (int y = squareY - blendRange; y < squareY + blendRange; y++)
							{
								// Don't try to target squares which would lay outside of the grid
								if (x < 0 || x >= resolution || y < 0 || y >= resolution)
								{
									continue;
								}

								// Pythagoras on the x and y dist gives us the 'as the crow flies' distance between the squares
								double distance = GeometryHelper.Pythagoras(squareX - x, squareY - y);

								// Weight and hence brightness is modified by 1/x^2 + 1 where x is the distance from actual item
								double additionalWeight = point.weight * (1d / ((distance * distance) + 1));
								squares[x, y].weights[heatmapLegendGroup] += additionalWeight;
							}
						}
					}

					// Increment the progress bar per MapItem
					progress += mapItem.count;
					progressBarMain.Value = (int)((progress / totalMapDataPoints) * progressBarMain.Maximum);
					Application.DoEvents();
				}

				// Find the largest weight value of all squares
				double largestWeight = 0;
				for (int x = 0; x < resolution; x++)
				{
					for (int y = 0; y < resolution; y++)
					{
						double weight = squares[x, y].GetTotalWeight();
						if (weight > largestWeight)
						{
							largestWeight = weight;
						}
					}
				}

				// Finally now weights are calculated, draw a square for every HeatGripMapSquare in the array
				for (int x = 0; x < resolution; x++)
				{
					int xCoord = x * pixelsPerSquare;

					// Don't draw grid squares which are entirely within the legend text area
					if (xCoord + pixelsPerSquare < plotXMin)
					{
						continue;
					}

					for (int y = 0; y < resolution; y++)
					{
						int yCoord = y * pixelsPerSquare;

						Color color = squares[x, y].GetColor(largestWeight);
						Brush brush = new SolidBrush(color);

						Rectangle heatMapSquare = new Rectangle(xCoord, yCoord, mapDimension / SettingsPlotHeatmap.resolution, mapDimension / SettingsPlotHeatmap.resolution);
						imageGraphic.FillRectangle(brush, heatMapSquare);
					}
				}
			}
			else if (SettingsPlot.IsCluster())
			{
				List<MapCluster> clusters = new List<MapCluster>();
				List<MapDataPoint> points = new List<MapDataPoint>();

				// Unroll all MapDataPoints
				foreach (MapItem mapItem in FormMaster.legendItems)
				{
					foreach (MapDataPoint point in mapItem.GetPlots())
					{
						points.Add(point);
					}
				}

				// First pass. Top-level - take a datapoint and make a new cluster if it's so-far not a member
				// Then go round all unadopted points and see if they're suitable to be a member
				// This generates an imperfect but good start for clustering points together
				foreach (MapDataPoint outerPoint in points)
				{
					// if it's already a member move on
					if (outerPoint.IsMemberOfCluster())
					{
						continue;
					}

					// Start a new cluster
					MapCluster cluster = new MapCluster(outerPoint);
					clusters.Add(cluster);

					// Inner-level - scan all datapoints for nearby points to add to this cluster, and add them to this cluster
					foreach (MapDataPoint innerPoint in points)
					{
						if (innerPoint.IsMemberOfCluster())
						{
							continue;
						}

						double totalDist = GeometryHelper.Pythagoras(innerPoint.Get2DPoint(), outerPoint.Get2DPoint());

						if (totalDist < clusteringRange)
						{
							cluster.AddMember(innerPoint);
						}
					}
				}

				/* Second pass - we've now put all in-range points into clusters arbitrarily based on which point(s) we selected first.
				However it is still possible for one point to be a member of a cluster which it is not closest to,
				as it was simply "adopted" by its parent cluster first.
				So now we iterate over all points against all clusters, and assess which of these clusters they should actually be members of,
				based off closest location, and then move them to the closer cluster.
				I believe it is possible for this algorithm to never reach an end as an equilibrium is possible with points being bounced between clusters.
				So we must decide on n passes of the datapoints and stop.*/
				for (int i = 0; i < clusterRefinementMaxIterations; i++)
				{
					int movedPoints = 0;
					foreach (MapDataPoint point in points)
					{
						MapCluster closestCluster = clusters.MinBy(cluster => cluster.GetDistance(point));

						// Move point to the nearest cluster
						if (point.GetParentCluster() != closestCluster)
						{
							point.LeaveCluster();
							closestCluster.AddMember(point);
							movedPoints++;
						}
					}

					//DrawMapClusters(clusters, imageGraphic);
					//GC.Collect();
					//mapFrame.Image = finalImage;
					//Application.DoEvents();

					// Finally none needed moving - we're done
					if (movedPoints == 0)
					{
						break;
					}
				}

				DrawMapClusters(clusters, imageGraphic);
			}

			GC.Collect();
			mapFrame.Image = finalImage;
		}

		static void DrawMapClusters(List<MapCluster> clusters, Graphics imageGraphic)
		{
			Pen clusterPolygonPen = new Pen(SettingsPlotStyle.GetFirstColor(), clusterPolygonLineThickness);
			Pen thinPen = new Pen(Color.White, 1);
			Pen thinGreenPen = new Pen(Color.Lime, 1);

			List<double> rankedWeights = clusters.OrderBy(cluster => cluster.GetMemberWeight()).Select(cluster => cluster.GetMemberWeight()).ToList();
			double averageWeight = clusters.Average(cluster => cluster.GetMemberWeight());

			foreach (MapCluster cluster in clusters)
			{
				List<PointF> convexHull = GeometryHelper.GetConvexHull(cluster.GetPoints());
				PointF centroid = GeometryHelper.GetCentroid(cluster.GetPoints());

				if (convexHull.Count > 1)
				{
					convexHull = GeometryHelper.ReducePolygon(convexHull, clusterPolygonPointReductionRange);

					// Convex hull too small or pointy
					if (GeometryHelper.AreaOfPolygon(convexHull) < clusterMinPolygonArea || GeometryHelper.GetPolygonSmallestAngle(convexHull) < clusterMinimumAngle)
					{
						float radius = Math.Max(clusterBoundingCircleMinRadius, cluster.GetBoundingRadius());

						imageGraphic.DrawEllipse(
							clusterPolygonPen,
							new RectangleF(centroid.X - radius, centroid.Y - radius, radius * 2, radius * 2));
					}
					else
					{
						imageGraphic.DrawPolygon(clusterPolygonPen, convexHull.ToArray());
					}
				}

				////DEBUG
				//foreach (PointF point in cluster.GetPoints())
				//{
				//	imageGraphic.DrawLine(thinGreenPen, new PointF(point.X + 4, point.Y + 4), new PointF(point.X - 4, point.Y - 4));
				//	imageGraphic.DrawLine(thinGreenPen, new PointF(point.X + 4, point.Y - 4), new PointF(point.X - 4, point.Y + 4));
				//	imageGraphic.DrawLine(thinPen, cluster.GetCentroid(), point);
				//}

				double weight = cluster.GetMemberWeight();
				string printWeight = Math.Round(weight, 1).ToString();

				float fontSize = Math.Min(Math.Max(20, mapLabelFontSize * (float)(weight / averageWeight)), 50);
				Font sizedFont = new Font(fontCollection.Families[0], fontSize, GraphicsUnit.Pixel);

				SizeF textBounds = imageGraphic.MeasureString(printWeight, sizedFont, new SizeF(mapLabelMaxWidth, mapLabelMaxWidth));
				RectangleF textBox = new RectangleF(
						centroid.X - (textBounds.Width / 2),
						centroid.Y - (textBounds.Height / 2),
						textBounds.Width, textBounds.Height);

				// Draw Drop shadow first
				imageGraphic.DrawString(printWeight, sizedFont, dropShadowBrush,
					new RectangleF(textBox.X + fontDropShadowOffset, textBox.Y + fontDropShadowOffset, textBox.Width, textBox.Height), stringFormatCenter);

				// Draw the count
				imageGraphic.DrawString(printWeight, sizedFont, clusterWeightBrush, textBox, stringFormatCenter);
			}

			GC.Collect();
		}

		static void DrawInfoWatermark(Graphics imageGraphic)
		{
			imageGraphic.TextRenderingHint = TextRenderingHint.AntiAlias;
			Space currentSpace = SettingsSpace.GetSpace();

			string infoText = (SettingsPlot.IsTopographic() ? "Topographic View\n" : string.Empty) + "Game version " + IOManager.GetGameVersion() +
				"\nMade with Mappalachia - github.com/AHeroicLlama/Mappalachia";

			if (!currentSpace.IsWorldspace())
			{
				infoText =
					currentSpace.displayName + " (" + currentSpace.editorID + ")\n" +
					"Height distribution: " + SettingsSpace.minHeightPerc + "% - " + SettingsSpace.maxHeightPerc + "%\n" +
					"Scale: 1:" + Math.Round(currentSpace.GetScaling().scale, 2) + "\n\n" +
					infoText;
			}

			imageGraphic.DrawString(infoText, legendFont, brushWhite, infoTextBounds, stringFormatBottomRight);
		}

		static void DrawMapMarkers(Graphics imageGraphic)
		{
			if (!SettingsMap.showMapIcons && !SettingsMap.showMapLabels)
			{
				return;
			}

			List<MapMarker> markers = Database.GetMapMarkers(SettingsSpace.GetCurrentFormID());

			if (SettingsMap.showMapIcons)
			{
				// Draw all markers first on a lower layer
				foreach (MapMarker marker in markers)
				{
					Image markerImage = IOManager.GetMapMarker(marker.markerName);
					imageGraphic.DrawImage(markerImage, new PointF((float)marker.x - (markerImage.Width / 2f), (float)marker.y - (markerImage.Height / 2f)));
				}
			}

			if (SettingsMap.showMapLabels)
			{
				// Now draw map marker labels on top
				imageGraphic.TextRenderingHint = TextRenderingHint.AntiAlias;
				foreach (MapMarker marker in markers)
				{
					Image markerImage = IOManager.GetMapMarker(marker.markerName);
					SizeF textBounds = imageGraphic.MeasureString(marker.label, mapLabelFont, new SizeF(mapLabelMaxWidth, mapLabelMaxWidth));
					float labelHeightOffset = SettingsMap.showMapIcons ? markerImage.Height / 2f : -textBounds.Height / 2f;

					RectangleF textBox = new RectangleF(
							(float)marker.x - (textBounds.Width / 2),
							(float)marker.y + labelHeightOffset,
							textBounds.Width, textBounds.Height);

					// Draw Drop shadow first
					imageGraphic.DrawString(marker.label, mapLabelFont, dropShadowBrush,
						new RectangleF(textBox.X + fontDropShadowOffset, textBox.Y + fontDropShadowOffset, textBox.Width, textBox.Height), stringFormatCenter);

					// Draw the map marker label
					imageGraphic.DrawString(marker.label, mapLabelFont, brushWhite, textBox, stringFormatCenter);
				}
			}
		}

		// Draws all legend text (and optional Icon beside) for every MapItem
		// Returns the number of items missed off the legend due to size constraints
		static int DrawLegend(Font font, Graphics imageGraphic)
		{
			if (FormMaster.legendItems.Count == 0 || SettingsMap.hideLegend)
			{
				return 0;
			}

			Dictionary<int, string> overridingLegendText = FormMaster.GatherOverriddenLegendTexts();
			List<int> drawnGroups = new List<int>();

			// Calculate the total height of all legend strings with their plot icons beside, combined
			int legendTotalHeight = 0;
			foreach (MapItem mapItem in FormMaster.legendItems)
			{
				// Skip legend groups that are merged/overridden and have already been accounted for
				if (drawnGroups.Contains(mapItem.legendGroup) && overridingLegendText.ContainsKey(mapItem.legendGroup))
				{
					continue;
				}

				legendTotalHeight += Math.Max(
					(int)Math.Ceiling(imageGraphic.MeasureString(mapItem.GetLegendText(false), font, legendBounds).Height),
					SettingsPlot.IsIconOrTopographic() ? SettingsPlotStyle.iconSize : 0);

				drawnGroups.Add(mapItem.legendGroup);
			}

			int skippedLegends = 0; // How many legend items did not fit onto the map

			// The initial Y coord where first legend item should be written, in order to Y-center the entire legend
			int legendCaretHeight = ((int)legendBounds.Height / 2) - (legendTotalHeight / 2);

			// Reset the drawn groups list, as we need to iterate over the items again
			drawnGroups = new List<int>();

			// Loop over every MapItem and draw the legend
			foreach (MapItem mapItem in FormMaster.legendItems)
			{
				// Skip legend groups that are merged/overridden and have already been drawn
				if (drawnGroups.Contains(mapItem.legendGroup) && overridingLegendText.ContainsKey(mapItem.legendGroup))
				{
					continue;
				}

				// Calculate positions and color for legend text (plus icon)
				int fontHeight = (int)Math.Ceiling(imageGraphic.MeasureString(mapItem.GetLegendText(false), font, legendBounds).Height);

				PlotIcon icon = mapItem.GetIcon();
				Image plotIconImg = SettingsPlot.IsIconOrTopographic() ? icon.GetIconImage() : null;

				Color legendColor = SettingsPlot.IsTopographic() ? SettingsPlotTopograph.legendColor : mapItem.GetLegendColor();
				Brush textBrush = new SolidBrush(legendColor);

				int iconHeight = SettingsPlot.IsIconOrTopographic() ?
					plotIconImg.Height :
					0;

				int legendHeight = Math.Max(fontHeight, iconHeight);

				// If the icon is taller than the text, offset the text it so it sits Y-centrally against the icon
				int textOffset = 0;
				if (iconHeight > fontHeight)
				{
					textOffset = (iconHeight - fontHeight) / 2;
				}

				// If the legend text/item fits on the map vertically
				if (legendCaretHeight > legendYPadding && legendCaretHeight + legendHeight < mapDimension - legendYPadding)
				{
					if (SettingsPlot.IsIconOrTopographic())
					{
						imageGraphic.DrawImage(plotIconImg, (float)(legendIconX - (plotIconImg.Width / 2d)), (float)(legendCaretHeight - (plotIconImg.Height / 2d) + (legendHeight / 2d)));
					}

					// Draw Drop shadow first
					imageGraphic.DrawString(mapItem.GetLegendText(false), font, dropShadowBrush,
						new RectangleF(legendXMin + fontDropShadowOffset, legendCaretHeight + textOffset + fontDropShadowOffset, legendWidth, legendHeight));

					// Draw the properly colored legend text
					imageGraphic.DrawString(mapItem.GetLegendText(false), font, textBrush,
						new RectangleF(legendXMin, legendCaretHeight + textOffset, legendWidth, legendHeight));
				}
				else
				{
					skippedLegends++;
				}

				drawnGroups.Add(mapItem.legendGroup);
				legendCaretHeight += legendHeight; // Move the 'caret' down for the next item, enough to fit the icon and the text
			}

			return skippedLegends;
		}

		// Draws an outline of all items in the current space to act as background/template
		static void DrawCellBackground(Graphics backgroundLayer)
		{
			if (SettingsSpace.CurrentSpaceIsWorld())
			{
				return;
			}

			SpaceScaling spaceScaling = SettingsSpace.GetSpace().GetScaling();

			int outlineWidth = SettingsSpace.outlineWidth;
			int outlineSize = SettingsSpace.outlineSize;

			Image plotIconImg = new Bitmap(outlineSize, outlineSize);
			Graphics plotIconGraphic = Graphics.FromImage(plotIconImg);
			plotIconGraphic.SmoothingMode = SmoothingMode.AntiAlias;
			Color outlineColor = Color.FromArgb(SettingsSpace.outlineAlpha, SettingsSpace.outlineColor);
			Pen outlinePen = new Pen(outlineColor, outlineWidth);
			plotIconGraphic.DrawEllipse(
				outlinePen,
				new RectangleF(outlineWidth, outlineWidth, outlineSize - (outlineWidth * 2), outlineSize - (outlineWidth * 2)));

			// Iterate over every data point and draw it
			foreach (MapDataPoint point in Database.GetAllSpaceCoords(SettingsSpace.GetCurrentFormID()))
			{
				// If this coordinate exceeds the user-selected space mapping height bounds, skip it
				// (Also accounts for the z-height of volumes)
				if (point.z < SettingsSpace.GetMinHeightCoordBound() || point.z > SettingsSpace.GetMaxHeightCoordBound())
				{
					continue;
				}

				point.x += spaceScaling.xOffset;
				point.y += spaceScaling.yOffset;

				// Multiply the coordinates by the scaling, but multiply around 0,0
				point.x = ((point.x - (mapDimension / 2)) * spaceScaling.scale) + (mapDimension / 2);
				point.y = ((point.y - (mapDimension / 2)) * spaceScaling.scale) + (mapDimension / 2);
				point.boundX *= spaceScaling.scale;
				point.boundY *= spaceScaling.scale;

				backgroundLayer.DrawImage(plotIconImg, (float)(point.x - (plotIconImg.Width / 2d)), (float)(point.y - (plotIconImg.Height / 2d)));
			}
		}

		// Return a color for the topograph plot given its normalized altitude, interpolated against the user-defined selection of topographic plot colors
		public static Color GetTopographColor(double colorValue)
		{
			// Find the first x colors in the icon color palette, where x is the number of distinct topography colors selected (Or the entire palette if smaller)
			int colorCount = Math.Min(SettingsPlotTopograph.colorBands, SettingsPlotStyle.paletteColor.Count);

			if (colorCount == 0)
			{
				return SettingsPlotTopograph.legendColor; // This should not happen - palette size nor distinct color count shouldn't be allowed to be 0
			}

			if (colorCount == 1)
			{
				// There is only one color in the palette - there is no distinction to be made
				return SettingsPlotStyle.GetFirstColor();
			}

			// Find the 'parent' colors above and below this plot on the scale which should define its color
			int colorBelow = (int)(colorValue * (colorCount - 1));
			int colorAbove = Math.Min(colorBelow + 1, colorCount - 1);

			double rangePerColor = 1d / (colorCount - 1);

			// Re-normalize the range once more to the range purely between the 2 parent colors.
			// Example: parent colors exist at 0.2 and 0.4, child color exists at 0.35. Normalized between parents it is therefore 0.75
			double rangeBetweenColors = (colorValue - (colorBelow * rangePerColor)) * (colorCount - 1);

			return ImageHelper.LerpColors(SettingsPlotStyle.paletteColor[colorBelow], SettingsPlotStyle.paletteColor[colorAbove], rangeBetweenColors);
		}

		public static void Open()
		{
			IOManager.OpenImage(finalImage);
		}
	}
}
