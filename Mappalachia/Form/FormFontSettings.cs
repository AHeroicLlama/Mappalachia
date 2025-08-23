namespace Mappalachia
{
	public partial class FormFontSettings : GenericToolForm
	{
		FontSettings InitialFontSettings { get; set; }

		public FontSettings FontSettings => new FontSettings()
		{
			SizeTitle = (int)numericUpDownTitle.Value,
			SizeLegend = (int)numericUpDownLegend.Value,
			SizeItemsInOtherSpaces = (int)numericUpDownItemsInOtherSpaces.Value,
			SizeMapMarkerLabel = (int)numericUpDownMapMarkerLabel.Value,
			SizeWatermark = (int)numericUpDownWatermark.Value,
			SizeClusterLabel = (int)numericUpDownClusterLabel.Value,
			SizeInstanceFormID = (int)numericUpDownInstanceFormID.Value,
			SizeRegionLevel = (int)numericUpDownRegionLevel.Value,
		};

		public FormFontSettings(FontSettings fontSettings)
		{
			InitializeComponent();
			InitialFontSettings = fontSettings;
			Initialize(fontSettings);
		}

		void Initialize(FontSettings fontSettings)
		{
			InitialFontSettings = fontSettings;

			numericUpDownTitle.Minimum = FontSettings.MinSizeStatic;
			numericUpDownTitle.Maximum = FontSettings.MaxSizeStatic;

			numericUpDownLegend.Minimum = FontSettings.MinSizePlotted;
			numericUpDownLegend.Maximum = FontSettings.MaxSizePlotted;

			numericUpDownItemsInOtherSpaces.Minimum = FontSettings.MinSizePlotted;
			numericUpDownItemsInOtherSpaces.Maximum = FontSettings.MaxSizePlotted;

			numericUpDownMapMarkerLabel.Minimum = FontSettings.MinSizePlotted;
			numericUpDownMapMarkerLabel.Maximum = FontSettings.MaxSizePlotted;

			numericUpDownWatermark.Minimum = FontSettings.MinSizeStatic;
			numericUpDownWatermark.Maximum = FontSettings.MaxSizeStatic;

			numericUpDownClusterLabel.Minimum = FontSettings.MinSizePlotted;
			numericUpDownClusterLabel.Maximum = FontSettings.MaxSizePlotted;

			numericUpDownInstanceFormID.Minimum = FontSettings.MinSizePlotted;
			numericUpDownInstanceFormID.Maximum = FontSettings.MaxSizePlotted;

			numericUpDownRegionLevel.Minimum = FontSettings.MinSizePlotted;
			numericUpDownRegionLevel.Maximum = FontSettings.MaxSizePlotted;

			numericUpDownTitle.Value = Math.Clamp(InitialFontSettings.SizeTitle, numericUpDownTitle.Minimum, numericUpDownTitle.Maximum);
			numericUpDownLegend.Value = Math.Clamp(InitialFontSettings.SizeLegend, numericUpDownLegend.Minimum, numericUpDownLegend.Maximum);
			numericUpDownItemsInOtherSpaces.Value = Math.Clamp(InitialFontSettings.SizeItemsInOtherSpaces, numericUpDownItemsInOtherSpaces.Minimum, numericUpDownItemsInOtherSpaces.Maximum);
			numericUpDownMapMarkerLabel.Value = Math.Clamp(InitialFontSettings.SizeMapMarkerLabel, numericUpDownMapMarkerLabel.Minimum, numericUpDownMapMarkerLabel.Maximum);
			numericUpDownWatermark.Value = Math.Clamp(InitialFontSettings.SizeWatermark, numericUpDownWatermark.Minimum, numericUpDownWatermark.Maximum);
			numericUpDownClusterLabel.Value = Math.Clamp(InitialFontSettings.SizeClusterLabel, numericUpDownClusterLabel.Minimum, numericUpDownClusterLabel.Maximum);
			numericUpDownInstanceFormID.Value = Math.Clamp(InitialFontSettings.SizeInstanceFormID, numericUpDownInstanceFormID.Minimum, numericUpDownInstanceFormID.Maximum);
			numericUpDownRegionLevel.Value = Math.Clamp(InitialFontSettings.SizeRegionLevel, numericUpDownRegionLevel.Minimum, numericUpDownRegionLevel.Maximum);
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void ButtonResetAll_Click(object sender, EventArgs e)
		{
			Initialize(new FontSettings());
		}
	}
}
