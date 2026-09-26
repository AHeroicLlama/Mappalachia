namespace Library
{
	public readonly struct Rotation(float x, float y, float z)
	{
		public float X { get; } = x;

		public float Y { get; } = y;

		public float Z { get; } = z;

		public override readonly string ToString()
		{
			return $"{{({X},{Y},{Z})}}";
		}
	}
}
