namespace Mappalachia
{
	partial class FormAbout
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
			tableLayoutPanel = new TableLayoutPanel();
			linkLabelGitHub = new LinkLabel();
			logoPictureBox = new PictureBox();
			labelProductName = new Label();
			labelVersion = new Label();
			labelCopyright = new Label();
			textBoxDescription = new TextBox();
			okButton = new Button();
			tableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)logoPictureBox).BeginInit();
			SuspendLayout();
			// 
			// tableLayoutPanel
			// 
			tableLayoutPanel.ColumnCount = 2;
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34.5F));
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65.5F));
			tableLayoutPanel.Controls.Add(linkLabelGitHub, 1, 3);
			tableLayoutPanel.Controls.Add(logoPictureBox, 0, 0);
			tableLayoutPanel.Controls.Add(labelProductName, 1, 0);
			tableLayoutPanel.Controls.Add(labelVersion, 1, 1);
			tableLayoutPanel.Controls.Add(labelCopyright, 1, 2);
			tableLayoutPanel.Controls.Add(textBoxDescription, 1, 4);
			tableLayoutPanel.Controls.Add(okButton, 1, 5);
			tableLayoutPanel.Dock = DockStyle.Fill;
			tableLayoutPanel.Location = new Point(10, 10);
			tableLayoutPanel.Margin = new Padding(4, 3, 4, 3);
			tableLayoutPanel.Name = "tableLayoutPanel";
			tableLayoutPanel.RowCount = 6;
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
			tableLayoutPanel.Size = new Size(487, 307);
			tableLayoutPanel.TabIndex = 0;
			// 
			// linkLabelGitHub
			// 
			linkLabelGitHub.AutoSize = true;
			linkLabelGitHub.Dock = DockStyle.Fill;
			linkLabelGitHub.ImageAlign = ContentAlignment.MiddleLeft;
			linkLabelGitHub.LinkArea = new LinkArea(0, 127);
			linkLabelGitHub.Location = new Point(175, 90);
			linkLabelGitHub.Margin = new Padding(7, 0, 4, 0);
			linkLabelGitHub.Name = "linkLabelGitHub";
			linkLabelGitHub.Size = new Size(308, 30);
			linkLabelGitHub.TabIndex = 1;
			linkLabelGitHub.TabStop = true;
			linkLabelGitHub.Text = "Repo";
			linkLabelGitHub.UseCompatibleTextRendering = true;
			linkLabelGitHub.LinkClicked += LinkLabelGitHub_LinkClicked;
			// 
			// logoPictureBox
			// 
			logoPictureBox.Dock = DockStyle.Fill;
			logoPictureBox.Image = (Image)resources.GetObject("logoPictureBox.Image");
			logoPictureBox.Location = new Point(4, 3);
			logoPictureBox.Margin = new Padding(4, 3, 4, 3);
			logoPictureBox.Name = "logoPictureBox";
			tableLayoutPanel.SetRowSpan(logoPictureBox, 6);
			logoPictureBox.Size = new Size(160, 301);
			logoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
			logoPictureBox.TabIndex = 12;
			logoPictureBox.TabStop = false;
			// 
			// labelProductName
			// 
			labelProductName.Dock = DockStyle.Fill;
			labelProductName.Location = new Point(175, 0);
			labelProductName.Margin = new Padding(7, 0, 4, 0);
			labelProductName.MaximumSize = new Size(0, 20);
			labelProductName.Name = "labelProductName";
			labelProductName.Size = new Size(308, 20);
			labelProductName.TabIndex = 19;
			labelProductName.Text = "Product Name";
			labelProductName.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// labelVersion
			// 
			labelVersion.Dock = DockStyle.Fill;
			labelVersion.Location = new Point(175, 30);
			labelVersion.Margin = new Padding(7, 0, 4, 0);
			labelVersion.MaximumSize = new Size(0, 20);
			labelVersion.Name = "labelVersion";
			labelVersion.Size = new Size(308, 20);
			labelVersion.TabIndex = 0;
			labelVersion.Text = "Version";
			labelVersion.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// labelCopyright
			// 
			labelCopyright.Dock = DockStyle.Fill;
			labelCopyright.Location = new Point(175, 60);
			labelCopyright.Margin = new Padding(7, 0, 4, 0);
			labelCopyright.MaximumSize = new Size(0, 20);
			labelCopyright.Name = "labelCopyright";
			labelCopyright.Size = new Size(308, 20);
			labelCopyright.TabIndex = 21;
			labelCopyright.Text = "Copyright";
			labelCopyright.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// textBoxDescription
			// 
			textBoxDescription.Dock = DockStyle.Fill;
			textBoxDescription.Location = new Point(175, 123);
			textBoxDescription.Margin = new Padding(7, 3, 4, 3);
			textBoxDescription.Multiline = true;
			textBoxDescription.Name = "textBoxDescription";
			textBoxDescription.ReadOnly = true;
			textBoxDescription.ScrollBars = ScrollBars.Both;
			textBoxDescription.Size = new Size(308, 147);
			textBoxDescription.TabIndex = 23;
			textBoxDescription.TabStop = false;
			textBoxDescription.Text = "Description";
			// 
			// okButton
			// 
			okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			okButton.DialogResult = DialogResult.Cancel;
			okButton.Location = new Point(395, 277);
			okButton.Margin = new Padding(4, 3, 4, 3);
			okButton.Name = "okButton";
			okButton.Size = new Size(88, 27);
			okButton.TabIndex = 24;
			okButton.Text = "&OK";
			// 
			// FormAbout
			// 
			AcceptButton = okButton;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(507, 327);
			Controls.Add(tableLayoutPanel);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			Margin = new Padding(4, 3, 4, 3);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "FormAbout";
			Padding = new Padding(10);
			ShowIcon = false;
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "AboutBox";
			tableLayoutPanel.ResumeLayout(false);
			tableLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)logoPictureBox).EndInit();
			ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.PictureBox logoPictureBox;
		private System.Windows.Forms.Label labelProductName;
		private System.Windows.Forms.Label labelVersion;
		private System.Windows.Forms.Label labelCopyright;
		private System.Windows.Forms.TextBox textBoxDescription;
		private System.Windows.Forms.Button okButton;
		private LinkLabel linkLabelGitHub;
	}
}
