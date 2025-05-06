namespace Mappalachia
{
	public partial class FormMapView : Form
	{
		int LastWidth { get; set; }

		int LastHeight { get; set; }

		public FormMapView()
		{
			InitializeComponent();
			KeepSquare(this, EventArgs.Empty);

			Shown += (s, e) =>
			{
				UpdateMap();
			};
		}

		public async void UpdateMap()
		{
			pictureBoxMapDisplay.Image = await Map.Draw(Settings.CurrentSpace, Settings.BackgroundImage);
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
