namespace Library
{
	public readonly struct Bounds(int x1, int y1, int z1, int x2, int y2, int z2)
	{
		public int X1 { get; } = x1;

		public int Y1 { get; } = y1;

		public int Z1 { get; } = z1;

		public int X2 { get; } = x2;

		public int Y2 { get; } = y2;

		public int Z2 { get; } = z2;

		public override readonly string ToString()
		{
			return $"{{({X1},{Y1},{Z1}), ({X2},{Y2},{Z2})}}";
		}
	}
}
