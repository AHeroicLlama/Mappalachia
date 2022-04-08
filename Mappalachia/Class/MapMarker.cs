namespace Mappalachia
{
	class MapMarker
	{
		public string label;
		public double x;
		public double y;

		public MapMarker(string label, int x, int y)
		{
			this.label = label;
			this.x = Map.ScaleCoordinate(x, false);
			this.y = Map.ScaleCoordinate(y, true);
		}
	}
}
