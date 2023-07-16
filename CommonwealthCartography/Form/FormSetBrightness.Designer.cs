using System;

namespace CommonwealthCartography.Forms
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
            this.numericUpDownBrightness.Location = new System.Drawing.Point(14, 14);
            this.numericUpDownBrightness.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numericUpDownBrightness.Name = "numericUpDownBrightness";
            this.numericUpDownBrightness.Size = new System.Drawing.Size(88, 23);
            this.numericUpDownBrightness.TabIndex = 0;
            // 
            // buttonBrightnessConfirm
            // 
            this.buttonBrightnessConfirm.Location = new System.Drawing.Point(14, 44);
            this.buttonBrightnessConfirm.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonBrightnessConfirm.Name = "buttonBrightnessConfirm";
            this.buttonBrightnessConfirm.Size = new System.Drawing.Size(88, 27);
            this.buttonBrightnessConfirm.TabIndex = 1;
            this.buttonBrightnessConfirm.Text = "OK";
            this.buttonBrightnessConfirm.UseVisualStyleBackColor = true;
            this.buttonBrightnessConfirm.Click += new System.EventHandler(this.ButtonBrightnessConfirm_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(108, 44);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(88, 27);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // FormSetBrightness
            // 
            this.AcceptButton = this.buttonBrightnessConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(212, 83);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonBrightnessConfirm);
            this.Controls.Add(this.numericUpDownBrightness);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
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