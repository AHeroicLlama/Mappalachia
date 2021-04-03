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

		public Cell(string formID, string editorID, string displayName)
		{
			this.formID = formID;
			this.editorID = editorID;
			this.displayName = displayName;
		}

		//Gets every data point present in this given cell
		public List<MapDataPoint> GetPlots()
		{
			return DataHelper.GetAllCellCoords(formID);
		}

		public CellScaling GetScaling()
		{
			return CellScaling.GetCellScaling(this);
		}

		//Returns the distribution of Y coordinates for items in the cell, broken into x bins
		public double[] GetHeightDistribution()
		{
			List<MapDataPoint> points = GetPlots();

			int minZ = -1;
			int maxZ = -1;

			//Find the lowest and highest z value in this cell (if not already found)
			bool first = true;
			foreach (MapDataPoint point in points)
			{
				if (first)
				{
					minZ = point.z;
					maxZ = point.z;
					first = false;
					continue;
				}

				if (point.z > maxZ)
				{
					maxZ = point.z;
				}
				if (point.z < minZ)
				{
					minZ = point.z;
				}
			}

			double range = Math.Abs(maxZ - minZ);

			int precision = SettingsMap.cellModeHeightPrecision;

			//Count how many items fall into the arbitrary <precision># different bins
			int[] distributionCount = new int[precision];
			foreach (MapDataPoint point in points)
			{
				//Calculate which numeric bin this item would fall into
				int placementBin = (int)(((point.z  - minZ) / range) * precision);
				
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
				distribution[i] = ((double)distributionCount[i] / points.Count) * precision;
			}

			return distribution;
		}
	}
}
