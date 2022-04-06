using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using Mappalachia.Class;
using Microsoft.Data.Sqlite;

namespace Mappalachia
{
	// File opening, reading, writing and cleanup operations
	class IOManager
	{
		static readonly string imgFolder = @"img\";
		static readonly string dataFolder = @"data\";
		static readonly string fontFolder = @"font\";

		static readonly string fontFileName = "futura_condensed_bold.ttf";
		static readonly string databaseFileName = "mappalachia.db";
		static readonly string imgFileNameMapMilitary = "military.jpg";
		static readonly string worldspaceMapFileExtension = ".jpg";
		static readonly string settingsFileName = "mappalachia_prefs.ini";

		static readonly string tempImageFolder = @"temp\";
		static readonly string tempImageBaseFileName = "mappalachia_preview";
		static readonly string tempImageFileExtension = ".png";

		static readonly Dictionary<string, Image> worldspaceMapImageCache = new Dictionary<string, Image>();
		static Image imageMapMilitary;

		static int tempImageLockedCount = 0;

		static string gameVersion;

		static PrivateFontCollection fontCollection;

		public static readonly string genericExceptionHelpText =
			"To counter common errors, please check that:\n" +
			"- Windows is up to date and you have the latest .Net Framework 4.8 installed.\n" +
			"- Any security software has not accidentally removed or quarantined Mappalachia files, or is otherwise interfering.\n" +
			"- The entire Mappalachia installation has been unzipped into one folder, with the same folder structure it came with.\n" +
			"- None of the installation has been moved, renamed, or deleted.\n" +
			"- (Where applicable) Destination folders or files are accessible and are not locked.\n" +
			"Failing these, try fully deleting and re-downloading Mappalachia.\n\n";

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
				throw new Exception("Codec not found in available ImageDecoders");
			}
			catch (Exception e)
			{
				Notify.Error(
					"Mappalachia was unable to find an Image Codec in order to write map images to file.\n\n" +
					"Mappalachia must exit.\n" +
					genericExceptionHelpText +
					e);

				Environment.Exit(1);
				return null;
			}
		}

		// Find a new place to put a temporary image
		// Use the default location if it's not used or can be deleted
		// Otherwise if it is locked, start incrementing on the file name.
		static string GetNewTempImageFilePath()
		{
			string idealTempFile = tempImageFolder + tempImageBaseFileName + tempImageFileExtension;
			if (File.Exists(idealTempFile))
			{
				try
				{	// If we can re-use the old location, let's
					File.Delete(idealTempFile);
					return idealTempFile;
				}
				catch
				{
					tempImageLockedCount++;
					return tempImageFolder + tempImageBaseFileName + tempImageLockedCount + tempImageFileExtension;
				}
			}
			else
			{
				return idealTempFile;
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
					"Mappalachia was unable to access the database located at " + dataFolder + databaseFileName + ".\n\n" +
					genericExceptionHelpText +
					"Mappalachia must exit.\n\n" + e);

				Environment.Exit(1);
				return null;
			}
		}

		// Open the current map image in the default viewer
		public static void OpenImage(Image image)
		{
			string tempImageFilePath = GetNewTempImageFilePath();

			try
			{
				Directory.CreateDirectory(tempImageFolder);
				image.Save(tempImageFilePath);
			}
			catch (Exception e)
			{
				Notify.Error(
					"Mappalachia was unable to store a temporary copy of the map image in its temporary folder at " + tempImageFilePath + ".\n\n" +
					genericExceptionHelpText +
					e);

				return;
			}

			try
			{
				Process.Start(tempImageFilePath);
			}
			catch (Exception e)
			{
				Notify.Error(
					"Mappalachia was unable to launch your default photo viewer to view the temporary map image at " + tempImageFilePath + ".\n\n" +
					genericExceptionHelpText +
					e);

				return;
			}
		}

		// Write the map image to the location with the file settings given
		public static void WriteToFile(string filePath, Image image, ImageFormat imageFormat, int jpegQuality)
		{
			try
			{
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
					"Mappalachia was unable to save your map to " + filePath + ".\n" +
					"Please try a different location.\n\n" +
					genericExceptionHelpText +
					e);

				return;
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
					"Mappalachia was unable to properly cleanup some temporary files left behind at " + tempImageFolder + ".\n" +
					"You may wish to remove the folder yourself, otherwise we will try again the next time Mappalachia is opened.\n\n" +
					genericExceptionHelpText +
					e);

				return;
			}
		}

		// Returns the appropriate background image for the map based on current settings
		public static Image GetImageForSpace(Space space)
        {
			if (space.IsWorldspace())
            {
				// Return the military map if this is Appalachia and they requested military
				if (SettingsMap.layerMilitary && space.editorID.Equals("Appalachia"))
				{
					return GetImageAppalachiaMilitary();
				}

				// Otherwise look for the background image for the worldspace
				string editorID = space.editorID;
				string filepath = imgFolder + editorID + worldspaceMapFileExtension;

				try
                {
					// Cache the image if not already
					if (!worldspaceMapImageCache.ContainsKey(editorID))
					{
						worldspaceMapImageCache.Add(editorID, Image.FromFile(filepath));
					}

					return new Bitmap(worldspaceMapImageCache[editorID]);
				}
				catch (FileNotFoundException e)
                {
					Notify.Error("Mappalachia was unable to find a background map image for the worldspace '" + editorID + "'.\n" + e);
					return EmptyMapBackground();
				}
				catch (Exception e)
				{
					Notify.Error("Mappalachia was unable to read the file '" + filepath + "'.\n" + genericExceptionHelpText + e);
					return EmptyMapBackground();
				}
			}

			// Interior so return empty background
			else
            {
				return EmptyMapBackground();
			}
        }

		public static Image GetImageAppalachiaMilitary()
		{
			if (imageMapMilitary == null)
			{
                try
                {
					imageMapMilitary = Image.FromFile(imgFolder + imgFileNameMapMilitary);
				}
				catch (Exception e)
				{
					Notify.Error("Mappalachia was unable to read the file '" + imgFileNameMapMilitary + "'.\n" + genericExceptionHelpText + e);
					imageMapMilitary = EmptyMapBackground();
				}
			}

			return new Bitmap(imageMapMilitary);
		}

		public static Image EmptyMapBackground()
		{
			return new Bitmap(Map.mapDimension, Map.mapDimension);
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
					"Mappalachia encountered an error while reading your user preferences file from " + settingsFileName + ". Your settings will be reset to default.\n" +
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
					"Mappalachia encountered an error while saving your preferences file to " + settingsFileName + ". Your settings will not be saved.\n" +
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
						"Mappalachia was unable to load the file " + fontPath + " and as a result your maps cannot be drawn.\n" +
						genericExceptionHelpText +
						"Mappalachia must exit.\n\n" + e);

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
	}
}
