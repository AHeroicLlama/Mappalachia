namespace BackgroundRenderer
{
	internal class Space
	{
		public string formID;
		public string editorID;
		public int xCenter;
		public int yCenter;
		public int xRange;
		public int yRange;

		public Space(string formID, string editorID, int xCenter, int yCenter, int xRange, int yRange)
		{
			this.formID = formID;
			this.editorID = editorID;
			this.xCenter = xCenter;
			this.yCenter = yCenter;
			this.xRange = xRange;
			this.yRange = yRange;
		}
	}
}
