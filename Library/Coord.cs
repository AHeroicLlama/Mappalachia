namespace Library
{
	public readonly struct Coord(float x, float y, float z = 0)
	{
		public float X { get; } = x;

		public float Y { get; } = y;

		public float Z { get; } = z;
	}
}
