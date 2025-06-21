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
			grayscaleMenuItem = new ToolStripMenuItem();
			highlightWaterMenuItem = new ToolStripMenuItem();
			setTitleToolStripMenuItem = new ToolStripMenuItem();
			mapMapMarkersMenuItem = new ToolStripMenuItem();
			mapMarkerIconsMenuItem = new ToolStripMenuItem();
			mapMarkerLabelsMenuItem = new ToolStripMenuItem();
			mapBackgroundImageMenuItem = new ToolStripMenuItem();
			backgroundNormalMenuItem = new ToolStripMenuItem();
			backgroundSatelliteMenuItem = new ToolStripMenuItem();
			backgroundMilitaryMenuItem = new ToolStripMenuItem();
			backgroundNoneMenuItem = new ToolStripMenuItem();
			mapLegendStyleMenuItem = new ToolStripMenuItem();
			legendNormalMenuItem = new ToolStripMenuItem();
			legendExtendedMenuItem = new ToolStripMenuItem();
			legendHiddenMenuItem = new ToolStripMenuItem();
			quickSaveToolStripMenuItem = new ToolStripMenuItem();
			exportToFileToolStripMenuItem = new ToolStripMenuItem();
			clearPlotsToolStripMenuItem = new ToolStripMenuItem();
			resetToolStripMenuItem = new ToolStripMenuItem();
			searchSettingsToolStripMenuItem = new ToolStripMenuItem();
			searchInAllSpacesToolStripMenuItem = new ToolStripMenuItem();
			advancedModeToolStripMenuItem = new ToolStripMenuItem();
			helpToolStripMenuItem = new ToolStripMenuItem();
			aboutToolStripMenuItem = new ToolStripMenuItem();
			openUserGuidesToolStripMenuItem = new ToolStripMenuItem();
			checkForUpdatesToolStripMenuItem = new ToolStripMenuItem();
			viewGitHubToolStripMenuItem = new ToolStripMenuItem();
			resetEverythingToolStripMenuItem = new ToolStripMenuItem();
			joinTheDiscordToolStripMenuItem = new ToolStripMenuItem();
			sayThanksToolStripMenuItem = new ToolStripMenuItem();
			buttonSearch = new Button();
			textBoxSearch = new TextBox();
			dataGridViewSearchResults = new DataGridView();
			comboBoxSpace = new ComboBox();
			buttonAddToMap = new Button();
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
			menuStripMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dataGridViewSearchResults).BeginInit();
			((System.ComponentModel.ISupportInitialize)dataGridViewItemsToPlot).BeginInit();
			groupBoxFilterSignature.SuspendLayout();
			groupBoxFilterLockLevel.SuspendLayout();
			SuspendLayout();
			// 
			// menuStripMain
			// 
			menuStripMain.Items.AddRange(new ToolStripItem[] { mapMenuItem, searchSettingsToolStripMenuItem, helpToolStripMenuItem, joinTheDiscordToolStripMenuItem, sayThanksToolStripMenuItem });
			menuStripMain.Location = new Point(0, 0);
			menuStripMain.Name = "menuStripMain";
			menuStripMain.ShowItemToolTips = true;
			menuStripMain.Size = new Size(977, 24);
			menuStripMain.TabIndex = 0;
			menuStripMain.Text = "menuStrip1";
			// 
			// mapMenuItem
			// 
			mapMenuItem.DropDownItems.AddRange(new ToolStripItem[] { showPreviewToolStripMenuItem, openExternallyToolStripMenuItem, grayscaleMenuItem, highlightWaterMenuItem, setTitleToolStripMenuItem, mapMapMarkersMenuItem, mapBackgroundImageMenuItem, mapLegendStyleMenuItem, quickSaveToolStripMenuItem, exportToFileToolStripMenuItem, clearPlotsToolStripMenuItem, resetToolStripMenuItem });
			mapMenuItem.Name = "mapMenuItem";
			mapMenuItem.Size = new Size(43, 20);
			mapMenuItem.Text = "Map";
			// 
			// showPreviewToolStripMenuItem
			// 
			showPreviewToolStripMenuItem.Name = "showPreviewToolStripMenuItem";
			showPreviewToolStripMenuItem.Size = new Size(191, 22);
			showPreviewToolStripMenuItem.Text = "Show Preview";
			showPreviewToolStripMenuItem.ToolTipText = "Bring up the preview window.";
			showPreviewToolStripMenuItem.Click += Map_ShowPreview_Click;
			// 
			// openExternallyToolStripMenuItem
			// 
			openExternallyToolStripMenuItem.Name = "openExternallyToolStripMenuItem";
			openExternallyToolStripMenuItem.Size = new Size(191, 22);
			openExternallyToolStripMenuItem.Text = "Open Externally";
			openExternallyToolStripMenuItem.ToolTipText = "Open the current map image in your default image viewer.";
			openExternallyToolStripMenuItem.Click += Map_OpenExternally;
			// 
			// grayscaleMenuItem
			// 
			grayscaleMenuItem.Name = "grayscaleMenuItem";
			grayscaleMenuItem.Size = new Size(191, 22);
			grayscaleMenuItem.Text = "Grayscale Background";
			grayscaleMenuItem.ToolTipText = "Set the background to black and white, making plots more visible.";
			grayscaleMenuItem.Click += Map_Grayscale_Click;
			// 
			// highlightWaterMenuItem
			// 
			highlightWaterMenuItem.Name = "highlightWaterMenuItem";
			highlightWaterMenuItem.Size = new Size(191, 22);
			highlightWaterMenuItem.Text = "Highlight Water";
			highlightWaterMenuItem.ToolTipText = "Overlay a blue highlight showing accessible surface water.";
			highlightWaterMenuItem.Click += Map_HightlightWater_Click;
			// 
			// setTitleToolStripMenuItem
			// 
			setTitleToolStripMenuItem.Name = "setTitleToolStripMenuItem";
			setTitleToolStripMenuItem.Size = new Size(191, 22);
			setTitleToolStripMenuItem.Text = "Set Title";
			setTitleToolStripMenuItem.ToolTipText = "Set a title for your map.";
			setTitleToolStripMenuItem.Click += Map_SetTitle_Click;
			// 
			// mapMapMarkersMenuItem
			// 
			mapMapMarkersMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mapMarkerIconsMenuItem, mapMarkerLabelsMenuItem });
			mapMapMarkersMenuItem.Name = "mapMapMarkersMenuItem";
			mapMapMarkersMenuItem.Size = new Size(191, 22);
			mapMapMarkersMenuItem.Text = "Map Markers";
			// 
			// mapMarkerIconsMenuItem
			// 
			mapMarkerIconsMenuItem.Name = "mapMarkerIconsMenuItem";
			mapMarkerIconsMenuItem.Size = new Size(107, 22);
			mapMarkerIconsMenuItem.Text = "Icons";
			mapMarkerIconsMenuItem.ToolTipText = "Display the icons of fast travel locations.";
			mapMarkerIconsMenuItem.Click += Map_MapMarkers_Icons_Click;
			// 
			// mapMarkerLabelsMenuItem
			// 
			mapMarkerLabelsMenuItem.Name = "mapMarkerLabelsMenuItem";
			mapMarkerLabelsMenuItem.Size = new Size(107, 22);
			mapMarkerLabelsMenuItem.Text = "Labels";
			mapMarkerLabelsMenuItem.ToolTipText = "Display the names of the fast travel locations.";
			mapMarkerLabelsMenuItem.Click += Map_MapMarkers_Labels_Click;
			// 
			// mapBackgroundImageMenuItem
			// 
			mapBackgroundImageMenuItem.DropDownItems.AddRange(new ToolStripItem[] { backgroundNormalMenuItem, backgroundSatelliteMenuItem, backgroundMilitaryMenuItem, backgroundNoneMenuItem });
			mapBackgroundImageMenuItem.Name = "mapBackgroundImageMenuItem";
			mapBackgroundImageMenuItem.Size = new Size(191, 22);
			mapBackgroundImageMenuItem.Text = "Background Image";
			// 
			// backgroundNormalMenuItem
			// 
			backgroundNormalMenuItem.Name = "backgroundNormalMenuItem";
			backgroundNormalMenuItem.Size = new Size(115, 22);
			backgroundNormalMenuItem.Text = "Normal";
			backgroundNormalMenuItem.ToolTipText = "Set the background image to the typical pause menu map.";
			backgroundNormalMenuItem.Click += Map_Background_Normal_Click;
			// 
			// backgroundSatelliteMenuItem
			// 
			backgroundSatelliteMenuItem.Name = "backgroundSatelliteMenuItem";
			backgroundSatelliteMenuItem.Size = new Size(115, 22);
			backgroundSatelliteMenuItem.Text = "Satellite";
			backgroundSatelliteMenuItem.ToolTipText = "Set the background image to a top-down render of the in-game world.";
			backgroundSatelliteMenuItem.Click += Map_Background_Satellite_Click;
			// 
			// backgroundMilitaryMenuItem
			// 
			backgroundMilitaryMenuItem.Name = "backgroundMilitaryMenuItem";
			backgroundMilitaryMenuItem.Size = new Size(115, 22);
			backgroundMilitaryMenuItem.Text = "Military";
			backgroundMilitaryMenuItem.ToolTipText = "Set the background image to the military-style topographic map found in train stations.";
			backgroundMilitaryMenuItem.Click += Map_Background_Military_Click;
			// 
			// backgroundNoneMenuItem
			// 
			backgroundNoneMenuItem.Name = "backgroundNoneMenuItem";
			backgroundNoneMenuItem.Size = new Size(115, 22);
			backgroundNoneMenuItem.Text = "None";
			backgroundNoneMenuItem.ToolTipText = "Disable the background map image.";
			backgroundNoneMenuItem.Click += Map_Background_None_Click;
			// 
			// mapLegendStyleMenuItem
			// 
			mapLegendStyleMenuItem.DropDownItems.AddRange(new ToolStripItem[] { legendNormalMenuItem, legendExtendedMenuItem, legendHiddenMenuItem });
			mapLegendStyleMenuItem.Name = "mapLegendStyleMenuItem";
			mapLegendStyleMenuItem.Size = new Size(191, 22);
			mapLegendStyleMenuItem.Text = "Legend Style";
			// 
			// legendNormalMenuItem
			// 
			legendNormalMenuItem.Name = "legendNormalMenuItem";
			legendNormalMenuItem.Size = new Size(122, 22);
			legendNormalMenuItem.Text = "Normal";
			legendNormalMenuItem.ToolTipText = "Draw the legend on the left hand side of the map.";
			legendNormalMenuItem.Click += Map_Legend_Normal_Click;
			// 
			// legendExtendedMenuItem
			// 
			legendExtendedMenuItem.Name = "legendExtendedMenuItem";
			legendExtendedMenuItem.Size = new Size(122, 22);
			legendExtendedMenuItem.Text = "Extended";
			legendExtendedMenuItem.ToolTipText = "Draw the legend outside the map, extending the final image to fit the legend.";
			legendExtendedMenuItem.Click += Map_Legend_Extended_Click;
			// 
			// legendHiddenMenuItem
			// 
			legendHiddenMenuItem.Name = "legendHiddenMenuItem";
			legendHiddenMenuItem.Size = new Size(122, 22);
			legendHiddenMenuItem.Text = "Hidden";
			legendHiddenMenuItem.ToolTipText = "No not draw a legend.";
			legendHiddenMenuItem.Click += Map_Legend_Hidden_Click;
			// 
			// quickSaveToolStripMenuItem
			// 
			quickSaveToolStripMenuItem.Name = "quickSaveToolStripMenuItem";
			quickSaveToolStripMenuItem.Size = new Size(191, 22);
			quickSaveToolStripMenuItem.Text = "Quick Save";
			quickSaveToolStripMenuItem.ToolTipText = "Quickly save the current image, using recommended settings.";
			quickSaveToolStripMenuItem.Click += Map_QuickSave_Click;
			// 
			// exportToFileToolStripMenuItem
			// 
			exportToFileToolStripMenuItem.Name = "exportToFileToolStripMenuItem";
			exportToFileToolStripMenuItem.Size = new Size(191, 22);
			exportToFileToolStripMenuItem.Text = "Export to File";
			exportToFileToolStripMenuItem.ToolTipText = "Save the current map image, with your choice of settings.";
			exportToFileToolStripMenuItem.Click += Map_ExportToFile_Click;
			// 
			// clearPlotsToolStripMenuItem
			// 
			clearPlotsToolStripMenuItem.Name = "clearPlotsToolStripMenuItem";
			clearPlotsToolStripMenuItem.Size = new Size(191, 22);
			clearPlotsToolStripMenuItem.Text = "Clear Plots";
			clearPlotsToolStripMenuItem.ToolTipText = "Clear just the currently plotted items.";
			clearPlotsToolStripMenuItem.Click += Map_ClearPlots_Click;
			// 
			// resetToolStripMenuItem
			// 
			resetToolStripMenuItem.Name = "resetToolStripMenuItem";
			resetToolStripMenuItem.Size = new Size(191, 22);
			resetToolStripMenuItem.Text = "Reset";
			resetToolStripMenuItem.ToolTipText = "Reset all map-related settings (those in this menu), and currently selected plots.";
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
			// helpToolStripMenuItem
			// 
			helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem, openUserGuidesToolStripMenuItem, checkForUpdatesToolStripMenuItem, viewGitHubToolStripMenuItem, resetEverythingToolStripMenuItem });
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
			buttonSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			buttonSearch.Location = new Point(800, 27);
			buttonSearch.Name = "buttonSearch";
			buttonSearch.Size = new Size(165, 23);
			buttonSearch.TabIndex = 5;
			buttonSearch.Text = "Search";
			buttonSearch.UseVisualStyleBackColor = true;
			buttonSearch.Click += ButtonSearch_Click;
			// 
			// textBoxSearch
			// 
			textBoxSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			textBoxSearch.Location = new Point(406, 29);
			textBoxSearch.Name = "textBoxSearch";
			textBoxSearch.Size = new Size(388, 23);
			textBoxSearch.TabIndex = 4;
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
			dataGridViewSearchResults.Location = new Point(12, 238);
			dataGridViewSearchResults.Name = "dataGridViewSearchResults";
			dataGridViewSearchResults.ReadOnly = true;
			dataGridViewSearchResults.RowHeadersVisible = false;
			dataGridViewSearchResults.ScrollBars = ScrollBars.Vertical;
			dataGridViewSearchResults.SelectionMode = DataGridViewSelectionMode.CellSelect;
			dataGridViewSearchResults.Size = new Size(953, 225);
			dataGridViewSearchResults.TabIndex = 3;
			// 
			// comboBoxSpace
			// 
			comboBoxSpace.DropDownStyle = ComboBoxStyle.DropDownList;
			comboBoxSpace.FormattingEnabled = true;
			comboBoxSpace.Location = new Point(12, 29);
			comboBoxSpace.Name = "comboBoxSpace";
			comboBoxSpace.Size = new Size(388, 23);
			comboBoxSpace.TabIndex = 6;
			comboBoxSpace.SelectionChangeCommitted += ComboBoxSpace_SelectionChangeCommitted;
			// 
			// buttonAddToMap
			// 
			buttonAddToMap.Anchor = AnchorStyles.Bottom;
			buttonAddToMap.Location = new Point(359, 469);
			buttonAddToMap.Name = "buttonAddToMap";
			buttonAddToMap.Size = new Size(126, 23);
			buttonAddToMap.TabIndex = 7;
			buttonAddToMap.Text = "Add to Map";
			buttonAddToMap.UseVisualStyleBackColor = true;
			buttonAddToMap.Click += ButtonAddToMap_Click;
			// 
			// buttonRemoveFromMap
			// 
			buttonRemoveFromMap.Anchor = AnchorStyles.Bottom;
			buttonRemoveFromMap.Location = new Point(491, 469);
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
			dataGridViewItemsToPlot.Location = new Point(12, 498);
			dataGridViewItemsToPlot.Name = "dataGridViewItemsToPlot";
			dataGridViewItemsToPlot.ReadOnly = true;
			dataGridViewItemsToPlot.RowHeadersVisible = false;
			dataGridViewItemsToPlot.ScrollBars = ScrollBars.Vertical;
			dataGridViewItemsToPlot.SelectionMode = DataGridViewSelectionMode.CellSelect;
			dataGridViewItemsToPlot.Size = new Size(953, 204);
			dataGridViewItemsToPlot.TabIndex = 9;
			// 
			// listViewSignature
			// 
			listViewSignature.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			listViewSignature.BackColor = SystemColors.ControlDark;
			listViewSignature.Location = new Point(6, 22);
			listViewSignature.Name = "listViewSignature";
			listViewSignature.Size = new Size(685, 111);
			listViewSignature.TabIndex = 10;
			listViewSignature.UseCompatibleStateImageBehavior = false;
			// 
			// listViewLockLevel
			// 
			listViewLockLevel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			listViewLockLevel.BackColor = SystemColors.ControlDark;
			listViewLockLevel.Location = new Point(6, 22);
			listViewLockLevel.Name = "listViewLockLevel";
			listViewLockLevel.Size = new Size(238, 111);
			listViewLockLevel.TabIndex = 11;
			listViewLockLevel.UseCompatibleStateImageBehavior = false;
			// 
			// groupBoxFilterSignature
			// 
			groupBoxFilterSignature.Anchor = AnchorStyles.Top;
			groupBoxFilterSignature.Controls.Add(buttonSelectRecommended);
			groupBoxFilterSignature.Controls.Add(buttonUnselectAllSignature);
			groupBoxFilterSignature.Controls.Add(buttonSelectAllSignature);
			groupBoxFilterSignature.Controls.Add(listViewSignature);
			groupBoxFilterSignature.Location = new Point(12, 58);
			groupBoxFilterSignature.Name = "groupBoxFilterSignature";
			groupBoxFilterSignature.Size = new Size(697, 165);
			groupBoxFilterSignature.TabIndex = 12;
			groupBoxFilterSignature.TabStop = false;
			groupBoxFilterSignature.Text = "Filter by category";
			// 
			// buttonSelectRecommended
			// 
			buttonSelectRecommended.Location = new Point(280, 136);
			buttonSelectRecommended.Name = "buttonSelectRecommended";
			buttonSelectRecommended.Size = new Size(137, 23);
			buttonSelectRecommended.TabIndex = 13;
			buttonSelectRecommended.Text = "Select Recommended";
			buttonSelectRecommended.UseVisualStyleBackColor = true;
			buttonSelectRecommended.Click += ButtonSelectRecommended_Click;
			// 
			// buttonUnselectAllSignature
			// 
			buttonUnselectAllSignature.Location = new Point(423, 136);
			buttonUnselectAllSignature.Name = "buttonUnselectAllSignature";
			buttonUnselectAllSignature.Size = new Size(80, 23);
			buttonUnselectAllSignature.TabIndex = 12;
			buttonUnselectAllSignature.Text = "Unselect All";
			buttonUnselectAllSignature.UseVisualStyleBackColor = true;
			buttonUnselectAllSignature.Click += ButtonUnselectAllSignature_Click;
			// 
			// buttonSelectAllSignature
			// 
			buttonSelectAllSignature.Location = new Point(194, 136);
			buttonSelectAllSignature.Name = "buttonSelectAllSignature";
			buttonSelectAllSignature.Size = new Size(80, 23);
			buttonSelectAllSignature.TabIndex = 11;
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
			groupBoxFilterLockLevel.Location = new Point(715, 58);
			groupBoxFilterLockLevel.Name = "groupBoxFilterLockLevel";
			groupBoxFilterLockLevel.Size = new Size(250, 165);
			groupBoxFilterLockLevel.TabIndex = 13;
			groupBoxFilterLockLevel.TabStop = false;
			groupBoxFilterLockLevel.Text = "Filter by lock level";
			// 
			// buttonUnselectAllLockLevel
			// 
			buttonUnselectAllLockLevel.Location = new Point(128, 136);
			buttonUnselectAllLockLevel.Name = "buttonUnselectAllLockLevel";
			buttonUnselectAllLockLevel.Size = new Size(80, 23);
			buttonUnselectAllLockLevel.TabIndex = 14;
			buttonUnselectAllLockLevel.Text = "Unselect All";
			buttonUnselectAllLockLevel.UseVisualStyleBackColor = true;
			buttonUnselectAllLockLevel.Click += ButtonUnselectAllLockLevel_Click;
			// 
			// buttonSelectAllLockLevel
			// 
			buttonSelectAllLockLevel.Location = new Point(42, 136);
			buttonSelectAllLockLevel.Name = "buttonSelectAllLockLevel";
			buttonSelectAllLockLevel.Size = new Size(80, 23);
			buttonSelectAllLockLevel.TabIndex = 13;
			buttonSelectAllLockLevel.Text = "Select All";
			buttonSelectAllLockLevel.UseVisualStyleBackColor = true;
			buttonSelectAllLockLevel.Click += ButtonSelectAllLockLevel_Click;
			// 
			// FormMain
			// 
			AcceptButton = buttonSearch;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = SystemColors.ControlDarkDark;
			ClientSize = new Size(977, 714);
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
		private ToolStripMenuItem grayscaleMenuItem;
		private ToolStripMenuItem highlightWaterMenuItem;
		private ToolStripMenuItem mapMapMarkersMenuItem;
		private ToolStripMenuItem mapMarkerIconsMenuItem;
		private ToolStripMenuItem mapMarkerLabelsMenuItem;
		private ToolStripMenuItem resetToolStripMenuItem;
		private ToolStripMenuItem mapBackgroundImageMenuItem;
		private ToolStripMenuItem backgroundNormalMenuItem;
		private ToolStripMenuItem backgroundSatelliteMenuItem;
		private ToolStripMenuItem backgroundMilitaryMenuItem;
		private ToolStripMenuItem backgroundNoneMenuItem;
		private ToolStripMenuItem mapLegendStyleMenuItem;
		private ToolStripMenuItem legendNormalMenuItem;
		private ToolStripMenuItem legendExtendedMenuItem;
		private ToolStripMenuItem legendHiddenMenuItem;
		private ToolStripMenuItem clearPlotsToolStripMenuItem;
		private ToolStripMenuItem quickSaveToolStripMenuItem;
		private ToolStripMenuItem exportToFileToolStripMenuItem;
		private ComboBox comboBoxSpace;
		private ToolStripMenuItem searchSettingsToolStripMenuItem;
		private ToolStripMenuItem searchInAllSpacesToolStripMenuItem;
		private Button buttonAddToMap;
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
	}
}
