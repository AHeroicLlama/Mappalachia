namespace Mappalachia
{
	partial class FormMain
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			menuStripMain = new MenuStrip();
			mapToolStripMenuItem = new ToolStripMenuItem();
			splitContainerMain = new SplitContainer();
			pictureBoxMapDisplay = new PictureBox();
			menuStripMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
			splitContainerMain.Panel2.SuspendLayout();
			splitContainerMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pictureBoxMapDisplay).BeginInit();
			SuspendLayout();
			// 
			// menuStripMain
			// 
			menuStripMain.Items.AddRange(new ToolStripItem[] { mapToolStripMenuItem });
			menuStripMain.Location = new Point(0, 0);
			menuStripMain.Name = "menuStripMain";
			menuStripMain.Size = new Size(1264, 24);
			menuStripMain.TabIndex = 0;
			menuStripMain.Text = "menuStrip1";
			// 
			// mapToolStripMenuItem
			// 
			mapToolStripMenuItem.Name = "mapToolStripMenuItem";
			mapToolStripMenuItem.Size = new Size(43, 20);
			mapToolStripMenuItem.Text = "Map";
			// 
			// splitContainerMain
			// 
			splitContainerMain.Dock = DockStyle.Fill;
			splitContainerMain.Location = new Point(0, 24);
			splitContainerMain.Name = "splitContainerMain";
			// 
			// splitContainerMain.Panel2
			// 
			splitContainerMain.Panel2.Controls.Add(pictureBoxMapDisplay);
			splitContainerMain.Size = new Size(1264, 657);
			splitContainerMain.SplitterDistance = 632;
			splitContainerMain.TabIndex = 1;
			// 
			// pictureBoxMapDisplay
			// 
			pictureBoxMapDisplay.Dock = DockStyle.Fill;
			pictureBoxMapDisplay.Location = new Point(0, 0);
			pictureBoxMapDisplay.Name = "pictureBoxMapDisplay";
			pictureBoxMapDisplay.Size = new Size(628, 657);
			pictureBoxMapDisplay.TabIndex = 0;
			pictureBoxMapDisplay.TabStop = false;
			// 
			// FormMain
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1264, 681);
			Controls.Add(splitContainerMain);
			Controls.Add(menuStripMain);
			Icon = (Icon)resources.GetObject("$this.Icon");
			MainMenuStrip = menuStripMain;
			Name = "FormMain";
			Text = "Mappalachia";
			menuStripMain.ResumeLayout(false);
			menuStripMain.PerformLayout();
			splitContainerMain.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
			splitContainerMain.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)pictureBoxMapDisplay).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private MenuStrip menuStripMain;
		private SplitContainer splitContainerMain;
		private PictureBox pictureBoxMapDisplay;
		private ToolStripMenuItem mapToolStripMenuItem;
	}
}
