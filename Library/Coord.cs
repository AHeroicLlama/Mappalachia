namespace Library
{
	public struct Coord
	{
		public Coord(double x, double y, double z = 0)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Coord()
		{
		}

		public double X { get; set; }

		public double Y { get; set; }

		public double Z { get; set; }

		public override readonly string ToString()
		{
			return $"{{X={X}, Y={Y}, Z={Z}}}";
		}
	}
}
