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

		public Space(bool isWorldspace, uint formID, string editorID, string displayName, float xOffset, float yOffset, float scale) : base(formID, editorID, displayName, isWorldspace ? "WLRD" : "CELL")
		{
			IsWorldspace = isWorldspace;
			XOffset = xOffset;
			YOffset = yOffset;
			Scale = scale;
		}

		public Space(bool isWorldspace, string editorID, uint formID = 0x00000000, string displayName = "") : base(formID, editorID, displayName, isWorldspace ? "WLRD" : "CELL")
		{
			IsWorldspace = isWorldspace;
		}

		bool IsAppalachia()
		{
			return EditorID == "Appalachia";
		}
	}
}
