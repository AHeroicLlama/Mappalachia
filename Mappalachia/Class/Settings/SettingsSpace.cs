using System;

namespace Mappalachia
{
	// Settings relating to Space selection
	class SettingsSpace
	{
		public const int heightPrecision = 25; // The definition (percentage increments) used in incrementing the height bounds and visualizing too

		static Space currentSpace;

		// User-definable settings
		public static int minHeightPerc = 0;
		public static int maxHeightPerc = 100;

		public static bool CurrentSpaceIsWorld()
		{
			// If this is called mid-form load, assumes worldspace
			return GetSpace() == null || GetSpace().IsWorldspace();
		}

		public static bool CurrentSpaceIsAppalachia()
		{
			return GetSpace() == null || GetSpace().IsAppalachia();
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
