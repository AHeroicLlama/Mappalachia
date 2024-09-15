namespace MappalachiaLibrary
{
	// Represents a Worldspace (WRLD) or Cell (CELL)
	public class Space : Entity
	{
		public bool IsWorldspace { get; }

		// The X/Y offsets between cell 0,0 and plotted image center.
		public float XOffset { get; }
		public float YOffset { get; }

		// The effective zoom so the cell suitably fills the image.
		public float Scale { get; }

		public Space(uint formID, string editorID, string displayName, bool isWorldspace, float xOffset = 0, float yOffset = 0, float scale = 1)
			: base(formID, editorID, displayName, isWorldspace ? "WLRD" : "CELL")
		{
			IsWorldspace = isWorldspace;
			XOffset = xOffset;
			YOffset = yOffset;
			Scale = scale;
		}

		public Space(string editorID, bool isWorldspace, uint formID = 0x00000000, string displayName = "")
			: base(formID, editorID, displayName, isWorldspace ? "WLRD" : "CELL")
		{
			IsWorldspace = isWorldspace;
		}

		bool IsAppalachia()
		{
			return EditorID.Equals("Appalachia", StringComparison.OrdinalIgnoreCase);
		}
	}
}
