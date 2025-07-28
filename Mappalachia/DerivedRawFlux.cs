using Library;
using static Library.Common;

namespace Mappalachia
{
	public class DerivedRawFlux(FluxColor color)
		: Entity(0, $"DerivedRaw{color}Flux", $"Raw {color} Flux", Signature.ALCH)
	{
		public FluxColor Color { get; } = color;

		public override string GetFriendlyFormID()
		{
			return "N/A";
		}
	}
}
