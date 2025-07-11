namespace Mappalachia
{
	public class PlotIcon
	{
		public Color Color { get; }

		public Image? Image { get; }

		public int Size { get; }

		const string IconShapeNames = "ABCDEF";

		public PlotIcon(int offset, List<Color> palette, int size)
		{
			int colorIndex = offset % palette.Count;
			int iconIndex = (offset / palette.Count) % IconShapeNames.Length;

			Color = palette[colorIndex];
			Image = FileIO.GetPlotIconImage(IconShapeNames[iconIndex].ToString());
			Image = Image.Resize(size, size);
			Image = Image.SetColor(Color);
		}
	}
}
