using System;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Mappalachia.Class;

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
            SettingsFileExport.setUseRecommended(checkBoxUseRecommended.Checked);
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

        // Update the values and enabled states of the form members when something relevant changes
        void UpdateFormState()
        {
            // Assign the values from the Settings Class
            checkBoxUseRecommended.Checked = SettingsFileExport.useRecommended;
            radioButtonPNG.Checked = SettingsFileExport.isPNG();
            radioButtonJPEG.Checked = SettingsFileExport.isJPEG();
            numericUpDownJPEGQuality.Value = SettingsFileExport.jpegQuality;

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
                Filter = SettingsFileExport.isPNG() ? "PNG|*.png" : "JPEG|*.jpeg",
                FileName = "Mappalachia Map",
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ImageFormat imageFormat = SettingsFileExport.isPNG() ? ImageFormat.Png : ImageFormat.Jpeg;
                IOManager.WriteToFile(dialog.FileName, Map.GetImage(), imageFormat, SettingsFileExport.jpegQuality);
                Close();
            }
        }
    }
}
