namespace Mappalachia
{
	public partial class FormClusterSettings : Form
	{
		FormMain FormMain { get; }

		bool LiveUpdateCausedDraw { get; set; } = false;

		// The settings when the form opened, so we can revert on cancel
		ClusterSettings InitialSettings { get; }

		public ClusterSettings ClusterSettings => new ClusterSettings
		{
			Range = trackBarClusterRange.Value,
			MinWeight = trackBarClusterMinWeight.Value,
			LiveUpdate = checkBoxLiveUpdate.Checked,
			ClusterPerLegendGroup = radioButtonPerLegendGroup.Checked,
		};

		bool Initialized { get; set; } = false;

		public FormClusterSettings(FormMain formMain)
		{
			InitializeComponent();

			FormMain = formMain;
			InitialSettings = FormMain.Settings.PlotSettings.ClusterSettings;

			trackBarClusterRange.Maximum = (int)Math.Min(FormMain.Settings.Space.MaxRange / 2, InitialSettings.MaxRange);

			trackBarClusterRange.Value = Math.Clamp(InitialSettings.Range, trackBarClusterRange.Minimum, trackBarClusterRange.Maximum);
			trackBarClusterMinWeight.Value = InitialSettings.MinWeight;
			checkBoxLiveUpdate.Checked = InitialSettings.LiveUpdate;
			radioButtonPerLegendGroup.Checked = InitialSettings.ClusterPerLegendGroup;
			radioButtonGroupEverything.Checked = !InitialSettings.ClusterPerLegendGroup;

			SetRangeLabel();
			SetWeightLabel();

			Initialized = true;
		}

		private void ButtonCancel_Click(object sender, EventArgs e)
		{
			FormMain.Settings.PlotSettings.ClusterSettings = InitialSettings;

			if (LiveUpdateCausedDraw)
			{
				DialogResult = DialogResult.Abort;
			}

			Close();
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void TrackBarClusterRange_ValueChanged(object sender, EventArgs e)
		{
			SetRangeLabel();
			UpdateMapView();
		}

		private void TrackBarClusterWeight_ValueChanged(object sender, EventArgs e)
		{
			SetWeightLabel();
			UpdateMapView();
		}

		private void CheckBoxLiveUpdate_CheckedChanged(object sender, EventArgs e)
		{
			UpdateMapView();
		}

		private void RadioButtonGroupEverything_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButtonGroupEverything.Checked)
			{
				UpdateMapView();
			}
		}

		private void RadioButtonPerLegendGroup_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButtonPerLegendGroup.Checked)
			{
				UpdateMapView();
			}
		}

		void SetRangeLabel()
		{
			labelClusterRange.Text = $"Cluster Range ({trackBarClusterRange.Value})";
		}

		void SetWeightLabel()
		{
			labelClusterMinWeight.Text = $"Min. Cluster Weight ({trackBarClusterMinWeight.Value})";
		}

		void UpdateMapView()
		{
			if (!Initialized)
			{
				return;
			}

			if (checkBoxLiveUpdate.Checked)
			{
				FormMain.SetClusterSettings(ClusterSettings);
				LiveUpdateCausedDraw = true;
			}
		}
	}
}
