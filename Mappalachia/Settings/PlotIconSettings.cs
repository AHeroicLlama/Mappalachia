using System.Text.Json.Serialization;

namespace Mappalachia
{
	public class PlotIconSettings
	{
		public static int MaxSize { get; } = 256;

		public static int MinSize { get; } = 16;

		public int Size { get; set; } = 50;

		List<Color> palette = new List<Color> { Color.OrangeRed, Color.Cyan, Color.Yellow, Color.Magenta, Color.Lime, Color.RoyalBlue, Color.Coral, Color.Green, };

		[JsonConverter(typeof(JsonConverterColorList))]
		public List<Color> Palette
		{
			get
			{
				return palette;
			}

			set
			{
				if (value is null || value.Count < 1)
				{
					palette = new List<Color>() { Color.White };
					return;
				}

				palette = value;
			}
		}

		List<Color> topographicPalette = new List<Color> { Color.Cyan, Color.OrangeRed, Color.White };

		[JsonConverter(typeof(JsonConverterColorList))]
		public List<Color> TopographicPalette
		{
			get
			{
				return topographicPalette;
			}

			set
			{
				if (value is null || value.Count < 1)
				{
					topographicPalette = new List<Color>() { Color.White };
					return;
				}

				topographicPalette = value;
			}
		}
	}
}
