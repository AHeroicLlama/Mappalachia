using System.Drawing.Imaging;

namespace Mappalachia
{
	static class ImageHelper
	{
		// Amends both (or either) the brightness and grayscale of the image
		public static void AdjustBrightnessOrGrayscale(this Image original, float brightness, bool grayscale)
		{
			using Graphics graphics = Graphics.FromImage(original);
			using ImageAttributes attributes = new ImageAttributes();

			attributes.SetColorMatrix(GenerateColorMatrix(brightness, grayscale));
			graphics.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
		}

		// Returns a ColorMatrix suitable for modifying the brightness and/or grayscale of an image
		static ColorMatrix GenerateColorMatrix(float brightness, bool grayscale)
		{
			return grayscale ?
			new ColorMatrix(
			[
				[0.299f * brightness, 0.299f * brightness, 0.299f * brightness, 0, 0],
				[0.587f * brightness, 0.587f * brightness, 0.587f * brightness, 0, 0],
				[0.114f * brightness, 0.114f * brightness, 0.114f * brightness, 0, 0],
				[0, 0, 0, 1, 0],
				[0, 0, 0, 0, 1],
			]) :
			new ColorMatrix(
			[
				[brightness, 0, 0, 0, 0],
				[0, brightness, 0, 0, 0],
				[0, 0, brightness, 0, 0],
				[0, 0, 0, 1, 0],
				[0, 0, 0, 0, 1],
			]);
		}
	}
}
