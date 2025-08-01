namespace Library
{
	public class DerivedNPC(string editorID)
		: Entity(0, editorID, $"Derived {editorID} Spawn", Signature.NPC_)
	{
		public override string GetFriendlyFormID()
		{
			return "N/A";
		}
	}
}
