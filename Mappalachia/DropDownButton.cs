using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Mappalachia
{
	public class DropDownButton : Button
	{
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public ContextMenuStrip? ContextMenu { get; set; }

		static int DividerInsetPosition { get; } = 30;

		static int TriangleWidth { get; set; } = 10;

		static int TriangleHeight { get; set; } = 10;

		public DropDownButton()
		{
			Padding = new Padding(Padding.Left, Padding.Top, Math.Max(DividerInsetPosition / 2, Padding.Right), Padding.Bottom);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			var dividerRectangle = new Rectangle(Width - DividerInsetPosition, 0, DividerInsetPosition, Height);

			// Check the drop down has been left clicked
			if (ContextMenu != null &&
				e.Button == MouseButtons.Left &&
				dividerRectangle.Contains(e.Location))
			{
				ContextMenu.Show(this, 0, Height);
			}
			else
			{
				base.OnMouseDown(e);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (ContextMenu == null || DividerInsetPosition <= 0)
			{
				return;
			}

			Point arrowTopLeft = new Point(
				ClientRectangle.Width - ((DividerInsetPosition + TriangleWidth) / 2),
				(ClientRectangle.Height - TriangleHeight) / 2);
			Point arrowTopRight = new Point(arrowTopLeft.X + TriangleWidth, arrowTopLeft.Y);
			Point arrowTip = new Point(arrowTopLeft.X + (TriangleWidth / 2), arrowTopLeft.Y + TriangleHeight);

			Brush arrowBrush = Enabled ? SystemBrushes.ControlText : SystemBrushes.ButtonShadow;
			e.Graphics.FillPolygon(
				arrowBrush,
				[arrowTopLeft, arrowTopRight, arrowTip]);

			int lineX = ClientRectangle.Width - DividerInsetPosition;
			Point lineTop = new Point(lineX, 0);
			Point lineBot = new Point(lineX, ClientRectangle.Height);

			using Pen pen = new Pen(Brushes.DarkGray) { DashStyle = DashStyle.Dot };
			e.Graphics.DrawLine(pen, lineTop, lineBot);
		}
	}
}
