using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Mappalachia.Class
{
	public static class GeometryHelper
	{
		public static float Pythagoras(double a, double b)
		{
			return (float)Math.Sqrt((a * a) + (b * b));
		}

		public static float Pythagoras(PointF a, PointF b)
		{
			return Pythagoras(Math.Abs(a.X - b.X), Math.Abs(a.Y - b.Y));
		}

		public static double AreaOfPolygon(List<PointF> polygon)
		{
			double area = 0;
			int b;

			for (int i = 0; i < polygon.Count; i++)
			{
				b = (i + 1) % polygon.Count;

				area += polygon[i].X * polygon[b].Y;
				area -= polygon[i].Y * polygon[b].X;
			}

			return Math.Abs(area / 2);
		}

		// Monotone Chain https://en.wikibooks.org/wiki/Algorithm_Implementation/Geometry/Convex_hull/Monotone_chain
		public static List<PointF> GetConvexHull(List<PointF> polygon)
		{
			int k = 0;
			List<PointF> hull = new List<PointF>(new PointF[2 * polygon.Count]);

			polygon.Sort((a, b) =>
				 a.X != b.X ? a.X.CompareTo(b.X) : a.Y.CompareTo(b.Y));

			// Build lower hull
			for (int i = 0; i < polygon.Count; ++i)
			{
				while (k >= 2 && VectorCrossProduct(hull[k - 2], hull[k - 1], polygon[i]) <= 0)
				{
					k--;
				}

				hull[k++] = polygon[i];
			}

			// Build upper hull
			for (int i = polygon.Count - 2, t = k + 1; i >= 0; i--)
			{
				while (k >= t && VectorCrossProduct(hull[k - 2], hull[k - 1], polygon[i]) <= 0)
				{
					k--;
				}

				hull[k++] = polygon[i];
			}

			return hull.Take(k - 1).ToList();
		}

		// Cross-product o->a and o->b
		public static double VectorCrossProduct(PointF o, PointF a, PointF b)
		{
			return ((a.X - o.X) * (b.Y - o.Y)) - ((a.Y - o.Y) * (b.X - o.X));
		}
	}
}
