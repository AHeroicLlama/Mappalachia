using System;
using System.Windows.Forms;

namespace Mappalachia
{
	public partial class FormSetClusterRange : Form
	{
		readonly int initialRangeValue;
		readonly int initialWeightCapValue;
		readonly bool initialWebValue;
		bool usedLiveUpdate;

		public FormSetClusterRange()
		{
			InitializeComponent();

			trackBarClusterRange.Minimum = SettingsPlotCluster.minRange;
			trackBarClusterRange.Maximum = SettingsPlotCluster.maxRange;
			trackBarClusterRange.Value = SettingsPlotCluster.clusterRange;

			trackBarMinClusterWeight.Minimum = SettingsPlotCluster.minWeightCap;
			trackBarMinClusterWeight.Maximum = SettingsPlotCluster.maxWeightCap;
			trackBarMinClusterWeight.Value = SettingsPlotCluster.minClusterWeight;

			checkBoxDrawClusterWeb.Checked = SettingsPlotCluster.clusterWeb;

			checkBoxliveUpdate.Enabled = SettingsPlot.IsCluster();
			checkBoxliveUpdate.Checked = SettingsPlotCluster.liveUpdate;

			initialRangeValue = SettingsPlotCluster.clusterRange;
			initialWeightCapValue = SettingsPlotCluster.minClusterWeight;
			initialWebValue = SettingsPlotCluster.clusterWeb;

			usedLiveUpdate = false;
		}

		private void TrackBarClusterRange_Leave(object sender, EventArgs e)
		{
			LiveUpdate();
		}

		private void TrackBarClusterRange_KeyUp(object sender, KeyEventArgs e)
		{
			LiveUpdate();
		}

		private void TrackBarClusterRange_MouseUp(object sender, MouseEventArgs e)
		{
			LiveUpdate();
		}

		private void TrackBarMinClusterWeight_Leave(object sender, EventArgs e)
		{
			LiveUpdate();
		}

		private void TrackBarMinClusterWeight_KeyUp(object sender, KeyEventArgs e)
		{
			LiveUpdate();
		}

		private void TrackBarMinClusterWeight_MouseUp(object sender, MouseEventArgs e)
		{
			LiveUpdate();
		}

		private void CheckBoxDrawClusterWeb_CheckedChanged(object sender, EventArgs e)
		{
			LiveUpdate();
		}

		private void CheckBoxliveUpdate_CheckedChanged(object sender, EventArgs e)
		{
			SettingsPlotCluster.liveUpdate = checkBoxliveUpdate.Checked;
			LiveUpdate();
		}

		void LiveUpdate()
		{
			if (SettingsPlotCluster.liveUpdate && SettingsPlot.IsCluster() && FormMaster.GetNonRegionLegendItems().Count > 0)
			{
				SettingsPlotCluster.clusterRange = trackBarClusterRange.Value;
				SettingsPlotCluster.minClusterWeight = trackBarMinClusterWeight.Value;
				SettingsPlotCluster.clusterWeb = checkBoxDrawClusterWeb.Checked;
				usedLiveUpdate = true;
				FormMaster.DrawMap(false);
			}
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			SettingsPlotCluster.clusterWeb = checkBoxDrawClusterWeb.Checked;
			SettingsPlotCluster.clusterRange = trackBarClusterRange.Value;
			SettingsPlotCluster.minClusterWeight = trackBarMinClusterWeight.Value;

			Close();

			if (SettingsPlot.IsCluster())
			{
				FormMaster.DrawMap(false);
			}
		}

		private void ButtonCancel_Click(object sender, EventArgs e)
		{
			// If they used live update but now wish to cancel, we need to ensure we reset to the initial value
			if (usedLiveUpdate)
			{
				SettingsPlotCluster.clusterRange = initialRangeValue;
				SettingsPlotCluster.minClusterWeight = initialWeightCapValue;
				SettingsPlotCluster.clusterWeb = initialWebValue;
				FormMaster.DrawMap(false);
			}
		}

		private void TrackBarClusterRange_ValueChanged(object sender, EventArgs e)
		{
			labelClusterRange.Text = $"Cluster Range ({trackBarClusterRange.Value})";
		}

		private void TrackBarMinClusterWeight_ValueChanged(object sender, EventArgs e)
		{
			labelMinClusterWeight.Text = $"Min. Cluster Weight ({trackBarMinClusterWeight.Value})";
		}
	}
}
