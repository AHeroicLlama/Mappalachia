namespace Library
{
	public class MapMarker
	{
		public string Icon { get; }

		public string Label { get; }

		public uint SpaceFormID { get; }

		public float X { get; }

		public float Y { get; }

		public MapMarker(string icon, string label, uint spaceFormID = 0x00000000, float x = 0, float y = 0)
		{
			Icon = icon;
			Label = label;
			X = x;
			Y = y;
		}
	}
}
