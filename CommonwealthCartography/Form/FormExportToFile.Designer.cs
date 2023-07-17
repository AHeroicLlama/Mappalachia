namespace CommonwealthCartography
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExportToFile));
			radioButtonJPEG = new System.Windows.Forms.RadioButton();
			radioButtonPNG = new System.Windows.Forms.RadioButton();
			checkBoxUseRecommended = new System.Windows.Forms.CheckBox();
			numericUpDownJPEGQuality = new System.Windows.Forms.NumericUpDown();
			labelJPEGQualityPerc = new System.Windows.Forms.Label();
			buttonOK = new System.Windows.Forms.Button();
			buttonCancel = new System.Windows.Forms.Button();
			toolTipControls = new System.Windows.Forms.ToolTip(components);
			checkBoxShowDirectory = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)numericUpDownJPEGQuality).BeginInit();
			SuspendLayout();
			// 
			// radioButtonJPEG
			// 
			radioButtonJPEG.AutoSize = true;
			radioButtonJPEG.Checked = true;
			radioButtonJPEG.Location = new System.Drawing.Point(14, 67);
			radioButtonJPEG.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			radioButtonJPEG.Name = "radioButtonJPEG";
			radioButtonJPEG.Size = new System.Drawing.Size(50, 19);
			radioButtonJPEG.TabIndex = 2;
			radioButtonJPEG.TabStop = true;
			radioButtonJPEG.Text = "JPEG";
			toolTipControls.SetToolTip(radioButtonJPEG, "Variable quality, good file size. No transparency. Suitable for sharing online.");
			radioButtonJPEG.UseVisualStyleBackColor = true;
			// 
			// radioButtonPNG
			// 
			radioButtonPNG.AutoSize = true;
			radioButtonPNG.Location = new System.Drawing.Point(14, 40);
			radioButtonPNG.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			radioButtonPNG.Name = "radioButtonPNG";
			radioButtonPNG.Size = new System.Drawing.Size(49, 19);
			radioButtonPNG.TabIndex = 1;
			radioButtonPNG.Text = "PNG";
			toolTipControls.SetToolTip(radioButtonPNG, "High quality, large file size. Supports transparency. Suitable for use in other media.");
			radioButtonPNG.UseVisualStyleBackColor = true;
			radioButtonPNG.CheckedChanged += RadioButtonPNG_CheckedChanged;
			// 
			// checkBoxUseRecommended
			// 
			checkBoxUseRecommended.AutoSize = true;
			checkBoxUseRecommended.Checked = true;
			checkBoxUseRecommended.CheckState = System.Windows.Forms.CheckState.Checked;
			checkBoxUseRecommended.Location = new System.Drawing.Point(14, 14);
			checkBoxUseRecommended.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			checkBoxUseRecommended.Name = "checkBoxUseRecommended";
			checkBoxUseRecommended.Size = new System.Drawing.Size(170, 19);
			checkBoxUseRecommended.TabIndex = 0;
			checkBoxUseRecommended.Text = "Use recommended settings";
			toolTipControls.SetToolTip(checkBoxUseRecommended, "Let Commonwealth Cartography decide settings, based on your current map.");
			checkBoxUseRecommended.UseVisualStyleBackColor = true;
			checkBoxUseRecommended.CheckedChanged += CheckBoxUseRecommended_CheckedChanged;
			// 
			// numericUpDownJPEGQuality
			// 
			numericUpDownJPEGQuality.Location = new System.Drawing.Point(76, 67);
			numericUpDownJPEGQuality.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			numericUpDownJPEGQuality.Minimum = new decimal(new int[] { 20, 0, 0, 0 });
			numericUpDownJPEGQuality.Name = "numericUpDownJPEGQuality";
			numericUpDownJPEGQuality.Size = new System.Drawing.Size(50, 23);
			numericUpDownJPEGQuality.TabIndex = 3;
			toolTipControls.SetToolTip(numericUpDownJPEGQuality, "The quality of the image in JPEG. Higher values provide better quality but a larger file size.");
			numericUpDownJPEGQuality.Value = new decimal(new int[] { 85, 0, 0, 0 });
			numericUpDownJPEGQuality.ValueChanged += NumericUpDownJPEGQuality_ValueChanged;
			// 
			// labelJPEGQualityPerc
			// 
			labelJPEGQualityPerc.AutoSize = true;
			labelJPEGQualityPerc.Location = new System.Drawing.Point(133, 69);
			labelJPEGQualityPerc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			labelJPEGQualityPerc.Name = "labelJPEGQualityPerc";
			labelJPEGQualityPerc.Size = new System.Drawing.Size(55, 15);
			labelJPEGQualityPerc.TabIndex = 4;
			labelJPEGQualityPerc.Text = "Quality%";
			toolTipControls.SetToolTip(labelJPEGQualityPerc, "The quality of the image in JPEG. Higher values provide better quality but a larger file size.");
			// 
			// buttonOK
			// 
			buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			buttonOK.Location = new System.Drawing.Point(14, 139);
			buttonOK.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new System.Drawing.Size(88, 27);
			buttonOK.TabIndex = 5;
			buttonOK.Text = "OK";
			toolTipControls.SetToolTip(buttonOK, "Save the file with these settings.");
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += ButtonOK_Click;
			// 
			// buttonCancel
			// 
			buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			buttonCancel.Location = new System.Drawing.Point(111, 139);
			buttonCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new System.Drawing.Size(88, 27);
			buttonCancel.TabIndex = 6;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			// 
			// checkBoxShowDirectory
			// 
			checkBoxShowDirectory.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			checkBoxShowDirectory.AutoSize = true;
			checkBoxShowDirectory.Location = new System.Drawing.Point(14, 114);
			checkBoxShowDirectory.Name = "checkBoxShowDirectory";
			checkBoxShowDirectory.Size = new System.Drawing.Size(145, 19);
			checkBoxShowDirectory.TabIndex = 7;
			checkBoxShowDirectory.Text = "Show in directory after";
			toolTipControls.SetToolTip(checkBoxShowDirectory, "Brings up the file explorer to the saved image, once saved.");
			checkBoxShowDirectory.UseVisualStyleBackColor = true;
			checkBoxShowDirectory.CheckedChanged += CheckBoxShowDirectory_CheckedChanged;
			// 
			// FormExportToFile
			// 
			AcceptButton = buttonOK;
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			CancelButton = buttonCancel;
			ClientSize = new System.Drawing.Size(212, 178);
			Controls.Add(checkBoxShowDirectory);
			Controls.Add(buttonCancel);
			Controls.Add(buttonOK);
			Controls.Add(labelJPEGQualityPerc);
			Controls.Add(numericUpDownJPEGQuality);
			Controls.Add(checkBoxUseRecommended);
			Controls.Add(radioButtonPNG);
			Controls.Add(radioButtonJPEG);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			Name = "FormExportToFile";
			StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Export to Image File";
			Load += FormExportToFile_Load;
			((System.ComponentModel.ISupportInitialize)numericUpDownJPEGQuality).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.RadioButton radioButtonJPEG;
		private System.Windows.Forms.RadioButton radioButtonPNG;
		private System.Windows.Forms.CheckBox checkBoxUseRecommended;
		private System.Windows.Forms.NumericUpDown numericUpDownJPEGQuality;
		private System.Windows.Forms.Label labelJPEGQualityPerc;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.ToolTip toolTipControls;
		private System.Windows.Forms.CheckBox checkBoxShowDirectory;
	}
}