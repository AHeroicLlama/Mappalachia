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

		static FileIO()
		{
			FontCollection.AddFontFile(Paths.FontPath);
		}

		// Return an image from the file path, or a cached version if loaded before
		static Image LoadImage(string path)
		{
			if (BackgroundImageCache.ContainsKey(path))
			{
				return BackgroundImageCache[path];
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

		// Returns the image for the icon of the mapMarker, uses caching
		public static Image GetMapMarkerImage(this MapMarker mapMarker)
		{
			string path = Paths.MapMarkersPath + mapMarker.Icon + MapMarkerImageFileType;

			if (MapMarkerIconImageCache.ContainsKey(mapMarker.Icon))
			{
				return MapMarkerIconImageCache[mapMarker.Icon];
			}

			Svg.SvgDocument document = Svg.SvgDocument.Open(path);
			Image marker = document.Draw((int)(document.Width * Map.MapMarkerIconScale), 0);

			MapMarkerIconImageCache[mapMarker.Icon] = marker;
			return marker;
		}
	}
}
