namespace Library
{
	public class MapMarker(string icon, string label, uint spaceFormID = 0x00000000, int x = 0, int y = 0)
	{
		public string Icon { get; } = icon;

		public string Label { get; } = label;

		public uint SpaceFormID { get; } = spaceFormID;

		public int X { get; } = x;

		public int Y { get; } = y;
	}
}
