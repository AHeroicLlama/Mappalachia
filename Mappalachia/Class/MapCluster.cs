using System.Collections.Generic;
using System.Linq;

namespace Mappalachia.Class
{
	public class MapCluster
	{
		List<MapDataPoint> members = new List<MapDataPoint>();
		double xCenter;
		double yCenter;

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

		void RecalculateCenters()
		{
			xCenter = members.Average(member => member.x);
			yCenter = members.Average(member => member.y);
		}

		public double GetXCenter()
		{
			return xCenter;
		}

		public double GetYCenter()
		{
			return yCenter;
		}

		public int GetMemberCount()
		{
			return members.Count;
		}

		public List<MapDataPoint> GetMembers()
		{
			return members;
		}

		public double GetDistance(MapDataPoint dataPoint)
		{
			double xDist = System.Math.Abs(dataPoint.x - GetXCenter());
			double yDist = System.Math.Abs(dataPoint.y - GetYCenter());
			return DataHelper.Pythagoras(xDist, yDist);
		}
	}
}
