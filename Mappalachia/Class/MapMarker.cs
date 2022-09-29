namespace Mappalachia
{
	class MapMarker
	{
		public string label;
		public string markerName;
		public double x;
		public double y;

		public MapMarker(string label, string markerName, int x, int y)
		{
			this.label = label;
			this.markerName = markerName;

			// Scale the coord to the image (flip y), then scale it to the given space, then apply a special correction for map markers
			this.x = Map.NudgeMapMarker(Map.ScaleCoordinateToSpace(Map.CorrectAxis(x, false), false), false);
			this.y = Map.NudgeMapMarker(Map.ScaleCoordinateToSpace(Map.CorrectAxis(y, true), true), true);
		}
	}
}
