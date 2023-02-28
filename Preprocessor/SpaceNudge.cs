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

				int nudgeX;
				int nudgeY;
				float nudgeScale;

				switch (spaceEditorId)
				{
					case "BlueRidgeOffice01":
						nudgeX = 200;
						nudgeY = 0;
						nudgeScale = 2.2f;
						break;

					case "CharlestonCapitolCourthouse01":
						nudgeX = -1300;
						nudgeY = -850;
						nudgeScale = 1.3f;
						break;

					case "CraterWatchstation01":
						nudgeX = -150;
						nudgeY = -2000;
						nudgeScale = 1.8f;
						break;

					case "DuncanDuncanRobotics01":
						nudgeX = -1100;
						nudgeY = 1200;
						nudgeScale = 2.5f;
						break;

					case "FortAtlasDungeon01":
						nudgeX = 700;
						nudgeY = 0;
						nudgeScale = 1.2f;
						break;

					case "FoundationInt01":
						nudgeX = 1400;
						nudgeY = 1800;
						nudgeScale = 1.7f;
						break;

					case "FoundationSupplyRoom01":
						nudgeX = 0;
						nudgeY = 500;
						nudgeScale = 1f;
						break;

					case "GarrahanMiningHQ01":
						nudgeX = 0;
						nudgeY = 650;
						nudgeScale = 1.3f;
						break;

					case "GarrahanMiningHQDungeon":
						nudgeX = 0;
						nudgeY = -250;
						nudgeScale = 2f;
						break;

					case "MonongahMissileSilo01":
						nudgeX = 300;
						nudgeY = -2450;
						nudgeScale = 2.2f;
						break;

					case "PoseidonPlant01":
						nudgeX = 400;
						nudgeY = 700;
						nudgeScale = 1.3f;
						break;

					case "PoseidonPlant02":
						nudgeX = -400;
						nudgeY = -2700;
						nudgeScale = 2f;
						break;

					case "RollinsLaborCamp01":
						nudgeX = 200;
						nudgeY = -1100;
						nudgeScale = 1.7f;
						break;

					case "SamBlackwellsDeathclawCave":
						nudgeX = -2200;
						nudgeY = 200;
						nudgeScale = 1.9f;
						break;

					case "SheltersVaultLivingQuarters01":
						nudgeX = -900;
						nudgeY = -700;
						nudgeScale = 1.3f;
						break;

					case "SheltersVaultServerRoom01":
						nudgeX = 1700;
						nudgeY = 1000;
						nudgeScale = 1.7f;
						break;

					case "SugarGrove02":
						nudgeX = -1500;
						nudgeY = -2900;
						nudgeScale = 2.2f;
						break;

					case "TheCraterCore01":
						nudgeX = 3650;
						nudgeY = 0;
						nudgeScale = 2.6f;
						break;

					case "TheWayward01":
						nudgeX = 1100;
						nudgeY = 600;
						nudgeScale = 2.2f;
						break;

					case "VTecAgCenter01":
						nudgeX = 500;
						nudgeY = -850;
						nudgeScale = 1.3f;
						break;

					case "WatogaMunicipalCenter02":
						nudgeX = 100;
						nudgeY = 1000;
						nudgeScale = 1f;
						break;

					case "WatogaTowers01":
						nudgeX = -1200;
						nudgeY = 0;
						nudgeScale = 1.1f;
						break;

					case "WatogaUnderground01":
						nudgeX = 350;
						nudgeY = 0;
						nudgeScale = 1.6f;
						break;

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
					$"{nudgeX},{nudgeY},{nudgeScale}", newFileHeader));
			}

			return new CSVFile("Space_Info.csv", newFileHeader, newFileRows);
		}
	}
}
