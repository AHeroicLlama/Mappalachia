using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Mappalachia.Class
{
	public class MapCluster
	{
		List<MapDataPoint> members = new List<MapDataPoint>();
		PointF center;

		// Create a new cluster with the initial single point
		public MapCluster(MapDataPoint mapDataPoint)
		{
			AddMember(mapDataPoint);
		}

		public void AddMember(MapDataPoint mapDataPoint)
		{
			mapDataPoint.SetClusterMembership(this);
			members.Add(mapDataPoint);
			RecalculateCenters();
		}

		public void RemoveMember(MapDataPoint mapDataPoint)
		{
			members.Remove(mapDataPoint);
			mapDataPoint.SetClusterMembership(null);

			if (members.Count > 0)
			{
				RecalculateCenters();
			}
		}

		public List<PointF> GetPoints()
		{
			return members.Select(member => new PointF(member.x, member.y)).ToList();
		}

		void RecalculateCenters()
		{
			center = new PointF (
				members.Average(member => member.x),
				members.Average(member => member.y));
		}

		public double GetMemberWeight()
		{
			return members.Sum(member => member.weight);
		}

		public double GetDistance(MapDataPoint dataPoint)
		{
			return GeometryHelper.Pythagoras(center, dataPoint.Get2DPoint());
		}

		public PointF GetCentroid()
		{
			return GeometryHelper.GetCentroid(GetPoints());
		}

		// Returns the distance of the furthest point from the centroid
		public float GetBoundingRadius()
		{
			PointF furthestPoint = members.MaxBy(member => GeometryHelper.Pythagoras(GetCentroid(), member.Get2DPoint())).Get2DPoint();
			return GeometryHelper.Pythagoras(furthestPoint, GetCentroid());
		}
	}
}
