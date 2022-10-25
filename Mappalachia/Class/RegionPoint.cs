namespace Mappalachia
{
	// Represents a single coordinate in a region
	public class RegionPoint
	{
		public int subRegionNumber; // The subregion number (of the parent region) of this point (most regions have just one - number 0)
		public int pointNumber; // The index of this point/vertex in the individual subregion/region
		public int x;
		public int y;

		public RegionPoint(int subRegionNumber, int pointNumber, int x, int y)
		{
			this.subRegionNumber = subRegionNumber;
			this.pointNumber = pointNumber;
			this.x = x;
			this.y = y;
		}
	}
}
