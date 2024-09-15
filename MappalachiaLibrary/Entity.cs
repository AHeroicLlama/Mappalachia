namespace MappalachiaLibrary
{
	// Represents any entity in the ESM
	public class Entity
	{
		public uint FormID { get; }
		public string EditorID { get; }
		public string Signature { get; }
		public string DisplayName { get; }

		public Entity(uint formID, string editorID, string signature, string displayName)
		{
			FormID = formID;
			EditorID = editorID;
			Signature = signature;
			DisplayName = displayName;
		}
	}
}
