using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Mappalachia.Class;
using Mappalachia.Forms;

namespace Mappalachia
{
	// The main form with map preview and all GUI controls inside it
	public partial class FormMaster : Form
	{
		public static List<MapItem> legendItems = new List<MapItem>();
		public static List<MapItem> searchResults = new List<MapItem>();

		static List<Cell> cells;

		public ProgressBar progressBar;

		static bool warnedLVLINotUsed = false; // Flag for if we've displayed certain warnings, so as to only show once per run
		static bool forceDrawBaseLayer = false; // Force a base layer redraw at the next draw event
		static Point lastMouseDownPos;

		public FormMaster()
		{
			InitializeComponent();

			Map.progressBarMain = progressBarMain;
			progressBar = progressBarMain;

			// Cleanup on launch in case it didn't run last time
			IOManager.Cleanup();

			// Populate UI elements
			PopulateSignatureFilterList();
			SelectRecommendedSignatures();
			PopulateLockTypeFilterList();
			PopulateVariableNPCSpawnList();
			PopulateScrapList();
			ProvideSearchHint();

			// Auto-select text box text and use search button with enter
			textBoxSearch.Select();
			textBoxSearch.SelectAll();
			AcceptButton = buttonSearch;

			// Load settings from the preferences file, if applicable.
			SettingsManager.LoadSettings();

			// Apply min/max values according to current settings
			numericUpDownNPCSpawnThreshold.Minimum = SettingsSearch.spawnChanceMin;
			numericUpDownNPCSpawnThreshold.Maximum = SettingsSearch.spawnChanceMax;
			numericMinZ.Increment = SettingsCell.GetHeightBinSize();
			numericMaxZ.Increment = numericMinZ.Increment;

			// Apply UI layouts according to current settings
			UpdateLocationColumnVisibility();
			UpdateResultsLockTypeColumnVisibility();
			UpdateVolumeEnabledState(false);
			UpdateAmountColumnToolTip();
			UpdatePlotMode(false);
			UpdateHeatMapColorMode(false);
			UpdateHeatMapResolution(false);
			UpdateMapLayerSettings(false);
			UpdateMapGrayscale(false);
			UpdateSearchInterior();
			UpdateShowFormID();
			UpdateSpawnChance();

			Map.SetOutput(pictureBoxMapPreview);

			// Assign settings for whichever Map Mode we're starting in
			// Also draws the map for the first time
			EnterMapMode(SettingsMap.mode);

			// Check for updates, only notify if update found
			UpdateChecker.CheckForUpdate(false);
		}

		// All Methods not directly responding to UI input
		#region Methods

		// Dynamically fill the Signature filter with every signature present based on the data
		void PopulateSignatureFilterList()
		{
			List<string> signatures = DataHelper.GetPermittedSignatures();
			List<string> orderedSignatures = new List<string>();

			// First run through the suggested order list. If any of our database signature items are in there, process them FIRST
			// This ensures items are added to the list in order
			foreach (string signature in DataHelper.suggestedSignatureSort)
			{
				// If this item is in the suggested sort - add it to the final items
				if (signatures.Contains(signature))
				{
					orderedSignatures.Add(signature);
				}
			}

			// Now we have processed some suggested sort items, any remaining signatures can be processed next
			foreach (string signature in signatures)
			{
				// This item was NOT picked up when we passed through the suggested sort - so add it onto the end
				if (!orderedSignatures.Contains(signature))
				{
					orderedSignatures.Add(signature);
				}
			}

			// Finally, now we have a list which starts with the suggested order, and ends with any unsorted items
			//...We can add them to the ListView on the form
			foreach (string signature in orderedSignatures)
			{
				ListViewItem thisItem = listViewFilterSignatures.Items.Add(signature);
				thisItem.Text = DataHelper.ConvertSignature(thisItem.Text, false);
				thisItem.ToolTipText = DataHelper.GetSignatureDescription(signature);
				thisItem.Checked = true;
			}
		}

		// Selects only the recommended signature filters
		void SelectRecommendedSignatures()
		{
			foreach (ListViewItem item in listViewFilterSignatures.Items)
			{
				// check the item if it's in the list of recommended signatures, otherwise uncheck
				item.Checked = DataHelper.recommendedSignatures.Contains(DataHelper.ConvertSignature(item.Text, true));
			}
		}

		// Dynamically fill the Lock Type filter with every lock type present based on the data
		void PopulateLockTypeFilterList()
		{
			List<string> lockLevels = DataHelper.GetPermittedLockTypes();
			List<string> orderedLockLevels = new List<string>();

			// First run through the suggested order list. If any of our database lock types are in there, process them FIRST
			// This ensures items are added to the list in order
			foreach (string lockLevel in DataHelper.suggestedLockLevelSort)
			{
				// If this item is in the suggested sort - add it to the final items
				if (lockLevels.Contains(lockLevel))
				{
					orderedLockLevels.Add(lockLevel);
				}
			}

			// Now we have processed some suggested sort items, any remaining lock types can be processed next
			foreach (string lockLevel in lockLevels)
			{
				// This item was NOT picked up when we passed through the suggested sort - so add it onto the end
				if (!orderedLockLevels.Contains(lockLevel))
				{
					orderedLockLevels.Add(lockLevel);
				}
			}

			// Finally, now we have a list which starts with the suggested order, and ends with any unsorted items
			//...We can add them to the ListView on the form
			foreach (string lockLevel in orderedLockLevels)
			{
				ListViewItem thisItem = listViewFilterLockTypes.Items.Add(lockLevel);
				thisItem.Text = DataHelper.ConvertLockLevel(thisItem.Text, false);
				thisItem.Checked = true;
			}
		}

		// Fill the list of Variable NPC Spawns that can be chosen to search
		void PopulateVariableNPCSpawnList()
		{
			foreach (string npcSpawn in DataHelper.GetVariableNPCTypes())
			{
				listBoxNPC.Items.Add(npcSpawn);
			}

			listBoxNPC.SelectedIndex = 0;
		}

		// Fill the list of Scrap types that can be chosen to search
		void PopulateScrapList()
		{
			foreach (string npcSpawn in DataHelper.GetVariableScrapTypes())
			{
				listBoxScrap.Items.Add(npcSpawn);
			}

			listBoxScrap.SelectedIndex = 0;
		}

		// Populate the Cell combo box (only visible in Cell mode) with all cells
		void PopulateCellList()
		{
			// Return if the list is already populated
			if (comboBoxCell.Items.Count != 0)
			{
				return;
			}

			cells = DataHelper.GetAllCells();

			foreach (Cell cell in cells)
			{
				comboBoxCell.Items.Add(cell.displayName + " (" + cell.editorID + ")");
			}

			comboBoxCell.SelectedIndex = comboBoxCell.Items.Count - 1;
		}

