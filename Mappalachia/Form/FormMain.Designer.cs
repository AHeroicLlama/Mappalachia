namespace Mappalachia
{
	partial class FormMain
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			menuStripMain = new MenuStrip();
			mapMenuItem = new ToolStripMenuItem();
			showPreviewToolStripMenuItem = new ToolStripMenuItem();
			openExternallyToolStripMenuItem = new ToolStripMenuItem();
			setTitleToolStripMenuItem = new ToolStripMenuItem();
			fontSizesToolStripMenuItem = new ToolStripMenuItem();
			mapMapMarkersToolStripMenuItem = new ToolStripMenuItem();
			mapMarkerIconsToolStripMenuItem = new ToolStripMenuItem();
			mapMarkerLabelsToolStripMenuItem = new ToolStripMenuItem();
			backgroundImageMenuItem = new ToolStripMenuItem();
			backgroundNormalToolStripMenuItem = new ToolStripMenuItem();
			backgroundSatelliteToolStripMenuItem = new ToolStripMenuItem();
			backgroundMilitaryToolStripMenuItem = new ToolStripMenuItem();
			backgroundNoneToolStripMenuItem = new ToolStripMenuItem();
			grayscaleToolStripMenuItem = new ToolStripMenuItem();
			setBrightnessToolStripMenuItem = new ToolStripMenuItem();
			highlightWaterToolStripMenuItem = new ToolStripMenuItem();
			showCompassToolStripMenuItem = new ToolStripMenuItem();
			compassAlwaysToolStripMenuItem = new ToolStripMenuItem();
			compassWhenUsefulToolStripMenuItem = new ToolStripMenuItem();
			compassNeverToolStripMenuItem = new ToolStripMenuItem();
			spotlightToolStripMenuItem = new ToolStripMenuItem();
			spotlightEnabledToolStripMenuItem = new ToolStripMenuItem();
			spotlightSetRangeToolStripMenuItem = new ToolStripMenuItem();
			spotlightCoordToolStripMenuItem = new ToolStripMenuItem();
			legendStyleToolStripMenuItem = new ToolStripMenuItem();
			legendNormalToolStripMenuItem = new ToolStripMenuItem();
			legendExtendedToolStripMenuItem = new ToolStripMenuItem();
			legendHiddenToolStripMenuItem = new ToolStripMenuItem();
			loadRecipeToolStripMenuItem = new ToolStripMenuItem();
			saveAsRecipeToolStripMenuItem = new ToolStripMenuItem();
			exportToFileToolStripMenuItem = new ToolStripMenuItem();
			quickSaveToolStripMenuItem = new ToolStripMenuItem();
			clearPlotsToolStripMenuItem = new ToolStripMenuItem();
			resetToolStripMenuItem = new ToolStripMenuItem();
			searchSettingsToolStripMenuItem = new ToolStripMenuItem();
			searchInAllSpacesToolStripMenuItem = new ToolStripMenuItem();
			searchInInstancesOnlyToolStripMenuItem = new ToolStripMenuItem();
			advancedModeToolStripMenuItem = new ToolStripMenuItem();
			plotSettingsMenuItem = new ToolStripMenuItem();
			plotModeMenuItem = new ToolStripMenuItem();
			plotModeStandardToolStripMenuItem = new ToolStripMenuItem();
			plotModeTopographicToolStripMenuItem = new ToolStripMenuItem();
			plotModeHeatmapToolStripMenuItem = new ToolStripMenuItem();
			plotModeClusterToolStripMenuItem = new ToolStripMenuItem();
			plotStylesToolStripMenuItem = new ToolStripMenuItem();
			heatmapSettingsToolStripMenuItem = new ToolStripMenuItem();
			clusterSettingsToolStripMenuItem = new ToolStripMenuItem();
			volumeDrawStyleToolStripMenuItem = new ToolStripMenuItem();
			volumeFillToolStripMenuItem = new ToolStripMenuItem();
			volumeBorderToolStripMenuItem = new ToolStripMenuItem();
			volumeBothToolStripMenuItem = new ToolStripMenuItem();
			showPlotsInOtherSpacesToolStripMenuItem = new ToolStripMenuItem();
			showRegionLevelsToolStripMenuItem = new ToolStripMenuItem();
			drawInstanceFormIDToolStripMenuItem = new ToolStripMenuItem();
			helpToolStripMenuItem = new ToolStripMenuItem();
			aboutToolStripMenuItem = new ToolStripMenuItem();
			openUserGuidesToolStripMenuItem = new ToolStripMenuItem();
			checkForUpdatesToolStripMenuItem = new ToolStripMenuItem();
			installSpotlightToolStripMenuItem = new ToolStripMenuItem();
			viewGitHubToolStripMenuItem = new ToolStripMenuItem();
			resetEverythingToolStripMenuItem = new ToolStripMenuItem();
			joinTheDiscordToolStripMenuItem = new ToolStripMenuItem();
			sayThanksToolStripMenuItem = new ToolStripMenuItem();
			buttonSearch = new Button();
			textBoxSearch = new TextBox();
			dataGridViewSearchResults = new DataGridView();
			comboBoxSpace = new ComboBox();
			buttonAddToMap = new DropDownButton();
			buttonRemoveFromMap = new Button();
			dataGridViewItemsToPlot = new DataGridView();
			listViewSignature = new ListView();
			listViewLockLevel = new ListView();
			groupBoxFilterSignature = new GroupBox();
			buttonSelectRecommended = new Button();
			buttonUnselectAllSignature = new Button();
			buttonSelectAllSignature = new Button();
			groupBoxFilterLockLevel = new GroupBox();
			buttonUnselectAllLockLevel = new Button();
			buttonSelectAllLockLevel = new Button();
			buttonUpdateMap = new Button();
			progressBarMain = new ProgressBar();
			labelProgressStatus = new Label();
			labelSearchResults = new Label();
			labelItemsToPlot = new Label();
			labelSearchTerm = new Label();
			labelSelectedSpace = new Label();
			panelBackground = new Panel();
			panelAutoScrollTrigger = new Panel();
			menuStripMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dataGridViewSearchResults).BeginInit();
			((System.ComponentModel.ISupportInitialize)dataGridViewItemsToPlot).BeginInit();
			groupBoxFilterSignature.SuspendLayout();
			groupBoxFilterLockLevel.SuspendLayout();
			panelBackground.SuspendLayout();
			SuspendLayout();
			// 
			// menuStripMain
			// 
			menuStripMain.AllowClickThrough = true;
			menuStripMain.Items.AddRange(new ToolStripItem[] { mapMenuItem, searchSettingsToolStripMenuItem, plotSettingsMenuItem, helpToolStripMenuItem, joinTheDiscordToolStripMenuItem, sayThanksToolStripMenuItem });
			menuStripMain.Location = new Point(0, 0);
			menuStripMain.Name = "menuStripMain";
			menuStripMain.ShowItemToolTips = true;
			menuStripMain.Size = new Size(984, 24);
			menuStripMain.TabIndex = 0;
			// 
			// mapMenuItem
			// 
			mapMenuItem.DropDownItems.AddRange(new ToolStripItem[] { showPreviewToolStripMenuItem, openExternallyToolStripMenuItem, setTitleToolStripMenuItem, fontSizesToolStripMenuItem, mapMapMarkersToolStripMenuItem, backgroundImageMenuItem, grayscaleToolStripMenuItem, setBrightnessToolStripMenuItem, highlightWaterToolStripMenuItem, showCompassToolStripMenuItem, spotlightToolStripMenuItem, legendStyleToolStripMenuItem, loadRecipeToolStripMenuItem, saveAsRecipeToolStripMenuItem, exportToFileToolStripMenuItem, quickSaveToolStripMenuItem, clearPlotsToolStripMenuItem, resetToolStripMenuItem });
			mapMenuItem.Name = "mapMenuItem";
			mapMenuItem.Size = new Size(43, 20);
			mapMenuItem.Text = "Map";
			// 
			// showPreviewToolStripMenuItem
			// 
			showPreviewToolStripMenuItem.Name = "showPreviewToolStripMenuItem";
			showPreviewToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Space;
			showPreviewToolStripMenuItem.Size = new Size(240, 22);
			showPreviewToolStripMenuItem.Text = "Show Preview";
			showPreviewToolStripMenuItem.ToolTipText = "Bring up the preview window.";
			showPreviewToolStripMenuItem.Click += Map_ShowPreview_Click;
			// 
			// openExternallyToolStripMenuItem
			// 
			openExternallyToolStripMenuItem.Name = "openExternallyToolStripMenuItem";
			openExternallyToolStripMenuItem.Size = new Size(240, 22);
			openExternallyToolStripMenuItem.Text = "Open Externally";
			openExternallyToolStripMenuItem.ToolTipText = "Open the current map image in your default image viewer.";
			openExternallyToolStripMenuItem.Click += Map_OpenExternally;
			// 
			// setTitleToolStripMenuItem
			// 
			setTitleToolStripMenuItem.Name = "setTitleToolStripMenuItem";
			setTitleToolStripMenuItem.Size = new Size(240, 22);
			setTitleToolStripMenuItem.Text = "Set Title";
			setTitleToolStripMenuItem.ToolTipText = "Set a title for your map.";
			setTitleToolStripMenuItem.Click += Map_SetTitle_Click;
			// 
			// fontSizesToolStripMenuItem
			// 
			fontSizesToolStripMenuItem.Name = "fontSizesToolStripMenuItem";
			fontSizesToolStripMenuItem.Size = new Size(240, 22);
			fontSizesToolStripMenuItem.Text = "Font Sizes";
			fontSizesToolStripMenuItem.ToolTipText = "Set font sizes.";
			fontSizesToolStripMenuItem.Click += Map_SetFontSizes_Click;
			// 
			// mapMapMarkersToolStripMenuItem
			// 
			mapMapMarkersToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mapMarkerIconsToolStripMenuItem, mapMarkerLabelsToolStripMenuItem });
			mapMapMarkersToolStripMenuItem.Name = "mapMapMarkersToolStripMenuItem";
			mapMapMarkersToolStripMenuItem.Size = new Size(240, 22);
			mapMapMarkersToolStripMenuItem.Text = "Map Markers";
			// 
			// mapMarkerIconsToolStripMenuItem
			// 
			mapMarkerIconsToolStripMenuItem.Name = "mapMarkerIconsToolStripMenuItem";
			mapMarkerIconsToolStripMenuItem.Size = new Size(107, 22);
			mapMarkerIconsToolStripMenuItem.Text = "Icons";
			mapMarkerIconsToolStripMenuItem.ToolTipText = "Display the icons of fast travel locations.";
			mapMarkerIconsToolStripMenuItem.Click += Map_MapMarkers_Icons_Click;
			// 
			// mapMarkerLabelsToolStripMenuItem
			// 
			mapMarkerLabelsToolStripMenuItem.Name = "mapMarkerLabelsToolStripMenuItem";
			mapMarkerLabelsToolStripMenuItem.Size = new Size(107, 22);
			mapMarkerLabelsToolStripMenuItem.Text = "Labels";
			mapMarkerLabelsToolStripMenuItem.ToolTipText = "Display the names of the fast travel locations.";
			mapMarkerLabelsToolStripMenuItem.Click += Map_MapMarkers_Labels_Click;
			// 
			// backgroundImageMenuItem
			// 
			backgroundImageMenuItem.DropDownItems.AddRange(new ToolStripItem[] { backgroundNormalToolStripMenuItem, backgroundSatelliteToolStripMenuItem, backgroundMilitaryToolStripMenuItem, backgroundNoneToolStripMenuItem });
			backgroundImageMenuItem.Name = "backgroundImageMenuItem";
			backgroundImageMenuItem.Size = new Size(240, 22);
			backgroundImageMenuItem.Text = "Background Image";
			// 
			// backgroundNormalToolStripMenuItem
			// 
			backgroundNormalToolStripMenuItem.Name = "backgroundNormalToolStripMenuItem";
			backgroundNormalToolStripMenuItem.Size = new Size(115, 22);
			backgroundNormalToolStripMenuItem.Text = "Normal";
			backgroundNormalToolStripMenuItem.ToolTipText = "Set the background image to the typical pause menu map.";
			backgroundNormalToolStripMenuItem.Click += Map_Background_Normal_Click;
			// 
			// backgroundSatelliteToolStripMenuItem
			// 
			backgroundSatelliteToolStripMenuItem.Name = "backgroundSatelliteToolStripMenuItem";
			backgroundSatelliteToolStripMenuItem.Size = new Size(115, 22);
			backgroundSatelliteToolStripMenuItem.Text = "Satellite";
			backgroundSatelliteToolStripMenuItem.ToolTipText = "Set the background image to a top-down render of the in-game world.";
			backgroundSatelliteToolStripMenuItem.Click += Map_Background_Satellite_Click;
			// 
			// backgroundMilitaryToolStripMenuItem
			// 
			backgroundMilitaryToolStripMenuItem.Name = "backgroundMilitaryToolStripMenuItem";
			backgroundMilitaryToolStripMenuItem.Size = new Size(115, 22);
			backgroundMilitaryToolStripMenuItem.Text = "Military";
			backgroundMilitaryToolStripMenuItem.ToolTipText = "Set the background image to the military-style topographic map found in train stations.";
			backgroundMilitaryToolStripMenuItem.Click += Map_Background_Military_Click;
			// 
			// backgroundNoneToolStripMenuItem
			// 
			backgroundNoneToolStripMenuItem.Name = "backgroundNoneToolStripMenuItem";
			backgroundNoneToolStripMenuItem.Size = new Size(115, 22);
			backgroundNoneToolStripMenuItem.Text = "None";
			backgroundNoneToolStripMenuItem.ToolTipText = "Disable the background map image.";
			backgroundNoneToolStripMenuItem.Click += Map_Background_None_Click;
			// 
			// grayscaleToolStripMenuItem
			// 
			grayscaleToolStripMenuItem.Name = "grayscaleToolStripMenuItem";
			grayscaleToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.G;
			grayscaleToolStripMenuItem.Size = new Size(240, 22);
			grayscaleToolStripMenuItem.Text = "Grayscale Background";
			grayscaleToolStripMenuItem.ToolTipText = "Set the background to black and white, making plots more visible.";
			grayscaleToolStripMenuItem.Click += Map_Grayscale_Click;
			// 
			// setBrightnessToolStripMenuItem
			// 
			setBrightnessToolStripMenuItem.Name = "setBrightnessToolStripMenuItem";
			setBrightnessToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.B;
			setBrightnessToolStripMenuItem.Size = new Size(240, 22);
			setBrightnessToolStripMenuItem.Text = "Set Brightness";
			setBrightnessToolStripMenuItem.Click += Map_SetBrightness_Click;
			// 
			// highlightWaterToolStripMenuItem
			// 
			highlightWaterToolStripMenuItem.Name = "highlightWaterToolStripMenuItem";
			highlightWaterToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.H;
			highlightWaterToolStripMenuItem.Size = new Size(240, 22);
			highlightWaterToolStripMenuItem.Text = "Highlight Water";
			highlightWaterToolStripMenuItem.ToolTipText = "Overlay a blue highlight showing accessible surface water.";
			highlightWaterToolStripMenuItem.Click += Map_HightlightWater_Click;
			// 
			// showCompassToolStripMenuItem
			// 
			showCompassToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { compassAlwaysToolStripMenuItem, compassWhenUsefulToolStripMenuItem, compassNeverToolStripMenuItem });
			showCompassToolStripMenuItem.Name = "showCompassToolStripMenuItem";
			showCompassToolStripMenuItem.Size = new Size(240, 22);
			showCompassToolStripMenuItem.Text = "Show Compass";
			showCompassToolStripMenuItem.ToolTipText = "Choose when to display a compass rose on the map.";
			// 
			// compassAlwaysToolStripMenuItem
			// 
			compassAlwaysToolStripMenuItem.Name = "compassAlwaysToolStripMenuItem";
			compassAlwaysToolStripMenuItem.Size = new Size(141, 22);
			compassAlwaysToolStripMenuItem.Text = "Always";
			compassAlwaysToolStripMenuItem.ToolTipText = "The compass rose is always drawn.";
			compassAlwaysToolStripMenuItem.Click += Map_Compass_Always_Click;
			// 
			// compassWhenUsefulToolStripMenuItem
			// 
			compassWhenUsefulToolStripMenuItem.Name = "compassWhenUsefulToolStripMenuItem";
			compassWhenUsefulToolStripMenuItem.Size = new Size(141, 22);
			compassWhenUsefulToolStripMenuItem.Text = "When Useful";
			compassWhenUsefulToolStripMenuItem.ToolTipText = "The compass is drawn only when North is not 'up' on the map already.";
			compassWhenUsefulToolStripMenuItem.Click += Map_Compass_WhenUseful_Click;
			// 
			// compassNeverToolStripMenuItem
			// 
			compassNeverToolStripMenuItem.Name = "compassNeverToolStripMenuItem";
			compassNeverToolStripMenuItem.Size = new Size(141, 22);
			compassNeverToolStripMenuItem.Text = "Never";
			compassNeverToolStripMenuItem.ToolTipText = "The compass is never drawn.";
			compassNeverToolStripMenuItem.Click += Map_Compass_Never_Click;
			// 
			// spotlightToolStripMenuItem
			// 
			spotlightToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { spotlightEnabledToolStripMenuItem, spotlightSetRangeToolStripMenuItem, spotlightCoordToolStripMenuItem });
			spotlightToolStripMenuItem.Name = "spotlightToolStripMenuItem";
			spotlightToolStripMenuItem.Size = new Size(240, 22);
			spotlightToolStripMenuItem.Text = "Spotlight";
			// 
			// spotlightEnabledToolStripMenuItem
			// 
			spotlightEnabledToolStripMenuItem.Name = "spotlightEnabledToolStripMenuItem";
			spotlightEnabledToolStripMenuItem.Size = new Size(116, 22);
			spotlightEnabledToolStripMenuItem.Text = "Enabled";
			spotlightEnabledToolStripMenuItem.Click += Map_Spotlight_Enabled_Click;
			// 
			// spotlightSetRangeToolStripMenuItem
			// 
			spotlightSetRangeToolStripMenuItem.Name = "spotlightSetRangeToolStripMenuItem";
			spotlightSetRangeToolStripMenuItem.Size = new Size(116, 22);
			spotlightSetRangeToolStripMenuItem.Text = "Set Size";
			spotlightSetRangeToolStripMenuItem.Click += Map_Spotlight_SetRange_Click;
			// 
			// spotlightCoordToolStripMenuItem
			// 
			spotlightCoordToolStripMenuItem.Name = "spotlightCoordToolStripMenuItem";
			spotlightCoordToolStripMenuItem.Size = new Size(116, 22);
			spotlightCoordToolStripMenuItem.Text = "Coord";
			spotlightCoordToolStripMenuItem.Click += Map_Spotlight_Coord_Click;
			// 
			// legendStyleToolStripMenuItem
			// 
			legendStyleToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { legendNormalToolStripMenuItem, legendExtendedToolStripMenuItem, legendHiddenToolStripMenuItem });
			legendStyleToolStripMenuItem.Name = "legendStyleToolStripMenuItem";
			legendStyleToolStripMenuItem.Size = new Size(240, 22);
			legendStyleToolStripMenuItem.Text = "Legend Style";
			// 
			// legendNormalToolStripMenuItem
			// 
			legendNormalToolStripMenuItem.Name = "legendNormalToolStripMenuItem";
			legendNormalToolStripMenuItem.Size = new Size(122, 22);
			legendNormalToolStripMenuItem.Text = "Normal";
			legendNormalToolStripMenuItem.ToolTipText = "Draw the legend on the left hand side of the map.";
			legendNormalToolStripMenuItem.Click += Map_Legend_Normal_Click;
			// 
			// legendExtendedToolStripMenuItem
			// 
			legendExtendedToolStripMenuItem.Name = "legendExtendedToolStripMenuItem";
			legendExtendedToolStripMenuItem.Size = new Size(122, 22);
			legendExtendedToolStripMenuItem.Text = "Extended";
			legendExtendedToolStripMenuItem.ToolTipText = "Draw the legend outside the map, extending the final image to fit the legend.";
			legendExtendedToolStripMenuItem.Click += Map_Legend_Extended_Click;
			// 
			// legendHiddenToolStripMenuItem
			// 
			legendHiddenToolStripMenuItem.Name = "legendHiddenToolStripMenuItem";
			legendHiddenToolStripMenuItem.Size = new Size(122, 22);
			legendHiddenToolStripMenuItem.Text = "Hidden";
			legendHiddenToolStripMenuItem.ToolTipText = "Do not draw a legend.";
			legendHiddenToolStripMenuItem.Click += Map_Legend_Hidden_Click;
			// 
			// loadRecipeToolStripMenuItem
			// 
			loadRecipeToolStripMenuItem.Name = "loadRecipeToolStripMenuItem";
			loadRecipeToolStripMenuItem.Size = new Size(240, 22);
			loadRecipeToolStripMenuItem.Text = "Load Recipe";
			loadRecipeToolStripMenuItem.ToolTipText = "Load an existing Map Recipe.";
			loadRecipeToolStripMenuItem.Click += Map_LoadRecipe_Click;
			// 
			// saveAsRecipeToolStripMenuItem
			// 
			saveAsRecipeToolStripMenuItem.Name = "saveAsRecipeToolStripMenuItem";
			saveAsRecipeToolStripMenuItem.Size = new Size(240, 22);
			saveAsRecipeToolStripMenuItem.Text = "Save as Recipe";
			saveAsRecipeToolStripMenuItem.ToolTipText = "Store your current map plots to a file, to recreate later or share with others.";
			saveAsRecipeToolStripMenuItem.Click += Map_SaveAsRecipe_Click;
			// 
			// exportToFileToolStripMenuItem
			// 
			exportToFileToolStripMenuItem.Name = "exportToFileToolStripMenuItem";
			exportToFileToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
			exportToFileToolStripMenuItem.Size = new Size(240, 22);
			exportToFileToolStripMenuItem.Text = "Save Image";
			exportToFileToolStripMenuItem.ToolTipText = "Save the current map image, with your choice of settings.";
			exportToFileToolStripMenuItem.Click += Map_SaveImage_Click;
			// 
			// quickSaveToolStripMenuItem
			// 
			quickSaveToolStripMenuItem.Name = "quickSaveToolStripMenuItem";
			quickSaveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
			quickSaveToolStripMenuItem.Size = new Size(240, 22);
			quickSaveToolStripMenuItem.Text = "Quick Save Image";
			quickSaveToolStripMenuItem.ToolTipText = "Quickly save the current image, using recommended settings.";
			quickSaveToolStripMenuItem.Click += Map_QuickSaveImage_Click;
			// 
			// clearPlotsToolStripMenuItem
			// 
			clearPlotsToolStripMenuItem.Name = "clearPlotsToolStripMenuItem";
			clearPlotsToolStripMenuItem.Size = new Size(240, 22);
			clearPlotsToolStripMenuItem.Text = "Clear Plots";
			clearPlotsToolStripMenuItem.ToolTipText = "Clear just the currently plotted items.";
			clearPlotsToolStripMenuItem.Click += Map_ClearPlots_Click;
			// 
			// resetToolStripMenuItem
			// 
			resetToolStripMenuItem.Name = "resetToolStripMenuItem";
			resetToolStripMenuItem.Size = new Size(240, 22);
			resetToolStripMenuItem.Text = "Reset";
			resetToolStripMenuItem.ToolTipText = "Reset all map-related settings (those in this menu), currently selected plots, and the chosen location.";
			resetToolStripMenuItem.Click += Map_Reset_Click;
			// 
			// searchSettingsToolStripMenuItem
			// 
			searchSettingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { searchInAllSpacesToolStripMenuItem, searchInInstancesOnlyToolStripMenuItem, advancedModeToolStripMenuItem });
			searchSettingsToolStripMenuItem.Name = "searchSettingsToolStripMenuItem";
			searchSettingsToolStripMenuItem.Size = new Size(99, 20);
			searchSettingsToolStripMenuItem.Text = "Search Settings";
			// 
			// searchInAllSpacesToolStripMenuItem
			// 
			searchInAllSpacesToolStripMenuItem.Name = "searchInAllSpacesToolStripMenuItem";
			searchInAllSpacesToolStripMenuItem.Size = new Size(200, 22);
			searchInAllSpacesToolStripMenuItem.Text = "Search in all Spaces";
			searchInAllSpacesToolStripMenuItem.ToolTipText = "Constrain the search to the selected Space only, or search all Spaces.";
			searchInAllSpacesToolStripMenuItem.Click += Search_SearchInAllSpaces_Click;
			// 
			// searchInInstancesOnlyToolStripMenuItem
			// 
			searchInInstancesOnlyToolStripMenuItem.Name = "searchInInstancesOnlyToolStripMenuItem";
			searchInInstancesOnlyToolStripMenuItem.Size = new Size(200, 22);
			searchInInstancesOnlyToolStripMenuItem.Text = "Search in instances only";
			searchInInstancesOnlyToolStripMenuItem.ToolTipText = "Only show results for spaces which are (or may be) instances.";
			searchInInstancesOnlyToolStripMenuItem.Click += Search_SearchInInstancesOnly_click;
			// 
			// advancedModeToolStripMenuItem
			// 
			advancedModeToolStripMenuItem.Name = "advancedModeToolStripMenuItem";
			advancedModeToolStripMenuItem.Size = new Size(200, 22);
			advancedModeToolStripMenuItem.Text = "Advanced Mode";
			advancedModeToolStripMenuItem.ToolTipText = "Toggle displaying search results in technical formats, and enable searching by FormID.";
			advancedModeToolStripMenuItem.Click += Search_AdvancedMode_Click;
			// 
			// plotSettingsMenuItem
			// 
			plotSettingsMenuItem.DropDownItems.AddRange(new ToolStripItem[] { plotModeMenuItem, plotStylesToolStripMenuItem, heatmapSettingsToolStripMenuItem, clusterSettingsToolStripMenuItem, volumeDrawStyleToolStripMenuItem, showPlotsInOtherSpacesToolStripMenuItem, showRegionLevelsToolStripMenuItem, drawInstanceFormIDToolStripMenuItem });
			plotSettingsMenuItem.Name = "plotSettingsMenuItem";
			plotSettingsMenuItem.Size = new Size(85, 20);
			plotSettingsMenuItem.Text = "Plot Settings";
			// 
			// plotModeMenuItem
			// 
			plotModeMenuItem.DropDownItems.AddRange(new ToolStripItem[] { plotModeStandardToolStripMenuItem, plotModeTopographicToolStripMenuItem, plotModeHeatmapToolStripMenuItem, plotModeClusterToolStripMenuItem });
			plotModeMenuItem.Name = "plotModeMenuItem";
			plotModeMenuItem.Size = new Size(240, 22);
			plotModeMenuItem.Text = "Plot Mode";
			// 
			// plotModeStandardToolStripMenuItem
			// 
			plotModeStandardToolStripMenuItem.Name = "plotModeStandardToolStripMenuItem";
			plotModeStandardToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.S;
			plotModeStandardToolStripMenuItem.Size = new Size(178, 22);
			plotModeStandardToolStripMenuItem.Text = "Standard";
			plotModeStandardToolStripMenuItem.ToolTipText = "Use icons to plot the location of each mapped entity.";
			plotModeStandardToolStripMenuItem.Click += Plot_Mode_Standard_Click;
			// 
			// plotModeTopographicToolStripMenuItem
			// 
			plotModeTopographicToolStripMenuItem.Name = "plotModeTopographicToolStripMenuItem";
			plotModeTopographicToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.T;
			plotModeTopographicToolStripMenuItem.Size = new Size(178, 22);
			plotModeTopographicToolStripMenuItem.Text = "Topographic";
			plotModeTopographicToolStripMenuItem.ToolTipText = "Similar to standard, but color is used to indicate the height of plots against a scale.";
			plotModeTopographicToolStripMenuItem.Click += Plot_Mode_Topographic_Click;
			// 
			// plotModeHeatmapToolStripMenuItem
			// 
			plotModeHeatmapToolStripMenuItem.Name = "plotModeHeatmapToolStripMenuItem";
			plotModeHeatmapToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.H;
			plotModeHeatmapToolStripMenuItem.Size = new Size(178, 22);
			plotModeHeatmapToolStripMenuItem.Text = "Heatmap";
			plotModeHeatmapToolStripMenuItem.ToolTipText = "Shows plotted items in a heatmap.";
			plotModeHeatmapToolStripMenuItem.Click += Plot_Mode_Heatmap_Click;
			// 
			// plotModeClusterToolStripMenuItem
			// 
			plotModeClusterToolStripMenuItem.Name = "plotModeClusterToolStripMenuItem";
			plotModeClusterToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.C;
			plotModeClusterToolStripMenuItem.Size = new Size(178, 22);
			plotModeClusterToolStripMenuItem.Text = "Cluster";
			plotModeClusterToolStripMenuItem.ToolTipText = "Shows the areas with the highest densities of your selected items - customizable.";
			plotModeClusterToolStripMenuItem.Click += Plot_Mode_Cluster_Click;
			// 
			// plotStylesToolStripMenuItem
			// 
			plotStylesToolStripMenuItem.Name = "plotStylesToolStripMenuItem";
			plotStylesToolStripMenuItem.Size = new Size(240, 22);
			plotStylesToolStripMenuItem.Text = "Plot Styles";
			plotStylesToolStripMenuItem.ToolTipText = "Edit color palettes and default plot icon settings.";
			plotStylesToolStripMenuItem.Click += Plot_PlotStyles_Click;
			// 
			// heatmapSettingsToolStripMenuItem
			// 
			heatmapSettingsToolStripMenuItem.Name = "heatmapSettingsToolStripMenuItem";
			heatmapSettingsToolStripMenuItem.Size = new Size(240, 22);
			heatmapSettingsToolStripMenuItem.Text = "Heatmap Settings";
			heatmapSettingsToolStripMenuItem.ToolTipText = "Adjust the range and intensity of heatmaps.";
			heatmapSettingsToolStripMenuItem.Click += Plot_HeatmapSettings_Click;
			// 
			// clusterSettingsToolStripMenuItem
			// 
			clusterSettingsToolStripMenuItem.Name = "clusterSettingsToolStripMenuItem";
			clusterSettingsToolStripMenuItem.Size = new Size(240, 22);
			clusterSettingsToolStripMenuItem.Text = "Cluster Settings";
			clusterSettingsToolStripMenuItem.ToolTipText = "Adjust settings relevant to Cluster plot mode.";
			clusterSettingsToolStripMenuItem.Click += Plot_ClusterSettings_Click;
			// 
			// volumeDrawStyleToolStripMenuItem
			// 
			volumeDrawStyleToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { volumeFillToolStripMenuItem, volumeBorderToolStripMenuItem, volumeBothToolStripMenuItem });
			volumeDrawStyleToolStripMenuItem.Name = "volumeDrawStyleToolStripMenuItem";
			volumeDrawStyleToolStripMenuItem.Size = new Size(240, 22);
			volumeDrawStyleToolStripMenuItem.Text = "Volume Draw Style";
			// 
			// volumeFillToolStripMenuItem
			// 
			volumeFillToolStripMenuItem.Name = "volumeFillToolStripMenuItem";
			volumeFillToolStripMenuItem.Size = new Size(109, 22);
			volumeFillToolStripMenuItem.Text = "Fill";
			volumeFillToolStripMenuItem.ToolTipText = "Draw the full volume of areas.";
			volumeFillToolStripMenuItem.Click += Plot_Volume_Fill_Click;
			// 
			// volumeBorderToolStripMenuItem
			// 
			volumeBorderToolStripMenuItem.Name = "volumeBorderToolStripMenuItem";
			volumeBorderToolStripMenuItem.Size = new Size(109, 22);
			volumeBorderToolStripMenuItem.Text = "Border";
			volumeBorderToolStripMenuItem.ToolTipText = "Draw an outline only for areas.";
			volumeBorderToolStripMenuItem.Click += Plot_Volume_Border_Click;
			// 
			// volumeBothToolStripMenuItem
			// 
			volumeBothToolStripMenuItem.Name = "volumeBothToolStripMenuItem";
			volumeBothToolStripMenuItem.Size = new Size(109, 22);
			volumeBothToolStripMenuItem.Text = "Both";
			volumeBothToolStripMenuItem.ToolTipText = "Draw the full volume and add a border to areas.";
			volumeBothToolStripMenuItem.Click += Plot_Volume_Both_Click;
			// 
			// showPlotsInOtherSpacesToolStripMenuItem
			// 
			showPlotsInOtherSpacesToolStripMenuItem.Name = "showPlotsInOtherSpacesToolStripMenuItem";
			showPlotsInOtherSpacesToolStripMenuItem.Size = new Size(240, 22);
			showPlotsInOtherSpacesToolStripMenuItem.Text = "Auto-find items in other spaces";
			showPlotsInOtherSpacesToolStripMenuItem.ToolTipText = "Automatically plot where spaces reachable from this one contain more of the plotted item.";
			showPlotsInOtherSpacesToolStripMenuItem.Click += Plot_ShowPlotsInOtherSpaces;
			// 
			// showRegionLevelsToolStripMenuItem
			// 
			showRegionLevelsToolStripMenuItem.Name = "showRegionLevelsToolStripMenuItem";
			showRegionLevelsToolStripMenuItem.Size = new Size(240, 22);
			showRegionLevelsToolStripMenuItem.Text = "Show Region Levels";
			showRegionLevelsToolStripMenuItem.ToolTipText = "Show the level range for plotted regions.";
			showRegionLevelsToolStripMenuItem.Click += Plot_ShowRegionLevels_Click;
			// 
			// drawInstanceFormIDToolStripMenuItem
			// 
			drawInstanceFormIDToolStripMenuItem.Name = "drawInstanceFormIDToolStripMenuItem";
			drawInstanceFormIDToolStripMenuItem.Size = new Size(240, 22);
			drawInstanceFormIDToolStripMenuItem.Text = "Show Instance FormID";
			drawInstanceFormIDToolStripMenuItem.ToolTipText = "(Advanced) Show the Form ID of the instance against its plot. Only applies to Standard or Topographic plot mode.";
			drawInstanceFormIDToolStripMenuItem.Click += Plot_DrawInstanceFormIDs_Click;
			// 
			// helpToolStripMenuItem
			// 
			helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem, openUserGuidesToolStripMenuItem, checkForUpdatesToolStripMenuItem, installSpotlightToolStripMenuItem, viewGitHubToolStripMenuItem, resetEverythingToolStripMenuItem });
			helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			helpToolStripMenuItem.ShortcutKeyDisplayString = "";
			helpToolStripMenuItem.Size = new Size(44, 20);
			helpToolStripMenuItem.Text = "Help";
			// 
			// aboutToolStripMenuItem
			// 
			aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			aboutToolStripMenuItem.Size = new Size(171, 22);
			aboutToolStripMenuItem.Text = "About";
			aboutToolStripMenuItem.ToolTipText = "View additional info about the application.";
			aboutToolStripMenuItem.Click += Help_About_Click;
			// 
			// openUserGuidesToolStripMenuItem
			// 
			openUserGuidesToolStripMenuItem.Name = "openUserGuidesToolStripMenuItem";
			openUserGuidesToolStripMenuItem.Size = new Size(171, 22);
			openUserGuidesToolStripMenuItem.Text = "Open User Guides";
			openUserGuidesToolStripMenuItem.ToolTipText = "Read documentation online.";
			openUserGuidesToolStripMenuItem.Click += Help_UserGuides_Click;
			// 
			// checkForUpdatesToolStripMenuItem
			// 
			checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
			checkForUpdatesToolStripMenuItem.Size = new Size(171, 22);
			checkForUpdatesToolStripMenuItem.Text = "Check for Updates";
			checkForUpdatesToolStripMenuItem.ToolTipText = "Check if there is a new version available.";
			checkForUpdatesToolStripMenuItem.Click += Help_CheckForUpdates_Click;
			// 
			// installSpotlightToolStripMenuItem
			// 
			installSpotlightToolStripMenuItem.Name = "installSpotlightToolStripMenuItem";
			installSpotlightToolStripMenuItem.Size = new Size(171, 22);
			installSpotlightToolStripMenuItem.Text = "Install Spotlight";
			installSpotlightToolStripMenuItem.ToolTipText = "View a guide on installing the Spotlight feature.";
			installSpotlightToolStripMenuItem.Click += Help_InstallSpotlight_Click;
			// 
			// viewGitHubToolStripMenuItem
			// 
			viewGitHubToolStripMenuItem.Name = "viewGitHubToolStripMenuItem";
			viewGitHubToolStripMenuItem.Size = new Size(171, 22);
			viewGitHubToolStripMenuItem.Text = "View GitHub";
			viewGitHubToolStripMenuItem.ToolTipText = "Go to the project home, or see the open source code.";
			viewGitHubToolStripMenuItem.Click += Help_ViewGitHub_Click;
			// 
			// resetEverythingToolStripMenuItem
			// 
			resetEverythingToolStripMenuItem.Name = "resetEverythingToolStripMenuItem";
			resetEverythingToolStripMenuItem.Size = new Size(171, 22);
			resetEverythingToolStripMenuItem.Text = "Reset Everything";
			resetEverythingToolStripMenuItem.ToolTipText = "Reset every application setting.";
			resetEverythingToolStripMenuItem.Click += Help_ResetEverything_Click;
			// 
			// joinTheDiscordToolStripMenuItem
			// 
			joinTheDiscordToolStripMenuItem.Name = "joinTheDiscordToolStripMenuItem";
			joinTheDiscordToolStripMenuItem.Size = new Size(103, 20);
			joinTheDiscordToolStripMenuItem.Text = "Join the Discord";
			joinTheDiscordToolStripMenuItem.ToolTipText = "Join the official Discord for help and discussion.";
			joinTheDiscordToolStripMenuItem.Click += Discord_Click;
			// 
			// sayThanksToolStripMenuItem
			// 
			sayThanksToolStripMenuItem.Name = "sayThanksToolStripMenuItem";
			sayThanksToolStripMenuItem.Size = new Size(140, 20);
			sayThanksToolStripMenuItem.Text = "Support the Developer ";
			sayThanksToolStripMenuItem.ToolTipText = "Say thanks through PayPal.";
			sayThanksToolStripMenuItem.Click += Donate_Click;
			// 
			// buttonSearch
			// 
			buttonSearch.Anchor = AnchorStyles.Top;
			buttonSearch.Location = new Point(408, 229);
			buttonSearch.Name = "buttonSearch";
			buttonSearch.Size = new Size(168, 21);
			buttonSearch.TabIndex = 3;
			buttonSearch.Text = "Search";
			buttonSearch.UseVisualStyleBackColor = true;
			buttonSearch.Click += ButtonSearch_Click;
			// 
			// textBoxSearch
			// 
			textBoxSearch.Anchor = AnchorStyles.Top;
			textBoxSearch.Location = new Point(12, 229);
			textBoxSearch.Name = "textBoxSearch";
			textBoxSearch.Size = new Size(390, 23);
			textBoxSearch.TabIndex = 2;
			textBoxSearch.Text = "Search Term";
			textBoxSearch.TextChanged += SearchTerm_TextChanged;
			// 
			// dataGridViewSearchResults
			// 
			dataGridViewSearchResults.AllowUserToAddRows = false;
			dataGridViewSearchResults.AllowUserToDeleteRows = false;
			dataGridViewSearchResults.AllowUserToOrderColumns = true;
			dataGridViewSearchResults.AllowUserToResizeRows = false;
			dataGridViewSearchResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			dataGridViewSearchResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dataGridViewSearchResults.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			dataGridViewSearchResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewSearchResults.EditMode = DataGridViewEditMode.EditProgrammatically;
			dataGridViewSearchResults.Location = new Point(12, 273);
			dataGridViewSearchResults.Name = "dataGridViewSearchResults";
			dataGridViewSearchResults.ReadOnly = true;
			dataGridViewSearchResults.ScrollBars = ScrollBars.Vertical;
			dataGridViewSearchResults.Size = new Size(960, 222);
			dataGridViewSearchResults.TabIndex = 6;
			// 
			// comboBoxSpace
			// 
			comboBoxSpace.Anchor = AnchorStyles.Top;
			comboBoxSpace.DropDownStyle = ComboBoxStyle.DropDownList;
			comboBoxSpace.FormattingEnabled = true;
			comboBoxSpace.Location = new Point(582, 229);
			comboBoxSpace.Name = "comboBoxSpace";
			comboBoxSpace.Size = new Size(390, 23);
			comboBoxSpace.TabIndex = 4;
			comboBoxSpace.SelectionChangeCommitted += ComboBoxSpace_SelectionChangeCommitted;
			// 
			// buttonAddToMap
			// 
			buttonAddToMap.Anchor = AnchorStyles.Bottom;
			buttonAddToMap.ContextMenu = null;
			buttonAddToMap.Location = new Point(363, 501);
			buttonAddToMap.Name = "buttonAddToMap";
			buttonAddToMap.Padding = new Padding(0, 0, 15, 0);
			buttonAddToMap.Size = new Size(126, 23);
			buttonAddToMap.TabIndex = 8;
			buttonAddToMap.Text = "Add to Map";
			buttonAddToMap.UseVisualStyleBackColor = true;
			// 
			// buttonRemoveFromMap
			// 
			buttonRemoveFromMap.Anchor = AnchorStyles.Bottom;
			buttonRemoveFromMap.Location = new Point(495, 501);
			buttonRemoveFromMap.Name = "buttonRemoveFromMap";
			buttonRemoveFromMap.Size = new Size(126, 23);
			buttonRemoveFromMap.TabIndex = 9;
			buttonRemoveFromMap.Text = "Remove from Map";
			buttonRemoveFromMap.UseVisualStyleBackColor = true;
			buttonRemoveFromMap.Click += ButtonRemoveFromMap_Click;
			// 
			// dataGridViewItemsToPlot
			// 
			dataGridViewItemsToPlot.AllowUserToAddRows = false;
			dataGridViewItemsToPlot.AllowUserToDeleteRows = false;
			dataGridViewItemsToPlot.AllowUserToOrderColumns = true;
			dataGridViewItemsToPlot.AllowUserToResizeRows = false;
			dataGridViewItemsToPlot.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			dataGridViewItemsToPlot.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dataGridViewItemsToPlot.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			dataGridViewItemsToPlot.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewItemsToPlot.EditMode = DataGridViewEditMode.EditOnEnter;
			dataGridViewItemsToPlot.Location = new Point(12, 530);
			dataGridViewItemsToPlot.Name = "dataGridViewItemsToPlot";
			dataGridViewItemsToPlot.ScrollBars = ScrollBars.Vertical;
			dataGridViewItemsToPlot.Size = new Size(960, 172);
			dataGridViewItemsToPlot.TabIndex = 10;
			dataGridViewItemsToPlot.CellClick += DataGridViewItemsToPlot_CellClick;
			dataGridViewItemsToPlot.CellPainting += DataGridViewItemsToPlot_CellPainting;
			dataGridViewItemsToPlot.CellValidating += DataGridViewItemsToPlot_CellValidating;
			// 
			// listViewSignature
			// 
			listViewSignature.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			listViewSignature.BackColor = SystemColors.ControlDark;
			listViewSignature.CheckBoxes = true;
			listViewSignature.Location = new Point(3, 16);
			listViewSignature.Margin = new Padding(0);
			listViewSignature.Name = "listViewSignature";
			listViewSignature.Scrollable = false;
			listViewSignature.Size = new Size(692, 135);
			listViewSignature.TabIndex = 0;
			listViewSignature.UseCompatibleStateImageBehavior = false;
			listViewSignature.View = View.List;
			// 
			// listViewLockLevel
			// 
			listViewLockLevel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			listViewLockLevel.BackColor = SystemColors.ControlDark;
			listViewLockLevel.CheckBoxes = true;
			listViewLockLevel.Location = new Point(3, 16);
			listViewLockLevel.Margin = new Padding(0);
			listViewLockLevel.Name = "listViewLockLevel";
			listViewLockLevel.Scrollable = false;
			listViewLockLevel.Size = new Size(253, 135);
			listViewLockLevel.TabIndex = 0;
			listViewLockLevel.UseCompatibleStateImageBehavior = false;
			listViewLockLevel.View = View.List;
			// 
			// groupBoxFilterSignature
			// 
			groupBoxFilterSignature.Anchor = AnchorStyles.Top;
			groupBoxFilterSignature.Controls.Add(buttonSelectRecommended);
			groupBoxFilterSignature.Controls.Add(buttonUnselectAllSignature);
			groupBoxFilterSignature.Controls.Add(buttonSelectAllSignature);
			groupBoxFilterSignature.Controls.Add(listViewSignature);
			groupBoxFilterSignature.Location = new Point(12, 25);
			groupBoxFilterSignature.Name = "groupBoxFilterSignature";
			groupBoxFilterSignature.Size = new Size(698, 183);
			groupBoxFilterSignature.TabIndex = 14;
			groupBoxFilterSignature.TabStop = false;
			groupBoxFilterSignature.Text = "Filter by category";
			// 
			// buttonSelectRecommended
			// 
			buttonSelectRecommended.Anchor = AnchorStyles.Bottom;
			buttonSelectRecommended.Location = new Point(274, 154);
			buttonSelectRecommended.Name = "buttonSelectRecommended";
			buttonSelectRecommended.Size = new Size(150, 23);
			buttonSelectRecommended.TabIndex = 2;
			buttonSelectRecommended.Text = "Select Recommended";
			buttonSelectRecommended.UseVisualStyleBackColor = true;
			buttonSelectRecommended.Click += ButtonSelectRecommended_Click;
			// 
			// buttonUnselectAllSignature
			// 
			buttonUnselectAllSignature.Anchor = AnchorStyles.Bottom;
			buttonUnselectAllSignature.Location = new Point(430, 154);
			buttonUnselectAllSignature.Name = "buttonUnselectAllSignature";
			buttonUnselectAllSignature.Size = new Size(90, 23);
			buttonUnselectAllSignature.TabIndex = 3;
			buttonUnselectAllSignature.Text = "Unselect All";
			buttonUnselectAllSignature.UseVisualStyleBackColor = true;
			buttonUnselectAllSignature.Click += ButtonUnselectAllSignature_Click;
			// 
			// buttonSelectAllSignature
			// 
			buttonSelectAllSignature.Anchor = AnchorStyles.Bottom;
			buttonSelectAllSignature.Location = new Point(178, 154);
			buttonSelectAllSignature.Name = "buttonSelectAllSignature";
			buttonSelectAllSignature.Size = new Size(90, 23);
			buttonSelectAllSignature.TabIndex = 1;
			buttonSelectAllSignature.Text = "Select All";
			buttonSelectAllSignature.UseVisualStyleBackColor = true;
			buttonSelectAllSignature.Click += ButtonSelectAllSignature_Click;
			// 
			// groupBoxFilterLockLevel
			// 
			groupBoxFilterLockLevel.Anchor = AnchorStyles.Top;
			groupBoxFilterLockLevel.Controls.Add(buttonUnselectAllLockLevel);
			groupBoxFilterLockLevel.Controls.Add(buttonSelectAllLockLevel);
			groupBoxFilterLockLevel.Controls.Add(listViewLockLevel);
			groupBoxFilterLockLevel.Location = new Point(713, 25);
			groupBoxFilterLockLevel.Margin = new Padding(0);
			groupBoxFilterLockLevel.Name = "groupBoxFilterLockLevel";
			groupBoxFilterLockLevel.Size = new Size(259, 183);
			groupBoxFilterLockLevel.TabIndex = 15;
			groupBoxFilterLockLevel.TabStop = false;
			groupBoxFilterLockLevel.Text = "Filter by lock level";
			// 
			// buttonUnselectAllLockLevel
			// 
			buttonUnselectAllLockLevel.Anchor = AnchorStyles.Bottom;
			buttonUnselectAllLockLevel.Location = new Point(132, 154);
			buttonUnselectAllLockLevel.Name = "buttonUnselectAllLockLevel";
			buttonUnselectAllLockLevel.Size = new Size(90, 23);
			buttonUnselectAllLockLevel.TabIndex = 2;
			buttonUnselectAllLockLevel.Text = "Unselect All";
			buttonUnselectAllLockLevel.UseVisualStyleBackColor = true;
			buttonUnselectAllLockLevel.Click += ButtonUnselectAllLockLevel_Click;
			// 
			// buttonSelectAllLockLevel
			// 
			buttonSelectAllLockLevel.Anchor = AnchorStyles.Bottom;
			buttonSelectAllLockLevel.Location = new Point(36, 154);
			buttonSelectAllLockLevel.Name = "buttonSelectAllLockLevel";
			buttonSelectAllLockLevel.Size = new Size(90, 23);
			buttonSelectAllLockLevel.TabIndex = 1;
			buttonSelectAllLockLevel.Text = "Select All";
			buttonSelectAllLockLevel.UseVisualStyleBackColor = true;
			buttonSelectAllLockLevel.Click += ButtonSelectAllLockLevel_Click;
			// 
			// buttonUpdateMap
			// 
			buttonUpdateMap.Anchor = AnchorStyles.Bottom;
			buttonUpdateMap.Location = new Point(415, 708);
			buttonUpdateMap.Name = "buttonUpdateMap";
			buttonUpdateMap.Size = new Size(155, 27);
			buttonUpdateMap.TabIndex = 11;
			buttonUpdateMap.Text = "Update Map";
			buttonUpdateMap.UseVisualStyleBackColor = true;
			buttonUpdateMap.Click += ButtonUpdateMap_Click;
			// 
			// progressBarMain
			// 
			progressBarMain.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			progressBarMain.Location = new Point(-2, 741);
			progressBarMain.Name = "progressBarMain";
			progressBarMain.Size = new Size(988, 22);
			progressBarMain.TabIndex = 12;
			// 
			// labelProgressStatus
			// 
			labelProgressStatus.Anchor = AnchorStyles.Bottom;
			labelProgressStatus.AutoSize = true;
			labelProgressStatus.BackColor = Color.FromArgb(230, 230, 230);
			labelProgressStatus.Location = new Point(466, 742);
			labelProgressStatus.Name = "labelProgressStatus";
			labelProgressStatus.Size = new Size(52, 15);
			labelProgressStatus.TabIndex = 13;
			labelProgressStatus.Text = "Progress";
			labelProgressStatus.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// labelSearchResults
			// 
			labelSearchResults.AutoSize = true;
			labelSearchResults.Location = new Point(12, 255);
			labelSearchResults.Name = "labelSearchResults";
			labelSearchResults.Size = new Size(79, 15);
			labelSearchResults.TabIndex = 5;
			labelSearchResults.Text = "Search results";
			// 
			// labelItemsToPlot
			// 
			labelItemsToPlot.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			labelItemsToPlot.AutoSize = true;
			labelItemsToPlot.Location = new Point(12, 512);
			labelItemsToPlot.Name = "labelItemsToPlot";
			labelItemsToPlot.Size = new Size(120, 15);
			labelItemsToPlot.TabIndex = 7;
			labelItemsToPlot.Text = "Items selected to plot";
			// 
			// labelSearchTerm
			// 
			labelSearchTerm.Anchor = AnchorStyles.Top;
			labelSearchTerm.AutoSize = true;
			labelSearchTerm.Location = new Point(12, 211);
			labelSearchTerm.Name = "labelSearchTerm";
			labelSearchTerm.Size = new Size(72, 15);
			labelSearchTerm.TabIndex = 1;
			labelSearchTerm.Text = "Search Term";
			// 
			// labelSelectedSpace
			// 
			labelSelectedSpace.Anchor = AnchorStyles.Top;
			labelSelectedSpace.AutoSize = true;
			labelSelectedSpace.Location = new Point(582, 211);
			labelSelectedSpace.Name = "labelSelectedSpace";
			labelSelectedSpace.Size = new Size(85, 15);
			labelSelectedSpace.TabIndex = 16;
			labelSelectedSpace.Text = "Selected Space";
			// 
			// panelBackground
			// 
			panelBackground.AutoScroll = true;
			panelBackground.Controls.Add(labelSearchTerm);
			panelBackground.Controls.Add(labelItemsToPlot);
			panelBackground.Controls.Add(labelSelectedSpace);
			panelBackground.Controls.Add(textBoxSearch);
			panelBackground.Controls.Add(labelProgressStatus);
			panelBackground.Controls.Add(buttonSearch);
			panelBackground.Controls.Add(comboBoxSpace);
			panelBackground.Controls.Add(buttonUpdateMap);
			panelBackground.Controls.Add(buttonRemoveFromMap);
			panelBackground.Controls.Add(dataGridViewSearchResults);
			panelBackground.Controls.Add(buttonAddToMap);
			panelBackground.Controls.Add(dataGridViewItemsToPlot);
			panelBackground.Controls.Add(groupBoxFilterLockLevel);
			panelBackground.Controls.Add(progressBarMain);
			panelBackground.Controls.Add(groupBoxFilterSignature);
			panelBackground.Controls.Add(labelSearchResults);
			panelBackground.Controls.Add(panelAutoScrollTrigger);
			panelBackground.Dock = DockStyle.Fill;
			panelBackground.Location = new Point(0, 0);
			panelBackground.Margin = new Padding(0);
			panelBackground.Name = "panelBackground";
			panelBackground.Size = new Size(984, 761);
			panelBackground.TabIndex = 17;
			// 
			// panelAutoScrollTrigger
			// 
			panelAutoScrollTrigger.Location = new Point(0, 25);
			panelAutoScrollTrigger.Name = "panelAutoScrollTrigger";
			panelAutoScrollTrigger.Size = new Size(972, 724);
			panelAutoScrollTrigger.TabIndex = 17;
			// 
			// FormMain
			// 
			AcceptButton = buttonSearch;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			AutoScroll = true;
			BackColor = SystemColors.ControlDarkDark;
			ClientSize = new Size(984, 761);
			Controls.Add(menuStripMain);
			Controls.Add(panelBackground);
			Icon = (Icon)resources.GetObject("$this.Icon");
			MainMenuStrip = menuStripMain;
			Name = "FormMain";
			Text = "Mappalachia";
			FormClosing += FormMain_FormClosing;
			Shown += FormMain_Shown;
			menuStripMain.ResumeLayout(false);
			menuStripMain.PerformLayout();
			((System.ComponentModel.ISupportInitialize)dataGridViewSearchResults).EndInit();
			((System.ComponentModel.ISupportInitialize)dataGridViewItemsToPlot).EndInit();
			groupBoxFilterSignature.ResumeLayout(false);
			groupBoxFilterLockLevel.ResumeLayout(false);
			panelBackground.ResumeLayout(false);
			panelBackground.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private MenuStrip menuStripMain;
		private ToolStripMenuItem mapMenuItem;
		private Button buttonSearch;
		private TextBox textBoxSearch;
		private DataGridView dataGridViewSearchResults;
		private ToolStripMenuItem showPreviewToolStripMenuItem;
		private ToolStripMenuItem joinTheDiscordToolStripMenuItem;
		private ToolStripMenuItem helpToolStripMenuItem;
		private ToolStripMenuItem aboutToolStripMenuItem;
		private ToolStripMenuItem checkForUpdatesToolStripMenuItem;
		private ToolStripMenuItem openUserGuidesToolStripMenuItem;
		private ToolStripMenuItem viewGitHubToolStripMenuItem;
		private ToolStripMenuItem sayThanksToolStripMenuItem;
		private ToolStripMenuItem grayscaleToolStripMenuItem;
		private ToolStripMenuItem highlightWaterToolStripMenuItem;
		private ToolStripMenuItem mapMapMarkersToolStripMenuItem;
		private ToolStripMenuItem mapMarkerIconsToolStripMenuItem;
		private ToolStripMenuItem mapMarkerLabelsToolStripMenuItem;
		private ToolStripMenuItem resetToolStripMenuItem;
		private ToolStripMenuItem backgroundImageMenuItem;
		private ToolStripMenuItem backgroundNormalToolStripMenuItem;
		private ToolStripMenuItem backgroundSatelliteToolStripMenuItem;
		private ToolStripMenuItem backgroundMilitaryToolStripMenuItem;
		private ToolStripMenuItem backgroundNoneToolStripMenuItem;
		private ToolStripMenuItem legendStyleToolStripMenuItem;
		private ToolStripMenuItem legendNormalToolStripMenuItem;
		private ToolStripMenuItem legendExtendedToolStripMenuItem;
		private ToolStripMenuItem legendHiddenToolStripMenuItem;
		private ToolStripMenuItem clearPlotsToolStripMenuItem;
		private ToolStripMenuItem quickSaveToolStripMenuItem;
		private ToolStripMenuItem exportToFileToolStripMenuItem;
		private ComboBox comboBoxSpace;
		private ToolStripMenuItem searchSettingsToolStripMenuItem;
		private ToolStripMenuItem searchInAllSpacesToolStripMenuItem;
		private DropDownButton buttonAddToMap;
		private Button buttonRemoveFromMap;
		private DataGridView dataGridViewItemsToPlot;
		private ToolStripMenuItem advancedModeToolStripMenuItem;
		private ToolStripMenuItem resetEverythingToolStripMenuItem;
		private ToolStripMenuItem setTitleToolStripMenuItem;
		private ToolStripMenuItem openExternallyToolStripMenuItem;
		private ListView listViewSignature;
		private ListView listViewLockLevel;
		private GroupBox groupBoxFilterSignature;
		private GroupBox groupBoxFilterLockLevel;
		private Button buttonSelectRecommended;
		private Button buttonUnselectAllSignature;
		private Button buttonSelectAllSignature;
		private Button buttonUnselectAllLockLevel;
		private Button buttonSelectAllLockLevel;
		private ToolStripMenuItem setBrightnessToolStripMenuItem;
		private ToolStripMenuItem plotSettingsMenuItem;
		private ToolStripMenuItem plotModeMenuItem;
		private ToolStripMenuItem plotModeStandardToolStripMenuItem;
		private ToolStripMenuItem plotModeTopographicToolStripMenuItem;
		private ToolStripMenuItem plotModeClusterToolStripMenuItem;
		private ToolStripMenuItem volumeDrawStyleToolStripMenuItem;
		private ToolStripMenuItem volumeFillToolStripMenuItem;
		private ToolStripMenuItem volumeBorderToolStripMenuItem;
		private ToolStripMenuItem volumeBothToolStripMenuItem;
		private ToolStripMenuItem clusterSettingsToolStripMenuItem;
		private ToolStripMenuItem drawInstanceFormIDToolStripMenuItem;
		private ToolStripMenuItem showPlotsInOtherSpacesToolStripMenuItem;
		private ToolStripMenuItem showRegionLevelsToolStripMenuItem;
		private Button buttonUpdateMap;
		private ToolStripMenuItem spotlightToolStripMenuItem;
		private ToolStripMenuItem spotlightEnabledToolStripMenuItem;
		private ToolStripMenuItem spotlightSetRangeToolStripMenuItem;
		private ToolStripMenuItem spotlightCoordToolStripMenuItem;
		private ProgressBar progressBarMain;
		private Label labelProgressStatus;
		private Label labelSearchResults;
		private Label labelItemsToPlot;
		private ToolStripMenuItem installSpotlightToolStripMenuItem;
		private ToolStripMenuItem searchInInstancesOnlyToolStripMenuItem;
		private ToolStripMenuItem fontSizesToolStripMenuItem;
		private ToolStripMenuItem showCompassToolStripMenuItem;
		private ToolStripMenuItem compassAlwaysToolStripMenuItem;
		private ToolStripMenuItem compassWhenUsefulToolStripMenuItem;
		private ToolStripMenuItem compassNeverToolStripMenuItem;
		private ToolStripMenuItem plotStylesToolStripMenuItem;
		private ToolStripMenuItem saveAsRecipeToolStripMenuItem;
		private ToolStripMenuItem loadRecipeToolStripMenuItem;
		private ToolStripMenuItem plotModeHeatmapToolStripMenuItem;
		private ToolStripMenuItem heatmapSettingsToolStripMenuItem;
		private Label labelSearchTerm;
		private Label labelSelectedSpace;
		private Panel panelBackground;
		private Panel panelAutoScrollTrigger;
	}
}
