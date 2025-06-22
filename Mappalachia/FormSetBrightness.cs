namespace Mappalachia
{
	public partial class FormSetBrightness : Form
	{
		public float BrightnessValue => (float)numericUpDownBrightness.Value / 100;

		public FormSetBrightness(Settings settings)
		{
			InitializeComponent();
			numericUpDownBrightness.Value = (int)(settings.MapSettings.Brightness * 100);
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
