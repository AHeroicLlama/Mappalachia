
namespace Mappalachia.Class
{
	partial class FormLegendTextControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLegendTextControl));
			this.dataGridViewLegendText = new System.Windows.Forms.DataGridView();
			this.columnLegendGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnOverrideText = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.buttonApply = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonResetAll = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewLegendText)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGridViewLegendText
			// 
			this.dataGridViewLegendText.AllowUserToAddRows = false;
			this.dataGridViewLegendText.AllowUserToDeleteRows = false;
			this.dataGridViewLegendText.AllowUserToResizeRows = false;
			this.dataGridViewLegendText.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridViewLegendText.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnLegendGroup,
            this.columnOverrideText});
			this.dataGridViewLegendText.Location = new System.Drawing.Point(12, 12);
			this.dataGridViewLegendText.Name = "dataGridViewLegendText";
			this.dataGridViewLegendText.RowHeadersVisible = false;
			this.dataGridViewLegendText.Size = new System.Drawing.Size(484, 285);
			this.dataGridViewLegendText.TabIndex = 0;
			// 
			// columnLegendGroup
			// 
			this.columnLegendGroup.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.columnLegendGroup.FillWeight = 15F;
			this.columnLegendGroup.HeaderText = "Legend Group";
			this.columnLegendGroup.Name = "columnLegendGroup";
			this.columnLegendGroup.ReadOnly = true;
			// 
			// columnOverrideText
			// 
			this.columnOverrideText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.columnOverrideText.HeaderText = "Overriding Legend Text";
			this.columnOverrideText.Name = "columnOverrideText";
			// 
			// buttonApply
			// 
			this.buttonApply.Location = new System.Drawing.Point(340, 303);
			this.buttonApply.Name = "buttonApply";
			this.buttonApply.Size = new System.Drawing.Size(75, 23);
			this.buttonApply.TabIndex = 1;
			this.buttonApply.Text = "Apply";
			this.toolTip1.SetToolTip(this.buttonApply, "Apply your current entries and update the map.");
			this.buttonApply.UseVisualStyleBackColor = true;
			this.buttonApply.Click += new System.EventHandler(this.ButtonApply);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(421, 303);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.toolTip1.SetToolTip(this.buttonCancel, "Undo current changes.");
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// buttonResetAll
			// 
			this.buttonResetAll.Location = new System.Drawing.Point(13, 303);
			this.buttonResetAll.Name = "buttonResetAll";
			this.buttonResetAll.Size = new System.Drawing.Size(75, 23);
			this.buttonResetAll.TabIndex = 3;
			this.buttonResetAll.Text = "Reset All";
			this.toolTip1.SetToolTip(this.buttonResetAll, "Clear all legend texts, resetting to defaults.");
			this.buttonResetAll.UseVisualStyleBackColor = true;
			this.buttonResetAll.Click += new System.EventHandler(this.ButtonResetAll);
			// 
			// FormLegendTextControl
			// 
			this.AcceptButton = this.buttonApply;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(508, 338);
			this.Controls.Add(this.buttonResetAll);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonApply);
			this.Controls.Add(this.dataGridViewLegendText);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormLegendTextControl";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Override Legend Text";
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewLegendText)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dataGridViewLegendText;
		private System.Windows.Forms.Button buttonApply;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonResetAll;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnLegendGroup;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnOverrideText;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}