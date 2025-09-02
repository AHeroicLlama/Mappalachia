using Library;

namespace Mappalachia
{
	public partial class FormSetSpotlightSize : GenericToolForm
	{
		public double SpotlightRange => Convert.ToDouble(numericUpDownSpotlightSize.Value);

		public FormSetSpotlightSize(Settings settings)
		{
			InitializeComponent();

			int maxSizeThisSpace = Math.Min((int)settings.Space.GetMaxSpotlightBenefit(), Common.SpotlightMaxSize);
			int cappedSpotlightSize = Math.Min(Math.Max((int)settings.MapSettings.SpotlightSize, 1), maxSizeThisSpace);

			trackBarSpotlightSize.Maximum = maxSizeThisSpace;
			numericUpDownSpotlightSize.Maximum = maxSizeThisSpace;

			numericUpDownSpotlightSize.Minimum = 1;
			trackBarSpotlightSize.Minimum = 1;

			trackBarSpotlightSize.Value = cappedSpotlightSize;
			numericUpDownSpotlightSize.Value = Convert.ToDecimal(cappedSpotlightSize);

			numericUpDownSpotlightSize.Select(0, 2);
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
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
