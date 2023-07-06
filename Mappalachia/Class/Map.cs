using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Threading;

namespace Mappalachia
{
	// The Map image, adjusting it, drawing it and plotting points onto it
	public static class Map
	{
		// Hidden settings
		public const int mapDimension = 4096; // All background images should be this^2
		public const double maxZoomRatio = 2;
		public const double minZoomRatio = 0.1;
		public const double markerIconScale = 1; // The scaling applied to map marker icons

		const int volumeGCThreshold = 2000000; // GC after drawing a volume of 2m px
		public const int volumeRejectThreshold = 4200000; // Reject drawing a volume of 4.2m px

		// Text positioning
		const int legendIconX = 59; // The X Coord of the plot icon that is drawn next to each legend string
		public const int plotXMin = 650; // Number of pixels in from the left of the map image where the player cannot reach
		const int topographKeyX = 3610;
		const int legendXMin = 116; // The padding in from the left where legend text begins
		const int legendWidth = plotXMin - legendXMin; // The resultant width (or length) of legend text rows in pixels
		const int legendYPadding = 80; // Vertical space at top/bottom of image where legend text will not be drawn
		static readonly SizeF legendBounds = new SizeF(legendWidth, mapDimension - (legendYPadding * 2)); // Used for MeasureString to calculate legend string dimensions

		// Map marker nudge
		// Adjust map markers away from their true coordinate so that they're instead more like in-game
		// In *pixels*
		const float markerNudgeX = 2f;
		const float markerNudgeY = 6f;
		const float markerNudgeScale = 1.005f;

		// Font and text
		public const int legendFontSize = 48;
		public const int mapLabelFontSize = 18;
		public const int titleFontSize = 72;
		const int fontDropShadowOffset = 3;
		const int mapLabelMaxWidth = 150; // Maximum width before a map marker label will enter a new line
		const int mapLabelBuffer = 10; // Arbitrary number of pixels extra allowed to buffer labels for weird edge cases in 32-bit deployments where label strings are truncated despite MeasureString thinking they'll fit.
		const int warningTextHeight = 300; // height in px off the bottom of image that red warning text is written.

		static readonly Brush dropShadowBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
		static readonly Brush brushWhite = new SolidBrush(Color.White);
		static readonly Brush brushRed = new SolidBrush(Color.Red);
		static readonly PrivateFontCollection fontCollection = IOManager.LoadFont();
		static readonly Font mapLabelFont = new Font(fontCollection.Families[0], mapLabelFontSize, GraphicsUnit.Pixel);
		static readonly Font legendFont = new Font(fontCollection.Families[0], legendFontSize, GraphicsUnit.Pixel);
		static readonly Font titleFont = new Font(fontCollection.Families[0], titleFontSize, FontStyle.Bold, GraphicsUnit.Pixel);
		static readonly RectangleF infoTextBounds = new RectangleF(legendIconX, 0, mapDimension - legendIconX, mapDimension);
		static readonly RectangleF warningTextBounds = new RectangleF(legendIconX, 0, mapDimension - legendIconX, mapDimension - warningTextHeight);
		static readonly RectangleF titleTextBounds = new RectangleF(2200, 0, 1730, mapDimension);

		static readonly RectangleF infoTextDropShadowBounds = GeometryHelper.OffsetRect(infoTextBounds, fontDropShadowOffset);
		static readonly RectangleF titleTextDropShadowBounds = GeometryHelper.OffsetRect(titleTextBounds, fontDropShadowOffset);

