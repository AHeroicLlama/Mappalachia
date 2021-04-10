using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;
using Mappalachia.Class;

namespace Mappalachia
{
	//The Map image, adjusting it, drawing it and plotting points onto it
	public static class Map
	{
		//Map-image coordinate scaling. Done manually by eye with reference points from in-game
		public static double scaling = 142;
		public static double xOffset = 1.7;
		public static double yOffset = 5.2;

		//Hidden settings
		public static readonly int fontSize = 36;
		public static readonly int mapDimension = 4096; //All layer images should be this^2
		public static readonly int maxZoom = (int)(mapDimension * 2.0);
		public static readonly int minZoom = (int)(mapDimension * 0.05);

		//Legend text positioning
		static readonly int legendIconX = 141; //The X Coord of the plot icon that is drawn next to each legend string
		public static readonly int plotXMin = 650; //Number of pixels in from the left of the map image where the player cannot reach
		static readonly int plotXMax = 3610;
		static readonly int plotYMin = 508;
		static readonly int plotYMax = 3382;
		static readonly int legendXMin = 220; //The padding in the from the left where legend text begins
		static readonly int legendWidth = plotXMin - legendXMin; //The resultant width (or length) of legend text rows in pixels
		static readonly SizeF legendBounds = new SizeF(legendWidth, mapDimension); //Used for MeasureString to calculate legend string dimensions

		//Volume plots
		public static readonly int volumeOpacity = 128;
		public static readonly uint minVolumeDimension = 8; //Minimum X or Y dimension in pixels below which a volume will use a plot icon instead
		public static readonly List<string> supportedVolumeShapes = new List<string> { "Box", "Line", "Plane", "Sphere", "Ellipsoid", };

		//Legend Font
		static readonly PrivateFontCollection fontCollection = IOManager.LoadFont();

		//Reference to map picture box
		static PictureBox mapFrame;

		//Reference to progress bar
		public static ProgressBar progressBarMain;

		static Image finalImage;
		static Image backgroundLayer;

		//Returns the result of Pythagoras theorem on 2 integers
		static double Pythagoras(int a, int b)
		{
			return Math.Sqrt((a * a) + (b * b));
		}

		//Attach the map image to the PictureBox on the master form
		public static void SetOutput(PictureBox pictureBox)
		{
			mapFrame = pictureBox;
		}

		//Construct the map background layer, without plotted points
		public static void DrawBaseLayer()
		{
			//Start with the chosen base map
			if (SettingsMap.IsCellModeActive())
			{
				backgroundLayer = new Bitmap(mapDimension, mapDimension);

				if (SettingsCell.drawOutline)
				{
					Graphics backgroundGraphics = Graphics.FromImage(backgroundLayer);
					DrawCellBackground(backgroundGraphics);
				}
			}
			else
			{
				backgroundLayer = SettingsMap.layerMilitary ?
					IOManager.GetImageMapMilitary() :
					IOManager.GetImageMapNormal();
			}

			Graphics graphic = Graphics.FromImage(backgroundLayer);

			//Add the Nuclear Winter layers if selected
			if (SettingsMap.layerNWMorgantown)
			{
				Image morgantownLayer = IOManager.GetImageLayerNWMorgantown();
				graphic.DrawImage(morgantownLayer, new Point(0, 0));
			}

			if (SettingsMap.layerNWFlatwoods)
			{
				Image flatwoodsLayer = IOManager.GetImageLayerNWFlatwoods();
				graphic.DrawImage(flatwoodsLayer, new Point(0, 0));
			}

			//Apply brightness adjustment and grayscale if selected
			float b = SettingsMap.brightness / 100f;
			ColorMatrix matrix;

			if (SettingsMap.grayScale)
			{
				matrix = new ColorMatrix(new float[][]
				{
					new float[] { 0.299f * b, 0.299f * b, 0.299f * b, 0, 0 },
					new float[] { 0.587f * b, 0.587f * b, 0.587f * b, 0, 0 },
					new float[] { 0.114f * b, 0.114f * b, 0.114f * b, 0, 0 },
					new float[] { 0, 0, 0, 1, 0 },
					new float[] { 0, 0, 0, 0, 1 },
				});
			}
			else
			{
				matrix = new ColorMatrix(new float[][]
				{
					new float[] { b, 0, 0, 0, 0 },
					new float[] { 0, b, 0, 0, 0 },
					new float[] { 0, 0, b, 0, 0 },
					new float[] { 0, 0, 0, 1, 0 },
					new float[] { 0, 0, 0, 0, 1 },
				});
			}

			ImageAttributes attributes = new ImageAttributes();
			attributes.SetColorMatrix(matrix);

			Point[] points =
			{
				new Point(0, 0),
				new Point(mapDimension, 0),
				new Point(0, mapDimension),
			};
			Rectangle rect = new Rectangle(0, 0, mapDimension, mapDimension);

			graphic.DrawImage(backgroundLayer, points, rect, GraphicsUnit.Pixel, attributes);

			Draw(); //Redraw the whole map since we updated the base layer
		}

