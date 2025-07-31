namespace Mappalachia
{
	partial class FormPlotIconSettings
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
			listViewStandardPalette = new ListView();
			labelStandardPalette = new Label();
			buttonAddColorStandard = new Button();
			buttonRemoveSelectedStandard = new Button();
			buttonRemoveSelectedTopograph = new Button();
			buttonAddColorTopograph = new Button();
			labelTopographPalette = new Label();
			listViewTopographPalette = new ListView();
			colorDialog = new ColorDialog();
			buttonResetAll = new Button();
			toolTip = new ToolTip(components);
			trackBarIconSize = new TrackBar();
			labelIconSize = new Label();
			((System.ComponentModel.ISupportInitialize)trackBarIconSize).BeginInit();
			SuspendLayout();
			// 
			// buttonOK
			// 
			buttonOK.Anchor = AnchorStyles.Bottom;
			buttonOK.Location = new Point(166, 307);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new Size(75, 23);
			buttonOK.TabIndex = 0;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += ButtonOK_Click;
			// 
			// buttonCancel
			// 
			buttonCancel.Anchor = AnchorStyles.Bottom;
			buttonCancel.Location = new Point(245, 307);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new Size(75, 23);
			buttonCancel.TabIndex = 1;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			// 
			// listViewStandardPalette
			// 
			listViewStandardPalette.Location = new Point(12, 27);
			listViewStandardPalette.Name = "listViewStandardPalette";
			listViewStandardPalette.Size = new Size(227, 133);
			listViewStandardPalette.TabIndex = 2;
			listViewStandardPalette.UseCompatibleStateImageBehavior = false;
			// 
			// labelStandardPalette
			// 
			labelStandardPalette.AutoSize = true;
			labelStandardPalette.Location = new Point(12, 9);
			labelStandardPalette.Name = "labelStandardPalette";
			labelStandardPalette.Size = new Size(134, 15);
			labelStandardPalette.TabIndex = 3;
			labelStandardPalette.Text = "Default Standard Palette";
			// 
			// buttonAddColorStandard
			// 
			buttonAddColorStandard.Location = new Point(12, 166);
			buttonAddColorStandard.Name = "buttonAddColorStandard";
			buttonAddColorStandard.Size = new Size(111, 23);
			buttonAddColorStandard.TabIndex = 4;
			buttonAddColorStandard.Text = "Add Color";
			buttonAddColorStandard.UseVisualStyleBackColor = true;
			buttonAddColorStandard.Click += ButtonAddColorStandard_Click;
			// 
			// buttonRemoveSelectedStandard
			// 
			buttonRemoveSelectedStandard.Location = new Point(128, 166);
			buttonRemoveSelectedStandard.Name = "buttonRemoveSelectedStandard";
			buttonRemoveSelectedStandard.Size = new Size(111, 23);
			buttonRemoveSelectedStandard.TabIndex = 5;
			buttonRemoveSelectedStandard.Text = "Remove Selected";
			buttonRemoveSelectedStandard.UseVisualStyleBackColor = true;
			buttonRemoveSelectedStandard.Click += ButtonRemoveSelectedStandard_Click;
			// 
			// buttonRemoveSelectedTopograph
			// 
			buttonRemoveSelectedTopograph.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			buttonRemoveSelectedTopograph.Location = new Point(360, 166);
			buttonRemoveSelectedTopograph.Name = "buttonRemoveSelectedTopograph";
			buttonRemoveSelectedTopograph.Size = new Size(114, 23);
			buttonRemoveSelectedTopograph.TabIndex = 9;
			buttonRemoveSelectedTopograph.Text = "Remove Selected";
			buttonRemoveSelectedTopograph.UseVisualStyleBackColor = true;
			buttonRemoveSelectedTopograph.Click += ButtonRemoveSelectedTopograph_Click;
			// 
			// buttonAddColorTopograph
			// 
			buttonAddColorTopograph.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			buttonAddColorTopograph.Location = new Point(245, 166);
			buttonAddColorTopograph.Name = "buttonAddColorTopograph";
			buttonAddColorTopograph.Size = new Size(111, 23);
			buttonAddColorTopograph.TabIndex = 8;
			buttonAddColorTopograph.Text = "Add Color";
			buttonAddColorTopograph.UseVisualStyleBackColor = true;
			buttonAddColorTopograph.Click += ButtonAddColorTopograph_Click;
			// 
			// labelTopographPalette
			// 
			labelTopographPalette.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			labelTopographPalette.AutoSize = true;
			labelTopographPalette.Location = new Point(243, 9);
			labelTopographPalette.Name = "labelTopographPalette";
			labelTopographPalette.Size = new Size(113, 15);
			labelTopographPalette.TabIndex = 7;
			labelTopographPalette.Text = "Topographic Palette";
			// 
			// listViewTopographPalette
			// 
			listViewTopographPalette.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			listViewTopographPalette.Location = new Point(247, 27);
			listViewTopographPalette.Name = "listViewTopographPalette";
			listViewTopographPalette.Size = new Size(227, 133);
			listViewTopographPalette.TabIndex = 6;
			listViewTopographPalette.UseCompatibleStateImageBehavior = false;
			// 
			// buttonResetAll
			// 
			buttonResetAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			buttonResetAll.Location = new Point(12, 307);
			buttonResetAll.Name = "buttonResetAll";
			buttonResetAll.Size = new Size(75, 23);
			buttonResetAll.TabIndex = 10;
			buttonResetAll.Text = "Reset All";
			toolTip.SetToolTip(buttonResetAll, "Reset all settings on this form.");
			buttonResetAll.UseVisualStyleBackColor = true;
			buttonResetAll.Click += ButtonResetAll_Click;
			// 
			// trackBarIconSize
			// 
			trackBarIconSize.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			trackBarIconSize.Location = new Point(12, 237);
			trackBarIconSize.Maximum = 256;
			trackBarIconSize.Minimum = 16;
			trackBarIconSize.Name = "trackBarIconSize";
			trackBarIconSize.Size = new Size(462, 45);
			trackBarIconSize.TabIndex = 11;
			trackBarIconSize.TickFrequency = 8;
			trackBarIconSize.Value = 16;
			trackBarIconSize.Scroll += TrackBarIconSize_Scroll;
			// 
			// labelIconSize
			// 
			labelIconSize.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			labelIconSize.AutoSize = true;
			labelIconSize.Location = new Point(12, 219);
			labelIconSize.Name = "labelIconSize";
			labelIconSize.Size = new Size(94, 15);
			labelIconSize.TabIndex = 12;
			labelIconSize.Text = "Default Icon Size";
			// 
			// FormPlotIconSettings
			// 
			AcceptButton = buttonOK;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = buttonCancel;
			ClientSize = new Size(486, 342);
			Controls.Add(labelIconSize);
			Controls.Add(trackBarIconSize);
			Controls.Add(buttonResetAll);
			Controls.Add(buttonRemoveSelectedTopograph);
			Controls.Add(buttonAddColorTopograph);
			Controls.Add(labelTopographPalette);
			Controls.Add(listViewTopographPalette);
			Controls.Add(buttonRemoveSelectedStandard);
			Controls.Add(buttonAddColorStandard);
			Controls.Add(labelStandardPalette);
			Controls.Add(listViewStandardPalette);
			Controls.Add(buttonCancel);
			Controls.Add(buttonOK);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "FormPlotIconSettings";
			ShowIcon = false;
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Plot Icon Settings";
			((System.ComponentModel.ISupportInitialize)trackBarIconSize).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Button buttonOK;
		private Button buttonCancel;
		private ListView listViewStandardPalette;
		private Label labelStandardPalette;
		private Button buttonAddColorStandard;
		private Button buttonRemoveSelectedStandard;
		private Button buttonRemoveSelectedTopograph;
		private Button buttonAddColorTopograph;
		private Label labelTopographPalette;
		private ListView listViewTopographPalette;
		private ColorDialog colorDialog;
		private Button buttonResetAll;
		private ToolTip toolTip;
		private TrackBar trackBarIconSize;
		private Label labelIconSize;
	}
}