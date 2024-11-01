﻿using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Xml;
using Library;

namespace Preprocessor
{
	internal partial class Preprocessor
	{
		static string BackgroundImageFileType { get; } = ".jpg";

		static string MaskImageFileType { get; } = ".png";

		static string MapMarkerImageFileType { get; } = ".svg";

		static uint MinRenderedCellImageSizeKB { get; } = 500;

		static uint MaxRenderedCellImageSizeKB { get; } = 6000;

		static uint MinMapMarkerImageSizeBytes { get; } = 500;

		static uint MaxMapMarkerImageSizeBytes { get; } = 12000;

		static float MaxBlackPixelsPerc { get; } = 90f;

		static void ValidateImageAssets()
		{
			Console.WriteLine("Validating image assets");

			// Collect all Spaces and MapMarkers. Group the map markers by icon
			List<MapMarker> mapMarkers = CommonDatabase.GetMapMarkers(Connection, "SELECT * FROM MapMarker GROUP BY icon ORDER BY icon ASC;");
			List<Space> spaces = CommonDatabase.GetSpaces(Connection, "SELECT * FROM Space ORDER BY isWorldspace DESC, spaceEditorID ASC;");

			// Perform file checks for all spaces
			spaces.ForEach(ValidateSpace);

			// Inverse of above, check there are no extraneous files
			// First check each cell file against a cell in the database
			foreach (string file in Directory.GetFiles(BuildTools.CellPath))
			{
				string expectedEditorId = Path.GetFileNameWithoutExtension(file);

				if (!spaces.Where(space => space.EditorID == expectedEditorId && !space.IsWorldspace).Any())
				{
					FailValidation($"File {file} has no corresponding Cell and should be deleted.");
				}
			}

			// Check the count of images in the cell image folder matches the count of cells in the DB
			int expectedCellImageFiles = spaces.Where(space => !space.IsWorldspace).Count();
			int actualCellImageFiles = Directory.GetFiles(BuildTools.CellPath).Length;
			if (actualCellImageFiles != expectedCellImageFiles)
			{
				FailValidation($"Too {(actualCellImageFiles < expectedCellImageFiles ? "few" : "many")} files in the cell image folder. Expected {expectedCellImageFiles}, found {actualCellImageFiles}");
			}

			// Count 3 files per worldspace, plus 1 extra for Appalachia for the "military" map
			int expectedWorldspaceImageFiles = (spaces.Where(space => space.IsWorldspace).Count() * 3) + spaces.Where(space => space.IsAppalachia()).Count();
			int actualWorldspaceImageFiles = Directory.GetFiles(BuildTools.WorldPath).Length;
			if (actualWorldspaceImageFiles != expectedWorldspaceImageFiles)
			{
				FailValidation($"Too {(actualWorldspaceImageFiles < expectedWorldspaceImageFiles ? "few" : "many")} files in the worldspace image folder. Expected {expectedWorldspaceImageFiles}, found {actualWorldspaceImageFiles}");
			}

			// Main check for Map Markers
			mapMarkers.ForEach(ValidateMapMarker);

			// Similarly as above, do the same file count check for map markers
			int expectedMapMarkerImageFiles = mapMarkers.Count;
			int actualMapMarkerImageFiles = Directory.GetFiles(BuildTools.MapMarkerPath).Length;
			if (actualMapMarkerImageFiles != expectedMapMarkerImageFiles)
			{
				FailValidation($"Too {(actualMapMarkerImageFiles < expectedMapMarkerImageFiles ? "few" : "many")} mapmarkers in the mapmarker image folder. Expected {expectedMapMarkerImageFiles}, found {actualMapMarkerImageFiles}");
			}
		}

		static bool ValidateSpaceImageExists(Space space, string path)
		{
			if (!File.Exists(path))
			{
				FailValidation($"Image for space {space.EditorID} was not found at {path}");
				return false;
			}

			return true;
		}

		static bool ValidateMapMarkerImageExists(MapMarker marker, string path)
		{
			if (!File.Exists(path))
			{
				FailValidation($"Image for marker icon {marker.Icon} was not found at {path}");
				return false;
			}

			return true;
		}

