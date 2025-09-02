namespace Library
{
	public class DerivedScrap(string editorID)
		: Entity(0, editorID, $"Derived {editorID} Scrap", Signature.MISC)
	{
		public override string GetFriendlyFormID()
		{
			return "N/A";
		}
	}
}
