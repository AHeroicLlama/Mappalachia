using Library;

namespace Mappalachia
{
	static class FileIO
	{
		static string BackgroundImageFileType { get; } = ".jpg";

		public static Image GetBackgroundImage(this Space space, BackgroundImageType backgroundImageType)
		{
			if (backgroundImageType == BackgroundImageType.None)
			{
				return new Bitmap(Common.MapImageResolution, Common.MapImageResolution);
			}

			string path = (space.IsWorldspace ? Paths.WorldspaceImgPath : Paths.CellImgPath) + space.EditorID;

			switch (backgroundImageType)
			{
				case BackgroundImageType.Render:
					break;
				case BackgroundImageType.Menu:
					path += "_menu";
					break;
				case BackgroundImageType.Military:
					path += "_military";
					break;
				default:
					throw new Exception("Unexpected BackgroundImageType: " + backgroundImageType);
			}

			path += BackgroundImageFileType;

			// TODO - do we error or just return blank image?
			if (!File.Exists(path))
			{
				return new Bitmap(Common.MapImageResolution, Common.MapImageResolution);
			}

			return new Bitmap(path);
		}
	}
}
