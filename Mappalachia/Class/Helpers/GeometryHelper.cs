using System;
using System.Drawing;

namespace Mappalachia
{
	static class GeometryHelper
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

		// Returns the maximum possible width of the bounding square of a quad with dimensions x/y, but an unknown rotation
		public static double GetMaximumBoundingBoxWidth(double width, double height)
		{
			return Pythagoras(width, height);
		}

		// Returns the rectangle offset by the given amount
		public static RectangleF OffsetRect(RectangleF rect, float offset)
		{
			return new RectangleF(rect.X + offset, rect.Y + offset, rect.Width, rect.Height);
		}
	}
}
