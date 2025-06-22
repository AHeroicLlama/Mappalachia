using System.Data;
using System.Drawing.Imaging;
using KGySoft.ComponentModel;
using Library;

namespace Mappalachia
{
	public partial class FormMain : Form
	{
		static int PlotIconCellPadding { get; } = 5;

		public Settings Settings { get; private set; } = Settings.LoadFromFile();

		FormMapView FormMapView { get; set; }

		SortableBindingList<GroupedSearchResult> SearchResults { get; set; } = new SortableBindingList<GroupedSearchResult>();

		SortableBindingList<GroupedSearchResult> ItemsToPlot { get; set; } = new SortableBindingList<GroupedSearchResult>();

		public FormMain()
		{
			InitializeComponent();

			UpdateChecker.CheckForUpdates(Settings);

			comboBoxSpace.DataSource = Database.AllSpaces;
			comboBoxSpace.DisplayMember = "FriendlyName";

			// Check the saved Space still exists and associate the selection, otherwise default it
			if (Database.AllSpaces.Contains(Settings.Space))
			{
				comboBoxSpace.SelectedItem = Settings.Space;
			}
			else
			{
				comboBoxSpace.SelectedIndex = 0;
			}

			UpdateFromSettings(false);

			ToolStripMenuItem addAsGroup = new ToolStripMenuItem() { Text = "Add as group" };
			addAsGroup.Click += (sender, e) => { ButtonAddToMap_Click(true); };
			buttonAddToMap.ContextMenu = new ContextMenuStrip() { Items = { addAsGroup } };
			buttonAddToMap.Click += (sender, e) => { ButtonAddToMap_Click(false); };

			InitializeSignatureListView();
			InitializeLockLevelListView();

			InitializeDataGridView(dataGridViewSearchResults, SearchResults);
			InitializeDataGridView(dataGridViewItemsToPlot, ItemsToPlot);

			dataGridViewItemsToPlot.Columns.Add(new DataGridViewButtonColumn() { Name = "Plot Icon", FillWeight = 2, DefaultCellStyle = new DataGridViewCellStyle() { BackColor = Color.DarkGray } });

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
				case "LegendGroup":
					return "The group that this instance belongs to, for the purpose of sharing the same plot icon.";

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
		static string GetCellToolTip(string columnName, GroupedSearchResult boundData, bool advanced)
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

		// Initialize the ListViews for the search filters
		static void InitializeListViewGeneric(ListView listView)
		{
			listView.View = View.SmallIcon;
			listView.CheckBoxes = true;
			listView.Scrollable = false;

			// Toggle the check by clicking the label
			listView.ItemSelectionChanged += (sender, eventArgs) =>
			{
				if (eventArgs.IsSelected && eventArgs.Item != null)
				{
					eventArgs.Item.Checked = !eventArgs.Item.Checked;
				}
			};
		}

		// Read in fields from Settings and update UI elements respectively
		void UpdateFromSettings(bool reDraw = true, bool reSearch = false)
		{
			Settings.ResolveConflictingSettings();

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

			foreach (ListViewItem item in listViewSignature.Items)
			{
				Signature data = (Signature)(item.Tag ?? throw new Exception("Signature item was null"));

				item.Checked = Settings.SearchSettings.SelectedSignatures.Contains(data);
				item.Text = Settings.SearchSettings.Advanced ? data.ToString() : data.ToFriendlyName();
				item.ToolTipText = Settings.SearchSettings.Advanced ? data.ToFriendlyName() : data.GetDescription();
			}

			foreach (ListViewItem item in listViewLockLevel.Items)
			{
				item.Checked = Settings.SearchSettings.SelectedLockLevels.Contains((LockLevel)(item.Tag ?? throw new Exception("LockLevel item was null")));
			}

			UpdateDataGridAppearences();

			if (reSearch)
			{
				Search();
			}

			if (reDraw)
			{
				FormMapView.UpdateMap(Settings);
			}
		}

		async void Search()
		{
			List<GroupedSearchResult> searchResults = await Database.Search(Settings);

			searchResults = searchResults
				.OrderByDescending(g => g.Space.Equals(Settings.Space))
				.ThenByDescending(g => g.Count)
				.ThenByDescending(g => g.SpawnWeight)
				.ThenBy(g => g.Entity.EditorID)
				.ToList();

			SearchResults.RaiseListChangedEvents = false;

			SearchResults.Clear();

			foreach (GroupedSearchResult instance in searchResults)
			{
				SearchResults.Add(instance);
			}

			SearchResults.RaiseListChangedEvents = true;
			SearchResults.ResetBindings();

			dataGridViewSearchResults.ClearSort();
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

			GroupedSearchResult instance = (GroupedSearchResult)(dataGridView.Rows[e.RowIndex].DataBoundItem ?? throw new Exception($"Column {e.RowIndex} bound to null"));
			e.ToolTipText = GetCellToolTip(columnName, instance, Settings.SearchSettings.Advanced);
		}

		// If the location cell is double clicked, set the current space to that space
		private void SwitchSpaceOnDoubleClick(DataGridView dataGridView, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || e.ColumnIndex < 0)
			{
				return;
			}

			DataGridViewColumn clickedColumn = dataGridView.Columns[e.ColumnIndex];

			if (clickedColumn.Name == "Location")
			{
				DataGridViewRow clickedRow = dataGridView.Rows[e.RowIndex];
				Space clickedSpace = ((GroupedSearchResult)(clickedRow.DataBoundItem ?? throw new Exception("Double clicked row data was null"))).Space;

				if (!clickedSpace.Equals(Settings.Space))
				{
					SetSetting(() => Settings.Space = Database.AllSpaces.FirstOrDefault(s => s.Equals(clickedSpace)) ?? Settings.Space, true, false);
				}
			}
		}

