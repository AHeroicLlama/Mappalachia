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
			mapMapMarkersToolStripMenuItem = new ToolStripMenuItem();
			mapMarkerIconsToolStripMenuItem = new ToolStripMenuItem();
			mapMarkerLabelsToolStripMenuItem = new ToolStripMenuItem();
			mapBackgroundImageMenuItem = new ToolStripMenuItem();
			backgroundNormalToolStripMenuItem = new ToolStripMenuItem();
			backgroundSatelliteToolStripMenuItem = new ToolStripMenuItem();
			backgroundMilitaryToolStripMenuItem = new ToolStripMenuItem();
			backgroundNoneToolStripMenuItem = new ToolStripMenuItem();
			grayscaleToolStripMenuItem = new ToolStripMenuItem();
			setBrightnessToolStripMenuItem = new ToolStripMenuItem();
			highlightWaterToolStripMenuItem = new ToolStripMenuItem();
			spotlightToolStripMenuItem = new ToolStripMenuItem();
			spotlightEnabledToolStripMenuItem = new ToolStripMenuItem();
			spotlightSetRangeToolStripMenuItem = new ToolStripMenuItem();
			spotlightCoordToolStripMenuItem = new ToolStripMenuItem();
			mapLegendStyleToolStripMenuItem = new ToolStripMenuItem();
			legendNormalToolStripMenuItem = new ToolStripMenuItem();
			legendExtendedToolStripMenuItem = new ToolStripMenuItem();
			legendHiddenToolStripMenuItem = new ToolStripMenuItem();
			quickSaveToolStripMenuItem = new ToolStripMenuItem();
			exportToFileToolStripMenuItem = new ToolStripMenuItem();
			clearPlotsToolStripMenuItem = new ToolStripMenuItem();
			resetToolStripMenuItem = new ToolStripMenuItem();
			searchSettingsToolStripMenuItem = new ToolStripMenuItem();
			searchInAllSpacesToolStripMenuItem = new ToolStripMenuItem();
			advancedModeToolStripMenuItem = new ToolStripMenuItem();
			plotSettingsMenuItem = new ToolStripMenuItem();
			plotModeMenuItem = new ToolStripMenuItem();
			plotModeStandardToolStripMenuItem = new ToolStripMenuItem();
			plotModeTopographicToolStripMenuItem = new ToolStripMenuItem();
			plotModeClusterToolStripMenuItem = new ToolStripMenuItem();
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
			menuStripMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dataGridViewSearchResults).BeginInit();
			((System.ComponentModel.ISupportInitialize)dataGridViewItemsToPlot).BeginInit();
			groupBoxFilterSignature.SuspendLayout();
			groupBoxFilterLockLevel.SuspendLayout();
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
			menuStripMain.Text = "menuStrip1";
			// 
			// mapMenuItem
			// 
			mapMenuItem.DropDownItems.AddRange(new ToolStripItem[] { showPreviewToolStripMenuItem, openExternallyToolStripMenuItem, setTitleToolStripMenuItem, mapMapMarkersToolStripMenuItem, mapBackgroundImageMenuItem, grayscaleToolStripMenuItem, setBrightnessToolStripMenuItem, highlightWaterToolStripMenuItem, spotlightToolStripMenuItem, mapLegendStyleToolStripMenuItem, quickSaveToolStripMenuItem, exportToFileToolStripMenuItem, clearPlotsToolStripMenuItem, resetToolStripMenuItem });
			mapMenuItem.Name = "mapMenuItem";
			mapMenuItem.Size = new Size(43, 20);
			mapMenuItem.Text = "Map";
			// 
			// showPreviewToolStripMenuItem
			// 
			showPreviewToolStripMenuItem.Name = "showPreviewToolStripMenuItem";
			showPreviewToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Space;
			showPreviewToolStripMenuItem.Size = new Size(233, 22);
			showPreviewToolStripMenuItem.Text = "Show Preview";
			showPreviewToolStripMenuItem.ToolTipText = "Bring up the preview window.";
			showPreviewToolStripMenuItem.Click += Map_ShowPreview_Click;
			// 
			// openExternallyToolStripMenuItem
			// 
			openExternallyToolStripMenuItem.Name = "openExternallyToolStripMenuItem";
			openExternallyToolStripMenuItem.Size = new Size(233, 22);
			openExternallyToolStripMenuItem.Text = "Open Externally";
			openExternallyToolStripMenuItem.ToolTipText = "Open the current map image in your default image viewer.";
			openExternallyToolStripMenuItem.Click += Map_OpenExternally;
			// 
			// setTitleToolStripMenuItem
			// 
			setTitleToolStripMenuItem.Name = "setTitleToolStripMenuItem";
			setTitleToolStripMenuItem.Size = new Size(233, 22);
			setTitleToolStripMenuItem.Text = "Set Title";
			setTitleToolStripMenuItem.ToolTipText = "Set a title for your map.";
			setTitleToolStripMenuItem.Click += Map_SetTitle_Click;
			// 
			// mapMapMarkersToolStripMenuItem
			// 
			mapMapMarkersToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mapMarkerIconsToolStripMenuItem, mapMarkerLabelsToolStripMenuItem });
			mapMapMarkersToolStripMenuItem.Name = "mapMapMarkersToolStripMenuItem";
			mapMapMarkersToolStripMenuItem.Size = new Size(233, 22);
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
			// mapBackgroundImageMenuItem
			// 
			mapBackgroundImageMenuItem.DropDownItems.AddRange(new ToolStripItem[] { backgroundNormalToolStripMenuItem, backgroundSatelliteToolStripMenuItem, backgroundMilitaryToolStripMenuItem, backgroundNoneToolStripMenuItem });
			mapBackgroundImageMenuItem.Name = "mapBackgroundImageMenuItem";
			mapBackgroundImageMenuItem.Size = new Size(233, 22);
			mapBackgroundImageMenuItem.Text = "Background Image";
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
			grayscaleToolStripMenuItem.Size = new Size(233, 22);
			grayscaleToolStripMenuItem.Text = "Grayscale Background";
			grayscaleToolStripMenuItem.ToolTipText = "Set the background to black and white, making plots more visible.";
			grayscaleToolStripMenuItem.Click += Map_Grayscale_Click;
			// 
			// setBrightnessToolStripMenuItem
			// 
			setBrightnessToolStripMenuItem.Name = "setBrightnessToolStripMenuItem";
			setBrightnessToolStripMenuItem.Size = new Size(233, 22);
			setBrightnessToolStripMenuItem.Text = "Set Brightness";
			setBrightnessToolStripMenuItem.Click += Map_SetBrightness_Click;
			// 
			// highlightWaterToolStripMenuItem
			// 
			highlightWaterToolStripMenuItem.Name = "highlightWaterToolStripMenuItem";
			highlightWaterToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.H;
			highlightWaterToolStripMenuItem.Size = new Size(233, 22);
			highlightWaterToolStripMenuItem.Text = "Highlight Water";
			highlightWaterToolStripMenuItem.ToolTipText = "Overlay a blue highlight showing accessible surface water.";
			highlightWaterToolStripMenuItem.Click += Map_HightlightWater_Click;
			// 
			// spotlightToolStripMenuItem
			// 
			spotlightToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { spotlightEnabledToolStripMenuItem, spotlightSetRangeToolStripMenuItem, spotlightCoordToolStripMenuItem });
			spotlightToolStripMenuItem.Name = "spotlightToolStripMenuItem";
			spotlightToolStripMenuItem.Size = new Size(233, 22);
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
			// mapLegendStyleToolStripMenuItem
			// 
			mapLegendStyleToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { legendNormalToolStripMenuItem, legendExtendedToolStripMenuItem, legendHiddenToolStripMenuItem });
			mapLegendStyleToolStripMenuItem.Name = "mapLegendStyleToolStripMenuItem";
			mapLegendStyleToolStripMenuItem.Size = new Size(233, 22);
			mapLegendStyleToolStripMenuItem.Text = "Legend Style";
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
			// quickSaveToolStripMenuItem
			// 
			quickSaveToolStripMenuItem.Name = "quickSaveToolStripMenuItem";
			quickSaveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
			quickSaveToolStripMenuItem.Size = new Size(233, 22);
			quickSaveToolStripMenuItem.Text = "Quick Save";
			quickSaveToolStripMenuItem.ToolTipText = "Quickly save the current image, using recommended settings.";
			quickSaveToolStripMenuItem.Click += Map_QuickSave_Click;
			// 
			// exportToFileToolStripMenuItem
			// 
			exportToFileToolStripMenuItem.Name = "exportToFileToolStripMenuItem";
			exportToFileToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
			exportToFileToolStripMenuItem.Size = new Size(233, 22);
			exportToFileToolStripMenuItem.Text = "Export to File";
			exportToFileToolStripMenuItem.ToolTipText = "Save the current map image, with your choice of settings.";
			exportToFileToolStripMenuItem.Click += Map_ExportToFile_Click;
			// 
			// clearPlotsToolStripMenuItem
			// 
			clearPlotsToolStripMenuItem.Name = "clearPlotsToolStripMenuItem";
			clearPlotsToolStripMenuItem.Size = new Size(233, 22);
			clearPlotsToolStripMenuItem.Text = "Clear Plots";
			clearPlotsToolStripMenuItem.ToolTipText = "Clear just the currently plotted items.";
			clearPlotsToolStripMenuItem.Click += Map_ClearPlots_Click;
			// 
			// resetToolStripMenuItem
			// 
			resetToolStripMenuItem.Name = "resetToolStripMenuItem";
			resetToolStripMenuItem.Size = new Size(233, 22);
			resetToolStripMenuItem.Text = "Reset";
			resetToolStripMenuItem.ToolTipText = "Reset all map-related settings (those in this menu), currently selected plots, and the chosen location.";
			resetToolStripMenuItem.Click += Map_Reset_Click;
			// 
			// searchSettingsToolStripMenuItem
			// 
			searchSettingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { searchInAllSpacesToolStripMenuItem, advancedModeToolStripMenuItem });
			searchSettingsToolStripMenuItem.Name = "searchSettingsToolStripMenuItem";
			searchSettingsToolStripMenuItem.Size = new Size(99, 20);
			searchSettingsToolStripMenuItem.Text = "Search Settings";
			// 
			// searchInAllSpacesToolStripMenuItem
			// 
			searchInAllSpacesToolStripMenuItem.Name = "searchInAllSpacesToolStripMenuItem";
			searchInAllSpacesToolStripMenuItem.Size = new Size(176, 22);
			searchInAllSpacesToolStripMenuItem.Text = "Search in all Spaces";
			searchInAllSpacesToolStripMenuItem.ToolTipText = "Constrain the search to the selected Space only, or search all Spaces.";
			searchInAllSpacesToolStripMenuItem.Click += Search_SearchInAllSpaces_Click;
			// 
			// advancedModeToolStripMenuItem
			// 
			advancedModeToolStripMenuItem.Name = "advancedModeToolStripMenuItem";
			advancedModeToolStripMenuItem.Size = new Size(176, 22);
			advancedModeToolStripMenuItem.Text = "Advanced Mode";
			advancedModeToolStripMenuItem.ToolTipText = "Toggle displaying search results in technical formats, and enable searching by FormID.";
			advancedModeToolStripMenuItem.Click += Search_AdvancedMode_Click;
			// 
			// plotSettingsMenuItem
			// 
			plotSettingsMenuItem.DropDownItems.AddRange(new ToolStripItem[] { plotModeMenuItem, clusterSettingsToolStripMenuItem, volumeDrawStyleToolStripMenuItem, showPlotsInOtherSpacesToolStripMenuItem, showRegionLevelsToolStripMenuItem, drawInstanceFormIDToolStripMenuItem });
			plotSettingsMenuItem.Name = "plotSettingsMenuItem";
			plotSettingsMenuItem.Size = new Size(85, 20);
			plotSettingsMenuItem.Text = "Plot Settings";
			// 
			// plotModeMenuItem
			// 
			plotModeMenuItem.DropDownItems.AddRange(new ToolStripItem[] { plotModeStandardToolStripMenuItem, plotModeTopographicToolStripMenuItem, plotModeClusterToolStripMenuItem });
			plotModeMenuItem.Name = "plotModeMenuItem";
			plotModeMenuItem.Size = new Size(227, 22);
			plotModeMenuItem.Text = "Plot Mode";
			// 
			// plotModeStandardToolStripMenuItem
			// 
			plotModeStandardToolStripMenuItem.Name = "plotModeStandardToolStripMenuItem";
			plotModeStandardToolStripMenuItem.Size = new Size(141, 22);
			plotModeStandardToolStripMenuItem.Text = "Standard";
			plotModeStandardToolStripMenuItem.ToolTipText = "Use icons to plot the location of each mapped entity.";
			plotModeStandardToolStripMenuItem.Click += Plot_Mode_Standard_Click;
			// 
			// plotModeTopographicToolStripMenuItem
			// 
			plotModeTopographicToolStripMenuItem.Name = "plotModeTopographicToolStripMenuItem";
			plotModeTopographicToolStripMenuItem.Size = new Size(141, 22);
			plotModeTopographicToolStripMenuItem.Text = "Topographic";
			plotModeTopographicToolStripMenuItem.ToolTipText = "Similar to standard, but color is used to indicate the height of plots against a scale.";
			plotModeTopographicToolStripMenuItem.Click += Plot_Mode_Topographic_Click;
			// 
			// plotModeClusterToolStripMenuItem
			// 
			plotModeClusterToolStripMenuItem.Name = "plotModeClusterToolStripMenuItem";
			plotModeClusterToolStripMenuItem.Size = new Size(141, 22);
			plotModeClusterToolStripMenuItem.Text = "Cluster";
			plotModeClusterToolStripMenuItem.ToolTipText = "Shows the areas with the highest densities of your selected items - customizable.";
			plotModeClusterToolStripMenuItem.Click += Plot_Mode_Cluster_Click;
			// 
			// clusterSettingsToolStripMenuItem
			// 
			clusterSettingsToolStripMenuItem.Name = "clusterSettingsToolStripMenuItem";
			clusterSettingsToolStripMenuItem.Size = new Size(227, 22);
			clusterSettingsToolStripMenuItem.Text = "Cluster Settings";
			clusterSettingsToolStripMenuItem.ToolTipText = "Adjust settings relevant to Cluster plot mode.";
			clusterSettingsToolStripMenuItem.Click += Plot_ClusterSettings_Click;
			// 
			// volumeDrawStyleToolStripMenuItem
			// 
			volumeDrawStyleToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { volumeFillToolStripMenuItem, volumeBorderToolStripMenuItem, volumeBothToolStripMenuItem });
			volumeDrawStyleToolStripMenuItem.Name = "volumeDrawStyleToolStripMenuItem";
			volumeDrawStyleToolStripMenuItem.Size = new Size(227, 22);
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
			showPlotsInOtherSpacesToolStripMenuItem.Size = new Size(227, 22);
			showPlotsInOtherSpacesToolStripMenuItem.Text = "Show plots in other locations";
			showPlotsInOtherSpacesToolStripMenuItem.ToolTipText = "Add notes to show where other locations (accessible from the current selected one) also contain quantities of your selected items.";
			showPlotsInOtherSpacesToolStripMenuItem.Click += Plot_ShowPlotsInOtherSpaces;
			// 
			// showRegionLevelsToolStripMenuItem
			// 
			showRegionLevelsToolStripMenuItem.Name = "showRegionLevelsToolStripMenuItem";
			showRegionLevelsToolStripMenuItem.Size = new Size(227, 22);
			showRegionLevelsToolStripMenuItem.Text = "Show Region Levels";
			showRegionLevelsToolStripMenuItem.ToolTipText = "Show the level range for plotted regions.";
			showRegionLevelsToolStripMenuItem.Click += Plot_ShowRegionLevels_Click;
			// 
			// drawInstanceFormIDToolStripMenuItem
			// 
			drawInstanceFormIDToolStripMenuItem.Name = "drawInstanceFormIDToolStripMenuItem";
			drawInstanceFormIDToolStripMenuItem.Size = new Size(227, 22);
			drawInstanceFormIDToolStripMenuItem.Text = "Show Instance FormID";
			drawInstanceFormIDToolStripMenuItem.ToolTipText = "(Advanced) Show the Form ID of the instance against its plot.";
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
			aboutToolStripMenuItem.Size = new Size(180, 22);
			aboutToolStripMenuItem.Text = "About";
			aboutToolStripMenuItem.ToolTipText = "View additional info about the application.";
			aboutToolStripMenuItem.Click += Help_About_Click;
			// 
			// openUserGuidesToolStripMenuItem
			// 
			openUserGuidesToolStripMenuItem.Name = "openUserGuidesToolStripMenuItem";
			openUserGuidesToolStripMenuItem.Size = new Size(180, 22);
			openUserGuidesToolStripMenuItem.Text = "Open User Guides";
			openUserGuidesToolStripMenuItem.ToolTipText = "Read documentation online.";
			openUserGuidesToolStripMenuItem.Click += Help_UserGuides_Click;
			// 
			// checkForUpdatesToolStripMenuItem
			// 
			checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
			checkForUpdatesToolStripMenuItem.Size = new Size(180, 22);
			checkForUpdatesToolStripMenuItem.Text = "Check for Updates";
			checkForUpdatesToolStripMenuItem.ToolTipText = "Check if there is a new version available.";
			checkForUpdatesToolStripMenuItem.Click += Help_CheckForUpdates_Click;
			// 
			// installSpotlightToolStripMenuItem
			// 
			installSpotlightToolStripMenuItem.Name = "installSpotlightToolStripMenuItem";
			installSpotlightToolStripMenuItem.Size = new Size(180, 22);
			installSpotlightToolStripMenuItem.Text = "Install Spotlight";
			installSpotlightToolStripMenuItem.ToolTipText = "View a guide on installing the Spotlight feature.";
			installSpotlightToolStripMenuItem.Click += Help_InstallSpotlight_Click;
			// 
			// viewGitHubToolStripMenuItem
			// 
			viewGitHubToolStripMenuItem.Name = "viewGitHubToolStripMenuItem";
			viewGitHubToolStripMenuItem.Size = new Size(180, 22);
			viewGitHubToolStripMenuItem.Text = "View GitHub";
			viewGitHubToolStripMenuItem.ToolTipText = "Go to the project home, or see the open source code.";
			viewGitHubToolStripMenuItem.Click += Help_ViewGitHub_Click;
			// 
			// resetEverythingToolStripMenuItem
			// 
			resetEverythingToolStripMenuItem.Name = "resetEverythingToolStripMenuItem";
			resetEverythingToolStripMenuItem.Size = new Size(180, 22);
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
			buttonSearch.Location = new Point(807, 29);
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
			textBoxSearch.Location = new Point(410, 30);
			textBoxSearch.Name = "textBoxSearch";
			textBoxSearch.Size = new Size(391, 23);
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
			dataGridViewSearchResults.Location = new Point(12, 257);
			dataGridViewSearchResults.MinimumSize = new Size(960, 0);
			dataGridViewSearchResults.Name = "dataGridViewSearchResults";
			dataGridViewSearchResults.ReadOnly = true;
			dataGridViewSearchResults.ScrollBars = ScrollBars.Vertical;
			dataGridViewSearchResults.Size = new Size(963, 237);
			dataGridViewSearchResults.TabIndex = 6;
			// 
			// comboBoxSpace
			// 
			comboBoxSpace.Anchor = AnchorStyles.Top;
			comboBoxSpace.DropDownStyle = ComboBoxStyle.DropDownList;
			comboBoxSpace.FormattingEnabled = true;
			comboBoxSpace.Location = new Point(12, 30);
			comboBoxSpace.Name = "comboBoxSpace";
			comboBoxSpace.Size = new Size(392, 23);
			comboBoxSpace.TabIndex = 1;
			comboBoxSpace.SelectionChangeCommitted += ComboBoxSpace_SelectionChangeCommitted;
			// 
			// buttonAddToMap
			// 
			buttonAddToMap.Anchor = AnchorStyles.Bottom;
			buttonAddToMap.ContextMenu = null;
			buttonAddToMap.Location = new Point(363, 500);
			buttonAddToMap.Name = "buttonAddToMap";
			buttonAddToMap.Padding = new Padding(0, 0, 15, 0);
			buttonAddToMap.Size = new Size(126, 23);
			buttonAddToMap.TabIndex = 7;
			buttonAddToMap.Text = "Add to Map";
			buttonAddToMap.UseVisualStyleBackColor = true;
			// 
			// buttonRemoveFromMap
			// 
			buttonRemoveFromMap.Anchor = AnchorStyles.Bottom;
			buttonRemoveFromMap.Location = new Point(495, 500);
			buttonRemoveFromMap.Name = "buttonRemoveFromMap";
			buttonRemoveFromMap.Size = new Size(126, 23);
			buttonRemoveFromMap.TabIndex = 8;
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
			dataGridViewItemsToPlot.EditMode = DataGridViewEditMode.EditProgrammatically;
			dataGridViewItemsToPlot.Location = new Point(12, 529);
			dataGridViewItemsToPlot.MinimumSize = new Size(960, 170);
			dataGridViewItemsToPlot.Name = "dataGridViewItemsToPlot";
			dataGridViewItemsToPlot.ReadOnly = true;
			dataGridViewItemsToPlot.ScrollBars = ScrollBars.Vertical;
			dataGridViewItemsToPlot.Size = new Size(963, 170);
			dataGridViewItemsToPlot.TabIndex = 9;
			dataGridViewItemsToPlot.CellPainting += DataGridViewItemsToPlot_CellPainting;
			// 
			// listViewSignature
			// 
			listViewSignature.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			listViewSignature.BackColor = SystemColors.ControlDark;
			listViewSignature.CheckBoxes = true;
			listViewSignature.Location = new Point(3, 16);
			listViewSignature.Margin = new Padding(0);
			listViewSignature.Name = "listViewSignature";
			listViewSignature.Size = new Size(693, 135);
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
			listViewLockLevel.Size = new Size(255, 135);
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
			groupBoxFilterSignature.Location = new Point(12, 56);
			groupBoxFilterSignature.Name = "groupBoxFilterSignature";
			groupBoxFilterSignature.Size = new Size(699, 183);
			groupBoxFilterSignature.TabIndex = 4;
			groupBoxFilterSignature.TabStop = false;
			groupBoxFilterSignature.Text = "Filter by category";
			// 
			// buttonSelectRecommended
			// 
			buttonSelectRecommended.Anchor = AnchorStyles.Bottom;
			buttonSelectRecommended.Location = new Point(281, 154);
			buttonSelectRecommended.Name = "buttonSelectRecommended";
			buttonSelectRecommended.Size = new Size(137, 23);
			buttonSelectRecommended.TabIndex = 2;
			buttonSelectRecommended.Text = "Select Recommended";
			buttonSelectRecommended.UseVisualStyleBackColor = true;
			buttonSelectRecommended.Click += ButtonSelectRecommended_Click;
			// 
			// buttonUnselectAllSignature
			// 
			buttonUnselectAllSignature.Anchor = AnchorStyles.Bottom;
			buttonUnselectAllSignature.Location = new Point(424, 154);
			buttonUnselectAllSignature.Name = "buttonUnselectAllSignature";
			buttonUnselectAllSignature.Size = new Size(80, 23);
			buttonUnselectAllSignature.TabIndex = 3;
			buttonUnselectAllSignature.Text = "Unselect All";
			buttonUnselectAllSignature.UseVisualStyleBackColor = true;
			buttonUnselectAllSignature.Click += ButtonUnselectAllSignature_Click;
			// 
			// buttonSelectAllSignature
			// 
			buttonSelectAllSignature.Anchor = AnchorStyles.Bottom;
			buttonSelectAllSignature.Location = new Point(195, 154);
			buttonSelectAllSignature.Name = "buttonSelectAllSignature";
			buttonSelectAllSignature.Size = new Size(80, 23);
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
			groupBoxFilterLockLevel.Location = new Point(714, 56);
			groupBoxFilterLockLevel.Margin = new Padding(0);
			groupBoxFilterLockLevel.Name = "groupBoxFilterLockLevel";
			groupBoxFilterLockLevel.Size = new Size(261, 183);
			groupBoxFilterLockLevel.TabIndex = 5;
			groupBoxFilterLockLevel.TabStop = false;
			groupBoxFilterLockLevel.Text = "Filter by lock level";
			// 
			// buttonUnselectAllLockLevel
			// 
			buttonUnselectAllLockLevel.Anchor = AnchorStyles.Bottom;
			buttonUnselectAllLockLevel.Location = new Point(133, 154);
			buttonUnselectAllLockLevel.Name = "buttonUnselectAllLockLevel";
			buttonUnselectAllLockLevel.Size = new Size(80, 23);
			buttonUnselectAllLockLevel.TabIndex = 2;
			buttonUnselectAllLockLevel.Text = "Unselect All";
			buttonUnselectAllLockLevel.UseVisualStyleBackColor = true;
			buttonUnselectAllLockLevel.Click += ButtonUnselectAllLockLevel_Click;
			// 
			// buttonSelectAllLockLevel
			// 
			buttonSelectAllLockLevel.Anchor = AnchorStyles.Bottom;
			buttonSelectAllLockLevel.Location = new Point(47, 154);
			buttonSelectAllLockLevel.Name = "buttonSelectAllLockLevel";
			buttonSelectAllLockLevel.Size = new Size(80, 23);
			buttonSelectAllLockLevel.TabIndex = 1;
			buttonSelectAllLockLevel.Text = "Select All";
			buttonSelectAllLockLevel.UseVisualStyleBackColor = true;
			buttonSelectAllLockLevel.Click += ButtonSelectAllLockLevel_Click;
			// 
			// buttonUpdateMap
			// 
			buttonUpdateMap.Anchor = AnchorStyles.Bottom;
			buttonUpdateMap.Location = new Point(415, 705);
			buttonUpdateMap.Name = "buttonUpdateMap";
			buttonUpdateMap.Size = new Size(155, 27);
			buttonUpdateMap.TabIndex = 10;
			buttonUpdateMap.Text = "Update Map";
			buttonUpdateMap.UseVisualStyleBackColor = true;
			buttonUpdateMap.Click += ButtonUpdateMap_Click;
			// 
			// progressBarMain
			// 
			progressBarMain.Dock = DockStyle.Bottom;
			progressBarMain.Location = new Point(0, 738);
			progressBarMain.Name = "progressBarMain";
			progressBarMain.Size = new Size(984, 23);
			progressBarMain.TabIndex = 11;
			// 
			// labelProgressStatus
			// 
			labelProgressStatus.Anchor = AnchorStyles.Bottom;
			labelProgressStatus.AutoSize = true;
			labelProgressStatus.BackColor = Color.FromArgb(230, 230, 230);
			labelProgressStatus.Location = new Point(466, 741);
			labelProgressStatus.Name = "labelProgressStatus";
			labelProgressStatus.Size = new Size(52, 15);
			labelProgressStatus.TabIndex = 12;
			labelProgressStatus.Text = "Progress";
			labelProgressStatus.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// labelSearchResults
			// 
			labelSearchResults.AutoSize = true;
			labelSearchResults.Location = new Point(12, 239);
			labelSearchResults.Name = "labelSearchResults";
			labelSearchResults.Size = new Size(79, 15);
			labelSearchResults.TabIndex = 13;
			labelSearchResults.Text = "Search results";
			// 
			// labelItemsToPlot
			// 
			labelItemsToPlot.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			labelItemsToPlot.AutoSize = true;
			labelItemsToPlot.Location = new Point(12, 511);
			labelItemsToPlot.Name = "labelItemsToPlot";
			labelItemsToPlot.Size = new Size(120, 15);
			labelItemsToPlot.TabIndex = 14;
			labelItemsToPlot.Text = "Items selected to plot";
			// 
			// FormMain
			// 
			AcceptButton = buttonSearch;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			AutoScroll = true;
			BackColor = SystemColors.ControlDarkDark;
			ClientSize = new Size(984, 761);
			Controls.Add(labelItemsToPlot);
			Controls.Add(labelSearchResults);
			Controls.Add(labelProgressStatus);
			Controls.Add(progressBarMain);
			Controls.Add(buttonUpdateMap);
			Controls.Add(groupBoxFilterLockLevel);
			Controls.Add(groupBoxFilterSignature);
			Controls.Add(dataGridViewItemsToPlot);
			Controls.Add(buttonRemoveFromMap);
			Controls.Add(buttonAddToMap);
			Controls.Add(comboBoxSpace);
			Controls.Add(buttonSearch);
			Controls.Add(textBoxSearch);
			Controls.Add(dataGridViewSearchResults);
			Controls.Add(menuStripMain);
			Icon = (Icon)resources.GetObject("$this.Icon");
			MainMenuStrip = menuStripMain;
			MinimumSize = new Size(1000, 800);
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
		private ToolStripMenuItem mapBackgroundImageMenuItem;
		private ToolStripMenuItem backgroundNormalToolStripMenuItem;
		private ToolStripMenuItem backgroundSatelliteToolStripMenuItem;
		private ToolStripMenuItem backgroundMilitaryToolStripMenuItem;
		private ToolStripMenuItem backgroundNoneToolStripMenuItem;
		private ToolStripMenuItem mapLegendStyleToolStripMenuItem;
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
	}
}
