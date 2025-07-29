namespace Mappalachia
{
	public partial class FormFontSettings : Form
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

			numericUpDownTitle.Minimum = InitialFontSettings.MinSizeStatic;
			numericUpDownTitle.Maximum = InitialFontSettings.MaxSizeStatic;

			numericUpDownLegend.Minimum = InitialFontSettings.MinSizePlotted;
			numericUpDownLegend.Maximum = InitialFontSettings.MaxSizePlotted;

			numericUpDownItemsInOtherSpaces.Minimum = InitialFontSettings.MinSizePlotted;
			numericUpDownItemsInOtherSpaces.Maximum = InitialFontSettings.MaxSizePlotted;

			numericUpDownMapMarkerLabel.Minimum = InitialFontSettings.MinSizePlotted;
			numericUpDownMapMarkerLabel.Maximum = InitialFontSettings.MaxSizePlotted;

			numericUpDownWatermark.Minimum = InitialFontSettings.MinSizeStatic;
			numericUpDownWatermark.Maximum = InitialFontSettings.MaxSizeStatic;

			numericUpDownClusterLabel.Minimum = InitialFontSettings.MinSizePlotted;
			numericUpDownClusterLabel.Maximum = InitialFontSettings.MaxSizePlotted;

			numericUpDownInstanceFormID.Minimum = InitialFontSettings.MinSizePlotted;
			numericUpDownInstanceFormID.Maximum = InitialFontSettings.MaxSizePlotted;

			numericUpDownRegionLevel.Minimum = InitialFontSettings.MinSizePlotted;
			numericUpDownRegionLevel.Maximum = InitialFontSettings.MaxSizePlotted;

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
