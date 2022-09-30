﻿namespace BackgroundRenderer
{
	internal class Space
	{
		public string formID;
		public string editorID;
		public int xCenter;
		public int yCenter;
		public int xRange;
		public int yRange;

		/* custom 'nudge' values - auto-centering some cells has undesirable consequences
		because unreachable assets are placed far away from the playable area.
		For cells affected significantly by this, we manually step in and adjust the auto-center and auto-scaling for the cell
		This means the playable area is centered and fills the map.
		Note that we do not override the auto-scale/center but we further adjust it
		These values are hardcoded in by the preprocessor*/
		public int nudgeX;
		public int nudgeY;
		public float nudgeScale;

		public Space(string formID, string editorID, int xCenter, int yCenter, int xRange, int yRange, int nudgeX, int nudgeY, float nudgeScale)
		{
			this.formID = formID;
			this.editorID = editorID;
			this.xCenter = xCenter;
			this.yCenter = yCenter;
			this.xRange = xRange;
			this.yRange = yRange;

			this.nudgeX = nudgeX;
			this.nudgeY = nudgeY;
			this.nudgeScale = nudgeScale;
		}
	}
}
