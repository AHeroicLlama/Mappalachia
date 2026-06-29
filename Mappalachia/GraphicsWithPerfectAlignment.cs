using System.Drawing.Drawing2D;

namespace Mappalachia
{
	/* Provides a way to configure a Graphics object with parameters which ensure drawn objects
	 align pixel-perfectly and without gaps, and to then restore that when done. */
	class GraphicsWithPerfectAlignment : IDisposable
	{
		SmoothingMode OriginalSmoothingMode { get; }

		PixelOffsetMode OriginalPixelOffsetMode { get; }

		InterpolationMode OriginalInterpolationMode { get; }

		Graphics Graphics { get; }

		public GraphicsWithPerfectAlignment(Graphics graphics)
		{
			OriginalSmoothingMode = graphics.SmoothingMode;
			OriginalPixelOffsetMode = graphics.PixelOffsetMode;
			OriginalInterpolationMode = graphics.InterpolationMode;

			Graphics = graphics;

			graphics.SmoothingMode = SmoothingMode.None;
			graphics.PixelOffsetMode = PixelOffsetMode.Half;
			graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
		}

		public void Restore()
		{
			Graphics.SmoothingMode = OriginalSmoothingMode;
			Graphics.PixelOffsetMode = OriginalPixelOffsetMode;
			Graphics.InterpolationMode = OriginalInterpolationMode;
		}

		public void Dispose() => Restore();
	}
}
