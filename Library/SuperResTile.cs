namespace Library
{
	public class SuperResTile(Space space, int xCenter, int yCenter)
	{
		public Space Space { get; } = space;

		public int XCenter { get; } = xCenter;

		public int YCenter { get; } = yCenter;

		public int GetXID()
		{
			return (XCenter - Common.TileRadius) / Common.TileWidth;
		}

		public int GetYID()
		{
			return (YCenter - Common.TileRadius) / Common.TileWidth;
		}

		// Returns if the coordinate exists within the tile
		public bool ContainsPoint(Coord coord)
		{
			return coord.X > XCenter - Common.TileRadius &&
				coord.X < XCenter + Common.TileRadius &&
				coord.Y > YCenter - Common.TileRadius &&
				coord.Y < YCenter + Common.TileRadius;
		}
	}
}
