using Library;

namespace Mappalachia
{
	static class FileIO
	{
		static string BackgroundImageFileType { get; } = ".jpg";

		public static Image GetBackgroundImage(this Space space, BackgroundImageType preferredBackgroundImageType = BackgroundImageType.Menu)
		{
			if (space.IsWorldspace)
			{
				if (space.IsAppalachia() && preferredBackgroundImageType == BackgroundImageType.Military)
				{
					return new Bitmap(Paths.WorldspaceImgPath + space.EditorID + "_military" + BackgroundImageFileType);
				}

				if (preferredBackgroundImageType == BackgroundImageType.Render)
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
