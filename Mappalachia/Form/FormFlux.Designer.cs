namespace Mappalachia
{
	partial class FormOptimalNuke
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOptimalNuke));
			buttonOK = new System.Windows.Forms.Button();
			buttonCancel = new System.Windows.Forms.Button();
			toolTip = new System.Windows.Forms.ToolTip(components);
			checkBoxEnabled = new System.Windows.Forms.CheckBox();
			labelCobalt = new System.Windows.Forms.Label();
			trackBarCobalt = new System.Windows.Forms.TrackBar();
			labelFluorescent = new System.Windows.Forms.Label();
			trackBarFluorescent = new System.Windows.Forms.TrackBar();
			labelCrimson = new System.Windows.Forms.Label();
			trackBarCrimson = new System.Windows.Forms.TrackBar();
			groupBoxFluxPriority = new System.Windows.Forms.GroupBox();
			labelYellowcake = new System.Windows.Forms.Label();
			trackBarYellowcake = new System.Windows.Forms.TrackBar();
			labelViolet = new System.Windows.Forms.Label();
			trackBarViolet = new System.Windows.Forms.TrackBar();
			((System.ComponentModel.ISupportInitialize)trackBarCobalt).BeginInit();
			((System.ComponentModel.ISupportInitialize)trackBarFluorescent).BeginInit();
			((System.ComponentModel.ISupportInitialize)trackBarCrimson).BeginInit();
			groupBoxFluxPriority.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)trackBarYellowcake).BeginInit();
			((System.ComponentModel.ISupportInitialize)trackBarViolet).BeginInit();
			SuspendLayout();
			// 
			// buttonOK
			// 
			buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			buttonOK.Location = new System.Drawing.Point(97, 324);
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
			buttonCancel.Location = new System.Drawing.Point(178, 324);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new System.Drawing.Size(75, 23);
			buttonCancel.TabIndex = 9;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			// 
			// checkBoxEnabled
			// 
			checkBoxEnabled.AutoSize = true;
			checkBoxEnabled.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			checkBoxEnabled.Location = new System.Drawing.Point(138, 12);
			checkBoxEnabled.Name = "checkBoxEnabled";
			checkBoxEnabled.Size = new System.Drawing.Size(75, 25);
			checkBoxEnabled.TabIndex = 6;
			checkBoxEnabled.Text = "Enable";
			toolTip.SetToolTip(checkBoxEnabled, "Enable the optimal nuke zone for the flux color priorities below.");
			checkBoxEnabled.UseVisualStyleBackColor = true;
			// 
			// labelCobalt
			// 
			labelCobalt.AutoSize = true;
			labelCobalt.Location = new System.Drawing.Point(39, 73);
			labelCobalt.Name = "labelCobalt";
			labelCobalt.Size = new System.Drawing.Size(42, 15);
			labelCobalt.TabIndex = 2;
			labelCobalt.Text = "Cobalt";
			toolTip.SetToolTip(labelCobalt, "Adjust the priority of Cobalt Flux.");
			// 
			// trackBarCobalt
			// 
			trackBarCobalt.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			trackBarCobalt.Location = new System.Drawing.Point(87, 73);
			trackBarCobalt.Maximum = 100;
			trackBarCobalt.Name = "trackBarCobalt";
			trackBarCobalt.Size = new System.Drawing.Size(256, 45);
			trackBarCobalt.TabIndex = 3;
			trackBarCobalt.TickFrequency = 5;
			toolTip.SetToolTip(trackBarCobalt, "Adjust the priority of Cobalt Flux.");
			trackBarCobalt.Value = 100;
			// 
			// labelFluorescent
			// 
			labelFluorescent.AutoSize = true;
			labelFluorescent.Location = new System.Drawing.Point(13, 124);
			labelFluorescent.Name = "labelFluorescent";
			labelFluorescent.Size = new System.Drawing.Size(68, 15);
			labelFluorescent.TabIndex = 4;
			labelFluorescent.Text = "Fluorescent";
			toolTip.SetToolTip(labelFluorescent, "Adjust the priority of Fluorescent Flux.");
			// 
			// trackBarFluorescent
			// 
			trackBarFluorescent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			trackBarFluorescent.Location = new System.Drawing.Point(87, 124);
			trackBarFluorescent.Maximum = 100;
			trackBarFluorescent.Name = "trackBarFluorescent";
			trackBarFluorescent.Size = new System.Drawing.Size(256, 45);
			trackBarFluorescent.TabIndex = 5;
			trackBarFluorescent.TickFrequency = 5;
			toolTip.SetToolTip(trackBarFluorescent, "Adjust the priority of Fluorescent Flux.");
			trackBarFluorescent.Value = 100;
			// 
			// labelCrimson
			// 
			labelCrimson.AutoSize = true;
			labelCrimson.Location = new System.Drawing.Point(29, 22);
			labelCrimson.Name = "labelCrimson";
			labelCrimson.Size = new System.Drawing.Size(52, 15);
			labelCrimson.TabIndex = 6;
			labelCrimson.Text = "Crimson";
			toolTip.SetToolTip(labelCrimson, "Adjust the priority of Crimson Flux.");
			// 
			// trackBarCrimson
			// 
			trackBarCrimson.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			trackBarCrimson.Location = new System.Drawing.Point(87, 22);
			trackBarCrimson.Maximum = 100;
			trackBarCrimson.Name = "trackBarCrimson";
			trackBarCrimson.Size = new System.Drawing.Size(256, 45);
			trackBarCrimson.TabIndex = 7;
			trackBarCrimson.TickFrequency = 5;
			toolTip.SetToolTip(trackBarCrimson, "Adjust the priority of Crimson Flux.");
			trackBarCrimson.Value = 100;
			// 
			// groupBoxFluxPriority
			// 
			groupBoxFluxPriority.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			groupBoxFluxPriority.Controls.Add(labelViolet);
			groupBoxFluxPriority.Controls.Add(trackBarViolet);
			groupBoxFluxPriority.Controls.Add(labelYellowcake);
			groupBoxFluxPriority.Controls.Add(trackBarYellowcake);
			groupBoxFluxPriority.Controls.Add(trackBarCrimson);
			groupBoxFluxPriority.Controls.Add(labelCrimson);
			groupBoxFluxPriority.Controls.Add(labelFluorescent);
			groupBoxFluxPriority.Controls.Add(trackBarFluorescent);
			groupBoxFluxPriority.Controls.Add(labelCobalt);
			groupBoxFluxPriority.Controls.Add(trackBarCobalt);
			groupBoxFluxPriority.Location = new System.Drawing.Point(12, 43);
			groupBoxFluxPriority.Name = "groupBoxFluxPriority";
			groupBoxFluxPriority.Size = new System.Drawing.Size(349, 275);
			groupBoxFluxPriority.TabIndex = 10;
			groupBoxFluxPriority.TabStop = false;
			groupBoxFluxPriority.Text = "Flux Priorities";
			// 
			// labelYellowcake
			// 
			labelYellowcake.AutoSize = true;
			labelYellowcake.Location = new System.Drawing.Point(16, 175);
			labelYellowcake.Name = "labelYellowcake";
			labelYellowcake.Size = new System.Drawing.Size(65, 15);
			labelYellowcake.TabIndex = 8;
			labelYellowcake.Text = "Yellowcake";
			toolTip.SetToolTip(labelYellowcake, "Adjust the priority of Yellowcake Flux.");
			// 
			// trackBarYellowcake
			// 
			trackBarYellowcake.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			trackBarYellowcake.Location = new System.Drawing.Point(87, 175);
			trackBarYellowcake.Maximum = 100;
			trackBarYellowcake.Name = "trackBarYellowcake";
			trackBarYellowcake.Size = new System.Drawing.Size(256, 45);
			trackBarYellowcake.TabIndex = 9;
			trackBarYellowcake.TickFrequency = 5;
			toolTip.SetToolTip(trackBarYellowcake, "Adjust the priority of Yellowcake Flux.");
			trackBarYellowcake.Value = 100;
			// 
			// labelViolet
			// 
			labelViolet.AutoSize = true;
			labelViolet.Location = new System.Drawing.Point(44, 226);
			labelViolet.Name = "labelViolet";
			labelViolet.Size = new System.Drawing.Size(37, 15);
			labelViolet.TabIndex = 10;
			labelViolet.Text = "Violet";
			toolTip.SetToolTip(labelViolet, "Adjust the priority of Violet Flux.");
			// 
			// trackBarViolet
			// 
			trackBarViolet.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			trackBarViolet.Location = new System.Drawing.Point(87, 226);
			trackBarViolet.Maximum = 100;
			trackBarViolet.Name = "trackBarViolet";
			trackBarViolet.Size = new System.Drawing.Size(256, 45);
			trackBarViolet.TabIndex = 11;
			trackBarViolet.TickFrequency = 5;
			toolTip.SetToolTip(trackBarViolet, "Adjust the priority of Violet Flux.");
			trackBarViolet.Value = 100;
			// 
			// FormFlux
			// 
			AcceptButton = buttonOK;
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			CancelButton = buttonCancel;
			ClientSize = new System.Drawing.Size(376, 359);
			Controls.Add(groupBoxFluxPriority);
			Controls.Add(checkBoxEnabled);
			Controls.Add(buttonCancel);
			Controls.Add(buttonOK);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			Name = "FormFlux";
			StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Optimal Nuke Zone Settings";
			((System.ComponentModel.ISupportInitialize)trackBarCobalt).EndInit();
			((System.ComponentModel.ISupportInitialize)trackBarFluorescent).EndInit();
			((System.ComponentModel.ISupportInitialize)trackBarCrimson).EndInit();
			groupBoxFluxPriority.ResumeLayout(false);
			groupBoxFluxPriority.PerformLayout();
			((System.ComponentModel.ISupportInitialize)trackBarYellowcake).EndInit();
			((System.ComponentModel.ISupportInitialize)trackBarViolet).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.CheckBox checkBoxEnabled;
		private System.Windows.Forms.GroupBox groupBoxFluxPriority;
		private System.Windows.Forms.Label labelFluorescent;
		private System.Windows.Forms.TrackBar trackBarFluorescent;
		private System.Windows.Forms.TrackBar trackBarCrimson;
		private System.Windows.Forms.Label labelCrimson;
		private System.Windows.Forms.Label labelCobalt;
		private System.Windows.Forms.TrackBar trackBarCobalt;
		private System.Windows.Forms.Label labelViolet;
		private System.Windows.Forms.TrackBar trackBarViolet;
		private System.Windows.Forms.Label labelYellowcake;
		private System.Windows.Forms.TrackBar trackBarYellowcake;
	}
}