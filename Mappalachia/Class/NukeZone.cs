using System.Drawing;

namespace Mappalachia.Class
{
	static class NukeZone
	{
		public enum FluxColor
		{
			Crimson,
			Cobalt,
			Fluorescent,
			Violet,
			Yellowcake,
		}

		public static readonly Color color = Color.FromArgb(128, 204, 76, 51);

		public const int radius = 20460;
	}
}
