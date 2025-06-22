using System.Drawing;

namespace Library
{
	public static class GeometryHelper
	{
		/* Return if 3 points (In that order) are;
		0 - Collinear
		1 - Clockwise
		-1 - Counter-Clockwise */
		static int GetOrientation(Coord a, Coord b, Coord c)
		{
			return Math.Sign(((b.Y - a.Y) * (c.X - b.X)) - ((b.X - a.X) * (c.Y - b.Y)));
		}

		// Returns if 2 lines given by coord pairs a and b, intersect
		public static bool LinesIntersect(Coord a1, Coord a2, Coord b1, Coord b2)
		{
			int orientA = GetOrientation(a1, a2, b1);
			int orientB = GetOrientation(a1, a2, b2);
			int orientC = GetOrientation(b1, b2, a1);
			int orientD = GetOrientation(b1, b2, a2);

			// Lines intersect
			if (orientA != orientB && orientC != orientD)
			{
				return true;
			}

			// Lines are collinear, return if they intersect
			return (orientA == 0 && IsBetween(a1, b1, a2)) ||
				(orientB == 0 && IsBetween(a1, b2, a2)) ||
				(orientC == 0 && IsBetween(b1, a1, b2)) ||
				(orientD == 0 && IsBetween(b1, a2, b2));
		}

		// Returns if the test coordinate lies between a and b
		// Assumes they are already collinear (See GetOrientation)
		static bool IsBetween(Coord a, Coord b, Coord test)
		{
			return test.X >= Math.Min(a.X, b.X) && test.X <= Math.Max(a.X, b.X) &&
				test.Y >= Math.Min(a.Y, b.Y) && test.Y <= Math.Max(a.Y, b.Y);
		}

		// Cross-product o->a and o->b
		public static double VectorCrossProduct(PointF o, PointF a, PointF b)
		{
			return ((a.X - o.X) * (b.Y - o.Y)) - ((a.Y - o.Y) * (b.X - o.X));
		}
	}
}
