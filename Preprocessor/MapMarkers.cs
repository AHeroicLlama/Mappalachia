using System.Collections.Generic;

namespace Mappalachia
{
    internal class MapMarkers
    {
		// Pull the MapMarker display text from position data and store it in a new file
		public static CSVFile ProcessMapMarkers(CSVFile positionData)
		{
			string newFileHeader = "spaceFormID,mapMarker,x,y,z";
			List<CSVRow> newFileRows = new List<CSVRow>();

			foreach (CSVRow row in positionData.rows)
			{
				string displayName = row.GetCellFromColumn("referenceFormID");

				if (Validation.matchFormID.IsMatch(displayName))
				{
					continue; // This is a normal entity which came without an actual display name so we assume it's not a map marker
				}

				// ... Otherwise we assume this is a map marker
				string newRow =
					row.GetCellFromColumn("spaceFormID") + "," +
					displayName + "," +
					row.GetCellFromColumn("x") + "," +
					row.GetCellFromColumn("y") + "," +
					row.GetCellFromColumn("z");
				newFileRows.Add(new CSVRow(newRow, newFileHeader));
			}

			return new CSVFile("Map_Markers.csv", newFileHeader, newFileRows);
		}
	}
}
