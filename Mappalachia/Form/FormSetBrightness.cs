using System;
using System.Windows.Forms;
using Mappalachia.Class;

namespace Mappalachia.Forms
{
	public partial class FormSetBrightness : Form
	{
		public FormSetBrightness()
		{
			InitializeComponent();
			numericUpDownBrightness.Value = SettingsMap.brightness;
			numericUpDownBrightness.Select(0, 3);
		}

		private void ButtonBrightnessConfirm_Click(object sender, EventArgs e)
		{
			SettingsMap.brightness = (int)numericUpDownBrightness.Value;

			Close();

			Map.DrawBaseLayer();
		}
	}
}
