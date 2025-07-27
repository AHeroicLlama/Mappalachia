namespace Mappalachia
{
	partial class FormEditPlotIcon
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
			pictureBoxIcon = new PictureBox();
			buttonOK = new Button();
			buttonCancel = new Button();
			buttonSelectIcon = new Button();
			buttonSelectColor = new Button();
			ColorDialog = new ColorDialog();
			trackBarIconSize = new TrackBar();
			labelSize = new Label();
			((System.ComponentModel.ISupportInitialize)pictureBoxIcon).BeginInit();
			((System.ComponentModel.ISupportInitialize)trackBarIconSize).BeginInit();
			SuspendLayout();
			// 
			// pictureBoxIcon
			// 
			pictureBoxIcon.Location = new Point(12, 12);
			pictureBoxIcon.Name = "pictureBoxIcon";
			pictureBoxIcon.Size = new Size(256, 256);
			pictureBoxIcon.SizeMode = PictureBoxSizeMode.CenterImage;
			pictureBoxIcon.TabIndex = 0;
			pictureBoxIcon.TabStop = false;
			// 
			// buttonOK
			// 
			buttonOK.Anchor = AnchorStyles.Bottom;
			buttonOK.Location = new Point(61, 411);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new Size(75, 23);
			buttonOK.TabIndex = 4;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += ButtonOK_Click;
			// 
			// buttonCancel
			// 
			buttonCancel.Anchor = AnchorStyles.Bottom;
			buttonCancel.Location = new Point(142, 411);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new Size(75, 23);
			buttonCancel.TabIndex = 5;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			// 
			// buttonSelectIcon
			// 
			buttonSelectIcon.Anchor = AnchorStyles.Top;
			buttonSelectIcon.Location = new Point(12, 274);
			buttonSelectIcon.Name = "buttonSelectIcon";
			buttonSelectIcon.Size = new Size(107, 23);
			buttonSelectIcon.TabIndex = 0;
			buttonSelectIcon.Text = "Select Icon";
			buttonSelectIcon.UseVisualStyleBackColor = true;
			buttonSelectIcon.Click += ButtonSelectIcon_Click;
			// 
			// buttonSelectColor
			// 
			buttonSelectColor.Anchor = AnchorStyles.Top;
			buttonSelectColor.Location = new Point(160, 274);
			buttonSelectColor.Name = "buttonSelectColor";
			buttonSelectColor.Size = new Size(107, 23);
			buttonSelectColor.TabIndex = 1;
			buttonSelectColor.Text = "Select Color";
			buttonSelectColor.UseVisualStyleBackColor = true;
			buttonSelectColor.Click += ButtonSelectColor_Click;
			// 
			// trackBarIconSize
			// 
			trackBarIconSize.Location = new Point(12, 339);
			trackBarIconSize.Maximum = 256;
			trackBarIconSize.Minimum = 16;
			trackBarIconSize.Name = "trackBarIconSize";
			trackBarIconSize.Size = new Size(255, 45);
			trackBarIconSize.TabIndex = 3;
			trackBarIconSize.TickFrequency = 8;
			trackBarIconSize.Value = 50;
			trackBarIconSize.Scroll += TrackBarIconSize_Scroll;
			// 
			// labelSize
			// 
			labelSize.AutoSize = true;
			labelSize.Location = new Point(12, 321);
			labelSize.Name = "labelSize";
			labelSize.Size = new Size(27, 15);
			labelSize.TabIndex = 2;
			labelSize.Text = "Size";
			// 
			// FormEditPlotIcon
			// 
			AcceptButton = buttonOK;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = buttonCancel;
			ClientSize = new Size(279, 446);
			Controls.Add(labelSize);
			Controls.Add(trackBarIconSize);
			Controls.Add(buttonSelectColor);
			Controls.Add(buttonSelectIcon);
			Controls.Add(buttonCancel);
			Controls.Add(buttonOK);
			Controls.Add(pictureBoxIcon);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "FormEditPlotIcon";
			ShowIcon = false;
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Edit Plot Icon";
			((System.ComponentModel.ISupportInitialize)pictureBoxIcon).EndInit();
			((System.ComponentModel.ISupportInitialize)trackBarIconSize).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private PictureBox pictureBoxIcon;
		private Button buttonOK;
		private Button buttonCancel;
		private Button buttonSelectIcon;
		private Button buttonSelectColor;
		private ColorDialog ColorDialog;
		private TrackBar trackBarIconSize;
		private Label labelSize;
	}
}