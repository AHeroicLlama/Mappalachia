using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing.Text;
using Library;

using static Library.Common;

namespace Mappalachia
{
	static class FileIO
	{
		static Dictionary<string, Image> BackgroundImageCache { get; } = new Dictionary<string, Image>();

		static Dictionary<string, Image> MapMarkerIconImageCache { get; } = new Dictionary<string, Image>();

		static PrivateFontCollection FontCollection { get; } = new PrivateFontCollection();

		static ImageCodecInfo JpegCodec { get; set; } = ImageCodecInfo.GetImageDecoders().Where(ic => ic.FormatID == ImageFormat.Jpeg.Guid).SingleOrDefault() ?? throw new Exception($"Failed to get jpeg codec");

		static FileIO()
		{
			FontCollection.AddFontFile(Paths.FontPath);
			CreateSavedMapsFolder();
		}

		// Return an image from the file path, or a cached version if loaded before
		static Image LoadImage(string path)
		{
			if (BackgroundImageCache.TryGetValue(path, out Image? value))
			{
				return value;
			}

			Image image = new Bitmap(path);
			BackgroundImageCache[path] = image;
			return image;
		}

		public static FontFamily GetFontFamily()
		{
			return FontCollection.Families.First();
		}

		public static Image GetBackgroundImage(this Space space, BackgroundImageType backgroundImageType)
		{
			if (backgroundImageType == BackgroundImageType.None)
			{
				return new Bitmap(MapImageResolution, MapImageResolution);
			}

			string path = (space.IsWorldspace ? Paths.WorldspaceImgPath : Paths.CellImgPath) + space.EditorID;

			switch (backgroundImageType)
			{
				case BackgroundImageType.Render:
					break;
				case BackgroundImageType.Menu:
					path += MenuAddendum;
					break;
				case BackgroundImageType.Military:
					path += MilitaryAddendum;
					break;
				default:
					throw new Exception("Unexpected BackgroundImageType: " + backgroundImageType);
			}

			path += BackgroundImageFileType;

			if (!File.Exists(path))
			{
				return new Bitmap(MapImageResolution, MapImageResolution);
			}

			return LoadImage(path);
		}

		public static Image GetWaterMask(this Space space)
		{
			if (!space.IsWorldspace)
			{
				return new Bitmap(MapImageResolution, MapImageResolution);
			}

			string path = Paths.WorldspaceImgPath + space.EditorID + WaterMaskAddendum + MaskImageFileType;

			if (!File.Exists(path))
			{
				return new Bitmap(MapImageResolution, MapImageResolution);
			}

			return LoadImage(path);
		}

		static string GetFileExtension(this ImageFormat format)
		{
			if (format == ImageFormat.Jpeg)
			{
				return ".jpg";
			}

			if (format == ImageFormat.Png)
			{
				return ".png";
			}

			throw new Exception($"Unexpected ImageFormat {format}");
		}

		public static void CreateSavedMapsFolder()
		{
			Directory.CreateDirectory(Paths.SavedMapsPath);
		}

		public static void QuickSave(Image image, MapSettings mapSettings)
		{
			CreateSavedMapsFolder();

			ImageFormat format = mapSettings.BackgroundImage == BackgroundImageType.None ? ImageFormat.Png : ImageFormat.Jpeg;
			string path = Paths.SavedMapsPath + "Mappalachia_QuickSave_" + DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss_FFF") + format.GetFileExtension();

			Save(image, format, path);
			Process.Start("explorer.exe", "/select," + path);
		}

		// Write image to file
		public static void Save(Image image, ImageFormat format, string path, int jpegQuality = 85)
		{
			if (format == ImageFormat.Png)
			{
				image.Save(path, ImageFormat.Png);
			}
			else if (format == ImageFormat.Jpeg)
			{
				EncoderParameters encoderParams = new EncoderParameters();
				encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, jpegQuality);

				image.Save(path, JpegCodec, encoderParams);
			}
		}

		// Returns the image for the icon of the mapMarker, uses caching
		public static Image GetMapMarkerImage(this MapMarker mapMarker)
		{
			string path = Paths.MapMarkersPath + mapMarker.Icon + MapMarkerImageFileType;

			if (MapMarkerIconImageCache.TryGetValue(mapMarker.Icon, out Image? value))
			{
				return value;
			}

			Svg.SvgDocument document = Svg.SvgDocument.Open(path);
			Image marker = document.Draw((int)(document.Width * Map.MapMarkerIconScale), 0);

			MapMarkerIconImageCache[mapMarker.Icon] = marker;
			return marker;
		}
	}
}
