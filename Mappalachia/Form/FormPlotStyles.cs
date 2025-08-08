namespace Mappalachia
{
	// Does user control of the PlotStyleSettings
	// Does not use a model
	public partial class FormPlotStyles : GenericlToolForm
	{
		// Create a new settings instance from the form state
		public PlotStyleSettings PlotStyleSettings => new PlotStyleSettings()
		{
			// Note we're using the BackColor field to actually store the color
			// as this is more reliable than parsing it to text and back again
			Palette = listViewStandardPalette.Items.Cast<ListViewItem>().Select(i => i.BackColor).ToList(),
			SecondaryPalette = listViewSecondaryPalette.Items.Cast<ListViewItem>().Select(i => i.BackColor).ToList(),
			Size = trackBarIconSize.Value,
		};

		public FormPlotStyles(PlotStyleSettings plotStyleSettings)
		{
			InitializeComponent();
			UpdatePalettes(plotStyleSettings);
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

		void UpdatePalettes(PlotStyleSettings plotStyleSettings)
		{
			listViewStandardPalette.Clear();
			listViewSecondaryPalette.Clear();

			foreach (Color color in plotStyleSettings.Palette)
			{
				listViewStandardPalette.Items.Add(new ListViewItem() { Text = color.Name, BackColor = color });
			}

			foreach (Color color in plotStyleSettings.SecondaryPalette)
			{
				listViewSecondaryPalette.Items.Add(new ListViewItem() { Text = color.Name, BackColor = color });
			}

			trackBarIconSize.Minimum = PlotStyleSettings.MinSize;
			trackBarIconSize.Maximum = PlotStyleSettings.MaxSize;
			trackBarIconSize.Value = Math.Clamp(plotStyleSettings.Size, trackBarIconSize.Minimum, trackBarIconSize.Maximum);
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
			AddColor(listViewSecondaryPalette);
		}

		private void ButtonRemoveSelectedTopograph_Click(object sender, EventArgs e)
		{
			RemoveColor(listViewSecondaryPalette);
		}

		private void ButtonResetAll_Click(object sender, EventArgs e)
		{
			PlotStyleSettings newSettings = new PlotStyleSettings();
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
			labelIconSize.Text = $"Default Icon Size ({trackBarIconSize.Value})";
		}
	}
}