		// Validate that there is not too many black pixels, which would indicate that the zoom or offset is incorrect
		static void ValidateImageBlackPx(Space space, string path)
		{
			using Bitmap bitmap = (Bitmap)Image.FromFile(path);

			// We copy the bitmap data out into a byte array, because accessing the pixels from the bitmap object is very slow
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
			int size = bitmapData.Stride * bitmap.Height;
			byte[] rgbValues = new byte[size];
			Marshal.Copy(bitmapData.Scan0, rgbValues, 0, size);
			int stride = bitmapData.Stride;

			int blackPxCount = 0;
			for (int x = 0; x < bitmapData.Height; x++)
			{
				for (int y = 0; y < bitmapData.Width; y++)
				{
					// If the R, G, and B component are 0 - if the pixel is black
					if (rgbValues[(x * stride) + (y * 3) + 2] == 0 &&
						rgbValues[(x * stride) + (y * 3) + 1] == 0 &&
						rgbValues[(x * stride) + (y * 3)] == 0)
					{
						blackPxCount++;
					}
				}
			}

			bitmap.UnlockBits(bitmapData);

			float blackPxPercent = blackPxCount / (bitmap.Width * (float)bitmap.Height) * 100;

			if (blackPxPercent > MaxBlackPixelsPerc)
			{
				FailValidation($"{space.EditorID} has too many black pixels ({blackPxPercent})%");
			}
		}

		static void ValidateImageDimensions(Space space, string path)
		{
			using Image? image = Image.FromFile(path);

			// Verify the file dimensions are within the pre-set ranges
			if (image.Width != Misc.MapImageResolution || image.Height != Misc.MapImageResolution)
			{
				FailValidation($"Image {path} is not the expected dimension of {Misc.MapImageResolution}x{Misc.MapImageResolution}");
			}
		}

		static void ValidateImageFileSize(Space space, string path)
		{
			if (space.IsWorldspace)
			{
				return;
			}

			int fileSizeKB = (int)(new FileInfo(path).Length / Misc.Kilobyte);

			if (fileSizeKB < MinRenderedCellImageSizeKB || fileSizeKB > MaxRenderedCellImageSizeKB)
			{
				FailValidation($"Image for {space.DisplayName} ({path}) appears to be an improper file size ({fileSizeKB}KB)");
			}
		}

		static void ValidateMapMarkerFileSize(MapMarker marker, string path)
		{
			int fileSize = (int)new FileInfo(path).Length;

			if (fileSize < MinMapMarkerImageSizeBytes || fileSize > MaxMapMarkerImageSizeBytes)
			{
				FailValidation($"Mapmarker {marker.Icon} ({path}) appears to be an improper file size ({fileSize} Bytes)");
			}
		}

		static void ValidateSVG(MapMarker marker, string path)
		{
			try
			{
				XmlDocument xml = new XmlDocument();
				xml.Load(path);
			}
			catch (XmlException e)
			{
				FailValidation($"{path} is not valid XML: {e.Message}");
			}
		}

		// Validates the background images for the given Space
		static void ValidateSpace(Space space)
		{
			Console.WriteLine($"Background image(s): {space.EditorID}");
			string filePath = (space.IsWorldspace ? BuildTools.WorldPath : BuildTools.CellPath) + space.EditorID + BackgroundImageFileType;

			if (!ValidateSpaceImageExists(space, filePath))
			{
				return;
			}

			ValidateImageDimensions(space, filePath);
			ValidateImageFileSize(space, filePath);
			ValidateImageBlackPx(space, filePath);

			// There are several additional files for Worldspaces
			if (space.IsWorldspace)
			{
				string watermaskFilePath = BuildTools.WorldPath + space.EditorID + "_waterMask" + MaskImageFileType;
				string menuMapPath = BuildTools.WorldPath + space.EditorID + "_menu" + BackgroundImageFileType;

				if (!ValidateSpaceImageExists(space, watermaskFilePath) | !ValidateSpaceImageExists(space, menuMapPath))
				{
					return;
				}

				ValidateImageDimensions(space, watermaskFilePath);
				ValidateImageDimensions(space, menuMapPath);

				// Appalachia specifically also has the 'military' map
				if (space.IsAppalachia())
				{
					string militaryMapPath = BuildTools.WorldPath + space.EditorID + "_military" + BackgroundImageFileType;

					if (!ValidateSpaceImageExists(space, militaryMapPath))
					{
						return;
					}

					ValidateImageDimensions(space, militaryMapPath);
				}
			}
		}

		static void ValidateMapMarker(MapMarker mapMarker)
		{
			Console.WriteLine($"Map Icon: {mapMarker.Icon}");
			string filePath = BuildTools.MapMarkerPath + mapMarker.Icon + MapMarkerImageFileType;

			if (!ValidateMapMarkerImageExists(mapMarker, filePath))
			{
				return;
			}

			ValidateMapMarkerFileSize(mapMarker, filePath);
			ValidateSVG(mapMarker, filePath);
		}
	}
}
