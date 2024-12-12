namespace Library
{
	public class MapMarker(string icon, string label, uint spaceFormID = 0x00000000, double x = 0, double y = 0)
	{
		public string Icon { get; } = icon;

		public string Label { get; } = label;

		public uint SpaceFormID { get; } = spaceFormID;

		public double X { get; } = x;

		public double Y { get; } = y;
	}
}
