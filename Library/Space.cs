namespace Library
{
	// Represents a Worldspace (WRLD) or Cell (CELL)
	public class Space : Entity
	{
		public bool IsWorldspace { get; }

		public int CenterX { get; }

		public int CenterY { get; }

		public int MaxRange { get; }

		public Space(uint formID, string editorID, string displayName, bool isWorldspace, int centerX = 0, int centerY = 0, int maxRange = 4096)
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
