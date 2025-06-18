using Library;

namespace Mappalachia
{
	public class DerivedNPC(string displayName)
		: Entity(0, $"Derived{displayName.WithoutWhitespace()}Spawn", displayName, Signature.NPC_)
	{
		public override string GetFriendlyFormID()
		{
			return "N/A";
		}
	}
}
