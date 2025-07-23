namespace Mappalachia
{
	partial class FormMapView
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMapView));
			pictureBoxMapDisplay = new PictureBox();
			menuStripPreview = new MenuStrip();
			resetZoomMenuItem = new ToolStripMenuItem();
			keepOnTopMenuItem = new ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)pictureBoxMapDisplay).BeginInit();
			menuStripPreview.SuspendLayout();
			SuspendLayout();
			// 
			// pictureBoxMapDisplay
			// 
			pictureBoxMapDisplay.BackColor = Color.Black;
			pictureBoxMapDisplay.Location = new Point(0, 27);
			pictureBoxMapDisplay.Name = "pictureBoxMapDisplay";
			pictureBoxMapDisplay.Size = new Size(1008, 961);
			pictureBoxMapDisplay.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBoxMapDisplay.TabIndex = 1;
			pictureBoxMapDisplay.TabStop = false;
			pictureBoxMapDisplay.MouseClick += PictureBoxMapDisplay_MouseClick;
			pictureBoxMapDisplay.MouseDoubleClick += PictureBoxMapDisplay_DoubleClick;
			pictureBoxMapDisplay.MouseDown += PictureBoxMapDisplay_MouseDown;
			pictureBoxMapDisplay.MouseMove += PictureBoxMapDisplay_MouseMove;
			// 
			// menuStripPreview
			// 
			menuStripPreview.AllowClickThrough = true;
			menuStripPreview.BackColor = SystemColors.ControlDark;
			menuStripPreview.Items.AddRange(new ToolStripItem[] { resetZoomMenuItem, keepOnTopMenuItem });
			menuStripPreview.Location = new Point(0, 0);
			menuStripPreview.Name = "menuStripPreview";
			menuStripPreview.ShowItemToolTips = true;
			menuStripPreview.Size = new Size(1008, 24);
			menuStripPreview.TabIndex = 0;
			// 
			// resetZoomMenuItem
			// 
			resetZoomMenuItem.Name = "resetZoomMenuItem";
			resetZoomMenuItem.Size = new Size(82, 20);
			resetZoomMenuItem.Text = "Reset Zoom";
			resetZoomMenuItem.ToolTipText = "Move and resize the map image to best fit the window.";
			resetZoomMenuItem.Click += ResetZoom_Click;
			// 
			// keepOnTopMenuItem
			// 
			keepOnTopMenuItem.CheckOnClick = true;
			keepOnTopMenuItem.Name = "keepOnTopMenuItem";
			keepOnTopMenuItem.Size = new Size(83, 20);
			keepOnTopMenuItem.Text = "Keep on top";
			keepOnTopMenuItem.ToolTipText = "Force the preview window to show over everything else.";
			keepOnTopMenuItem.Click += KeepOnTop_Click;
			// 
			// FormMapView
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.Black;
			ClientSize = new Size(1008, 985);
			Controls.Add(pictureBoxMapDisplay);
			Controls.Add(menuStripPreview);
			Icon = (Icon)resources.GetObject("$this.Icon");
			MainMenuStrip = menuStripPreview;
			MinimumSize = new Size(256, 256);
			Name = "FormMapView";
			ShowInTaskbar = false;
			Text = "Mappalachia: Preview";
			FormClosing += FormMapView_FormClosing;
			((System.ComponentModel.ISupportInitialize)pictureBoxMapDisplay).EndInit();
			menuStripPreview.ResumeLayout(false);
			menuStripPreview.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private PictureBox pictureBoxMapDisplay;
		private MenuStrip menuStripPreview;
		private ToolStripMenuItem resetZoomMenuItem;
		private ToolStripMenuItem keepOnTopMenuItem;
	}
}