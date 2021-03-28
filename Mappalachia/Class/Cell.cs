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
		public double[] GetHeightDistribution(int precision)
		{
			int minZ = 0;
			int maxZ = 0;

			bool first = true;

			List<MapDataPoint> points = GetPlots();

			//Find the lowest and highest z value
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

			//Count how many items fall into the arbitrary 100 different segments
			int[] distributionCount = new int[precision];
			foreach (MapDataPoint point in points)
			{
				int placementPercent = (int)(((point.z  - minZ) / range) * (double)precision);
				
				//One value should be exactly the precision value, but there isn't an index there
				if (placementPercent == precision)
				{
					distributionCount[precision - 1]++;
					continue;
				}

				distributionCount[placementPercent]++;
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
