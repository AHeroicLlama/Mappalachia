using Library;

namespace Mappalachia
{
	public class DerivedNPC(string displayName)
		: Entity(0, $"Derived{displayName.WithoutWhitespace()}Spawn", displayName, Signature.NPC_)
	{
	}
}
