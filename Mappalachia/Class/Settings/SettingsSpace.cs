using System;
using System.Drawing;

namespace Mappalachia.Class
{
	// Settings relating to Space selection
	class SettingsSpace
	{
		public static readonly int heightPrecision = 25; // The definition (percentage increments) used in incrementing the height bounds and visualizing too

		// Settings for pseudo-plot-icons used to draw space outline
		public static readonly int outlineWidth = 3;
		public static readonly int outlineSize = 25;
		public static readonly Color outlineColor = Color.White;
		public static readonly int outlineAlpha = 200;

		static Space currentSpace;

		// User-definable settings
		public static int minHeightPerc = 0;
		public static int maxHeightPerc = 100;

		public static bool CurrentSpaceIsWorld()
		{
			// If this is called mid-form load, assumes worlspace
			return GetSpace() == null || GetSpace().IsWorldspace();
		}

		public static string GetCurrentFormID()
		{
			return GetSpace().formID;
		}

		public static int GetHeightBinSize()
		{
			return (int)Math.Round(100d / heightPrecision);
		}

		public static Space GetSpace()
		{
			return currentSpace;
		}

		public static void SetSpace(Space space)
		{
			currentSpace = space;
			FormMaster.DrawMap(true);
		}

		public static double GetMinHeightCoordBound()
		{
			return ((minHeightPerc / 100d) * currentSpace.zRange) + currentSpace.zMin;
		}

		public static double GetMaxHeightCoordBound()
		{
			return ((maxHeightPerc / 100d) * currentSpace.zRange) + currentSpace.zMin;
		}
	}
}
