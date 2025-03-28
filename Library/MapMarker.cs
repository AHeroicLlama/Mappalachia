namespace Library
{
	public class MapMarker(string icon, string label, uint spaceFormID, Coord coord)
	{
		public string Icon { get; } = icon;

		public string Label { get; } = label;

		public uint SpaceFormID { get; } = spaceFormID;

		public Coord Coord { get; } = coord;
	}
}
