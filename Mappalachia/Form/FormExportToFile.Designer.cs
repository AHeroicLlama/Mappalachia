namespace Mappalachia
{
    partial class FormExportToFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExportToFile));
            this.radioButtonJPEG = new System.Windows.Forms.RadioButton();
            this.radioButtonPNG = new System.Windows.Forms.RadioButton();
            this.checkBoxUseRecommended = new System.Windows.Forms.CheckBox();
            this.numericUpDownJPEGQuality = new System.Windows.Forms.NumericUpDown();
            this.labelJPEGQualityPerc = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownJPEGQuality)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButtonJPEG
            // 
            this.radioButtonJPEG.AutoSize = true;
            this.radioButtonJPEG.Checked = true;
            this.radioButtonJPEG.Location = new System.Drawing.Point(12, 58);
            this.radioButtonJPEG.Name = "radioButtonJPEG";
            this.radioButtonJPEG.Size = new System.Drawing.Size(52, 17);
            this.radioButtonJPEG.TabIndex = 2;
            this.radioButtonJPEG.TabStop = true;
            this.radioButtonJPEG.Text = "JPEG";
            this.toolTip1.SetToolTip(this.radioButtonJPEG, "Variable quality, good file size. No transparency. Suitable for sharing online.");
            this.radioButtonJPEG.UseVisualStyleBackColor = true;
            // 
            // radioButtonPNG
            // 
            this.radioButtonPNG.AutoSize = true;
            this.radioButtonPNG.Location = new System.Drawing.Point(12, 35);
            this.radioButtonPNG.Name = "radioButtonPNG";
            this.radioButtonPNG.Size = new System.Drawing.Size(48, 17);
            this.radioButtonPNG.TabIndex = 1;
            this.radioButtonPNG.Text = "PNG";
            this.toolTip1.SetToolTip(this.radioButtonPNG, "High quality, large file size. Supports transparency. Suitable for use in other m" +
        "edia.");
            this.radioButtonPNG.UseVisualStyleBackColor = true;
            this.radioButtonPNG.CheckedChanged += new System.EventHandler(this.RadioButtonPNG_CheckedChanged);
            // 
            // checkBoxUseRecommended
            // 
            this.checkBoxUseRecommended.AutoSize = true;
            this.checkBoxUseRecommended.Checked = true;
            this.checkBoxUseRecommended.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUseRecommended.Location = new System.Drawing.Point(12, 12);
            this.checkBoxUseRecommended.Name = "checkBoxUseRecommended";
            this.checkBoxUseRecommended.Size = new System.Drawing.Size(154, 17);
            this.checkBoxUseRecommended.TabIndex = 0;
            this.checkBoxUseRecommended.Text = "Use recommended settings";
            this.toolTip1.SetToolTip(this.checkBoxUseRecommended, "Let Mappalachia decide settings, based on your currently selected space.");
            this.checkBoxUseRecommended.UseVisualStyleBackColor = true;
            this.checkBoxUseRecommended.CheckedChanged += new System.EventHandler(this.CheckBoxUseRecommended_CheckedChanged);
            // 
            // numericUpDownJPEGQuality
            // 
            this.numericUpDownJPEGQuality.Location = new System.Drawing.Point(65, 58);
            this.numericUpDownJPEGQuality.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownJPEGQuality.Name = "numericUpDownJPEGQuality";
            this.numericUpDownJPEGQuality.Size = new System.Drawing.Size(43, 20);
            this.numericUpDownJPEGQuality.TabIndex = 3;
            this.toolTip1.SetToolTip(this.numericUpDownJPEGQuality, "The quality of the image in JPEG. Higher values provide better quality but a larg" +
        "er file size.");
            this.numericUpDownJPEGQuality.Value = new decimal(new int[] {
            85,
            0,
            0,
            0});
            // 
            // labelJPEGQualityPerc
            // 
            this.labelJPEGQualityPerc.AutoSize = true;
            this.labelJPEGQualityPerc.Location = new System.Drawing.Point(114, 60);
            this.labelJPEGQualityPerc.Name = "labelJPEGQualityPerc";
            this.labelJPEGQualityPerc.Size = new System.Drawing.Size(47, 13);
            this.labelJPEGQualityPerc.TabIndex = 4;
            this.labelJPEGQualityPerc.Text = "Quality%";
            this.toolTip1.SetToolTip(this.labelJPEGQualityPerc, "The quality of the image in JPEG. Higher values provide better quality but a larg" +
        "er file size.");
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(12, 91);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            this.toolTip1.SetToolTip(this.buttonOK, "Save the file with these settings.");
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(95, 91);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // FormExportToFile
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(182, 126);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.labelJPEGQualityPerc);
            this.Controls.Add(this.numericUpDownJPEGQuality);
            this.Controls.Add(this.checkBoxUseRecommended);
            this.Controls.Add(this.radioButtonPNG);
            this.Controls.Add(this.radioButtonJPEG);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormExportToFile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export to Image File";
            this.Load += new System.EventHandler(this.FormExportToFile_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownJPEGQuality)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonJPEG;
        private System.Windows.Forms.RadioButton radioButtonPNG;
        private System.Windows.Forms.CheckBox checkBoxUseRecommended;
        private System.Windows.Forms.NumericUpDown numericUpDownJPEGQuality;
        private System.Windows.Forms.Label labelJPEGQualityPerc;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}