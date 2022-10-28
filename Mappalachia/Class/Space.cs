using System;
using System.Collections.Generic;
using System.Linq;

namespace Mappalachia
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

		/* custom 'nudge' values - auto-centering some cells has undesirable consequences
		because unreachable assets are placed far away from the playable area.
		For cells affected significantly by this, we manually step in and adjust the auto-center and auto-scaling for the cell
		This means the playable area is centered and fills the map.
		Note that we do not override the auto-scale/center but we further adjust it
		These values are hardcoded in by the preprocessor*/
		public int nudgeX;
		public int nudgeY;
		public float nudgeScale;

		readonly List<int> zPlots;

		public Space(string formID, string editorID, string displayName, bool isWorldspace, int xCenter, int yCenter, int xMin, int xMax, int yMin, int yMax, int nudgeX, int nudgeY, float nudgeScale)
		{
			this.formID = formID;
			this.editorID = editorID;
			this.displayName = displayName;
			this.isWorldspace = isWorldspace;
			this.xRange = Math.Abs(Map.CorrectAxis(xMax, false) - Map.CorrectAxis(xMin, false));
			this.yRange = Math.Abs(Map.CorrectAxis(yMax, true) - Map.CorrectAxis(yMin, true));
			this.nudgeX = nudgeX;
			this.nudgeY = nudgeY;
			this.nudgeScale = nudgeScale;

			// Manual calibration. Unlike cells which will dynamically resize based on their contents
			// the 'overworld' has a set map size, so plots must be scaled to fit
			if (IsAppalachia())
			{
				xOffset = 2000f;
				yOffset = 2000f;

				scale = 0.007001f;

				this.nudgeX = 0;
				this.nudgeY = 0;
				this.nudgeScale = 1f;
			}
			else
			{
				xOffset = -Map.CorrectAxis(xCenter, false) + (Map.mapDimension / 2);
				yOffset = -Map.CorrectAxis(yCenter, true) + (Map.mapDimension / 2);

				scale = Map.mapDimension / (float)Math.Max(xRange, yRange);
			}

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
				int placementBin = (int)(((point - zMin) / (double)zRange) * precision);

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

		public bool IsAppalachia()
		{
			return editorID == "Appalachia";
		}
	}
}
