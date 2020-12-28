namespace Mappalachia.Forms
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetBrightness));
			this.numericUpDownBrightness = new System.Windows.Forms.NumericUpDown();
			this.buttonBrightnessConfirm = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownBrightness)).BeginInit();
			this.SuspendLayout();
			// 
			// numericUpDownBrightness
			// 
			this.numericUpDownBrightness.Location = new System.Drawing.Point(12, 12);
			this.numericUpDownBrightness.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
			this.numericUpDownBrightness.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.numericUpDownBrightness.Name = "numericUpDownBrightness";
			this.numericUpDownBrightness.Size = new System.Drawing.Size(75, 20);
			this.numericUpDownBrightness.TabIndex = 0;
			this.numericUpDownBrightness.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
			// 
			// buttonBrightnessConfirm
			// 
			this.buttonBrightnessConfirm.Location = new System.Drawing.Point(12, 38);
			this.buttonBrightnessConfirm.Name = "buttonBrightnessConfirm";
			this.buttonBrightnessConfirm.Size = new System.Drawing.Size(75, 23);
			this.buttonBrightnessConfirm.TabIndex = 1;
			this.buttonBrightnessConfirm.Text = "OK";
			this.buttonBrightnessConfirm.UseVisualStyleBackColor = true;
			this.buttonBrightnessConfirm.Click += new System.EventHandler(this.ButtonBrightnessConfirm_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(93, 38);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// FormSetBrightness
			// 
			this.AcceptButton = this.buttonBrightnessConfirm;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(182, 72);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonBrightnessConfirm);
			this.Controls.Add(this.numericUpDownBrightness);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormSetBrightness";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Set Brightness (%)";
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownBrightness)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.NumericUpDown numericUpDownBrightness;
		private System.Windows.Forms.Button buttonBrightnessConfirm;
		private System.Windows.Forms.Button buttonCancel;
	}
}