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

		// Core constructor
		public MapDataPoint(int x, int y, double weight)
		{
			Initialize(x, y, weight);
		}

		//2D Primitive shapes
		public MapDataPoint(int x, int y, double weight, string primitiveShape, int boundX, int boundY, int rotationZ)
		{
			Initialize(x, y, weight, primitiveShape, boundX, boundY, rotationZ);
		}

		//3D (Interior)
		public MapDataPoint(int x, int y, int z, double weight)
		{
			this.z = z;
			Initialize(x, y, weight);
		}

		//3D Primitive Shapes (Interior)
		public MapDataPoint(int x, int y, int z, double weight, string primitiveShape, int boundX, int boundY, int boundZ, int rotationZ)
		{
			this.z = z;
			this.boundZ = boundZ;
			Initialize(x, y, weight, primitiveShape, boundX, boundY, rotationZ);
		}

		// Core reference for all constructors
		void Initialize(int x, int y, double weight)
		{
			// Scale the coordinate point down from worldspace coordinates to image coordinates, and map them from their 4-axis grid to a 2-axis grid
			this.x = (x / Map.scaling) + (Map.mapDimension / 2d) + Map.xOffset;
			this.y = (y / Map.scaling) + (Map.mapDimension / 2d) + Map.yOffset;

			this.weight = weight;
		}

		//2D Primitive shapes
		void Initialize(int x, int y, double weight, string primitiveShape, int boundX, int boundY, int rotationZ)
		{
			// overloaded parts
			this.primitiveShape = primitiveShape;
			this.boundX = boundX / Map.scaling;
			this.boundY = boundY / Map.scaling;
			this.rotationZ = rotationZ;

			// Special case to ensure Line volumes (Default width 16 units) have enough pixel width to still be drawn/visible
			if (primitiveShape == "Line" && boundY == 16)
			{
				this.boundY = Map.minVolumeDimension;
			}

			Initialize(x, y, weight);
		}
	}
}
