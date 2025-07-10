namespace Mappalachia
{
	partial class FormSetTitle
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
			textBox = new TextBox();
			buttonOK = new Button();
			buttonCancel = new Button();
			SuspendLayout();
			// 
			// textBox
			// 
			textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			textBox.Location = new Point(12, 12);
			textBox.MaxLength = 256;
			textBox.Name = "textBox";
			textBox.Size = new Size(300, 23);
			textBox.TabIndex = 0;
			// 
			// buttonOK
			// 
			buttonOK.Anchor = AnchorStyles.Bottom;
			buttonOK.Location = new Point(84, 41);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new Size(75, 23);
			buttonOK.TabIndex = 1;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += ButtonOK_Click;
			// 
			// buttonCancel
			// 
			buttonCancel.Anchor = AnchorStyles.Bottom;
			buttonCancel.Location = new Point(165, 41);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new Size(75, 23);
			buttonCancel.TabIndex = 2;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			// 
			// FormSetTitle
			// 
			AcceptButton = buttonOK;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = buttonCancel;
			ClientSize = new Size(324, 69);
			Controls.Add(buttonCancel);
			Controls.Add(buttonOK);
			Controls.Add(textBox);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "FormSetTitle";
			ShowIcon = false;
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Set Map Title";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private TextBox textBox;
		private Button buttonOK;
		private Button buttonCancel;
	}
}