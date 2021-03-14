using System;
using System.Collections.Generic;

namespace Mappalachia.Class
{
	class CellScaling
	{
		public double xOffset;
		public double yOffset;
		public double scale;

		static double zoomPadding = Map.legendXMin; //Number of pixels in from each side where the scaling/zooming will stop

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
			List<MapDataPoint> points = cell.GetPlots();

			double xMax = 0;
			double xMin = 0;
			double yMax = 0;
			double yMin = 0;

			//Identify the maximum bounds of all coordinates here
			bool first = true;
			foreach (MapDataPoint point in points)
			{
				//This is the first plot - set all its values to the min and max
				if (first)
				{
					xMax = point.x;
					xMin = point.x;
					yMax = point.y;
					yMin = point.y;

					first = false;
				}
				else
				{
					if (point.x > xMax)
					{
						xMax = point.x;
					}
					if (point.x < xMin)
					{
						xMin = point.x;
					}
					if (point.y > yMax)
					{
						yMax = point.y;
					}
					if (point.y < yMin)
					{
						yMin = point.y;
					}
				}
			}

			double xCenter = (Math.Abs(xMax) + Math.Abs(xMin)) / 2;
			double yCenter = (Math.Abs(yMax) + Math.Abs(yMin)) / 2;

			double xRange = Math.Abs(xMax) - Math.Abs(xMin);
			double yRange = Math.Abs(yMax) - Math.Abs(yMin);
			double largestWidth = Math.Max(xRange, yRange);

			double bestZoomRatio = (Map.mapDimension - zoomPadding * 2) / largestWidth;

			return new CellScaling(-(xCenter - Map.mapDimension / 2), -(yCenter - Map.mapDimension / 2), bestZoomRatio);
		}
	}
}
