namespace Mappalachia
{
	enum BackgroundImageType
	{
		Render,
		Menu,
		Military,
	}

	public static class Map
	{
		public static Image Draw()
		{
			return new Bitmap(FileIO.GetImageForSpace(Settings.CurrentSpace));
		}
	}
}
