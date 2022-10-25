using System;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Mappalachia
{
	public partial class FormExportToFile : Form
	{
		public FormExportToFile()
		{
			InitializeComponent();
		}

		private void FormExportToFile_Load(object sender, EventArgs e)
		{
			UpdateFormState();
		}

		private void CheckBoxUseRecommended_CheckedChanged(object sender, EventArgs e)
		{
			SettingsFileExport.SetUseRecommended(checkBoxUseRecommended.Checked);
			UpdateFormState();
		}

		private void RadioButtonPNG_CheckedChanged(object sender, EventArgs e)
		{
			SettingsFileExport.fileType = radioButtonPNG.Checked ? SettingsFileExport.FileType.PNG : SettingsFileExport.FileType.JPEG;
			UpdateFormState();
		}

		private void NumericUpDownJPEGQuality_ValueChanged(object sender, EventArgs e)
		{
			SettingsFileExport.jpegQuality = (int)numericUpDownJPEGQuality.Value;
			UpdateFormState();
		}

		private void CheckBoxShowDirectory_CheckedChanged(object sender, EventArgs e)
		{
			SettingsFileExport.openExplorer = checkBoxShowDirectory.Checked;
		}

		// Update the values and enabled states of the form members when something relevant changes
		void UpdateFormState()
		{
			// Assign the values from the Settings Class
			checkBoxUseRecommended.Checked = SettingsFileExport.useRecommended;
			radioButtonPNG.Checked = SettingsFileExport.IsPNG();
			radioButtonJPEG.Checked = SettingsFileExport.IsJPEG();
			numericUpDownJPEGQuality.Value = SettingsFileExport.jpegQuality;
			checkBoxShowDirectory.Checked = SettingsFileExport.openExplorer;

			// Update the form enabled states accordingly
			radioButtonJPEG.Enabled = !checkBoxUseRecommended.Checked;
			radioButtonPNG.Enabled = !checkBoxUseRecommended.Checked;
			numericUpDownJPEGQuality.Enabled = !checkBoxUseRecommended.Checked && radioButtonJPEG.Checked;
			labelJPEGQualityPerc.Enabled = numericUpDownJPEGQuality.Enabled;
		}

		// Show the save dialog, then ask IOManager to write the file
		private void ButtonOK_Click(object sender, EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog
			{
				Filter = SettingsFileExport.IsPNG() ? "PNG|*.png" : "JPEG|*.jpeg",
				FileName = "Mappalachia Map",
			};

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				string fileName = dialog.FileName;
				ImageFormat imageFormat = SettingsFileExport.IsPNG() ? ImageFormat.Png : ImageFormat.Jpeg;
				IOManager.WriteToFile(fileName, Map.GetImage(), imageFormat, SettingsFileExport.jpegQuality);

				if (SettingsFileExport.openExplorer)
				{
					IOManager.SelectFile(fileName);
				}

				Close();
			}
		}
	}
}
