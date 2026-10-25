namespace Library
{
	public enum ShapeType
	{
		Box,
		Sphere,
		Line,
		Plane,
		Ellipsoid,
		Cylinder,
	}

	// Represents a Primitive Shape
	public readonly struct Shape(ShapeType shapeType, double boundX, double boundY, double boundZ)
	{
		public ShapeType ShapeType { get; } = shapeType;

		public double BoundX { get; } = boundX;

		public double BoundY { get; } = boundY;

		public double BoundZ { get; } = boundZ;
	}
}
