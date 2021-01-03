using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Mappalachia.Class;
using Mappalachia.Forms;
using Microsoft.Data.Sqlite;

namespace Mappalachia
{
	//The main form with map preview and all GUI controls inside it
	public partial class FormMaster : Form
	{
		public ProgressBar progressBar;

		public static List<MapItem> legendItems = new List<MapItem>();
		public static List<MapItem> searchResults = new List<MapItem>();

		//Flags on if we've displayed certain warnings, so as to only show once per run
		static bool warnedLVLINotUsed = false;

		static Point lastMouseDownPos;

		public FormMaster()
		{
			InitializeComponent();

			Map.progressBarMain = progressBarMain;

			//Cleanup on launch in case it didn't run last time
			IOManager.Cleanup();

			//Instantiate the DB connection
			Queries.CreateConnection();

			progressBar = progressBarMain;

			//Populate UI elements
			PopulateSignatureFilterList();
			PopulateLockTypeFilterList();
			PopulateVariableNPCSpawnList();
			PopulateScrapList();
			ProvideSearchHint();

			//Auto-select text box text and use search button with enter
			textBoxSearch.Select();
			textBoxSearch.SelectAll();
			AcceptButton = buttonSearch;

			//Apply UI layouts according to current settings
			UpdateLocationColumnVisibility();
			UpdateResultsLockTypeColumnVisibility();
			UpdateLegendLockTypeColumnVisibility();
			UpdateVolumeEnabledState();
			UpdateAmountToolTip();
			UpdatePlotMode(false);
			UpdateHeatMapColorMode(false);
			UpdateHeatMapResolution(false);
			UpdateMapLayerSettings(false);
			UpdateMapGrayscale(false);
			UpdateSearchInterior();
			UpdateShowFormID();
			UpdateFilterWarnings();

			Map.SetOutput(pictureBoxMapPreview);

			//Draw and display the map
			Map.DrawBaseLayer();
		}

		//All Methods not directly responding to UI input
		#region Methods

		//Dynamically fill the Signature filter with every signature present based on the data
		void PopulateSignatureFilterList()
		{
			foreach (string signature in DataHelper.GetPermittedSignatures())
			{
				ListViewItem thisItem = listViewFilterSignatures.Items.Add(signature);
				thisItem.Text = DataHelper.ConvertSignature(thisItem.Text, false);
				thisItem.ToolTipText = DataHelper.GetSignatureDescription(signature);
				thisItem.Checked = true;
			}
		}

		//Dynamically fill the Lock Type filter with every lock type present based on the data
		void PopulateLockTypeFilterList()
		{
			foreach (string lockType in DataHelper.GetPermittedLockTypes())
			{
				ListViewItem thisItem = listViewFilterLockTypes.Items.Add(DataHelper.ConvertLockLevel(lockType, false));
				thisItem.Checked = true;
			}
		}

		//Fill the list of Variable NPC Spawns that can be chosen to search
		void PopulateVariableNPCSpawnList()
		{
			foreach (string npcSpawn in DataHelper.GetVariableNPCTypes())
			{
				listBoxNPC.Items.Add(npcSpawn);
			}

			listBoxNPC.SelectedIndex = 0;
		}

		//Fill the list of Scrap types that can be chosen to search
		void PopulateScrapList()
		{
			foreach (string npcSpawn in DataHelper.GetVariableScrapTypes())
			{
				listBoxScrap.Items.Add(npcSpawn);
			}

			listBoxScrap.SelectedIndex = 0;
		}

		//Fill the search box with a suggested search term.
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

		//Update the visibility of the search results location column, given current settings
		void UpdateLocationColumnVisibility()
		{
			gridViewSearchResults.Columns["columnSearchLocation"].Visible = SettingsSearch.searchInterior;
		}

		//Update the visiblity of the lock type column in the results view, given current settings
		void UpdateResultsLockTypeColumnVisibility()
		{
			//Check if the lock type filter is in use - if it is we probably want to show the column.
			if (tabControlSimpleNPCJunk.SelectedTab == tabPageSimple)
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

			//If they are all checked, or this is not a search where filters apply - hide the column again
			gridViewSearchResults.Columns["columnSearchLockLevel"].Visible = false;
		}

