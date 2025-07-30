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
			groupBoxLegendGroup = new GroupBox();
			radioButtonGroupEverything = new RadioButton();
			radioButtonPerLegendGroup = new RadioButton();
			((System.ComponentModel.ISupportInitialize)trackBarClusterRange).BeginInit();
			((System.ComponentModel.ISupportInitialize)trackBarClusterMinWeight).BeginInit();
			groupBoxLegendGroup.SuspendLayout();
			SuspendLayout();
			// 
			// buttonOK
			// 
			buttonOK.Anchor = AnchorStyles.Bottom;
			buttonOK.Location = new Point(194, 187);
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
			buttonCancel.Location = new Point(275, 187);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new Size(75, 23);
			buttonCancel.TabIndex = 7;
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
			trackBarClusterRange.Minimum = 100;
			trackBarClusterRange.Name = "trackBarClusterRange";
			trackBarClusterRange.Size = new Size(364, 45);
			trackBarClusterRange.SmallChange = 100;
			trackBarClusterRange.TabIndex = 1;
			trackBarClusterRange.TickFrequency = 5000;
			toolTipClusterSettings.SetToolTip(trackBarClusterRange, "The maximum 'search' range in pixels that each cluster will extend.");
			trackBarClusterRange.Value = 100;
			trackBarClusterRange.ValueChanged += TrackBarClusterRange_ValueChanged;
			// 
			// labelClusterRange
			// 
			labelClusterRange.AutoSize = true;
			labelClusterRange.Location = new Point(37, 21);
			labelClusterRange.Name = "labelClusterRange";
			labelClusterRange.Size = new Size(128, 15);
			labelClusterRange.TabIndex = 0;
			labelClusterRange.Text = "Cluster Radius (XXXXX)";
			toolTipClusterSettings.SetToolTip(labelClusterRange, "The maximum 'search' range in pixels that each cluster will extend.");
			labelClusterRange.MouseClick += LabelClusterRange_MouseClick;
			// 
			// trackBarClusterMinWeight
			// 
			trackBarClusterMinWeight.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			trackBarClusterMinWeight.Location = new Point(169, 63);
			trackBarClusterMinWeight.Maximum = 2000;
			trackBarClusterMinWeight.Name = "trackBarClusterMinWeight";
			trackBarClusterMinWeight.Size = new Size(364, 45);
			trackBarClusterMinWeight.TabIndex = 3;
			trackBarClusterMinWeight.TickFrequency = 50;
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
			checkBoxLiveUpdate.Location = new Point(100, 190);
			checkBoxLiveUpdate.Name = "checkBoxLiveUpdate";
			checkBoxLiveUpdate.Size = new Size(88, 19);
			checkBoxLiveUpdate.TabIndex = 5;
			checkBoxLiveUpdate.Text = "Live Update";
			toolTipClusterSettings.SetToolTip(checkBoxLiveUpdate, "Automatically re-draw the map when you move the sliders. May result in laggy behavior.");
			checkBoxLiveUpdate.UseVisualStyleBackColor = true;
			checkBoxLiveUpdate.CheckedChanged += CheckBoxLiveUpdate_CheckedChanged;
			// 
			// groupBoxLegendGroup
			// 
			groupBoxLegendGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			groupBoxLegendGroup.Controls.Add(radioButtonGroupEverything);
			groupBoxLegendGroup.Controls.Add(radioButtonPerLegendGroup);
			groupBoxLegendGroup.Location = new Point(169, 114);
			groupBoxLegendGroup.Name = "groupBoxLegendGroup";
			groupBoxLegendGroup.Size = new Size(364, 49);
			groupBoxLegendGroup.TabIndex = 4;
			groupBoxLegendGroup.TabStop = false;
			groupBoxLegendGroup.Text = "Grouping Mode";
			toolTipClusterSettings.SetToolTip(groupBoxLegendGroup, "Define how clusters are calculated.");
			// 
			// radioButtonGroupEverything
			// 
			radioButtonGroupEverything.AutoSize = true;
			radioButtonGroupEverything.Location = new Point(6, 22);
			radioButtonGroupEverything.Name = "radioButtonGroupEverything";
			radioButtonGroupEverything.Size = new Size(117, 19);
			radioButtonGroupEverything.TabIndex = 0;
			radioButtonGroupEverything.TabStop = true;
			radioButtonGroupEverything.Text = "Group Everything";
			toolTipClusterSettings.SetToolTip(radioButtonGroupEverything, "All plotted items are clustered together.");
			radioButtonGroupEverything.UseVisualStyleBackColor = true;
			radioButtonGroupEverything.CheckedChanged += RadioButtonGroupEverything_CheckedChanged;
			// 
			// radioButtonPerLegendGroup
			// 
			radioButtonPerLegendGroup.AutoSize = true;
			radioButtonPerLegendGroup.Location = new Point(129, 22);
			radioButtonPerLegendGroup.Name = "radioButtonPerLegendGroup";
			radioButtonPerLegendGroup.Size = new Size(120, 19);
			radioButtonPerLegendGroup.TabIndex = 1;
			radioButtonPerLegendGroup.TabStop = true;
			radioButtonPerLegendGroup.Text = "Per Legend Group";
			toolTipClusterSettings.SetToolTip(radioButtonPerLegendGroup, "Distinct legend groups form independent clusters.");
			radioButtonPerLegendGroup.UseVisualStyleBackColor = true;
			radioButtonPerLegendGroup.CheckedChanged += RadioButtonPerLegendGroup_CheckedChanged;
			// 
			// FormClusterSettings
			// 
			AcceptButton = buttonOK;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = buttonCancel;
			ClientSize = new Size(545, 222);
			Controls.Add(groupBoxLegendGroup);
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
			groupBoxLegendGroup.ResumeLayout(false);
			groupBoxLegendGroup.PerformLayout();
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
		private GroupBox groupBoxLegendGroup;
		private RadioButton radioButtonPerLegendGroup;
		private RadioButton radioButtonGroupEverything;
	}
}