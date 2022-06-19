using System.Collections.Generic;
using System.Linq;

namespace Mappalachia.Class
{
	public class MapCluster
	{
		public List<MapDataPoint> members = new List<MapDataPoint>();
		public Polygon polygon;

		// Create a new cluster with the initial single point
		public MapCluster(MapDataPoint mapDataPoint)
		{
			AddMember(mapDataPoint);
		}

		public Polygon GetPolygon()
		{
			return polygon;
		}

		public void AddMember(MapDataPoint mapDataPoint)
		{
			if (polygon == null)
			{
				polygon = new Polygon(mapDataPoint.Get2DPoint());
			}
			else
			{
				polygon.AddVert(mapDataPoint.Get2DPoint());
			}

			mapDataPoint.SetClusterMembership(this);
			members.Add(mapDataPoint);
		}

		public void RemoveMember(MapDataPoint mapDataPoint)
		{
			polygon.RemoveVert(mapDataPoint.Get2DPoint());
			members.Remove(mapDataPoint);
			mapDataPoint.SetClusterMembership(null);
		}

		public double GetMemberWeight()
		{
			return members.Sum(member => member.weight);
		}
	}
}
