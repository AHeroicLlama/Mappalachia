namespace Library
{
	public class DerivedNPC(string editorID)
		: Entity(0, editorID, $"Derived {editorID.WithoutWhitespace()} Spawn", Signature.NPC_)
	{
		public override string GetFriendlyFormID()
		{
			return "N/A";
		}
	}
}