		//Construct the final map by drawing plots over the background layer
		public static void Draw()
		{
			//Reset the current image to the background layer
			finalImage = (Image)backgroundLayer.Clone();

			Graphics imageGraphic = Graphics.FromImage(finalImage);
			Font font = new Font(fontCollection.Families[0], fontSize, GraphicsUnit.Pixel);

			CellScaling cellScaling = null;

			//Prepare the game version and watermark to be printed later
			string infoText = "Game version " + AssemblyInfo.gameVersion + "\nMade with Mappalachia - github.com/AHeroicLlama/Mappalachia";

			//Additional steps for cell mode (Add further text to watermark text, get cell height boundings)
			if (SettingsMap.IsCellModeActive())
			{
				Cell currentCell = SettingsCell.GetCell();

				//Assign the CellScaling property - also used later in GenerateIconPlotLayer()
				cellScaling = currentCell.GetScaling();

				infoText =
					currentCell.displayName + " (" + currentCell.editorID + ")\n" +
					"Height distribution: " + SettingsCell.minHeightPerc + "% - " + SettingsCell.maxHeightPerc + "%\n" +
					"Scale: 1:" + Math.Round(cellScaling.scale, 2) + "\n\n" +
					infoText;
			}

			//Gather resources for drawing informational watermark text
			Brush brushWhite = new SolidBrush(Color.White);
			RectangleF infoTextBounds = new RectangleF(plotXMin, 0, mapDimension - plotXMin, mapDimension);
			StringFormat stringFormatBottomRight = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far }; //Align the text bottom-right
			StringFormat stringFormatBottomLeft = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Far }; //Align the text bottom-left
			
			//Draws bottom-right info text
			imageGraphic.DrawString(infoText, font, brushWhite, infoTextBounds, stringFormatBottomRight);

			//Draw all legend text for every MapItem
			int skippedLegends = DrawLegend(font, imageGraphic);

			//Adds additional text if some items were missed from legend
			if (skippedLegends > 0)
			{
				string extraLegendText = "+" + skippedLegends + " more item" + (skippedLegends == 1 ? string.Empty : "s") + "...";
				imageGraphic.DrawString(extraLegendText, font, brushWhite, infoTextBounds, stringFormatBottomLeft);
			}

			//Start progress bar off at 0
			progressBarMain.Value = 0;
			float totalMapItems = FormMaster.legendItems.Count;
			float progress = 0;

			//Nothing else to plot - ensure we update for the background layer but then return
			if (FormMaster.legendItems.Count == 0)
			{
				mapFrame.Image = finalImage;
				return;
			}

			if (SettingsPlot.IsIcon())
			{
				//Processing each MapItem in serial, draw plots for every matching valid MapDataPoint
				foreach (MapItem mapItem in FormMaster.legendItems)
				{
					PlotIcon plotIcon = mapItem.GetIcon();
					Image plotIconImg = plotIcon.GetIconImage();

					Color volumeColor = Color.FromArgb(volumeOpacity, plotIcon.color);
					Brush volumeBrush = new SolidBrush(volumeColor);

					//Iterate over every data point and draw it
					foreach (MapDataPoint point in mapItem.GetPlots())
					{
						if (SettingsMap.IsCellModeActive())
						{
							//If this coordinate exceeds the user-selected cell mapping height bounds, skip it
							//(Also accounts for the z-height of volumes)
							if (point.z + (point.boundZ / 2d) < SettingsCell.GetMinHeightCoordBound() || point.z - (point.boundZ / 2d) > SettingsCell.GetMaxHeightCoordBound())
							{
								continue;
							}

							point.x += cellScaling.xOffset;
							point.y += cellScaling.yOffset;

							//Multiply the coordinates by the scaling, but multiply around 0,0
							point.x = ((point.x - (mapDimension / 2)) * cellScaling.scale) + (mapDimension / 2);
							point.y = ((point.y - (mapDimension / 2)) * cellScaling.scale) + (mapDimension / 2);
							point.boundX *= cellScaling.scale;
							point.boundY *= cellScaling.scale;
						}
						else //Skip the point if its origin is outside the surface world
						if (point.x < plotXMin || point.x >= plotXMax || point.y < plotYMin || point.y >= plotYMax)
						{
							continue;
						}

						//If this meets all the criteria to be suitable to be drawn as a volume
						if (point.primitiveShape != string.Empty && //This is a primitive shape at all
							SettingsPlot.drawVolumes && //Volume drawing is enabled
							supportedVolumeShapes.Contains(point.primitiveShape) && //This volume shape is supported
							point.boundX >= minVolumeDimension && point.boundY >= minVolumeDimension) //This is large enough to be visible if drawn as a volume
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
									continue; //This *shouldn't* be reached, given that supportedVolumeShapes is maintained
							}

							volumeImage = ImageTools.RotateImage(volumeImage, point.rotationZ);
							imageGraphic.DrawImage(volumeImage, (float)(point.x - (volumeImage.Width / 2)), (float)(point.y - (volumeImage.Height / 2)));
						}
						else //This MapDataPoint is not suitable to be drawn as a volume - draw a normal plot icon
						{
							imageGraphic.DrawImage(plotIconImg, (float)(point.x - (plotIconImg.Width / 2d)), (float)(point.y - (plotIconImg.Height / 2d)));
						}
					}

