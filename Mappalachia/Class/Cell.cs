using System;
using System.Collections.Generic;

namespace Mappalachia.Class
{
	//Represents an internal cell. Used to select and differentiate cells in Cell mode
	public class Cell
	{
		public string formID;
		public string editorID;
		public string displayName;
		List<MapDataPoint> plots;
		public double xMin;
		public double xMax;
		public double yMin;
		public double yMax;
		public int zMin;
		public int zMax;

		public Cell(string formID, string editorID, string displayName)
		{
			this.formID = formID;
			this.editorID = editorID;
			this.displayName = displayName;
		}

		//Gets the plotting data and coordinate extremities
		void InitializePlotData()
		{
			plots = DataHelper.GetAllCellCoords(formID);

			//Identify the maximum bounds of all coordinates here
			bool first = true;
			foreach (MapDataPoint point in plots)
			{
				//This is the first plot - set all its values to the min and max
				if (first)
				{
					xMin = point.x;
					xMax = point.x;
					yMin = point.y;
					yMax = point.y;
					zMin = point.z;
					zMax = point.z;

					first = false;
				}
				else
				{
					if (point.x < xMin)
					{
						xMin = point.x;
					}
					if (point.x > xMax)
					{
						xMax = point.x;
					}
					if (point.y < yMin)
					{
						yMin = point.y;
					}
					if (point.y > yMax)
					{
						yMax = point.y;
					}
					if (point.z < zMin)
					{
						zMin = point.z;
					}
					if (point.z > zMax)
					{
						zMax = point.z;
					}
				}
			}
		}

		public CellScaling GetScaling()
		{
			if (plots == null)
			{
				InitializePlotData();
			}

			return CellScaling.GetCellScaling(this);
		}

		//Returns the distribution of Y coordinates for items in the cell, broken into x bins
		public double[] GetHeightDistribution()
		{
			if (plots == null)
			{
				InitializePlotData();
			}

			double range = Math.Abs(zMax - zMin);

			int precision = SettingsMap.cellModeHeightPrecision;

			//Count how many items fall into the arbitrary <precision># different bins
			int[] distributionCount = new int[precision];
			foreach (MapDataPoint point in plots)
			{
				//Calculate which numeric bin this item would fall into
				int placementBin = (int)(((point.z  - zMin) / range) * precision);
				
				//At least one value will be exactly the precision value, (it's the highest thing)
				//But trying to put this in a bin results in accessing element n of array of size n, which is out of bounds
				//So this is a special case for the top-most item to be moved back into the upper bin
				if (placementBin == precision)
				{
					distributionCount[precision - 1]++;
					continue;
				}

				distributionCount[placementBin]++;
			}

			//Normalise the count of items to find which percentage of items fall into the segments
			double[] distribution = new double[precision];
			for (int i = 0; i < distributionCount.Length; i++)
			{
				distribution[i] = ((double)distributionCount[i] / plots.Count) * precision;
			}

			return distribution;
		}
	}
}
