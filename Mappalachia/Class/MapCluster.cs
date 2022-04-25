using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Mappalachia.Class
{
	public class MapCluster
	{
		List<MapDataPoint> members = new List<MapDataPoint>();
		public float xCenter;
		public float yCenter;
		public float xCentroid;
		public float yCentroid;

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
			xCenter = members.Average(member => member.x);
			yCenter = members.Average(member => member.y);
		}

		public double GetMemberCount()
		{
			return members.Count;
		}

		public double GetMemberWeight()
		{
			return members.Sum(member => member.weight);
		}

		public PointF GetCenter()
		{
			return new PointF(xCenter, yCenter);
		}

		public double GetDistance(MapDataPoint dataPoint)
		{
			return GetDistance(dataPoint.GetPoint());
		}

		public double GetDistance(PointF point)
		{
			return GeometryHelper.Pythagoras(Math.Abs(point.X - xCenter), Math.Abs(point.Y - yCenter));
		}

		public double GetCentroidDistance(PointF point)
		{
			return GeometryHelper.Pythagoras(Math.Abs(point.X - xCentroid), Math.Abs(point.Y - yCentroid));
		}

		public PointF GetCentroid()
		{
			return new PointF(xCentroid, yCentroid);
		}

		// Returns the distance of the furthest point from the centroid
		public float GetBoundingRadius()
		{
			return GeometryHelper.Pythagoras(GetCentroidFurthestPoint(), GetCentroid());
		}

		public PointF GetCentroidFurthestPoint()
		{
			return members.MaxBy(member => GeometryHelper.Pythagoras(Math.Abs(member.x - xCentroid), Math.Abs(member.y - yCentroid))).GetPoint();
		}
	}
}
