using System;
using System.Collections.Generic;
using System.Linq;

namespace Mappalachia.Class
{
	// Represents a CELL or WRLD.
	public class Space
	{
		public string formID;
		public string editorID;
		public string displayName;
		public float xOffset;
		public float yOffset;
		public float xRange;
		public float yRange;
		public int zMin;
		public int zRange;
		public float scale;
		readonly bool isWorldspace;

		List<int> zPlots;

		public Space(string formID, string editorID, string displayName, bool isWorldspace, int xCenter, int yCenter, int xMin, int xMax, int yMin, int yMax)
		{
			this.formID = formID;
			this.editorID = editorID;
			this.displayName = displayName;
			this.isWorldspace = isWorldspace;
			this.xRange = Math.Abs(Map.ScaleCoordinate(xMax, false) - Map.ScaleCoordinate(xMin, false));
			this.yRange = Math.Abs(Map.ScaleCoordinate(yMax, true) - Map.ScaleCoordinate(yMin, true));

			xOffset = -Map.ScaleCoordinate(xCenter, false) + (Map.mapDimension / 2);
			yOffset = -Map.ScaleCoordinate(yCenter, true) + (Map.mapDimension / 2);

			scale = IsWorldspace() ? 1 : Map.mapDimension / (float)Math.Max(xRange, yRange);

			if (!IsWorldspace())
			{
				zPlots = Database.GetAllSpaceZCoords(formID);
				zMin = zPlots.Min();
				zRange = Math.Abs(zPlots.Max() - zMin);
			}
		}

		// Returns the distribution of Y coordinates for items in the space, broken into x bins
		public double[] GetHeightDistribution()
		{
			if (IsWorldspace())
			{
				return new double[] { 0, 1 };
			}

			int precision = SettingsSpace.heightPrecision;

			// Count how many items fall into the arbitrary <precision># different bins
			int[] distributionCount = new int[precision];
			foreach (int point in zPlots)
			{
				// Calculate which numeric bin this item would fall into
				int placementBin = (int)(((point - zMin) / zRange) * precision);

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
				distribution[i] = ((double)distributionCount[i] / zPlots.Count) * precision;
			}

			return distribution;
		}

		// Returns if this Space is a Worldspace
		public bool IsWorldspace()
		{
			return isWorldspace;
		}
	}
}
