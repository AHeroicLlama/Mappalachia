namespace Mappalachia
{
	//A single data point to be mapped (one totally unique instance of an object)
	public class MapDataPoint
	{
		public double x;
		public double y;
		public double weight; //The magnitude/importance of this plot (EG 2.0 may represent 2x scrap from a single junk, or 0.33 may represent a 33% chance of spawning)
		public string primitiveShape; //The name of the primitive shape which describes this item (only typically applicable to ACTI)
		public double? boundX; //The bounds of the primitiveShape
		public double? boundY;

		public MapDataPoint(int x, int y, double weight, string primitiveShape, int? boundX, int? boundY)
		{
			//Scale the coordinate point down from worldspace coordinates to image coordinates, and map them from their 4-axis grid to a 2-axis grid
			this.x = (x / Map.xScale) + (Map.mapDimension / 2d) + Map.xOffset;
			this.y = (y / Map.YScale) + (Map.mapDimension / 2d) + Map.yOffset;

			this.weight = weight;

			this.primitiveShape = primitiveShape;
			this.boundX = boundX / Map.xScale;
			this.boundY = boundY / Map.YScale;
		}
	}
}