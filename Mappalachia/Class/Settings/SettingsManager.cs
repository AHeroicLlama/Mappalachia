using System;
using System.Collections.Generic;
using System.Drawing;

namespace Mappalachia.Class
{
	// Handles the parsing of settings between file and memory.
	static class SettingsManager
	{
		// Keep a record on the prefs file of the preferences file version to assist future compatibility
		static readonly int prefsIteration = 6;

		// Gather all settings and write them to the preferences file
		public static void SaveSettings()
		{
			List<string> settings = new List<string>();
			settings.Add("#Mappalachia Preferences file. Modify at your own risk. Edits will be overwritten.");

			// Gather collections into lists for later
			List<string> colors = new List<string>();
			List<string> shapes = new List<string>();

			foreach (Color color in SettingsPlotStyle.paletteColor)
			{
				colors.Add(color.R + "," + color.G + "," + color.B);
			}

			foreach (PlotIconShape shape in SettingsPlotStyle.paletteShape)
			{
				shapes.Add(
					BoolToIntStr(shape.diamond) +
					BoolToIntStr(shape.square) +
					BoolToIntStr(shape.circle) +
					BoolToIntStr(shape.crosshairInner) +
					BoolToIntStr(shape.crosshairOuter) +
					BoolToIntStr(shape.fill));
			}

			// Preferences Version
			settings.Add("[Versioning]");
			settings.Add("version=" + prefsIteration);

			// SettingsMap
			settings.Add("[Map]");
			settings.Add("brightness=" + SettingsMap.brightness);
			settings.Add("layerMilitary=" + BoolToIntStr(SettingsMap.layerMilitary));
			settings.Add("grayScale=" + BoolToIntStr(SettingsMap.grayScale));
			settings.Add("showMapLabels=" + BoolToIntStr(SettingsMap.showMapLabels));
			settings.Add("showMapIcons=" + BoolToIntStr(SettingsMap.showMapIcons));
			settings.Add("hideLegend=" + BoolToIntStr(SettingsMap.hideLegend));

			// SettingsSearch
			settings.Add("[Search]");
			settings.Add("showFormID=" + BoolToIntStr(SettingsSearch.showFormID));
			settings.Add("searchInAllSpaces=" + BoolToIntStr(SettingsSearch.searchInAllSpaces));
			settings.Add("spawnChance=" + SettingsSearch.spawnChance);

			// SettingsPlot
			settings.Add("[Plot]");
			settings.Add("mode=" + SettingsPlot.mode);
			settings.Add("drawVolumes=" + BoolToIntStr(SettingsPlot.drawVolumes));

			// SettingsPlotIcon
			settings.Add("[PlotIcon]");
			settings.Add("iconSize=" + SettingsPlotStyle.iconSize);
			settings.Add("lineWidth=" + SettingsPlotStyle.lineWidth);
			settings.Add("iconOpacityPercent=" + SettingsPlotStyle.iconOpacityPercent);
			settings.Add("shadowOpacityPercent=" + SettingsPlotStyle.shadowOpacityPercent);
			settings.Add("paletteColor=" + string.Join(":", colors));
			settings.Add("paletteShape=" + string.Join(":", shapes));

			// SettingsPlotHeatmap
			settings.Add("[PlotHeatmap]");
			settings.Add("resolution=" + SettingsPlotHeatmap.resolution);
			settings.Add("colorMode=" + SettingsPlotHeatmap.colorMode);

			// SettingsPlotTopograph
			settings.Add("[PlotTopograph]");
			settings.Add("colorBands=" + SettingsPlotTopograph.colorBands);

			// SettingsFileExport
			settings.Add("[FileExport]");
			settings.Add("useRecommended=" + BoolToIntStr(SettingsFileExport.useRecommended));
			settings.Add("fileType=" + SettingsFileExport.fileType);
			settings.Add("jpegQuality=" + SettingsFileExport.jpegQuality);

			// Write the list of strings to the prefs file
			IOManager.WritePreferences(settings);
		}