		// Fill the search box with a suggested search term.
		void ProvideSearchHint()
		{
			List<string> searchTermHints = new List<string>
			{
				"Nuka Cola",
				"Caps Stash",
				"Thistle",
				"Overseer's Cache",
				"Vein",
				"Instrument",
				"Babylon",
				"Recipe: Delbert's",
				"Protest Sign",
				"Trunk Boss",
				"Treasure Map Mound",
				"Hardpoint",
				"Rare",
				"Teddy Bear",
				"Workbench",
				"NoCampAllowed",
				"PowerArmorFurniture",
				"RETrigger",
			};

			textBoxSearch.Text = searchTermHints[new Random().Next(searchTermHints.Count)];
		}

		// Update the visibility of the search results location column, given current settings
		void UpdateLocationColumnVisibility()
		{
			gridViewSearchResults.Columns["columnSearchLocation"].Visible = SettingsSearch.searchInterior || SettingsMap.IsCellModeActive();
		}

		// Update the visiblity of the lock type column in the results view, given current settings
		void UpdateResultsLockTypeColumnVisibility()
		{
			// Check if the lock type filter is in use - if it is we probably want to show the column.
			if (tabControlStandardNPCJunk.SelectedTab == tabPageStandard)
			{
				foreach (ListViewItem lockType in listViewFilterLockTypes.Items)
				{
					if (!lockType.Checked)
					{
						gridViewSearchResults.Columns["columnSearchLockLevel"].Visible = true;
						return;
					}
				}
			}

			// If they are all checked, or this is not a search where filters apply - hide the column again
			gridViewSearchResults.Columns["columnSearchLockLevel"].Visible = false;
		}

		// Update the map settings > draw volume check, based on current settings
		void UpdateVolumeEnabledState(bool reDraw)
		{
			drawVolumesMenuItem.Checked = SettingsPlot.drawVolumes;

			if (reDraw)
			{
				DrawMap(false);
			}
		}

		// Update tooltip on "amount" column header - as it changes depending on interior searching or not
		void UpdateAmountColumnToolTip()
		{
			gridViewSearchResults.Columns["columnSearchAmount"].ToolTipText = SettingsSearch.searchInterior ?
				"The amount of instances which can be found in the listed location." :
				"The amount of instances which can be found on the surface of Appalachia.";
		}

		// Update the Plot Settings > Mode options based on the actual value in PlotSettings
		void UpdatePlotMode(bool reDraw)
		{
			switch (SettingsPlot.mode)
			{
				case SettingsPlot.Mode.Icon:
					modeIconMenuItem.Checked = true;
					modeHeatmapMenuItem.Checked = false;
					break;
				case SettingsPlot.Mode.Heatmap:
					modeIconMenuItem.Checked = false;
					modeHeatmapMenuItem.Checked = true;
					break;
			}

			if (reDraw)
			{
				DrawMap(false);
			}
		}

		// Update the UI with the currently selected heatmap color mode
		void UpdateHeatMapColorMode(bool reDraw)
		{
			switch (SettingsPlotHeatmap.colorMode)
			{
				case SettingsPlotHeatmap.ColorMode.Mono:
					monoColorModeMenuItem.Checked = true;
					duoColorModeMenuItem.Checked = false;
					break;
				case SettingsPlotHeatmap.ColorMode.Duo:
					monoColorModeMenuItem.Checked = false;
					duoColorModeMenuItem.Checked = true;
					break;
			}

			if (reDraw && SettingsPlot.IsHeatmap())
			{
				DrawMap(false);
			}
		}

		// Update the UI with the currently selected heatmap resolution
		void UpdateHeatMapResolution(bool reDraw)
		{
			UncheckAllResolutions();
			switch (SettingsPlotHeatmap.resolution)
			{
				case 128:
					resolution128MenuItem.Checked = true;
					break;
				case 256:
					resolution256MenuItem.Checked = true;
					break;
				case 512:
					resolution512MenuItem.Checked = true;
					break;
				case 1024:
					resolution1024MenuItem.Checked = true;
					break;
				default:
					Notify.Error("Unsupported Heatmap resolution " + SettingsPlotHeatmap.resolution);
					break;
			}

			if (reDraw && SettingsPlot.IsHeatmap())
			{
				DrawMap(false);
			}
		}

		// Update check marks in the UI with current MapSettings, and redraw the map if true
		void UpdateMapLayerSettings(bool reDraw)
		{
			layerMilitaryMenuItem.Checked = SettingsMap.layerMilitary;
			layerNWFlatwoodsMenuItem.Checked = SettingsMap.layerNWFlatwoods;
			layerNWMorgantownMenuItem.Checked = SettingsMap.layerNWMorgantown;

			if (reDraw)
			{
				DrawMap(true);
			}
		}

		// Update check mark in the UI with current MapSettings for grayscale
		void UpdateMapGrayscale(bool reDraw)
		{
			grayscaleMenuItem.Checked = SettingsMap.grayScale;

			if (reDraw)
			{
				DrawMap(true);
			}
		}

		// Update check mark in the UI for Search Interiors option
		void UpdateSearchInterior()
		{
			interiorSearchMenuItem.Checked = SettingsSearch.searchInterior;
		}

		// Update check mark in the UI for Show FormID option
		void UpdateShowFormID()
		{
			showFormIDMenuItem.Checked = SettingsSearch.showFormID;
			gridViewSearchResults.Columns["columnSearchFormID"].Visible = SettingsSearch.showFormID;
		}

		// Update the minimum spawn chance % value on the NPC Search tab
		void UpdateSpawnChance()
		{
			numericUpDownNPCSpawnThreshold.Value = SettingsSearch.spawnChance;
		}

		// Update the UI with min and max cell height settings stored in cell mode settings
		void UpdateCellHeightSettings()
		{
			numericMinZ.Value = SettingsCell.minHeightPerc;
			numericMaxZ.Value = SettingsCell.maxHeightPerc;
		}

		void UpdateCellDrawOutLine()
		{
			checkBoxCellDrawOutline.Checked = SettingsCell.drawOutline;
		}

		// Applies the config necessary for exiting the current map mode
		void ExitMapMode()
		{
			ClearSearchResults();
			ClearLegend();

			switch (SettingsMap.mode)
			{
				case SettingsMap.Mode.Cell:
					cellModeMenuItem.Checked = false;
					interiorSearchMenuItem.Enabled = true;
					layerMenuItem.Enabled = true;
					brightnessMenuItem.Enabled = true;
					grayscaleMenuItem.Enabled = true;
					tabControlStandardNPCJunk.TabPages.Add(tabPageNpcScrapSearch);
					groupBoxCellModeSettings.Visible = false;
					break;

				case SettingsMap.Mode.Worldspace:
					break;
			}
		}

