namespace Mappalachia
{
	enum BackgroundImageType
	{
		Render,
		Menu,
		Military,
		None,
	}

	enum LegendStyle
	{
		Normal,
		Extended,
		None,
	}

	public static class Map
	{
		public static async Task<Image> Draw()
		{
			return new Bitmap(Settings.CurrentSpace.GetBackgroundImage());
		}
	}
}
