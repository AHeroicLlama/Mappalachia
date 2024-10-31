namespace Library
{
	// Represents any entity in the ESM
	public class Entity(uint formID, string editorID, string displayName, string signature)
	{
		public uint FormID { get; } = formID;

		public string EditorID { get; } = editorID;

		public string DisplayName { get; } = displayName;

		public string Signature { get; } = signature;
	}
}
