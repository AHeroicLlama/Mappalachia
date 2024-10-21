namespace Library
{
	// Represents a Worldspace (WRLD) or Cell (CELL)
	public class Space : Entity
	{
		public bool IsWorldspace { get; }

		public Space(uint formID, string editorID, string displayName, bool isWorldspace)
			: base(formID, editorID, displayName, isWorldspace ? "WLRD" : "CELL")
		{
			IsWorldspace = isWorldspace;
		}

		public bool IsAppalachia()
		{
			return EditorID.Equals("Appalachia", StringComparison.OrdinalIgnoreCase);
		}
	}
}
