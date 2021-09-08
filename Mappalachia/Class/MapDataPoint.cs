namespace Mappalachia
{
	// A single data point to be mapped (one totally unique instance of an object)
	public class MapDataPoint
	{
		public double x;
		public double y;
		public int z; // Height - currently only used in Cell mode
		public double weight; // The magnitude/importance of this plot (EG 2.0 may represent 2x scrap from a single junk, or 0.33 may represent a 33% chance of spawning)
		public string primitiveShape; // The name of the primitive shape which describes this item (only typically applicable to ACTI)
		public double boundX; // The bounds of the primitiveShape
		public double boundY;
		public int boundZ;
		public int rotationZ;

		public MapDataPoint(int x, int y, int z)
		{
			Initialize(x, y, z);
		}

		public MapDataPoint(int x, int y, int z, string primitiveShape, int boundX, int boundY, int boundZ, int rotationZ)
		{
			Initialize(x, y, z);

			this.primitiveShape = primitiveShape;
			this.boundX = boundX / Map.scaling;
			this.boundY = boundY / Map.scaling;
			this.boundZ = boundZ;
			this.rotationZ = rotationZ;

			// Special case to ensure Line volumes (Default width 16 units) have enough pixel width to still be drawn
			if (primitiveShape == "Line" && boundY == 16)
			{
				this.boundY = Map.minVolumeDimension;
			}
		}

		void Initialize(int x, int y, int z)
		{
			this.x = (x / Map.scaling) + (Map.mapDimension / 2d) + Map.xOffset;
			this.y = (-y / Map.scaling) + (Map.mapDimension / 2d) + Map.yOffset;
			this.z = z;

			// Default weight, can be assigned to later
			weight = 1d;
		}
	}
}
