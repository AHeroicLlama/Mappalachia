using System;

namespace Mappalachia.Class
{
	public class SpaceScaling
	{
		public double xOffset;
		public double yOffset;
		public double scale;

		static double zoomPadding = Map.plotXMin + SettingsPlotIcon.iconSize; // Number of pixels in from each side where the scaling/zooming will stop

		public SpaceScaling(double xOffset, double yOffset, double scale)
		{
			this.xOffset = xOffset;
			this.yOffset = yOffset;
			this.scale = scale;
		}

		// Gets the Space scaling for a given space
		// Identify the extremities and 'center of mass' for plots within a set
		// This is used to center and properly scale maps of cells - as the scaling is no longer in relation to the map image
		public static SpaceScaling GetSpaceScaling(Space space)
		{
			double xCenter = (Math.Abs(space.xMax) + Math.Abs(space.xMin)) / 2;
			double yCenter = (Math.Abs(space.yMax) + Math.Abs(space.yMin)) / 2;

			double xRange = Math.Abs(space.xMax) - Math.Abs(space.xMin);
			double yRange = Math.Abs(space.yMax) - Math.Abs(space.yMin);
			double largestWidth = Math.Max(xRange, yRange);

			// Prevent division by 0 if space contains a single item
			if (largestWidth == 0)
			{
				largestWidth = 1;
			}

			double bestZoomRatio = (Map.mapDimension - (zoomPadding * 2)) / largestWidth;

			return new SpaceScaling(-(xCenter - (Map.mapDimension / 2)), -(yCenter - (Map.mapDimension / 2)), bestZoomRatio);
		}
	}
}
