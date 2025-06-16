using System.ComponentModel;
using System.Data;
using System.Drawing.Imaging;
using Library;

namespace Mappalachia
{
	public partial class FormMain : Form
	{
		public Settings Settings { get; private set; } = Settings.LoadFromFile();

		FormMapView FormMapView { get; set; }

		BindingList<GroupedInstance> SearchResults { get; set; } = new BindingList<GroupedInstance>();

		BindingList<GroupedInstance> ItemsToPlot { get; set; } = new BindingList<GroupedInstance>();

		public FormMain()
		{
			InitializeComponent();

			UpdateFromSettings(false);

			UpdateChecker.CheckForUpdates(Settings);

			InitializeDataGridView(dataGridViewSearchResults, SearchResults);
			InitializeDataGridView(dataGridViewItemsToPlot, ItemsToPlot);

			comboBoxSpace.DataSource = Database.AllSpaces;
			comboBoxSpace.DisplayMember = "FriendlyName";
			comboBoxSpace.SelectedItem = Settings.Space;

			foreach (ToolStripMenuItem item in new[] { mapMenuItem, mapMapMarkersMenuItem, mapBackgroundImageMenuItem, mapLegendStyleMenuItem })
			{
				item.DropDown.Closing += DontCloseClickedDropDown;
			}

			FormMapView = new FormMapView(Settings);
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
					return "The total number of identical instances of this entity, at this location";

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
					return (!boundData.InContainer && !advanced) ? "This entity is placed directly in the world" : string.Empty;

				case "Location":
					return advanced ? boundData.Space.DisplayName : boundData.Space.EditorID;

				default:
					return string.Empty;
			}
		}

		// Read in fields from Settings and update UI elements respectively
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

			UpdateDataGridAppearences();

