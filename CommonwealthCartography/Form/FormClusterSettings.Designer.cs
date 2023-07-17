namespace CommonwealthCartography
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
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetClusterRange));
			trackBarClusterRange = new System.Windows.Forms.TrackBar();
			buttonOK = new System.Windows.Forms.Button();
			buttonCancel = new System.Windows.Forms.Button();
			checkBoxliveUpdate = new System.Windows.Forms.CheckBox();
			toolTip = new System.Windows.Forms.ToolTip(components);
			trackBarMinClusterWeight = new System.Windows.Forms.TrackBar();
			labelClusterRange = new System.Windows.Forms.Label();
			labelMinClusterWeight = new System.Windows.Forms.Label();
			checkBoxDrawClusterWeb = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)trackBarClusterRange).BeginInit();
			((System.ComponentModel.ISupportInitialize)trackBarMinClusterWeight).BeginInit();
			SuspendLayout();
			// 
			// trackBarClusterRange
			// 
			trackBarClusterRange.Location = new System.Drawing.Point(159, 12);
			trackBarClusterRange.Maximum = 2000;
			trackBarClusterRange.Minimum = 1;
			trackBarClusterRange.Name = "trackBarClusterRange";
			trackBarClusterRange.Size = new System.Drawing.Size(475, 45);
			trackBarClusterRange.TabIndex = 1;
			trackBarClusterRange.TickFrequency = 100;
			toolTip.SetToolTip(trackBarClusterRange, "Adjust the maximum size of clusters.");
			trackBarClusterRange.Value = 100;
			trackBarClusterRange.ValueChanged += TrackBarClusterRange_ValueChanged;
			// 
			// buttonOK
			// 
			buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			buttonOK.Location = new System.Drawing.Point(249, 163);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new System.Drawing.Size(75, 23);
			buttonOK.TabIndex = 8;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += ButtonOK_Click;
			// 
			// buttonCancel
			// 
			buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			buttonCancel.Location = new System.Drawing.Point(330, 163);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new System.Drawing.Size(75, 23);
			buttonCancel.TabIndex = 9;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			buttonCancel.Click += ButtonCancel_Click;
			// 
			// checkBoxliveUpdate
			// 
			checkBoxliveUpdate.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			checkBoxliveUpdate.AutoSize = true;
			checkBoxliveUpdate.Location = new System.Drawing.Point(155, 166);
			checkBoxliveUpdate.Name = "checkBoxliveUpdate";
			checkBoxliveUpdate.Size = new System.Drawing.Size(88, 19);
			checkBoxliveUpdate.TabIndex = 7;
			checkBoxliveUpdate.Text = "Live Update";
			toolTip.SetToolTip(checkBoxliveUpdate, "Constantly re-draw the map as you change cluster values.");
			checkBoxliveUpdate.UseVisualStyleBackColor = true;
			checkBoxliveUpdate.CheckedChanged += CheckBoxliveUpdate_CheckedChanged;
			// 
			// trackBarMinClusterWeight
			// 
			trackBarMinClusterWeight.Location = new System.Drawing.Point(159, 63);
			trackBarMinClusterWeight.Maximum = 200;
			trackBarMinClusterWeight.Minimum = 1;
			trackBarMinClusterWeight.Name = "trackBarMinClusterWeight";
			trackBarMinClusterWeight.Size = new System.Drawing.Size(475, 45);
			trackBarMinClusterWeight.TabIndex = 4;
			trackBarMinClusterWeight.TickFrequency = 10;
			toolTip.SetToolTip(trackBarMinClusterWeight, "Adjust the minimum weight of clusters.");
			trackBarMinClusterWeight.Value = 3;
			trackBarMinClusterWeight.ValueChanged += TrackBarMinClusterWeight_ValueChanged;
			// 
			// labelClusterRange
			// 
			labelClusterRange.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			labelClusterRange.AutoSize = true;
			labelClusterRange.Location = new System.Drawing.Point(38, 21);
			labelClusterRange.Name = "labelClusterRange";
			labelClusterRange.Size = new System.Drawing.Size(115, 15);
			labelClusterRange.TabIndex = 0;
			labelClusterRange.Text = "Cluster Range (1234)";
			toolTip.SetToolTip(labelClusterRange, "Adjust the maximum size of clusters.");
			// 
			// labelMinClusterWeight
			// 
			labelMinClusterWeight.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			labelMinClusterWeight.AutoSize = true;
			labelMinClusterWeight.Location = new System.Drawing.Point(12, 72);
			labelMinClusterWeight.Name = "labelMinClusterWeight";
			labelMinClusterWeight.Size = new System.Drawing.Size(141, 15);
			labelMinClusterWeight.TabIndex = 3;
			labelMinClusterWeight.Text = "Min. Cluster Weight (123)";
			toolTip.SetToolTip(labelMinClusterWeight, "Adjust the minimum weight of clusters.");
			// 
			// checkBoxDrawClusterWeb
			// 
			checkBoxDrawClusterWeb.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			checkBoxDrawClusterWeb.AutoSize = true;
			checkBoxDrawClusterWeb.Location = new System.Drawing.Point(33, 112);
			checkBoxDrawClusterWeb.Name = "checkBoxDrawClusterWeb";
			checkBoxDrawClusterWeb.Size = new System.Drawing.Size(120, 19);
			checkBoxDrawClusterWeb.TabIndex = 6;
			checkBoxDrawClusterWeb.Text = "Draw Cluster Web";
			toolTip.SetToolTip(checkBoxDrawClusterWeb, "Draw a 'web' identifying the cluster's member entities.");
			checkBoxDrawClusterWeb.UseVisualStyleBackColor = true;
			checkBoxDrawClusterWeb.CheckedChanged += CheckBoxDrawClusterWeb_CheckedChanged;
			// 
			// FormSetClusterRange
			// 
			AcceptButton = buttonOK;
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			CancelButton = buttonCancel;
			ClientSize = new System.Drawing.Size(646, 191);
			Controls.Add(checkBoxDrawClusterWeb);
			Controls.Add(labelMinClusterWeight);
			Controls.Add(labelClusterRange);
			Controls.Add(trackBarMinClusterWeight);
			Controls.Add(checkBoxliveUpdate);
			Controls.Add(buttonCancel);
			Controls.Add(buttonOK);
			Controls.Add(trackBarClusterRange);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			Name = "FormSetClusterRange";
			StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Cluster Settings";
			Load += FormSetClusterRange_Load;
			((System.ComponentModel.ISupportInitialize)trackBarClusterRange).EndInit();
			((System.ComponentModel.ISupportInitialize)trackBarMinClusterWeight).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private System.Windows.Forms.TrackBar trackBarClusterRange;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.CheckBox checkBoxliveUpdate;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.TrackBar trackBarMinClusterWeight;
		private System.Windows.Forms.Label labelClusterRange;
		private System.Windows.Forms.Label labelMinClusterWeight;
		private System.Windows.Forms.CheckBox checkBoxDrawClusterWeb;
	}
}