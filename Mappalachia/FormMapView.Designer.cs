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
			((System.ComponentModel.ISupportInitialize)pictureBoxMapDisplay).BeginInit();
			SuspendLayout();
			// 
			// pictureBoxMapDisplay
			// 
			pictureBoxMapDisplay.Dock = DockStyle.Fill;
			pictureBoxMapDisplay.Location = new Point(0, 0);
			pictureBoxMapDisplay.Name = "pictureBoxMapDisplay";
			pictureBoxMapDisplay.Size = new Size(1008, 985);
			pictureBoxMapDisplay.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBoxMapDisplay.TabIndex = 1;
			pictureBoxMapDisplay.TabStop = false;
			// 
			// FormMapView
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.Black;
			ClientSize = new Size(1008, 985);
			Controls.Add(pictureBoxMapDisplay);
			Icon = (Icon)resources.GetObject("$this.Icon");
			MinimizeBox = false;
			MinimumSize = new Size(256, 256);
			Name = "FormMapView";
			ShowInTaskbar = false;
			Text = "Mappalachia: Preview";
			FormClosing += FormMapView_FormClosing;
			ResizeEnd += KeepSquare;
			((System.ComponentModel.ISupportInitialize)pictureBoxMapDisplay).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private PictureBox pictureBoxMapDisplay;
	}
}