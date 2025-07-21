using Library;

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
		};

		public int ClusterMinWeightValue => trackBarClusterMinWeight.Value;

		public FormClusterSettings(FormMain formMain)
		{
			InitializeComponent();

			// Disable while loading so setting the trackbars doesn't fire draws
			checkBoxLiveUpdate.Checked = false;

			FormMain = formMain;
			InitialSettings = FormMain.Settings.PlotSettings.ClusterSettings;

			trackBarClusterRange.Maximum = (int)(FormMain.Settings.Space.MaxRange / 2);

			trackBarClusterRange.Value = Math.Min(InitialSettings.Range, trackBarClusterRange.Maximum);
			trackBarClusterMinWeight.Value = InitialSettings.MinWeight;
			checkBoxLiveUpdate.Checked = InitialSettings.LiveUpdate;

			SetRangeLabel();
			SetWeightLabel();
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
			if (checkBoxLiveUpdate.Checked)
			{
				FormMain.SetClusterSettings(ClusterSettings);
				LiveUpdateCausedDraw = true;
			}
		}
	}
}
