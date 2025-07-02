namespace Mappalachia
{
	public partial class FormSetSpotlightRange : Form
	{
		public int SpotlightRange => (int)numericUpDownSpotlightRange.Value;

		public FormSetSpotlightRange(Settings settings)
		{
			InitializeComponent();
			numericUpDownSpotlightRange.Value = settings.MapSettings.SpotlightTileRange;
			numericUpDownSpotlightRange.Select(0, 2);
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