		static readonly StringFormat stringFormatBottomRight = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far }; // Align the text bottom-right
		static readonly StringFormat stringFormatBottomCenter = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far }; // Align the text bottom-center
		static readonly StringFormat stringFormatBottomLeft = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Far }; // Align the text bottom-left
		static readonly StringFormat stringFormatTopRight = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near }; // Align the text top-right
		static readonly StringFormat stringFormatTopCenter = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near }; // Align the text top-right
		static readonly StringFormat stringFormatCenter = new StringFormat() { Alignment = StringAlignment.Center }; // Align the text centrally

		// Volume plots
		public const byte volumeOpacity = 128;
		public const float minVolumeDimension = 15f; // Minimum X or Y dimension in pixels (Those smaller are blown up to this dimension)

		// Region plots
		public const byte regionOpacity = 32;
		const int regionEdgeThickness = 5;

		static Image finalImage;
		static Image backgroundLayer;

		public static Image GetImage()
		{
			return finalImage;
		}

		// Flip the Y-Axis (game-space coord to image-space coords)
		public static float CorrectAxis(int coord, bool isYAxis)
		{
			return isYAxis ? -coord : coord;
		}

		// Scales a coordinate down from world- to image-space coordiates, given the variables of the currently selected Space
		public static float ScaleCoordinateToSpace(float coord, bool isYAxis)
		{
			Space currentSpace = SettingsSpace.GetSpace();
			coord += isYAxis ? currentSpace.yOffset : currentSpace.xOffset;
			coord = ((coord - (mapDimension / 2)) * currentSpace.scale * currentSpace.nudgeScale) + (mapDimension / 2);

			coord += isYAxis ? currentSpace.nudgeY : currentSpace.nudgeX;

			return coord;
		}

		// Adjusts the map markers away from their "true" coordinates, to account for some error which causes them to be slightly different in-game
		public static float NudgeMapMarker(float coord, bool isYAxis)
		{
			coord += isYAxis ? markerNudgeY : markerNudgeX;
			coord = ((coord - (mapDimension / 2)) * markerNudgeScale) + (mapDimension / 2);

			return coord;
		}

		// Finalise the map draw, displaying the given final image
		static void FinishDraw(Image finalImage)
		{
			FormMaster.SetMapImage(finalImage);
			FormMaster.UpdateProgressBar(0);
			GC.Collect();
		}

		static void CancelDraw()
		{
			FormMaster.SetMapImage(backgroundLayer);
			FormMaster.UpdateProgressBar(0, "Cancelled");
			GC.Collect();
		}

		// Construct the map background layer, without plotted points
		public static void DrawBaseLayer(CancellationToken cancToken)
		{
			FormMaster.UpdateProgressBar(0, "Rendering background...");

			// Start with the basic image
			backgroundLayer = IOManager.GetImageForSpace(SettingsSpace.GetSpace());

			Graphics graphic = Graphics.FromImage(backgroundLayer);

			// Overlay the water mask if required
			if (SettingsMap.highlightWater && SettingsSpace.CurrentSpaceIsAppalachia())
			{
				graphic.DrawImage(IOManager.GetImageAppalachiaWaterMask(), new Point(0, 0));
			}

			// Assign suitable ColorMatrix attribute given user selected Brightness and Grayscale state
			ImageAttributes attributes = new ImageAttributes();
			attributes.SetColorMatrix(ImageHelper.GenerateColorMatrix(SettingsMap.brightness / 100f, SettingsMap.grayScale));

			Point[] points =
			{
				new Point(0, 0),
				new Point(mapDimension, 0),
				new Point(0, mapDimension),
			};
			Rectangle rect = new Rectangle(0, 0, mapDimension, mapDimension);

			DrawMapMarkers(graphic);

			FormMaster.UpdateProgressBar(0.5, "Rendering background...");

			graphic.DrawImage(backgroundLayer, points, rect, GraphicsUnit.Pixel, attributes);

			Draw(cancToken); // Redraw the whole map since we updated the base layer
		}

		// Draw a height-color key in Topography mode
		static void DrawTopographicKey(Graphics graphic)
		{
			// If there is no need to draw the key
			if (!SettingsPlot.IsTopographic() || FormMaster.GetNonRegionLegendItems().Count == 0)
			{
				return;
			}

			// Identify the sizing and locations for drawing the height-color-key strings
			double numHeightKeys = SettingsPlotTopograph.heightKeyIndicators;
			Font topographFont = new Font(fontCollection.Families[0], 62, GraphicsUnit.Pixel);
			float singleLineHeight = graphic.MeasureString(SettingsPlotTopograph.heightKeyString, topographFont, new SizeF(infoTextBounds.Width, infoTextBounds.Height)).Height;

			// Identify the lower limit to start printing the key so that it ends up centered
			double baseHeight = (mapDimension / 2) - (singleLineHeight * (numHeightKeys / 2d));

			for (int i = 0; i <= numHeightKeys - 1; i++)
			{
				Brush brush = new SolidBrush(GetTopographColor(i / (numHeightKeys - 1)));
				graphic.DrawString(SettingsPlotTopograph.heightKeyString, topographFont, brush, new RectangleF(topographKeyX, 0, mapDimension - topographKeyX, (float)(mapDimension - baseHeight)), stringFormatBottomRight);
				baseHeight += singleLineHeight;
			}
		}

		// Construct the final map by drawing plots over the background layer
		public static void Draw(CancellationToken cancToken)
		{
			// Start progress bar off at 0
			FormMaster.UpdateProgressBar(0, "Beginning draw...");

			// Reset the current image to the background layer
			finalImage = new Bitmap(backgroundLayer);
			Graphics imageGraphic = Graphics.FromImage(finalImage);
			imageGraphic.SmoothingMode = SmoothingMode.AntiAlias;
			imageGraphic.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

			// Draw the bottom-right info/watermark paragraph and title
			DrawTitle(imageGraphic);
			DrawInfoWatermark(imageGraphic);

			// Nothing else to plot - ensure we update for the background layer but then return
			if (FormMaster.GetAllLegendItems().Count == 0)
			{
				FinishDraw(finalImage);
				return;
			}

			if (SettingsPlot.IsTopographic())
			{
				DrawTopographicKey(imageGraphic);
			}

			DrawRegions(FormMaster.GetRegionLegendItems(), imageGraphic);

			// Count how many Map Data Points are due to be mapped
			int totalMapDataPoints = FormMaster.GetAllLegendItems().Sum(mapItem => mapItem.count);

			// Loop through every MapDataPoint represented by all the MapItems to find the min/max z coord in the dataset
			bool first = true;
			int zMin = 0;
			int zMax = 0;
			double zRange = 0;
			if (SettingsPlot.IsTopographic())
			{
				foreach (MapItem mapItem in FormMaster.GetNonRegionLegendItems())
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

				zMin = Math.Max(zMin, SettingsPlotTopograph.zThreshLower);
				zMax = Math.Min(zMax, SettingsPlotTopograph.zThreshUpper);

				zRange = Math.Abs(zMax - zMin);

				if (zRange == 0)
				{
					zRange = 1;
				}
			}

			if (SettingsPlot.IsIconOrTopographic())
			{
				float progress = 0;

				// Processing each MapItem in serial, draw plots for every matching valid MapDataPoint
				foreach (MapItem mapItem in FormMaster.GetNonRegionLegendItems())
				{
					FormMaster.UpdateProgressBar("Plotting " + mapItem.editorID + "...");

					// Generate a Plot Icon and colours/brushes to be used for all instances of the MapItem
					PlotIcon plotIcon = mapItem.GetIcon();
					Image plotIconImg = SettingsPlot.IsIcon() ? plotIcon.GetIconImage() : null; // Icon mode has icon per MapItem, Topography needs icons per MapDataPoint and will be generated later
					Color volumeColor = Color.FromArgb(volumeOpacity, plotIcon.color);
					Brush volumeBrush = new SolidBrush(volumeColor);

					// Iterate over every data point and draw it
					foreach (MapDataPoint point in mapItem.GetPlots())
					{
						if (cancToken.IsCancellationRequested)
						{
							CancelDraw();
							return;
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

							if (z < zMin || z > zMax)
							{
								continue;
							}

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

						// If we should draw this as a volume
						if (SettingsPlot.drawVolumes && point.QualifiesForVolumeDraw())
						{
							Image volumeImage = new Bitmap((int)Math.Round(point.boundX), (int)Math.Round(point.boundY));
							Graphics volumeGraphic = Graphics.FromImage(volumeImage);
							volumeGraphic.SmoothingMode = SmoothingMode.AntiAlias;

							switch (point.primitiveShape)
							{
								case "Box":
								case "Line":
								case "Plane":
									volumeGraphic.FillRectangle(volumeBrush, new RectangleF(0, 0, (float)Math.Round(point.boundX), (float)Math.Round(point.boundY)));
									break;
								case "Sphere":
								case "Ellipsoid":
									volumeGraphic.FillEllipse(volumeBrush, new RectangleF(0, 0, (float)Math.Round(point.boundX), (float)Math.Round(point.boundY)));
									break;
								default:
									throw new NotSupportedException($"Shape \"{point.primitiveShape}\" is unsupported.");
							}

							volumeImage = ImageHelper.RotateImage(volumeImage, point.rotationZ);
							imageGraphic.DrawImage(volumeImage, (float)(point.x - (volumeImage.Width / 2)), (float)(point.y - (volumeImage.Height / 2)));

							// Run GC if we just drew a very large volume
							if (point.GetVolumeBounds() > volumeGCThreshold)
							{
								GC.Collect();
							}
						}

						// This MapDataPoint is not suitable to be drawn as a volume - draw a normal plot icon, or topographic plot
						else
						{
							// Draw the plot icon
							imageGraphic.DrawImage(plotIconImg, (float)(point.x - (plotIconImg.Width / 2d)), (float)(point.y - (plotIconImg.Height / 2d)));

							// Optionally label the Form ID of the MapDataPoint
							if (SettingsPlot.labelInstanceIDs && mapItem.type == MapItem.Type.Standard)
							{
								string text = point.instanceFormID;

								SizeF textBounds = imageGraphic.MeasureString(text, mapLabelFont);

								RectangleF textBox = new RectangleF(
										point.x - (textBounds.Width / 2),
										point.y + (plotIconImg.Height / 2),
										textBounds.Width,
										textBounds.Height);

								// Use the 6 least significant fiures of the hex formid to define the color, override the alpha to 255
								Color color = ImageHelper.GetColorFromText(string.Concat("FF", text.AsSpan(2)));
								imageGraphic.DrawString(text, mapLabelFont, new SolidBrush(color), textBox, stringFormatTopCenter);
                            }
						}
					}

					// Increment the progress bar per MapItem
					progress += mapItem.count;
					FormMaster.UpdateProgressBar(progress / totalMapDataPoints);
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

				float progress = 0;

				foreach (MapItem mapItem in FormMaster.GetNonRegionLegendItems())
				{
					int heatmapLegendGroup = SettingsPlotHeatmap.IsDuo() ? mapItem.legendGroup % 2 : 0;

					FormMaster.UpdateProgressBar($"Evaluating weights of {mapItem.editorID}...");

					foreach (MapDataPoint point in mapItem.GetPlots())
					{
						if (cancToken.IsCancellationRequested)
						{
							CancelDraw();
							return;
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
								double distanceSqrd = ((squareX - x) * (squareX - x)) + ((squareY - y) * (squareY - y));

								// Weight and hence brightness is modified by 1/x^2 + 1 where x is the distance from actual item
								double additionalWeight = point.weight * (1d / (distanceSqrd + 1));
								squares[x, y].weights[heatmapLegendGroup] += additionalWeight;
							}
						}
					}

					progress += mapItem.count;
					FormMaster.UpdateProgressBar(progress / totalMapDataPoints);
				}

				FormMaster.UpdateProgressBar("Rendering weighted squares...");

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

					for (int y = 0; y < resolution; y++)
					{
						if (cancToken.IsCancellationRequested)
						{
							CancelDraw();
							return;
						}

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

				FormMaster.UpdateProgressBar(0.1, "Enumerating coordinate points...");

				// Unroll all MapDataPoints
				foreach (MapItem mapItem in FormMaster.GetNonRegionLegendItems())
				{
					if (cancToken.IsCancellationRequested)
					{
						CancelDraw();
						return;
					}

					points.AddRange(mapItem.GetPlots());
				}

				FormMaster.UpdateProgressBar(0.25, "Defining Clusters...");

				// First pass. Top-level - take a datapoint and make a new cluster if it's so-far not a member
				// Then go round all unadopted points and see if they're suitable to be a member
				// This generates an imperfect but good start for clustering points together
				foreach (MapDataPoint outerPoint in points)
				{
					if (cancToken.IsCancellationRequested)
					{
						CancelDraw();
						return;
					}

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

						// Pythagoras
						float xDist = innerPoint.x - outerPoint.x;
						float yDist = innerPoint.y - outerPoint.y;
						double totalDistSqrd = (xDist * xDist) + (yDist * yDist);

						if (totalDistSqrd < SettingsPlotCluster.clusterRange * SettingsPlotCluster.clusterRange)
						{
							cluster.AddMember(innerPoint);
						}
					}
				}

				FormMaster.UpdateProgressBar(0.5, "Refining Clusters...");

				// Second pass - Now the clusters have been defined, verify each point is adopted by its closest cluster, otherwise move it to correct one
				foreach (MapDataPoint point in points)
				{
					if (cancToken.IsCancellationRequested)
					{
						CancelDraw();
						return;
					}

					MapCluster closestCluster = clusters.MinBy(cluster =>
					{
						// Pythagoras
						float a = point.x - cluster.polygon.initialPoint.X;
						float b = point.y - cluster.polygon.initialPoint.Y;
						return (a * a) + (b * b);
					});

					// Move point to the nearest cluster
					if (point.GetParentCluster() != closestCluster)
					{
						point.LeaveCluster();
						closestCluster.AddMember(point);
					}
				}

				FormMaster.UpdateProgressBar(0.8, "Rendering clusters...");
				DrawClusters(clusters, imageGraphic);
			}

			// Create a new wider bitmap with graphic, then print the legend to that and store it as the final image
			if (SettingsMap.ExtendedMargin())
			{
				// Reset the current image to the background layer
				Bitmap expandedImage = new Bitmap(mapDimension + plotXMin, mapDimension);
				Graphics expadedImageGraphic = Graphics.FromImage(expandedImage);
				expadedImageGraphic.SmoothingMode = SmoothingMode.AntiAlias;
				expadedImageGraphic.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

				expadedImageGraphic.DrawImage(finalImage, new Point(plotXMin, 0));

				// Draw all legend text for every MapItem
				DrawLegend(legendFont, expadedImageGraphic);
				finalImage = expandedImage;
			}

			// Draw the legend within the square map
			else
			{
				DrawLegend(legendFont, imageGraphic);
			}

			FinishDraw(finalImage);
		}

		static void DrawRegions(List<MapItem> items, Graphics imageGraphic)
		{
			foreach (MapItem item in items)
			{
				Region region = item.GetRegion();

				Color color = item.GetLegendColor();
				Pen pen = new Pen(color, regionEdgeThickness);

				Color alphaColor = Color.FromArgb(regionOpacity, color.R, color.G, color.B);
				Brush brush = new SolidBrush(alphaColor);

				for (int i = 0; i < region.GetSubRegionCount(); i++)
				{
					List<PointF> rawPoints = new List<PointF>();

					foreach (RegionPoint point in region.GetSubRegionPoints(i))
					{
						rawPoints.Add(new PointF(
							ScaleCoordinateToSpace(CorrectAxis(point.x, false), false),
							ScaleCoordinateToSpace(CorrectAxis(point.y, true), true)));
					}

					PointF[] points = rawPoints.ToArray();
					imageGraphic.DrawPolygon(pen, points);

					if (SettingsPlot.fillRegions)
					{
						imageGraphic.FillPolygon(brush, points);
					}
				}
			}
		}

		static void DrawClusters(List<MapCluster> clusters, Graphics imageGraphic)
		{
			if (clusters.Count == 0)
			{
				return;
			}

			Pen clusterPolygonPen = new Pen(SettingsPlotStyle.GetFirstColor(), SettingsPlotCluster.polygonLineThickness);
			Pen clusterWebPen = new Pen(SettingsPlotStyle.GetSecondColor(), SettingsPlotCluster.webLineThickness);

			double largestClusterWeight = clusters.OrderByDescending(c => c.GetMemberWeight()).First().GetMemberWeight();

			// The effective weight cap given we always draw the largest cluster if the min weight was too restrictive
			// This value will either be the users cap, or the weight of the next "heaviest" cluster, for which we're going to force draw
			int localWeightCap = (int)Math.Min(largestClusterWeight, SettingsPlotCluster.minClusterWeight);

			// Drop clusters not within local weight cap
			clusters = clusters.Where(c => c.GetMemberWeight() >= localWeightCap).ToList();

			// If we're resultantly overriding the cap to draw the next best
			if (localWeightCap < SettingsPlotCluster.minClusterWeight)
			{
				string plural = clusters.Count > 1 ? "s" : string.Empty;
				string warning = $"Min. cluster weight too high. Showing next-largest cluster{plural}.";
				imageGraphic.DrawString(warning, legendFont, brushRed, warningTextBounds, stringFormatBottomCenter);
			}

			// Step through clusters and generate the convex hull and centroid
			clusters.ForEach(c => c.GenerateFinalRenderProperties());

			// Draw the cluster shapes
			foreach (MapCluster cluster in clusters)
			{
				if (cluster.finalPolygon.GetVerts().Count > 1)
				{
					imageGraphic.DrawPolygon(clusterPolygonPen, cluster.finalPolygon.GetVerts().ToArray());
				}

				// Draw the 'cluster web'
				if (SettingsPlotCluster.clusterWeb)
				{
					foreach (MapDataPoint point in cluster.members)
					{
						imageGraphic.DrawLine(clusterWebPen, cluster.finalCentroid, point.Get2DPoint());
					}
				}
			}

			// Now label the cluster weights
			foreach (MapCluster cluster in clusters)
			{
				string weight = Math.Round(cluster.GetMemberWeight(), 1).ToString();
				int fontSize = Math.Max(Math.Min(SettingsPlotCluster.fontSize, SettingsPlotCluster.clusterRange), SettingsPlotCluster.minFontSize);
				Font font = new Font(fontCollection.Families[0], fontSize, GraphicsUnit.Pixel);

				SizeF textBounds = imageGraphic.MeasureString(weight, font, new SizeF(256, 128));

				RectangleF textBox = new RectangleF(
						cluster.finalCentroid.X - (textBounds.Width / 2),
						cluster.finalCentroid.Y - (textBounds.Height / 2),
						textBounds.Width,
						textBounds.Height);

				RectangleF textBoxDropShadow = new RectangleF(
						textBox.X + fontDropShadowOffset,
						textBox.Y + fontDropShadowOffset,
						textBox.Width,
						textBox.Height);

				imageGraphic.DrawString(weight, font, dropShadowBrush, textBoxDropShadow, stringFormatCenter);
				imageGraphic.DrawString(weight, font, SettingsPlotCluster.weightBrush, textBox, stringFormatCenter);
			}

			GC.Collect();
		}

		static void DrawInfoWatermark(Graphics imageGraphic)
		{
			imageGraphic.TextRenderingHint = TextRenderingHint.AntiAlias;
			Space currentSpace = SettingsSpace.GetSpace();

			string infoText = $"Game version {IOManager.GetGameVersion()} | Made with Mappalachia - github.com/AHeroicLlama/Mappalachia";

			if (!currentSpace.IsWorldspace())
			{
				infoText = $"{currentSpace.displayName} ({currentSpace.editorID}), Scale 1:{Math.Round(currentSpace.scale * currentSpace.nudgeScale, 2)}" +
					((SettingsSpace.minHeightPerc != 0 || SettingsSpace.maxHeightPerc != 100) ?
						$", Height: {SettingsSpace.minHeightPerc}% - {SettingsSpace.maxHeightPerc}%" : string.Empty) +
						"\n" + infoText;
			}

			if (SettingsPlot.IsCluster() && FormMaster.GetAllLegendItems().Count > 0)
			{
				string weightString = SettingsPlotCluster.minClusterWeight > 1 ? $" weight {SettingsPlotCluster.minClusterWeight}+," : string.Empty;
				string separator = SettingsSpace.CurrentSpaceIsWorld() ? "\n" : " | ";
				infoText = $"(Clusters of{weightString} range {SettingsPlotCluster.clusterRange}px){separator}" + infoText;
			}

			imageGraphic.DrawString(infoText, legendFont, dropShadowBrush, infoTextDropShadowBounds, stringFormatBottomRight);
			imageGraphic.DrawString(infoText, legendFont, brushWhite, infoTextBounds, stringFormatBottomRight);
		}

		// Draw the user-defined title of the map
		static void DrawTitle(Graphics imageGraphic)
		{
			string title = SettingsMap.title;

			if (string.IsNullOrWhiteSpace(title))
			{
				return;
			}

			title = $"\"{title}\"";

			imageGraphic.TextRenderingHint = TextRenderingHint.AntiAlias;

			imageGraphic.DrawString(title, titleFont, dropShadowBrush, titleTextDropShadowBounds, stringFormatTopRight);
			imageGraphic.DrawString(title, titleFont, brushWhite, titleTextBounds, stringFormatTopRight);
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

					if (SettingsMap.grayScaleMapIcons)
					{
						markerImage = ImageHelper.AdjustBrightnessOrGrayscale(markerImage, 1, true);
					}

					imageGraphic.DrawImage(markerImage, (int)(marker.x - (markerImage.Width / 2)), (int)(marker.y - (markerImage.Height / 2)));
				}
			}

			if (SettingsMap.showMapLabels)
			{
				// Now draw map marker labels on top
				imageGraphic.TextRenderingHint = TextRenderingHint.AntiAlias;
				foreach (MapMarker marker in markers)
				{
					SizeF textBounds = imageGraphic.MeasureString(marker.label, mapLabelFont, new SizeF(mapLabelMaxWidth - mapLabelBuffer, mapLabelMaxWidth - mapLabelBuffer));

					float labelHeightOffset = SettingsMap.showMapIcons ?
						IOManager.GetMapMarker(marker.markerName).Height / 2f :
						-textBounds.Height / 2f;

					RectangleF textBox = new RectangleF(
							(float)marker.x - (textBounds.Width / 2) - (mapLabelBuffer / 2),
							(float)marker.y + labelHeightOffset,
							textBounds.Width + mapLabelBuffer, textBounds.Height);

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
		static void DrawLegend(Font font, Graphics imageGraphic)
		{
			if (FormMaster.GetAllLegendItems().Count == 0 || SettingsMap.HiddenMargin())
			{
				return;
			}

			Dictionary<int, string> overridingLegendText = FormMaster.GatherOverriddenLegendTexts();
			List<int> drawnGroups = new List<int>();

			// Calculate the total height of all legend strings with their plot icons beside, combined
			int legendTotalHeight = 0;
			foreach (MapItem mapItem in FormMaster.GetAllLegendItems())
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
			foreach (MapItem mapItem in FormMaster.GetAllLegendItems())
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

				Color legendColor = mapItem.GetLegendColor();
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
					if (SettingsPlot.IsIconOrTopographic() && !mapItem.IsRegion())
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

			// Adds additional text if some items were missed from legend
			if (skippedLegends > 0)
			{
				string extraLegendText = "+" + skippedLegends + " more item" + (skippedLegends == 1 ? string.Empty : "s") + "...";
				imageGraphic.DrawString(extraLegendText, legendFont, brushWhite, infoTextBounds, stringFormatBottomLeft);
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
			IOManager.QuickSaveImage(IOManager.OpenImageMode.TempSaveInViewer);
		}
	}
}
