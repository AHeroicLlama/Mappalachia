using System;
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
		public static double xScale = 142.2;
		public static double YScale = 141.8;
		public static double xOffset = 1.7;
		public static double yOffset = 5.2;

		//Hidden settings
		public static readonly int fontSize = 36;
		public static readonly int mapDimension = 4096; //All layer images should be this^2
		public static readonly int maxZoom = (int)(mapDimension * 2.0);
		public static readonly int minZoom = (int)(mapDimension * 0.05);

		public static readonly int defaultBrightness = 30;

		//Legend text positioning
		static readonly int legendIconX = 141; //The X Coord of the plot icon that is drawn next to each legend string
		static readonly int legendXMax = 650; //Number of pixels in from the left of the map image where the player cannot reach
		static readonly int legendXMin = 220; //The padding in the from the left where legend text begins
		static readonly int legendWidth = legendXMax - legendXMin; //The resultant width (or length) of legend text rows in pixels
		static readonly SizeF legendBounds = new SizeF(legendWidth, mapDimension); //Used for MeasureString to calculate legend string dimensions

		//Volume plots
		public static readonly int volumeOpacity = 128;
		public static readonly int minVolumeArea = 100; //Minimum Area in pixels below which a volume will use a plot icon instead

		//Legend Font
		static readonly PrivateFontCollection fontCollection = IOManager.LoadFont();

		//Reference to map picture box
		static PictureBox mapFrame;

		//Reference to progress bar
		public static ProgressBar progressBarMain;

		static Image finalImage;
		static Image backgroundLayer;

		//Attach the map image to the PictureBox on the master form
		public static void SetOutput(PictureBox pictureBox)
		{
			mapFrame = pictureBox;
		}

		//Construct the map background layer, without plotted points
		public static void DrawBaseLayer()
		{
			//Start with the chosen base map
			backgroundLayer = SettingsMap.layerMilitary ?
				IOManager.GetImageMapMilitary() :
				IOManager.GetImageMapNormal();

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

			//Draw the game version onto the map
			string versionText = "Game version " + AssemblyInfo.gameVersion;
			Brush brushWhite = new SolidBrush(Color.White);
			SizeF versionBounds = new SizeF(mapDimension, mapDimension);
			int versionTextHeight = (int)imageGraphic.MeasureString(versionText, font, versionBounds).Height;
			RectangleF versionTextPosition = new RectangleF(0, mapDimension - versionTextHeight, versionBounds.Width, versionBounds.Height);
			imageGraphic.DrawString(versionText, font, brushWhite, versionTextPosition);

			if (FormMaster.legendItems.Count == 0)
			{
				mapFrame.Image = finalImage;
				return;
			}

			int totalLegendGroups = FormMaster.FindSumLegendGroups();

			if (SettingsPlot.mode == SettingsPlot.Mode.Icon)
			{
				int iconHeight = SettingsPlotIcon.iconSize;
				int colorTotal = SettingsPlotIcon.paletteColor.Count;
				int shapeTotal = SettingsPlotIcon.paletteShape.Count;

				//Calculate the total height of all legend strings with their plot icons beside, combined
				int legendTotalHeight = 0;
				foreach (MapItem mapItem in FormMaster.legendItems)
				{
					legendTotalHeight += Math.Max(
						(int)Math.Ceiling(imageGraphic.MeasureString(mapItem.GetLegendText(), font, legendBounds).Height),
						SettingsPlotIcon.iconSize);
				}

				//The Y coord where first legend item should be written, in order to Y-center the entire legend
				int legendCaretHeight = (mapDimension / 2) - (legendTotalHeight / 2);

				//Provide warnings if we are going to repeat icons, or if the legend text is too big to fit on the map
				if (legendTotalHeight > mapDimension && totalLegendGroups > (colorTotal * shapeTotal))
				{
					NonBlockingNotify.Warn("There are too many items to fit onto the legend, and there are also too few colors and shapes in the palettes to use a unique icon for each item. " +
						"The map will still be drawn, but the legend will extrude the map and icons will be repeated.");
				}
				else if (legendTotalHeight > mapDimension)
				{
					NonBlockingNotify.Warn("There are too many items to fit onto the legend. The map will still be drawn, but the legend will extrude the map.");
				}
				else if (totalLegendGroups > (colorTotal * shapeTotal))
				{
					NonBlockingNotify.Warn("There are too few colors and shapes in the palettes to use a unique icon for each item. The map will still be drawn, but plot icons will be repeated.");
				}

				//Start progress bar off at 0
				progressBarMain.Value = 0;
				float totalMapItems = FormMaster.legendItems.Count;
				float progress = 0;

				//Plot layers generated in serial to keep memory use reasonable
				//Generate a layer for each legend item to be plotted, and draw it over the current image
				foreach (MapItem mapItem in FormMaster.legendItems)
				{
					//Calculate the height in pixels of the legend text plus icon if it were drawn
					int fontHeight = (int)Math.Ceiling(imageGraphic.MeasureString(mapItem.GetLegendText(), font, legendBounds).Height);
					int legendHeight = Math.Max(fontHeight, iconHeight);

					//If the icon is taller than the text, offset the text it so it sits Y-centrally against the icon
					int textOffset = 0;
					if (iconHeight > fontHeight)
					{
						textOffset = (iconHeight - fontHeight) / 2;
					}

					//Identify which item from each palette should be used.
					//First iterate through every color, then every palette, and repeat.
					int colorIndex = mapItem.legendGroup % colorTotal;
					int shapeIndex = (mapItem.legendGroup / colorTotal) % shapeTotal;

					Color color = SettingsPlotIcon.paletteColor[colorIndex];
					PlotIconShape shape = SettingsPlotIcon.paletteShape[shapeIndex];

					//Draw the plot layer
					imageGraphic.DrawImage(GenerateIconPlotLayer(mapItem, color, font, shape, legendCaretHeight, legendHeight, textOffset), 0, 0, mapDimension, mapDimension);

					legendCaretHeight += legendHeight; //Move the caret down for the next item, enough to fit the icon and the text

					GC.Collect();
					GC.WaitForPendingFinalizers();

					//Increment the progress bar per MapItem
					progress += 1;
					progressBarMain.Value = (int)((progress / totalMapItems) * progressBarMain.Maximum);
					Application.DoEvents();
				}
			}
			else if (SettingsPlot.mode == SettingsPlot.Mode.Heatmap)
			{
				int resolution = SettingsPlotHeatmap.resolution;
				int blendRange = SettingsPlotHeatmap.blendDistance;

				//Calculate the total height of all legend strings combined
				int legendTotalHeight = 0;
				foreach (MapItem mapItem in FormMaster.legendItems)
				{
					legendTotalHeight += (int)Math.Ceiling(imageGraphic.MeasureString(mapItem.GetLegendText(), font, legendBounds).Height);
				}

				//The initial Y coord where first legend item should be written, in order to Y-center the entire legend
				int legendCaretHeight = (mapDimension / 2) - (legendTotalHeight / 2);

				if (legendTotalHeight > mapDimension)
				{
					NonBlockingNotify.Warn("There are too many items to fit onto the legend. The map will still be drawn, but the legend will extrude the map.");
				}

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

				//Start the progress bar at 0
				progressBarMain.Value = 0;
				float totalMapItems = FormMaster.legendItems.Count;
				float progress = 0;

				foreach (MapItem mapItem in FormMaster.legendItems)
				{
					int legendGroup = SettingsPlotHeatmap.colorMode == SettingsPlotHeatmap.ColorMode.Duo ? mapItem.legendGroup % 2 : 0;

					foreach (MapDataPoint point in mapItem.GetPlots())
					{
						//Ignore points outside the map
						if (point.x < 0 || point.x >= mapDimension || point.y < 0 || point.y >= mapDimension)
						{
							continue;
						}

						//Identify which grid square this MapDataPoint falls within
						int squareX = (int)Math.Floor(point.x / pixelsPerSquare);
						int squareY = (int)Math.Floor(point.y / pixelsPerSquare);

						//Loop over every grid square within range, and increment by the weight proportional to the distance
						for (int x = squareX - blendRange; x < squareX + blendRange; x++)
						{
							for (int y = squareY - blendRange; y < squareY + blendRange; y++)
							{
								//Drop squares which would lay outside of the grid
								if (x < 0 || x >= resolution || y < 0 || y >= resolution)
								{
									continue;
								}

								//Pythagoras on the x and y dist gives us the 'as the crow flies' distance between the squares
								double distance = Math.Sqrt(
									Math.Pow(Math.Abs(squareX - x), 2) +
									Math.Pow(Math.Abs(squareY - y), 2));

								//Weight and hence brightness is modified by 1/x^2 + 1 where x is the distance from actual item
								double additionalWeight = point.weight * (1d / ((distance * distance) + 1));
								squares[x, y].weights[legendGroup] += additionalWeight;
							}
						}
					}

					//Calculate positions and color for legend text, draw the string then move the 'caret' down
					int fontHeight = (int)Math.Ceiling(imageGraphic.MeasureString(mapItem.GetLegendText(), font, legendBounds).Height);
					Color legendColor = legendGroup == 0 ? Color.Red : Color.Blue;
					Brush textBrush = new SolidBrush(legendColor);

					imageGraphic.DrawString(mapItem.GetLegendText(), font, textBrush, new RectangleF(legendXMin, legendCaretHeight, legendWidth, fontHeight));
					legendCaretHeight += fontHeight;

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
						HeatMapGridSquare square = squares[x, y];
						if (square.GetTotalWeight() > largestWeight)
						{
							largestWeight = square.GetTotalWeight();
						}
					}
				}

				//Finally now weights are calculated, draw a square for every HeatGripMapSquare in the array
				for (int x = 0; x < resolution; x++)
				{
					int xCoord = x * pixelsPerSquare;

					//Don't draw grid squares which are entirely within the legend text area
					if (xCoord + pixelsPerSquare < legendXMax)
					{
						continue;
					}

					for (int y = 0; y < resolution; y++)
					{
						int yCoord = y * pixelsPerSquare;
						HeatMapGridSquare square = squares[x, y];

						Color color = square.GetColor(largestWeight);
						Brush brush = new SolidBrush(color);

						Rectangle heatMapSquare = new Rectangle(xCoord, yCoord, mapDimension / SettingsPlotHeatmap.resolution, mapDimension / SettingsPlotHeatmap.resolution);
						imageGraphic.FillRectangle(brush, heatMapSquare);
					}
				}

				GC.Collect();
				GC.WaitForPendingFinalizers();
			}

			mapFrame.Image = finalImage;
		}

		//Generate a layer of plots for the given MapItem
		public static Image GenerateIconPlotLayer(MapItem mapItem, Color color, Font font, PlotIconShape shape, int legendYPos, int legendHeight, int textOffset)
		{
			Bitmap layer = new Bitmap(mapDimension, mapDimension);
			Graphics plotLayer = Graphics.FromImage(layer);
			plotLayer.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

			PlotIcon plotIcon = new PlotIcon(color, shape);
			Image plotIconImg = plotIcon.GetIconImage();

			Brush textBrush = new SolidBrush(plotIcon.color);

			Color volumeColor = Color.FromArgb(volumeOpacity, color);
			Brush volumeBrush = new SolidBrush(volumeColor);

			foreach (MapDataPoint point in mapItem.GetPlots())
			{
				//Skip the point if it's fully outside the map image
				if (point.x < 0 || point.x >= mapDimension || point.y < 0 || point.y >= mapDimension)
				{
					continue;
				}

				//This is not suitable to be drawn as a volume - draw a simple plotIcon
				if (!SettingsPlot.volumeEnabled || //Volume drawing is disabled
					string.IsNullOrEmpty(point.primitiveShape) || //This is not a volume
					((int)point.boundX * (int)point.boundY) <= minVolumeArea) //This is too small to be drawn as a volume
				{
					//Skip plots that are placed outside the game world on the left/western side
					//This prevents drawing plots over the legend text
					if (point.x < legendXMax)
					{
						continue;
					}

					plotLayer.DrawImage(plotIconImg, (float)(point.x - (plotIconImg.Width / 2d)), (float)(point.y - (plotIconImg.Height / 2d)));
				}

				//This MapDataPoint is suitable to be drawn as a volume
				else
				{
					Image volumeImage = new Bitmap((int)point.boundX, (int)point.boundY);
					Graphics volumeGraphic = Graphics.FromImage(volumeImage);
					volumeGraphic.SmoothingMode = SmoothingMode.AntiAlias;

					switch (point.primitiveShape)
					{
						case "Box":
							volumeGraphic.FillRectangle(volumeBrush, new Rectangle(0, 0, (int)point.boundX, (int)point.boundY));
							break;
						case "Sphere":
							volumeGraphic.FillEllipse(volumeBrush, new Rectangle(0, 0, (int)point.boundX, (int)point.boundY));
							break;
						default:
							NonBlockingNotify.Error("Unexpected volume primitive shape '" + point.primitiveShape + "'. This shape cannot be drawn yet.");
							break;
					}

					plotLayer.DrawImage(volumeImage, (float)(point.x - (point.boundX / 2)), (float)(point.y - (point.boundY / 2)));
				}
			}

			plotLayer.DrawImage(plotIconImg, (float)(legendIconX - (plotIconImg.Width / 2d)), (float)(legendYPos - (plotIconImg.Height / 2d) + (legendHeight / 2d)));
			plotLayer.DrawString(mapItem.GetLegendText(), font, textBrush, new RectangleF(legendXMin, legendYPos + textOffset, legendWidth, legendHeight));

			return layer;
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
			SettingsMap.brightness = defaultBrightness;
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