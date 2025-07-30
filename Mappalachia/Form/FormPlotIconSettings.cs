namespace Mappalachia
{
	// Does user control of the PlotIconSettings
	// Does not use a model
	public partial class FormPlotIconSettings : Form
	{
		// Create a new settings instance from the form state
		public PlotIconSettings PlotIconSettings => new PlotIconSettings()
		{
			// Note we're using the BackColor field to actually store the color
			// as this is more reliable than parsing it to text and back again
			Palette = listViewStandardPalette.Items.Cast<ListViewItem>().Select(i => i.BackColor).ToList(),
			TopographicPalette = listViewTopographPalette.Items.Cast<ListViewItem>().Select(i => i.BackColor).ToList(),
			Size = trackBarIconSize.Value,
		};

		public FormPlotIconSettings(PlotIconSettings plotIconSettings)
		{
			InitializeComponent();
			UpdatePalettes(plotIconSettings);
			SetSizeLabel();
		}

		static void RemoveColor(ListView listview)
		{
			foreach (ListViewItem item in listview.Items)
			{
				if (item.Selected)
				{
					item.Remove();
				}
			}
		}

		void AddColor(ListView listview)
		{
			if (colorDialog.ShowDialog() == DialogResult.OK)
			{
				Color color = colorDialog.Color;
				listview.Items.Add(new ListViewItem() { Text = color.Name, BackColor = color });
			}
		}

		void UpdatePalettes(PlotIconSettings plotIconSettings)
		{
			listViewStandardPalette.Clear();
			listViewTopographPalette.Clear();

			foreach (Color color in plotIconSettings.Palette)
			{
				listViewStandardPalette.Items.Add(new ListViewItem() { Text = color.Name, BackColor = color });
			}

			foreach (Color color in plotIconSettings.TopographicPalette)
			{
				listViewTopographPalette.Items.Add(new ListViewItem() { Text = color.Name, BackColor = color });
			}

			trackBarIconSize.Minimum = PlotIconSettings.MinSize;
			trackBarIconSize.Maximum = PlotIconSettings.MaxSize;
			trackBarIconSize.Value = Math.Clamp(plotIconSettings.Size, trackBarIconSize.Minimum, trackBarIconSize.Maximum);
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void ButtonAddColorStandard_Click(object sender, EventArgs e)
		{
			AddColor(listViewStandardPalette);
		}

		private void ButtonRemoveSelectedStandard_Click(object sender, EventArgs e)
		{
			RemoveColor(listViewStandardPalette);
		}

		private void ButtonAddColorTopograph_Click(object sender, EventArgs e)
		{
			AddColor(listViewTopographPalette);
		}

		private void ButtonRemoveSelectedTopograph_Click(object sender, EventArgs e)
		{
			RemoveColor(listViewTopographPalette);
		}

		private void ButtonResetAll_Click(object sender, EventArgs e)
		{
			PlotIconSettings newSettings = new PlotIconSettings();
			trackBarIconSize.Value = newSettings.Size;
			SetSizeLabel();

			UpdatePalettes(newSettings);
		}

		private void TrackBarIconSize_Scroll(object sender, EventArgs e)
		{
			SetSizeLabel();
		}

		void SetSizeLabel()
		{
			labelIconSize.Text = $"Size ({trackBarIconSize.Value})";
		}
	}
}
