using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mappalachia
{
	class CSVFile
	{
		public string fileName;
		public List<string> header;
		public List<CSVRow> rows = new List<CSVRow>();

		// Instantiate a CSVFile from a file on disk
		public CSVFile(string fileName)
		{
			this.fileName = fileName.Split('/').Last();

			header = new List<string>(File.ReadLines(fileName).First().Split(','));

			foreach (string row in File.ReadLines(fileName).Skip(1))
			{
				rows.Add(new CSVRow(row, header));
			}
		}

		// Instantiate a CSVFile from new cells, headers, and a file name
		public CSVFile(string fileName, List<string> header, List<CSVRow> rows)
		{
			this.fileName = fileName;
			this.header = header;
			this.rows = rows;
		}

		// Escape problematic SQL characters
		public void Sanitize()
		{
			Parallel.ForEach(rows, row => row.Sanitize());
		}

		// Correct potentially mistranslated raw data
		public void CorrectAnomalies()
		{
			Parallel.ForEach(rows, row => row.CorrectAnomalies());
		}

		// Reduce long references and names to just the data required in them - typically just the FormID
		public void ReduceReferences()
		{
			Parallel.ForEach(rows, row => row.ReduceReferences());
		}

		// Reduce large decimals to integers (The precision is unnecessary due to downscaling)
		public void ReduceDecimals()
		{
			Parallel.ForEach(rows, row => row.ReduceDecimals());
		}

		// Run basic data integrity checks on expected values in columns
		public void Validate()
		{
			Parallel.ForEach(rows, row => row.Validate());
		}

		public void WriteToFile(string filePath)
		{
			List<string> rowsOut = new List<string>();

			// Intentionally writing without header, as these are re-defined later through SQLite
			foreach (CSVRow row in rows)
			{
				rowsOut.Add(row.ToString());
			}

			File.WriteAllLines(filePath + fileName, rowsOut);
		}
	}
}
