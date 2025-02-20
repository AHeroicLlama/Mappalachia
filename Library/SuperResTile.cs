namespace Library
{
	public class SuperResTile(Space space, int xCenter, int yCenter)
	{
		public Space Space { get; } = space;

		public int XCenter { get; } = xCenter;

		public int YCenter { get; } = yCenter;

		public int GetXID()
		{
			return (XCenter - Common.TileWidth) / Common.SuperResTileSize;
		}

		public int GetYID()
		{
			return (YCenter - Common.TileWidth) / Common.SuperResTileSize;
		}
	}
}
