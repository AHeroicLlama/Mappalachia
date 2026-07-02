using System.ComponentModel;
using Library;

namespace Mappalachia
{
	public partial class FormMapView : Form
	{
		// The factor by which the map changes size for each scroll tick
		double ZoomRate { get; } = 1.2;

		int MaxDimension { get; } = 16384;

		int MinDimension { get; } = 128;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Image MapImage
		{
			get
			{
				return pictureBoxMapDisplay.Image ?? throw new Exception("Map image is null");
			}

			set
			{
				try
				{
					pictureBoxMapDisplay.Image = value;
				}
				catch (InvalidOperationException)
				{
					// Not recoverable, no action
				}
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

		// Try to pass shortcut keys to Main Form first
		protected override bool ProcessCmdKey(ref Message message, Keys keys)
		{
			if (FormMain.ProcessCmdKeyFromChild(ref message, keys))
			{
				return true;
			}

			return base.ProcessCmdKey(ref message, keys);
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

		// Open external on double-left-click
		private void PictureBoxMapDisplay_DoubleClick(object sender, EventArgs e)
		{
			if (((MouseEventArgs)e).Button == MouseButtons.Left)
			{
				FileIO.TempSave(MapImage, true);
			}
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

		// On right click, show a context menu, when selected pass the click location to Spotlight and lookup via the main form
		private void PictureBoxMapDisplay_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
			{
				return;
			}

			ContextMenuStrip contextMenu = new ContextMenuStrip();

			if (FormMain.Settings.Space.IsSuitableForSpotlight())
			{
				ToolStripMenuItem spotlight = new ToolStripMenuItem() { Text = "Spotlight Here" };

				spotlight.Click += async (s, args) =>
				{
					await FormMain.SetSpotlightLocation(GetClickPoint(e));
					SizeMapToForm();
				};

				contextMenu.Items.Add(spotlight);

				if (FormMain.Settings.MapSettings.SpotlightEnabled)
				{
					ToolStripMenuItem turnOff = new ToolStripMenuItem() { Text = "Turn Off Spotlight" };
					ToolStripMenuItem setSize = new ToolStripMenuItem() { Text = "Set Spotlight Size..." };

					turnOff.Click += async (s, args) =>
					{
						await FormMain.ToggleSpotlight(false);
						SizeMapToForm();
					};

					setSize.Click += async (s, args) =>
					{
						await FormMain.OpenSpotlightSetSizeDialog(TopMost);
					};

					contextMenu.Items.Add(setSize);
					contextMenu.Items.Add(turnOff);
				}

				contextMenu.Items.Add("-");
			}

			ToolStripMenuItem lookup = new ToolStripMenuItem() { Text = "Lookup Here" };

			lookup.Click += async (s, args) =>
			{
				await FormMain.SetLookupLocation(GetClickPoint(e));
			};

			contextMenu.Items.Add(lookup);

			if (FormMain.Settings.MapSettings.LookupEnabled)
			{
				ToolStripMenuItem turnOffLookup = new ToolStripMenuItem() { Text = "Turn Off Lookup" };

				turnOffLookup.Click += async (s, args) =>
				{
					await FormMain.ToggleLookup(false);
				};

				contextMenu.Items.Add(turnOffLookup);
			}

			if (contextMenu.Items.Count > 0)
			{
				contextMenu.Show(pictureBoxMapDisplay, e.Location);
			}
		}

		// Returns the pixel coordinate point of the click, accounting for pan/zoom and extended legend style
		PointF GetClickPoint(MouseEventArgs e)
		{
			float zoomFactor = (float)MapImage.Width / pictureBoxMapDisplay.Width;

			// Extended legend style results in a final image larger than the normal dimensions
			if (FormMain.Settings.MapSettings.LegendStyle == LegendStyle.Extended)
			{
				return new PointF(
					(e.X * zoomFactor) - (MapImage.Width - Common.MapImageResolution),
					(e.Y * zoomFactor) - ((MapImage.Width - Common.MapImageResolution) / 2));
			}
			else
			{
				return new PointF(e.X * zoomFactor, e.Y * zoomFactor);
			}
		}
	}
}
