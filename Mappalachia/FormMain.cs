using System.ComponentModel;
using System.Data;
using System.Drawing.Imaging;
using Library;

namespace Mappalachia
{
	public partial class FormMain : Form
	{
		Settings Settings { get; set; } = Settings.LoadFromFile();

		FormMapView MapViewForm { get; set; }

		BindingList<GroupedInstance> SearchResults { get; set; } = new BindingList<GroupedInstance>();

		BindingList<GroupedInstance> ItemsToPlot { get; set; } = new BindingList<GroupedInstance>();

		public FormMain()
		{
			InitializeComponent();

			UpdateChecker.CheckForUpdates(Settings);

			// Spawn the map view form
			MapViewForm = new FormMapView();
			MapViewForm.Show();

			InitializeSearchResultsGrid();
			InitializeItemsToPlotGrid();
			InitializeSpaceDropDown();

			UpdateFromSettings();

			mapMenuItem.DropDown.Closing += DontCloseClickedDropDown;
			mapMapMarkersMenuItem.DropDown.Closing += DontCloseClickedDropDown;
			mapBackgroundImageMenuItem.DropDown.Closing += DontCloseClickedDropDown;
			mapLegendStyleMenuItem.DropDown.Closing += DontCloseClickedDropDown;
		}

		// Adds the columns to the search results and items to plot
		static void AddDGVColumns(DataGridView dgv)
		{
			dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "FormID" });
			dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "EditorID" });
			dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "DisplayName" });
			dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Signature" });
			dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Label" });
			dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "InContainer" });
			dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "LockLevel" });
			dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "SpawnWeight" });
			dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Count" });
			dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Location" });

			foreach (DataGridViewColumn column in dgv.Columns)
			{
				column.SortMode = DataGridViewColumnSortMode.Automatic;
			}
		}

		// Return the header for a DataGridViewColumn with the given name
		static string GetColumnHeader(string columnName, bool advanced)
		{
			switch (columnName)
			{
				case "FormID":
					return "Form ID";

				case "EditorID":
					return advanced ? "Editor ID" : "Technical Name";

				case "Signature":
					return advanced ? "Signature" : "Category";

				case "SpawnWeight":
					return "Weighted Chance %";

				default:
					return string.Concat(columnName.Select(c => char.IsUpper(c) ? " " + c : c.ToString())).Trim();
			}
		}

		// Return the tooltip for the header of a DataGridViewColumn with the given name
		static string GetColumnHeaderToolTip(string columnName, bool advanced)
		{
			switch (columnName)
			{
				case "FormID":
					return advanced ? string.Empty : "An internal code name for this entity";

				case "EditorID":
					return advanced ? string.Empty : "The developer's name for this entity";

				case "Signature":
					return advanced ? string.Empty : "The category for this entity";

				case "SpawnWeight":
					return "The effective expected value of entities at each individual instance. Most entities are always present (100%).\n" +
						"However for example an NPC may only spawn 1/5 of the time (20%), but a container may contain 2x of the item (200%), and a piece of junk may contain 3 of the specified scrap (300%)";

				case "Count":
					return "The total number of instances this entity, at this location";

				case "Location":
					return "The location where this entity may be found";

				case "Label":
					return "A unique developer tag for a particular instance of this entity";

				default:
					return string.Empty;
			}
		}

		// Return the tooltip for a data cell of a DataGridViewColumn with the given name, and bound data
		static string GetCellToolTip(string columnName, GroupedInstance boundData, bool advanced)
		{
			switch (columnName)
			{
				case "Signature":
					return advanced ? boundData.Entity.Signature.ToFriendlyName() : boundData.Entity.Signature.GetDescription();

				case "InContainer":
					return (boundData.InContainer || advanced) ? string.Empty : "This entity is placed directly in the world";

				case "Location":
					return advanced ? boundData.Space.DisplayName : boundData.Space.EditorID;

				default:
					return string.Empty;
			}
		}

		// Does the data mapping from the row's bound object to the cell
		void MapDataGridData(DataGridViewCellFormattingEventArgs e, DataGridView dataGridView)
		{
			GroupedInstance instance = (GroupedInstance)(dataGridView.Rows[e.RowIndex].DataBoundItem ?? throw new Exception($"Column {e.RowIndex} bound to null"));
			string columnName = dataGridViewSearchResults.Columns[e.ColumnIndex].Name;

			switch (columnName)
			{
				case "FormID":
					e.Value = instance.Entity.FormID.ToHex();
					break;

				case "EditorID":
					e.Value = instance.Entity.EditorID;
					break;

				case "DisplayName":
					e.Value = instance.Entity.DisplayName;
					break;

				case "Signature":
					e.Value = Settings.SearchSettings.Advanced ? instance.Entity.Signature : instance.Entity.Signature.ToFriendlyName();
					break;

				case "Label":
					e.Value = instance.Label;
					break;

				case "InContainer":
					e.Value = instance.InContainer ? "Yes" : "No";
					break;

				case "LockLevel":
					e.Value = !instance.Entity.Signature.IsLockable() && !instance.InContainer && instance.LockLevel == LockLevel.None ?
						"N/A" : instance.LockLevel.ToFriendlyName();
					break;

				case "SpawnWeight":
					e.Value = Math.Round(instance.SpawnWeight * 100, 2);
					break;

				case "Count":
					e.Value = instance.Count;
					break;

				case "Location":
					e.Value = Settings.SearchSettings.Advanced ? instance.Space.EditorID : instance.Space.DisplayName;
					break;

				default:
					throw new Exception($"Column {columnName} not mapped to any data");
			}
		}

		// Reads in fields from Settings and updates UI elements respectively
		void UpdateFromSettings(bool reDraw = true)
		{
			comboBoxSpace.SelectedItem = Settings.Space;

			grayscaleMenuItem.Checked = Settings.MapSettings.GrayscaleBackground;
			highlightWaterMenuItem.Checked = Settings.MapSettings.HighlightWater;
			mapMarkerIconsMenuItem.Checked = Settings.MapSettings.MapMarkerIcons;
			mapMarkerLabelsMenuItem.Checked = Settings.MapSettings.MapMarkerLabels;

			backgroundNormalMenuItem.Checked = false;
			backgroundMilitaryMenuItem.Checked = false;
			backgroundSatelliteMenuItem.Checked = false;
			backgroundNoneMenuItem.Checked = false;

			switch (Settings.MapSettings.BackgroundImage)
			{
				case BackgroundImageType.Menu:
					backgroundNormalMenuItem.Checked = true;
					break;
				case BackgroundImageType.Render:
					backgroundSatelliteMenuItem.Checked = true;
					break;
				case BackgroundImageType.Military:
					backgroundMilitaryMenuItem.Checked = true;
					break;
				case BackgroundImageType.None:
					backgroundNoneMenuItem.Checked = true;
					break;
				default:
					throw new Exception($"Invalid {nameof(Settings.MapSettings.BackgroundImage)} value {Settings.MapSettings.BackgroundImage}");
			}

			legendNormalMenuItem.Checked = false;
			legendExtendedMenuItem.Checked = false;
			legendHiddenMenuItem.Checked = false;

			switch (Settings.MapSettings.LegendStyle)
			{
				case LegendStyle.Normal:
					legendNormalMenuItem.Checked = true;
					break;
				case LegendStyle.Extended:
					legendExtendedMenuItem.Checked = true;
					break;
				case LegendStyle.None:
					legendHiddenMenuItem.Checked = true;
					break;
				default:
					throw new Exception($"Invalid {nameof(Settings.MapSettings.LegendStyle)} value {Settings.MapSettings.LegendStyle}");
			}

			backgroundNormalMenuItem.Enabled = Settings.Space.IsWorldspace;
			backgroundMilitaryMenuItem.Enabled = Settings.Space.IsAppalachia();

			highlightWaterMenuItem.Enabled = Settings.Space.IsWorldspace;
			mapMapMarkersMenuItem.Enabled = Settings.Space.IsWorldspace;

			textBoxSearch.Text = Settings.SearchSettings.SearchTerm;
			searchInAllSpacesToolStripMenuItem.Checked = Settings.SearchSettings.SearchInAllSpaces;
			advancedModeToolStripMenuItem.Checked = Settings.SearchSettings.Advanced;

			textBoxSearch.Text = Settings.SearchSettings.SearchTerm;

			SetDataGridAppearences();

			// TODO - Doing this every time is inelegant.
			if (reDraw)
			{
				MapViewForm.UpdateMap(Settings);
			}
		}

		// Applies the header text and column visibility of all DGV Columns, based on Settings
		// We don't need to update tooltips as they are fetched dynamically via events
		void SetDataGridAppearences()
		{
			foreach (DataGridView dataGridView in new List<DataGridView>() { dataGridViewSearchResults, dataGridViewItemsToPlot })
			{
				foreach (DataGridViewColumn column in dataGridView.Columns)
				{
					column.HeaderText = GetColumnHeader(column.Name, Settings.SearchSettings.Advanced);

					if (column.Name.Equals("FormID"))
					{
						column.Visible = Settings.SearchSettings.Advanced;
					}
				}
			}
		}

		// Sets the tooltip/mouse-over text for cells and column headers
		void SetDataGridToolTip(DataGridViewCellToolTipTextNeededEventArgs e)
		{
			// Row header - exit
			if (e.ColumnIndex < 0)
			{
				return;
			}

			string columnName = dataGridViewSearchResults.Columns[e.ColumnIndex].Name;

			// Column header
			if (e.RowIndex == -1)
			{
				e.ToolTipText = GetColumnHeaderToolTip(columnName, Settings.SearchSettings.Advanced);
				return;
			}

			GroupedInstance instance = (GroupedInstance)(dataGridViewSearchResults.Rows[e.RowIndex].DataBoundItem ?? throw new Exception($"Column {e.RowIndex} bound to null"));
			e.ToolTipText = GetCellToolTip(columnName, instance, Settings.SearchSettings.Advanced);
		}

		// Setup the columns and styling for the search results grid
		void InitializeSearchResultsGrid()
		{
			dataGridViewSearchResults.AutoGenerateColumns = false;
			dataGridViewSearchResults.Columns.Clear();

			BindingSource source = new BindingSource(SearchResults, string.Empty);
			dataGridViewSearchResults.DataSource = source;

			AddDGVColumns(dataGridViewSearchResults);

			SetDataGridAppearences();

			dataGridViewSearchResults.CellFormatting += (s, e) =>
			{
				MapDataGridData(e, dataGridViewSearchResults);
			};

			dataGridViewSearchResults.CellToolTipTextNeeded += (s, e) =>
			{
				SetDataGridToolTip(e);
			};
		}

		// Setup the columns and styling for the items to plot grid
		void InitializeItemsToPlotGrid()
		{
			dataGridViewItemsToPlot.AutoGenerateColumns = false;
			dataGridViewItemsToPlot.Columns.Clear();

			BindingSource source = new BindingSource(ItemsToPlot, string.Empty);
			dataGridViewItemsToPlot.DataSource = source;

			AddDGVColumns(dataGridViewItemsToPlot);

			SetDataGridAppearences();

			dataGridViewItemsToPlot.CellFormatting += (s, e) =>
			{
				MapDataGridData(e, dataGridViewItemsToPlot);
			};

			dataGridViewItemsToPlot.CellToolTipTextNeeded += (s, e) =>
			{
				SetDataGridToolTip(e);
			};
		}

		void InitializeSpaceDropDown()
		{
			comboBoxSpace.DataSource = Database.AllSpaces;
			comboBoxSpace.DisplayMember = "FriendlyName";
		}

		void DontCloseClickedDropDown(object? sender, ToolStripDropDownClosingEventArgs e)
		{
			if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
			{
				e.Cancel = true;
			}
		}

		private void Map_ShowPreview_Click(object sender, EventArgs e)
		{
			MapViewForm.BringToFront();
			MapViewForm.Show();
			MapViewForm.Focus();
		}

		private void Discord_Click(object sender, EventArgs e)
		{
			Common.OpenURI(URLs.DiscordInvite);
		}

		private void Help_About_Click(object sender, EventArgs e)
		{
			new FormAbout().ShowDialog();
		}

		private void Help_UserGuides_Click(object sender, EventArgs e)
		{
			Common.OpenURI(URLs.HelpDocs);
		}

		private void Help_CheckForUpdates_Click(object sender, EventArgs e)
		{
			UpdateChecker.CheckForUpdates(Settings, true);
		}

		private void Help_ViewGitHub_Click(object sender, EventArgs e)
		{
			Common.OpenURI(URLs.GitHub);
		}

		// Note that this creates a new instance of Settings, so references passed will become disconnected
		private void Help_ResetEverything_Click(object sender, EventArgs e)
		{
			Settings = new Settings();
			SearchResults.Clear();
			ItemsToPlot.Clear();
			MapViewForm.SizeMapToForm();
			UpdateFromSettings();
		}

		private void Donate_Click(object sender, EventArgs e)
		{
			Common.OpenURI(URLs.DonatePaypal);
		}

		private async void ButtonSearch_Click(object sender, EventArgs e)
		{
			List<GroupedInstance> searchResults = await Database.Search(Settings);

			searchResults = searchResults
				.OrderByDescending(g => g.Space == Settings.Space)
				.ThenByDescending(g => g.Count)
				.ThenByDescending(g => g.SpawnWeight)
				.ThenBy(g => g.Entity.EditorID)
				.ToList();

			SearchResults.RaiseListChangedEvents = false;

			SearchResults.Clear();

			foreach (GroupedInstance instance in searchResults)
			{
				SearchResults.Add(instance);
			}

			SearchResults.RaiseListChangedEvents = true;
			SearchResults.ResetBindings();
		}

		private void Map_Grayscale_Click(object sender, EventArgs e)
		{
			Settings.MapSettings.GrayscaleBackground = !Settings.MapSettings.GrayscaleBackground;
			UpdateFromSettings();
		}

		private void Map_HightlightWater_Click(object sender, EventArgs e)
		{
			Settings.MapSettings.HighlightWater = !Settings.MapSettings.HighlightWater;
			UpdateFromSettings();
		}

		private void Map_MapMarkers_Icons_Click(object sender, EventArgs e)
		{
			Settings.MapSettings.MapMarkerIcons = !Settings.MapSettings.MapMarkerIcons;
			UpdateFromSettings();
		}

		private void Map_MapMarkers_Labels_Click(object sender, EventArgs e)
		{
			Settings.MapSettings.MapMarkerLabels = !Settings.MapSettings.MapMarkerLabels;
			UpdateFromSettings();
		}

		private void Map_Background_Normal_Click(object sender, EventArgs e)
		{
			Settings.MapSettings.BackgroundImage = BackgroundImageType.Menu;
			UpdateFromSettings();
		}

		private void Map_Background_Satellite_Click(object sender, EventArgs e)
		{
			Settings.MapSettings.BackgroundImage = BackgroundImageType.Render;
			UpdateFromSettings();
		}

		private void Map_Background_Military_Click(object sender, EventArgs e)
		{
			Settings.MapSettings.BackgroundImage = BackgroundImageType.Military;
			UpdateFromSettings();
		}

		private void Map_Background_None_Click(object sender, EventArgs e)
		{
			Settings.MapSettings.BackgroundImage = BackgroundImageType.None;
			UpdateFromSettings();
		}

		private void Map_Legend_Normal_Click(object sender, EventArgs e)
		{
			Settings.MapSettings.LegendStyle = LegendStyle.Normal;
			UpdateFromSettings();
		}

		private void Map_Legend_Extended_Click(object sender, EventArgs e)
		{
			Settings.MapSettings.LegendStyle = LegendStyle.Extended;
			UpdateFromSettings();
		}

		private void Map_Legend_Hidden_Click(object sender, EventArgs e)
		{
			Settings.MapSettings.LegendStyle = LegendStyle.None;
			UpdateFromSettings();
		}

		private void Map_SetTitle_Click(object sender, EventArgs e)
		{
			FormSetTitle titleForm = new FormSetTitle(Settings);

			if (titleForm.ShowDialog() == DialogResult.OK)
			{
				Settings.MapSettings.Title = titleForm.TextBoxValue;
				UpdateFromSettings();
			}
		}

		private void Map_QuickSave_Click(object sender, EventArgs e)
		{
			FileIO.QuickSave(MapViewForm.GetCurrentMapImage(), Settings);
			mapMenuItem.DropDown.Close();
		}

		private void Map_ExportToFile_Click(object sender, EventArgs e)
		{
			FileIO.CreateSavedMapsFolder();

			FormExportToFile formExportToFile = new FormExportToFile(Settings);

			if (formExportToFile.ShowDialog() == DialogResult.OK)
			{
				SaveFileDialog saveDialog = new SaveFileDialog
				{
					Filter = formExportToFile.Filter,
					FileName = FileIO.GetRecommendedMapFileName(Settings),
					InitialDirectory = Path.GetFullPath(Paths.SavedMapsPath),
				};

				if (saveDialog.ShowDialog() == DialogResult.OK)
				{
					Image saveTarget = MapViewForm.GetCurrentMapImage();
					ImageFormat imageFormat = formExportToFile.ImageFormat;

					// If PNG is recommended and selected, set black backgrounds transparent
					if (Settings.GetImageFileTypeRecommendation() == ImageFormat.Png && imageFormat == ImageFormat.Png)
					{
						saveTarget.ReplaceBlackWithTransparency();
					}

					FileIO.Save(saveTarget, formExportToFile.ImageFormat, saveDialog.FileName, formExportToFile.JpgQuality);
				}
			}

			mapMenuItem.DropDown.Close();
		}

		private void Map_ClearPlots_Click(object sender, EventArgs e)
		{
			ItemsToPlot.Clear();

			mapMenuItem.DropDown.Close();
			UpdateFromSettings();
		}

		private void Map_Reset_Click(object sender, EventArgs e)
		{
			Settings.MapSettings = new MapSettings();
			ItemsToPlot.Clear();
			MapViewForm.SizeMapToForm();

			mapMenuItem.DropDown.Close();
			UpdateFromSettings();
		}

		private void ComboBoxSpace_SelectionChangeCommitted(object sender, EventArgs e)
		{
			Settings.Space = (Space)(comboBoxSpace.SelectedItem ?? throw new Exception("Space combobox SelectedItem was null"));
			UpdateFromSettings();
		}

		private void Search_SearchInAllSpaces_Click(object sender, EventArgs e)
		{
			Settings.SearchSettings.SearchInAllSpaces = !Settings.SearchSettings.SearchInAllSpaces;
			UpdateFromSettings(false);
		}

		private void Search_AdvancedMode_Click(object sender, EventArgs e)
		{
			Settings.SearchSettings.Advanced = !Settings.SearchSettings.Advanced;
			UpdateFromSettings(false);
		}

		private void SearchTerm_TextChanged(object sender, EventArgs e)
		{
			Settings.SearchSettings.SearchTerm = textBoxSearch.Text;
		}

		private void ButtonAddToMap_Click(object sender, EventArgs e)
		{
			ItemsToPlot.RaiseListChangedEvents = false;

			List<GroupedInstance> itemsToAdd = new List<GroupedInstance>();

			// From the selected cells, find the unique rows, and find the data bound to those
			foreach (DataGridViewRow? row in dataGridViewSearchResults.SelectedCells.Cast<DataGridViewCell>().DistinctBy(c => c.RowIndex).Select(c => c.OwningRow).Reverse())
			{
				GroupedInstance instance = (GroupedInstance)(row?.DataBoundItem ?? throw new Exception("Row was or was bound to null"));

				if (!ItemsToPlot.Contains(instance))
				{
					itemsToAdd.Add(instance);
				}
			}

			// Add the valid items to the actual list
			// We do this in 2 loops to avoid checking the new items against themselves with the contains check
			foreach (GroupedInstance instance in itemsToAdd)
			{
				ItemsToPlot.Add(instance);
			}

			ItemsToPlot.RaiseListChangedEvents = true;
			ItemsToPlot.ResetBindings();
		}

		private void ButtonRemoveFromMap_Click(object sender, EventArgs e)
		{
			ItemsToPlot.RaiseListChangedEvents = false;

			List<DataGridViewRow?> selectedRows = dataGridViewItemsToPlot.SelectedCells.Cast<DataGridViewCell>().DistinctBy(c => c.RowIndex).Select(c => c.OwningRow).ToList();

			// Shortcut to remove all
			if (selectedRows.Count == ItemsToPlot.Count)
			{
				ItemsToPlot.Clear();
			}
			else
			{
				// From the selected rows, find the data bound to those, and remove it from the ItemsToPlot
				foreach (DataGridViewRow? row in selectedRows)
				{
					ItemsToPlot.Remove((GroupedInstance)(row?.DataBoundItem ?? throw new Exception("Row was or was bound to null")));
				}
			}

			ItemsToPlot.RaiseListChangedEvents = true;
			ItemsToPlot.ResetBindings();
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.SaveToFile();
		}
	}
}
