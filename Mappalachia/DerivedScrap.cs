using Library;

namespace Mappalachia
{
	public class DerivedScrap(string displayName, float componentQuantity)
		: Entity(0, $"Derived{displayName.WithoutWhitespace()}", displayName, Signature.MISC)
	{
		public override float GetSpawnWeight()
		{
			return componentQuantity;
		}
	}
}