		// Applies the config necessary for entering a map mode
		void EnterMapMode(SettingsMap.Mode incomingMode)
		{
			switch (incomingMode)
			{
				case SettingsMap.Mode.Cell:
					SettingsMap.mode = incomingMode;
					cellModeMenuItem.Checked = true;
					interiorSearchMenuItem.Enabled = false;
					layerMenuItem.Enabled = false;
					brightnessMenuItem.Enabled = false;
					grayscaleMenuItem.Enabled = false;
					tabControlStandardNPCJunk.TabPages.Remove(tabPageNpcScrapSearch);

					PopulateCellList();
					UpdateCellHeightSettings();
					UpdateCellDrawOutLine();

					textBoxSearch.Text = string.Empty;
					tabControlStandardNPCJunk.SelectedTab = tabPageStandard;
					groupBoxCellModeSettings.Visible = true;
					break;

				case SettingsMap.Mode.Worldspace:
					SettingsMap.mode = incomingMode;
					cellModeMenuItem.Checked = false;
					break;
			}

			UpdateLocationColumnVisibility();
			Map.Reset();
		}

		// Unselect all resolution options under heatmap resolution. Used to remove any current selection
		void UncheckAllResolutions()
		{
			resolution128MenuItem.Checked = false;
			resolution256MenuItem.Checked = false;
			resolution512MenuItem.Checked = false;
			resolution1024MenuItem.Checked = false;
		}

		// Collect the enabled signatures from the UI to a list for use by a query
		List<string> GetEnabledSignatures()
		{
			List<string> filteredSignatures = new List<string>();
			foreach (ListViewItem signature in listViewFilterSignatures.Items)
			{
				if (signature.Checked)
				{
					filteredSignatures.Add(DataHelper.ConvertSignature(signature.Text, true));
				}
			}

			return filteredSignatures;
		}

		// Collect the enabled lock types from the UI to a list for use by a query
		List<string> GetEnabledLockTypes()
		{
			List<string> lockTypes = new List<string>();
			foreach (ListViewItem lockType in listViewFilterLockTypes.Items)
			{
				if (lockType.Checked)
				{
					lockTypes.Add(DataHelper.ConvertLockLevel(lockType.Text, true));
				}
			}

			return lockTypes;
		}

		// Warn the user if they appear to be trying to search for something that might actually be in a LVLI, but they have unselected it
		void WarnWhenLVLINotSelected()
		{
			// Already warned - we only do this once per session
			if (warnedLVLINotUsed)
			{
				return;
			}

			List<string> enabledSignatures = GetEnabledSignatures();
			if (!enabledSignatures.Contains("LVLI"))
			{
				// A list of signatures that seem to typically be represented by LVLI
				foreach (string signatureToWarn in new List<string> { "FLOR", "ALCH", "WEAP", "ARMO", "BOOK", "AMMO" })
				{
					if (enabledSignatures.Contains(signatureToWarn))
					{
						Notify.Info(
							"Your search results may be restricted because you have 'Loot' unchecked under the categories filter.\n" +
							"Many desirable items in Fallout 76 are categorized generically as 'Loot'.\n" +
							"If you can't find what you're looking for, make sure to enable it and search again.");
						warnedLVLINotUsed = true;
						return;
					}
				}
			}
		}

		// Warn the user if all filters of a category are blank and therefore no results would match
		bool WarnWhenAllFiltersBlank()
		{
			if (GetEnabledSignatures().Count == 0 || GetEnabledLockTypes().Count == 0)
			{
				Notify.Info(
					"No search results were found because you have disabled every filter for a category.\n" +
					"You must enable at least one filter per category to find anything.");
				return true;
			}

			return false;
		}

		// Totally clears all search results
		void ClearSearchResults()
		{
			searchResults = new List<MapItem>();
			UpdateSearchResultsGrid();
		}

		// Wipe and re-populate the search results UI element with the items in "List<MapItem> searchResults"
		void UpdateSearchResultsGrid()
		{
			gridViewSearchResults.Enabled = false;
			gridViewSearchResults.Rows.Clear();
			int index = 0;

			foreach (MapItem mapItem in searchResults)
			{
				gridViewSearchResults.Rows.Add(
					mapItem.uniqueIdentifier,
					mapItem.editorID,
					mapItem.displayName,
					DataHelper.ConvertSignature(mapItem.signature, false),
					string.Join(", ", DataHelper.ConvertLockLevelCollection(mapItem.filteredLockTypes, false)),
					mapItem.weight == -1 ? "Unknown" : mapItem.weight.ToString(),
					mapItem.count,
					mapItem.location,
					mapItem.locationEditorID,
					index); // Index relates the UI row to the List<MapItem>, even if the UI is sorted

				index++;
			}

			gridViewSearchResults.Enabled = true;
		}

		void NotifyIfNoResults()
		{
			if (searchResults.Count == 0)
			{
				Notify.Info("No results found for that search.");
			}
		}

		// Wipe and re-populate the Legend Grid View with items contained in "List<MapItem> legendItems"
		void UpdateLegendGrid(MapItem lastSelectedItem)
		{
			gridViewLegend.Enabled = false;
			gridViewLegend.Rows.Clear();

			foreach (MapItem legend in legendItems)
			{
				gridViewLegend.Rows.Add(legend.legendGroup, legend.GetLegendText(false));
			}

			gridViewLegend.Enabled = true;

			// If the list is populated we can select an index
			if (gridViewLegend.RowCount >= 1)
			{
				// Provide a default at the end of the list
				int indexToSelect = gridViewLegend.RowCount - 1;

				// If provided, find and re-select the last selected item
				if (lastSelectedItem != null)
				{
					indexToSelect = legendItems.IndexOf(legendItems.Where(m => m.editorID == lastSelectedItem.editorID).First());
				}

				// Select and scroll to the best new index
				gridViewLegend.Rows[indexToSelect].Selected = true;
				gridViewLegend.FirstDisplayedScrollingRowIndex = indexToSelect;
			}
		}

		// Finds the unique instances of legend groups with overridden legend texts, in order for them to be merged together
		public static Dictionary<int, string> GatherOverriddenLegendTexts()
		{
			Dictionary<int, string> overriddenTexts = new Dictionary<int, string>();

			foreach (MapItem mapItem in legendItems)
			{
				if (overriddenTexts.ContainsKey(mapItem.legendGroup) || string.IsNullOrWhiteSpace(mapItem.overridingLegendText))
				{
					continue; // Either this isn't overridden, or we already have this one - skip
				}
				// This must be a new MapItem with overridden text - add it to the Dictionary
				else if (!string.IsNullOrWhiteSpace(mapItem.overridingLegendText))
				{
					overriddenTexts.Add(mapItem.legendGroup, mapItem.overridingLegendText);
				}
			}

			return overriddenTexts;
		}

