using MappalachiaLibrary;
using Microsoft.Data.Sqlite;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Preprocessor
{
	internal partial class Preprocessor
	{
		const string BackgroundImageFileType = ".jpg";
		const string MaskImageFileType = ".png";
		const string MapMarkerImageFileType = ".svg";
		const uint MinRenderedCellImageSizeKB = 500;
		const uint MaxRenderedCellImageSizeKB = 6000;
		const float MaxBlackPixelsPerc = 90f;

		static void ValidateImageAssets()
		{
			Console.WriteLine("Validating image assets");

			ValidationFailures.Clear();
			SqliteCommand query = Connection.CreateCommand();
			SqliteDataReader reader;

			// Collect all spaces
			List<Space> spaces = new List<Space>();
			query.CommandText = "SELECT spaceEditorID, isWorldspace FROM Space";
			reader = query.ExecuteReader();
			while (reader.Read())
			{
				spaces.Add(new Space(reader.GetString(0), reader.GetInt32(1) == 1));
			}

			// Collect all map markers
			List<MapMarker> mapMarkers = new List<MapMarker>();
			query = Connection.CreateCommand();
			query.CommandText = "SELECT DISTINCT icon FROM MapMarker";
			reader = query.ExecuteReader();
			while (reader.Read())
			{
				mapMarkers.Add(new MapMarker(reader.GetString(0), string.Empty));
			}

			// Validate the image files for the database objects exist, and have expected parameters
			spaces.ForEach(ValidateSpaceImage);
			mapMarkers.ForEach(ValidateMapMarkerImage);

			// Inverse of above, check there are no extraneous files
			// TODO - check there are no extraneous image for cells we don't carry
		}

		static void ValidateSpaceImage(Space space)
		{
			string filePath = (space.IsWorldspace ? BuildPaths.GetImageWorldPath() : BuildPaths.GetImageCellPath()) + space.EditorID + BackgroundImageFileType;

			// Verify the file exists
			if (!File.Exists(filePath))
			{
				FailValidation($"Image for space {space.EditorID} was not found at {filePath}");
				return;
			}

			// Verify the file dimensions are within the pre-set ranges
			using (Image? image = Image.FromFile(filePath))
			{
				if (image.Width != Misc.MapImageResolution || image.Height != Misc.MapImageResolution)
				{
					FailValidation($"Image {filePath} is not the expected dimension of {Misc.MapImageResolution}x{Misc.MapImageResolution}");
					return;
				}

				// Validate that there is not too many black pixels, which would indicate that the zoom or offset is incorrect
				using (Bitmap bitmap = (Bitmap)image)
				{
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

					float blackPxPercent = blackPxCount / (Misc.MapImageResolution * (float)Misc.MapImageResolution) * 100;

					if (blackPxPercent > MaxBlackPixelsPerc)
					{
						FailValidation($"{space.EditorID} has too many black pixels ({blackPxPercent})%");
					}
				}
			}

			int fileSizeKB = (int)(new FileInfo(filePath).Length / Misc.Kilobyte);

			if (!space.IsWorldspace)
			{
				if (fileSizeKB < MinRenderedCellImageSizeKB || fileSizeKB > MaxRenderedCellImageSizeKB)
				{
					FailValidation($"Image {filePath} appears to be an improper file size ({fileSizeKB}KB)");
					return;
				}
			}

			// TODO worldspace-specific checks
		}

		static void ValidateMapMarkerImage(MapMarker mapMarker)
		{
			string filePath = BuildPaths.GetImageMapMarkerPath() + mapMarker.Icon + MapMarkerImageFileType;
			
			// TODO
		}
	}
}
