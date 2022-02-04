using System;
using System.Collections.Generic;
using System.Linq;

namespace Mappalachia.Class
{
	// Represents an CELL or WRLD.
	public class Space
	{
		public string formID;
		public string editorID;
		public string displayName;
		public double xMin;
		public double xMax;
		public double yMin;
		public double yMax;
		public int zMin;
		public int zMax;
		public double heightRange;

		List<MapDataPoint> plots;

		public Space(string formID, string editorID, string displayName)
		{
			this.formID = formID;
			this.editorID = editorID;
			this.displayName = displayName;
		}

		// Gets the plotting data and coordinate extremities
		void InitializePlotData()
		{
			plots = DataHelper.GetAllSpaceCoords(formID);

			plots = plots.OrderBy(plot => plot.x).ToList();
			xMin = plots.First().x;
			xMax = plots.Last().x;

			plots = plots.OrderBy(plot => plot.y).ToList();
			yMin = plots.First().y;
			yMax = plots.Last().y;

			plots = plots.OrderBy(plot => plot.z).ToList();
			zMin = plots.First().z;
			zMax = plots.Last().z;

			heightRange = Math.Abs(zMax - zMin);
		}

		public SpaceScaling GetScaling()
		{
			if (plots == null)
			{
				InitializePlotData();
			}

			return SpaceScaling.GetSpaceScaling(this);
		}

		// Returns the distribution of Y coordinates for items in the space, broken into x bins
		public double[] GetHeightDistribution()
		{
			if (plots == null)
			{
				InitializePlotData();
			}

			int precision = SettingsSpace.heightPrecision;

			// Count how many items fall into the arbitrary <precision># different bins
			int[] distributionCount = new int[precision];
			foreach (MapDataPoint point in plots)
			{
				// Calculate which numeric bin this item would fall into
				int placementBin = (int)(((point.z - zMin) / heightRange) * precision);

				// At least one value will be exactly the precision value, (it's the highest thing)
				// But trying to put this in a bin results in accessing element n of array of size n, which is out of bounds
				// So this is a special case for the top-most item to be moved back into the upper (n-1) bin
				if (placementBin == precision)
				{
					distributionCount[precision - 1]++;
					continue;
				}

				distributionCount[placementBin]++;
			}

			// Normalize the count of items to find which percentage of items fall into the segments
			double[] distribution = new double[precision];
			for (int i = 0; i < distributionCount.Length; i++)
			{
				distribution[i] = ((double)distributionCount[i] / plots.Count) * precision;
			}

			return distribution;
		}

		// Returns if the currently selected Space is a Worldspace
		public bool IsWorldspace()
		{
			return editorID == "Appalachia";
		}
	}
}
