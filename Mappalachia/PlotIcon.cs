namespace Mappalachia
{
	public class PlotIcon()
	{
		public Color Color { get; set; } = Color.Red;

		Image? Image { get; set; }

		// TODO - placeholder
		public Image GetImage()
		{
			Image ??= Database.AllMapMarkers[new Random().Next(Database.AllMapMarkers.Count)].GetMapMarkerImage();
			return Image;
		}

		public override bool Equals(object? other)
		{
			if (other is null)
			{
				return false;
			}

			if (other is not PlotIcon)
			{
				return false;
			}

			// TODO
			throw new NotImplementedException();
		}

		public override int GetHashCode()
		{
			// TODO
			throw new NotImplementedException();
		}
	}
}
