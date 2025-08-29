namespace Mappalachia
{
	public partial class FormHeatmapSettings : GenericToolForm
	{
		FormMain FormMain { get; }

		// The settings when the form opened, so we can revert on cancel
		HeatmapSettings InitialSettings { get; }

		public HeatmapSettings HeatmapSettings => new HeatmapSettings(
			trackBarRange.Value,
			trackBarIntensity.Value,
			checkBoxLiveUpdate.Checked);

		bool Initialized { get; set; } = false;

		public FormHeatmapSettings(FormMain formMain)
		{
			InitializeComponent();

			FormMain = formMain;
			InitialSettings = FormMain.Settings.PlotSettings.HeatmapSettings;

			trackBarRange.Maximum = HeatmapSettings.MaxRange;
			trackBarRange.Minimum = HeatmapSettings.MinRange;

			trackBarIntensity.Maximum = HeatmapSettings.MaxIntensity;
			trackBarIntensity.Minimum = HeatmapSettings.MinIntensity;

			trackBarRange.Value = Math.Clamp(InitialSettings.Range, trackBarRange.Minimum, trackBarRange.Maximum);
			trackBarIntensity.Value = InitialSettings.Intensity;

			checkBoxLiveUpdate.Checked = InitialSettings.LiveUpdate;

			SetRangeLabel();
			SetIntensityLabel();

			Initialized = true;
			UpdateMapView();
		}

		private void ButtonCancel_Click(object sender, EventArgs e)
		{
			FormMain.Settings.PlotSettings.HeatmapSettings = InitialSettings;
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void TrackBarRange_ValueChanged(object sender, EventArgs e)
		{
			SetRangeLabel();
			UpdateMapView();
		}

		private void TrackBarIntensity_ValueChanged(object sender, EventArgs e)
		{
			SetIntensityLabel();
			UpdateMapView();
		}

		private void CheckBoxLiveUpdate_CheckedChanged(object sender, EventArgs e)
		{
			UpdateMapView();
		}

		void SetRangeLabel()
		{
			labelClusterRange.Text = $"Range ({trackBarRange.Value})";
		}

		void SetIntensityLabel()
		{
			labelClusterMinWeight.Text = $"Intensity ({trackBarIntensity.Value})";
		}

		async void UpdateMapView()
		{
			if (!Initialized)
			{
				return;
			}

			if (checkBoxLiveUpdate.Checked)
			{
				await FormMain.HeatmapSettingsLiveUpdate(HeatmapSettings);
			}
		}
	}
}
