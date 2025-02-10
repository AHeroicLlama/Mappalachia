namespace Library
{
	// Represents a Worldspace (WRLD) or Cell (CELL)
	public class Space(uint formID, string editorID, string displayName, bool isWorldspace, double centerX, double centerY, double maxRange)
		: Entity(formID, editorID, displayName, isWorldspace ? Signature.WRLD : Signature.CELL)
	{
		public bool IsWorldspace { get; } = isWorldspace;

		public double CenterX { get; } = centerX;

		public double CenterY { get; } = centerY;

		public double MaxRange { get; } = maxRange;

		public bool IsAppalachia()
		{
			return EditorID.Equals("Appalachia", StringComparison.OrdinalIgnoreCase);
		}

		public double GetRadius()
		{
			return MaxRange / 2d;
		}

		public double GetMinX()
		{
			return CenterX - GetRadius();
		}

		public double GetMaxX()
		{
			return CenterX + GetRadius();
		}

		public double GetMinY()
		{
			return CenterY - GetRadius();
		}

		public double GetMaxY()
		{
			return CenterY + GetRadius();
		}
	}
}
