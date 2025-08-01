using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Mappalachia
{
	public class DropDownButton : Button
	{
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public ContextMenuStrip? ContextMenu { get; set; }

		float DividerInsetPosition { get; set; }

		float TriangleWidth { get; set; }

		float TriangleHeight { get; set; }

		public DropDownButton()
		{
			Padding = new Padding(Padding.Left, Padding.Top, (int)Math.Round(Math.Max(DividerInsetPosition / 2, Padding.Right)), Padding.Bottom);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			var dividerRectangle = new RectangleF(Width - DividerInsetPosition, 0, DividerInsetPosition, Height);

			// Check the drop down has been left clicked
			if (ContextMenu is not null &&
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

			DividerInsetPosition = ClientRectangle.Width / 5f;
			TriangleWidth = DividerInsetPosition / 3f;
			TriangleHeight = ClientRectangle.Height / 5f;

			if (ContextMenu is null || DividerInsetPosition <= 0)
			{
				return;
			}

			PointF arrowTopLeft = new PointF(
				ClientRectangle.Width - ((DividerInsetPosition + TriangleWidth) / 2),
				(ClientRectangle.Height - TriangleHeight) / 2);
			PointF arrowTopRight = new PointF(arrowTopLeft.X + TriangleWidth, arrowTopLeft.Y);
			PointF arrowTip = new PointF(arrowTopLeft.X + (TriangleWidth / 2), arrowTopLeft.Y + TriangleHeight);

			Brush arrowBrush = Enabled ? SystemBrushes.ControlText : SystemBrushes.ButtonShadow;
			e.Graphics.FillPolygon(
				arrowBrush,
				[arrowTopLeft, arrowTopRight, arrowTip]);

			float lineX = ClientRectangle.Width - DividerInsetPosition;
			PointF lineTop = new PointF(lineX, 0);
			PointF lineBot = new PointF(lineX, ClientRectangle.Height);

			using Pen pen = new Pen(Brushes.DarkGray) { DashStyle = DashStyle.Dot };
			e.Graphics.DrawLine(pen, lineTop, lineBot);
		}
	}
}
