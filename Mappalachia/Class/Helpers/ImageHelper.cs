using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace Mappalachia
{
	// Methods for tweaking and applying effects to images.
	static class ImageHelper
	{
		static readonly Regex htmlColor = new Regex("ff[0-9a-f]{6}"); // Match the alpha-less 'html' hex color

		// Shift the ARGB values of an image
		public static Image AdjustARGB(Image image, Color argb)
		{
			float a = argb.A / 255f;
			float r = argb.R / 255f;
			float g = argb.G / 255f;
			float b = argb.B / 255f;

			// Declare new ColorMatrix with ARGB values and store it in an ImageAttributes
			ColorMatrix matrix = new ColorMatrix(new float[][]
			{
				new float[] { r, 0, 0, 0, 0 },
				new float[] { 0, g, 0, 0, 0 },
				new float[] { 0, 0, b, 0, 0 },
				new float[] { 0, 0, 0, a, 0 },
				new float[] { 0, 0, 0, 0, 1 },
			});
			ImageAttributes attributes = new ImageAttributes();
			attributes.SetColorMatrix(matrix);

			Point[] points =
			{
				new Point(0, 0),
				new Point(image.Width, 0),
				new Point(0, image.Height),
			};
			Rectangle rectangle = new Rectangle(0, 0, image.Width, image.Height);

			Bitmap bitmap = new Bitmap(image.Width, image.Height);
			Graphics graphic = Graphics.FromImage(bitmap);

			// Redraw the image with new ImageAttributes
			graphic.DrawImage(image, points, rectangle, GraphicsUnit.Pixel, attributes);

			return bitmap;
		}

		// Add a basic drop shadow effect from all 4 cardinal directions
		public static Image AddDropShadow(Image image, float shadowWidth, int shadowOpacity)
		{
			// Generate the basic image for the shadow shape
			Image dropShadow = new Bitmap(image, image.Width, image.Height);

			// Pad the image out enough to fit the additional size required by the shadow extending the original
			dropShadow = PadImage(dropShadow, shadowWidth);
			Graphics totalShadowGraphic = Graphics.FromImage(dropShadow);

			// Draw the shadow with offset in 2 directions
			totalShadowGraphic.DrawImage(dropShadow, shadowWidth, shadowWidth);

			// Color the shadow black and apply alpha evenly
			dropShadow = AdjustARGB(dropShadow, Color.FromArgb(shadowOpacity, Color.Black));
			Graphics dropShadowGraphic = Graphics.FromImage(dropShadow);

			// Draw the original image over the drop shadow
			dropShadowGraphic.DrawImage(image, shadowWidth, shadowWidth);

			return dropShadow;
		}

		// Pad an image with x additional pixels around the border
		static Image PadImage(Image image, float pad)
		{
			// Create blank canvas
			Bitmap paddedImage = new Bitmap((int)(image.Width + (pad * 2)), (int)(image.Height + (pad * 2)));
			Graphics canvas = Graphics.FromImage(paddedImage);

			// Draw original image centered over expanded canvas
			canvas.DrawImage(image, pad, pad, image.Width, image.Height);

			return paddedImage;
		}

		// Return a bitmap rotated by given angle. May enlarge image in order to avoid shapes falling outside of the boundary
		public static Image RotateImage(Image image, int angle)
		{
			// Pythagoras on the X and Y coord of the bitmap gives us the maximum possible boundary required when all rotations considered
			int newDimension = (int)Math.Ceiling(GeometryHelper.GetMaximumBoundingBoxWidth(image.Width, image.Height));
			Image newImage = new Bitmap(newDimension, newDimension);

			Graphics graphic = Graphics.FromImage(newImage);

			// Move the image to the center, rotate it, then move it back
			graphic.TranslateTransform(newImage.Width / 2, newImage.Height / 2);
			graphic.RotateTransform(angle);
			graphic.TranslateTransform(-newImage.Width / 2, -newImage.Height / 2);

			graphic.DrawImage(image, (newDimension - image.Width) / 2, (newDimension - image.Height) / 2, image.Width, image.Height);

			return newImage;
		}

		// Linearly interpolates the RGB values of two colors to form a new 'child' color
		public static Color LerpColors(Color colorX, Color colorY, double range)
		{
			return Color.FromArgb(
				(int)(colorX.R + ((colorY.R - colorX.R) * range)),
				(int)(colorX.G + ((colorY.G - colorX.G) * range)),
				(int)(colorX.B + ((colorY.B - colorX.B) * range)));
		}

		// Find a color by its name or html code
		public static Color GetColorFromText(string colorNameOrCode)
		{
			try
			{
				return htmlColor.Match(colorNameOrCode.ToLower()).Success ?
					ColorTranslator.FromHtml("#" + colorNameOrCode.ToLower()) :
					Color.FromName(colorNameOrCode);
			}
			catch (Exception)
			{
				Notify.Error("Invalid color name " + colorNameOrCode + ". Unable to display color on palette.");
				return Color.Gray;
			}
		}

		public static Image AdjustBrightnessOrGrayscale(Image image, float brightness, bool grayscale)
		{
			Image newImage = new Bitmap(image.Width, image.Height);
			Graphics graphic = Graphics.FromImage(newImage);

			ImageAttributes attributes = new ImageAttributes();
			attributes.SetColorMatrix(GenerateColorMatrix(brightness, grayscale));

			Point[] points =
			{
				new Point(0, 0),
				new Point(newImage.Width, 0),
				new Point(0, newImage.Height),
			};

			Rectangle rect = new Rectangle(0, 0, newImage.Width, newImage.Height);
			graphic.DrawImage(image, points, rect, GraphicsUnit.Pixel, attributes);

			return newImage;
		}

		// Returns a ColorMatrix suitable for modifying the brightness and/or grayscale of an image
		public static ColorMatrix GenerateColorMatrix(float brightness, bool grayscale)
		{
			return grayscale ?
				new ColorMatrix(new float[][]
				{
					new float[] { 0.299f * brightness, 0.299f * brightness, 0.299f * brightness, 0, 0 },
					new float[] { 0.587f * brightness, 0.587f * brightness, 0.587f * brightness, 0, 0 },
					new float[] { 0.114f * brightness, 0.114f * brightness, 0.114f * brightness, 0, 0 },
					new float[] { 0, 0, 0, 1, 0 },
					new float[] { 0, 0, 0, 0, 1 },
				}) :
				new ColorMatrix(new float[][]
				{
					new float[] { brightness, 0, 0, 0, 0 },
					new float[] { 0, brightness, 0, 0, 0 },
					new float[] { 0, 0, brightness, 0, 0 },
					new float[] { 0, 0, 0, 1, 0 },
					new float[] { 0, 0, 0, 0, 1 },
				});
		}
	}
}
