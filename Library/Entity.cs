namespace Library
{
	// Represents any entity in the ESM
	public class Entity(uint formID, string editorID, string signature, string displayName)
	{
		public uint FormID { get; } = formID;

		public string EditorID { get; } = editorID;

		public string Signature { get; } = signature;

		public string DisplayName { get; } = displayName;
	}
}
