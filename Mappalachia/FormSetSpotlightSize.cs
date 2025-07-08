namespace Mappalachia
{
	public partial class FormSetSpotlightSize : Form
	{
		public double SpotlightRange => Convert.ToDouble(numericUpDownSpotlightSize.Value);

		public FormSetSpotlightSize(Settings settings)
		{
			InitializeComponent();
			trackBarSpotlightSize.Value = (int)Math.Round(settings.MapSettings.SpotlightSize);
			numericUpDownSpotlightSize.Value = Convert.ToDecimal(settings.MapSettings.SpotlightSize);
			numericUpDownSpotlightSize.Select(0, 2);
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void TrackBarSpotlightSize_ValueChanged(object sender, EventArgs e)
		{
			numericUpDownSpotlightSize.Value = trackBarSpotlightSize.Value;
		}

		private void NumericUpDownSpotlightSize_ValueChanged(object sender, EventArgs e)
		{
			trackBarSpotlightSize.Value = (int)Math.Round(numericUpDownSpotlightSize.Value);
		}
	}
}
