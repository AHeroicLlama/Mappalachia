using System.Windows.Forms;

namespace CommonwealthCartography
{
	public partial class FormSetTitle : Form
	{
		public FormSetTitle()
		{
			InitializeComponent();

			textBoxTitle.Text = SettingsMap.title;
			textBoxTitle.SelectAll();
		}

		private void ButtonOK_Click(object sender, System.EventArgs e)
		{
			if (SettingsMap.title != textBoxTitle.Text)
			{
				SettingsMap.title = textBoxTitle.Text;
				FormMaster.DrawMap(true);
			}

			Close();
		}

		private void ButtonCancel_Click(object sender, System.EventArgs e)
		{
			Close();
		}
	}
}
