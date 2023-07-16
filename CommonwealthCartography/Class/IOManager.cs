using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace CommonwealthCartography
{
	// File opening, reading, writing and cleanup operations
	class IOManager
	{
		public enum OpenImageMode
		{
			QuickSaveInExplorer,
			TempSaveInViewer,
		}

		const string imgFolder = @"img\";
		const string dataFolder = @"data\";
		const string fontFolder = @"font\";
		const string mapMarkerfolder = imgFolder + @"mapmarker\";

		const string fontFileName = "futura_condensed_bold.ttf";
		const string databaseFileName = "commonwealth_cartography.db";
		const string imgFileNameCommonwealthMilitary = "Commonwealth_military.jpg"; // TODO - does this exist?
		const string imgFileNameCommonwealthSatellite = "Commonwealth_render.jpg";
		const string imgFileNameCommonwealthWaterMask = "Commonwealth_waterMask.png";
		const string MapFileExtension = ".jpg";
		const string settingsFileName = "commonwealth_cartography_prefs.ini";
		const string mapMarkerFileExtension = ".svg";

		const string tempImageFolder = @"temp\";
		const string quickSaveFolder = @"QuickSaves\";

		static readonly Dictionary<string, Image> worldspaceMapImageCache = new Dictionary<string, Image>();
		static readonly ConcurrentDictionary<string, Image> mapMarkerimageCache = new ConcurrentDictionary<string, Image>();

		static Image imageCommonwealthMilitary;
		static Image imageCommonwealthSatellite;
		static Image imageCommonwealthWaterMask;

		static string gameVersion;

		static PrivateFontCollection fontCollection;

		public const string genericExceptionHelpText =
			"To counter common errors, please check that:\n" +
			"- Any security software has not accidentally removed or quarantined Commonwealth Cartography files, or is otherwise interfering.\n" +
			"- The entire Commonwealth Cartography installation has been unzipped into one folder, with the same folder structure it came with.\n" +
			"- None of the installation has been moved, renamed, or deleted.\n" +
			"- (Where applicable) Destination folders or files are accessible and are not locked.\n" +
			"Failing these, try fully deleting and re-downloading Commonwealth Cartography.\n\n";

		private static ImageCodecInfo GetEncoder(ImageFormat format)
		{
			try
			{
				ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
				foreach (ImageCodecInfo codec in codecs)
				{
					if (codec.FormatID == format.Guid)
					{
						return codec;
					}
				}

				// Expired all codecs without finding correct one
				throw new KeyNotFoundException("Codec not found in available ImageDecoders");
			}
			catch (Exception e)
			{
				Notify.Error(
					"Commonwealth Cartography was unable to find an Image Codec in order to write map images to file.\n\n" +
					"Commonwealth Cartography must exit.\n" +
					genericExceptionHelpText +
					e);

				Environment.Exit(1);
				return null;
			}
		}

		// Return an open connection to the database. Exit if it fails.
		public static SqliteConnection OpenDatabase()
		{
			try
			{
				SqliteConnection connection = new SqliteConnection("Data Source=" + dataFolder + databaseFileName + ";Mode=ReadOnly");
				connection.Open();
				return connection;
			}
			catch (Exception e)
			{
				Notify.Error(
					"Commonwealth Cartography was unable to access the database located at " + dataFolder + databaseFileName + ".\n\n" +
					genericExceptionHelpText +
					"Commonwealth Cartography must exit.\n\n" + e);

				Environment.Exit(1);
				return null;
			}
		}

		static void GenerateFolder(string folderName)
		{
			try
			{
				Directory.CreateDirectory(folderName);
			}
			catch (Exception e)
			{
				Notify.Error(
					"Commonwealth Cartography was unable to create the directory at " + folderName + "\n\n" +
					genericExceptionHelpText +
					e);
			}
		}

		// Saves the map image with recommended paramaters, and either opens it in default file viewer, or selects it in explorer
		public static void QuickSaveImage(OpenImageMode openImageMode)
		{
			string folderPath = string.Empty;

			if (openImageMode == OpenImageMode.TempSaveInViewer)
			{
				folderPath = tempImageFolder;
			}
			else if (openImageMode == OpenImageMode.QuickSaveInExplorer)
			{
				folderPath = quickSaveFolder;
			}

			string filePath = $"{folderPath}CommonwealthCartography_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_FFF")}.{SettingsFileExport.GetFileTypeRecommendation()}";

			try
			{
				GenerateFolder(folderPath);
				ImageFormat imageFormat;

				switch (SettingsFileExport.GetFileTypeRecommendation())
				{
					case SettingsFileExport.ExtensionType.PNG:
						imageFormat = ImageFormat.Png;
						break;

					case SettingsFileExport.ExtensionType.JPEG:
						imageFormat = ImageFormat.Jpeg;
						break;

					default:
						imageFormat = ImageFormat.Jpeg;
						break;
				}

				WriteToFile(filePath, Map.GetImage(), imageFormat, SettingsFileExport.jpegQualityDefault);

				if (openImageMode == OpenImageMode.TempSaveInViewer)
				{
					Process.Start(new ProcessStartInfo { FileName = filePath, UseShellExecute = true });
				}
				else if (openImageMode == OpenImageMode.QuickSaveInExplorer)
				{
					SelectFile(filePath);
				}
			}
			catch (Exception e)
			{
				Notify.Error(
					"Commonwealth Cartography was unable to save or open your temporary map image at " + filePath + ".\n\n" +
					genericExceptionHelpText +
					e);

				return;
			}
		}

		// Write the given image to the location with the file settings given
		public static void WriteToFile(string filePath, Image image, ImageFormat imageFormat, int jpegQuality)
		{
			try
			{
				FormMaster.UpdateProgressBar("Writing map image to disk...", true);
				if (imageFormat == ImageFormat.Png)
				{
					image.Save(filePath, ImageFormat.Png);
				}
				else if (imageFormat == ImageFormat.Jpeg)
				{
					EncoderParameters encoderParams = new EncoderParameters(1);
					encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, jpegQuality);

					image.Save(filePath, GetEncoder(ImageFormat.Jpeg), encoderParams);
				}
				else
				{
					throw new ArgumentException("Invalid ImageFormat type " + imageFormat);
				}
			}
			catch (Exception e)
			{
				Notify.Error(
					"Commonwealth Cartography was unable to save your map to " + filePath + ".\n\n" +
					genericExceptionHelpText +
					e);
			}
			finally
			{
				FormMaster.UpdateProgressBar(0);
			}
		}

		public static void SelectFile(string path)
		{
			try
			{
				Process.Start("explorer.exe", "/select," + path);
			}
			catch (Exception e)
			{
				Notify.Error(
					"Commonwealth Cartography was unable to select the given file at " + path + ".\n\n" +
					genericExceptionHelpText +
					e);
			}
		}

		// Remove temp files generated by map preview
		public static void Cleanup()
		{
			try
			{
				if (Directory.Exists(tempImageFolder))
				{
					Directory.Delete(tempImageFolder, true);
				}
			}
			catch (Exception e)
			{
				Notify.Info(
					"Commonwealth Cartography was unable to properly cleanup some temporary files left behind at " + tempImageFolder + ".\n" +
					"You may wish to remove the folder yourself, otherwise we will try again the next time Commonwealth Cartography is opened.\n\n" +
					genericExceptionHelpText +
					e);

				return;
			}
		}

		// Returns the appropriate background image for the map based on current settings
		public static Image GetImageForSpace(Space space)
		{
			if (SettingsMap.background == SettingsMap.Background.None)
			{
				return EmptyMapBackground();
			}

			// Return the non-standard maps if this is Commonwealth and they requested it
			if (space.IsCommonwealth())
			{
				if (SettingsMap.background == SettingsMap.Background.Military)
				{
					return GetImageCommonwealthMilitary();
				}
				else if (SettingsMap.background == SettingsMap.Background.Satellite)
				{
					return GetImageCommonwealthSatellite();
				}
			}

			// Otherwise look for the default background image for the worldspace
			string editorID = space.editorID;

			// Special case for duplicate cells which can share an image
			if (editorID.StartsWith("CharGen"))
			{
				editorID = "CharGen01";
			}

			string filepath = imgFolder + (!space.IsWorldspace() ? "\\cell\\" : string.Empty) + editorID + MapFileExtension;

			try
			{
				// Use the cache of the image (if cached)
				if (space.IsWorldspace())
				{
					// Cache the image if not already
					if (!worldspaceMapImageCache.ContainsKey(editorID))
					{
						worldspaceMapImageCache.Add(editorID, Image.FromFile(filepath));
					}

					return new Bitmap(worldspaceMapImageCache[editorID]);
				}

				// Don't cache the image - read from disk
				else
				{
					return new Bitmap(Image.FromFile(filepath));
				}
			}
			catch (FileNotFoundException e)
			{
				Notify.Error("Commonwealth Cartography was unable to find a background map image for the worldspace '" + editorID + "'.\n" + e);
				return EmptyMapBackground();
			}
			catch (Exception e)
			{
				Notify.Error("Commonwealth Cartography was unable to read the file '" + filepath + "'.\n" + genericExceptionHelpText + e);
				return EmptyMapBackground();
			}
		}

		public static Image GetImageCommonwealthMilitary()
		{
			if (imageCommonwealthMilitary == null)
			{
				try
				{
					imageCommonwealthMilitary = Image.FromFile(imgFolder + imgFileNameCommonwealthMilitary);
				}
				catch (Exception e)
				{
					Notify.Error("Commonwealth Cartography was unable to read the file '" + imgFileNameCommonwealthMilitary + "'.\n" + genericExceptionHelpText + e);
					imageCommonwealthMilitary = EmptyMapBackground();
				}
			}

			return new Bitmap(imageCommonwealthMilitary);
		}

		public static Image GetImageCommonwealthSatellite()
		{
			if (imageCommonwealthSatellite == null)
			{
				try
				{
					imageCommonwealthSatellite = Image.FromFile(imgFolder + imgFileNameCommonwealthSatellite);
				}
				catch (Exception e)
				{
					Notify.Error("Commonwealth Cartography was unable to read the file '" + imgFileNameCommonwealthSatellite + "'.\n" + genericExceptionHelpText + e);
					imageCommonwealthSatellite = EmptyMapBackground();
				}
			}

			return new Bitmap(imageCommonwealthSatellite);
		}

		public static Image GetImageCommonwealthWaterMask()
		{
			if (imageCommonwealthWaterMask == null)
			{
				try
				{
					imageCommonwealthWaterMask = Image.FromFile(imgFolder + imgFileNameCommonwealthWaterMask);
				}
				catch (Exception e)
				{
					Notify.Error("Commonwealth Cartography was unable to read the file '" + imgFileNameCommonwealthWaterMask + "'.\n" + genericExceptionHelpText + e);
					imageCommonwealthWaterMask = EmptyMapBackground();
				}
			}

			return new Bitmap(imageCommonwealthWaterMask);
		}

		public static Image EmptyMapBackground()
		{
			return new Bitmap(Map.mapDimension, Map.mapDimension);
		}

		public static Image GetMapMarker(string mapMarkerName)
		{
			// Sort of like a constructor for the dict
			if (mapMarkerimageCache.IsEmpty)
			{
				try
				{
					BuildMapMarkerCache();
				}
				catch (Exception)
				{ } // Parallel caching failed but we can still try to draw them on-demand in series
			}

			if (!mapMarkerimageCache.ContainsKey(mapMarkerName))
			{
				try
				{
					CacheMarker(mapMarkerName);
				}
				catch (Exception e)
				{
					Notify.Error("Commonwealth Cartography was unable to find or read the map marker image file for '" + mapMarkerName + "'.\n" + genericExceptionHelpText + e);
					if (!mapMarkerimageCache.ContainsKey(mapMarkerName))
					{
						mapMarkerimageCache[mapMarkerName] = new Bitmap(2, 2);
					}
				}
			}

			return new Bitmap(mapMarkerimageCache[mapMarkerName]);
		}

		// Parallel draws all the SVG map markers into a dictionary
		static void BuildMapMarkerCache()
		{
			FormMaster.UpdateProgressBar(0, "Caching rendered map icons...");
			List<string> markerNames = Database.GetUniqueMarkerNames();
			Parallel.ForEach(markerNames, markerName => CacheMarker(markerName));
		}

		static void CacheMarker(string mapMarkerName)
		{
			Svg.SvgDocument document = Svg.SvgDocument.Open(mapMarkerfolder + mapMarkerName + mapMarkerFileExtension);
			Image marker = document.Draw((int)(document.Width * Map.markerIconScale), 0);

			if (!mapMarkerimageCache.ContainsKey(mapMarkerName))
			{
				mapMarkerimageCache[mapMarkerName] = marker;
			}
		}

		// Read preferences from file
		public static List<string> ReadPreferences()
		{
			try
			{
				if (File.Exists(settingsFileName))
				{
					return File.ReadAllLines(settingsFileName).ToList();
				}
				else
				{
					return null; // The prefs file did not exist
				}
			}
			catch (Exception e)
			{
				Notify.Error(
					"Commonwealth Cartography encountered an error while reading your user preferences file from " + settingsFileName + ". Your settings will be reset to default.\n" +
					genericExceptionHelpText +
					"\n\n" + e);

				return null;
			}
		}

		// Save preferences to file
		public static void WritePreferences(List<string> prefs)
		{
			try
			{
				File.WriteAllLines(settingsFileName, prefs);
			}
			catch (Exception e)
			{
				Notify.Error(
					"Commonwealth Cartography encountered an error while saving your preferences file to " + settingsFileName + ". Your settings will not be saved.\n" +
					genericExceptionHelpText +
					"\n\n" + e);
			}
		}

		// Add the font to a PrivateFontCollection
		public static PrivateFontCollection LoadFont()
		{
			if (fontCollection == null)
			{
				string fontPath = fontFolder + fontFileName;
				try
				{
					fontCollection = new PrivateFontCollection();
					fontCollection.AddFontFile(fontPath);
				}
				catch (Exception e)
				{
					Notify.Error(
						"Commonwealth Cartography was unable to load the file " + fontPath + " and as a result your maps cannot be drawn.\n" +
						genericExceptionHelpText +
						"Commonwealth Cartography must exit.\n\n" + e);

					Environment.Exit(1);
					return null;
				}
			}

			return fontCollection;
		}

		// Return the game version string stored in the db
		public static string GetGameVersion()
		{
			if (string.IsNullOrEmpty(gameVersion))
			{
				gameVersion = Database.GetGameVersion();
			}

			return gameVersion;
		}

		public static void LaunchURL(string url)
		{
			Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
		}

		// Approximately removes illegal characters from a would-be filename
		// Only use for better suggested filenames, not safe for direct file writing
		public static string SanitizeFilename(string filename)
		{
			return string.Concat(filename.Split(Path.GetInvalidFileNameChars())).Trim().TrimEnd('.');
		}
	}
}
