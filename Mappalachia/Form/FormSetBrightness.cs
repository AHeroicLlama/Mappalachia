namespace Mappalachia
{
	public partial class FormSetBrightness : GenericlToolForm
	{
		public float BrightnessValue => (float)Math.Round(numericUpDownBrightness.Value) / 100;

		public FormSetBrightness(Settings settings)
		{
			InitializeComponent();
			numericUpDownBrightness.Value = (int)Math.Round(settings.MapSettings.Brightness * 100);
			numericUpDownBrightness.Select(0, 3);
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
	}
}
