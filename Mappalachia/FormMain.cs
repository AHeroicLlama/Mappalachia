using System.Data;
using Library;

namespace Mappalachia
{
	public partial class FormMain : Form
	{
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
			MapViewForm = new FormMapView();
			MapViewForm.Show();

			textBoxSearch.Text = SearchTermHints[Random.Next(SearchTermHints.Count)];
			InitializeSearchResultsGrid();
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

		void SetSearchResultsGridStyle()
		{
			DataGridViewColumn formIDColumn = dataGridViewSearchResults.Columns["Form ID"] ?? throw new NullReferenceException("Search Results Form ID Column not found");
			formIDColumn.DefaultCellStyle.Font = new Font("Consolas", 9);
		}

		private void Map_ShowPreview_Click(object sender, EventArgs e)
		{
			// Don't expect this to be normally possible
			if (MapViewForm.IsDisposed)
			{
				MapViewForm = new FormMapView();
			}

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
			SearchResults = await Database.Search(textBoxSearch.Text);

			SearchResults = SearchResults
				.OrderByDescending(g => g.Space == Settings.CurrentSpace)
				.ThenByDescending(g => g.Count)
				.ThenByDescending(g => g.SpawnWeight)
				.ThenBy(g => g.Entity.EditorID)
				.ToList();

			UpdateSearchResultsGrid();
		}
	}
}
