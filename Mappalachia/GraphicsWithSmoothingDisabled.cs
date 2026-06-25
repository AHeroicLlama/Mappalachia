using System.Drawing.Drawing2D;

namespace Mappalachia
{
	/* Provides a way to configure a Graphics object with parameters which ensure drawn objects
	 align pixel-perfectly and without gaps, and to then restore that when done. */
	class GraphicsWithSmoothingDisabled : IDisposable
	{
		SmoothingMode OriginalSmoothingMode { get; }

		Graphics Graphics { get; }

		public GraphicsWithSmoothingDisabled(Graphics graphics)
		{
			OriginalSmoothingMode = graphics.SmoothingMode;
			Graphics = graphics;

			graphics.SmoothingMode = SmoothingMode.None;
		}

		public void Restore()
		{
			Graphics.SmoothingMode = OriginalSmoothingMode;
		}

		public void Dispose() => Restore();
	}
}
