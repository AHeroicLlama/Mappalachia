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
			numericUpDownBrightness = new System.Windows.Forms.NumericUpDown();
			buttonBrightnessConfirm = new System.Windows.Forms.Button();
			buttonCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)numericUpDownBrightness).BeginInit();
			SuspendLayout();
			// 
			// numericUpDownBrightness
			// 
			numericUpDownBrightness.Location = new System.Drawing.Point(14, 14);
			numericUpDownBrightness.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			numericUpDownBrightness.Name = "numericUpDownBrightness";
			numericUpDownBrightness.Size = new System.Drawing.Size(88, 23);
			numericUpDownBrightness.TabIndex = 0;
			// 
			// buttonBrightnessConfirm
			// 
			buttonBrightnessConfirm.Location = new System.Drawing.Point(14, 44);
			buttonBrightnessConfirm.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			buttonBrightnessConfirm.Name = "buttonBrightnessConfirm";
			buttonBrightnessConfirm.Size = new System.Drawing.Size(88, 27);
			buttonBrightnessConfirm.TabIndex = 1;
			buttonBrightnessConfirm.Text = "OK";
			buttonBrightnessConfirm.UseVisualStyleBackColor = true;
			buttonBrightnessConfirm.Click += ButtonBrightnessConfirm_Click;
			// 
			// buttonCancel
			// 
			buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			buttonCancel.Location = new System.Drawing.Point(108, 44);
			buttonCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new System.Drawing.Size(88, 27);
			buttonCancel.TabIndex = 2;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			// 
			// FormSetBrightness
			// 
			AcceptButton = buttonBrightnessConfirm;
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			CancelButton = buttonCancel;
			ClientSize = new System.Drawing.Size(212, 83);
			Controls.Add(buttonCancel);
			Controls.Add(buttonBrightnessConfirm);
			Controls.Add(numericUpDownBrightness);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			Name = "FormSetBrightness";
			StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Set Brightness (%)";
			((System.ComponentModel.ISupportInitialize)numericUpDownBrightness).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.NumericUpDown numericUpDownBrightness;
		private System.Windows.Forms.Button buttonBrightnessConfirm;
		private System.Windows.Forms.Button buttonCancel;
	}
}