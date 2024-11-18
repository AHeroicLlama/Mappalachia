using Library;

namespace Mappalachia
{
	static class FileIO
	{
		static string BackgroundImageFileType { get; } = ".jpg";

		public static Image GetImageForSpace(Space space, BackgroundImageType preferrredBackgroundImageType = BackgroundImageType.Menu)
		{
			if (space.IsWorldspace)
			{
				if (space.IsAppalachia() && preferrredBackgroundImageType == BackgroundImageType.Military)
				{
					return new Bitmap(Paths.WorldspaceImgPath + space.EditorID + "_military" + BackgroundImageFileType);
				}

				if (preferrredBackgroundImageType == BackgroundImageType.Render)
				{
					return new Bitmap(Paths.WorldspaceImgPath + space.EditorID + BackgroundImageFileType);
				}

				return new Bitmap(Paths.WorldspaceImgPath + space.EditorID + "_menu" + BackgroundImageFileType);
			}
			else
			{
				return new Bitmap(Paths.CellImgPath + space.EditorID + BackgroundImageFileType);
			}
		}
	}
}
