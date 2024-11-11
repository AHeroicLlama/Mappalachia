namespace Library
{
	// Represents a Worldspace (WRLD) or Cell (CELL)
	public class Space(uint formID, string editorID, string displayName, bool isWorldspace, double centerX = 0, double centerY = 0, double maxRange = 4096)
		: Entity(formID, editorID, displayName, isWorldspace ? "WLRD" : "CELL")
	{
		public bool IsWorldspace { get; } = isWorldspace;

		public double CenterX { get; } = centerX;

		public double CenterY { get; } = centerY;

		public double MaxRange { get; } = maxRange;

		public bool IsAppalachia()
		{
			return EditorID.Equals("Appalachia", StringComparison.OrdinalIgnoreCase);
		}
	}
}
