namespace Library
{
	public readonly struct Coord(double x, double y, double z = 0)
	{
		public double X { get; } = x;

		public double Y { get; } = y;

		public double Z { get; } = z;

		public override string ToString()
		{
			return $"{{X={X}, Y={Y}, Z={Z}}}";
		}
	}
}