		//Update the visiblity of the lock type column in the legend view, given current settings
		void UpdateLegendLockTypeColumnVisibility()
		{
			//If any mapped items have lock type relevancy, show the column
			foreach (MapItem item in legendItems)
			{
				if (item.GetLockRelevant())
				{
					gridViewLegend.Columns["columnLegendLockType"].Visible = true;
					return;
				}
			}

			//No reason to show the column, so hide it
			gridViewLegend.Columns["columnLegendLockType"].Visible = false;
		}

		//Update the map settings > draw volume check, based on current settings
		void UpdateVolumeEnabledState()
		{
			drawVolumesMenuItem.Checked = SettingsPlot.volumeEnabled;
		}

		//Update tooltip on "amount" column header - as it changes depending on interior searching or not
		void UpdateAmountToolTip()
		{
			gridViewSearchResults.Columns["columnSearchAmount"].ToolTipText = SettingsSearch.searchInterior ?
				"The amount of instances which can be found in the listed location." :
				"The amount of instances which can be found on the surface of Appalachia.";
		}

		//Update the Plot Settings > Mode options based on the actual value in PlotSettings
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
				Map.Draw();
			}
		}

		//Update the UI with the currently selected heatmap color mode
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

			if (reDraw && SettingsPlot.mode == SettingsPlot.Mode.Heatmap)
			{
				Map.Draw();
			}
		}

		//Update the UI with the currently selected heatmap resolution
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

			if (reDraw && SettingsPlot.mode == SettingsPlot.Mode.Heatmap)
			{
				Map.Draw();
			}
		}

		//Update check marks in the UI with current MapSettings, and redraw the map if true
		void UpdateMapLayerSettings(bool reDraw)
		{
			layerMilitaryMenuItem.Checked = SettingsMap.layerMilitary;
			layerNWFlatwoodsMenuItem.Checked = SettingsMap.layerNWFlatwoods;
			layerNWMorgantownMenuItem.Checked = SettingsMap.layerNWMorgantown;

			if (reDraw)
			{
				Map.DrawBaseLayer();
			}
		}

		//Update check mark in the UI with current MapSettings for grayscale
		void UpdateMapGrayscale(bool reDraw)
		{
			grayscaleMenuItem.Checked = SettingsMap.grayScale;

			if (reDraw)
			{
				Map.DrawBaseLayer();
			}
		}

		//Update check mark in the UI for Search Interiors option
		void UpdateSearchInterior()
		{
			interiorSearchMenuItem.Checked = SettingsSearch.searchInterior;
		}

		//Update check mark in the UI for Show FormID option
		void UpdateShowFormID()
		{
			showFormIDMenuItem.Checked = SettingsSearch.showFormID;
			gridViewSearchResults.Columns["columnSearchFormID"].Visible = SettingsSearch.showFormID;
		}

		//Update check mark in the UI for disable filter warnings option
		void UpdateFilterWarnings()
		{
			disableFilterWarningsMenuItem.Checked = !SettingsSearch.filterWarnings;
		}

		//Unselect all resolution options under heatmap resolution. Used to remove any current selection
		void UncheckAllResolutions()
		{
			resolution128MenuItem.Checked = false;
			resolution256MenuItem.Checked = false;
			resolution512MenuItem.Checked = false;
			resolution1024MenuItem.Checked = false;
		}

		//Collect the enabled signatures from the UI to a list for use by a query
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

		//Collect the enabled lock types from the UI to a list for use by a query
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

		//Conduct the simple search with given parameters
		void GatherSearchResultsSimple(string searchTerm, bool searchInteriors, List<string> allowedSignatures, List<string> allowedLockTypes)
		{
			try
			{
				using (SqliteDataReader reader = Queries.ExecuteQuerySimpleSearch(searchInteriors, searchTerm, allowedSignatures, allowedLockTypes))
				{
					while (reader.Read())
					{
						searchResults.Add(new MapItem(
							Type.Simple,
							reader.GetString(5), //FormID
							reader.GetString(1), //Editor ID
							reader.GetString(0), //Display Name
							reader.GetString(2), //Signature
							allowedLockTypes, //The Lock Types filtered for this set of items.
							DataHelper.GetSpawnChance(reader.GetString(2)), //Spawn chance
							reader.GetInt32(3), //Count
							reader.GetString(6), //Cell Display Name/location
							reader.GetString(7))); //Cell EditorID
					}
				}
			}
			catch (Exception e)
			{
				Notify.Error("Mappalachia encountered an error while searching the database:\n" +
				IOManager.genericExceptionHelpText +
				e);
			}
		}

		//Search for variable NPC Spawns of the given NPC, where their spawn chance is above the threshold
		void GatherSearchResultsNPC(string searchTerm, int minChance)
		{
			try
			{
				using (SqliteDataReader reader = Queries.ExecuteQueryNPCSearch(searchTerm, minChance / 100.00, SettingsSearch.searchInterior))
				{
					//Collect some variables which will always be the same for every result and are required for an instance of MapItem
					string signature = DataHelper.ConvertSignature("NPC_", false);
					List<string> lockTypes = DataHelper.GetPermittedLockTypes();

					while (reader.Read())
					{
						//Sub-query for interior can return null
						if (reader.IsDBNull(0))
						{
							continue;
						}

						double spawnChance = Math.Round(reader.GetDouble(2), 2);

						searchResults.Add(new MapItem(
							Type.NPC,
							reader.GetString(0), //FormID
							reader.GetString(0) + " (Up to " + spawnChance + "%)", //Editor ID
							reader.GetString(0), //Display Name
							signature,
							lockTypes, //The Lock Types filtered for this set of items.
							spawnChance,
							reader.GetInt32(1), //Count
							reader.GetString(3), //Cell Display Name/location
							reader.GetString(4))); //Cell editorID
					}
				}

				//Expand the NPC search, by also conducting a simple search of only NPC_, ignorant of lock filter
				GatherSearchResultsSimple(searchTerm, SettingsSearch.searchInterior, new List<string> { "NPC_" }, DataHelper.GetPermittedLockTypes());

				//Remove search results containing "corpse" as these are dead and not traditional NPCs
				//This isn't perfect and won't remove all dead NPCs. Common pattern seems to be many prefixed with 'Enc' are dead, but not all
				List<MapItem> itemsWithoutCorpse = new List<MapItem>();
				foreach (MapItem item in searchResults)
				{
					if (item.editorID.Contains("corpse") || item.editorID.Contains("Corpse"))
					{
						continue;
					}
					else
					{
						itemsWithoutCorpse.Add(item);
					}
				}

				searchResults = new List<MapItem>(itemsWithoutCorpse);
			}
			catch (Exception e)
			{
				Notify.Error("Mappalachia encountered an error while searching the database:\n" +
				IOManager.genericExceptionHelpText +
				e);
			}
		}

		//Search for junk items containing the given Scrap name
		void GatherSearchResultsScrap(string searchTerm)
		{
			try
			{
				using (SqliteDataReader reader = Queries.ExecuteQueryScrapSearch(searchTerm, SettingsSearch.searchInterior))
				{
					//Collect some variables which will always be the same for every result and are required for an instance of MapItem
					string signature = DataHelper.ConvertSignature("MISC", false);
					List<string> lockTypes = DataHelper.GetPermittedLockTypes();
					double spawnChance = DataHelper.GetSpawnChance("MISC");

					while (reader.Read())
					{
						//Sub-query for interior can return null
						if (reader.IsDBNull(0))
						{
							continue;
						}

						searchResults.Add(new MapItem(
							Type.Scrap,
							reader.GetString(0), //FormID
							reader.GetString(0) + " scraps from junk", //Editor ID
							reader.GetString(0), //Display Name
							signature,
							lockTypes, //The Lock Types filtered for this set of items.
							spawnChance,
							reader.GetInt32(1), //Count
							reader.GetString(2), //Cell Display Name/location
							reader.GetString(3))); //Cell editorID
					}
				}
			}
			catch (Exception e)
			{
				Notify.Error("Mappalachia encountered an error while searching the database:\n" +
				IOManager.genericExceptionHelpText +
				e);
			}
		}

		//Warn the user if they appear to be trying to search for something that might actually be in a LVLI, but they have unselected it
		void WarnWhenLVLINotSelected()
		{
			//Already warned - we only do this once per session
			if (warnedLVLINotUsed)
			{
				return;
			}

			if (SettingsSearch.filterWarnings)
			{
				List<string> enabledSignatures = GetEnabledSignatures();
				if (!enabledSignatures.Contains("LVLI"))
				{
					//A list of signatures that seem to typically be represented by LVLI
					foreach (string signatureToWarn in new List<string> { "FLOR", "ALCH", "WEAP", "ARMO", "BOOK", "AMMO" })
					{
						if (enabledSignatures.Contains(signatureToWarn))
						{
							Notify.Info(
								"Your search results may be restricted because you have 'Loot' unchecked under the categories filter.\n" +
								"Confusingly, many items in Fallout 76 are categorized generically as 'Loot', rather than what you may expect.\n" +
								"If you can't find what you're looking for, make sure to enable it and search again.\n\n" +
								"You may disable this warning under 'Search Settings>Disable Filter Warnings'");
							warnedLVLINotUsed = true;
							return;
						}
					}
				}
			}
		}

		//Warn the user if all filters of a category are blank and therefore no results would match
		void WarnWhenAllFiltersBlank()
		{
			if (GetEnabledSignatures().Count == 0 || GetEnabledLockTypes().Count == 0)
			{
				Notify.Info(
					"No search results were found because you have disabled every filter for a category.\n" +
					"You must enable at least one filter per category to find anything.");
				return;
			}
		}

		//Wipe and re-populate the search results UI element with the items in "List<MapItem> searchResults"
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
					mapItem.locationID,
					index); //Index helps relate the UI row to the List<MapItem>, even if the UI is sorted

				index++;
			}

			gridViewSearchResults.Enabled = true;
		}

		//Wipe and re-populate the Legend Grid View with items contained in "List<MapItem> legendItems"
		void UpdateLegendGrid()
		{
			gridViewLegend.Enabled = false;
			gridViewLegend.Rows.Clear();

			foreach (MapItem legend in legendItems)
			{
				gridViewLegend.Rows.Add(legend.legendGroup, legend.editorID, legend.displayName, legend.GetLockRelevant() ? string.Join(", ", DataHelper.ConvertLockLevelCollection(legend.filteredLockTypes, false)) : "N/A");
			}

			UpdateLegendLockTypeColumnVisibility();
			gridViewLegend.Enabled = true;
		}

		//Wipe away the legend items and update the UI. Doesn't re-draw the map
		void ClearLegend()
		{
			legendItems.Clear();
			UpdateLegendGrid();
		}

		//Find the lowest free legend group value in LegendItems
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

		//Find the sum total of unique legend groups
		public static int FindSumLegendGroups()
		{
			List<int> legendGroupNumbers = new List<int>();

			foreach (MapItem item in legendItems)
			{
				if (!legendGroupNumbers.Contains(item.legendGroup))
				{
					legendGroupNumbers.Add(item.legendGroup);
				}
			}

			return legendGroupNumbers.Count;
		}

		//User-activated draw. Draw the plot points onto the map, if there is anything to plot
		void DrawMapFromUI()
		{
			if (legendItems.Count > 0)
			{
				Map.Draw();
			}
			else
			{
				Notify.Info("There is nothing to map. Add items to the 'items to plot' list by first selecting them from search results.");
			}
		}

		void PreviewMap()
		{
			Map.Open();
		}

		#endregion

		//All Methods which represent responses to UI input
		#region UI Controls

		//Map > Plot - Draw the map
		void Map_Plot(object sender, EventArgs e)
		{
			DrawMapFromUI();
		}

		//Map > View - Write the map image to disk and launch in default program
		void Map_View(object sender, EventArgs e)
		{
			PreviewMap();
		}

		//Map > Layer > Military - Toggle the map background to be military
		void Map_Layer_Military(object sender, EventArgs e)
		{
			SettingsMap.layerMilitary = !SettingsMap.layerMilitary;
			UpdateMapLayerSettings(true);
		}

		//Map > Layer > NW Flatwoods - Toggle the NW Flatwoods layer
		void Map_Layer_NWFlatwoods(object sender, EventArgs e)
		{
			SettingsMap.layerNWFlatwoods = !SettingsMap.layerNWFlatwoods;
			UpdateMapLayerSettings(true);
		}

		//Map > Layer > NW MorganTown - Toggle the NW Morgantown later
		void Map_Layer_NWMorgantown(object sender, EventArgs e)
		{
			SettingsMap.layerNWMorgantown = !SettingsMap.layerNWMorgantown;
			UpdateMapLayerSettings(true);
		}

		//Map > Brightness... - Open the brightness adjust form
		void Map_Brightness(object sender, EventArgs e)
		{
			FormSetBrightness formSetBrightness = new FormSetBrightness();
			formSetBrightness.ShowDialog();
		}

		//Map > Grayscale - Toggle grayscale drawing of map base layer
		private void Map_Grayscale(object sender, EventArgs e)
		{
			SettingsMap.grayScale = !SettingsMap.grayScale;
			UpdateMapGrayscale(true);
		}

		//Map > Export To File - Export the map image to file
		void Map_Export(object sender, EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog
			{
				Filter = "JPEG|*.jpeg",
				FileName = "Mappalachia Map",
			};

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				Map.WriteToFile(dialog.FileName);
			}
		}

		//Map > Clear - Remove legend items and remove plotted layers from the map
		void Map_Clear(object sender, EventArgs e)
		{
			ClearLegend();
			buttonAddToLegend.Enabled = true;
			Map.Draw();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		//Map > Reset - Hard reset the map, all layers, plots, legend items and pan/zoom
		void Map_Reset(object sender, EventArgs e)
		{
			ClearLegend();
			Map.Reset();

			UpdateMapLayerSettings(false);
			UpdateMapGrayscale(false);

			//Reset pan and zoom
			pictureBoxMapPreview.Location = new Point(0, 0);
			pictureBoxMapPreview.Width = splitContainerMain.Panel1.Width;
			pictureBoxMapPreview.Height = splitContainerMain.Panel1.Height;

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		//Search Settings > Toggle interiors - Toggle including interiors in search results
		void Search_Interior(object sender, EventArgs e)
		{
			SettingsSearch.searchInterior = !SettingsSearch.searchInterior;
			UpdateSearchInterior();
		}

		//Search Settings > Show FormID - Toggle visibility of FormID column
		void Search_FormID(object sender, EventArgs e)
		{
			SettingsSearch.showFormID = !SettingsSearch.showFormID;
			UpdateShowFormID();
		}

		//Search Settings > Disable filter warnings - Disable warnings for Signature filter misuse
		void Search_FilterWarnings(object sender, EventArgs e)
		{
			SettingsSearch.filterWarnings = !SettingsSearch.filterWarnings;
			UpdateFilterWarnings();
		}

		//Plot Settings > Mode > Icon - Change plot mode to icon
		private void Plot_Mode_Icon(object sender, EventArgs e)
		{
			SettingsPlot.mode = SettingsPlot.Mode.Icon;
			UpdatePlotMode(true);
		}

		//Plot Settings > Mode > Heatmap - Change plot mode to heatmap
		private void Plot_Mode_Heatmap(object sender, EventArgs e)
		{
			SettingsPlot.mode = SettingsPlot.Mode.Heatmap;
			UpdatePlotMode(true);
		}

		//Plot Settings > Plot Icon Settings - Open plot settings form
		private void Plot_PlotIconSettings(object sender, EventArgs e)
		{
			FormPlotIconSettings formPlotSettings = new FormPlotIconSettings();
			formPlotSettings.ShowDialog();
		}

		//Plot Setting > Heatmap Settings > Color Mode > Mono - Change color mode to mono
		private void Plot_HeatMap_ColorMode_Mono(object sender, EventArgs e)
		{
			SettingsPlotHeatmap.colorMode = SettingsPlotHeatmap.ColorMode.Mono;
			UpdateHeatMapColorMode(true);
		}

		//Plot Setting > Heatmap Settings > Color Mode > Duo - Change color mode to duo
		private void Plot_HeatMap_ColorMode_Duo(object sender, EventArgs e)
		{
			SettingsPlotHeatmap.colorMode = SettingsPlotHeatmap.ColorMode.Duo;
			UpdateHeatMapColorMode(true);
		}

		//Plot Setting > Heatmap Settings > Resolution > 128 - Change resolution to 128
		private void Plot_HeatMap_Resolution_128(object sender, EventArgs e)
		{
			SettingsPlotHeatmap.resolution = 128;
			UpdateHeatMapResolution(true);
		}

		//Plot Setting > Heatmap Settings > Resolution > 256 - Change resolution to 256
		private void Plot_HeatMap_Resolution_256(object sender, EventArgs e)
		{
			SettingsPlotHeatmap.resolution = 256;
			UpdateHeatMapResolution(true);
		}

		//Plot Setting > Heatmap Settings > Resolution > 512 - Change resolution to 512
		private void Plot_HeatMap_Resolution_512(object sender, EventArgs e)
		{
			SettingsPlotHeatmap.resolution = 512;
			UpdateHeatMapResolution(true);
		}

		//Plot Setting > Heatmap Settings > Resolution > 1024 - Change resolution to 1024
		private void Plot_HeatMap_Resolution_1024(object sender, EventArgs e)
		{
			SettingsPlotHeatmap.resolution = 1024;
			UpdateHeatMapResolution(true);
		}

		//Plot Settings > Draw Volumes - Toggle drawing volumes
		private void Plot_DrawVolumes(object sender, EventArgs e)
		{
			SettingsPlot.volumeEnabled = !SettingsPlot.volumeEnabled;
			UpdateVolumeEnabledState();
		}

		//Help > About - Show the About box
		void Help_About(object sender, EventArgs e)
		{
			FormAbout aboutBox = new FormAbout();
			aboutBox.ShowDialog();
		}

		//Help > User Guides - Open help guides at github master
		void Help_UserGuides(object sender, EventArgs e)
		{
			Process.Start("https://github.com/AHeroicLlama/Mappalachia/tree/master/Guides_user");
		}

		//Help > Check for Updates - Open the releases at github master
		private void Help_CheckForUpdates(object sender, EventArgs e)
		{
			Process.Start("https://github.com/AHeroicLlama/Mappalachia/releases/latest");
		}

		//Donate to the Author - Launch donate URL
		void Donate(object sender, EventArgs e)
		{
			Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=TDVKFJ97TFFVC&source=url");
		}

		//Signature select all
		void ButtonSelectAllSignature(object sender, EventArgs e)
		{
			foreach (ListViewItem item in listViewFilterSignatures.Items)
			{
				item.Checked = true;
			}
		}

		//Signature deselect all
		void ButtonDeselectAllSignature(object sender, EventArgs e)
		{
			foreach (ListViewItem item in listViewFilterSignatures.Items)
			{
				item.Checked = false;
			}
		}

		//Lock select all
		void ButtonSelectAllLock(object sender, EventArgs e)
		{
			foreach (ListViewItem item in listViewFilterLockTypes.Items)
			{
				item.Checked = true;
			}
		}

		//Lock deselect all
		void ButtonDeselectAllLock(object sender, EventArgs e)
		{
			foreach (ListViewItem item in listViewFilterLockTypes.Items)
			{
				item.Checked = false;
			}
		}

		//Search Button - Gather parameters, execute query and populate results
		void ButtonSearchSimple(object sender, EventArgs e)
		{
			//Check for and show warnings
			WarnWhenLVLINotSelected();
			WarnWhenAllFiltersBlank();

			//Clear the results from last search
			searchResults.Clear();

			//Execute the search
			GatherSearchResultsSimple(textBoxSearch.Text, SettingsSearch.searchInterior, GetEnabledSignatures(), GetEnabledLockTypes());

			//Perform UI update
			UpdateLocationColumnVisibility();
			UpdateResultsLockTypeColumnVisibility();
			UpdateAmountToolTip();
			textBoxSearch.Select();
			textBoxSearch.SelectAll();

			UpdateSearchResultsGrid();
		}

		//Scrap search
		void ButtonSearchScrap(object sender, EventArgs e)
		{
			UpdateResultsLockTypeColumnVisibility();
			UpdateLocationColumnVisibility();
			searchResults.Clear();
			GatherSearchResultsScrap(listBoxScrap.SelectedItem.ToString());
			UpdateSearchResultsGrid();
		}

		//NPC Search
		void ButtonSearchNPC(object sender, EventArgs e)
		{
			UpdateResultsLockTypeColumnVisibility();
			UpdateLocationColumnVisibility();
			searchResults.Clear();
			GatherSearchResultsNPC(
				listBoxNPC.SelectedItem.ToString(),
				(int)numericUpDownNPCSpawnThreshold.Value);
			UpdateSearchResultsGrid();
		}

		//Add selected valid items from the search results to the legend.
		private void ButtonAddToLegend(object sender, EventArgs e)
		{
			if (gridViewSearchResults.SelectedRows.Count == 0)
			{
				Notify.Info("Please select items from the search results first.");
				return;
			}

			List<string> rejectedItems = new List<string>();

			foreach (DataGridViewRow row in gridViewSearchResults.SelectedRows.Cast<DataGridViewRow>().Reverse())
			{
				MapItem selectedItem = searchResults[Convert.ToInt32(row.Cells["columnSearchIndex"].Value)];

				//Warn if a selected item is not valid - otherwise add it.
				if (selectedItem.location != "Appalachia" || legendItems.Contains(selectedItem))
				{
					rejectedItems.Add(selectedItem.editorID);
				}
				else
				{
					selectedItem.legendGroup = FindLowestAvailableLegendGroupValue();
					legendItems.Add(selectedItem);
				}
			}

			UpdateLegendGrid();

			if (rejectedItems.Count > 0)
			{
				Notify.Info(
					"The following items were not added to the legend because either they already existed on the legend, or " +
					"they were items from an interior cell, and are therefore not suitable to be placed on the world map.\n\n" +
					string.Join("\n", rejectedItems));
			}
		}

		//Remove selected rows from legend DataGridView
		private void ButtonRemoveFromLegend(object sender, EventArgs e)
		{
			if (gridViewLegend.SelectedRows.Count == 0)
			{
				Notify.Info("Please select items from the 'items to plot' list first.");
				return;
			}

			buttonAddToLegend.Enabled = true;
			List<MapItem> remainingLegendItems = new List<MapItem>();

			//Removing multiple list items by index breaks, as the indices keep changing
			//So we instead re-create and replace the legendItems list minus the removed items
			foreach (DataGridViewRow row in gridViewLegend.Rows)
			{
				if (!gridViewLegend.SelectedRows.Contains(row))
				{
					remainingLegendItems.Add(legendItems[row.Index]);
				}
			}

			legendItems = new List<MapItem>(remainingLegendItems);

			UpdateLegendGrid();
		}

		//Handle the entry and assignment of new values to the Legend Group column
		//This method assumes the Legend Group cell was edited as this is the only editable column
		private void GridViewLegend_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewCell cell = gridViewLegend.Rows[e.RowIndex].Cells[0];
			object cellValue = cell.Value;

			//First verify the value entered into Legend Group cell is a non-null positive int
			if (cellValue == null ||
				!uint.TryParse(cellValue.ToString(), out _) ||
				!int.TryParse(cellValue.ToString(), out _))
			{
				Notify.Warn("\"" + cell.Value + "\" must be a positive whole number.");
				cell.Value = 0; //Overwrite the edit by assigning 0, but continue the method so that the value of 0 is reflected in the MapItem too.
			}

			MapItem item = legendItems[e.RowIndex];
			item.legendGroup = int.Parse(cell.Value.ToString());
		}

		void ButtonDrawMap(object sender, EventArgs e)
		{
			DrawMapFromUI();
		}

		//Double click map for preview
		private void MapPreview_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			PreviewMap();
		}

		//Record left click location on map in order to pan with mouse
		private void MapPreview_MouseDown(object sender, MouseEventArgs mouseEvent)
		{
			if (mouseEvent.Button == MouseButtons.Left)
			{
				lastMouseDownPos = mouseEvent.Location;
			}
		}

		//Pan the map while left mouse is held
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

		//Pick up scroll wheel events and apply them to zoom the map preview
		protected override void OnMouseWheel(MouseEventArgs mouseEvent)
		{
			int minZoom = Map.minZoom;
			int maxZoom = Map.maxZoom;

			//Record the center coord of the 'viewport' (The container of the PictureBox)
			int viewPortCenterX = splitContainerMain.Panel2.Width / 2;
			int viewPortCenterY = splitContainerMain.Panel2.Height / 2;

			//Record initial positions
			Point location = pictureBoxMapPreview.Location;
			int width = pictureBoxMapPreview.Size.Width;
			int height = pictureBoxMapPreview.Size.Height;
			Point apparentCenter = new Point(
				(int)((viewPortCenterX - location.X) * (Map.mapDimension / (float)width)),
				(int)((viewPortCenterY - location.Y) * (Map.mapDimension / (float)height)));

			//Calculate how much to zoom
			float magnitude = Math.Abs(mouseEvent.Delta) / 1500f;
			float zoomRatio = 1 + ((mouseEvent.Delta > 0) ? magnitude : -magnitude);

			//Calculate the new dimensions once zoomed, given zoom limits
			int newWidth = Math.Max(Math.Min(maxZoom, (int)(width * zoomRatio)), minZoom);
			int newHeight = Math.Max(Math.Min(maxZoom, (int)(height * zoomRatio)), minZoom);

			//Calculate the required position offset while also factoring the offset to keep the apparent center pixel the same
			int widthOffset = (width - newWidth) * apparentCenter.X / Map.mapDimension;
			int heightOffset = (height - newHeight) * apparentCenter.Y / Map.mapDimension;

			//Apply the new zoom and position
			pictureBoxMapPreview.Size = new Size(newWidth, newHeight);
			pictureBoxMapPreview.Location = new Point(location.X + widthOffset, location.Y + heightOffset);
		}

		//Select items in the filters just by clicking their label
		void ListViewFilterSignatures_SelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			if (e.IsSelected)
			{
				e.Item.Checked = !e.Item.Checked;
			}
		}

		//Select items in the filters just by clicking their label
		void ListViewFilterLockTypes_SelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			if (e.IsSelected)
			{
				e.Item.Checked = !e.Item.Checked;
			}
		}

		//Show dynamic tooltips on certain columns
		void GridViewSearchResults_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{
			//Ignore the header
			if (e.RowIndex == -1)
			{
				return;
			}

			//Indentify which cell was hovered
			DataGridViewCell hoveredCell = gridViewSearchResults.Rows[e.RowIndex].Cells[e.ColumnIndex];

			//Explain the signature AKA category
			if (gridViewSearchResults.Columns[e.ColumnIndex].Name == "columnSearchCategory")
			{
				hoveredCell.ToolTipText = DataHelper.GetSignatureDescription(hoveredCell.Value.ToString());
			}

			//Explain what is meant by "varies" under spawn chance
			else if (gridViewSearchResults.Columns[e.ColumnIndex].Name == "columnSearchChance")
			{
				if (hoveredCell.Value.ToString() == "Unknown")
				{
					hoveredCell.ToolTipText = "For a number of possible reasons, Mappalachia is unable to provide a single, static figure to describe the spawn chances of this entity.";
				}
			}

			//Give the technical name of an interior cell display name for those curious
			else if (gridViewSearchResults.Columns[e.ColumnIndex].Name == "columnSearchLocation")
			{
				hoveredCell.ToolTipText = gridViewSearchResults.Rows[e.RowIndex].Cells["columnSearchLocationID"].Value.ToString();
			}
		}

		//Explain Legend Group on mouseover with tooltip
		private void GridViewLegend_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{
			//Only apply to Legend Group column
			DataGridViewColumn column = gridViewLegend.Columns[e.ColumnIndex];
			if (column.HeaderText != "Legend Group")
			{
				return;
			}

			string legendGroupTooltip = "The legend group allows you to group items together, so they are represented by the same color and/or shape.";

			//Assign the tooltip to the column header
			column.ToolTipText = legendGroupTooltip;

			//Also assign to any cell (not the header)
			if (e.RowIndex != -1)
			{
				gridViewLegend.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = legendGroupTooltip;
			}
		}

		//Change the default enter action depending on the currently selected control
		void ListBoxNPC_MouseEnter(object sender, EventArgs e)
		{
			AcceptButton = buttonSearchNPC;
		}

		//Change the default enter action depending on the currently selected control
		void NumericUpDownNPCSpawnThreshold_MouseEnter(object sender, EventArgs e)
		{
			AcceptButton = buttonSearchNPC;
		}

		//Change the default enter action depending on the currently selected control
		void ListBoxScrap_SelectedIndexChanged(object sender, EventArgs e)
		{
			AcceptButton = buttonSearchScrap;
		}

		//Change the default enter action depending on the currently selected control
		void TabControlMain_SelectedIndexChanged(object sender, EventArgs e)
		{
			AcceptButton = tabControlSimpleNPCJunk.SelectedTab == tabPageSimple ? buttonSearch : buttonSearchNPC;
		}

		#endregion

		void FormMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			IOManager.Cleanup();
		}
	}
}