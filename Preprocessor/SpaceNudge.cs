using System.Collections.Generic;

namespace Mappalachia
{
	internal class SpaceNudge
	{
		public static CSVFile AddNudgeToSpaces(CSVFile spaceInfoFile)
		{
			List<string> newFileHeader = new List<string> { "spaceFormID", "spaceEditorID", "spaceDisplayName", "isWorldspace", "nudgeX", "nudgeY", "nudgeScale" };
			List<CSVRow> newFileRows = new List<CSVRow>();

			foreach (CSVRow row in spaceInfoFile.rows)
			{
				string spaceEditorId = row.GetCellFromColumn("spaceEditorID");

				int nudgeX = 0;
				int nudgeY = 0;
				float nudgeScale = 1f;

				switch (spaceEditorId)
				{
					case "BlueRidgeOffice01":
						nudgeX = 200;
						nudgeY = 0;
						nudgeScale = 2.2f;
						break;

					default:
						nudgeX = 0;
						nudgeY = 0;
						nudgeScale = 1f;
						break;
				}

				newFileRows.Add(new CSVRow($"{row.GetCellFromColumn("spaceFormID")},{row.GetCellFromColumn("spaceEditorID")}," +
					$"{row.GetCellFromColumn("spaceDisplayName")},{row.GetCellFromColumn("isWorldspace")},{nudgeX},{nudgeY},{nudgeScale}", newFileHeader));
			}

			return new CSVFile("Space_Info.csv", newFileHeader, newFileRows);
		}
	}
}
