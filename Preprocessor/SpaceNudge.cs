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
					case "Commonwealth":
						nudgeX = 945;
						nudgeY = 160;
						nudgeScale = 1.98f;
						break;

					case "DLC03FarHarbor":
						nudgeX = -470;
						nudgeY = -475;
						nudgeScale = 1.247f;
						break;

					case "NukaWorld":
						nudgeX = -100;
						nudgeY = 330;
						nudgeScale = 1.66f;
						break;

					case "SanctuaryHillsWorld":
						nudgeX = 4300;
						nudgeY = 5500;
						nudgeScale = 3.5f;
						break;

					case "AndrewStation01":
						nudgeX = 2300;
						nudgeY = 700;
						nudgeScale = 2f;
						break;

					case "ArcjetSystems02":
						nudgeX = 700;
						nudgeY = -1200;
						nudgeScale = 2f;
						break;

					case "BackBayTallOffice01":
						nudgeX = -300;
						nudgeY = 0;
						nudgeScale = 1f;
						break;

					case "BackStreetApparel01":
						nudgeX = 400;
						nudgeY = -300;
						nudgeScale = 1.5f;
						break;

					case "BostonAirportRuins01":
						nudgeX = 2200;
						nudgeY = 0;
						nudgeScale = 2.2f;
						break;

					case "BostonCommon02":
						nudgeX = 0;
						nudgeY = 1300;
						nudgeScale = 1.5f;
						break;

					case "BostonPublicLibrary01":
						nudgeX = 0;
						nudgeY = 0;
						nudgeScale = 1.5f;
						break;

					case "CambridgeEastCITRaiderCamp01":
						nudgeX = 0;
						nudgeY = 1700;
						nudgeScale = 1.4f;
						break;

					case "CambridgeKendallHospital01":
						nudgeX = 1800;
						nudgeY = -500;
						nudgeScale = 1.4f;
						break;

					case "CambridgeMassChemical01":
						nudgeX = -700;
						nudgeY = 0;
						nudgeScale = 1.3f;
						break;

					case "CambridgePlumbing01":
						nudgeX = 0;
						nudgeY = 0;
						nudgeScale = 1.7f;
						break;

					case "CambridgePolymerLabs01":
						nudgeX = 0;
						nudgeY = 600;
						nudgeScale = 1.5f;
						break;

					case "CollegeSquare01":
						nudgeX = 0;
						nudgeY = 500;
						nudgeScale = 1.3f;
						break;

					case "CorvegaAssemblyPlant01":
						nudgeX = 200;
						nudgeY = -500;
						nudgeScale = 1.4f;
						break;

					case "DBTechHighSchool01":
						nudgeX = -2900;
						nudgeY = 0;
						nudgeScale = 2.5f;
						break;

					case "DBTechHighSchool02":
						nudgeX = -1500;
						nudgeY = 500;
						nudgeScale = 1.2f;
						break;

					case "DLC01Lair01":
						nudgeX = 1000;
						nudgeY = 16000;
						nudgeScale = 9f;
						break;

					case "DLC03AlderseaDaySpa01":
						nudgeX = 0;
						nudgeY = 0;
						nudgeScale = 1.5f;
						break;

					case "DLC03BeaverCreekLanes01":
						nudgeX = -200;
						nudgeY = 600;
						nudgeScale = 1.5f;
						break;

					case "DLC03EagleCoveTannery01":
						nudgeX = 0;
						nudgeY = -500;
						nudgeScale = 1.5f;
						break;

					case "DLC03FarHarborLastPlank":
						nudgeX = 1300;
						nudgeY = 400;
						nudgeScale = 2f;
						break;

					case "DLC03KyeBunker01":
						nudgeX = -500;
						nudgeY = 0;
						nudgeScale = 1.5f;
						break;

					case "DLC03NorthwoodRidgeQuarry01":
						nudgeX = 0;
						nudgeY = -1000;
						nudgeScale = 1.5f;
						break;

					case "DLC03NucleusSubInterior01":
						nudgeX = 300;
						nudgeY = -2100;
						nudgeScale = 2f;
						break;

					case "DLC03POI40Int":
						nudgeX = 0;
						nudgeY = -1500;
						nudgeScale = 2.5f;
						break;

					case "DLC03WindFarmBuilding01":
						nudgeX = 0;
						nudgeY = -200;
						nudgeScale = 1.3f;
						break;

					case "DLC04ColaCars01":
						nudgeX = 2800;
						nudgeY = 1000;
						nudgeScale = 2.2f;
						break;

					case "DLC04GrandchesterMansion01":
						nudgeX = -1000;
						nudgeY = 0;
						nudgeScale = 1.5f;
						break;

					case "DLC04GZBattlezone01":
						nudgeX = 0;
						nudgeY = -800;
						nudgeScale = 1.5f;
						break;

					case "DLC04GZNukaGalaxy01":
						nudgeX = -700;
						nudgeY = 2000;
						nudgeScale = 2f;
						break;

					case "DLC04GZTheater01":
						nudgeX = -4000;
						nudgeY = -500;
						nudgeScale = 3f;
						break;

					case "DLC04HubCappysCafe01":
						nudgeX = -1000;
						nudgeY = 0;
						nudgeScale = 1f;
						break;

					case "DLC04KiddieKingdomTheater01":
						nudgeX = 0;
						nudgeY = -500;
						nudgeScale = 1.3f;
						break;

					case "DLC04KiddieKingdomZ01":
						nudgeX = 500;
						nudgeY = 0;
						nudgeScale = 1.3f;
						break;

					case "DLC04Nukacade01":
						nudgeX = 500;
						nudgeY = 0;
						nudgeScale = 1.5f;
						break;

					case "DLC04SafariPrimateHouse":
						nudgeX = -500;
						nudgeY = 0;
						nudgeScale = 1f;
						break;

					case "DLC04SafariReptileHouse":
						nudgeX = -500;
						nudgeY = 0;
						nudgeScale = 1f;
						break;

					case "DmndAllFaithsChapel01":
						nudgeX = 1000;
						nudgeY = 0;
						nudgeScale = 1f;
						break;

					case "DmndArturosHouse01":
						nudgeX = 2500;
						nudgeY = -800;
						nudgeScale = 2f;
						break;

					case "DmndFallons01":
						nudgeX = 1300;
						nudgeY = 0;
						nudgeScale = 1.5f;
						break;

					case "DmndGreenhouse01":
						nudgeX = 2500;
						nudgeY = 0;
						nudgeScale = 2f;
						break;

					case "DmndPlayerHouse01":
						nudgeX = 1000;
						nudgeY = 0;
						nudgeScale = 1f;
						break;

					case "DmndRadio01":
						nudgeX = 1000;
						nudgeY = 0;
						nudgeScale = 1f;
						break;

					case "DmndSecurity01":
						nudgeX = 0;
						nudgeY = -1000;
						nudgeScale = 1f;
						break;

					case "DmndSurgeryBasement01":
						nudgeX = 1800;
						nudgeY = -500;
						nudgeScale = 1.5f;
						break;

					case "DunwichBorers01":
						nudgeX = 100;
						nudgeY = 0;
						nudgeScale = 1.2f;
						break;

					case "EsplanadeMansion01":
						nudgeX = 1500;
						nudgeY = 800;
						nudgeScale = 1f;
						break;

					case "FaneuilHall01":
						nudgeX = -2500;
						nudgeY = 0;
						nudgeScale = 2f;
						break;

					case "FensBank01":
						nudgeX = 0;
						nudgeY = 500;
						nudgeScale = 1.8f;
						break;

					case "FensCafeBuilding":
						nudgeX = 0;
						nudgeY = 0;
						nudgeScale = 1.8f;
						break;

					case "FensKenmoreStation":
						nudgeX = 0;
						nudgeY = 0;
						nudgeScale = 1.8f;
						break;

					case "FensParkviewApartments01":
						nudgeX = 0;
						nudgeY = 0;
						nudgeScale = 1.6f;
						break;

					case "FensParkviewApartments02":
						nudgeX = 500;
						nudgeY = 0;
						nudgeScale = 1.5f;
						break;

					case "Financial06":
						nudgeX = 0;
						nudgeY = -500;
						nudgeScale = 1.5f;
						break;

					case "FortStrong01":
						nudgeX = 2000;
						nudgeY = -1300;
						nudgeScale = 2f;
						break;

					case "FortStrong02":
						nudgeX = -500;
						nudgeY = 0;
						nudgeScale = 1.2f;
						break;

					case "GlowingSeaPOIDB05Int":
						nudgeX = 500;
						nudgeY = 500;
						nudgeScale = 0.9f;
						break;

					case "GlowingSeaPOIDB06Int":
						nudgeX = -4500;
						nudgeY = 4500;
						nudgeScale = 3f;
						break;

					case "GNN01":
						nudgeX = -100;
						nudgeY = 0;
						nudgeScale = 1.1f;
						break;

					case "GoodneighborHotelRexford":
						nudgeX = 800;
						nudgeY = 800;
						nudgeScale = 1.3f;
						break;

					case "GoodneighborTheMemoryDen":
						nudgeX = -1000;
						nudgeY = 0;
						nudgeScale = 1.3f;
						break;

					case "GoodneighborTheThirdRail":
						nudgeX = 0;
						nudgeY = 1000;
						nudgeScale = 1.8f;
						break;

					case "GraygardenHomestead01":
						nudgeX = -1300;
						nudgeY = -1700;
						nudgeScale = 1.7f;
						break;

					case "GreenetechGenetics01":
						nudgeX = 500;
						nudgeY = 0;
						nudgeScale = 1.2f;
						break;

					case "GreenetechGenetics02":
						nudgeX = 0;
						nudgeY = -600;
						nudgeScale = 1.3f;
						break;

					case "HardwareTown01":
						nudgeX = 400;
						nudgeY = -500;
						nudgeScale = 1.6f;
						break;

					case "Libertalia01":
						nudgeX = -1000;
						nudgeY = 300;
						nudgeScale = 1.6f;
						break;

					case "MassFusion02Trans":
						nudgeX = 800;
						nudgeY = -800;
						nudgeScale = 1f;
						break;

					case "MQ203KelloggsBrain":
						nudgeX = 2000;
						nudgeY = -700;
						nudgeScale = 2f;
						break;

					case "MuseumOfWitchcraft01":
						nudgeX = -1200;
						nudgeY = 700;
						nudgeScale = 1.5f;
						break;

					case "NahantOceanSociety01":
						nudgeX = 0;
						nudgeY = 0;
						nudgeScale = 1.2f;
						break;

					case "NHMFreightDepot01":
						nudgeX = 0;
						nudgeY = 0;
						nudgeScale = 1.4f;
						break;

					case "PoseidonReservoir01":
						nudgeX = -2400;
						nudgeY = 0;
						nudgeScale = 2f;
						break;

					case "PrewarTVStudio":
						nudgeX = 500;
						nudgeY = 0;
						nudgeScale = 1f;
						break;

					case "PrydwenHull01":
						nudgeX = 1000;
						nudgeY = 0;
						nudgeScale = 1f;
						break;

					case "QuincyPD01":
						nudgeX = -4500;
						nudgeY = 2500;
						nudgeScale = 3f;
						break;

					case "RelayTowerInt01":
						nudgeX = -5000;
						nudgeY = 800;
						nudgeScale = 3f;
						break;

					case "RelayTowerInt05":
						nudgeX = -800;
						nudgeY = 0;
						nudgeScale = 1f;
						break;

					case "RelayTowerInt14":
						nudgeX = -2000;
						nudgeY = 0;
						nudgeScale = 1.7f;
						break;

					case "SaugusIronworks01":
						nudgeX = -1200;
						nudgeY = 0;
						nudgeScale = 1.5f;
						break;

					case "SaugusIronworks02":
						nudgeX = -1200;
						nudgeY = 0;
						nudgeScale = 1.5f;
						break;

					case "SouthBostonPoliceDepartment01":
						nudgeX = 2000;
						nudgeY = -500;
						nudgeScale = 1.6f;
						break;

					case "SuperDuperMart01":
						nudgeX = 0;
						nudgeY = 0;
						nudgeScale = 1.2f;
						break;

					case "TiconderogaStation01":
						nudgeX = 2500;
						nudgeY = 2500;
						nudgeScale = 2.5f;
						break;

					case "TrinityTower01":
						nudgeX = 0;
						nudgeY = 0;
						nudgeScale = 1.3f;
						break;

					case "UniversityPoint02":
						nudgeX = -1500;
						nudgeY = 700;
						nudgeScale = 1.4f;
						break;

					case "Vault81Entry":
						nudgeX = -1000;
						nudgeY = -1000;
						nudgeScale = 1.5f;
						break;

					case "Waterfront02":
						nudgeX = 0;
						nudgeY = 2000;
						nudgeScale = 1.5f;
						break;

					case "zLexingtonApartments":
						nudgeX = 0;
						nudgeY = 0;
						nudgeScale = 1.3f;
						break;

					case "zLexingtonLaundromat":
						nudgeX = 0;
						nudgeY = 0;
						nudgeScale = 1.6f;
						break;

					case "zLexingtonPharmacy":
						nudgeX = 0;
						nudgeY = 0;
						nudgeScale = 1.6f;
						break;

					case "zWaystation":
						nudgeX = 0;
						nudgeY = 0;
						nudgeScale = 0.8f;
						break;

					case "ElevMinUseTransUtil":
					case "ElevTransHiTech":
					case "ElevTransHiTechInstitute":
					case "ElevTransPub":
					case "ElevTransUtil":
					case "ElevTransVault":
						nudgeX = 0;
						nudgeY = -800;
						nudgeScale = 1f;
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
					$"{nudgeX},{nudgeY},{nudgeScale},{row.GetCellFromColumn("esmNumber")}", newFileHeader));
			}

			return new CSVFile("Space_Info.csv", newFileHeader, newFileRows);
		}
	}
}
