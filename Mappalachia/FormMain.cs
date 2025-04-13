using System.Data;
using Library;

namespace Mappalachia
{
	public partial class FormMain : Form
	{
		DataTable SearchResultsDataTable { get; } = new DataTable();

		public static Random Random { get; } = new Random();

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

		public FormMain()
		{
			InitializeComponent();
			textBoxSearch.Text = SearchTermHints[Random.Next(SearchTermHints.Count)];
			InitializeSearchResultsGrid();
		}

		private async void ButtonSearch_Click(object sender, EventArgs e)
		{
			dataGridViewSearchResults.DataSource = null;
			SearchResultsDataTable.Rows.Clear();

			List<GroupedInstance> searchResults = await Database.Search(textBoxSearch.Text);

			searchResults = searchResults
				.OrderByDescending(g => g.Space == Settings.CurrentSpace)
				.ThenByDescending(g => g.Count)
				.ThenByDescending(g => g.SpawnWeight)
				.ThenBy(g => g.Entity.EditorID)
				.ToList();

			foreach (GroupedInstance groupedInstance in searchResults)
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
		}

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

			DataGridViewColumn formIDColumn = dataGridViewSearchResults.Columns["Form ID"] ?? throw new Exception("Search Results Form ID Column not found");
			formIDColumn.DefaultCellStyle.Font = new Font("Consolas", 8);
		}
	}
}
