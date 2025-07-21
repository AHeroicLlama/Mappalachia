namespace Mappalachia
{
	public partial class FormSetTitle : Form
	{
		public string TextBoxValue => textBox.Text;

		public int FontSize => (int)numericUpDownFontSize.Value;

		public FormSetTitle(Settings settings)
		{
			InitializeComponent();
			textBox.Text = settings.MapSettings.Title;
			numericUpDownFontSize.Value = Math.Clamp(settings.MapSettings.TitleFontSize, numericUpDownFontSize.Minimum, numericUpDownFontSize.Maximum);
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
