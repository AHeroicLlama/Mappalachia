namespace CommonwealthCartography
{
	partial class FormPlotStyleSettings
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPlotStyleSettings));
			checkBoxCrosshairOuter = new System.Windows.Forms.CheckBox();
			checkBoxCircle = new System.Windows.Forms.CheckBox();
			checkBoxCrosshairInner = new System.Windows.Forms.CheckBox();
			checkBoxSquare = new System.Windows.Forms.CheckBox();
			checkBoxDiamond = new System.Windows.Forms.CheckBox();
			buttonApply = new System.Windows.Forms.Button();
			buttonCancel = new System.Windows.Forms.Button();
			groupBoxColorPalette = new System.Windows.Forms.GroupBox();
			buttonRemoveColor = new System.Windows.Forms.Button();
			labelLoadPalette = new System.Windows.Forms.Label();
			comboBoxPalette = new System.Windows.Forms.ComboBox();
			buttonAddColor = new System.Windows.Forms.Button();
			listViewColorPalette = new System.Windows.Forms.ListView();
			labelOpacityShadow = new System.Windows.Forms.Label();
			labelOpacity = new System.Windows.Forms.Label();
			trackBarIconWidth = new System.Windows.Forms.TrackBar();
			labelIconWidth = new System.Windows.Forms.Label();
			trackBarIconSize = new System.Windows.Forms.TrackBar();
			labelIconSize = new System.Windows.Forms.Label();
			toolTipControls = new System.Windows.Forms.ToolTip(components);
			listViewShapePalette = new System.Windows.Forms.ListView();
			trackBarIconOpacity = new System.Windows.Forms.TrackBar();
			trackBarShadowOpacity = new System.Windows.Forms.TrackBar();
			buttonRemoveShape = new System.Windows.Forms.Button();
			buttonAddShape = new System.Windows.Forms.Button();
			buttonReset = new System.Windows.Forms.Button();
			checkBoxFill = new System.Windows.Forms.CheckBox();
			checkBoxMarker = new System.Windows.Forms.CheckBox();
			checkBoxFrame = new System.Windows.Forms.CheckBox();
			colorDialogPalette = new System.Windows.Forms.ColorDialog();
			groupBoxIconSettings = new System.Windows.Forms.GroupBox();
			groupBoxIconPalette = new System.Windows.Forms.GroupBox();
			groupBoxColorPalette.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)trackBarIconWidth).BeginInit();
			((System.ComponentModel.ISupportInitialize)trackBarIconSize).BeginInit();
			((System.ComponentModel.ISupportInitialize)trackBarIconOpacity).BeginInit();
			((System.ComponentModel.ISupportInitialize)trackBarShadowOpacity).BeginInit();
			groupBoxIconSettings.SuspendLayout();
			groupBoxIconPalette.SuspendLayout();
			SuspendLayout();
			// 
			// checkBoxCrosshairOuter
			// 
			checkBoxCrosshairOuter.AutoSize = true;
			checkBoxCrosshairOuter.Location = new System.Drawing.Point(93, 114);
			checkBoxCrosshairOuter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			checkBoxCrosshairOuter.Name = "checkBoxCrosshairOuter";
			checkBoxCrosshairOuter.Size = new System.Drawing.Size(108, 19);
			checkBoxCrosshairOuter.TabIndex = 5;
			checkBoxCrosshairOuter.Text = "Outer Crosshair";
			toolTipControls.SetToolTip(checkBoxCrosshairOuter, "Add a crosshair extruding from the plot icon.");
			checkBoxCrosshairOuter.UseVisualStyleBackColor = true;
			// 
			// checkBoxCircle
			// 
			checkBoxCircle.AutoSize = true;
			checkBoxCircle.Location = new System.Drawing.Point(7, 139);
			checkBoxCircle.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			checkBoxCircle.Name = "checkBoxCircle";
			checkBoxCircle.Size = new System.Drawing.Size(56, 19);
			checkBoxCircle.TabIndex = 3;
			checkBoxCircle.Text = "Circle";
			toolTipControls.SetToolTip(checkBoxCircle, "Add a circle shape to the plot icon.");
			checkBoxCircle.UseVisualStyleBackColor = true;
			// 
			// checkBoxCrosshairInner
			// 
			checkBoxCrosshairInner.AutoSize = true;
			checkBoxCrosshairInner.Location = new System.Drawing.Point(93, 89);
			checkBoxCrosshairInner.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			checkBoxCrosshairInner.Name = "checkBoxCrosshairInner";
			checkBoxCrosshairInner.Size = new System.Drawing.Size(105, 19);
			checkBoxCrosshairInner.TabIndex = 4;
			checkBoxCrosshairInner.Text = "Inner Crosshair";
			toolTipControls.SetToolTip(checkBoxCrosshairInner, "Add a crosshair within the plot icon.");
			checkBoxCrosshairInner.UseVisualStyleBackColor = true;
			// 
			// checkBoxSquare
			// 
			checkBoxSquare.AutoSize = true;
			checkBoxSquare.Location = new System.Drawing.Point(7, 114);
			checkBoxSquare.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			checkBoxSquare.Name = "checkBoxSquare";
			checkBoxSquare.Size = new System.Drawing.Size(62, 19);
			checkBoxSquare.TabIndex = 2;
			checkBoxSquare.Text = "Square";
			toolTipControls.SetToolTip(checkBoxSquare, "Add a square shape to the plot icon.");
			checkBoxSquare.UseVisualStyleBackColor = true;
			// 
			// checkBoxDiamond
			// 
			checkBoxDiamond.AutoSize = true;
			checkBoxDiamond.Location = new System.Drawing.Point(7, 89);
			checkBoxDiamond.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			checkBoxDiamond.Name = "checkBoxDiamond";
			checkBoxDiamond.Size = new System.Drawing.Size(75, 19);
			checkBoxDiamond.TabIndex = 1;
			checkBoxDiamond.Text = "Diamond";
			toolTipControls.SetToolTip(checkBoxDiamond, "Add a diamond shape to the plot icon.");
			checkBoxDiamond.UseVisualStyleBackColor = true;
			// 
			// buttonApply
			// 
			buttonApply.Location = new System.Drawing.Point(312, 396);
			buttonApply.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			buttonApply.Name = "buttonApply";
			buttonApply.Size = new System.Drawing.Size(88, 27);
			buttonApply.TabIndex = 3;
			buttonApply.Text = "Apply";
			toolTipControls.SetToolTip(buttonApply, "Confirm and apply these settings.");
			buttonApply.UseVisualStyleBackColor = true;
			buttonApply.Click += ButtonApply_Click;
			// 
			// buttonCancel
			// 
			buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			buttonCancel.Location = new System.Drawing.Point(406, 396);
			buttonCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new System.Drawing.Size(88, 27);
			buttonCancel.TabIndex = 4;
			buttonCancel.Text = "Cancel";
			toolTipControls.SetToolTip(buttonCancel, "Discard any changes.");
			buttonCancel.UseVisualStyleBackColor = true;
			// 
			// groupBoxColorPalette
			// 
			groupBoxColorPalette.Controls.Add(buttonRemoveColor);
			groupBoxColorPalette.Controls.Add(labelLoadPalette);
			groupBoxColorPalette.Controls.Add(comboBoxPalette);
			groupBoxColorPalette.Controls.Add(buttonAddColor);
			groupBoxColorPalette.Controls.Add(listViewColorPalette);
			groupBoxColorPalette.Location = new System.Drawing.Point(14, 163);
			groupBoxColorPalette.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			groupBoxColorPalette.Name = "groupBoxColorPalette";
			groupBoxColorPalette.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
			groupBoxColorPalette.Size = new System.Drawing.Size(244, 226);
			groupBoxColorPalette.TabIndex = 1;
			groupBoxColorPalette.TabStop = false;
			groupBoxColorPalette.Text = "Color Palette";
			toolTipControls.SetToolTip(groupBoxColorPalette, "The collection of colors which will be used to plot different items.");
			// 
			// buttonRemoveColor
			// 
			buttonRemoveColor.Location = new System.Drawing.Point(113, 189);
			buttonRemoveColor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			buttonRemoveColor.Name = "buttonRemoveColor";
			buttonRemoveColor.Size = new System.Drawing.Size(122, 27);
			buttonRemoveColor.TabIndex = 3;
			buttonRemoveColor.Text = "Remove Selected";
			toolTipControls.SetToolTip(buttonRemoveColor, "Remove the selected color(s) from the palette.");
			buttonRemoveColor.UseVisualStyleBackColor = true;
			buttonRemoveColor.Click += ButtonRemoveColor_Click;
			// 
			// labelLoadPalette
			// 
			labelLoadPalette.AutoSize = true;
			labelLoadPalette.Location = new System.Drawing.Point(7, 18);
			labelLoadPalette.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			labelLoadPalette.Name = "labelLoadPalette";
			labelLoadPalette.Size = new System.Drawing.Size(68, 15);
			labelLoadPalette.TabIndex = 3;
			labelLoadPalette.Text = "Load preset";
			toolTipControls.SetToolTip(labelLoadPalette, "Choose a premade color palette.");
			// 
			// comboBoxPalette
			// 
			comboBoxPalette.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			comboBoxPalette.FormattingEnabled = true;
			comboBoxPalette.Items.AddRange(new object[] { "Default", "Colorblind (IBM)", "Colorblind (Wong)", "Colorblind (Tol)" });
			comboBoxPalette.Location = new System.Drawing.Point(7, 37);
			comboBoxPalette.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			comboBoxPalette.Name = "comboBoxPalette";
			comboBoxPalette.Size = new System.Drawing.Size(229, 23);
			comboBoxPalette.TabIndex = 0;
			toolTipControls.SetToolTip(comboBoxPalette, "Choose a premade color palette.");
			comboBoxPalette.SelectedIndexChanged += ComboBoxColorPalette_SelectedIndexChanged;
			// 
			// buttonAddColor
			// 
			buttonAddColor.Location = new System.Drawing.Point(7, 189);
			buttonAddColor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			buttonAddColor.Name = "buttonAddColor";
			buttonAddColor.Size = new System.Drawing.Size(99, 27);
			buttonAddColor.TabIndex = 2;
			buttonAddColor.Text = "Add Color";
			toolTipControls.SetToolTip(buttonAddColor, "Choose a new color to add to the palette.");
			buttonAddColor.UseVisualStyleBackColor = true;
			buttonAddColor.Click += ButtonAddColor_Click;
			// 
			// listViewColorPalette
			// 
			listViewColorPalette.LabelWrap = false;
			listViewColorPalette.Location = new System.Drawing.Point(7, 68);
			listViewColorPalette.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			listViewColorPalette.Name = "listViewColorPalette";
			listViewColorPalette.Size = new System.Drawing.Size(229, 114);
			listViewColorPalette.TabIndex = 1;
			toolTipControls.SetToolTip(listViewColorPalette, "The collection of colors which will be used to plot different items.");
			listViewColorPalette.UseCompatibleStateImageBehavior = false;
			listViewColorPalette.View = System.Windows.Forms.View.SmallIcon;
			// 
			// labelOpacityShadow
			// 
			labelOpacityShadow.AutoSize = true;
			labelOpacityShadow.Location = new System.Drawing.Point(245, 81);
			labelOpacityShadow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			labelOpacityShadow.Name = "labelOpacityShadow";
			labelOpacityShadow.RightToLeft = System.Windows.Forms.RightToLeft.No;
			labelOpacityShadow.Size = new System.Drawing.Size(93, 15);
			labelOpacityShadow.TabIndex = 6;
			labelOpacityShadow.Text = "Shadow Opacity";
			toolTipControls.SetToolTip(labelOpacityShadow, "The opacity (apparent darkness) of a cast shadow.");
			// 
			// labelOpacity
			// 
			labelOpacity.AutoSize = true;
			labelOpacity.Location = new System.Drawing.Point(266, 22);
			labelOpacity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			labelOpacity.Name = "labelOpacity";
			labelOpacity.RightToLeft = System.Windows.Forms.RightToLeft.No;
			labelOpacity.Size = new System.Drawing.Size(74, 15);
			labelOpacity.TabIndex = 4;
			labelOpacity.Text = "Icon Opacity";
			toolTipControls.SetToolTip(labelOpacity, "The opacity of the icon.");
			// 
			// trackBarIconWidth
			// 
			trackBarIconWidth.LargeChange = 1;
			trackBarIconWidth.Location = new System.Drawing.Point(83, 81);
			trackBarIconWidth.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			trackBarIconWidth.Name = "trackBarIconWidth";
			trackBarIconWidth.Size = new System.Drawing.Size(121, 45);
			trackBarIconWidth.TabIndex = 1;
			toolTipControls.SetToolTip(trackBarIconWidth, "The width of the lines (and shadows) which form the plot icon.");
			trackBarIconWidth.Value = 1;
			// 
			// labelIconWidth
			// 
			labelIconWidth.AutoSize = true;
			labelIconWidth.Location = new System.Drawing.Point(7, 81);
			labelIconWidth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			labelIconWidth.Name = "labelIconWidth";
			labelIconWidth.RightToLeft = System.Windows.Forms.RightToLeft.No;
			labelIconWidth.Size = new System.Drawing.Size(65, 15);
			labelIconWidth.TabIndex = 10;
			labelIconWidth.Text = "Icon Width";
			toolTipControls.SetToolTip(labelIconWidth, "The width of the lines (and shadows) which form the plot icon.");
			// 
			// trackBarIconSize
			// 
			trackBarIconSize.LargeChange = 3;
			trackBarIconSize.Location = new System.Drawing.Point(83, 22);
			trackBarIconSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			trackBarIconSize.Name = "trackBarIconSize";
			trackBarIconSize.Size = new System.Drawing.Size(121, 45);
			trackBarIconSize.TabIndex = 0;
			toolTipControls.SetToolTip(trackBarIconSize, "The maximum diameter of the plot icon.");
			trackBarIconSize.Value = 1;
			// 
			// labelIconSize
			// 
			labelIconSize.AutoSize = true;
			labelIconSize.Location = new System.Drawing.Point(16, 22);
			labelIconSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			labelIconSize.Name = "labelIconSize";
			labelIconSize.Size = new System.Drawing.Size(53, 15);
			labelIconSize.TabIndex = 10;
			labelIconSize.Text = "Icon Size";
			toolTipControls.SetToolTip(labelIconSize, "The maximum diameter of the plot icon.");
			// 
			// listViewShapePalette
			// 
			listViewShapePalette.LabelWrap = false;
			listViewShapePalette.Location = new System.Drawing.Point(7, 22);
			listViewShapePalette.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			listViewShapePalette.MultiSelect = false;
			listViewShapePalette.Name = "listViewShapePalette";
			listViewShapePalette.Size = new System.Drawing.Size(214, 61);
			listViewShapePalette.TabIndex = 0;
			toolTipControls.SetToolTip(listViewShapePalette, "The collection of shapes which will be used to plot different items.");
			listViewShapePalette.UseCompatibleStateImageBehavior = false;
			listViewShapePalette.View = System.Windows.Forms.View.SmallIcon;
			listViewShapePalette.SelectedIndexChanged += ListViewShapePalette_SelectedIndexChanged;
			// 
			// trackBarIconOpacity
			// 
			trackBarIconOpacity.LargeChange = 3;
			trackBarIconOpacity.Location = new System.Drawing.Point(351, 22);
			trackBarIconOpacity.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			trackBarIconOpacity.Name = "trackBarIconOpacity";
			trackBarIconOpacity.Size = new System.Drawing.Size(121, 45);
			trackBarIconOpacity.TabIndex = 2;
			toolTipControls.SetToolTip(trackBarIconOpacity, "The opacity of the icon.");
			trackBarIconOpacity.Value = 1;
			// 
			// trackBarShadowOpacity
			// 
			trackBarShadowOpacity.LargeChange = 3;
			trackBarShadowOpacity.Location = new System.Drawing.Point(351, 81);
			trackBarShadowOpacity.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			trackBarShadowOpacity.Name = "trackBarShadowOpacity";
			trackBarShadowOpacity.Size = new System.Drawing.Size(121, 45);
			trackBarShadowOpacity.TabIndex = 5;
			toolTipControls.SetToolTip(trackBarShadowOpacity, "The opacity (apparent darkness) of a cast shadow.");
			// 
			// buttonRemoveShape
			// 
			buttonRemoveShape.Location = new System.Drawing.Point(94, 189);
			buttonRemoveShape.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			buttonRemoveShape.Name = "buttonRemoveShape";
			buttonRemoveShape.Size = new System.Drawing.Size(119, 27);
			buttonRemoveShape.TabIndex = 7;
			buttonRemoveShape.Text = "Remove Selected";
			toolTipControls.SetToolTip(buttonRemoveShape, "Remove the currently selected shape from the palette.");
			buttonRemoveShape.UseVisualStyleBackColor = true;
			buttonRemoveShape.Click += ButtonRemoveShape_Click;
			// 
			// buttonAddShape
			// 
			buttonAddShape.Location = new System.Drawing.Point(7, 189);
			buttonAddShape.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			buttonAddShape.Name = "buttonAddShape";
			buttonAddShape.Size = new System.Drawing.Size(80, 27);
			buttonAddShape.TabIndex = 6;
			buttonAddShape.Text = "Add Shape";
			toolTipControls.SetToolTip(buttonAddShape, "Add a new shape to the palette.");
			buttonAddShape.UseVisualStyleBackColor = true;
			buttonAddShape.Click += ButtonAddShape_Click;
			// 
			// buttonReset
			// 
			buttonReset.Location = new System.Drawing.Point(14, 396);
			buttonReset.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			buttonReset.Name = "buttonReset";
			buttonReset.Size = new System.Drawing.Size(113, 27);
			buttonReset.TabIndex = 5;
			buttonReset.Text = "Reset to Default";
			toolTipControls.SetToolTip(buttonReset, "Reset all plot settings on this form to default values.");
			buttonReset.UseVisualStyleBackColor = true;
			buttonReset.Click += ButtonReset_Click;
			// 
			// checkBoxFill
			// 
			checkBoxFill.AutoSize = true;
			checkBoxFill.Location = new System.Drawing.Point(93, 164);
			checkBoxFill.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			checkBoxFill.Name = "checkBoxFill";
			checkBoxFill.Size = new System.Drawing.Size(76, 19);
			checkBoxFill.TabIndex = 8;
			checkBoxFill.Text = "Fill Shape";
			toolTipControls.SetToolTip(checkBoxFill, "Fill hollow shapes with solid color.");
			checkBoxFill.UseVisualStyleBackColor = true;
			// 
			// checkBoxMarker
			// 
			checkBoxMarker.AutoSize = true;
			checkBoxMarker.Location = new System.Drawing.Point(93, 139);
			checkBoxMarker.Name = "checkBoxMarker";
			checkBoxMarker.Size = new System.Drawing.Size(63, 19);
			checkBoxMarker.TabIndex = 10;
			checkBoxMarker.Text = "Marker";
			toolTipControls.SetToolTip(checkBoxMarker, "An isosceles triangle pointing to the item.");
			checkBoxMarker.UseVisualStyleBackColor = true;
			// 
			// checkBoxFrame
			// 
			checkBoxFrame.AutoSize = true;
			checkBoxFrame.Location = new System.Drawing.Point(7, 164);
			checkBoxFrame.Name = "checkBoxFrame";
			checkBoxFrame.Size = new System.Drawing.Size(59, 19);
			checkBoxFrame.TabIndex = 9;
			checkBoxFrame.Text = "Frame";
			toolTipControls.SetToolTip(checkBoxFrame, "A corner-only frame.");
			checkBoxFrame.UseVisualStyleBackColor = true;
			// 
			// groupBoxIconSettings
			// 
			groupBoxIconSettings.Controls.Add(trackBarShadowOpacity);
			groupBoxIconSettings.Controls.Add(labelOpacityShadow);
			groupBoxIconSettings.Controls.Add(trackBarIconOpacity);
			groupBoxIconSettings.Controls.Add(labelIconWidth);
			groupBoxIconSettings.Controls.Add(trackBarIconWidth);
			groupBoxIconSettings.Controls.Add(labelOpacity);
			groupBoxIconSettings.Controls.Add(trackBarIconSize);
			groupBoxIconSettings.Controls.Add(labelIconSize);
			groupBoxIconSettings.Location = new System.Drawing.Point(14, 14);
			groupBoxIconSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			groupBoxIconSettings.Name = "groupBoxIconSettings";
			groupBoxIconSettings.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
			groupBoxIconSettings.Size = new System.Drawing.Size(479, 142);
			groupBoxIconSettings.TabIndex = 0;
			groupBoxIconSettings.TabStop = false;
			groupBoxIconSettings.Text = "Icon Settings";
			// 
			// groupBoxIconPalette
			// 
			groupBoxIconPalette.Controls.Add(checkBoxMarker);
			groupBoxIconPalette.Controls.Add(checkBoxFrame);
			groupBoxIconPalette.Controls.Add(checkBoxFill);
			groupBoxIconPalette.Controls.Add(listViewShapePalette);
			groupBoxIconPalette.Controls.Add(checkBoxCrosshairOuter);
			groupBoxIconPalette.Controls.Add(buttonRemoveShape);
			groupBoxIconPalette.Controls.Add(buttonAddShape);
			groupBoxIconPalette.Controls.Add(checkBoxCircle);
			groupBoxIconPalette.Controls.Add(checkBoxDiamond);
			groupBoxIconPalette.Controls.Add(checkBoxCrosshairInner);
			groupBoxIconPalette.Controls.Add(checkBoxSquare);
			groupBoxIconPalette.Location = new System.Drawing.Point(265, 163);
			groupBoxIconPalette.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			groupBoxIconPalette.Name = "groupBoxIconPalette";
			groupBoxIconPalette.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
			groupBoxIconPalette.Size = new System.Drawing.Size(229, 226);
			groupBoxIconPalette.TabIndex = 2;
			groupBoxIconPalette.TabStop = false;
			groupBoxIconPalette.Text = "Icon Shape Palette";
			// 
			// FormPlotStyleSettings
			// 
			AcceptButton = buttonApply;
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			CancelButton = buttonCancel;
			ClientSize = new System.Drawing.Size(506, 435);
			Controls.Add(groupBoxIconSettings);
			Controls.Add(buttonReset);
			Controls.Add(groupBoxColorPalette);
			Controls.Add(buttonCancel);
			Controls.Add(groupBoxIconPalette);
			Controls.Add(buttonApply);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "FormPlotStyleSettings";
			StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Plot Style Settings";
			groupBoxColorPalette.ResumeLayout(false);
			groupBoxColorPalette.PerformLayout();
			((System.ComponentModel.ISupportInitialize)trackBarIconWidth).EndInit();
			((System.ComponentModel.ISupportInitialize)trackBarIconSize).EndInit();
			((System.ComponentModel.ISupportInitialize)trackBarIconOpacity).EndInit();
			((System.ComponentModel.ISupportInitialize)trackBarShadowOpacity).EndInit();
			groupBoxIconSettings.ResumeLayout(false);
			groupBoxIconSettings.PerformLayout();
			groupBoxIconPalette.ResumeLayout(false);
			groupBoxIconPalette.PerformLayout();
			ResumeLayout(false);
		}

		#endregion
		private System.Windows.Forms.CheckBox checkBoxCircle;
		private System.Windows.Forms.CheckBox checkBoxSquare;
		private System.Windows.Forms.CheckBox checkBoxDiamond;
		private System.Windows.Forms.CheckBox checkBoxCrosshairOuter;
		private System.Windows.Forms.CheckBox checkBoxCrosshairInner;
		private System.Windows.Forms.Button buttonApply;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label labelOpacity;
		private System.Windows.Forms.GroupBox groupBoxColorPalette;
		private System.Windows.Forms.Label labelOpacityShadow;
		private System.Windows.Forms.TrackBar trackBarIconWidth;
		private System.Windows.Forms.Label labelIconWidth;
		private System.Windows.Forms.TrackBar trackBarIconSize;
		private System.Windows.Forms.Label labelIconSize;
		private System.Windows.Forms.ToolTip toolTipControls;
		private System.Windows.Forms.ColorDialog colorDialogPalette;
		private System.Windows.Forms.Button buttonAddColor;
		private System.Windows.Forms.GroupBox groupBoxIconSettings;
		private System.Windows.Forms.Label labelLoadPalette;
		private System.Windows.Forms.ComboBox comboBoxPalette;
		private System.Windows.Forms.Button buttonRemoveColor;
		private System.Windows.Forms.GroupBox groupBoxIconPalette;
		private System.Windows.Forms.Button buttonRemoveShape;
		private System.Windows.Forms.Button buttonAddShape;
		private System.Windows.Forms.ListView listViewColorPalette;
		private System.Windows.Forms.ListView listViewShapePalette;
		private System.Windows.Forms.TrackBar trackBarIconOpacity;
		private System.Windows.Forms.TrackBar trackBarShadowOpacity;
		private System.Windows.Forms.Button buttonReset;
		private System.Windows.Forms.CheckBox checkBoxFill;
		private System.Windows.Forms.CheckBox checkBoxMarker;
		private System.Windows.Forms.CheckBox checkBoxFrame;
	}
}