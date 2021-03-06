﻿using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Mappalachia.Class
{
	// Methods for tweaking and applying effects to images.
	static class ImageTools
	{
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

			// Draw the shadow with offset from all 4 directions
			totalShadowGraphic.DrawImage(dropShadow, 0, -shadowWidth); // Up
			totalShadowGraphic.DrawImage(dropShadow, 0, shadowWidth); // Down
			totalShadowGraphic.DrawImage(dropShadow, -shadowWidth, 0); // Left
			totalShadowGraphic.DrawImage(dropShadow, shadowWidth, 0); // Right

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
			int newDimension = (int)Math.Ceiling(Math.Sqrt((image.Width * image.Width) + (image.Height * image.Height)));
			Image newImage = new Bitmap(newDimension, newDimension);

			Graphics graphic = Graphics.FromImage(newImage);

			// Move the image to the center, rotate it, then move it back
			graphic.TranslateTransform(newImage.Width / 2, newImage.Height / 2);
			graphic.RotateTransform(angle);
			graphic.TranslateTransform(-newImage.Width / 2, -newImage.Height / 2);

			graphic.DrawImage(image, (newDimension - image.Width) / 2, (newDimension - image.Height) / 2, image.Width, image.Height);

			return newImage;
		}
	}
}
