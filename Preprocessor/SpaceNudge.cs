using System.Collections.Generic;

namespace CommonwealthCartography
{
	internal class SpaceNudge
	{
		public static CSVFile AddNudgeToSpaces(CSVFile spaceInfoFile)
		{
			List<string> newFileHeader = new List<string> { "spaceFormID", "spaceEditorID", "spaceDisplayName", "isWorldspace", "nudgeX", "nudgeY", "nudgeScale", "esmNumber" };
			List<CSVRow> newFileRows = new List<CSVRow>();

			foreach (CSVRow row in spaceInfoFile.rows)
			{
				string spaceEditorId = row.GetCellFromColumn("spaceEditorID");

				int nudgeX;
				int nudgeY;
				float nudgeScale;

				switch (spaceEditorId)
				{
					default:
						nudgeX = 0;
						nudgeY = 0;
						nudgeScale = 1f;
						break;
				}

				newFileRows.Add(new CSVRow(
					$"{row.GetCellFromColumn("spaceFormID")}," +
					$"{row.GetCellFromColumn("spaceEditorID")}," +
					$"{row.GetCellFromColumn("spaceDisplayName")}," +
					$"{row.GetCellFromColumn("isWorldspace")}," +
					$"{nudgeX},{nudgeY},{nudgeScale},{row.GetCellFromColumn("esmNumber")}", newFileHeader));
			}

			return new CSVFile("Space_Info.csv", newFileHeader, newFileRows);
		}
	}
}
