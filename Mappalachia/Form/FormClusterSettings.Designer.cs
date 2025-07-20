namespace Mappalachia
{
	partial class FormClusterSettings
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
			trackBarClusterRange = new TrackBar();
			labelClusterRange = new Label();
			trackBarClusterMinWeight = new TrackBar();
			labelClusterMinWeight = new Label();
			checkBoxLiveUpdate = new CheckBox();
			toolTipClusterSettings = new ToolTip(components);
			((System.ComponentModel.ISupportInitialize)trackBarClusterRange).BeginInit();
			((System.ComponentModel.ISupportInitialize)trackBarClusterMinWeight).BeginInit();
			SuspendLayout();
			// 
			// buttonOK
			// 
			buttonOK.Anchor = AnchorStyles.Bottom;
			buttonOK.Location = new Point(194, 114);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new Size(75, 23);
			buttonOK.TabIndex = 5;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += ButtonOK_Click;
			// 
			// buttonCancel
			// 
			buttonCancel.Anchor = AnchorStyles.Bottom;
			buttonCancel.Location = new Point(275, 114);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new Size(75, 23);
			buttonCancel.TabIndex = 6;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			buttonCancel.Click += ButtonCancel_Click;
			// 
			// trackBarClusterRange
			// 
			trackBarClusterRange.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			trackBarClusterRange.LargeChange = 2000;
			trackBarClusterRange.Location = new Point(169, 12);
			trackBarClusterRange.Maximum = 200000;
			trackBarClusterRange.Minimum = 1;
			trackBarClusterRange.Name = "trackBarClusterRange";
			trackBarClusterRange.Size = new Size(364, 45);
			trackBarClusterRange.SmallChange = 500;
			trackBarClusterRange.TabIndex = 1;
			trackBarClusterRange.TickFrequency = 2000;
			toolTipClusterSettings.SetToolTip(trackBarClusterRange, "The maximum 'search' range in pixels that each cluster will extend.");
			trackBarClusterRange.Value = 1;
			trackBarClusterRange.ValueChanged += TrackBarClusterRange_ValueChanged;
			// 
			// labelClusterRange
			// 
			labelClusterRange.AutoSize = true;
			labelClusterRange.Location = new Point(37, 21);
			labelClusterRange.Name = "labelClusterRange";
			labelClusterRange.Size = new Size(126, 15);
			labelClusterRange.TabIndex = 0;
			labelClusterRange.Text = "Cluster Range (XXXXX)";
			toolTipClusterSettings.SetToolTip(labelClusterRange, "The maximum 'search' range in pixels that each cluster will extend.");
			// 
			// trackBarClusterMinWeight
			// 
			trackBarClusterMinWeight.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			trackBarClusterMinWeight.Location = new Point(169, 63);
			trackBarClusterMinWeight.Maximum = 200;
			trackBarClusterMinWeight.Name = "trackBarClusterMinWeight";
			trackBarClusterMinWeight.Size = new Size(364, 45);
			trackBarClusterMinWeight.TabIndex = 3;
			trackBarClusterMinWeight.TickFrequency = 5;
			toolTipClusterSettings.SetToolTip(trackBarClusterMinWeight, "The minimum weight/quantity required to draw a cluster.");
			trackBarClusterMinWeight.ValueChanged += TrackBarClusterWeight_ValueChanged;
			// 
			// labelClusterMinWeight
			// 
			labelClusterMinWeight.AutoSize = true;
			labelClusterMinWeight.Location = new Point(12, 70);
			labelClusterMinWeight.Name = "labelClusterMinWeight";
			labelClusterMinWeight.Size = new Size(151, 15);
			labelClusterMinWeight.TabIndex = 2;
			labelClusterMinWeight.Text = "Min. Cluster Weight (XXXX)";
			toolTipClusterSettings.SetToolTip(labelClusterMinWeight, "The minimum weight/quantity required to draw a cluster.");
			// 
			// checkBoxLiveUpdate
			// 
			checkBoxLiveUpdate.Anchor = AnchorStyles.Bottom;
			checkBoxLiveUpdate.AutoSize = true;
			checkBoxLiveUpdate.Checked = true;
			checkBoxLiveUpdate.CheckState = CheckState.Checked;
			checkBoxLiveUpdate.Location = new Point(100, 117);
			checkBoxLiveUpdate.Name = "checkBoxLiveUpdate";
			checkBoxLiveUpdate.Size = new Size(88, 19);
			checkBoxLiveUpdate.TabIndex = 4;
			checkBoxLiveUpdate.Text = "Live Update";
			toolTipClusterSettings.SetToolTip(checkBoxLiveUpdate, "Automatically re-draw the map when you move the sliders. May result in laggy behavior.");
			checkBoxLiveUpdate.UseVisualStyleBackColor = true;
			checkBoxLiveUpdate.CheckedChanged += CheckBoxLiveUpdate_CheckedChanged;
			// 
			// FormClusterSettings
			// 
			AcceptButton = buttonOK;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = buttonCancel;
			ClientSize = new Size(545, 149);
			Controls.Add(checkBoxLiveUpdate);
			Controls.Add(labelClusterMinWeight);
			Controls.Add(trackBarClusterMinWeight);
			Controls.Add(labelClusterRange);
			Controls.Add(trackBarClusterRange);
			Controls.Add(buttonCancel);
			Controls.Add(buttonOK);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "FormClusterSettings";
			ShowIcon = false;
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Cluster Settings";
			((System.ComponentModel.ISupportInitialize)trackBarClusterRange).EndInit();
			((System.ComponentModel.ISupportInitialize)trackBarClusterMinWeight).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Button buttonOK;
		private Button buttonCancel;
		private TrackBar trackBarClusterRange;
		private Label labelClusterRange;
		private TrackBar trackBarClusterMinWeight;
		private Label labelClusterMinWeight;
		private CheckBox checkBoxLiveUpdate;
		private ToolTip toolTipClusterSettings;
	}
}