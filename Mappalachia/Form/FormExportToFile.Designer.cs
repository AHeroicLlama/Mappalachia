namespace Mappalachia
{
	partial class FormExportToFile
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
			components = new System.ComponentModel.Container();
			radioPNG = new RadioButton();
			radioJPG = new RadioButton();
			numericUpDownJPGQuality = new NumericUpDown();
			labelFileType = new Label();
			labelJPGQuality = new Label();
			buttonOK = new Button();
			toolTipExportToFile = new ToolTip(components);
			buttonCancel = new Button();
			((System.ComponentModel.ISupportInitialize)numericUpDownJPGQuality).BeginInit();
			SuspendLayout();
			// 
			// radioPNG
			// 
			radioPNG.AutoSize = true;
			radioPNG.Location = new Point(12, 29);
			radioPNG.Name = "radioPNG";
			radioPNG.Size = new Size(49, 19);
			radioPNG.TabIndex = 1;
			radioPNG.TabStop = true;
			radioPNG.Text = "PNG";
			toolTipExportToFile.SetToolTip(radioPNG, "Supports transparency, large file size.");
			radioPNG.UseVisualStyleBackColor = true;
			radioPNG.CheckedChanged += Radio_CheckChanged;
			// 
			// radioJPG
			// 
			radioJPG.AutoSize = true;
			radioJPG.Location = new Point(12, 54);
			radioJPG.Name = "radioJPG";
			radioJPG.Size = new Size(44, 19);
			radioJPG.TabIndex = 2;
			radioJPG.TabStop = true;
			radioJPG.Text = "JPG";
			toolTipExportToFile.SetToolTip(radioJPG, "No transparency, variable file size.");
			radioJPG.UseVisualStyleBackColor = true;
			radioJPG.CheckedChanged += Radio_CheckChanged;
			// 
			// numericUpDownJPGQuality
			// 
			numericUpDownJPGQuality.Location = new Point(12, 104);
			numericUpDownJPGQuality.Minimum = new decimal(new int[] { 20, 0, 0, 0 });
			numericUpDownJPGQuality.Name = "numericUpDownJPGQuality";
			numericUpDownJPGQuality.Size = new Size(67, 23);
			numericUpDownJPGQuality.TabIndex = 4;
			numericUpDownJPGQuality.Value = new decimal(new int[] { 20, 0, 0, 0 });
			// 
			// labelFileType
			// 
			labelFileType.AutoSize = true;
			labelFileType.Location = new Point(12, 9);
			labelFileType.Name = "labelFileType";
			labelFileType.Size = new Size(53, 15);
			labelFileType.TabIndex = 0;
			labelFileType.Text = "File Type";
			// 
			// labelJPGQuality
			// 
			labelJPGQuality.AutoSize = true;
			labelJPGQuality.Location = new Point(8, 86);
			labelJPGQuality.Name = "labelJPGQuality";
			labelJPGQuality.Size = new Size(88, 15);
			labelJPGQuality.TabIndex = 3;
			labelJPGQuality.Text = "JPG Quality (%)";
			// 
			// buttonOK
			// 
			buttonOK.Anchor = AnchorStyles.Bottom;
			buttonOK.Location = new Point(12, 138);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new Size(75, 23);
			buttonOK.TabIndex = 5;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += ButtonOK_Click;
			// 
			// buttonCancel
			// 
			buttonCancel.Anchor = AnchorStyles.Bottom;
			buttonCancel.Location = new Point(95, 138);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new Size(75, 23);
			buttonCancel.TabIndex = 6;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			// 
			// FormExportToFile
			// 
			AcceptButton = buttonOK;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = buttonCancel;
			ClientSize = new Size(182, 173);
			Controls.Add(buttonCancel);
			Controls.Add(buttonOK);
			Controls.Add(labelJPGQuality);
			Controls.Add(labelFileType);
			Controls.Add(radioPNG);
			Controls.Add(radioJPG);
			Controls.Add(numericUpDownJPGQuality);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "FormExportToFile";
			ShowIcon = false;
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Export To File";
			((System.ComponentModel.ISupportInitialize)numericUpDownJPGQuality).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private RadioButton radioPNG;
		private RadioButton radioJPG;
		private NumericUpDown numericUpDownJPGQuality;
		private Label labelFileType;
		private Label labelJPGQuality;
		private ToolTip toolTipExportToFile;
		private Button buttonOK;
		private Button buttonCancel;
	}
}