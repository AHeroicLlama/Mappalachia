using System;
using System.Collections.Generic;
using System.Drawing;

namespace Mappalachia.Class
{
	//Handles the loading and saving of settings between file and memory.
	static class SettingsManager
	{


		//Gather all settings and write them to the preferences file
		public static void SaveSettings()
		{
			List<string> settings = new List<string>();
			settings.Add("#Mappalachia Preferences file. Modify at your own risk. Edits will be overwritten.");

			//Gather collections into single strings for later
			string paletteColor = "";
			string paletteShape = "";

			foreach (Color color in SettingsPlotIcon.paletteColor)
			{
				paletteColor += "(" + color.R + "," + color.G + "," + color.B + ")";
			}

			foreach (PlotIconShape shape in SettingsPlotIcon.paletteShape)
			{
				paletteShape += "(" + 
					BoolToInt(shape.diamond) +
					BoolToInt(shape.square) +
					BoolToInt(shape.circle) +
					BoolToInt(shape.crosshairInner) +
					BoolToInt(shape.crosshairOuter) +
					BoolToInt(shape.fill) + ")";
			}

			//SettingsMap
			settings.Add("[Map]");
			settings.Add("brightness=" + SettingsMap.brightness);
			settings.Add("layerMilitary=" + BoolToInt(SettingsMap.layerMilitary));
			settings.Add("layerNWMorgantown=" + BoolToInt(SettingsMap.layerNWMorgantown));
			settings.Add("layerNWFlatwoods=" + BoolToInt(SettingsMap.layerNWFlatwoods));
			settings.Add("grayScale=" + BoolToInt(SettingsMap.grayScale));

			//SettingsSearch
			settings.Add("[Search]");
			settings.Add("searchInterior=" + BoolToInt(SettingsSearch.searchInterior));
			settings.Add("showFormID=" + BoolToInt(SettingsSearch.showFormID));
			settings.Add("filterWarnings=" + BoolToInt(SettingsSearch.filterWarnings));

			//SettingsPlot
			settings.Add("[Plot]");
			settings.Add("mode=" + SettingsPlot.mode);
			settings.Add("drawVolumes=" + BoolToInt(SettingsPlot.drawVolumes));

			//SettingsPlotIcon
			settings.Add("[PlotIcon]");
			settings.Add("iconSize=" + SettingsPlotIcon.iconSize);
			settings.Add("lineWidth=" + SettingsPlotIcon.lineWidth);
			settings.Add("iconOpacityPercent=" + SettingsPlotIcon.iconOpacityPercent);
			settings.Add("shadowOpacityPercent=" + SettingsPlotIcon.shadowOpacityPercent);
			settings.Add("paletteColor=" + paletteColor.Trim());
			settings.Add("paletteShape=" + paletteShape.Trim());

			//SettingsPlotHeatmap
			settings.Add("[PlotHeatmap]");
			settings.Add("resolution=" + SettingsPlotHeatmap.resolution);
			settings.Add("colorMode=" + SettingsPlotHeatmap.colorMode);

			//Write the list of strings to the prefs file
			IOManager.WritePreferences(settings);
		}