		// Wipe away the legend items and update the UI. Doesn't re-draw the map
		void ClearLegend()
		{
			foreach (MapItem item in legendItems)
			{
				item.overridingLegendText = string.Empty;
			}

			legendItems = new List<MapItem>();
			UpdateLegendGrid(null);
		}

		// Find the next-lowest free legend group value in LegendItems
		int FindLowestAvailableLegendGroupValue()
		{
			int n = legendItems.Count;
			bool[] takenIndices = new bool[n];

			foreach (MapItem item in legendItems)
			{
				if (item.legendGroup < n)
				{
					takenIndices[item.legendGroup] = true;
				}
			}

			int earliestIndex = Array.IndexOf(takenIndices, false);
			return earliestIndex < 0 ? n : earliestIndex;
		}

		// Remove plots from map and empty the legend list
		void ClearMap()
		{
			ClearLegend();
			DrawMap(false);
		}

		// User-activated draw. Draw the plot points onto the map, if there is anything to plot
		void DrawMap(bool drawBaseLayer)
		{
			// Disable control of items which can cause another draw event
			buttonDrawMap.Enabled = false;
			buttonAddToLegend.Enabled = false;
			buttonRemoveFromLegend.Enabled = false;
			numericMinZ.Enabled = false;
			numericMaxZ.Enabled = false;
			comboBoxCell.Enabled = false;
			checkBoxCellDrawOutline.Enabled = false;
			grayscaleMenuItem.Enabled = false;
			brightnessMenuItem.Enabled = false;
			clearMenuItem.Enabled = false;
			resetMenuItem.Enabled = false;
			layerMenuItem.Enabled = false;
			plotSettingsMenuItem.Enabled = false;
			advancedModeMenuItem.Enabled = false;

			if (drawBaseLayer || forceDrawBaseLayer)
			{
				Map.DrawBaseLayer();
				forceDrawBaseLayer = false;
			}
			else
			{
				Map.Draw();
			}

			// Re-enable disabled controls after
			buttonDrawMap.Enabled = true;
			buttonAddToLegend.Enabled = true;
			buttonRemoveFromLegend.Enabled = true;
			numericMinZ.Enabled = true;
			numericMaxZ.Enabled = true;
			comboBoxCell.Enabled = true;
			checkBoxCellDrawOutline.Enabled = true;
			grayscaleMenuItem.Enabled = true;
			brightnessMenuItem.Enabled = true;
			clearMenuItem.Enabled = true;
			resetMenuItem.Enabled = true;
			layerMenuItem.Enabled = true;
			plotSettingsMenuItem.Enabled = true;
			advancedModeMenuItem.Enabled = true;

			GC.Collect();
		}

		void PreviewMap()
		{
			Map.Open();
		}

		// Render a crude text graph to visualize height distribution in the currently selected cell
		void ShowCellModeHeightDistribution()
		{
			string textVisualization = string.Empty;
			int i = 0;
			foreach (double value in SettingsCell.GetCell().GetHeightDistribution())
			{
				textVisualization = (i * SettingsCell.GetHeightBinSize()).ToString().PadLeft(2, '0') + "%:" + new string('#', (int)Math.Round(value)) + "\n" + textVisualization;
				i++;
			}

			MessageBox.Show(textVisualization, "Height distribution for " + SettingsCell.GetCell().editorID);
		}

		#endregion

		// All Methods which represent responses to UI input
		#region UI Controls

		// Map > View - Write the map image to disk and launch in default program
		void Map_View(object sender, EventArgs e)
		{
			PreviewMap();
		}

		// Map > Layer > Military - Toggle the map background to be military
		void Map_Layer_Military(object sender, EventArgs e)
		{
			SettingsMap.layerMilitary = !SettingsMap.layerMilitary;
			UpdateMapLayerSettings(true);
		}

		// Map > Layer > NW Flatwoods - Toggle the NW Flatwoods layer
		void Map_Layer_NWFlatwoods(object sender, EventArgs e)
		{
			SettingsMap.layerNWFlatwoods = !SettingsMap.layerNWFlatwoods;
			UpdateMapLayerSettings(true);
		}

		// Map > Layer > NW MorganTown - Toggle the NW Morgantown later
		void Map_Layer_NWMorgantown(object sender, EventArgs e)
		{
			SettingsMap.layerNWMorgantown = !SettingsMap.layerNWMorgantown;
			UpdateMapLayerSettings(true);
		}

		// Map > Advanced Modes > Cell Mode
		private void Map_CellMode(object sender, EventArgs e)
		{
			if (!SettingsMap.IsCellModeActive())
			{
				DialogResult question = MessageBox.Show(
					"Cell mode is an advanced mode designed to make detailed maps of individual cells.\n" +
					"If you want to search generally for items across all cells, you should enable Search Settings > Search Interiors.\n\n" +
					"Switching to Cell mode may change or disable certain other settings.\n" +
					"Please read the user documentation on Cell Mode for full details.\n\n" +
					"Continue to Cell mode?",
					"Switch to Cell mode?", MessageBoxButtons.YesNo);

				if (question == DialogResult.Yes)
				{
					ExitMapMode();
					EnterMapMode(SettingsMap.Mode.Cell);
				}
			}
			else
			{
				ExitMapMode();
				EnterMapMode(SettingsMap.Mode.Worldspace);
			}
		}

		// Map > Brightness... - Open the brightness adjust form
		void Map_Brightness(object sender, EventArgs e)
		{
			FormSetBrightness formSetBrightness = new FormSetBrightness();
			formSetBrightness.ShowDialog();
		}

		// Map > Grayscale - Toggle grayscale drawing of map base layer
		private void Map_Grayscale(object sender, EventArgs e)
		{
			SettingsMap.grayScale = !SettingsMap.grayScale;
			UpdateMapGrayscale(true);
		}

		// Map > Export To File - Export the map image to file
		void Map_Export(object sender, EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog
			{
				Filter = SettingsMap.IsCellModeActive() ? "PNG|*.png" : "JPEG|*.jpeg",
				FileName = "Mappalachia Map",
			};

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				Map.WriteToFile(dialog.FileName);
			}
		}

		// Map > Clear - Remove legend items and remove plotted layers from the map
		void Map_Clear(object sender, EventArgs e)
		{
			ClearMap();
		}

