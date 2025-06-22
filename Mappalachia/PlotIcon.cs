namespace Mappalachia
{
	public class PlotIcon(Color color)
	{
		public Color Color { get; set; } = color;

		Image? Image { get; set; }

		// TODO - placeholder
		public Image GetImage()
		{
			Image ??= Database.AllMapMarkers[new Random().Next(Database.AllMapMarkers.Count) - 1].GetMapMarkerImage();
			return Image;
		}
	}
}
