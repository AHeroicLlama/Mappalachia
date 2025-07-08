namespace Mappalachia
{
	public partial class FormSetSpotlightSize : Form
	{
		public int SpotlightRange => (int)numericUpDownSpotlightRange.Value;

		public FormSetSpotlightSize(Settings settings)
		{
			InitializeComponent();
			numericUpDownSpotlightRange.Value = settings.MapSettings.SpotlightSize;
			numericUpDownSpotlightRange.Select(0, 2);
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
