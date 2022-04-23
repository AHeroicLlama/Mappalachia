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
			this.x = Map.ScaleCoordinate(x, false);
			this.y = Map.ScaleCoordinate(y, true);
		}
	}
}
