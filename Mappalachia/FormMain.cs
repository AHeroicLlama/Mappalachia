using System.Data;
using Library;

namespace Mappalachia
{
	public partial class FormMain : Form
	{
		public Settings Settings { get; } = new Settings();

		static List<string> SearchTermHints { get; } = new List<string>
		{
			"Alcohol",
			"Alien Blaster",
			"Caps Stash",
			"Flora",
			"Fusion Core",
			"Ginseng",
			"Hardpoint",
			"Instrument",
			"LPI_Chem",
			"LPI_Food",
			"LvlCritter",
			"NoCampAllowed",
			"Nuka Cola",
			"Overseer's Cache",
			"P01C_Bucket_Loot",
			"PowerArmorFurniture_",
			"Pre War Money",
			"Protest Sign",
			"Pumpkin",
			"RETrigger",
			"Rare",
			"Recipe",
			"SFM04_Organic_Pod",
			"Strange Encounter",
			"Tales from West Virginia",
			"Teddy Bear",
			"Thistle",
			"Treasure Map Mound",
			"Trunk Boss",
			"Vein",
			"Wind Chimes",
			"Workbench",
		};

		Random Random { get; } = new Random();

		FormMapView MapViewForm { get; set; }

		DataTable SearchResultsDataTable { get; } = new DataTable();

		List<GroupedInstance> SearchResults { get; set; } = new List<GroupedInstance>();

		public FormMain()
		{
			InitializeComponent();

			// Spawn the map view form
			MapViewForm = new FormMapView(this);
			MapViewForm.Show();

			textBoxSearch.Text = SearchTermHints[Random.Next(SearchTermHints.Count)];
			InitializeSearchResultsGrid();
			InitializeSpaceDropDown();
			UpdateFromSettings();

			mapMenuItem.DropDown.Closing += DontCloseClickedDropDown;
			mapMapMarkersMenuItem.DropDown.Closing += DontCloseClickedDropDown;
			mapBackgroundImageMenuItem.DropDown.Closing += DontCloseClickedDropDown;
			mapLegendStyleMenuItem.DropDown.Closing += DontCloseClickedDropDown;
		}

		void DontCloseClickedDropDown(object? sender, ToolStripDropDownClosingEventArgs e)
		{
			if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
			{
				e.Cancel = true;
			}
		}

		// Reads in fields from Settings and updates UI elements respectively
		void UpdateFromSettings(bool reDraw = true)
		{
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

			searchInAllSpacesToolStripMenuItem.Checked = Settings.SearchSettings.SearchInAllSpaces;
			textBoxSearch.Text = Settings.SearchSettings.SearchTerm;

			// TODO - Doing this every time is inelegant.
			if (reDraw)
			{
				MapViewForm.UpdateMap();
			}
		}

		// Re-populates the search results grid with the current search results
		void UpdateSearchResultsGrid()
		{
			dataGridViewSearchResults.DataSource = null;
			SearchResultsDataTable.Rows.Clear();

			foreach (GroupedInstance groupedInstance in SearchResults)
			{
				SearchResultsDataTable.Rows.Add(
					groupedInstance.Entity.FormID.ToHex(),
					groupedInstance.Entity.EditorID,
					groupedInstance.Entity.DisplayName,
					groupedInstance.Entity.Signature.ToString(),
					groupedInstance.Space.DisplayName,
					groupedInstance.Label,
					groupedInstance.LockLevel.ToString(),
					groupedInstance.SpawnWeight,
					groupedInstance.Count);
			}

			dataGridViewSearchResults.DataSource = SearchResultsDataTable;
			SetSearchResultsGridStyle();
		}

		// Setup the columns and styling for the search results grid
		void InitializeSearchResultsGrid()
		{
			SearchResultsDataTable.Columns.AddRange(
			[
				new DataColumn("Form ID"),
				new DataColumn("Editor ID"),
				new DataColumn("Display Name"),
				new DataColumn("Signature"),
				new DataColumn("Space Display Name"),
				new DataColumn("Label"),
				new DataColumn("Lock Level"),
				new DataColumn("Weight", typeof(float)),
				new DataColumn("Count", typeof(int)),
			]);

			dataGridViewSearchResults.DataSource = SearchResultsDataTable;
			SetSearchResultsGridStyle();
		}

		void InitializeSpaceDropDown()
		{
			comboBoxSpace.Items.Clear();

			foreach (Space space in Database.AllSpaces)
			{
				comboBoxSpace.Items.Add($"{space.DisplayName} ({space.EditorID})");
			}

			comboBoxSpace.SelectedIndex = 0;
		}

		void SetSearchResultsGridStyle()
		{
			DataGridViewColumn formIDColumn = dataGridViewSearchResults.Columns["Form ID"] ?? throw new NullReferenceException("Search Results Form ID Column not found");
			formIDColumn.DefaultCellStyle.Font = new Font("Consolas", 9);
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
			UpdateChecker.CheckForUpdates(true);
		}

		private void Help_ViewGitHub_Click(object sender, EventArgs e)
		{
			Common.OpenURI(URLs.GitHub);
		}

		private void Donate_Click(object sender, EventArgs e)
		{
			Common.OpenURI(URLs.DonatePaypal);
		}

		private async void ButtonSearch_Click(object sender, EventArgs e)
		{
			SearchResults = await Database.Search(Settings);

			SearchResults = SearchResults
				.OrderByDescending(g => g.Space == Settings.Space)
				.ThenByDescending(g => g.Count)
				.ThenByDescending(g => g.SpawnWeight)
				.ThenBy(g => g.Entity.EditorID)
				.ToList();

			UpdateSearchResultsGrid();
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

		private void Map_QuickSave_Click(object sender, EventArgs e)
		{
			// TODO
			mapMenuItem.DropDown.Close();
		}

		private void Map_ExportToFile_Click(object sender, EventArgs e)
		{
			// TODO
			mapMenuItem.DropDown.Close();
		}

		private void Map_ClearPlots_Click(object sender, EventArgs e)
		{
			// TODO
			mapMenuItem.DropDown.Close();
		}

		private void Map_Reset_Click(object sender, EventArgs e)
		{
			// TODO
			mapMenuItem.DropDown.Close();
		}

		private void ComboBoxSpace_SelectedIndexChanged(object sender, EventArgs e)
		{
			Settings.Space = Database.AllSpaces[comboBoxSpace.SelectedIndex];
			UpdateFromSettings();
		}

		private void Search_SearchInAllSpaces_Click(object sender, EventArgs e)
		{
			Settings.SearchSettings.SearchInAllSpaces = !Settings.SearchSettings.SearchInAllSpaces;
			UpdateFromSettings(false);
		}

		private void SearchTerm_TextChanged(object sender, EventArgs e)
		{
			Settings.SearchSettings.SearchTerm = textBoxSearch.Text;
			UpdateFromSettings(false);
		}
	}
}
