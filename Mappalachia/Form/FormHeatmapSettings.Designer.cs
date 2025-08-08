namespace Mappalachia
{
	partial class FormHeatmapSettings
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
			buttonOK = new Button();
			buttonCancel = new Button();
			trackBarRange = new TrackBar();
			labelClusterRange = new Label();
			trackBarIntensity = new TrackBar();
			labelClusterMinWeight = new Label();
			checkBoxLiveUpdate = new CheckBox();
			toolTipClusterSettings = new ToolTip(components);
			((System.ComponentModel.ISupportInitialize)trackBarRange).BeginInit();
			((System.ComponentModel.ISupportInitialize)trackBarIntensity).BeginInit();
			SuspendLayout();
			// 
			// buttonOK
			// 
			buttonOK.Anchor = AnchorStyles.Bottom;
			buttonOK.Location = new Point(135, 113);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new Size(75, 23);
			buttonOK.TabIndex = 6;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += ButtonOK_Click;
			// 
			// buttonCancel
			// 
			buttonCancel.Anchor = AnchorStyles.Bottom;
			buttonCancel.Location = new Point(216, 113);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new Size(75, 23);
			buttonCancel.TabIndex = 7;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			buttonCancel.Click += ButtonCancel_Click;
			// 
			// trackBarRange
			// 
			trackBarRange.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			trackBarRange.LargeChange = 16;
			trackBarRange.Location = new Point(102, 12);
			trackBarRange.Maximum = 512;
			trackBarRange.Minimum = 10;
			trackBarRange.Name = "trackBarRange";
			trackBarRange.Size = new Size(313, 45);
			trackBarRange.SmallChange = 4;
			trackBarRange.TabIndex = 1;
			trackBarRange.TickFrequency = 20;
			toolTipClusterSettings.SetToolTip(trackBarRange, "The range of influence in pixels that the 'heat' from each entity spreads.");
			trackBarRange.Value = 100;
			trackBarRange.ValueChanged += TrackBarRange_ValueChanged;
			// 
			// labelClusterRange
			// 
			labelClusterRange.AutoSize = true;
			labelClusterRange.Location = new Point(24, 12);
			labelClusterRange.Name = "labelClusterRange";
			labelClusterRange.Size = new Size(72, 15);
			labelClusterRange.TabIndex = 0;
			labelClusterRange.Text = "Range (XXX)";
			toolTipClusterSettings.SetToolTip(labelClusterRange, "The range of influence in pixels that the 'heat' from each entity spreads.");
			// 
			// trackBarIntensity
			// 
			trackBarIntensity.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			trackBarIntensity.Location = new Point(102, 62);
			trackBarIntensity.Maximum = 128;
			trackBarIntensity.Minimum = 4;
			trackBarIntensity.Name = "trackBarIntensity";
			trackBarIntensity.Size = new Size(313, 45);
			trackBarIntensity.SmallChange = 2;
			trackBarIntensity.TabIndex = 3;
			trackBarIntensity.TickFrequency = 5;
			toolTipClusterSettings.SetToolTip(trackBarIntensity, "The individual heat which each item applies to the map.");
			trackBarIntensity.Value = 4;
			trackBarIntensity.ValueChanged += TrackBarIntensity_ValueChanged;
			// 
			// labelClusterMinWeight
			// 
			labelClusterMinWeight.AutoSize = true;
			labelClusterMinWeight.Location = new Point(12, 62);
			labelClusterMinWeight.Name = "labelClusterMinWeight";
			labelClusterMinWeight.Size = new Size(84, 15);
			labelClusterMinWeight.TabIndex = 2;
			labelClusterMinWeight.Text = "Intensity (XXX)";
			toolTipClusterSettings.SetToolTip(labelClusterMinWeight, "The individual heat which each item applies to the map.");
			// 
			// checkBoxLiveUpdate
			// 
			checkBoxLiveUpdate.Anchor = AnchorStyles.Bottom;
			checkBoxLiveUpdate.AutoSize = true;
			checkBoxLiveUpdate.Checked = true;
			checkBoxLiveUpdate.CheckState = CheckState.Checked;
			checkBoxLiveUpdate.Location = new Point(41, 116);
			checkBoxLiveUpdate.Name = "checkBoxLiveUpdate";
			checkBoxLiveUpdate.Size = new Size(88, 19);
			checkBoxLiveUpdate.TabIndex = 5;
			checkBoxLiveUpdate.Text = "Live Update";
			toolTipClusterSettings.SetToolTip(checkBoxLiveUpdate, "Automatically re-draw the map when you move the sliders. May result in laggy behavior.");
			checkBoxLiveUpdate.UseVisualStyleBackColor = true;
			checkBoxLiveUpdate.CheckedChanged += CheckBoxLiveUpdate_CheckedChanged;
			// 
			// FormHeatmapSettings
			// 
			AcceptButton = buttonOK;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = buttonCancel;
			ClientSize = new Size(427, 148);
			Controls.Add(checkBoxLiveUpdate);
			Controls.Add(labelClusterMinWeight);
			Controls.Add(trackBarIntensity);
			Controls.Add(labelClusterRange);
			Controls.Add(trackBarRange);
			Controls.Add(buttonCancel);
			Controls.Add(buttonOK);
			Name = "FormHeatmapSettings";
			Text = "Heatmap Settings";
			((System.ComponentModel.ISupportInitialize)trackBarRange).EndInit();
			((System.ComponentModel.ISupportInitialize)trackBarIntensity).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Button buttonOK;
		private Button buttonCancel;
		private TrackBar trackBarRange;
		private Label labelClusterRange;
		private TrackBar trackBarIntensity;
		private Label labelClusterMinWeight;
		private CheckBox checkBoxLiveUpdate;
		private ToolTip toolTipClusterSettings;
	}
}