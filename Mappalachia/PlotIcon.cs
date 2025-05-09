namespace Mappalachia
{
	public class PlotIcon(Color color)
	{
		public Color Color { get; set; } = color;

		// TODO - placeholder, requires shapes too
		public Image GetImage()
		{
			Pen pen = new Pen(Color, 8);
			Image image = new Bitmap(32, 32);
			Graphics graphics = Graphics.FromImage(image);
			graphics.DrawLine(pen, -16, -16, 16, 16);
			graphics.DrawLine(pen, 16, -16, 16, -16);

			return image;
		}
	}
}
