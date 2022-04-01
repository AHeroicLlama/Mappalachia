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
            UpdateFormState();
        }

        private void RadioButtonPNG_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFormState();
        }

        // Update the values and enabled states of the form members if something relevant happens
        void UpdateFormState()
        {
            radioButtonJPEG.Enabled = !checkBoxUseRecommended.Checked;
            radioButtonPNG.Enabled = !checkBoxUseRecommended.Checked;
            numericUpDownJPEGQuality.Enabled = (!checkBoxUseRecommended.Checked && radioButtonJPEG.Checked);
            labelJPEGQualityPerc.Enabled = numericUpDownJPEGQuality.Enabled;

            if (checkBoxUseRecommended.Checked)
            {
                // Worldspace - prefer JPEG allowing for compression
                if (SettingsSpace.CurrentSpaceIsWorld())
                {
                    radioButtonJPEG.Checked = true;
                    numericUpDownJPEGQuality.Value = 85;
                }
                // Cell - perfer PNG allowing for transparency
                else
                {
                    radioButtonPNG.Checked = true;
                }
            }
        }

        // Show the save dialog, then ask IOManager to write the file
        private void ButtonOK_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = radioButtonPNG.Checked ? "PNG|*.png" : "JPEG|*.jpeg",
                FileName = "Mappalachia Map",
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ImageFormat imageFormat = radioButtonPNG.Checked ? ImageFormat.Png : ImageFormat.Jpeg;
                IOManager.WriteToFile(dialog.FileName, Map.GetImage(), imageFormat, (int)numericUpDownJPEGQuality.Value);
                Close();
            }
        }
    }
}
