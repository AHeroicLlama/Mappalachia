using System;

namespace Mappalachia.Class
{
	public class CellScaling
	{
		public double xOffset;
		public double yOffset;
		public double scale;

		static double zoomPadding = Map.plotXMin + SettingsPlotIcon.iconSize; //Number of pixels in from each side where the scaling/zooming will stop

		public CellScaling(double xOffset, double yOffset, double scale)
		{
			this.xOffset = xOffset;
			this.yOffset = yOffset;
			this.scale = scale;
		}

		//Gets the Cell scaling for a given Cell
		//Identify the extremities and 'center of mass' for plots within a set
		//This is used to center and properly scale maps in Cell mode - as the scaling is no longer in relation to the map image
		public static CellScaling GetCellScaling(Cell cell)
		{
			double xCenter = (Math.Abs(cell.xMax) + Math.Abs(cell.xMin)) / 2;
			double yCenter = (Math.Abs(cell.yMax) + Math.Abs(cell.yMin)) / 2;

			double xRange = Math.Abs(cell.xMax) - Math.Abs(cell.xMin);
			double yRange = Math.Abs(cell.yMax) - Math.Abs(cell.yMin);
			double largestWidth = Math.Max(xRange, yRange);

			double bestZoomRatio = (Map.mapDimension - (zoomPadding * 2)) / largestWidth;

			return new CellScaling(-(xCenter - (Map.mapDimension / 2)), -(yCenter - (Map.mapDimension / 2)), bestZoomRatio);
		}
	}
}
