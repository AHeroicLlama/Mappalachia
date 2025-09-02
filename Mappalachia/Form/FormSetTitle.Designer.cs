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
			labelTitle = new Label();
			SuspendLayout();
			// 
			// textBox
			// 
			textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			textBox.Location = new Point(12, 25);
			textBox.MaxLength = 256;
			textBox.Name = "textBox";
			textBox.Size = new Size(310, 23);
			textBox.TabIndex = 0;
			// 
			// buttonOK
			// 
			buttonOK.Anchor = AnchorStyles.Bottom;
			buttonOK.Location = new Point(89, 60);
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
			buttonCancel.Location = new Point(170, 60);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new Size(75, 23);
			buttonCancel.TabIndex = 2;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			// 
			// labelTitle
			// 
			labelTitle.AutoSize = true;
			labelTitle.Location = new Point(12, 7);
			labelTitle.Name = "labelTitle";
			labelTitle.Size = new Size(30, 15);
			labelTitle.TabIndex = 3;
			labelTitle.Text = "Title";
			// 
			// FormSetTitle
			// 
			AcceptButton = buttonOK;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = buttonCancel;
			ClientSize = new Size(334, 88);
			Controls.Add(labelTitle);
			Controls.Add(buttonCancel);
			Controls.Add(buttonOK);
			Controls.Add(textBox);
			Name = "FormSetTitle";
			Text = "Set Map Title";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private TextBox textBox;
		private Button buttonOK;
		private Button buttonCancel;
		private Label labelTitle;
	}
}