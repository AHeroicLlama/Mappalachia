using System;
using System.Collections.Generic;

namespace Mappalachia
{
	class CSVRow
	{
		public List<CSVCell> cells = new List<CSVCell>();

		public CSVRow(string row, List<string> headers)
		{
			List<string> cellValues = new List<string>(row.Split(','));

			// Check we're not truncating or missing some data here.
			if (cellValues.Count != headers.Count)
			{
				throw new Exception("Row \"" + row + "\" under header \"" + string.Join(", ", headers) + "\": Number of cells did not match number of headers");
			}

			// Get a cell for each defined column and add it to the collection of cells.
			int i = 0;
			foreach (string cellValue in cellValues)
			{
				cells.Add(new CSVCell(cellValue, headers[i]));
				i++;
			}
		}

		public override string ToString()
		{
			return string.Join(",", cells);
		}

		// Return the cell value from under the given column header
		public string GetCellFromColumn(string name)
		{
			foreach (CSVCell cell in cells)
			{
				if (cell.columnName == name)
				{
					return cell.data;
				}
			}

			throw new Exception("Cell could not be found under column " + name);
		}

		// Escape problematic SQL characters
		public void Sanitize()
		{
			foreach (CSVCell cell in cells)
			{
				cell.Sanitize();
			}
		}

		// Correct potentially mistranslated raw data
		public void CorrectAnomalies()
		{
			foreach (CSVCell cell in cells)
			{
				cell.CorrectAnomalies();
			}
		}

		// Reduce long references and names to just the data required in them - typically just the FormID
		public void ReduceReferences()
		{
			foreach (CSVCell cell in cells)
			{
				cell.ReduceReferences();
			}
		}

		// Reduce large decimals to integers (The precision is unnecessary due to downscaling)
		public void ReduceDecimals()
		{
			foreach (CSVCell cell in cells)
			{
				cell.ReduceDecimals();
			}
		}

		// Run basic data integrity checks on expected values in columns
		public void Validate()
		{
			foreach (CSVCell cell in cells)
			{
				cell.Validate();
			}
		}
	}
}
