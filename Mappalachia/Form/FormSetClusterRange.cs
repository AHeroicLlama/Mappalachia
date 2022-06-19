using System;
using System.Windows.Forms;

namespace Mappalachia
{
	public partial class FormSetClusterRange : Form
	{
		static int initialValue;
		static bool usedLiveUpdate;

		public FormSetClusterRange()
		{
			InitializeComponent();

			trackBarClusterRange.Minimum = SettingsPlotCluster.minRange;
			trackBarClusterRange.Maximum = SettingsPlotCluster.maxRange;
			trackBarClusterRange.Value = SettingsPlotCluster.clusterRange;

			textBoxClusterRange.Text = trackBarClusterRange.Value.ToString();

			checkBoxliveUpdate.Enabled = SettingsPlot.IsCluster();
			checkBoxliveUpdate.Checked = SettingsPlotCluster.liveUpdate;

			initialValue = SettingsPlotCluster.clusterRange;
			usedLiveUpdate = false;
		}

		private void trackBarClusterRange_Scroll(object sender, EventArgs e)
		{
			textBoxClusterRange.Text = trackBarClusterRange.Value.ToString();
		}

		private void textBoxClusterRange_ExitFocus(object sender, EventArgs e)
		{
			string text = textBoxClusterRange.Text;

			if (text == string.Empty)
			{
				text = SettingsPlotCluster.defaultClusterRange.ToString();
			}

			int value = int.Parse(text);
			value = Math.Max(Math.Min(trackBarClusterRange.Maximum, value), trackBarClusterRange.Minimum);

			trackBarClusterRange.Value = value;
			textBoxClusterRange.Text = value.ToString();

			LiveUpdate();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			SettingsPlotCluster.clusterRange = trackBarClusterRange.Value;

			Close();

			if (SettingsPlot.IsCluster())
			{
				FormMaster.DrawMap(false);
			}
		}

		private void trackBarClusterRange_Leave(object sender, EventArgs e)
		{
			LiveUpdate();
		}

		private void trackBarClusterRange_KeyUp(object sender, KeyEventArgs e)
		{
			LiveUpdate();
		}

		private void trackBarClusterRange_MouseUp(object sender, MouseEventArgs e)
		{
			LiveUpdate();
		}

		void LiveUpdate()
		{
			if (checkBoxliveUpdate.Checked && SettingsPlot.IsCluster())
			{
				SettingsPlotCluster.clusterRange = trackBarClusterRange.Value;
				FormMaster.DrawMap(false);
				usedLiveUpdate = true;
			}
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			// If they used live update but now wish to cancel, we need to ensure we reset to the initial value
			if (usedLiveUpdate)
			{
				SettingsPlotCluster.clusterRange = initialValue;
				FormMaster.DrawMap(false);
			}
		}

		private void textBoxClusterRange_TextChanged(object sender, EventArgs e)
		{
			bool changedText = false;

			int initialCaretIndex = textBoxClusterRange.SelectionStart;

			string text = string.Empty;

			foreach (char c in textBoxClusterRange.Text)
			{
				if (char.IsDigit(c))
				{
					text += c;
				}
				else
				{
					changedText = true;
				}
			}

			if (changedText)
			{
				textBoxClusterRange.Text = text;
				textBoxClusterRange.SelectionStart = Math.Max(0, Math.Min(initialCaretIndex - 1, textBoxClusterRange.Text.Length - 1));
			}
		}

		private void checkBoxliveUpdate_CheckedChanged(object sender, EventArgs e)
		{
			SettingsPlotCluster.liveUpdate = checkBoxliveUpdate.Checked;
		}
	}
}
