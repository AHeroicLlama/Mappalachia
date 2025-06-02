namespace Mappalachia
{
	public partial class FormMapView : Form
	{
		Settings Settings { get; }

		double ZoomRate { get; } = 1.2;

		int MaxDimension { get; } = (int)Math.Pow(2, 14); // 16k

		int MinDimension { get; } = (int)Math.Pow(2, 7); // 128

		Point LastMouseDragEnd { get; set; }

		public FormMapView(Settings settings)
		{
			InitializeComponent();
			SquareForm(this, EventArgs.Empty);

			Settings = settings;

			SizeMapToForm();
		}

		void CenterMapInForm()
		{
			pictureBoxMapDisplay.Location = new Point((ClientSize.Width / 2) - (pictureBoxMapDisplay.Width / 2), (ClientSize.Height / 2) - (pictureBoxMapDisplay.Height / 2));
		}

		public void SizeMapToForm()
		{
			pictureBoxMapDisplay.Width = ClientSize.Width;
			pictureBoxMapDisplay.Height = ClientSize.Height;

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

		public void UpdateMap()
		{
			pictureBoxMapDisplay.Image = Map.Draw(Settings);
		}

		// Set the form itself so the 'client area'/viewport is square, (matching the map image)
		void SquareForm(object sender, EventArgs e)
		{
			Rectangle workingArea = Screen.FromControl(this).WorkingArea;

			// How does the form window itself differ from its content
			int additionalBorderHeight = Height - ClientSize.Height;
			int additionalBorderWidth = Width - ClientSize.Width;

			int newDimension = Math.Max(ClientSize.Width, ClientSize.Height);

			// Cap to the current monitor's min dimension, minus the borders
			newDimension = Math.Min(Math.Min(newDimension, workingArea.Width - additionalBorderWidth), workingArea.Height - additionalBorderHeight);

			// Apply the resize
			ClientSize = new Size(newDimension, newDimension);
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
	}
}
