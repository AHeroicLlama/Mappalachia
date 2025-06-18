using Library;

namespace Mappalachia
{
	public class DerivedScrap(string displayName)
		: Entity(0, $"Derived{displayName.WithoutWhitespace()}Scrap", displayName, Signature.MISC)
	{
		public override string GetFriendlyFormID()
		{
			return "N/A";
		}
	}
}
