namespace Mappalachia
{
	public partial class FormSetTitle : Form
	{
		public string TextBoxValue => textBox.Text.Trim();

		public FormSetTitle(Settings settings)
		{
			InitializeComponent();
			textBox.Text = settings.MapSettings.Title;
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
	}
}
