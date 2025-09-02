namespace Library
{
	public class RegionPoint(Region parentRegion, Coord point, uint subRegionIndex, uint coordIndex)
		: Instance(parentRegion, parentRegion.Space, point, 0, string.Empty, null, LockLevel.None, null)
	{
		// The index of the subregion of the parent region, to which this point belongs
		// Most regions are formed of a single enclosed polygon (all points subregion index 0)
		// However a few regions are formed of multiple separate polygons (subregion indexes 0 and up)
		public uint SubRegionIndex { get; } = subRegionIndex;

		// The index of this point (vertex) within its subregion (polygon), ascending clockwise
		public uint CoordIndex { get; } = coordIndex;
	}
}
