using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Mappalachia.Class
{
	public partial class FormLegendTextControl : Form
	{
		public FormLegendTextControl()
		{
			InitializeComponent();

			dataGridViewLegendText.Enabled = false;
			foreach (KeyValuePair<int, string> entry in SettingsLegendText.legendTexts)
			{
				dataGridViewLegendText.Rows.Add(entry.Key, entry.Value);
			}
			dataGridViewLegendText.Enabled = true;
		}

		private void ButtonApply(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dataGridViewLegendText.Rows)
			{
				if (row.Cells["columnOverrideText"].Value.ToString() != string.Empty)
				{
					SettingsLegendText.UpdateLegend(row.Cells["columnLegendGroup"].Value.ToString(), row.Cells["columnOverrideText"].Value.ToString());
				}
			}
			Close();
		}

		private void ButtonResetAll(object sender, EventArgs e)
		{
			SettingsLegendText.ResetAll();
			Close();
		}
	}
}
