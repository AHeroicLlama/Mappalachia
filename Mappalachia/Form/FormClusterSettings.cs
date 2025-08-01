namespace Mappalachia
{
	public partial class FormClusterSettings : Form
	{
		FormMain FormMain { get; }

		// The settings when the form opened, so we can revert on cancel
		ClusterSettings InitialSettings { get; }

		public ClusterSettings ClusterSettings => new ClusterSettings(
			trackBarClusterRange.Value,
			trackBarClusterMinWeight.Value,
			checkBoxLiveUpdate.Checked,
			radioButtonPerLegendGroup.Checked);

		bool Initialized { get; set; } = false;

		public FormClusterSettings(FormMain formMain)
		{
			InitializeComponent();

			FormMain = formMain;
			InitialSettings = FormMain.Settings.PlotSettings.ClusterSettings;

			trackBarClusterRange.Maximum = (int)Math.Min(FormMain.Settings.Space.MaxRange / 2, ClusterSettings.MaxRange);

			trackBarClusterRange.Value = Math.Clamp(InitialSettings.Range, trackBarClusterRange.Minimum, trackBarClusterRange.Maximum);
			trackBarClusterMinWeight.Value = InitialSettings.MinWeight;
			checkBoxLiveUpdate.Checked = InitialSettings.LiveUpdate;
			radioButtonPerLegendGroup.Checked = InitialSettings.ClusterPerLegendGroup;
			radioButtonGroupEverything.Checked = !InitialSettings.ClusterPerLegendGroup;

			SetRangeLabel();
			SetWeightLabel();

			Initialized = true;
			UpdateMapView();
		}

		private void ButtonCancel_Click(object sender, EventArgs e)
		{
			FormMain.Settings.PlotSettings.ClusterSettings = InitialSettings;
			DialogResult = DialogResult.Abort;
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
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
			labelClusterRange.Text = $"Cluster Radius ({trackBarClusterRange.Value})";
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
				FormMain.ClusterSettingsLiveUpdate(ClusterSettings);
			}
		}

		private void LabelClusterRange_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
			{
				return;
			}

			ContextMenuStrip contextMenu = new ContextMenuStrip();
			ToolStripMenuItem setToNukerange = new ToolStripMenuItem() { Text = "Set to Nuke blast zone radius" };

			setToNukerange.Click += (s, args) =>
			{
				trackBarClusterRange.Value = Map.BlastRadius;
			};

			contextMenu.Items.Add(setToNukerange);
			contextMenu.Show(this, e.Location);
		}
	}
}
