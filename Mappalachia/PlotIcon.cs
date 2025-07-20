namespace Mappalachia
{
	public class PlotIcon
	{
		public Color Color { get; }

		public Image? Image { get; set; }

		public PlotIcon(int offset, List<Color> palette, int size)
		{
			int colorIndex = offset % palette.Count;
			Color = palette[colorIndex];

			int availableIcons = FileIO.GetAvailblePlotIconCount();
			int iconIndex = (offset / palette.Count) % availableIcons;

			// Gather the raw icon from file and set it to the requested size
			using Image baseIcon = FileIO.GetPlotIconImage(iconIndex).Resize(size, size);

			// Draw the dropshadow of the icon, then draw the main icon over that, meanwhile settings its color
			Image finalImage = new Bitmap(baseIcon.Width + (Map.DropShadowOffset * 2), baseIcon.Height + (Map.DropShadowOffset * 2));
			using Graphics graphics = Graphics.FromImage(finalImage);
			graphics.DrawImage(baseIcon.SetColor(Map.DropShadowColor), new PointF(Map.DropShadowOffset * 2, Map.DropShadowOffset * 2));
			graphics.DrawImage(baseIcon.SetColor(Color), new PointF(Map.DropShadowOffset, Map.DropShadowOffset));

			Image = finalImage;
		}
	}
}
