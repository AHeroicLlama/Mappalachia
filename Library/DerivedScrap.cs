namespace Library
{
	public class DerivedScrap(string editorID)
		: Entity(0, editorID, $"Derived {editorID.WithoutWhitespace()} Scrap", Signature.MISC)
	{
		public override string GetFriendlyFormID()
		{
			return "N/A";
		}
	}
}
