using System.Drawing.Imaging;

namespace Mappalachia
{
	public partial class FormExportToFile : GenericlToolForm
	{
		public int JpgQuality => (int)numericUpDownJPGQuality.Value;

		public ImageFormat ImageFormat => radioJPG.Checked ? ImageFormat.Jpeg : ImageFormat.Png;

		public string Filter => radioJPG.Checked ? "JPG|*.jpg|JPEG|*.jpeg" : "PNG|*.png";

		public FormExportToFile(Settings settings)
		{
			InitializeComponent();

			radioPNG.Checked = FileIO.GetImageFileTypeRecommendation(settings) == ImageFormat.Png;
			radioJPG.Checked = !radioPNG.Checked;

			numericUpDownJPGQuality.Value = 85;
			numericUpDownJPGQuality.Enabled = radioJPG.Checked;
		}

		private void Radio_CheckChanged(object sender, EventArgs e)
		{
			numericUpDownJPGQuality.Enabled = radioJPG.Checked;
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
	}
}
