using System.Text.Json.Serialization;

namespace Library
{
	// Represents a Worldspace (WRLD) or Cell (CELL)
	public class Space(uint formID, string editorID, string displayName, bool isWorldspace, double centerX, double centerY, double maxRange)
		: Entity(formID, editorID, displayName, isWorldspace ? Signature.WRLD : Signature.CELL)
	{
		[JsonIgnore]
		public bool IsWorldspace { get; } = isWorldspace;

		[JsonIgnore]
		public double CenterX { get; } = centerX;

		[JsonIgnore]
		public double CenterY { get; } = centerY;

		[JsonIgnore]
		public double MaxRange { get; } = maxRange;

		// Accessed via variable name in the UI - provides the name for the Space select dropdown
		[JsonIgnore]
		public string FriendlyName => $"{DisplayName} ({EditorID})";

		public bool IsAppalachia()
		{
			return EditorID.Equals("APPALACHIA", StringComparison.OrdinalIgnoreCase);
		}

		public double GetRadius()
		{
			return MaxRange / 2d;
		}

		public double GetMinX()
		{
			return CenterX - GetRadius();
		}

		public double GetMaxX()
		{
			return CenterX + GetRadius();
		}

		public double GetMinY()
		{
			return CenterY - GetRadius();
		}

		public double GetMaxY()
		{
			return CenterY + GetRadius();
		}

		// Return the super res tiles for the space
		public List<SuperResTile> GetTiles()
		{
			List<SuperResTile> tiles = new List<SuperResTile>();

			// The maximum possible coordinate which could be captured in a tile
			double minX = GetMinX() - Common.TileWidth;
			double maxX = GetMaxX() + Common.TileWidth;
			double minY = GetMinY() - Common.TileWidth;
			double maxY = GetMaxY() + Common.TileWidth;

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
					tiles.Add(new SuperResTile(this, x, y));
				}
			}

			return tiles;
		}
	}
}