		//Load the prefs file and parse them into settings, then apply them to Settings variables
		public static void LoadSettings()
		{
			//Read the preferences file
			List<string> settings = IOManager.ReadPreferences();

			//The file was not found (probably a new install) - we can silently skip
			if (settings == null)
			{
				return;
			}

			foreach (string line in settings)
			{
				try
				{
					//Skip lines which are commented, section headers, or empty 
					if (line.StartsWith("#") || line.StartsWith("[") || string.IsNullOrEmpty(line))
					{
						continue;
					}

					//Take the values either side of the "=", before any "#"
					string[] kvp = line.Split('#')[0].Split('=');

					//Verify we have precisely two strings for our kvp
					if (kvp.Length != 2)
					{
						throw new ArgumentException("Unable to parse a valid Key-Value pair from line.");
					}

					string key = kvp[0].Trim();
					string value = kvp[1].Trim();

					//Verify we have a key and a value
					if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
					{
						throw new ArgumentException("Key and/or Value were blank.");
					}

					switch (key)
					{
						case "brightness":
							int brightness = Convert.ToInt32(value);
							if (ValidateWithinRange(brightness, SettingsMap.minBrightness, SettingsMap.maxBrightness))
							{
								SettingsMap.brightness = brightness;
							}
							break;

						case "layerMilitary":
							SettingsMap.layerMilitary = StringIntToBool(value);
							break;

						case "layerNWMorgantown":
							SettingsMap.layerNWMorgantown = StringIntToBool(value);
							break;

						case "layerNWFlatwoods":
							SettingsMap.layerNWFlatwoods = StringIntToBool(value);
							break;

						case "grayScale":
							SettingsMap.grayScale = StringIntToBool(value);
							break;

						case "searchInterior":
							SettingsSearch.searchInterior = StringIntToBool(value);
							break;

						case "showFormID":
							SettingsSearch.showFormID = StringIntToBool(value);
							break;

						case "filterWarnings":
							SettingsSearch.filterWarnings = StringIntToBool(value);
							break;

						case "mode":
							if (value == "Icon")
							{
								SettingsPlot.mode = SettingsPlot.Mode.Icon;
							}
							else if (value == "Heatmap")
							{
								SettingsPlot.mode = SettingsPlot.Mode.Heatmap;
							}
							else
							{
								throw new ArgumentException("Invalid plot mode.");
							}
							break;

						case "drawVolumes":
							SettingsPlot.drawVolumes = StringIntToBool(value);
							break;

						case "iconSize":
							int iconSize = Convert.ToInt32(value);
							if (ValidateWithinRange(iconSize, SettingsPlotIcon.iconSizeMin, SettingsPlotIcon.iconSizeMax))
							{
								SettingsPlotIcon.iconSize = iconSize;
							}
							break;

						case "lineWidth":
							int lineWidth = Convert.ToInt32(value);
							if (ValidateWithinRange(lineWidth, SettingsPlotIcon.lineWidthMin, SettingsPlotIcon.lineWidthMax))
							{
								SettingsPlotIcon.lineWidth = lineWidth;
							}
							break;

						case "iconOpacityPercent":
							int iconOpacityPercent = Convert.ToInt32(value);
							if (ValidateWithinRange(iconOpacityPercent, SettingsPlotIcon.iconOpacityPercentMin, SettingsPlotIcon.iconOpacityPercentMax))
							{
								SettingsPlotIcon.iconOpacityPercent = iconOpacityPercent;
							}
							break;

						case "shadowOpacityPercent":
							int shadowOpacityPercent = Convert.ToInt32(value);
							if (ValidateWithinRange(shadowOpacityPercent, SettingsPlotIcon.shadowOpacityPercentMin, SettingsPlotIcon.shadowOpacityPercentMax))
							{
								SettingsPlotIcon.shadowOpacityPercent = shadowOpacityPercent;
							}
							break;

						case "paletteColor":
							//TODO
							break;

						case "paletteShape":
							//TODO
							break;

						case "resolution":
							int resolution = Convert.ToInt32(value);
							if (SettingsPlotHeatmap.validResolutions.Contains(resolution))
							{
								SettingsPlotHeatmap.resolution = resolution;
							}
							else
							{
								throw new ArgumentException("Resolution " + resolution + " Not supported.");
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

						default:
							//The key was none of the expected values - must be wrong
							throw new ArgumentException("Did not match any known Keys.");
					}
				}
				catch (Exception e)
				{
					Notify.Warn("Error loading preference from preferences file. Line: \"" + line + "\".\n" +
						"If this line related to a setting, it will be restored to default, otherwise it will be ignored.\n\n" +
						IOManager.genericExceptionHelpText + "\n" + e);
				}
			}
		}

		//Simply convert a boolean to an int representation
		static int BoolToInt(bool variable)
		{
			return variable ? 1 : 0;
		}

		//Convert a string representation of an integer into a boolean. Throw an exception if not 0 or 1
		static bool StringIntToBool(string variable)
		{
			if (variable != "0" && variable != "1")
			{
				throw new ArgumentException("Expected exactly 0 or 1");
			}

			return variable == "1";
		}

		//Return if an int is between two values inclusive. Throw exception if outside
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
