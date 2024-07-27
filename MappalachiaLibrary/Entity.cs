namespace MappalachiaLibrary
{
	// Represents any entity in the ESM
	public class Entity
	{
		public uint FormID { get; init; }
		public string EditorID { get; init; }
		public string DisplayName { get; init; }
		public string Signature { get; init; }

        public Entity(uint formID, string editorID, string displayName, string signature)
        {
			FormID = formID;
			EditorID = editorID;
			DisplayName = displayName;
			Signature = signature;
        }
    }
}
