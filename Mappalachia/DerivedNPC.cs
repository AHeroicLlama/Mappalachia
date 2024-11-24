using Library;

namespace Mappalachia
{
	public class DerivedNPC(string displayName)
		: Entity(0, $"Derived{displayName.WithoutWhitespace()}", displayName, Signature.NPC_)
	{
	}
}