		// Map > Reset - Hard reset the map, all layers, plots, legend items and pan/zoom
		void Map_Reset(object sender, EventArgs e)
		{
			ClearLegend();
			Map.Reset();

			UpdateMapLayerSettings(false);
			UpdateMapGrayscale(false);

			// Reset pan and zoom
			pictureBoxMapPreview.Location = new Point(0, 0);
			pictureBoxMapPreview.Width = splitContainerMain.Panel1.Width;
			pictureBoxMapPreview.Height = splitContainerMain.Panel1.Height;

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		// Search Settings > Toggle interiors - Toggle including interiors in search results
		void Search_Interior(object sender, EventArgs e)
		{
			SettingsSearch.searchInterior = !SettingsSearch.searchInterior;
			UpdateSearchInterior();
		}

		// Search Settings > Show FormID - Toggle visibility of FormID column
		void Search_FormID(object sender, EventArgs e)
		{
			SettingsSearch.showFormID = !SettingsSearch.showFormID;
			UpdateShowFormID();
		}

		// Plot Settings > Mode > Icon - Change plot mode to icon
		private void Plot_Mode_Icon(object sender, EventArgs e)
		{
			SettingsPlot.mode = SettingsPlot.Mode.Icon;
			UpdatePlotMode(true);
		}

		// Plot Settings > Mode > Heatmap - Change plot mode to heatmap
		private void Plot_Mode_Heatmap(object sender, EventArgs e)
		{
			SettingsPlot.mode = SettingsPlot.Mode.Heatmap;
			UpdatePlotMode(true);
		}

		// Plot Settings > Plot Icon Settings - Open plot settings form
		private void Plot_PlotIconSettings(object sender, EventArgs e)
		{
			FormPlotIconSettings formPlotSettings = new FormPlotIconSettings();
			formPlotSettings.ShowDialog();
		}

		// Plot Setting > Heatmap Settings > Color Mode > Mono - Change color mode to mono
		private void Plot_HeatMap_ColorMode_Mono(object sender, EventArgs e)
		{
			SettingsPlotHeatmap.colorMode = SettingsPlotHeatmap.ColorMode.Mono;
			UpdateHeatMapColorMode(true);
		}

		// Plot Setting > Heatmap Settings > Color Mode > Duo - Change color mode to duo
		private void Plot_HeatMap_ColorMode_Duo(object sender, EventArgs e)
		{
			SettingsPlotHeatmap.colorMode = SettingsPlotHeatmap.ColorMode.Duo;
			UpdateHeatMapColorMode(true);
		}

		// Plot Setting > Heatmap Settings > Resolution > 128 - Change resolution to 128
		private void Plot_HeatMap_Resolution_128(object sender, EventArgs e)
		{
			SettingsPlotHeatmap.resolution = 128;
			UpdateHeatMapResolution(true);
		}

		// Plot Setting > Heatmap Settings > Resolution > 256 - Change resolution to 256
		private void Plot_HeatMap_Resolution_256(object sender, EventArgs e)
		{
			SettingsPlotHeatmap.resolution = 256;
			UpdateHeatMapResolution(true);
		}

		// Plot Setting > Heatmap Settings > Resolution > 512 - Change resolution to 512
		private void Plot_HeatMap_Resolution_512(object sender, EventArgs e)
		{
			SettingsPlotHeatmap.resolution = 512;
			UpdateHeatMapResolution(true);
		}

		// Plot Setting > Heatmap Settings > Resolution > 1024 - Change resolution to 1024
		private void Plot_HeatMap_Resolution_1024(object sender, EventArgs e)
		{
			SettingsPlotHeatmap.resolution = 1024;
			UpdateHeatMapResolution(true);
		}

		// Plot Settings > Draw Volumes - Toggle drawing volumes
		private void Plot_DrawVolumes(object sender, EventArgs e)
		{
			SettingsPlot.drawVolumes = !SettingsPlot.drawVolumes;
			UpdateVolumeEnabledState(true);
		}

		// Help > About - Show the About box
		void Help_About(object sender, EventArgs e)
		{
			FormAbout aboutBox = new FormAbout();
			aboutBox.ShowDialog();
		}

		// Help > User Guides - Open help guides at github master
		void Help_UserGuides(object sender, EventArgs e)
		{
			Process.Start("https://github.com/AHeroicLlama/Mappalachia/tree/master/User_Guides");
		}

		// Help > Check for Updates - Notify the user if there is an update available. Reports back if there were errors.
		private void Help_CheckForUpdates(object sender, EventArgs e)
		{
			UpdateChecker.CheckForUpdate(true);
		}

		// Donate to the Author - Launch donate URL
		void Donate(object sender, EventArgs e)
		{
			Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=TDVKFJ97TFFVC&source=url");
		}

		// Signature select all
		void ButtonSelectAllSignature(object sender, EventArgs e)
		{
			foreach (ListViewItem item in listViewFilterSignatures.Items)
			{
				item.Checked = true;
			}
		}

		// Signature select recommended
		private void ButtonSelectRecommendedSignature(object sender, EventArgs e)
		{
			SelectRecommendedSignatures();
		}

		// Signature deselect all
		void ButtonDeselectAllSignature(object sender, EventArgs e)
		{
			foreach (ListViewItem item in listViewFilterSignatures.Items)
			{
				item.Checked = false;
			}
		}

		// Lock select all
		void ButtonSelectAllLock(object sender, EventArgs e)
		{
			foreach (ListViewItem item in listViewFilterLockTypes.Items)
			{
				item.Checked = true;
			}
		}

		// Lock deselect all
		void ButtonDeselectAllLock(object sender, EventArgs e)
		{
			foreach (ListViewItem item in listViewFilterLockTypes.Items)
			{
				item.Checked = false;
			}
		}

		// In Cell mode, the current items in search results and legend are inherently connected to the cellFormID.
		// This cellFormID is connected to the currently select cell in this ComboBox, and therefore changing it mid-map-production would confuse the target cell
		private void ComboBoxCell_SelectedIndexChanged(object sender, EventArgs e)
		{
			ClearSearchResults();
			ClearLegend();
			numericMinZ.Value = numericMinZ.Minimum;
			numericMaxZ.Value = numericMinZ.Maximum;
			SettingsCell.SetCell(cells[comboBoxCell.SelectedIndex]);
		}

		private void CheckBoxCellDrawOutline_CheckedChanged(object sender, EventArgs e)
		{
			SettingsCell.drawOutline = checkBoxCellDrawOutline.Checked;
			DrawMap(true);
		}

		private void ButtonCellHeightDistribution_Click(object sender, EventArgs e)
		{
			ShowCellModeHeightDistribution();
		}

		// Cell mode height range changed - maintain the min safely below the max
		private void NumericMinZ_ValueChanged(object sender, EventArgs e)
		{
			if (numericMaxZ.Value <= numericMinZ.Value)
			{
				numericMaxZ.Value = Math.Min(numericMaxZ.Value + numericMaxZ.Increment, numericMaxZ.Maximum);

				// It's likely the user just pressed tab to cross to the max value
				// We just highlighted it, but adjusting the value will un-highlight it again
				// So, re-highlight it
				NumericMaxZ_Enter(sender, e);
			}

			// Special case where the max could not go higher, so this must stay an increment lower
			if (numericMinZ.Value == numericMaxZ.Maximum)
			{
				numericMinZ.Value -= numericMinZ.Increment;
			}

			SettingsCell.minHeightPerc = (int)numericMinZ.Value;
			forceDrawBaseLayer = true;
		}

		// Cell mode height range changed - maintain the max safely above the min
		private void NumericMaxZ_ValueChanged(object sender, EventArgs e)
		{
			if (numericMinZ.Value >= numericMaxZ.Value)
			{
				numericMinZ.Value = Math.Max(numericMinZ.Value - numericMinZ.Increment, numericMinZ.Minimum);

				// It's likely the user just pressed shift-tab to cross to the min value
				// We just highlighted it, but adjusting the value will un-highlight it again
				// So, re-highlight it
				NumericMinZ_Enter(sender, e);
			}

			// Special case where the min could not go lower, so this must stay an increment higher
			if (numericMaxZ.Value == numericMinZ.Minimum)
			{
				numericMaxZ.Value += numericMaxZ.Increment;
			}

			SettingsCell.maxHeightPerc = (int)numericMaxZ.Value;
			forceDrawBaseLayer = true;
		}

		// Select the value to overwrite when entered
		private void NumericMinZ_Enter(object sender, EventArgs e)
		{
			numericMinZ.Select(0, numericMinZ.Text.Length);
		}

		private void NumericMaxZ_Enter(object sender, EventArgs e)
		{
			numericMaxZ.Select(0, numericMaxZ.Text.Length);
		}

		// Clicked on - pass to enter event
		private void NumericMinZ_MouseDown(object sender, MouseEventArgs e)
		{
			NumericMinZ_Enter(sender, e);
		}

		private void NumericMaxZ_MouseDown(object sender, MouseEventArgs e)
		{
			NumericMaxZ_Enter(sender, e);
		}

		// Search Button - Gather parameters, execute query and populate results
		void ButtonSearchStandard(object sender, EventArgs e)
		{
			// Disable the button to reduce stacking search operations and reset the progress bar
			buttonSearch.Enabled = false;
			progressBarMain.Value = progressBarMain.Minimum;

			// Check for and show warnings
			WarnWhenLVLINotSelected();

			// If there are no filters selected, inform and cancel the search
			if (WarnWhenAllFiltersBlank())
			{
				buttonSearch.Enabled = true;
				return;
			}

			// Pre-query - set progress to 1/2
			progressBarMain.Value = progressBarMain.Value = progressBarMain.Maximum / 2;

			// Execute the search
			searchResults = SettingsMap.IsCellModeActive() ?
				DataHelper.SearchCell(textBoxSearch.Text, SettingsCell.GetCell(), GetEnabledSignatures(), GetEnabledLockTypes()) :
				DataHelper.SearchStandard(textBoxSearch.Text, SettingsSearch.searchInterior, GetEnabledSignatures(), GetEnabledLockTypes());

			// Post-query - set progress to 3/4
			progressBarMain.Value = progressBarMain.Value = (int)(progressBarMain.Maximum * 0.75);

			// Perform UI update
			UpdateLocationColumnVisibility();
			UpdateResultsLockTypeColumnVisibility();
			UpdateAmountColumnToolTip();
			textBoxSearch.Select();
			textBoxSearch.SelectAll();

			UpdateSearchResultsGrid();
			NotifyIfNoResults();

			// Search complete - re-enable UI and set progress bar to full
			buttonSearch.Enabled = true;
			progressBarMain.Value = progressBarMain.Maximum;
		}

		// Scrap search
		void ButtonSearchScrap(object sender, EventArgs e)
		{
			// Disable the button to reduce stacking search operations and reset the progress bar
			buttonSearchScrap.Enabled = false;
			progressBarMain.Value = progressBarMain.Minimum;

			UpdateResultsLockTypeColumnVisibility();
			UpdateLocationColumnVisibility();

			// Pre-query - set progress to 1/2
			progressBarMain.Value = progressBarMain.Value = progressBarMain.Maximum / 2;

			searchResults = DataHelper.SearchScrap(listBoxScrap.SelectedItem.ToString());

			// Post-query - set progress to 3/4
			progressBarMain.Value = progressBarMain.Value = (int)(progressBarMain.Maximum * 0.75);

			UpdateSearchResultsGrid();
			NotifyIfNoResults();

			// Search complete - re-enable UI and set progress bar to full
			buttonSearchScrap.Enabled = true;
			progressBarMain.Value = progressBarMain.Maximum;
		}

		// NPC Search
		void ButtonSearchNPC(object sender, EventArgs e)
		{
			// Disable the button to reduce stacking search operations and reset the progress bar
			buttonSearchNPC.Enabled = false;
			progressBarMain.Value = progressBarMain.Minimum;

			UpdateResultsLockTypeColumnVisibility();
			UpdateLocationColumnVisibility();

			// Pre-query - set progress to 1/2
			progressBarMain.Value = progressBarMain.Value = progressBarMain.Maximum / 2;

			searchResults = DataHelper.SearchNPC(
				listBoxNPC.SelectedItem.ToString(),
				SettingsSearch.spawnChance);

			// Post-query - set progress to 3/4
			progressBarMain.Value = progressBarMain.Value = (int)(progressBarMain.Maximum * 0.75);

			UpdateSearchResultsGrid();
			NotifyIfNoResults();

			// Search complete - re-enable UI and set progress bar to full
			buttonSearchNPC.Enabled = true;
			progressBarMain.Value = progressBarMain.Maximum;
		}

		// Add selected valid items from the search results to the legend.
		private void ButtonAddToLegend(object sender, EventArgs e)
		{
			int totalItems = gridViewSearchResults.SelectedRows.Count;

			// Only use or animate the progress bar for adding to legend if this is a large operation
			bool useProgressBar = totalItems > 5000;
			float progress = 0;

			if (totalItems == 0)
			{
				Notify.Info("Please select items from the search results first.");
				return;
			}

			List<string> rejectedItemsDuplicate = new List<string>(); // Items rejected because they're already present
			List<string> rejectedItemsInterior = new List<string>(); // Items rejected because they belong to a cell
			int legendGroup = -1;

			// Get a single legend group for this group of item(s)
			if (checkBoxAddAsGroup.Checked)
			{
				legendGroup = FindLowestAvailableLegendGroupValue();
			}

			// Record the contents of the legend before we start adding to it
			List<MapItem> legendItemsBeforeAdd = new List<MapItem>(legendItems);

			if (useProgressBar)
			{
				progressBarMain.Value = progressBarMain.Minimum;
			}

			foreach (DataGridViewRow row in gridViewSearchResults.SelectedRows.Cast<DataGridViewRow>().Reverse())
			{
				MapItem selectedItem = searchResults[Convert.ToInt32(row.Cells["columnSearchIndex"].Value)];

				// Warn if a selected item is a cell item or already on the legend list - otherwise add it.
				if (selectedItem.location != "Appalachia" && !SettingsMap.IsCellModeActive())
				{
					rejectedItemsInterior.Add(selectedItem.editorID);
				}
				else if (legendItemsBeforeAdd.Contains(selectedItem))
				{
					rejectedItemsDuplicate.Add(selectedItem.editorID);
				}
				else // Item is fine - add it
				{
					// If the legend group is already fixed (added as group) use that, otherwise use a new legend group
					selectedItem.legendGroup = (legendGroup == -1) ?
						FindLowestAvailableLegendGroupValue() :
						legendGroup;

					legendItems.Add(selectedItem);
				}

				if (useProgressBar)
				{
					progress++;
					progressBarMain.Value = (int)(progress / totalItems * progressBarMain.Maximum);
				}
			}

			int totalRejectedItems = rejectedItemsInterior.Count + rejectedItemsDuplicate.Count;

			// Update the legend grid, as long as there's at least one item we didn't have to reject
			if (totalRejectedItems < gridViewSearchResults.SelectedRows.Count)
			{
				UpdateLegendGrid(null);
			}

			// If we dropped items, let the user know why.
			if (totalRejectedItems > 0)
			{
				// Cap the list of items we warn about to prevent a huge error box
				int maxItemsToShow = 8;
				int truncatedItems = totalRejectedItems - maxItemsToShow;

				string message = "The following items were not added to the legend because ";
				if (rejectedItemsDuplicate.Count > 0 && rejectedItemsInterior.Count > 0)
				{
					message += "some were already present, and others were from internal cells.";
				}
				else if (rejectedItemsDuplicate.Count > 0)
				{
					message += "some were already present.";
				}
				else if (rejectedItemsInterior.Count > 0)
				{
					message += "some were from internal cells.";
				}

				if (rejectedItemsInterior.Count > 0)
				{
					message += "\nIf there is a specific internal cell you wish to make maps for, you can do so in Cell Mode.\n" +
						"(Map > Advanced Modes > Cell mode)";
				}

				// Add a line to say that a further x items (not shown) were not added
				message += "\n\n" + string.Join("\n", rejectedItemsInterior.Concat(rejectedItemsDuplicate).Take(maxItemsToShow)) + 
					(truncatedItems > 0 ? "\n(+ " + truncatedItems + " more...)" : string.Empty);

				Notify.Info(message); 
			}
		}

		// Remove selected rows from legend DataGridView
		private void ButtonRemoveFromLegend(object sender, EventArgs e)
		{
			if (gridViewLegend.SelectedRows.Count == 0)
			{
				Notify.Info("Please select items from the 'items to plot' list first.");
				return;
			}

			// If they are removing *everything*, then we can skip the processing below and just wipe the list
			if (gridViewLegend.SelectedRows.Count == legendItems.Count)
			{
				ClearLegend();
				return;
			}

			List<MapItem> remainingLegendItems = new List<MapItem>();
			List<int> selectedIndexes = new List<int>();

			// Removing multiple list items by index breaks, as the indices keep changing
			// So we find which items *weren't* selected, then make a new list of remaining items from that
			foreach (DataGridViewRow row in gridViewLegend.SelectedRows)
			{
				selectedIndexes.Add(row.Index);
			}

			for (int i = 0; i < legendItems.Count; i++)
			{
				if (!selectedIndexes.Contains(i))
				{
					remainingLegendItems.Add(legendItems[i]);
				}
				else
				{
					legendItems[i].overridingLegendText = string.Empty;
				}
			}

			legendItems = remainingLegendItems;

			UpdateLegendGrid(null);
		}

		// Handle the entry and assignment of new values to the Legend Group column or Overriding legend text
		private void GridViewLegend_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewRow editedRow = gridViewLegend.Rows[e.RowIndex];
			DataGridViewCell editedCell = editedRow.Cells[e.ColumnIndex];
			object cellValue = editedCell.Value;
			MapItem editedItem = legendItems[e.RowIndex];

			// Legend group
			if (e.ColumnIndex == 0)
			{
				// First verify the value entered into Legend Group cell is a non-null positive int
				if (cellValue == null ||
					!uint.TryParse(cellValue.ToString(), out _) ||
					!int.TryParse(cellValue.ToString(), out _))
				{
					Notify.Warn("\"" + editedCell.Value + "\" must be a positive whole number.");
					editedCell.Value = 0; // Overwrite the edit by assigning 0, but continue the method so that the value of 0 is reflected in the MapItem too.
				}

				// Record the overridden legend texts, *before* we edit the legend group, in case the edit conflicts
				Dictionary<int, string> overriddenLegendTexts = GatherOverriddenLegendTexts();

				int legendGroup = int.Parse(editedCell.Value.ToString());
				editedItem.legendGroup = legendGroup;

				// We just edited into a legend group which is already overridden
				// Inherit the overriding text of the one which came first
				if (overriddenLegendTexts.ContainsKey(legendGroup))
				{
					editedItem.overridingLegendText = overriddenLegendTexts[legendGroup];
					BeginInvoke((MethodInvoker)delegate { UpdateLegendGrid(editedItem); });
				}
			}
			else if (e.ColumnIndex == 1) // Override legend text
			{
				int targetLegendGroup = int.Parse(editedRow.Cells[0].Value.ToString());

				/*Loop over all items of the same legend group (including of the edited row),
				assigning the overriding text, or reverting to default if blank*/
				foreach (MapItem mapItem in legendItems.FindAll(m => m.legendGroup == targetLegendGroup))
				{
					// If the value entered was blank/deleted, reset to default
					if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
					{
						mapItem.overridingLegendText = string.Empty;
					}
					else // Use the overridden text
					{
						mapItem.overridingLegendText = cellValue.ToString();
					}
				}

				BeginInvoke((MethodInvoker)delegate { UpdateLegendGrid(editedItem); });
			}
		}

