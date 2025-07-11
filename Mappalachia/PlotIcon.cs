namespace Mappalachia
{
	public class PlotIcon
	{
		public Color Color { get; set; }

		public Image? Image { get; set; }

		const string IconShapeNames = "ABCDEF";

		public PlotIcon(int offset, List<Color> palette)
		{
			int colorIndex = offset % palette.Count;
			int iconIndex = (offset / palette.Count) % IconShapeNames.Length;

			Color = palette[colorIndex];
			Image = FileIO.GetPlotIconImage(IconShapeNames[iconIndex].ToString());
			Image = Image.SetColor(Color);
		}
	}
}
