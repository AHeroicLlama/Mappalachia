namespace Mappalachia
{
	partial class FormSetSpotlightSize
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
			buttonOK = new Button();
			buttonCancel = new Button();
			trackBarSpotlightSize = new TrackBar();
			numericUpDownSpotlightSize = new NumericUpDown();
			((System.ComponentModel.ISupportInitialize)trackBarSpotlightSize).BeginInit();
			((System.ComponentModel.ISupportInitialize)numericUpDownSpotlightSize).BeginInit();
			SuspendLayout();
			// 
			// buttonOK
			// 
			buttonOK.Anchor = AnchorStyles.Bottom;
			buttonOK.Location = new Point(12, 92);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new Size(75, 23);
			buttonOK.TabIndex = 2;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += ButtonOK_Click;
			// 
			// buttonCancel
			// 
			buttonCancel.Anchor = AnchorStyles.Bottom;
			buttonCancel.Location = new Point(93, 92);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new Size(75, 23);
			buttonCancel.TabIndex = 3;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			// 
			// trackBarSpotlightSize
			// 
			trackBarSpotlightSize.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			trackBarSpotlightSize.LargeChange = 2;
			trackBarSpotlightSize.Location = new Point(12, 12);
			trackBarSpotlightSize.Maximum = 32;
			trackBarSpotlightSize.Minimum = 1;
			trackBarSpotlightSize.Name = "trackBarSpotlightSize";
			trackBarSpotlightSize.Size = new Size(158, 45);
			trackBarSpotlightSize.TabIndex = 1;
			trackBarSpotlightSize.Value = 1;
			trackBarSpotlightSize.ValueChanged += TrackBarSpotlightSize_ValueChanged;
			// 
			// numericUpDownSpotlightSize
			// 
			numericUpDownSpotlightSize.Anchor = AnchorStyles.Top;
			numericUpDownSpotlightSize.Location = new Point(69, 63);
			numericUpDownSpotlightSize.Maximum = new decimal(new int[] { 32, 0, 0, 0 });
			numericUpDownSpotlightSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownSpotlightSize.Name = "numericUpDownSpotlightSize";
			numericUpDownSpotlightSize.Size = new Size(44, 23);
			numericUpDownSpotlightSize.TabIndex = 0;
			numericUpDownSpotlightSize.Value = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownSpotlightSize.ValueChanged += NumericUpDownSpotlightSize_ValueChanged;
			// 
			// FormSetSpotlightSize
			// 
			AcceptButton = buttonOK;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = buttonCancel;
			ClientSize = new Size(182, 126);
			Controls.Add(numericUpDownSpotlightSize);
			Controls.Add(trackBarSpotlightSize);
			Controls.Add(buttonCancel);
			Controls.Add(buttonOK);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "FormSetSpotlightSize";
			ShowIcon = false;
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Set Spotlight Size";
			((System.ComponentModel.ISupportInitialize)trackBarSpotlightSize).EndInit();
			((System.ComponentModel.ISupportInitialize)numericUpDownSpotlightSize).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Button buttonOK;
		private Button buttonCancel;
		private TrackBar trackBarSpotlightSize;
		private NumericUpDown numericUpDownSpotlightSize;
	}
}