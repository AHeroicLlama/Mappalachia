namespace Mappalachia
{
	public class PlotIcon
	{
		public PlotIcon(PlotIcon source)
		{
			Parent = source.Parent;
			Size = source.Size;
			Color = source.Color;
			baseIconImage = source.BaseIconImage;
		}

		public PlotIcon(int offset, List<Color> palette, int size, GroupedSearchResult parent)
		{
			Size = size;
			Parent = parent;

			int colorIndex = offset % palette.Count;
			Color = palette[colorIndex];

			int availableIcons = FileIO.GetAvailblePlotIconCount();
			int iconIndex = (offset / palette.Count) % availableIcons;

			// Gather the raw icon from file and set it to the requested size
			baseIconImage = FileIO.GetPlotIconImage(iconIndex);
		}

		int size;

		public int Size
		{
			get => size;
			set
			{
				size = value;
				InvalidateImage();
			}
		}

		Color color;

		public Color Color
		{
			get => color;
			set
			{
				color = value;
				InvalidateImage();
			}
		}

		Image baseIconImage;

		// The raw plot icon image from file - no shadow, no color
		public Image BaseIconImage
		{
			private get => baseIconImage;
			set
			{
				baseIconImage = value;
				InvalidateImage();
			}
		}

		Image? DropShadowCache { get; set; }

		Image? FinalImage { get; set; }

		public GroupedSearchResult Parent { get; }

		public Image GetImage()
		{
			FinalImage ??= GenerateIconImage(Color);
			return FinalImage;
		}

		void InvalidateImage()
		{
			FinalImage = null;
			DropShadowCache = null;
		}

		// Re-draw the icon image, using a different color
		public Image GetImage(Color color)
		{
			return GenerateIconImage(color);
		}

		Image GenerateIconImage(Color color)
		{
			if (Parent.Entity is Library.Region)
			{
				Image volumeImage = new Bitmap(Size, Size);
				using Graphics volumeGraphics = ImageHelper.GraphicsFromImageHQ(volumeImage);
				volumeGraphics.Clear(color);

				return volumeImage;
			}

			// Draw the dropshadow of the icon, then draw the main icon over that, meanwhile settings its color
			Image image = DropShadowCache != null ? new Bitmap(DropShadowCache) : new Bitmap(Size + (Map.DropShadowOffset * 2), Size + (Map.DropShadowOffset * 2));
			using Graphics graphics = ImageHelper.GraphicsFromImageHQ(image);

			if (DropShadowCache == null)
			{
				graphics.DrawImage(BaseIconImage.SetColor(Map.DropShadowColor), new RectangleF(Map.DropShadowOffset * 2, Map.DropShadowOffset * 2, Size, Size));
				DropShadowCache = image;
			}

			graphics.DrawImage(BaseIconImage.SetColor(color), new RectangleF(Map.DropShadowOffset, Map.DropShadowOffset, Size, Size));

			FinalImage = image;
			return FinalImage;
		}
	}
}
