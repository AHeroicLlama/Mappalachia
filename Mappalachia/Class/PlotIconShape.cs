using System;

namespace Mappalachia.Class
{
	// The shape settings that can be applied to a PlotIcon
	public class PlotIconShape
	{
		public readonly bool diamond;
		public readonly bool square;
		public readonly bool circle;
		public readonly bool crosshairInner;
		public readonly bool crosshairOuter;
		public readonly bool fill;

		static readonly Random rnd = new Random();

		public PlotIconShape(bool diamond, bool square, bool circle, bool crosshairInner, bool crosshairOuter, bool fill)
		{
			this.diamond = diamond;
			this.square = square;
			this.circle = circle;
			this.crosshairInner = crosshairInner;
			this.crosshairOuter = crosshairOuter;
			this.fill = fill;
		}

		// Instantiate with random shapes
		public PlotIconShape()
		{
			diamond = rnd.Next(2) == 1;
			square = rnd.Next(2) == 1;
			circle = rnd.Next(2) == 1;
			crosshairInner = rnd.Next(2) == 1;
			crosshairOuter = rnd.Next(2) == 1;
			fill = false;

			// Verify we have selected at least one option
			if (!(diamond || square || circle || crosshairInner || crosshairOuter))
			{
				int choice = rnd.Next(5);
				switch (choice)
				{
					case 0:
						diamond = true;
						break;
					case 1:
						square = true;
						break;
					case 2:
						circle = true;
						break;
					case 3:
						crosshairInner = true;
						break;
					case 4:
						crosshairOuter = true;
						break;
				}
			}
		}
	}
}
