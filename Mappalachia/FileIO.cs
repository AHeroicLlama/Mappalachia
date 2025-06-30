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

		static Dictionary<SuperResTile, Image> SuperResTileImageCache { get; } = new Dictionary<SuperResTile, Image>();

		static PrivateFontCollection FontCollection { get; } = new PrivateFontCollection();

		static ImageCodecInfo JpgCodec { get; set; } = ImageCodecInfo.GetImageDecoders().SingleOrDefault(ic => ic.FormatID == ImageFormat.Jpeg.Guid) ?? throw new Exception($"Failed to get jpeg codec");

		static Image EmptyMapImage { get; } = new Bitmap(MapImageResolution, MapImageResolution);

		static FileIO()
		{
			FontCollection.AddFontFile(Paths.FontPath);
			CreateSavedMapsFolder();
		}

		static Image? doorMarker;

		public static Image DoorMarker
		{
			get
			{
				doorMarker ??= LoadSVGIcon(Paths.DoorMarkerPath);
				return doorMarker;
			}
		}

		public static FontFamily GetFontFamily()
		{
			return FontCollection.Families.First();
		}

		// Return the image for the super res tile, uses caching
		public static Image GetImage(this SuperResTile tile)
		{
			if (SuperResTileImageCache.TryGetValue(tile, out Image? value))
			{
				return value;
			}

			string path = $"{Paths.SuperResTilePath}{tile.Space.EditorID}\\{tile.GetXID()}.{tile.GetYID()}{SuperResTileImageFileType}";

			// This may be common as we do not pre-render tiles outside of the playable space.
			// TODO we need to handle informing of downloading them, if delivered as an 'addon' download.
			if (!File.Exists(path))
			{
				return EmptyMapImage;
			}

			Image image = new Bitmap(path);
			SuperResTileImageCache[tile] = image;

			return image;
		}

		// Return a background image from the file path, or a cached version if loaded before
		static Image LoadBackgroundImage(string path)
		{
			if (BackgroundImageCache.TryGetValue(path, out Image? value))
			{
				return value;
			}

			Image image = new Bitmap(path);
			BackgroundImageCache[path] = image;
			return image;
		}

		public static Image GetBackgroundImage(this Space space, BackgroundImageType backgroundImageType)
		{
			if (backgroundImageType == BackgroundImageType.None)
			{
				return EmptyMapImage;
			}

			string path = (space.IsWorldspace ? Paths.WorldspaceImgPath : Paths.CellImgPath) + space.EditorID;

			switch (backgroundImageType)
			{
				case BackgroundImageType.Render:
					break;
				case BackgroundImageType.Menu:
					path += BackgroundMenuAddendum;
					break;
				case BackgroundImageType.Military:
					path += BackgroundMilitaryAddendum;
					break;
				default:
					throw new Exception("Unexpected BackgroundImageType: " + backgroundImageType);
			}

			path += BackgroundImageFileType;

			if (!File.Exists(path))
			{
				return EmptyMapImage;
			}

			return LoadBackgroundImage(path);
		}

		public static Image GetWaterMask(this Space space)
		{
			if (!space.IsWorldspace)
			{
				return EmptyMapImage;
			}

			string path = Paths.WorldspaceImgPath + space.EditorID + WaterMaskAddendum + MaskImageFileType;

			if (!File.Exists(path))
			{
				return EmptyMapImage;
			}

			return LoadBackgroundImage(path);
		}

		// Returns the image for the icon of the mapMarker, uses caching
		public static Image GetMapMarkerImage(this MapMarker mapMarker)
		{
			string path = Paths.MapMarkersPath + mapMarker.Icon + MapMarkerImageFileType;

			if (MapMarkerIconImageCache.TryGetValue(mapMarker.Icon, out Image? value))
			{
				return value;
			}

			Image marker = LoadSVGIcon(path);

			MapMarkerIconImageCache[mapMarker.Icon] = marker;
			return marker;
		}

		// Return an Image of an SVG icon document from the path
		static Bitmap LoadSVGIcon(string path)
		{
			Svg.SvgDocument document = Svg.SvgDocument.Open(path);
			return document.Draw((int)(document.Width * Map.IconScale), 0);
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

		static string GetDateTimeString()
		{
			return DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss_FFF");
		}

		public static void CreateSavedMapsFolder()
		{
			Directory.CreateDirectory(Paths.SavedMapsPath);
		}

		// Save an image to the temp folder
		public static void TempSave(Image image, bool openAferSave = false)
		{
			Directory.CreateDirectory(Paths.TempPath);

			ImageFormat format = ImageFormat.Jpeg;
			string path = Paths.TempPath + "Mappalachia_Temp_" + GetDateTimeString() + format.GetFileExtension();

			Save(image, format, path, 100);

			if (openAferSave)
			{
				OpenURI(path);
			}
		}

		public static void QuickSave(Image image, Settings settings)
		{
			CreateSavedMapsFolder();

			ImageFormat format = settings.MapSettings.BackgroundImage == BackgroundImageType.None ? ImageFormat.Png : ImageFormat.Jpeg;
			string path = Paths.SavedMapsPath + GetRecommendedMapFileName(settings) + "_" + GetDateTimeString() + format.GetFileExtension();

			Save(image, format, path);
			Process.Start("explorer.exe", "/select," + path);
		}

		// Write image to file
		public static void Save(Image image, ImageFormat format, string path, int jpgQuality = 85)
		{
			if (format == ImageFormat.Png)
			{
				image.Save(path, ImageFormat.Png);
			}
			else if (format == ImageFormat.Jpeg)
			{
				EncoderParameters encoderParams = new EncoderParameters();
				encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, jpgQuality);

				image.Save(path, JpgCodec, encoderParams);
			}
		}

		public static ImageFormat GetImageFileTypeRecommendation(Settings settings)
		{
			return !settings.Space.IsWorldspace || settings.MapSettings.BackgroundImage == BackgroundImageType.None ? ImageFormat.Png : ImageFormat.Jpeg;
		}

		// Return a string that would make a good default map file name
		public static string GetRecommendedMapFileName(Settings settings)
		{
			// Try to use the map title for the file name, but filter and trim it first
			// If it's blank, use the space name instead
			string fileName = settings.MapSettings.Title.SanitizeForFileName();

			if (fileName.IsNullOrWhiteSpace())
			{
				fileName = $"Mappalachia Map of {settings.Space.DisplayName}".SanitizeForFileName();
			}

			return fileName;
		}

		// Remove invalid file name/path chars, trim whitespace, and remove trailing periods.
		static string SanitizeForFileName(this string potentialFileName)
		{
			return string.Concat(potentialFileName.Split(Path.GetInvalidFileNameChars())).Trim().TrimEnd('.');
		}

		public static void Cleanup()
		{
			if (Directory.Exists(Paths.TempPath))
			{
				try
				{
					Directory.Delete(Paths.TempPath, true);
				}

				// This may fail if an external image editor or the like has locked the file, but we don't care.
				catch
				{
				}
			}
		}
	}
}
