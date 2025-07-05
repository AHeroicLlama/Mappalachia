using System.Drawing;
using static Library.Common;

namespace Library
{
	public class SpotlightTile(Space space, int xCenter, int yCenter)
	{
		// Returns all the tiles which exist within the rectangle of world coordinates
		public static List<SpotlightTile> GetTilesInRect(RectangleF rect, Space space)
		{
			List<SpotlightTile> tiles = new List<SpotlightTile>();

			// Loop over every tile for the rectangle
			for (int x = SnapToTileCenter(rect.Left); x <= SnapToTileCenter(rect.Right); x += TileWidth)
			{
				for (int y = SnapToTileCenter(rect.Bottom); y <= SnapToTileCenter(rect.Top); y += TileWidth)
				{
					tiles.Add(new SpotlightTile(space, x, y));
				}
			}

			return tiles;
		}

		// Returns an int representing the center coord of the tile (or rather lines of tiles) which exist at the coordinate dimension
		static int SnapToTileCenter(float position)
		{
			// Start with the passed position
			// Then remove the remainder of the tile width (hence rounding down to the nearest tile edge)
			// Then move the point back to the center of the tile, away from zero
			return (int)(position - (position % TileWidth) + (TileRadius * (position < 0 ? -1 : 1)));
		}

		public Space Space { get; } = space;

		public int XCenter { get; } = xCenter;

		public int YCenter { get; } = yCenter;

		public int XId => (XCenter - TileRadius) / TileWidth;

		public int YId => (YCenter - TileRadius) / TileWidth;

		// Returns if the coordinate exists within the tile
		public bool ContainsPoint(Coord coord)
		{
			return coord.X > XCenter - TileRadius &&
				coord.X < XCenter + TileRadius &&
				coord.Y > YCenter - TileRadius &&
				coord.Y < YCenter + TileRadius;
		}

		// Returns the rectangleF in world coordinates which defines the location of this tile
		public RectangleF GetRectangle()
		{
			return new RectangleF(XCenter - TileRadius, YCenter + TileRadius, TileWidth, TileWidth);
		}

		public override bool Equals(object? obj)
		{
			if (obj == null)
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			SpotlightTile other = (SpotlightTile)obj;

			return XCenter == other.XCenter && YCenter == other.YCenter && Space.Equals(other.Space);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(XCenter, YCenter, Space);
		}
	}
}
