using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Xml;
using Library;
using static Library.BuildTools;
using static Library.Common;

namespace Preprocessor
{
	internal partial class Preprocessor
	{
		static uint MinRenderedCellImageSizeKB { get; } = 500;

		static uint MaxRenderedCellImageSizeKB { get; } = 6500;

		static uint MinTileImageSizeKB { get; } = 85;

		static uint MaxTileImageSizeKB { get; } = 9000;

		static uint MinMapMarkerImageSizeBytes { get; } = 500;

		static uint MaxMapMarkerImageSizeBytes { get; } = 16000;

		static float MaxBlackPixelsBackgroundPerc { get; } = 90f;

		static async void ValidateImageAssets()
		{
			Console.WriteLine("Validating image assets");

			// Collect all Spaces and MapMarkers. Group the map markers by icon
			List<MapMarker> mapMarkers = await CommonDatabase.GetMapMarkers(Connection, "SELECT * FROM MapMarker GROUP BY icon ORDER BY icon ASC;");
			List<Space> spaces = await CommonDatabase.GetAllSpaces(GetNewConnection());

			// Perform file checks for all spaces
			Parallel.ForEach(spaces, space =>
			{
				ValidateSpace(space);
			});

			// Inverse of above (excluding super res), check there are no extraneous files
			// First check each cell file against a cell in the database
			Parallel.ForEach(Directory.GetFiles(CellPath), file =>
			{
				string expectedEditorId = Path.GetFileNameWithoutExtension(file);

				if (!spaces.Where(space => space.EditorID == expectedEditorId && !space.IsWorldspace).Any())
				{
					FailValidation($"File {file} has no corresponding Cell and should be deleted.");
				}
			});

			// Check the count of images in the cell image folder matches the count of cells in the DB
			int expectedCellImageFiles = spaces.Where(space => !space.IsWorldspace).Count();
			int actualCellImageFiles = Directory.GetFiles(CellPath).Length;

			if (actualCellImageFiles != expectedCellImageFiles)
			{
				FailValidation($"Too {(actualCellImageFiles < expectedCellImageFiles ? "few" : "many")} files in the cell image folder. Expected {expectedCellImageFiles}, found {actualCellImageFiles}");
			}

			// Count 3 files per worldspace, plus 1 extra for Appalachia for the "military" map
			int expectedWorldspaceImageFiles = (spaces.Where(space => space.IsWorldspace).Count() * 3) + spaces.Where(space => space.IsAppalachia()).Count();
			int actualWorldspaceImageFiles = Directory.GetFiles(WorldPath).Length;

			if (actualWorldspaceImageFiles != expectedWorldspaceImageFiles)
			{
				FailValidation($"Too {(actualWorldspaceImageFiles < expectedWorldspaceImageFiles ? "few" : "many")} files in the worldspace image folder. Expected {expectedWorldspaceImageFiles}, found {actualWorldspaceImageFiles}");
			}

			// Main check for Map Markers
			Parallel.ForEach(mapMarkers, ValidateMapMarker);

			// Unique case for door marker (not a MapMarker, not in the DB, but still an icon svg)
			Console.WriteLine("Icon: DoorMarker");
			string doorMarkerPath = ImagePath + "DoorMarker" + MapMarkerImageFileType;
			MapMarker doorMarker = new MapMarker("DoorMarker", string.Empty, 0, new Coord(0, 0));

			if (ValidateMapMarkerImageExists(doorMarker, doorMarkerPath))
			{
				ValidateMapMarkerFileSize(doorMarker, doorMarkerPath);
				ValidateSVG(doorMarkerPath);
			}

			// Similarly as above, do the same file count check for map markers
			int expectedMapMarkerImageFiles = mapMarkers.Count;
			int actualMapMarkerImageFiles = Directory.GetFiles(MapMarkerPath).Length;

			if (actualMapMarkerImageFiles != expectedMapMarkerImageFiles)
			{
				FailValidation($"Too {(actualMapMarkerImageFiles < expectedMapMarkerImageFiles ? "few" : "many")} mapmarkers in the mapmarker image folder. Expected {expectedMapMarkerImageFiles}, found {actualMapMarkerImageFiles}");
			}

			// Similarly, now check the super res structure for extraneous files
			CleanUpSuperRes();
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

		// Validate that there is not too many black pixels in the background image which would indicate that the zoom or offset is incorrect
		static void ValidateBackgroundImageBlackPx(Space space, string path)
		{
			float blackPxPercent = GetBlackPxPercent(path);

			if (blackPxPercent > MaxBlackPixelsBackgroundPerc)
			{
				FailValidation($"Background image for {space.EditorID} has too many black pixels ({blackPxPercent}%)");
			}
		}

		// Validate that the super res tile is not 100% black
		static void ValidateTileImageBlackPx(SuperResTile tile, string path)
		{
			float blackPxPercent = GetBlackPxPercent(path);

			if (blackPxPercent == 100f)
			{
				FailValidation($"Super res tile {tile.XId}, {tile.YId} for {tile.Space.EditorID} has too many black pixels ({blackPxPercent}%)");
			}
		}

		// Returns the percent of pixels in the image in the given file path which are pure black
		static float GetBlackPxPercent(string filePath)
		{
			using Bitmap bitmap = (Bitmap)Image.FromFile(filePath);

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
					// If the R, G, and B component are 0 (if the pixel is black)
					if (rgbValues[(x * stride) + (y * 3) + 2] == 0 &&
						rgbValues[(x * stride) + (y * 3) + 1] == 0 &&
						rgbValues[(x * stride) + (y * 3)] == 0)
					{
						blackPxCount++;
					}
				}
			}

