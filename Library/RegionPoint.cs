namespace Library
{
	public class RegionPoint(Region parentRegion, Space space, Coord point, uint regionIndex, uint coordIndex)
		: Instance(parentRegion, space, point, 0, string.Empty, null, LockLevel.None, null)
	{
		public uint RegionIndex { get; } = regionIndex;

		public uint CoordIndex { get; } = coordIndex;
	}
}