		// Handle the drawing of plot icons on the cell of plot icon column
		private void DataGridViewItemsToPlot_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.RowIndex < 0 || e.ColumnIndex < 0)
			{
				return;
			}

			DataGridViewColumn paintedColumn = dataGridViewItemsToPlot.Columns[e.ColumnIndex];

			if (paintedColumn.Name == "Plot Icon")
			{
				e.PaintBackground(e.ClipBounds, true);

				DataGridViewRow paintedRow = dataGridViewItemsToPlot.Rows[e.RowIndex];
				GroupedSearchResult data = (GroupedSearchResult)(paintedRow.DataBoundItem ?? throw new NullReferenceException("Painted row data was null"));
				Image icon = data.PlotIcon?.GetImage() ?? throw new NullReferenceException("Data icon image was null");

				Rectangle cellBounds = e.CellBounds;

				double resizeFactor = Math.Min(
					cellBounds.Width / (double)icon.Width,
					cellBounds.Height / (double)icon.Height);

				int width = (int)(icon.Width * resizeFactor) - PlotIconCellPadding;
				int height = (int)(icon.Height * resizeFactor) - PlotIconCellPadding;

				Rectangle iconBounds = new Rectangle(
					cellBounds.X + (cellBounds.Width / 2) - (width / 2),
					cellBounds.Y + (cellBounds.Height / 2) - (height / 2),
					width,
					height);

				if (e.Graphics == null)
				{
					throw new NullReferenceException("Graphics object for cell icon painting event was null");
				}

				e.Graphics.DrawImage(icon, iconBounds);

				e.Handled = true;
			}
		}

		// Programatically configure either DGV
		void InitializeDataGridView(DataGridView dataGridView, SortableBindingList<GroupedSearchResult> data)
		{
			dataGridView.AutoGenerateColumns = false;
			dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dataGridView.DataSource = new BindingSource(data, string.Empty);

			dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "FormID", DataPropertyName = "DataValueFormID", FillWeight = 2 });
			dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "EditorID", DataPropertyName = "DataValueEditorID", FillWeight = 6 });
			dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "DisplayName", DataPropertyName = "DataValueDisplayName", FillWeight = 5 });
			dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Signature", DataPropertyName = "DataValueSignature", FillWeight = 3 });
			dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Label", DataPropertyName = "DataValueLabel", FillWeight = 3 });
			dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "InContainer", DataPropertyName = "DataValueInContainer", FillWeight = 2 });
			dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "LockLevel", DataPropertyName = "DataValueLockLevel", FillWeight = 2 });
			dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "SpawnWeight", DataPropertyName = "DataValueSpawnWeight", DefaultCellStyle = new DataGridViewCellStyle() { Format = "#,0.##" }, FillWeight = 2 });
			dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Count", DataPropertyName = "DataValueCount", DefaultCellStyle = new DataGridViewCellStyle() { Format = "N0" }, FillWeight = 2 });
			dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Location", DataPropertyName = "DataValueLocation", FillWeight = 4 });

			UpdateDataGridAppearences();

			dataGridView.CellToolTipTextNeeded += (s, e) => SetDataGridCellToolTip(dataGridView, e);
			dataGridView.CellDoubleClick += (s, e) => SwitchSpaceOnDoubleClick(dataGridView, e);
		}

		void InitializeSignatureListView()
		{
			InitializeListViewGeneric(listViewSignature);

			listViewSignature.ShowItemToolTips = true;

			// Populate the list with all searchable signatures
			foreach (Signature signature in Enum.GetValues<Signature>().Where(s => s.CanBeSearchedFor()))
			{
				listViewSignature.Items.Add(new ListViewItem(signature.ToFriendlyName())
				{
					Tag = signature,
					ToolTipText = signature.GetDescription(),
					Checked = Settings.SearchSettings.SelectedSignatures.Contains(signature),
				});
			}

			// Whenever a checked status changes, update the Settings
			listViewSignature.ItemChecked += (sender, eventArgs) =>
			{
				Signature item = (Signature)(eventArgs.Item?.Tag ?? throw new Exception("Signature item was null"));

				if (eventArgs.Item.Checked)
				{
					Settings.SearchSettings.SelectedSignatures.Add(item);
				}
				else
				{
					Settings.SearchSettings.SelectedSignatures.RemoveAll(s => s.Equals(item));
				}
			};
		}

		void InitializeLockLevelListView()
		{
			InitializeListViewGeneric(listViewLockLevel);

			// Populate the list with all searchable lock levels
			foreach (LockLevel lockLevel in Enum.GetValues<LockLevel>())
			{
				listViewLockLevel.Items.Add(new ListViewItem(lockLevel.ToFriendlyName())
				{
					Tag = lockLevel,
					Checked = Settings.SearchSettings.SelectedLockLevels.Contains(lockLevel),
				});
			}

			// Whenever a checked status changes, update the Settings
			listViewLockLevel.ItemChecked += (sender, eventArgs) =>
			{
				LockLevel item = (LockLevel)(eventArgs.Item?.Tag ?? throw new Exception("LockLevel item was null"));

				if (eventArgs.Item.Checked)
				{
					Settings.SearchSettings.SelectedLockLevels.Add(item);
				}
				else
				{
					Settings.SearchSettings.SelectedLockLevels.RemoveAll(s => s.Equals(item));
				}
			};
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
		void SetSetting(Action setSetting, bool redraw = true, bool reSearch = false)
		{
			setSetting();
			UpdateFromSettings(redraw, reSearch);
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

		private void ButtonSearch_Click(object sender, EventArgs e)
		{
			Search();
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

		private void Map_SetBrightness_Click(object sender, EventArgs e)
		{
			FormSetBrightness brightnessForm = new FormSetBrightness(Settings);

			if (brightnessForm.ShowDialog() == DialogResult.OK)
			{
				SetSetting(() => Settings.MapSettings.Brightness = brightnessForm.BrightnessValue);
			}
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
				SetSetting(() => Settings.MapSettings.Title = titleForm.TextBoxValue);
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
			dataGridViewItemsToPlot.ClearSort();

			mapMenuItem.DropDown.Close();
			UpdateFromSettings();
		}

		private void Map_Reset_Click(object sender, EventArgs e)
		{
			Settings.MapSettings = new MapSettings(Settings);
			Settings.ResolveConflictingSettings();

			ItemsToPlot.Clear();
			dataGridViewItemsToPlot.ClearSort();
			FormMapView.SizeMapToForm();

			mapMenuItem.DropDown.Close();
			UpdateFromSettings();
		}

		private void ComboBoxSpace_SelectionChangeCommitted(object sender, EventArgs e)
		{
			Settings.Space = (Space)(comboBoxSpace.SelectedItem ?? throw new Exception("Space combobox SelectedItem was null"));
			Settings.ResolveConflictingSettings();
			UpdateFromSettings();
		}

		private void Search_SearchInAllSpaces_Click(object sender, EventArgs e)
		{
			SetSetting(() => Settings.SearchSettings.SearchInAllSpaces = !Settings.SearchSettings.SearchInAllSpaces, false, true);
		}

		private void Search_AdvancedMode_Click(object sender, EventArgs e)
		{
			SetSetting(() => Settings.SearchSettings.Advanced = !Settings.SearchSettings.Advanced, false, true);
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
			dataGridViewSearchResults.ClearSort();
			dataGridViewItemsToPlot.ClearSort();
			FormMapView.SizeMapToForm();
			UpdateFromSettings();
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

		private void ButtonSelectAllSignature_Click(object sender, EventArgs e)
		{
			Settings.SearchSettings.SelectedSignatures = Enum.GetValues<Signature>().ToList();
			UpdateFromSettings(false, false);
		}

		private void ButtonSelectRecommended_Click(object sender, EventArgs e)
		{
			Settings.SearchSettings.SelectedSignatures = Enum.GetValues<Signature>().Where(s => s.IsRecommendedSelection()).ToList();
			UpdateFromSettings(false, false);
		}

		private void ButtonUnselectAllSignature_Click(object sender, EventArgs e)
		{
			SetSetting(() => Settings.SearchSettings.SelectedSignatures.Clear(), false, false);
		}

		private void ButtonSelectAllLockLevel_Click(object sender, EventArgs e)
		{
			Settings.SearchSettings.SelectedLockLevels = Enum.GetValues<LockLevel>().ToList();
			UpdateFromSettings(false, false);
		}

		private void ButtonUnselectAllLockLevel_Click(object sender, EventArgs e)
		{
			SetSetting(() => Settings.SearchSettings.SelectedLockLevels.Clear(), false, false);
		}

		private void ButtonAddToMap_Click(bool addAsGroup = false)
		{
			ItemsToPlot.RaiseListChangedEvents = false;

			List<GroupedSearchResult> itemsToAdd = new List<GroupedSearchResult>();

			List<DataGridViewRow?> selectedRows = dataGridViewSearchResults.SelectedCells
				.Cast<DataGridViewCell>()
				.DistinctBy(c => c.RowIndex)
				.Select(c => c.OwningRow)
				.Reverse()
				.ToList();

			if (selectedRows.Count == 0)
			{
				return;
			}

			// From the selected cells, find the unique rows, and find the data bound to those
			foreach (DataGridViewRow? row in selectedRows)
			{
				GroupedSearchResult instance = (GroupedSearchResult)(row?.DataBoundItem ?? throw new Exception("Row was or was bound to null"));

				if (!ItemsToPlot.Contains(instance))
				{
					itemsToAdd.Add(instance);
				}
			}

			PlotIcon? groupPlotIcon = addAsGroup ? new PlotIcon() : null;

			// Add the valid items to the actual list
			// We do this in 2 loops to avoid checking the new items against themselves with the contains check
			foreach (GroupedSearchResult instance in itemsToAdd)
			{
				if (addAsGroup)
				{
					instance.PlotIcon = groupPlotIcon;
				}
				else
				{
					instance.PlotIcon = new PlotIcon();
				}

				ItemsToPlot.Add(instance);
			}

			ItemsToPlot.RaiseListChangedEvents = true;
			ItemsToPlot.ResetBindings();

			// Scroll to bottom of list
			dataGridViewItemsToPlot.FirstDisplayedScrollingRowIndex = dataGridViewItemsToPlot.Rows.Count - 1;
		}

		private void ButtonRemoveFromMap_Click(object sender, EventArgs e)
		{
			ItemsToPlot.RaiseListChangedEvents = false;

			List<DataGridViewRow?> selectedRows = dataGridViewItemsToPlot.SelectedCells.Cast<DataGridViewCell>().DistinctBy(c => c.RowIndex).Select(c => c.OwningRow).ToList();

			if (selectedRows.Count == 0)
			{
				return;
			}

			// Shortcut to remove all
			if (selectedRows.Count == ItemsToPlot.Count)
			{
				ItemsToPlot.Clear();
			}
			else
			{
				// Collect the instances attached to the rows
				// Then in a separate loop (to avoid removing from the list which we are looping), remove them
				List<GroupedSearchResult> itemsToRemove = selectedRows.Select(row => (GroupedSearchResult)(row?.DataBoundItem ?? throw new Exception("Row was or was bound to null"))).ToList();

				foreach (GroupedSearchResult instance in itemsToRemove)
				{
					ItemsToPlot.Remove(instance);
				}
			}

			ItemsToPlot.RaiseListChangedEvents = true;
			ItemsToPlot.ResetBindings();

			if (ItemsToPlot.Count == 0)
			{
				dataGridViewItemsToPlot.ClearSort();
			}
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.SaveToFile();
			FileIO.Cleanup();
		}
	}
}
