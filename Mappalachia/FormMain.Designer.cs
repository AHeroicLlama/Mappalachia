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
			grayscaleMenuItem = new ToolStripMenuItem();
			highlightWaterMenuItem = new ToolStripMenuItem();
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
			joinTheDiscordToolStripMenuItem = new ToolStripMenuItem();
			sayThanksToolStripMenuItem = new ToolStripMenuItem();
			buttonSearch = new Button();
			textBoxSearch = new TextBox();
			dataGridViewSearchResults = new DataGridView();
			comboBoxSpace = new ComboBox();
			buttonAddToMap = new Button();
			buttonRemoveFromMap = new Button();
			dataGridViewItemsToPlot = new DataGridView();
			menuStripMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dataGridViewSearchResults).BeginInit();
			((System.ComponentModel.ISupportInitialize)dataGridViewItemsToPlot).BeginInit();
			SuspendLayout();
			// 
			// menuStripMain
			// 
			menuStripMain.Items.AddRange(new ToolStripItem[] { mapMenuItem, searchSettingsToolStripMenuItem, helpToolStripMenuItem, joinTheDiscordToolStripMenuItem, sayThanksToolStripMenuItem });
			menuStripMain.Location = new Point(0, 0);
			menuStripMain.Name = "menuStripMain";
			menuStripMain.ShowItemToolTips = true;
			menuStripMain.Size = new Size(1264, 24);
			menuStripMain.TabIndex = 0;
			menuStripMain.Text = "menuStrip1";
			// 
			// mapMenuItem
			// 
			mapMenuItem.DropDownItems.AddRange(new ToolStripItem[] { showPreviewToolStripMenuItem, grayscaleMenuItem, highlightWaterMenuItem, mapMapMarkersMenuItem, mapBackgroundImageMenuItem, mapLegendStyleMenuItem, quickSaveToolStripMenuItem, exportToFileToolStripMenuItem, clearPlotsToolStripMenuItem, resetToolStripMenuItem });
			mapMenuItem.Name = "mapMenuItem";
			mapMenuItem.Size = new Size(43, 20);
			mapMenuItem.Text = "Map";
			// 
			// showPreviewToolStripMenuItem
			// 
			showPreviewToolStripMenuItem.Name = "showPreviewToolStripMenuItem";
			showPreviewToolStripMenuItem.Size = new Size(191, 22);
			showPreviewToolStripMenuItem.Text = "Show Preview";
			showPreviewToolStripMenuItem.Click += Map_ShowPreview_Click;
			// 
			// grayscaleMenuItem
			// 
			grayscaleMenuItem.Name = "grayscaleMenuItem";
			grayscaleMenuItem.Size = new Size(191, 22);
			grayscaleMenuItem.Text = "Grayscale Background";
			grayscaleMenuItem.Click += Map_Grayscale_Click;
			// 
			// highlightWaterMenuItem
			// 
			highlightWaterMenuItem.Name = "highlightWaterMenuItem";
			highlightWaterMenuItem.Size = new Size(191, 22);
			highlightWaterMenuItem.Text = "Highlight Water";
			highlightWaterMenuItem.Click += Map_HightlightWater_Click;
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
			mapMarkerIconsMenuItem.Click += Map_MapMarkers_Icons_Click;
			// 
			// mapMarkerLabelsMenuItem
			// 
			mapMarkerLabelsMenuItem.Name = "mapMarkerLabelsMenuItem";
			mapMarkerLabelsMenuItem.Size = new Size(107, 22);
			mapMarkerLabelsMenuItem.Text = "Labels";
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
			backgroundNormalMenuItem.Click += Map_Background_Normal_Click;
			// 
			// backgroundSatelliteMenuItem
			// 
			backgroundSatelliteMenuItem.Name = "backgroundSatelliteMenuItem";
			backgroundSatelliteMenuItem.Size = new Size(115, 22);
			backgroundSatelliteMenuItem.Text = "Satellite";
			backgroundSatelliteMenuItem.Click += Map_Background_Satellite_Click;
			// 
			// backgroundMilitaryMenuItem
			// 
			backgroundMilitaryMenuItem.Name = "backgroundMilitaryMenuItem";
			backgroundMilitaryMenuItem.Size = new Size(115, 22);
			backgroundMilitaryMenuItem.Text = "Military";
			backgroundMilitaryMenuItem.Click += Map_Background_Military_Click;
			// 
			// backgroundNoneMenuItem
			// 
			backgroundNoneMenuItem.Name = "backgroundNoneMenuItem";
			backgroundNoneMenuItem.Size = new Size(115, 22);
			backgroundNoneMenuItem.Text = "None";
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
			legendNormalMenuItem.Click += Map_Legend_Normal_Click;
			// 
			// legendExtendedMenuItem
			// 
			legendExtendedMenuItem.Name = "legendExtendedMenuItem";
			legendExtendedMenuItem.Size = new Size(122, 22);
			legendExtendedMenuItem.Text = "Extended";
			legendExtendedMenuItem.Click += Map_Legend_Extended_Click;
			// 
			// legendHiddenMenuItem
			// 
			legendHiddenMenuItem.Name = "legendHiddenMenuItem";
			legendHiddenMenuItem.Size = new Size(122, 22);
			legendHiddenMenuItem.Text = "Hidden";
			legendHiddenMenuItem.Click += Map_Legend_Hidden_Click;
			// 
			// quickSaveToolStripMenuItem
			// 
			quickSaveToolStripMenuItem.Name = "quickSaveToolStripMenuItem";
			quickSaveToolStripMenuItem.Size = new Size(191, 22);
			quickSaveToolStripMenuItem.Text = "Quick Save";
			quickSaveToolStripMenuItem.Click += Map_QuickSave_Click;
			// 
			// exportToFileToolStripMenuItem
			// 
			exportToFileToolStripMenuItem.Name = "exportToFileToolStripMenuItem";
			exportToFileToolStripMenuItem.Size = new Size(191, 22);
			exportToFileToolStripMenuItem.Text = "Export to File";
			exportToFileToolStripMenuItem.Click += Map_ExportToFile_Click;
			// 
			// clearPlotsToolStripMenuItem
			// 
			clearPlotsToolStripMenuItem.Name = "clearPlotsToolStripMenuItem";
			clearPlotsToolStripMenuItem.Size = new Size(191, 22);
			clearPlotsToolStripMenuItem.Text = "Clear Plots";
			clearPlotsToolStripMenuItem.Click += Map_ClearPlots_Click;
			// 
			// resetToolStripMenuItem
			// 
			resetToolStripMenuItem.Name = "resetToolStripMenuItem";
			resetToolStripMenuItem.Size = new Size(191, 22);
			resetToolStripMenuItem.Text = "Reset";
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
			searchInAllSpacesToolStripMenuItem.Size = new Size(180, 22);
			searchInAllSpacesToolStripMenuItem.Text = "Search in all Spaces";
			searchInAllSpacesToolStripMenuItem.ToolTipText = "Constrain the search to the selected Space only, or search all Spaces.";
			searchInAllSpacesToolStripMenuItem.Click += Search_SearchInAllSpaces_Click;
			// 
			// advancedModeToolStripMenuItem
			// 
			advancedModeToolStripMenuItem.Name = "advancedModeToolStripMenuItem";
			advancedModeToolStripMenuItem.Size = new Size(180, 22);
			advancedModeToolStripMenuItem.Text = "Advanced Mode";
			advancedModeToolStripMenuItem.ToolTipText = "Toggle displaying search results in advanced dataminer format.";
			advancedModeToolStripMenuItem.Click += Search_AdvancedMode_Click;
			// 
			// helpToolStripMenuItem
			// 
			helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem, openUserGuidesToolStripMenuItem, checkForUpdatesToolStripMenuItem, viewGitHubToolStripMenuItem });
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
			aboutToolStripMenuItem.ToolTipText = "View additional info about the application,";
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
			buttonSearch.Location = new Point(743, 28);
			buttonSearch.Name = "buttonSearch";
			buttonSearch.Size = new Size(75, 23);
			buttonSearch.TabIndex = 5;
			buttonSearch.Text = "Search";
			buttonSearch.UseVisualStyleBackColor = true;
			buttonSearch.Click += ButtonSearch_Click;
			// 
			// textBoxSearch
			// 
			textBoxSearch.Anchor = AnchorStyles.Top;
			textBoxSearch.Location = new Point(447, 29);
			textBoxSearch.Name = "textBoxSearch";
			textBoxSearch.Size = new Size(290, 23);
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
			dataGridViewSearchResults.Location = new Point(12, 56);
			dataGridViewSearchResults.Name = "dataGridViewSearchResults";
			dataGridViewSearchResults.ReadOnly = true;
			dataGridViewSearchResults.RowHeadersVisible = false;
			dataGridViewSearchResults.ScrollBars = ScrollBars.Vertical;
			dataGridViewSearchResults.SelectionMode = DataGridViewSelectionMode.CellSelect;
			dataGridViewSearchResults.Size = new Size(1240, 309);
			dataGridViewSearchResults.TabIndex = 3;
			// 
			// comboBoxSpace
			// 
			comboBoxSpace.Anchor = AnchorStyles.Top;
			comboBoxSpace.FormattingEnabled = true;
			comboBoxSpace.Location = new Point(12, 28);
			comboBoxSpace.Name = "comboBoxSpace";
			comboBoxSpace.Size = new Size(429, 23);
			comboBoxSpace.TabIndex = 6;
			comboBoxSpace.Text = "Selected Space";
			comboBoxSpace.SelectedIndexChanged += ComboBoxSpace_SelectedIndexChanged;
			// 
			// buttonAddToMap
			// 
			buttonAddToMap.Location = new Point(503, 371);
			buttonAddToMap.Name = "buttonAddToMap";
			buttonAddToMap.Size = new Size(126, 23);
			buttonAddToMap.TabIndex = 7;
			buttonAddToMap.Text = "Add to Map";
			buttonAddToMap.UseVisualStyleBackColor = true;
			buttonAddToMap.Click += ButtonAddToMap_Click;
			// 
			// buttonRemoveFromMap
			// 
			buttonRemoveFromMap.Location = new Point(635, 371);
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
			dataGridViewItemsToPlot.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			dataGridViewItemsToPlot.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dataGridViewItemsToPlot.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			dataGridViewItemsToPlot.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewItemsToPlot.EditMode = DataGridViewEditMode.EditProgrammatically;
			dataGridViewItemsToPlot.Location = new Point(12, 400);
			dataGridViewItemsToPlot.Name = "dataGridViewItemsToPlot";
			dataGridViewItemsToPlot.ReadOnly = true;
			dataGridViewItemsToPlot.RowHeadersVisible = false;
			dataGridViewItemsToPlot.ScrollBars = ScrollBars.Vertical;
			dataGridViewItemsToPlot.SelectionMode = DataGridViewSelectionMode.CellSelect;
			dataGridViewItemsToPlot.Size = new Size(1240, 269);
			dataGridViewItemsToPlot.TabIndex = 9;
			// 
			// FormMain
			// 
			AcceptButton = buttonSearch;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = SystemColors.ControlDarkDark;
			ClientSize = new Size(1264, 681);
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
			menuStripMain.ResumeLayout(false);
			menuStripMain.PerformLayout();
			((System.ComponentModel.ISupportInitialize)dataGridViewSearchResults).EndInit();
			((System.ComponentModel.ISupportInitialize)dataGridViewItemsToPlot).EndInit();
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
	}
}
