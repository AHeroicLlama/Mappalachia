using System.Drawing;

namespace Library
{
	public class SuperResTile(Space space, int xCenter, int yCenter)
	{
		// Returns all the tiles which exist within the rectangle of world coordinates
		public static List<SuperResTile> GetTilesInRect(RectangleF rect, Space space)
		{
			List<SuperResTile> tiles = new List<SuperResTile>();

			// The maximum possible coordinate which could be captured in a tile
			double minX = rect.Left - Common.TileWidth;
			double maxX = rect.Right + Common.TileWidth;
			double minY = rect.Bottom - Common.TileWidth;
			double maxY = rect.Top + Common.TileWidth;

			// The center coord of the edge tiles: the coordinate edge, rounded down, minus the tile radius
			int minXCenter = (int)(minX - (minX % Common.TileWidth)) - Common.TileRadius;
			int maxXCenter = (int)(maxX - (maxX % Common.TileWidth)) - Common.TileRadius;
			int minYCenter = (int)(minY - (minY % Common.TileWidth)) - Common.TileRadius;
			int maxYCenter = (int)(maxY - (maxY % Common.TileWidth)) - Common.TileRadius;

			// Loop over every tile for the space
			for (int x = minXCenter; x <= maxXCenter; x += Common.TileWidth)
			{
				for (int y = minYCenter; y <= maxYCenter; y += Common.TileWidth)
				{
					tiles.Add(new SuperResTile(space, x, y));
				}
			}

			return tiles;
		}

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

		// Returns a rectangle which represents the tile's bounds in world coordinates
		public RectangleF GetRectangle()
		{
			return new RectangleF(XCenter - Common.TileRadius, YCenter + Common.TileRadius, Common.TileWidth, Common.TileWidth);
		}
	}
}
