namespace Library
{
	public class MapMarker(string icon, string label, uint spaceFormID = 0x00000000, float x = 0, float y = 0)
	{
		public string Icon { get; } = icon;

		public string Label { get; } = label;

		public uint SpaceFormID { get; } = spaceFormID;

		public float X { get; } = x;

		public float Y { get; } = y;
	}
}
