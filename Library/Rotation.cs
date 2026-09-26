namespace Library
{
	public struct Rotation
	{
		public Rotation(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public float X { get; }

		public float Y { get; }

		public float Z { get; }

		public override readonly string ToString()
		{
			return $"{{({X},{Y},{Z})}}";
		}
	}
}
