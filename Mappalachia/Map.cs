using Library;

namespace Mappalachia
{
	public enum BackgroundImageType
	{
		Render,
		Menu,
		Military,
		None,
	}

	public enum LegendStyle
	{
		Normal,
		Extended,
		None,
	}

	public static class Map
	{
		public static async Task<Image> Draw(Space space, BackgroundImageType preferredBackgroundImageType)
		{
			// Filter for preferred bg images which are not applicable to the selected Space
			if (preferredBackgroundImageType == BackgroundImageType.Military && !space.IsAppalachia())
			{
				preferredBackgroundImageType = BackgroundImageType.Menu;
			}

			if (preferredBackgroundImageType == BackgroundImageType.Menu && !space.IsWorldspace)
			{
				preferredBackgroundImageType = BackgroundImageType.Render;
			}

			return new Bitmap(space.GetBackgroundImage(preferredBackgroundImageType));
		}
	}
}
