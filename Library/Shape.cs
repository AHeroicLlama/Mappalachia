namespace Library
{
	public enum ShapeType
	{
		Box,
		Sphere,
		Line,
		Plane,
		Ellipsoid,
	}

	// Represents a Primitive Shape
	public readonly struct Shape(ShapeType shapeType, float boundX, float boundY, float boundZ, float rotZ)
	{
		public ShapeType ShapeType { get; } = shapeType;

		public float BoundX { get; } = boundX;

		public float BoundY { get; } = boundY;

		public float BoundZ { get; } = boundZ;

		// The rotation of the shape in degrees
		public float RotZ { get; } = rotZ;
	}
}
