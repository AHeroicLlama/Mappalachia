namespace Library
{
	// Represents a Worldspace (WRLD) or Cell (CELL)
	public class Space : Entity
	{
		public bool IsWorldspace { get; }

		public double CenterX { get; }

		public double CenterY { get; }

		public double MaxRange { get; }

		public Space(uint formID, string editorID, string displayName, bool isWorldspace, double centerX = 0, double centerY = 0, double maxRange = 4096)
			: base(formID, editorID, displayName, isWorldspace ? "WLRD" : "CELL")
		{
			IsWorldspace = isWorldspace;
			CenterX = centerX;
			CenterY = centerY;
			MaxRange = maxRange;
		}

		public bool IsAppalachia()
		{
			return EditorID.Equals("Appalachia", StringComparison.OrdinalIgnoreCase);
		}
	}
}
