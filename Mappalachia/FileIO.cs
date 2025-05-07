using Library;
using static Library.Common;

namespace Mappalachia
{
	static class FileIO
	{
		static Dictionary<string, Image> BackgroundImageCache { get; } = new Dictionary<string, Image>();

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

			// TODO - do we error or just return blank image?
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

			// TODO - do we error or just return the blank image?
			if (!File.Exists(path))
			{
				return new Bitmap(MapImageResolution, MapImageResolution);
			}

			return LoadImage(path);
		}
	}
}
