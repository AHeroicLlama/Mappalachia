namespace Mappalachia
{
	public partial class FormMapView : Form
	{
		double ZoomRate { get; } = 1.2;

		int MaxDimension { get; } = (int)Math.Pow(2, 14); // 16k

		int MinDimension { get; } = (int)Math.Pow(2, 7); // 128

		Point LastMouseDragEnd { get; set; }

		public FormMapView()
		{
			InitializeComponent();
			SquareForm(this, EventArgs.Empty);

			SizeMapToForm();
			UpdateKeepOnTopText();
			menuStripPreview.BringToFront();
		}

		// Returns the currently displayed map as an Image
		public Image GetCurrentMapImage()
		{
			return pictureBoxMapDisplay.Image ?? throw new Exception("Map image is null");
		}

		void CenterMapInForm()
		{
			pictureBoxMapDisplay.Location = new Point((ClientSize.Width / 2) - (pictureBoxMapDisplay.Width / 2), ((ClientSize.Height + menuStripPreview.Height) / 2) - (pictureBoxMapDisplay.Height / 2));
		}

		public void SizeMapToForm()
		{
			int dimension = Math.Min(ClientSize.Width, ClientSize.Height - menuStripPreview.Height);

			pictureBoxMapDisplay.Width = dimension;
			pictureBoxMapDisplay.Height = dimension;

			CenterMapInForm();
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

			Point centerAboutPoint = e.Location;

			pictureBoxMapDisplay.Location = new Point(
				(int)Math.Round(((pictureBoxMapDisplay.Location.X - centerAboutPoint.X) * factor) + centerAboutPoint.X),
				(int)Math.Round(((pictureBoxMapDisplay.Location.Y - centerAboutPoint.Y) * factor) + centerAboutPoint.Y));
		}

		public void UpdateMap(Settings settings)
		{
			pictureBoxMapDisplay.Image = Map.Draw(settings);
		}

		// Set the form itself so the 'client area'/viewport is square, (matching the map image)
		void SquareForm(object sender, EventArgs e)
		{
			int minDimension = Math.Max(ClientSize.Width, ClientSize.Height);
			ClientSize = new Size(minDimension - menuStripPreview.Height, minDimension);
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

		// Don't close - just hide
		private void FormMapView_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			Hide();
		}

		private void ResetZoom_Click(object sender, EventArgs e)
		{
			SizeMapToForm();
		}

		private void KeepOnTop_Click(object sender, EventArgs e)
		{
			TopMost = !TopMost;
			UpdateKeepOnTopText();
		}

		void UpdateKeepOnTopText()
		{
			keepOnTopMenuItem.Text = "Keep on top: " + (TopMost ? "On" : "Off");
		}
	}
}
