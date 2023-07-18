using System.Collections.Generic;

namespace Mappalachia
{
	internal class GenericCSVHelper
	{
		// Returns a copy of the given CSV with a new column added, copied from another
		public static CSVFile DuplicateColumn(CSVFile inputFile, string sourceColumn, string newColumn)
		{
			List<string> newFileHeader = inputFile.header;
			newFileHeader.Add(newColumn);
			inputFile.header = newFileHeader;
			List<CSVRow> newFileRows = new List<CSVRow>();

			foreach (CSVRow row in inputFile.rows)
			{
				string clone = row.GetCellFromColumn(sourceColumn);
				string newRow = row.ToString() + "," + clone;
				newFileRows.Add(new CSVRow(newRow, newFileHeader));
			}

			return new CSVFile(inputFile.fileName, newFileHeader, newFileRows);
		}
	}
}
