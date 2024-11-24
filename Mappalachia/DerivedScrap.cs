using Library;

namespace Mappalachia
{
	public class DerivedScrap(string displayName)
		: Entity(0, $"Derived{displayName.WithoutWhitespace()}", displayName, Signature.MISC)
	{
	}
}