			// TODO - Is doing this every time inelegant?
			if (reDraw)
			{
				FormMapView.UpdateMap(Settings);
			}
		}

		// Sets the tooltip/mouse-over text for cells and column headers
		void SetDataGridCellToolTip(DataGridView dataGridView, DataGridViewCellToolTipTextNeededEventArgs e)
		{
			// Row header - exit
			if (e.ColumnIndex == -1)
			{
				return;
			}

			string columnName = dataGridView.Columns[e.ColumnIndex].Name;

			// Column header
			if (e.RowIndex == -1)
			{
				e.ToolTipText = GetColumnHeaderToolTip(columnName, Settings.SearchSettings.Advanced);
				return;
			}

			GroupedInstance instance = (GroupedInstance)(dataGridView.Rows[e.RowIndex].DataBoundItem ?? throw new Exception($"Column {e.RowIndex} bound to null"));
			e.ToolTipText = GetCellToolTip(columnName, instance, Settings.SearchSettings.Advanced);
		}

		// Programatically configure either DGV
		void InitializeDataGridView(DataGridView dataGridView, BindingList<GroupedInstance> data)
		{
			dataGridView.AutoGenerateColumns = false;
			dataGridView.Columns.Clear();
			dataGridView.DataSource = new BindingSource(data, string.Empty);

			dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "FormID", DataPropertyName = "DataValueFormID", });
			dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "EditorID", DataPropertyName = "DataValueEditorID" });
			dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "DisplayName", DataPropertyName = "DataValueDisplayName" });
			dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Signature", DataPropertyName = "DataValueSignature" });
			dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Label", DataPropertyName = "DataValueLabel" });
			dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "InContainer", DataPropertyName = "DataValueInContainer" });
			dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "LockLevel", DataPropertyName = "DataValueLockLevel" });
			dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "SpawnWeight", DataPropertyName = "DataValueSpawnWeight" });
			dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Count", DataPropertyName = "DataValueCount" });
			dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Location", DataPropertyName = "DataValueLocation" });

			UpdateDataGridAppearences();

			dataGridView.CellToolTipTextNeeded += (s, e) => SetDataGridCellToolTip(dataGridView, e);
		}

		// Applies the header text and column visibility of all DGV Columns, based on Settings
		// We don't need to update tooltips as they are fetched dynamically via CellToolTipNeeded events
		void UpdateDataGridAppearences()
		{
			foreach (DataGridView dataGridView in new List<DataGridView>() { dataGridViewSearchResults, dataGridViewItemsToPlot })
			{
				foreach (DataGridViewColumn column in dataGridView.Columns)
				{
					column.HeaderText = GetColumnHeader(column.Name, Settings.SearchSettings.Advanced);

					// Hide the Form ID column except when in advanced mode
					if (column.Name.Equals("FormID"))
					{
						column.Visible = Settings.SearchSettings.Advanced;
					}
				}
			}
		}

		// Shorthand to apply a new user-selected setting, update the UI, and optionally redraw the map
		void SetSetting(Action setSetting, bool redraw = true)
		{
			setSetting();
			UpdateFromSettings(redraw);
		}

		void DontCloseClickedDropDown(object? sender, ToolStripDropDownClosingEventArgs e)
		{
			if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
			{
				e.Cancel = true;
			}
		}

		// Defer showing the map view until after the main form has been shown
		private void FormMain_Shown(object sender, EventArgs e)
		{
			FormMapView.Show();
			BringToFront();
		}

		private async void ButtonSearch_Click(object sender, EventArgs e)
		{
			List<GroupedInstance> searchResults = await Database.Search(Settings);

			searchResults = searchResults
				.OrderByDescending(g => g.Space.Equals(Settings.Space))
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

		private void Map_ShowPreview_Click(object sender, EventArgs e)
		{
			FormMapView.BringToFront();
			FormMapView.Show();
			FormMapView.Focus();

			if (FormMapView.WindowState == FormWindowState.Minimized)
			{
				FormMapView.WindowState = FormWindowState.Normal;
				FormMapView.SizeMapToForm();
			}
		}

		private void Map_OpenExternally(object sender, EventArgs e)
		{
			FileIO.TempSave(FormMapView.GetCurrentMapImage(), true);
		}

		private void Map_Grayscale_Click(object sender, EventArgs e)
		{
			SetSetting(() => Settings.MapSettings.GrayscaleBackground = !Settings.MapSettings.GrayscaleBackground);
		}

		private void Map_HightlightWater_Click(object sender, EventArgs e)
		{
			SetSetting(() => Settings.MapSettings.HighlightWater = !Settings.MapSettings.HighlightWater);
		}

		private void Map_MapMarkers_Icons_Click(object sender, EventArgs e)
		{
			SetSetting(() => Settings.MapSettings.MapMarkerIcons = !Settings.MapSettings.MapMarkerIcons);
		}

		private void Map_MapMarkers_Labels_Click(object sender, EventArgs e)
		{
			SetSetting(() => Settings.MapSettings.MapMarkerLabels = !Settings.MapSettings.MapMarkerLabels);
		}

		private void Map_Background_Normal_Click(object sender, EventArgs e)
		{
			SetSetting(() => Settings.MapSettings.BackgroundImage = BackgroundImageType.Menu);
		}

		private void Map_Background_Satellite_Click(object sender, EventArgs e)
		{
			SetSetting(() => Settings.MapSettings.BackgroundImage = BackgroundImageType.Render);
		}

		private void Map_Background_Military_Click(object sender, EventArgs e)
		{
			SetSetting(() => Settings.MapSettings.BackgroundImage = BackgroundImageType.Military);
		}

		private void Map_Background_None_Click(object sender, EventArgs e)
		{
			SetSetting(() => Settings.MapSettings.BackgroundImage = BackgroundImageType.None);
		}

		private void Map_Legend_Normal_Click(object sender, EventArgs e)
		{
			SetSetting(() => Settings.MapSettings.LegendStyle = LegendStyle.Normal);
		}

		private void Map_Legend_Extended_Click(object sender, EventArgs e)
		{
			SetSetting(() => Settings.MapSettings.LegendStyle = LegendStyle.Extended);
		}

		private void Map_Legend_Hidden_Click(object sender, EventArgs e)
		{
			SetSetting(() => Settings.MapSettings.LegendStyle = LegendStyle.None);
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
			FileIO.QuickSave(FormMapView.GetCurrentMapImage(), Settings);
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
					Image saveTarget = FormMapView.GetCurrentMapImage();
					ImageFormat imageFormat = formExportToFile.ImageFormat;

					// If PNG is recommended and selected, set black backgrounds transparent
					if (FileIO.GetImageFileTypeRecommendation(Settings) == ImageFormat.Png && imageFormat == ImageFormat.Png)
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
			Settings.MapSettings = new MapSettings(Settings);
			Settings.ResolveConflictingSettings();

			ItemsToPlot.Clear();
			FormMapView.SizeMapToForm();

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
			SetSetting(() => Settings.SearchSettings.SearchInAllSpaces = !Settings.SearchSettings.SearchInAllSpaces, true);
		}

		private void Search_AdvancedMode_Click(object sender, EventArgs e)
		{
			SetSetting(() => Settings.SearchSettings.Advanced = !Settings.SearchSettings.Advanced, true);
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
			FormMapView.SizeMapToForm();
			UpdateFromSettings();
			Settings.ResolveConflictingSettings();
		}

		private void Discord_Click(object sender, EventArgs e)
		{
			Common.OpenURI(URLs.DiscordInvite);
		}

		private void Donate_Click(object sender, EventArgs e)
		{
			Common.OpenURI(URLs.DonatePaypal);
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
				// Collect the instances attached to the rows
				// Then in a separate loop (to avoid removing from the list which we are looping), remove them
				List<GroupedInstance> itemsToRemove = selectedRows.Select(row => (GroupedInstance)(row?.DataBoundItem ?? throw new Exception("Row was or was bound to null"))).ToList();

				foreach (GroupedInstance instance in itemsToRemove)
				{
					ItemsToPlot.Remove(instance);
				}
			}

			ItemsToPlot.RaiseListChangedEvents = true;
			ItemsToPlot.ResetBindings();
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.SaveToFile();
			FileIO.Cleanup();
		}
	}
}
