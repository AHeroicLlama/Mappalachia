using System;
using System.Drawing;

namespace Mappalachia.Class
{
	//All user settings related to cell mode
	class SettingsCell
	{
		public static readonly int heightPrecision = 50; //The definition (percentage increments) used in setting the height bounds and visualizing too
		public static readonly string targetDefaultCell = "ValleyGalleria"; //Find the cell with editorId containing this and use it as the default on startup

		//Settings for pseudo-plot-icons used to draw cell outline
		public static readonly int outlineWidth = 3;
		public static readonly int outlineSize = 25;
		public static readonly Color outlineColor = Color.Gray;
		public static readonly int outlineAlpha = 200;

		public static int GetHeightBinSize()
		{
			return (int)Math.Round(100d / heightPrecision);
		}

		static Cell currentCell;

		public static bool drawOutline = true;

		public static int minHeightPerc = 0;
		public static int maxHeightPerc = 100;

		public static Cell GetCell()
		{
			return currentCell;
		}

		public static void SetCell(Cell cell)
		{
			currentCell = cell;
			Map.DrawBaseLayer();
		}

		public static double GetMinHeightCoordBound()
		{
			return ((minHeightPerc / 100d) * currentCell.heightRange) + currentCell.zMin;
		}

		public static double GetMaxHeightCoordBound()
		{
			return ((maxHeightPerc / 100d) * currentCell.heightRange) + currentCell.zMin;

		}
	}
}
