namespace Library
{
	public class Region(uint formID, string editorID)
		: Entity(formID, editorID, string.Empty, Signature.REGN)
	{
		public List<RegionPoint> Points { get; } = new List<RegionPoint>();

		public void AddPoint(RegionPoint point)
		{
			Points.Add(point);
		}

		public int GetSubRegionCount()
		{
			return Points.DistinctBy(p => p.RegionIndex).Count();
		}

		public List<RegionPoint> GetSubRegion(uint regionIndex)
		{
			return Points.Where(p => p.RegionIndex == regionIndex).ToList();
		}
	}
}
