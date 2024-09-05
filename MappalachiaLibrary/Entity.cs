namespace MappalachiaLibrary
{
	// Represents any entity in the ESM
	public class Entity
	{
		public uint FormID { get; }
		public string EditorID { get; }
		public string DisplayName { get; }
		public string Signature { get; }

		public Entity(uint formID, string editorID, string displayName, string signature)
		{
			FormID = formID;
			EditorID = editorID;
			DisplayName = displayName;
			Signature = signature;
		}
	}
}
