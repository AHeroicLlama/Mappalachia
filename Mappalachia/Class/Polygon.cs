using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Mappalachia.Class
{
	public class Polygon
	{
		List<PointF> verts;
		public PointF initialPoint;

		public Polygon(List<PointF> verts)
		{
			this.verts = verts;
			initialPoint = verts[0];
		}

		public Polygon(PointF vert)
		{
			verts = new List<PointF>();
			AddVert(vert);
			initialPoint = verts[0];
		}

		public List<PointF> GetVerts()
		{
			return verts;
		}

		public void AddVert(PointF newVert)
		{
			verts.Add(newVert);
		}

		public void RemoveVert(PointF vert)
		{
			verts.Remove(vert);
		}

		public PointF GetCentroid()
		{
			return new PointF(verts.Average(point => point.X), verts.Average(point => point.Y));
		}

		// Transforms the polygon into one with points closer than the threshold merged into a single point
		public void ReduceResolution(float threshold)
		{
			List<PointF> newPolygon = new List<PointF>();

			for (int i = 0; i < verts.Count; i++)
			{
				if (GeometryHelper.Pythagoras(GeometryHelper.GetCyclicItem(verts, i), GeometryHelper.GetCyclicItem(verts, i + 1)) > threshold)
				{
					newPolygon.Add(verts[i]);
				}
			}

			// If we discarded all points, return a single point representing them all
			if (newPolygon.Count == 0)
			{
				newPolygon = new List<PointF>() { GetCentroid() };
			}

			verts = newPolygon;
		}

		// Returns the distance of the furthest point from the given point
		public float GetFurthestVertDist(PointF point)
		{
			return verts.Max(vert => GeometryHelper.Pythagoras(point, vert));
		}

		public double GetArea()
		{
			double area = 0;
			int b;

			for (int i = 0; i < verts.Count; i++)
			{
				b = (i + 1) % verts.Count;

				area += verts[i].X * verts[b].Y;
				area -= verts[i].Y * verts[b].X;
			}

			return Math.Abs(area / 2);
		}

		public Polygon GetConvexHull()
		{
			if (verts.Count == 1)
			{
				return new Polygon(verts[0]);
			}

			// Monotone Chain https://en.wikibooks.org/wiki/Algorithm_Implementation/Geometry/Convex_hull/Monotone_chain
			int k = 0;
			List<PointF> hull = new List<PointF>(new PointF[2 * verts.Count]);

			verts.Sort((a, b) =>
					a.X != b.X ? a.X.CompareTo(b.X) : a.Y.CompareTo(b.Y));

			// Build lower hull
			for (int i = 0; i < verts.Count; ++i)
			{
				while (k >= 2 && GeometryHelper.VectorCrossProduct(hull[k - 2], hull[k - 1], verts[i]) <= 0)
				{
					k--;
				}

				hull[k++] = verts[i];
			}

			// Build upper hull
			for (int i = verts.Count - 2, t = k + 1; i >= 0; i--)
			{
				while (k >= t && GeometryHelper.VectorCrossProduct(hull[k - 2], hull[k - 1], verts[i]) <= 0)
				{
					k--;
				}

				hull[k++] = verts[i];
			}

			return new Polygon(hull.Take(k - 1).ToList());
		}
	}
}
