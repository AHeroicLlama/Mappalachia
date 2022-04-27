using System;
using System.Collections.Generic;
using System.Drawing;

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
			return Pythagoras(a.X - b.X, a.Y - b.Y);
		}

		// Cross-product o->a and o->b
		public static double VectorCrossProduct(PointF o, PointF a, PointF b)
		{
			return ((a.X - o.X) * (b.Y - o.Y)) - ((a.Y - o.Y) * (b.X - o.X));
		}

		// returns the angle formed at o where a->o becomes o->b
		public static double VectorAngle(PointF a, PointF o, PointF b)
		{
			return RadToDeg(Math.Atan2(b.Y - o.Y, b.X - o.X) -
							Math.Atan2(a.Y - o.Y, a.X - o.X));
		}

		public static double RadToDeg(double rad)
		{
			return rad * (180 / Math.PI);
		}

		// returns the nth item in a list as if it were cyclic (supports <0 or >n)
		public static T GetCyclicItem<T>(List<T> collection, int n)
		{
			n %= collection.Count;

			if (n < 0)
			{
				n = collection.Count + n;
			}

			return collection[n];
		}
	}
}
