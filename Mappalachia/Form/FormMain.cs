using System.Data;
using System.Diagnostics;
using Library;

namespace Mappalachia
{
	public partial class FormMain : Form
	{
		public FormMain()
		{
			InitializeComponent();
			pictureBoxMapDisplay.Image = Map.Draw();

			Stopwatch stopwatch = Stopwatch.StartNew();

			List<GroupedInstance> searchResults = Database.Search("");

			Console.WriteLine($"Query completed {stopwatch.Elapsed}");
			stopwatch.Restart();

			// TODO reuse this
			DataTable tableModel = new DataTable();
			tableModel.Columns.AddRange(
			[
				new DataColumn("Form ID"),
				new DataColumn("Editor ID"),
				new DataColumn("Display Name"),
				new DataColumn("Signature"),
				new DataColumn("Space Editor ID"),
				new DataColumn("Space Display Name"),
				new DataColumn("Label"),
				new DataColumn("Lock Level"),
				new DataColumn("Weight", typeof(float)),
				new DataColumn("Count", typeof(int)),
			]);

			Console.WriteLine($"Table model built {stopwatch.Elapsed}");
			stopwatch.Restart();

			foreach (GroupedInstance groupedInstance in searchResults)
			{
				tableModel.Rows.Add(
					groupedInstance.Entity.FormID.ToHex(),
					groupedInstance.Entity.EditorID,
					groupedInstance.Entity.DisplayName,
					groupedInstance.Entity.Signature.ToString(),
					groupedInstance.Space.EditorID,
					groupedInstance.Space.DisplayName,
					groupedInstance.Label,
					groupedInstance.LockLevel.ToString(),
					groupedInstance.SpawnWeight,
					groupedInstance.Count);
			}

			Console.WriteLine($"Rows added to table model {stopwatch.Elapsed}");
			stopwatch.Restart();

			dataGridViewSearchResults.DataSource = tableModel;

			Console.WriteLine($"Data source bound {stopwatch.Elapsed}");
			stopwatch.Restart();
		}
	}
}
