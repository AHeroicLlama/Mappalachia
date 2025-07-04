using System.ComponentModel;
using Library;

namespace Mappalachia
{
	public partial class FormMapView : Form
	{
		// The factor by which the map changes size for each scroll tick
		double ZoomRate { get; } = 1.2;

		int MaxDimension { get; } = (int)Math.Pow(2, 14); // 16k

		int MinDimension { get; } = (int)Math.Pow(2, 7); // 128

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Image MapImage
		{
			get
			{
				return pictureBoxMapDisplay.Image ?? throw new Exception("Map image is null");
			}

			set
			{
				pictureBoxMapDisplay.Image = value;
			}
		}

		FormMain FormMain { get; }

		Point LastMouseDragEnd { get; set; }

		public FormMapView(FormMain formMain)
		{
			InitializeComponent();

			SquareForm(this, EventArgs.Empty);
			SizeMapToForm();
			UpdateKeepOnTopText();
			menuStripPreview.BringToFront();

			FormMain = formMain;
		}

		// Return a value effectively representing the current zoom level
		float GetZoomFactor()
		{
			return (float)Common.MapImageResolution / pictureBoxMapDisplay.Width;
		}

		// Return a rectangle representing the extents that the map image is actually visible, given pan or zoom
		public RectangleF GetCurrentPanZoomView()
		{
			float factor = GetZoomFactor();

			return new RectangleF(
				(pictureBoxMapDisplay.Location.X * -1) * factor,
				((pictureBoxMapDisplay.Location.Y * -1) + menuStripPreview.Height) * factor,
				ClientSize.Width * factor,
				(ClientSize.Height - menuStripPreview.Height) * factor);
		}

		// Set the form itself so the 'client area'/viewport is square, (matching the map image)
		void SquareForm(object sender, EventArgs e)
		{
			int minDimension = Math.Max(ClientSize.Width, ClientSize.Height);
			ClientSize = new Size(minDimension - menuStripPreview.Height, minDimension);
		}

		public void SizeMapToForm()
		{
			int dimension = Math.Min(ClientSize.Width, ClientSize.Height - menuStripPreview.Height);

			pictureBoxMapDisplay.Width = dimension;
			pictureBoxMapDisplay.Height = dimension;

			CenterMapInForm();
		}

		void CenterMapInForm()
		{
			pictureBoxMapDisplay.Location = new Point(
				(ClientSize.Width / 2) - (pictureBoxMapDisplay.Width / 2),
				((ClientSize.Height + menuStripPreview.Height) / 2) - (pictureBoxMapDisplay.Height / 2));
		}

		void UpdateKeepOnTopText()
		{
			keepOnTopMenuItem.Text = "Keep on top: " + (TopMost ? "On" : "Off");
		}

		private void KeepOnTop_Click(object sender, EventArgs e)
		{
			TopMost = !TopMost;
			UpdateKeepOnTopText();
		}

		private void ResetZoom_Click(object sender, EventArgs e)
		{
			SizeMapToForm();
		}

		private void PictureBoxMapDisplay_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				pictureBoxMapDisplay.Location = new Point(
					pictureBoxMapDisplay.Location.X + e.Location.X - LastMouseDragEnd.X,
					pictureBoxMapDisplay.Location.Y + e.Location.Y - LastMouseDragEnd.Y);
			}
		}

		private void PictureBoxMapDisplay_MouseDown(object sender, MouseEventArgs e)
		{
			LastMouseDragEnd = e.Location;
		}

		private void PictureBoxMapDisplay_DoubleClick(object sender, EventArgs e)
		{
			FileIO.TempSave(MapImage, true);
		}

		// Intercept mouse wheel events to handle zooming
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (e.Delta == 0)
			{
				return;
			}

			double factor = e.Delta > 0 ? ZoomRate : 1 / ZoomRate;
			int newDimension = (int)Math.Round(pictureBoxMapDisplay.Width * factor);

			// Cap against min/max zoom
			if ((newDimension > MaxDimension) ||
				(newDimension < MinDimension))
			{
				return;
			}

			pictureBoxMapDisplay.Width = newDimension;
			pictureBoxMapDisplay.Height = newDimension;

			// Center about the cursor
			Point centerAboutPoint = e.Location;

			pictureBoxMapDisplay.Location = new Point(
				(int)Math.Round(((pictureBoxMapDisplay.Location.X - centerAboutPoint.X) * factor) + centerAboutPoint.X),
				(int)Math.Round(((pictureBoxMapDisplay.Location.Y - centerAboutPoint.Y) * factor) + centerAboutPoint.Y));
		}

		// Don't close - just hide
		private void FormMapView_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			Hide();
		}

		// On right click, show a context menu, when selected pass the click location to a draw call on the main form
		private void PictureBoxMapDisplay_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
			{
				return;
			}

			ContextMenuStrip contextMenu = new ContextMenuStrip();
			ToolStripMenuItem enhance = new ToolStripMenuItem() { Text = FormMain.IsSpotlightEnabled() ? "Disable Spotlight" : "Spotlight Here" };
			contextMenu.Items.Add(enhance);

			enhance.Click += (s, args) =>
			{
				float factor = GetZoomFactor();

				if (FormMain.IsSpotlightEnabled())
				{
					FormMain.TurnOffSpotlight();
				}
				else
				{
					FormMain.SetSpotlightLocation(new PointF(e.X * factor, e.Y * factor));
				}

				SizeMapToForm();
			};

			contextMenu.Show(pictureBoxMapDisplay, e.Location);
		}
	}
}
