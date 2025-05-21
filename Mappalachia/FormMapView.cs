namespace Mappalachia
{
	public partial class FormMapView : Form
	{
		int LastWidth { get; set; }

		int LastHeight { get; set; }

		FormMain MainForm { get; }

		double ZoomRate { get; } = 1.1;

		int MaxDimension { get; } = (int)Math.Pow(2, 14); // 16k

		int MinDimension { get; } = (int)Math.Pow(2, 7); // 128

		public FormMapView(FormMain mainForm)
		{
			InitializeComponent();
			KeepSquare(this, EventArgs.Empty);

			Shown += (s, e) =>
			{
				UpdateMap();
			};

			MainForm = mainForm;

			SizeMapToForm();
		}

		void CenterMapInForm()
		{
			pictureBoxMapDisplay.Location = new Point((ClientSize.Width / 2) - (pictureBoxMapDisplay.Width / 2), (ClientSize.Height / 2) - (pictureBoxMapDisplay.Height / 2));
		}

		void SizeMapToForm()
		{
			pictureBoxMapDisplay.Width = ClientSize.Width;
			pictureBoxMapDisplay.Height = ClientSize.Height;

			CenterMapInForm();
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (e.Delta == 0)
			{
				return;
			}

			double factor = e.Delta > 0 ? ZoomRate : 1 / ZoomRate;

			int newDimension = (int)Math.Round(Math.Min(Math.Max(pictureBoxMapDisplay.Width * factor, MinDimension), MaxDimension));

			pictureBoxMapDisplay.Width = newDimension;
			pictureBoxMapDisplay.Height = newDimension;

			CenterMapInForm();
		}

		public void UpdateMap()
		{
			pictureBoxMapDisplay.Image = Map.Draw(MainForm.Settings);
		}

		// Called when resized, this sizes the form so that the map image fits squarely within it
		void KeepSquare(object sender, EventArgs e)
		{
			Rectangle workingArea = Screen.FromControl(this).WorkingArea;

			// How does the form window itself differ from its content
			int additionalBorderHeight = Height - ClientSize.Height;
			int additionalBorderWidth = Width - ClientSize.Width;

			// What were the parameters of this resize event
			int widthChange = ClientSize.Width - LastWidth;
			int heightChange = ClientSize.Height - LastHeight;

			int newDimension;

			if (heightChange == 0)
			{
				newDimension = ClientSize.Width;
			}
			else if (widthChange == 0)
			{
				newDimension = ClientSize.Height;
			}

			// Both or neither changed
			else
			{
				newDimension = Math.Max(ClientSize.Width, ClientSize.Height);
			}

			// Cap to the current monitor's min dimension, minus the borders
			newDimension = Math.Min(Math.Min(newDimension, workingArea.Width - additionalBorderWidth), workingArea.Height - additionalBorderHeight);

			// Apply the resize
			ClientSize = new Size(newDimension, newDimension);

			// Re-capture the dimensions for the next resize
			LastWidth = ClientSize.Width;
			LastHeight = ClientSize.Height;

			// If the form was actually resized, ensure the dimensions we added mean it stays within the display
			if (heightChange != 0 || widthChange != 0)
			{
				KeepWithinDisplay(sender, e);
			}
		}

		// Moves the form in from the bottom or right of the display
		void KeepWithinDisplay(object sender, EventArgs e)
		{
			Rectangle workingArea = Screen.FromControl(this).WorkingArea;

			int xOverstep = Location.X + Width - workingArea.Right;
			int yOverstep = Location.Y + Height - workingArea.Bottom;

			if (xOverstep > 0)
			{
				Location = new Point(Location.X - xOverstep, Location.Y);
			}

			if (yOverstep > 0)
			{
				Location = new Point(Location.X, Location.Y - yOverstep);
			}
		}

		// Don't close - just hide
		private void FormMapView_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			Hide();
		}
	}
}
