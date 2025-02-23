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

		public List<List<RegionPoint>> GetAllSubRegions()
		{
			List<List<RegionPoint>> subRegions = new List<List<RegionPoint>>();

			for (uint i = 0; i < GetSubRegionCount(); i++)
			{
				subRegions.Add(GetSubRegion(i));
			}

			return subRegions;
		}

		// Ray casting algorithm over each subregion
		// TODO tests
		public bool ContainsPoint(Coord n)
		{
			foreach (List<RegionPoint> subRegion in GetAllSubRegions())
			{
				int count = subRegion.Count;
				bool inside = false;

				// Iterate all adjacent pairs of points of the subregion, wrapping around
				for (int i = 0, j = count - 1; i < count; j = i++)
				{
					Coord a = subRegion[i].Point;
					Coord b = subRegion[j].Point;

					// If n is not between a and b in y dimension, skip
					if ((a.Y > n.Y) == (b.Y > n.Y))
					{
						continue;
					}

					double intersectionX = a.X + ((b.X - a.X) * ((n.Y - a.Y) / (b.Y - a.Y)));

					// X is left of line, flip polarity of inside flag
					if (n.X < intersectionX)
					{
						inside = !inside;
					}
				}

				// Inside this subregion, no need to check others
				if (inside)
				{
					return true;
				}
			}

			// not inside any subregion
			return false;
		}

		public bool ContainsPoint(Instance instance)
		{
			return ContainsPoint(instance.Point);
		}
	}
}
