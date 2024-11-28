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
			buttonSearch = new Button();
			textBoxSearch = new TextBox();
			dataGridViewSearchResults = new DataGridView();
			pictureBoxMapDisplay = new PictureBox();
			menuStripMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
			splitContainerMain.Panel1.SuspendLayout();
			splitContainerMain.Panel2.SuspendLayout();
			splitContainerMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dataGridViewSearchResults).BeginInit();
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
			splitContainerMain.BackColor = SystemColors.ControlDarkDark;
			splitContainerMain.Dock = DockStyle.Fill;
			splitContainerMain.Location = new Point(0, 24);
			splitContainerMain.Name = "splitContainerMain";
			// 
			// splitContainerMain.Panel1
			// 
			splitContainerMain.Panel1.Controls.Add(buttonSearch);
			splitContainerMain.Panel1.Controls.Add(textBoxSearch);
			splitContainerMain.Panel1.Controls.Add(dataGridViewSearchResults);
			// 
			// splitContainerMain.Panel2
			// 
			splitContainerMain.Panel2.Controls.Add(pictureBoxMapDisplay);
			splitContainerMain.Size = new Size(1264, 657);
			splitContainerMain.SplitterDistance = 632;
			splitContainerMain.TabIndex = 1;
			// 
			// buttonSearch
			// 
			buttonSearch.Location = new Point(427, 12);
			buttonSearch.Name = "buttonSearch";
			buttonSearch.Size = new Size(75, 23);
			buttonSearch.TabIndex = 2;
			buttonSearch.Text = "Search";
			buttonSearch.UseVisualStyleBackColor = true;
			buttonSearch.Click += ButtonSearch_Click;
			// 
			// textBoxSearch
			// 
			textBoxSearch.Location = new Point(131, 12);
			textBoxSearch.Name = "textBoxSearch";
			textBoxSearch.Size = new Size(290, 23);
			textBoxSearch.TabIndex = 1;
			textBoxSearch.Text = "Iron";
			// 
			// dataGridViewSearchResults
			// 
			dataGridViewSearchResults.AllowUserToAddRows = false;
			dataGridViewSearchResults.AllowUserToDeleteRows = false;
			dataGridViewSearchResults.AllowUserToOrderColumns = true;
			dataGridViewSearchResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			dataGridViewSearchResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dataGridViewSearchResults.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			dataGridViewSearchResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewSearchResults.EditMode = DataGridViewEditMode.EditProgrammatically;
			dataGridViewSearchResults.Location = new Point(3, 92);
			dataGridViewSearchResults.Name = "dataGridViewSearchResults";
			dataGridViewSearchResults.ReadOnly = true;
			dataGridViewSearchResults.RowHeadersVisible = false;
			dataGridViewSearchResults.ScrollBars = ScrollBars.Vertical;
			dataGridViewSearchResults.SelectionMode = DataGridViewSelectionMode.CellSelect;
			dataGridViewSearchResults.Size = new Size(626, 309);
			dataGridViewSearchResults.TabIndex = 0;
			// 
			// pictureBoxMapDisplay
			// 
			pictureBoxMapDisplay.Dock = DockStyle.Fill;
			pictureBoxMapDisplay.Location = new Point(0, 0);
			pictureBoxMapDisplay.Name = "pictureBoxMapDisplay";
			pictureBoxMapDisplay.Size = new Size(628, 657);
			pictureBoxMapDisplay.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBoxMapDisplay.TabIndex = 0;
			pictureBoxMapDisplay.TabStop = false;
			// 
			// FormMain
			// 
			AcceptButton = buttonSearch;
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
			splitContainerMain.Panel1.ResumeLayout(false);
			splitContainerMain.Panel1.PerformLayout();
			splitContainerMain.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
			splitContainerMain.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dataGridViewSearchResults).EndInit();
			((System.ComponentModel.ISupportInitialize)pictureBoxMapDisplay).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private MenuStrip menuStripMain;
		private SplitContainer splitContainerMain;
		private PictureBox pictureBoxMapDisplay;
		private ToolStripMenuItem mapToolStripMenuItem;
		private DataGridView dataGridViewSearchResults;
		private Button buttonSearch;
		private TextBox textBoxSearch;
	}
}
