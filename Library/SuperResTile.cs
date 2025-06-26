using System.Drawing;
using static Library.Common;

namespace Library
{
	public class SuperResTile(Space space, int xCenter, int yCenter)
	{
		// Returns all the tiles which exist within the rectangle of world coordinates
		public static List<SuperResTile> GetTilesInRect(RectangleF rect, Space space)
		{
			List<SuperResTile> tiles = new List<SuperResTile>();

			int minXCenter = (int)((rect.Left - TileRadius) - (rect.Left % TileWidth));
			int maxXCenter = (int)((rect.Right + TileRadius) - (rect.Right % TileWidth));

			int minYCenter = (int)((rect.Bottom - TileRadius) - (rect.Bottom % TileWidth));
			int maxYCenter = (int)((rect.Top + TileRadius) - (rect.Top % TileWidth));

			// Loop over every tile for the rectangle
			for (int x = minXCenter; x <= maxXCenter; x += TileWidth)
			{
				for (int y = minYCenter; y <= maxYCenter; y += TileWidth)
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
			return (XCenter - TileRadius) / TileWidth;
		}

		public int GetYID()
		{
			return (YCenter - TileRadius) / TileWidth;
		}

		// Returns if the coordinate exists within the tile
		public bool ContainsPoint(Coord coord)
		{
			return coord.X > XCenter - TileRadius &&
				coord.X < XCenter + TileRadius &&
				coord.Y > YCenter - TileRadius &&
				coord.Y < YCenter + TileRadius;
		}

		// Returns a rectangle which represents the tile's bounds in world coordinates
		public RectangleF GetRectangle()
		{
			return new RectangleF(XCenter - TileRadius, YCenter + TileRadius, TileWidth, TileWidth);
		}
	}
}
