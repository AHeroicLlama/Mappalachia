namespace Mappalachia
{
	partial class FormSetBrightness
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
			numericUpDownBrightness = new NumericUpDown();
			buttonOK = new Button();
			buttonCancel = new Button();
			((System.ComponentModel.ISupportInitialize)numericUpDownBrightness).BeginInit();
			SuspendLayout();
			// 
			// numericUpDownBrightness
			// 
			numericUpDownBrightness.Location = new Point(12, 12);
			numericUpDownBrightness.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
			numericUpDownBrightness.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
			numericUpDownBrightness.Name = "numericUpDownBrightness";
			numericUpDownBrightness.Size = new Size(153, 23);
			numericUpDownBrightness.TabIndex = 0;
			numericUpDownBrightness.Value = new decimal(new int[] { 10, 0, 0, 0 });
			// 
			// buttonOK
			// 
			buttonOK.Location = new Point(10, 41);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new Size(75, 23);
			buttonOK.TabIndex = 1;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += ButtonOK_Click;
			// 
			// buttonCancel
			// 
			buttonCancel.Location = new Point(91, 41);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new Size(75, 23);
			buttonCancel.TabIndex = 2;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			// 
			// FormSetBrightness
			// 
			AcceptButton = buttonOK;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = buttonCancel;
			ClientSize = new Size(177, 74);
			Controls.Add(buttonCancel);
			Controls.Add(buttonOK);
			Controls.Add(numericUpDownBrightness);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "FormSetBrightness";
			ShowIcon = false;
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Set Brightness (%)";
			((System.ComponentModel.ISupportInitialize)numericUpDownBrightness).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private NumericUpDown numericUpDownBrightness;
		private Button buttonOK;
		private Button buttonCancel;
	}
}