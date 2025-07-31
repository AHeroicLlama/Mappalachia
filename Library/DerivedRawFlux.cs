using static Library.Common;

namespace Library
{
	public class DerivedRawFlux(FluxColor color)
		: Entity(0, color.ToString(), $"Derived Raw {color} Flux", Signature.ALCH)
	{
		public FluxColor Color { get; } = color;

		public override string GetFriendlyFormID()
		{
			return "N/A";
		}
	}
}
