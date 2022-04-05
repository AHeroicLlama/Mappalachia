using System.Collections.Generic;

namespace Mappalachia
{
	static class JunkScrap
	{
		// Use the Junk Scrap and Component Quantity CSVFiles to generate a new file for Quantified Junk Scrap
		public static CSVFile ProcessJunkScrap(CSVFile junkFile, CSVFile componentQuantity)
		{
			List<CSVRow> newFileRows = new List<CSVRow>();

			foreach (CSVRow row in junkFile.rows)
			{
				string component = row.GetCellFromColumn("component");
				string formID = row.GetCellFromColumn("junkFormID");
				string lookupQuantity = row.GetCellFromColumn("componentQuantity");
				int quantity = -1;

				// Find what the quantity is by looking up against the componentQuantity
				foreach (CSVRow quantityRow in componentQuantity.rows)
				{
					// Find the row where this component quantities are stored
					if (quantityRow.GetCellFromColumn("component") == component)
					{
						quantity = int.Parse(quantityRow.GetCellFromColumn(lookupQuantity));
					}
				}

				string newRow = component + "," + quantity + "," + formID;
				newFileRows.Add(new CSVRow(newRow, junkFile.header));
			}

			return new CSVFile("Quantified_Scrap.csv", new List<string> { "component", "componentQuantity", "junkFormID" }, newFileRows);
		}
	}
}
