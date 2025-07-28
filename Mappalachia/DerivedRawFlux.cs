using Library;
using static Library.Common;

namespace Mappalachia
{
	public class DerivedRawFlux(FluxColor color)
		: Entity(0, $"DerivedRaw{color}Flux", color.ToString(), Signature.ALCH)
	{
		public override string GetFriendlyFormID()
		{
			return "N/A";
		}
	}
}
