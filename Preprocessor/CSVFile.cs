using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mappalachia
{
	class CSVFile
	{
		public string fileName;
		public string header;
		public List<CSVRow> rows = new List<CSVRow>();

		//Instantiate a CSVFile from a file on disk
		public CSVFile(string fileName)
		{
			this.fileName = fileName.Split('/').Last();
			List<string> rawRows = new List<string>(File.ReadAllLines(fileName));
			header = rawRows[0];

			foreach (string row in rawRows.Skip(1))
			{
				rows.Add(new CSVRow(row, header));
			}
		}

		//Instantiate a CSVFile from new cells, headers, and a file name
		public CSVFile(string fileName, string header, List<CSVRow> rows)
		{
			this.fileName = fileName;
			this.header = header;
			this.rows = rows;
		}

		//Find the ESM which this CSVFile was sourced from, based on its name
		public string GetESM()
		{
			if (fileName.StartsWith("SeventySix"))
			{
				return "SeventySix";
			}
			else if (fileName.StartsWith("NW"))
			{
				return "NW";
			}
			else
			{
				throw new Exception("File " + fileName + " has an unknown ESM type");
			}
		}

		//Escape problematic SQL characters
		public void Sanitize()
		{
			foreach (CSVRow row in rows)
			{
				row.Sanitize();
			}
		}

		//Reduce long references and names to just the data required in them - typically just the FormID
		public void ReduceReferences()
		{
			foreach (CSVRow row in rows)
			{
				row.ReduceReferences();
			}
		}

		//Reduce large decimals to integers (The precision is unnecessary due to downscaling)
		public void ReduceDecimals()
		{
			foreach (CSVRow row in rows)
			{
				row.ReduceDecimals();
			}
		}

		//Run basic data integrity checks on expected values in columns
		public void Validate()
		{
			foreach (CSVRow row in rows)
			{
				row.Validate();
			}
		}

		public void WriteToFile(string filePath)
		{
			List<string> rowsOut = new List<string>();

			//Intentionally writing without header, as these are re-defined later through SQLite
			foreach (CSVRow row in rows)
			{
				rowsOut.Add(row.ToString());
			}

			File.WriteAllLines(filePath + fileName, rowsOut);
		}
	}
}