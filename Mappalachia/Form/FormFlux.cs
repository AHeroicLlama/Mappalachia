using System;
using System.Windows.Forms;

namespace Mappalachia
{
	public partial class FormOptimalNuke : Form
	{
		public FormOptimalNuke()
		{
			InitializeComponent();

			// Load in settings
			checkBoxEnabled.Checked = SettingsMap.drawOptimalNukeZone;

			trackBarCrimson.Value = (int)SettingsMap.fluxWeightCrimson * 100;
			trackBarCobalt.Value = (int)SettingsMap.fluxWeightCobalt * 100;
			trackBarFluorescent.Value = (int)SettingsMap.fluxWeightFluorescent * 100;
			trackBarViolet.Value = (int)SettingsMap.fluxWeightViolet * 100;
			trackBarYellowcake.Value = (int)SettingsMap.fluxWeightYellowcake * 100;
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			// Store form values to settings
			SettingsMap.drawOptimalNukeZone = checkBoxEnabled.Checked;

			SettingsMap.fluxWeightCrimson = trackBarCrimson.Value / 100;
			SettingsMap.fluxWeightCobalt = trackBarCobalt.Value / 100;
			SettingsMap.fluxWeightFluorescent = trackBarFluorescent.Value / 100;
			SettingsMap.fluxWeightViolet = trackBarViolet.Value / 100;
			SettingsMap.fluxWeightYellowcake = trackBarYellowcake.Value / 100;

			Close();

			if (SettingsMap.drawOptimalNukeZone)
			{
				FormMaster.QueueDraw(false);
			}
		}
	}
}