		// Load the prefs file and parse them into settings, then apply them to Settings variables
		public static void LoadSettings()
		{
			// Read the preferences file
			List<string> settings = IOManager.ReadPreferences();

			// The file was not found (probably a new install) - we can silently skip
			if (settings == null)
			{
				return;
			}

			foreach (string line in settings)
			{
				try
				{
					// Skip lines which are commented, section headers, or empty
					if (line.StartsWith("#") || line.StartsWith("[") || string.IsNullOrEmpty(line))
					{
						continue;
					}

					// Take the values either side of the "=", before any "#"
					string[] kvp = line.Split('#')[0].Split('=');

					// Verify we have precisely two strings for our kvp
					if (kvp.Length != 2)
					{
						throw new ArgumentException("Unable to parse a valid Key-Value pair from line.");
					}

					string key = kvp[0].Trim();
					string value = kvp[1].Trim();

					// Verify we have a key and a value
					if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
					{
						throw new ArgumentException("Key and/or Value were blank.");
					}

					switch (key)
					{
						case "version":
							// Nothing to do
							break;

						case "brightness":
							int brightness = Convert.ToInt32(value);
							if (ValidateWithinRange(brightness, SettingsMap.brightnessMin, SettingsMap.brightnessMax))
							{
								SettingsMap.brightness = brightness;
							}

							break;

						case "layerMilitary":
							SettingsMap.layerMilitary = StrIntToBool(value);
							break;

						case "grayScale":
							SettingsMap.grayScale = StrIntToBool(value);
							break;

						case "showMapMarkers": // Historic name
						case "showMapLabels":
							SettingsMap.showMapLabels = StrIntToBool(value);
							break;

						case "showMapIcons":
							SettingsMap.showMapIcons = StrIntToBool(value);
							break;

						case "hideLegend":
							SettingsMap.hideLegend = StrIntToBool(value);
							break;

						case "showFormID":
							SettingsSearch.showFormID = StrIntToBool(value);
							break;

						case "searchInAllSpaces":
							SettingsSearch.searchInAllSpaces = StrIntToBool(value);
							break;

						case "spawnChance":
							int spawnChance = Convert.ToInt32(value);
							if (ValidateWithinRange(spawnChance, SettingsSearch.spawnChanceMin, SettingsSearch.spawnChanceMax))
							{
								SettingsSearch.spawnChance = spawnChance;
							}

							break;

						case "mode":
							switch (value)
							{
								case "Icon":
									SettingsPlot.mode = SettingsPlot.Mode.Icon;
									break;

								case "Heatmap":
									SettingsPlot.mode = SettingsPlot.Mode.Heatmap;
									break;

								case "Topography":
									SettingsPlot.mode = SettingsPlot.Mode.Topography;
									break;

								default:
									throw new ArgumentException("Invalid plot mode.");
							}

							break;

						case "drawVolumes":
							SettingsPlot.drawVolumes = StrIntToBool(value);
							break;

						case "iconSize":
							int iconSize = Convert.ToInt32(value);
							if (ValidateWithinRange(iconSize, SettingsPlotStyle.iconSizeMin, SettingsPlotStyle.iconSizeMax))
							{
								SettingsPlotStyle.iconSize = iconSize;
							}

							break;

						case "lineWidth":
							int lineWidth = Convert.ToInt32(value);
							if (ValidateWithinRange(lineWidth, SettingsPlotStyle.lineWidthMin, SettingsPlotStyle.lineWidthMax))
							{
								SettingsPlotStyle.lineWidth = lineWidth;
							}

							break;

						case "iconOpacityPercent":
							int iconOpacityPercent = Convert.ToInt32(value);
							if (ValidateWithinRange(iconOpacityPercent, SettingsPlotStyle.iconOpacityPercentMin, SettingsPlotStyle.iconOpacityPercentMax))
							{
								SettingsPlotStyle.iconOpacityPercent = iconOpacityPercent;
							}

							break;

						case "shadowOpacityPercent":
							int shadowOpacityPercent = Convert.ToInt32(value);
							if (ValidateWithinRange(shadowOpacityPercent, SettingsPlotStyle.shadowOpacityPercentMin, SettingsPlotStyle.shadowOpacityPercentMax))
							{
								SettingsPlotStyle.shadowOpacityPercent = shadowOpacityPercent;
							}

							break;

						case "paletteColor":
							string[] colors = value.Split(':');

							if (colors.Length < 1)
							{
								throw new ArgumentException("Too few colors defined.");
							}

							List<Color> loadedColorPalette = new List<Color>();

							foreach (string color in colors)
							{
								// Separate the RGB values
								string[] rgbValues = color.Split(',');

								if (rgbValues.Length != 3)
								{
									throw new ArgumentException("Malformed color \"" + color + "\".");
								}

								// Calculate the individual RGB values
								int red = Convert.ToInt32(rgbValues[0]);
								int green = Convert.ToInt32(rgbValues[1]);
								int blue = Convert.ToInt32(rgbValues[2]);

								loadedColorPalette.Add(Color.FromArgb(red, green, blue));
							}

							// Overwrite the default palette with our loaded palette
							SettingsPlotStyle.paletteColor = new List<Color>(loadedColorPalette);
							break;

						case "paletteShape":
							string[] shapes = value.Split(':');

							if (shapes.Length < 1)
							{
								throw new ArgumentException("Too few shapes defined.");
							}

							// The total variables which define a shape
							int totalShapeOptions = 6;

							List<PlotIconShape> loadedShapePalette = new List<PlotIconShape>();

							foreach (string shape in shapes)
							{
								if (shape.Length != totalShapeOptions)
								{
									throw new ArgumentException("Malformed shape \"" + shape + "\".");
								}

								// First 5 digits represent the shape options. All 0 would be no shape, hence invisible
								if (shape.StartsWith("00000"))
								{
									throw new ArgumentException("Shape cannot have false values for every shape type setting.");
								}

								bool diamond = StrIntToBool(shape[0]);
								bool square = StrIntToBool(shape[1]);
								bool circle = StrIntToBool(shape[2]);
								bool crosshairInner = StrIntToBool(shape[3]);
								bool crosshairOuter = StrIntToBool(shape[4]);
								bool fill = StrIntToBool(shape[5]);

								loadedShapePalette.Add(new PlotIconShape(diamond, square, circle, crosshairInner, crosshairOuter, fill));
							}

							// Overwrite the default palette with our loaded palette
							SettingsPlotStyle.paletteShape = new List<PlotIconShape>(loadedShapePalette);
							break;

						case "resolution":
							int resolution = Convert.ToInt32(value);
							if (SettingsPlotHeatmap.validResolutions.Contains(resolution))
							{
								SettingsPlotHeatmap.resolution = resolution;
							}
							else
							{
								throw new ArgumentException("Resolution " + resolution + " not supported.");
							}

							break;

						case "colorMode":
							if (value == "Mono")
							{
								SettingsPlotHeatmap.colorMode = SettingsPlotHeatmap.ColorMode.Mono;
							}
							else if (value == "Duo")
							{
								SettingsPlotHeatmap.colorMode = SettingsPlotHeatmap.ColorMode.Duo;
							}
							else
							{
								throw new ArgumentException("Invalid color mode.");
							}

							break;

						case "colorBands":
							int colorBands = Convert.ToInt32(value);
							if (ValidateWithinRange(colorBands, 2, 5))
							{
								SettingsPlotTopograph.colorBands = colorBands;
							}

							break;

						case "useRecommended":
							SettingsFileExport.SetUseRecommended(StrIntToBool(value));
							break;

						case "fileType":
							if (value == "PNG")
							{
								SettingsFileExport.fileType = SettingsFileExport.FileType.PNG;
							}
							else if (value == "JPEG")
							{
								SettingsFileExport.fileType = SettingsFileExport.FileType.JPEG;
							}
							else
							{
								throw new ArgumentException("Invalid export file type.");
							}

							break;

						case "jpegQuality":
							int jpegQuality = Convert.ToInt32(value);
							if (ValidateWithinRange(jpegQuality, SettingsFileExport.jpegQualityMin, SettingsFileExport.jpegQualityMax))
							{
								SettingsFileExport.jpegQuality = jpegQuality;
							}

							break;

						// Legacy settings - ignore
						case "filterWarnings":
						case "layerNWMorgantown":
						case "layerNWFlatwoods":
						case "searchInterior":
							break;

						default:
							// The key was none of the expected values - must be wrong
							throw new ArgumentException("Did not match any known Keys.");
					}
				}
				catch (Exception e)
				{
					Notify.Warn("Error loading preference from preferences file. Line: \"" + line + "\".\n" +
						"If this line related to a setting, it will be restored to default. This line will be replaced or removed.\n\n" +
						IOManager.genericExceptionHelpText + "\n" + e);
				}
			}
		}

		// Convert a boolean to a string representation of an int
		static string BoolToIntStr(bool variable)
		{
			return variable ? "1" : "0";
		}

		// Convert a string representation of an int into a boolean. Throw an exception if not 0 or 1
		static bool StrIntToBool(string variable)
		{
			if (variable != "0" && variable != "1")
			{
				throw new ArgumentException("Expected exactly 0 or 1");
			}

			return variable == "1";
		}

		// Overload
		static bool StrIntToBool(char variable)
		{
			return StrIntToBool(char.ToString(variable));
		}

		// Return if an int is between two values inclusive. Throw exception if outside
		static bool ValidateWithinRange(int value, int min, int max)
		{
			if (value >= min && value <= max)
			{
				return true;
			}
			else
			{
				throw new ArgumentException("Value " + value + " outside of acceptable range. (" + min + "-" + max + ")");
			}
		}
	}
}