					//Increment the progress bar per MapItem
					progress += 1;
					progressBarMain.Value = (int)((progress / totalMapItems) * progressBarMain.Maximum);
					Application.DoEvents();
				}
			}
			else if (SettingsPlot.IsHeatmap())
			{
				int resolution = SettingsPlotHeatmap.resolution;
				int blendRange = SettingsPlotHeatmap.blendDistance;

				//Create a 2D Array of HeatMapGridSquare
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
						if (SettingsMap.IsCellModeActive())
						{
							point.x += cellScaling.xOffset;
							point.y += cellScaling.yOffset;

							point.x = ((point.x - (mapDimension / 2)) * cellScaling.scale) + (mapDimension / 2);
							point.y = ((point.y - (mapDimension / 2)) * cellScaling.scale) + (mapDimension / 2);
						}

						//Identify which grid square this MapDataPoint falls within
						int squareX = (int)Math.Floor(point.x / pixelsPerSquare);
						int squareY = (int)Math.Floor(point.y / pixelsPerSquare);

						//Loop over every grid square within range, and increment by the weight proportional to the distance
						for (int x = squareX - blendRange; x < squareX + blendRange; x++)
						{
							for (int y = squareY - blendRange; y < squareY + blendRange; y++)
							{
								//Don't try to target squares which would lay outside of the grid
								if (x < 0 || x >= resolution || y < 0 || y >= resolution)
								{
									continue;
								}

								//Pythagoras on the x and y dist gives us the 'as the crow flies' distance between the squares
								double distance = Pythagoras(squareX - x, squareY - y);

								//Console.WriteLine((squareX - x) + ", " + (squareY - y));

								//Weight and hence brightness is modified by 1/x^2 + 1 where x is the distance from actual item
								double additionalWeight = point.weight * (1d / ((distance * distance) + 1));
								squares[x, y].weights[heatmapLegendGroup] += additionalWeight;
							}
						}
					}

					//Increment the progress bar per MapItem
					progress += 1;
					progressBarMain.Value = (int)((progress / totalMapItems) * progressBarMain.Maximum);
					Application.DoEvents();
				}

				//Find the largest weight value of all squares
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

				//Finally now weights are calculated, draw a square for every HeatGripMapSquare in the array
				for (int x = 0; x < resolution; x++)
				{
					int xCoord = x * pixelsPerSquare;

					//Don't draw grid squares which are entirely within the legend text area
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

			mapFrame.Image = finalImage;
			GC.Collect();
		}

		//Draws all legend text (and optional Icon beside) for every MapItem
		//Returns the number of items missed off the legend due to size constraints
		static int DrawLegend(Font font, Graphics imageGraphic)
		{
			Dictionary<int, string> overridingLegendText = LegendTextManager.GetOverriddenTexts();
			List<int> drawnGroups = new List<int>();

			//Calculate the total height of all legend strings with their plot icons beside, combined
			int legendTotalHeight = 0;
			foreach (MapItem mapItem in FormMaster.legendItems)
			{
				//Skip legend groups that are merged/overridden and have already been accounted for
				if (drawnGroups.Contains(mapItem.legendGroup) && overridingLegendText.ContainsKey(mapItem.legendGroup))
				{
					continue;
				}

				legendTotalHeight += Math.Max(
					(int)Math.Ceiling(imageGraphic.MeasureString(mapItem.GetLegendText(), font, legendBounds).Height),
					SettingsPlot.IsIcon() ? SettingsPlotIcon.iconSize : 0);

				drawnGroups.Add(mapItem.legendGroup);
			}

			int skippedLegends = 0; //How many legend items did not fit onto the map

			//The initial Y coord where first legend item should be written, in order to Y-center the entire legend
			int legendCaretHeight = (mapDimension / 2) - (legendTotalHeight / 2);

			//Reset the drawn groups list, as we need to iterate over the items again
			drawnGroups = new List<int>();

			//Loop over every MapItem and draw the legend
			foreach (MapItem mapItem in FormMaster.legendItems)
			{
				//Skip legend groups that are merged/overridden and have already been drawn
				if (drawnGroups.Contains(mapItem.legendGroup) && overridingLegendText.ContainsKey(mapItem.legendGroup))
				{
					continue;
				}

				//Calculate positions and color for legend text (plus icon)
				int fontHeight = (int)Math.Ceiling(imageGraphic.MeasureString(mapItem.GetLegendText(), font, legendBounds).Height);

				PlotIcon icon = mapItem.GetIcon();
				Image plotIconImg = SettingsPlot.IsIcon() ? icon.GetIconImage() : null;

				Color legendColor = mapItem.GetLegendColor();
				Brush textBrush = new SolidBrush(legendColor);

				int iconHeight = SettingsPlot.IsIcon() ?
					plotIconImg.Height :
					0;

				int legendHeight = Math.Max(fontHeight, iconHeight);

				//If the icon is taller than the text, offset the text it so it sits Y-centrally against the icon
				int textOffset = 0;
				if (iconHeight > fontHeight)
				{
					textOffset = (iconHeight - fontHeight) / 2;
				}

				//If the legend text/item fits on the map vertically
				if (legendCaretHeight > 0 && legendCaretHeight + legendHeight < mapDimension)
				{
					if (SettingsPlot.IsIcon())
					{
						imageGraphic.DrawImage(plotIconImg, (float)(legendIconX - (plotIconImg.Width / 2d)), (float)(legendCaretHeight - (plotIconImg.Height / 2d) + (legendHeight / 2d)));
					}

					imageGraphic.DrawString(mapItem.GetLegendText(), font, textBrush, new RectangleF(legendXMin, legendCaretHeight + textOffset, legendWidth, legendHeight));
				}
				else
				{
					skippedLegends++;
				}

				drawnGroups.Add(mapItem.legendGroup);
				legendCaretHeight += legendHeight; //Move the 'caret' down for the next item, enough to fit the icon and the text
			}

			return skippedLegends;
		}

		//Draws an outline of all items in the current cell to act as background/template
		static void DrawCellBackground(Graphics backgroundLayer)
		{
			if (!SettingsMap.IsCellModeActive())
			{
				return;
			}

			CellScaling cellScaling = SettingsCell.GetCell().GetScaling();

			List<MapDataPoint> allInCell = DataHelper.GetAllCellCoords(SettingsCell.GetCell().formID);

			int outlineWidth = SettingsCell.outlineWidth;
			int outlineSize = SettingsCell.outlineSize;

			Image plotIconImg = new Bitmap(outlineSize, outlineSize);
			Graphics plotIconGraphic = Graphics.FromImage(plotIconImg);
			plotIconGraphic.SmoothingMode = SmoothingMode.AntiAlias;
			Color outlineColor = Color.FromArgb(SettingsCell.outlineAlpha, SettingsCell.outlineColor);
			Pen outlinePen = new Pen(outlineColor, outlineWidth);
			plotIconGraphic.DrawEllipse(outlinePen, 
				new RectangleF(outlineWidth, outlineWidth, outlineSize - (outlineWidth * 2), outlineSize - (outlineWidth * 2)));

			//Iterate over every data point and draw it
			foreach (MapDataPoint point in allInCell)
			{
				//If this coordinate exceeds the user-selected cell mapping height bounds, skip it
				//(Also accounts for the z-height of volumes)
				if (point.z < SettingsCell.GetMinHeightCoordBound() || point.z > SettingsCell.GetMaxHeightCoordBound())
				{
					continue;
				}

				point.x += cellScaling.xOffset;
				point.y += cellScaling.yOffset;

				//Multiply the coordinates by the scaling, but multiply around 0,0
				point.x = ((point.x - (mapDimension / 2)) * cellScaling.scale) + (mapDimension / 2);
				point.y = ((point.y - (mapDimension / 2)) * cellScaling.scale) + (mapDimension / 2);
				point.boundX *= cellScaling.scale;
				point.boundY *= cellScaling.scale;

				backgroundLayer.DrawImage(plotIconImg, (float)(point.x - (plotIconImg.Width / 2d)), (float)(point.y - (plotIconImg.Height / 2d)));
			}
		}

		public static void Open()
		{
			IOManager.OpenImage(finalImage);
		}

		public static void WriteToFile(string fileName)
		{
			IOManager.WriteToFile(fileName, finalImage);
		}

		//Reset map-specific settings and redraw it
		public static void Reset()
		{
			SettingsMap.brightness = SettingsMap.brightnessDefault;
			SettingsMap.layerMilitary = false;
			SettingsMap.layerNWFlatwoods = false;
			SettingsMap.layerNWMorgantown = false;
			SettingsMap.grayScale = false;

			DrawBaseLayer();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
	}
}
