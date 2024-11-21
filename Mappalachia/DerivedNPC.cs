using Library;

namespace Mappalachia
{
	public class DerivedNPC(string displayName, float spawnWeight)
		: Entity(0, $"Derived{displayName.WithoutWhitespace()}", displayName, Signature.NPC_)
	{
		public override float GetSpawnWeight()
		{
			return spawnWeight;
		}
	}
}
