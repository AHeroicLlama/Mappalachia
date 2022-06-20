namespace Mappalachia
{
	partial class FormSetClusterRange
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetClusterRange));
			this.textBoxClusterRange = new System.Windows.Forms.TextBox();
			this.trackBarClusterRange = new System.Windows.Forms.TrackBar();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.checkBoxliveUpdate = new System.Windows.Forms.CheckBox();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.trackBarClusterRange)).BeginInit();
			this.SuspendLayout();
			// 
			// textBoxClusterRange
			// 
			this.textBoxClusterRange.Location = new System.Drawing.Point(559, 12);
			this.textBoxClusterRange.Name = "textBoxClusterRange";
			this.textBoxClusterRange.Size = new System.Drawing.Size(84, 23);
			this.textBoxClusterRange.TabIndex = 1;
			this.textBoxClusterRange.TextChanged += new System.EventHandler(this.textBoxClusterRange_TextChanged);
			this.textBoxClusterRange.Leave += new System.EventHandler(this.textBoxClusterRange_ExitFocus);
			// 
			// trackBarClusterRange
			// 
			this.trackBarClusterRange.LargeChange = 20;
			this.trackBarClusterRange.Location = new System.Drawing.Point(12, 12);
			this.trackBarClusterRange.Maximum = 150;
			this.trackBarClusterRange.Minimum = 50;
			this.trackBarClusterRange.Name = "trackBarClusterRange";
			this.trackBarClusterRange.Size = new System.Drawing.Size(541, 45);
			this.trackBarClusterRange.SmallChange = 5;
			this.trackBarClusterRange.TabIndex = 0;
			this.trackBarClusterRange.Value = 100;
			this.trackBarClusterRange.Scroll += new System.EventHandler(this.trackBarClusterRange_Scroll);
			this.trackBarClusterRange.KeyUp += new System.Windows.Forms.KeyEventHandler(this.trackBarClusterRange_KeyUp);
			this.trackBarClusterRange.Leave += new System.EventHandler(this.trackBarClusterRange_Leave);
			this.trackBarClusterRange.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBarClusterRange_MouseUp);
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(249, 63);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 3;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(330, 63);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 4;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// checkBoxliveUpdate
			// 
			this.checkBoxliveUpdate.AutoSize = true;
			this.checkBoxliveUpdate.Location = new System.Drawing.Point(18, 60);
			this.checkBoxliveUpdate.Name = "checkBoxliveUpdate";
			this.checkBoxliveUpdate.Size = new System.Drawing.Size(88, 19);
			this.checkBoxliveUpdate.TabIndex = 2;
			this.checkBoxliveUpdate.Text = "Live Update";
			this.toolTip.SetToolTip(this.checkBoxliveUpdate, "Constantly re-draw the map as you change the range.");
			this.checkBoxliveUpdate.UseVisualStyleBackColor = true;
			this.checkBoxliveUpdate.CheckedChanged += new System.EventHandler(this.checkBoxliveUpdate_CheckedChanged);
			// 
			// FormSetClusterRange
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(655, 91);
			this.Controls.Add(this.checkBoxliveUpdate);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.textBoxClusterRange);
			this.Controls.Add(this.trackBarClusterRange);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormSetClusterRange";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Set Cluster Range";
			((System.ComponentModel.ISupportInitialize)(this.trackBarClusterRange)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBoxClusterRange;
		private System.Windows.Forms.TrackBar trackBarClusterRange;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.CheckBox checkBoxliveUpdate;
		private System.Windows.Forms.ToolTip toolTip;
	}
}