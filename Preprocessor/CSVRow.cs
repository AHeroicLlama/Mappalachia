using System;
using System.Collections.Generic;
using System.Linq;

namespace Mappalachia
{
	class CSVRow
	{
		public List<CSVCell> cells;

		public CSVRow(string row, string header)
		{
			List<string> columnHeaders = new List<string>(header.Split(','));
			cells = new List<CSVCell>();
			int i = 0;

			//Verify there is no data in the columns beyond those specified by the headers
			//Essentially, check we're not truncating some data here.
			List<string> truncatedCells = new List<string>(row.Split(',')).Skip(columnHeaders.Count).ToList();
			foreach (string truncatedCell in truncatedCells)
			{
				if (truncatedCell != string.Empty)
				{
					throw new Exception("Cell data \"" + truncatedCell + "\" under header \"" + header + "\" was truncated.");
				}
			}

			//Get a cell for each defined column and add it to the collection of cells.
			foreach (string cellValue in new List<string>(row.Split(',')).GetRange(0, columnHeaders.Count))
			{
				cells.Add(new CSVCell(cellValue, columnHeaders[i]));
				i++;
			}
		}

		public override string ToString()
		{
			return string.Join(",", cells);
		}

		//Return the cell value from under the given column header
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

		//Escape problematic SQL characters
		public void Sanitize()
		{
			foreach (CSVCell cell in cells)
			{
				cell.Sanitize();
			}
		}

		//Reduce long references and names to just the data required in them - typically just the FormID
		public void ReduceReferences()
		{
			foreach (CSVCell cell in cells)
			{
				cell.ReduceReferences();
			}
		}

		//Reduce large decimals to integers (The precision is unnecessary due to downscaling)
		public void ReduceDecimals()
		{
			foreach (CSVCell cell in cells)
			{
				cell.ReduceDecimals();
			}
		}

		//Run basic data integrity checks on expected values in columns
		public void Validate()
		{
			foreach (CSVCell cell in cells)
			{
				cell.Validate();
			}
		}
	}
}
