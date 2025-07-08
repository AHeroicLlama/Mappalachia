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
			numericUpDownSpotlightRange = new NumericUpDown();
			((System.ComponentModel.ISupportInitialize)numericUpDownSpotlightRange).BeginInit();
			SuspendLayout();
			// 
			// buttonOK
			// 
			buttonOK.Location = new Point(11, 41);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new Size(75, 23);
			buttonOK.TabIndex = 1;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += ButtonOK_Click;
			// 
			// buttonCancel
			// 
			buttonCancel.Location = new Point(92, 41);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new Size(75, 23);
			buttonCancel.TabIndex = 2;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			// 
			// numericUpDownSpotlightRange
			// 
			numericUpDownSpotlightRange.Location = new Point(63, 12);
			numericUpDownSpotlightRange.Maximum = new decimal(new int[] { 8, 0, 0, 0 });
			numericUpDownSpotlightRange.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownSpotlightRange.Name = "numericUpDownSpotlightRange";
			numericUpDownSpotlightRange.Size = new Size(51, 23);
			numericUpDownSpotlightRange.TabIndex = 0;
			numericUpDownSpotlightRange.Value = new decimal(new int[] { 1, 0, 0, 0 });
			// 
			// FormSetSpotlightSize
			// 
			AcceptButton = buttonOK;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = buttonCancel;
			ClientSize = new Size(176, 76);
			Controls.Add(numericUpDownSpotlightRange);
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
			((System.ComponentModel.ISupportInitialize)numericUpDownSpotlightRange).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private Button buttonOK;
		private Button buttonCancel;
		private NumericUpDown numericUpDownSpotlightRange;
	}
}