			bitmap.UnlockBits(bitmapData);

			return blackPxCount / (bitmap.Width * (float)bitmap.Height) * 100;
		}

		static void ValidateImageDimensions(string path)
		{
			using Image? image = Image.FromFile(path);

			// Verify the file dimensions are within the pre-set ranges
			if (image.Width != MapImageResolution || image.Height != MapImageResolution)
			{
				FailValidation($"Image {path} is not the expected dimension of {MapImageResolution}x{MapImageResolution}");
			}
		}

		static void ValidateBackgroundImageFileSize(Space space, string path)
		{
			if (space.IsWorldspace)
			{
				return;
			}

			int fileSizeKB = (int)(new FileInfo(path).Length / Kilobyte);

			if (fileSizeKB < MinRenderedCellImageSizeKB || fileSizeKB > MaxRenderedCellImageSizeKB)
			{
				FailValidation($"Image for {space.DisplayName} ({path}) appears to be an improper file size ({fileSizeKB}KB)");
			}
		}

		static void ValidateTileImageFileSize(SuperResTile tile, string path)
		{
			int fileSizeKB = (int)(new FileInfo(path).Length / Kilobyte);

			if (fileSizeKB < MinTileImageSizeKB || fileSizeKB > MaxTileImageSizeKB)
			{
				FailValidation($"Tile Image for {tile.Space.DisplayName} ({path}) appears to be an improper file size ({fileSizeKB}KB)");
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

		static void ValidateSVG(string path)
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
			string filePath = (space.IsWorldspace ? WorldPath : CellPath) + space.EditorID + BackgroundImageFileType;

			if (ValidateSpaceImageExists(space, filePath))
			{
				ValidateBackgroundImageFileSize(space, filePath);
				ValidateImageDimensions(filePath);
				ValidateBackgroundImageBlackPx(space, filePath);
			}

			int i = 0;
			List<SuperResTile> tiles = space.GetTiles();

			// Validate Super Res tiles
			Parallel.ForEach(tiles, tile =>
			{
				Interlocked.Increment(ref i);

				string file = tile.GetFilePath();

				// We don't fail on a missing super res tile, because it may have been deleted due to being empty, or not rendered due to being outside the world border
				if (File.Exists(file))
				{
					Console.WriteLine($"Super res tile {i} of {tiles.Count}: {space.EditorID}");
					ValidateTileImageFileSize(tile, file);
					ValidateImageDimensions(file);
					ValidateTileImageBlackPx(tile, file);
				}
			});

			// There are several additional files for Worldspaces
			if (space.IsWorldspace)
			{
				string watermaskFilePath = WorldPath + space.EditorID + WaterMaskAddendum + MaskImageFileType;
				string menuMapPath = WorldPath + space.EditorID + BackgroundMenuAddendum + BackgroundImageFileType;

				if (!ValidateSpaceImageExists(space, watermaskFilePath) | !ValidateSpaceImageExists(space, menuMapPath))
				{
					return;
				}

				ValidateImageDimensions(watermaskFilePath);
				ValidateImageDimensions(menuMapPath);

				// Appalachia specifically also has the 'military' map
				if (space.IsAppalachia())
				{
					string militaryMapPath = WorldPath + space.EditorID + BackgroundMilitaryAddendum + BackgroundImageFileType;

					if (!ValidateSpaceImageExists(space, militaryMapPath))
					{
						return;
					}

					ValidateImageDimensions(militaryMapPath);
				}
			}
		}

		static void ValidateMapMarker(MapMarker mapMarker)
		{
			Console.WriteLine($"Map Icon: {mapMarker.Icon}");
			string filePath = MapMarkerPath + mapMarker.Icon + MapMarkerImageFileType;

			if (!ValidateMapMarkerImageExists(mapMarker, filePath))
			{
				return;
			}

			ValidateMapMarkerFileSize(mapMarker, filePath);
			ValidateSVG(filePath);
		}
	}
}
