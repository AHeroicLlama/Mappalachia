using System;
using System.Collections.Generic;

namespace Mappalachia.Class
{
	// Represents a CELL or WRLD.
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

		List<int> zPlots;

		public Space(string formID, string editorID, string displayName)
		{
			this.formID = formID;
			this.editorID = editorID;
			this.displayName = displayName;
		}

		// Gets the plotting data and coordinate extremities
		void InitializePlotData()
		{
			if (IsWorldspace())
			{
				return;
			}

			zPlots = Database.GetAllSpaceZCoords(formID);
			(double, double, double, double, int, int) extremities = Database.GetSpaceExtremities(formID);

			xMin = extremities.Item1;
			xMax = extremities.Item2;

			yMin = extremities.Item3;
			yMax = extremities.Item4;

			zMin = extremities.Item5;
			zMax = extremities.Item6;

			heightRange = Math.Abs(zMax - zMin);
		}

		public SpaceScaling GetScaling()
		{
			if (IsWorldspace())
			{
				return new SpaceScaling(0, 0, 1);
			}

			if (zPlots == null)
			{
				InitializePlotData();
			}

			return SpaceScaling.GetSpaceScaling(this);
		}

		// Returns the distribution of Y coordinates for items in the space, broken into x bins
		public double[] GetHeightDistribution()
		{
			if (IsWorldspace())
			{
				return new double[] { 0, 1 };
			}

			if (zPlots == null)
			{
				InitializePlotData();
			}

			int precision = SettingsSpace.heightPrecision;

			// Count how many items fall into the arbitrary <precision># different bins
			int[] distributionCount = new int[precision];
			foreach (int point in zPlots)
			{
				// Calculate which numeric bin this item would fall into
				int placementBin = (int)(((point - zMin) / heightRange) * precision);

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
			return editorID == "Appalachia";
		}
	}
}
