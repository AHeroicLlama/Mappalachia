using System.Text.Json.Serialization;

namespace Mappalachia
{
	public class PlotIcon
	{
		public PlotIcon(PlotIcon source)
		{
			ParentIsRegion = source.ParentIsRegion;
			Size = source.Size;
			Color = source.Color;
			baseIconImage = source.BaseIconImage;
			BaseIconIndex = source.BaseIconIndex;
		}

		public PlotIcon(int offset, List<Color> palette, int size, bool parentIsRegion)
		{
			BaseIconIndex = offset;
			Size = size;
			ParentIsRegion = parentIsRegion;

			int colorIndex = offset % palette.Count;
			Color = palette[colorIndex];

			int availableIcons = FileIO.GetAvailablePlotIconCount();
			BaseIconIndex = (offset / palette.Count) % availableIcons;

			// Gather the raw icon from file and set it to the requested size
			baseIconImage = FileIO.GetPlotIconImage(BaseIconIndex);
		}

		public PlotIcon()
		{
		}

		public bool ParentIsRegion { get; set; }

		public int BaseIconIndex { get; set; }

		int size;

		public int Size
		{
			get => size;
			set
			{
				size = value;
				InvalidateCache();
			}
		}

		Color color;

		[JsonConverter(typeof(JsonConverterColor))]
		public Color Color
		{
			get => color;
			set
			{
				color = value;
				InvalidateCache();
			}
		}

		Image? baseIconImage;

		// The raw plot icon image from file - no shadow, no color
		public Image BaseIconImage
		{
			private get
			{
				baseIconImage ??= FileIO.GetPlotIconImage(BaseIconIndex);
				return baseIconImage;
			}

			set
			{
				baseIconImage = value;
				InvalidateCache();
			}
		}

		// A cache of this icon in various colors
		// Plotting in standard mode will also add one entry to this cache
		Dictionary<Color, Image> ColorCache { get; set; } = new Dictionary<Color, Image>();

		// A cache of just the drop shadow image for this icon
		Image? DropShadowCache { get; set; }

		// A cache of the final icon image (for standard plot mode)
		Image? FinalImageCache { get; set; }

		public Image GetImage()
		{
			FinalImageCache ??= GenerateIconImage(Color);
			return FinalImageCache;
		}

		void InvalidateCache()
		{
			FinalImageCache = null;
			DropShadowCache = null;
			ColorCache.Clear();
		}

		// Re-draw the icon image, using a different color
		public Image GetImage(Color color)
		{
			return GenerateIconImage(color);
		}

		Image GenerateIconImage(Color color)
		{
			if (ParentIsRegion)
			{
				Image volumeImage = new Bitmap(Size, Size);
				using Graphics volumeGraphics = ImageHelper.GraphicsFromImageHQ(volumeImage);
				volumeGraphics.Clear(color);

				return volumeImage;
			}

			if (ColorCache.TryGetValue(color, out Image? cachedColoredIcon))
			{
				return cachedColoredIcon;
			}

			try
			{
				// Draw the drop shadow of the icon, then draw the main icon over that, meanwhile settings its color
				Image image = DropShadowCache is not null ? new Bitmap(DropShadowCache) : new Bitmap(Size + (Map.DropShadowOffset * 2), Size + (Map.DropShadowOffset * 2));
				using Graphics graphics = ImageHelper.GraphicsFromImageHQ(image);

				if (DropShadowCache is null)
				{
					graphics.DrawImage(BaseIconImage.SetColor(Map.DropShadowColor), new RectangleF(Map.DropShadowOffset * 2, Map.DropShadowOffset * 2, Size, Size));
					DropShadowCache = new Bitmap(image);
				}

				graphics.DrawImage(BaseIconImage.SetColor(color), new RectangleF(Map.DropShadowOffset, Map.DropShadowOffset, Size, Size));

				ColorCache.TryAdd(color, image);

				return image;
			}

			// Protects from a rare race condition where multiple threads try to generate and populate the cache having both missed
			catch (Exception)
			{
				return ColorCache[color];
			}
		}
	}
}
