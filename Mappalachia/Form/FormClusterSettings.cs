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
		bool initialized = false;

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

			UpdateRangeLabel();
			UpdateWeightLabel();

			usedLiveUpdate = false;
			initialized = true;
		}

		void UpdateRangeLabel()
		{
			labelClusterRange.Text = $"Cluster Range ({trackBarClusterRange.Value})";
		}

		void UpdateWeightLabel()
		{
			labelMinClusterWeight.Text = $"Min. Cluster Weight ({trackBarMinClusterWeight.Value})";
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

		private void TrackBarClusterRange_ValueChanged(object sender, EventArgs e)
		{
			UpdateRangeLabel();
			LiveUpdate();
		}

		private void TrackBarMinClusterWeight_ValueChanged(object sender, EventArgs e)
		{
			UpdateWeightLabel();
			LiveUpdate();
		}

		void LiveUpdate()
		{
			// If the form is still loading or liveUpdate is off or it's not in cluster mode or there are no clusters to draw
			if (!initialized || !SettingsPlotCluster.liveUpdate || !SettingsPlot.IsCluster() || FormMaster.GetNonRegionLegendItems().Count == 0)
			{
				return;
			}

			SettingsPlotCluster.clusterRange = trackBarClusterRange.Value;
			SettingsPlotCluster.minClusterWeight = trackBarMinClusterWeight.Value;
			SettingsPlotCluster.clusterWeb = checkBoxDrawClusterWeb.Checked;
			usedLiveUpdate = true;
			FormMaster.QueueDraw(false);
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			SettingsPlotCluster.clusterWeb = checkBoxDrawClusterWeb.Checked;
			SettingsPlotCluster.clusterRange = trackBarClusterRange.Value;
			SettingsPlotCluster.minClusterWeight = trackBarMinClusterWeight.Value;

			Close();

			if (SettingsPlot.IsCluster())
			{
				FormMaster.QueueDraw(false);
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

		// Offset the window to the left so it doesn't obscure the map preview when it loads
		private void FormSetClusterRange_Load(object sender, EventArgs e)
		{
			Location = new System.Drawing.Point(Location.X - (Width / 2), Location.Y);
		}
	}
}
