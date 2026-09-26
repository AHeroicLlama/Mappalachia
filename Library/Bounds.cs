namespace Library
{
	public struct Bounds
	{
		public Bounds(int x1, int y1, int z1, int x2, int y2, int z2)
		{
			X1 = x1;
			Y1 = y1;
			Z1 = z1;
			X2 = x2;
			Y2 = y2;
			Z2 = z2;
		}

		public int X1 { get; }

		public int Y1 { get; }

		public int Z1 { get; }

		public int X2 { get; }

		public int Y2 { get; }

		public int Z2 { get; }

		public int XRange => X2 - X1;

		public int YRange => Y2 - Y1;

		public int ZRange => Z2 - Z1;

		public Coord Corner => new Coord(X1, Y1, Z1);

		public override readonly string ToString()
		{
			return $"{{({X1},{Y1},{Z1}), ({X2},{Y2},{Z2})}}";
		}
	}
}
