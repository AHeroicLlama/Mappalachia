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
			foreach (KeyValuePair<int, string> entry in LegendTextManager.allLegendTexts)
			{
				dataGridViewLegendText.Rows.Add(entry.Key, entry.Value);
			}
			dataGridViewLegendText.Enabled = true;
		}

		private void ButtonApply(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dataGridViewLegendText.Rows)
			{
				DataGridViewCell cell = row.Cells["columnOverrideText"];

				if (cell.Value != null && cell.Value.ToString() != string.Empty)
				{
					LegendTextManager.UpdateLegend((int)row.Cells["columnLegendGroup"].Value, cell.Value.ToString());
				}
			}
			Close();
			Map.Draw();
		}

		private void ButtonResetAll(object sender, EventArgs e)
		{
			LegendTextManager.ResetAll();
			Close();
			Map.Draw();
		}
	}
}
