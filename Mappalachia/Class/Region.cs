using System.Collections.Generic;
using System.Linq;

namespace Mappalachia
{
	// Represents an entire REGN
	// Parallels a MapDataPoint, but unique as it represents a collection of lists of coordinates
	public class Region
	{
		List<RegionPoint> points;

		public Region(List<RegionPoint> points)
		{
			this.points = points;
		}

		// Returns the count of unique subregions in the region
		public int GetSubRegionCount()
		{
			return points.Max(p => p.subRegionNumber + 1);
		}

		// Returns the list of verts/points comprising the subregion with the given number
		// Region points are 0-indexed
		public List<RegionPoint> GetSubRegionPoints(int subRegionNumber)
		{
			return points.Where(p => p.subRegionNumber == subRegionNumber).ToList();
		}
	}
}
