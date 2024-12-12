using System.Data;
using Library;

namespace Mappalachia
{
	public partial class FormMain : Form
	{
		DataTable SearchResultsDataTable { get; } = new DataTable();

		public FormMain()
		{
			InitializeComponent();
			pictureBoxMapDisplay.Image = Map.Draw();
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

			dataGridViewSearchResults.Columns["Form ID"].DefaultCellStyle.Font = new Font("Consolas", 8);
		}
	}
}