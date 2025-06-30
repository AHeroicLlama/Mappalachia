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

			// Loop over every tile for the rectangle
			for (int x = SnapToTileCenter(rect.Left); x <= SnapToTileCenter(rect.Right); x += TileWidth)
			{
				for (int y = SnapToTileCenter(rect.Bottom); y <= SnapToTileCenter(rect.Top); y += TileWidth)
				{
					tiles.Add(new SuperResTile(space, x, y));
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

		// Returns the Coord of the top-left corner of the tile, in world coordinates
		public Coord GetTopLeft()
		{
			return new Coord(XCenter - TileRadius, YCenter + TileRadius);
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

			SuperResTile other = (SuperResTile)obj;

			return XCenter == other.XCenter && YCenter == other.YCenter && Space.Equals(other.Space);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(XCenter, YCenter, Space);
		}
	}
}
