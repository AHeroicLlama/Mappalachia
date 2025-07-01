using System.Drawing.Imaging;
using System.Runtime.InteropServices;

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

		// Returns a copy of the original image, where all opaque black pixels are transparent
		// Note similar implementation to Preprocessor.ImageAssetValidation.GetBlackPxPercent
		public static Image ReplaceBlackWithTransparency(this Image image)
		{
			Bitmap bitmap = (Bitmap)image;

			// We copy the bitmap data out into a byte array, because accessing the pixels from the bitmap object is very slow
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			int size = bitmapData.Stride * bitmap.Height;
			byte[] buffer = new byte[size];
			Marshal.Copy(bitmapData.Scan0, buffer, 0, size);
			int stride = bitmapData.Stride;

			for (int x = 0; x < bitmapData.Height; x++)
			{
				for (int y = 0; y < bitmapData.Width; y++)
				{
					int alphaAddress = (x * stride) + (y * 4) + 3;

					// If the R, G, and B components are 0, but the A component is not (if the pixel is black but not already transparent)
					if (buffer[alphaAddress] != 0 &&
						buffer[(x * stride) + (y * 4) + 2] == 0 &&
						buffer[(x * stride) + (y * 4) + 1] == 0 &&
						buffer[(x * stride) + (y * 4)] == 0)
					{
						buffer[alphaAddress] = 0;
					}
				}
			}

			Marshal.Copy(buffer, 0, bitmapData.Scan0, size);
			bitmap.UnlockBits(bitmapData);

			return bitmap;
		}

		// Linearly interpolates the RGB values of two colors to form a new 'child' color
		public static Color LerpColors(Color colorX, Color colorY, double range)
		{
			return Color.FromArgb(
				(int)(colorX.R + ((colorY.R - colorX.R) * range)),
				(int)(colorX.G + ((colorY.G - colorX.G) * range)),
				(int)(colorX.B + ((colorY.B - colorX.B) * range)));
		}

		// Returns the Point which is the rounded version of the PointF
		public static Point AsPointByRounding(this PointF point)
		{
			return new Point(
				(int)Math.Round(point.X),
				(int)Math.Round(point.Y));
		}
	}
}