		void ButtonDrawMap(object sender, EventArgs e)
		{
			DrawMap(false);
		}

		// Double click map for preview
		private void MapPreview_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			PreviewMap();
		}

		// Record left click location on map in order to pan with mouse
		private void MapPreview_MouseDown(object sender, MouseEventArgs mouseEvent)
		{
			if (mouseEvent.Button == MouseButtons.Left)
			{
				lastMouseDownPos = mouseEvent.Location;
			}
		}

		// Pan the map while left mouse is held
		private void MapPreview_MouseMove(object sender, MouseEventArgs mouseEvent)
		{
			if (mouseEvent.Button == MouseButtons.Left)
			{
				int deltaX = mouseEvent.Location.X - lastMouseDownPos.X;
				int deltaY = mouseEvent.Location.Y - lastMouseDownPos.Y;

				int newX = pictureBoxMapPreview.Location.X + deltaX;
				int newY = pictureBoxMapPreview.Location.Y + deltaY;

				pictureBoxMapPreview.Location = new Point(newX, newY);
			}
		}

		// Pick up scroll wheel events and apply them to zoom the map preview
		protected override void OnMouseWheel(MouseEventArgs mouseEvent)
		{
			int minZoom = Map.minZoom;
			int maxZoom = Map.maxZoom;

			// Record the center coord of the 'viewport' (The container of the PictureBox)
			int viewPortCenterX = splitContainerMain.Panel2.Width / 2;
			int viewPortCenterY = splitContainerMain.Panel2.Height / 2;

			// Record initial positions
			Point location = pictureBoxMapPreview.Location;
			int width = pictureBoxMapPreview.Size.Width;
			int height = pictureBoxMapPreview.Size.Height;
			Point apparentCenter = new Point(
				(int)((viewPortCenterX - location.X) * (Map.mapDimension / (float)width)),
				(int)((viewPortCenterY - location.Y) * (Map.mapDimension / (float)height)));

			// Calculate how much to zoom
			float magnitude = Math.Abs(mouseEvent.Delta) / 1500f;
			float zoomRatio = 1 + ((mouseEvent.Delta > 0) ? magnitude : -magnitude);

			// Calculate the new dimensions once zoomed, given zoom limits
			int newWidth = Math.Max(Math.Min(maxZoom, (int)(width * zoomRatio)), minZoom);
			int newHeight = Math.Max(Math.Min(maxZoom, (int)(height * zoomRatio)), minZoom);

			// Calculate the required position offset while also factoring the offset to keep the apparent center pixel the same
			int widthOffset = (width - newWidth) * apparentCenter.X / Map.mapDimension;
			int heightOffset = (height - newHeight) * apparentCenter.Y / Map.mapDimension;

			// Apply the new zoom and position
			pictureBoxMapPreview.Size = new Size(newWidth, newHeight);
			pictureBoxMapPreview.Location = new Point(location.X + widthOffset, location.Y + heightOffset);
		}

		// Select items in the filters just by clicking their label
		void ListViewFilterSignatures_SelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			if (e.IsSelected)
			{
				e.Item.Checked = !e.Item.Checked;
			}
		}

		// Select items in the filters just by clicking their label
		void ListViewFilterLockTypes_SelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			if (e.IsSelected)
			{
				e.Item.Checked = !e.Item.Checked;
			}
		}

		// Show dynamic tooltips on certain columns
		void GridViewSearchResults_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{
			// Ignore the header
			if (e.RowIndex == -1)
			{
				return;
			}

			// Indentify which cell was hovered
			DataGridViewCell hoveredCell = gridViewSearchResults.Rows[e.RowIndex].Cells[e.ColumnIndex];

			// Explain the signature AKA category
			if (gridViewSearchResults.Columns[e.ColumnIndex].Name == "columnSearchCategory")
			{
				hoveredCell.ToolTipText = DataHelper.GetSignatureDescription(hoveredCell.Value.ToString());
			}

			// Explain what is meant by "unknown" under spawn chance
			else if (gridViewSearchResults.Columns[e.ColumnIndex].Name == "columnSearchChance")
			{
				if (hoveredCell.Value.ToString() == "Unknown")
				{
					hoveredCell.ToolTipText = "For a number of possible reasons, Mappalachia is unable to provide a single, static figure to describe the spawn chances of this entity.";
				}
			}

			// Give the technical name of an interior cell display name for those curious
			else if (gridViewSearchResults.Columns[e.ColumnIndex].Name == "columnSearchLocation")
			{
				hoveredCell.ToolTipText = gridViewSearchResults.Rows[e.RowIndex].Cells["columnSearchLocationID"].Value.ToString();
			}
		}

		// Explain Legend Group on mouseover with tooltip
		private void GridViewLegend_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{
			// Only apply to Legend Group column
			DataGridViewColumn column = gridViewLegend.Columns[e.ColumnIndex];
			if (column.HeaderText != "Legend Group")
			{
				return;
			}

			string legendGroupTooltip = "The legend group allows you to group items together, so they are represented by the same color and/or shape.";

			// Assign the tooltip to the column header
			column.ToolTipText = legendGroupTooltip;

			// Also assign to any cell (not the header)
			if (e.RowIndex != -1)
			{
				gridViewLegend.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = legendGroupTooltip;
			}
		}

		// Change the default enter action depending on the currently selected control
		void ListBoxNPC_MouseEnter(object sender, EventArgs e)
		{
			AcceptButton = buttonSearchNPC;
		}

		// Change the default enter action depending on the currently selected control
		void NumericUpDownNPCSpawnThreshold_MouseEnter(object sender, EventArgs e)
		{
			AcceptButton = buttonSearchNPC;
		}

		// Change the default enter action depending on the currently selected control
		void ListBoxScrap_SelectedIndexChanged(object sender, EventArgs e)
		{
			AcceptButton = buttonSearchScrap;
		}

		// Change the default enter action depending on the currently selected control
		void TabControlMain_SelectedIndexChanged(object sender, EventArgs e)
		{
			AcceptButton = tabControlStandardNPCJunk.SelectedTab == tabPageStandard ? buttonSearch : buttonSearchNPC;
		}

		// User updated value in min spawn chance - update the setting too
		private void NumericUpDownNPCSpawnThreshold_ValueChanged(object sender, EventArgs e)
		{
			SettingsSearch.spawnChance = (int)numericUpDownNPCSpawnThreshold.Value;
		}

		#endregion

		void FormMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			IOManager.Cleanup();
			SettingsManager.SaveSettings();

			try
			{
				// Ensures any potentially long-running map building task is stopped too
				Environment.Exit(0);
			}
			catch (Exception)
			{ } // If this fails, we're already exiting and there is no action to take
		}
	}
}
