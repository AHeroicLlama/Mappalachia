using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace BackgroundRenderer
{
	public partial class BackgroundRenderer
	{
		const string magickPath = "C:\\Program Files\\ImageMagick-7.1.1-Q16-HDRI\\magick.exe";
		const string fo4DataPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Fallout 4\\Data";
		static readonly string thisAppPath = Directory.GetCurrentDirectory();
		static readonly string commonwealthCartographyRoot = thisAppPath + "..\\..\\..\\..\\..\\";
		static readonly string databasePath = Path.GetFullPath(commonwealthCartographyRoot + "CommonwealthCartography\\data\\commonwealth_cartography.db");
		static readonly string imageDirectory = Path.GetFullPath(commonwealthCartographyRoot + "CommonwealthCartography\\img\\");
		static readonly string cellDirectory = Path.GetFullPath(imageDirectory + "cell\\");
		static readonly string utilsRenderPath = Path.GetFullPath(commonwealthCartographyRoot + "FO76Utils\\render.exe");

		const double maxScale = 16;
		const double minScale = 0.002;

		const int cellRenderResolution = 4096; // (Recommend 4096) use 16384 for minor increase in quality only if you have 12h to wait and a high-end PC
		const int worldspaceRenderResolution = 16384;
		const int nativeResolution = 4096;
		const int SSAA = 2; // 0,1,2
		const bool keepDDSRender = false; // Whether or not to keep the raw DDS outputs after they're converted to JPEG
		const int cellQuality = 85; // The % quality of the final JPEGs
		const int worldspaceQuality = 100;

		// Manually-adjusted camera heights for cells which would otherwise be predominantly obscured by a roof or ceiling
		static readonly Dictionary<string, int> recommendedHeights = new Dictionary<string, int>()
		{
			{ "BackBayTallOffice01", 700 },
			{ "BeaconHillBostonBugle", 1000 },
			{ "BigJohnsSalvage01", 500 },
			{ "BostonPublicLibrary02", 1100 },
			{ "CambridgeChurchGraveyard01", 700 },
			{ "CambridgeCollegeAdminBuilding", 4200 },
			{ "CambridgeConstructionSiteWarehouse01", 5700 },
			{ "CambridgeDiner01", -2300 },
			{ "CambridgeMassChemical01", 0 },
			{ "CambridgePlumbing01", 500 },
			{ "CambridgeScienceCenter01", 300 },
			{ "CharlestownDrugDen", 500 },
			{ "CharlestownHouse01", 700 },
			{ "CorvegaAssemblyPlant01", 1100 },
			{ "CroupManor01", -50 },
			{ "CustomHouseTower01", 500 },
			{ "DBTechHighSchool01", 1700 },
			{ "DLC03AlderseaDaySpa01", 200 },
			{ "DLC03BeaverCreekLanes01", 600 },
			{ "DLC03EchoLakeLumber01", 700 },
			{ "DLC03FarHarborLastPlank", 2000 },
			{ "DLC03PineCrestCavern01", 0 },
			{ "DLC03VimPopFactory01", 500 },
			{ "DLC04ColaCars01", 300 },
			{ "DLC04Gauntlet01", 2000 },
			{ "DLC04GrandchesterMansion01", 1000 },
			{ "DLC04KiddieKingdomTheater01", 1000 },
			{ "DLC04KiddieKingdomZ01", 700 },
			{ "DLC04Nukacade01", 500 },
			{ "DLC04SafariPrimateHouse", 500 },
			{ "DLC04SafariReptileHouse", 500 },
			{ "DmndAllFaithsChapel01", 50 },
			{ "DmndArturosHouse01", 250 },
			{ "DmndArturosHouse02", 200 },
			{ "DmndChoiceChops01", 120 },
			{ "DmndEarlsHouse01", 120 },
			{ "DmndGreenhouse01", 350 },
			{ "DmndJohnsHouse01", 300 },
			{ "DmndMoeCroninsHouse01", 300 },
			{ "DmndOutfieldShengsHouse01", 300 },
			{ "DmndPembrokesHouse01", 250 },
			{ "DmndPlayerHouse01", 600 },
			{ "DmndPublick01", 500 },
			{ "DmndSchoolhouse01", 400 },
			{ "DmndScienceCenter01", 600 },
			{ "DmndSolomonsHouse01", 250 },
			{ "DmndStandsCodman01", 650 },
			{ "DmndStandsHawthorne01", 150 },
			{ "DmndStandsKellogg01", 300 },
			{ "DmndStandsTaphouse01", 200 },
			{ "DmndSurgeryBasement01", 75 },
			{ "DmndValentines01", 400 },
			{ "DmndWarehouseA01", 200 },
			{ "DmndWaterfrontCrockersHouse01", 500 },
			{ "DmndWaterfrontSunsHouse01", 400 },
			{ "ElevMinUseTransUtil", 200 },
			{ "ElevTransHiTech", 200 },
			{ "ElevTransHiTechInstitute", 200 },
			{ "ElevTransPub", 200 },
			{ "ElevTransUtil", 200 },
			{ "ElevTransVault", 200 },
			{ "EsplanadeChurch01", 400 },
			{ "EsplanadeMansion01", 400 },
			{ "FensParkviewApartments02", -100 },
			{ "Financial06", -300 },
			{ "FortHagen01", 600 },
			{ "FourLeafFishpacking01", 200 },
			{ "FourLeafFishpacking02", 0 },
			{ "GeneralAtomicsGalleria01", 100 },
			{ "GlowingSeaPOIDB05Int", 550 },
			{ "GlowingSeaPOIDB06Int", 1100 },
			{ "GoodneighborBobbisPlace", 400 },
			{ "GoodneighborOldStateHouse", 600 },
			{ "GoodneighborTheMemoryDen", 600 },
			{ "GoodneighborTheThirdRail", -200 },
			{ "GoodneighborWarehouse01", 900 },
			{ "GoodneighborWarehouse02", 600 },
			{ "GoodneighborWarehouse03", 600 },
			{ "GwinnettBrewery01", 1000 },
			{ "HardwareTown01", 500 },
			{ "HestersRobotics01", 500 },
			{ "HubrisComics01", 1000 },
			{ "InstituteTunnel01", 3000 },
			{ "IrishPrideShipyard01", 1300 },
			{ "Libertalia01", 1200 },
			{ "LongneckLukowskis01", 2000 },
			{ "MassFusion02Trans", 200 },
			{ "NahantOceanSociety01", 600 },
			{ "NHMFreightDepot01", 600 },
			{ "NorthEndBoxingGym", 200 },
			{ "ParsonsState01", 200 },
			{ "PickmanGallery01", 1000 },
			{ "PoseidonEnergy02", 400 },
			{ "PoseidonReservoir01", 1000 },
			{ "PrewarTVStudio", 900 },
			{ "PrydwenHull01", 600 },
			{ "QuincyPD01", 1000 },
			{ "RailroadHQ01", 100 },
			{ "RelayTowerInt03", 100 },
			{ "RelayTowerInt04", 800 },
			{ "RelayTowerInt05", 1800 },
			{ "RelayTowerInt08", 1300 },
			{ "RelayTowerInt09", 800 },
			{ "REObject02Interior", 300 },
			{ "SanctuaryRosaHouse", 100 },
			{ "SaugusIronworks01", 1000 },
			{ "SaugusIronworks02", 2000 },
			{ "SentinelSite01", 100 },
			{ "SlocumsJoeHQOffice", 200 },
			{ "SouthBoston25", 300 },
			{ "SuffolkCountyCharterSchool01", 200 },
			{ "SuperDuperMart01", 200 },
			{ "Theater16PearwoodResidences01", -1200 },
			{ "Theater27TickerTapeLounge", 4600 },
			{ "TheaterHub360", 200 },
			{ "TheaterMassBayMedicalCenter01", -2000 },
			{ "TheaterMassBayMedicalCenter02", 1600 },
			{ "TheCastle01", 2000 },
			{ "ThicketExcavations01", 400 },
			{ "TrinityChurch01", 800 },
			{ "USSConstitution01", -1900 },
			{ "Waterfront12", 600 },
			{ "WRVRBroadcastCenter01", 200 },
			{ "zLexingtonLaundromat", -600 },
			{ "zLexingtonPharmacy", -800 },
			{ "zPOIJoel06", 200 },
			{ "zWaystation", 200 },
		};

		// Cells which are so small, fo76utils won't render at 16k, so we force render at native 4k
		static readonly List<string> extraSmallCells = new List<string>()
		{
			"ElevTransHiTechInstitute",
			"ElevTransHiTech",
			"ElevTransPub",
			"MassFusion02Trans",
		};

		// Debug parameters
		static readonly bool debugOn = false;
		static readonly string debugEditorID = "";
		static readonly int debugNudgeX = 0;
		static readonly int debugNudgeY = 0;
		static readonly float debugScale = 1f;
		static readonly int debugCameraZ = 65536;

		// Renders a space with parameters setup for debugging, designed to be used to find the appropriate scale/nudge/z-heights for spaces
		static void DebugRender()
		{
			Console.WriteLine("Debug Rendering " + debugEditorID);

			SqliteConnection connection = new SqliteConnection("Data Source=" + databasePath + ";Mode=ReadOnly");
			connection.Open();
			SqliteCommand query = connection.CreateCommand();
			query.CommandText = $"SELECT spaceFormID, spaceEditorID, isWorldspace, xCenter, yCenter, xMin, xMax, yMin, yMax, nudgeX, nudgeY, nudgeScale, esmNumber FROM Space_Info WHERE spaceEditorID = '{debugEditorID}'";
			query.Parameters.Clear();
			SqliteDataReader reader = query.ExecuteReader();
			reader.Read();

			Space space = new Space(
				reader.GetString(0),
				reader.GetString(1),
				reader.GetBoolean(2),
				reader.GetInt32(3),
				reader.GetInt32(4),
				Math.Abs(reader.GetInt32(6) - reader.GetInt32(5)),
				Math.Abs(reader.GetInt32(8) - reader.GetInt32(7)),
				debugNudgeX,
				debugNudgeY,
				debugScale,
				reader.GetInt32(12));

			if (space.formID == string.Empty)
			{
				space.formID = "0000003C";
			}

			int resolution = 2048;
			int renderResolution = resolution;
			int range = Math.Max(space.xRange, space.yRange);
			double scale = ((double)resolution / range) * space.nudgeScale;

			string renderFile = $"{imageDirectory}{space.editorID}{(space.isWorldspace ? "_render_debug" : string.Empty)}.dds";
			double cameraX = space.xCenter - (space.nudgeX * (renderResolution / 4096d) / scale);
			double cameraY = space.yCenter + (space.nudgeY * (renderResolution / 4096d) / scale);

			string renderCommand = $"{utilsRenderPath} {GetESMPath(space.esmNumber)} {renderFile} {resolution} {resolution} " +
				$"\"{fo4DataPath}\" -w 0x{space.formID} -l 0 -cam {scale} 180 0 0 {cameraX} {cameraY} {debugCameraZ} " +
				$"-light 1.8 65 180 -rq 0 -scol 1 -ssaa 0 -ltxtres 64 -mlod 4 -xm fog -xm cloud -xm effects {(space.isWorldspace ? "-xm meshes" : "")}";

			Process render = Process.Start("CMD.exe", "/C " + renderCommand);
			render.WaitForExit();

			Process.Start(new ProcessStartInfo { FileName = renderFile, UseShellExecute = true });
		}

		public static void Main()
		{
			Console.Title = "Commonwealth Cartography Background Renderer";

			if (!File.Exists(magickPath))
			{
				Console.WriteLine($"Can't find ImageMagick at {magickPath}, please check the hardcoded path is correct and you have an installation at that path.");
				Console.ReadKey();
				return;
			}

			if (!File.Exists(utilsRenderPath))
			{
				Console.WriteLine($"Can't find fo76utils render at {utilsRenderPath}, please check the hardcoded path is correct and you have it placed at that path.");
				Console.ReadKey();
				return;
			}

			if (!File.Exists(databasePath))
			{
				Console.WriteLine($"Can't find Commonwealth Cartography database at {databasePath}, please check the database has been built or copied from a release to that path.");
				Console.ReadKey();
				return;
			}

			if (debugOn)
			{
				DebugRender();
				return;
			}

			Console.WriteLine("Paste space-separated EditorIDs of Cells or Worldspaces you need rendering. Otherwise paste nothing to render all");
			string arg = Console.ReadLine() ?? string.Empty;
			List<string> args = arg.Split(' ').Where(a => !string.IsNullOrWhiteSpace(a)).ToList();

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			if (args.Count == 0)
			{
				Console.WriteLine("Rendering all spaces in database...");
			}
			else
			{
				Console.WriteLine("\nOnly rendering spaces with the following EditorIDs:");
				foreach (string editorId in args)
				{
					Console.WriteLine(editorId);
				}

				Console.WriteLine();
			}

			Console.WriteLine($"Rendered cells will be placed at {cellDirectory}\nRendered Worldspaces at {imageDirectory}\n");
			if (!Directory.Exists(cellDirectory))
			{
				Directory.CreateDirectory(cellDirectory);
			}

			List<Space> spaces = new List<Space>();

			SqliteConnection connection = new SqliteConnection("Data Source=" + databasePath + ";Mode=ReadOnly");
			connection.Open();

			SqliteCommand query = connection.CreateCommand();
			query.CommandText = "SELECT spaceFormID, spaceEditorID, isWorldspace, xCenter, yCenter, xMin, xMax, yMin, yMax, nudgeX, nudgeY, nudgeScale, esmNumber FROM Space_Info ORDER BY isWorldspace desc";
			query.Parameters.Clear();
			SqliteDataReader reader = query.ExecuteReader();

			while (reader.Read())
			{
				string formId = reader.GetString(0);
				string editorId = reader.GetString(1);

				// We specified certain cells, so skip everything not asked for
				if (args.Count > 0 && !args.Contains(editorId))
				{
					Console.WriteLine("Skipping " + editorId);
					continue;
				}

				// Account for database hack which replaces repeated Commonwealth formid with empty string
				if (formId == string.Empty)
				{
					formId = "0000003C";
				}

				spaces.Add(new Space(
					formId,
					editorId,
					reader.GetBoolean(2),
					reader.GetInt32(3),
					reader.GetInt32(4),
					Math.Abs(reader.GetInt32(6) - reader.GetInt32(5)),
					Math.Abs(reader.GetInt32(8) - reader.GetInt32(7)),
					reader.GetInt32(9),
					reader.GetInt32(10),
					reader.GetFloat(11),
					reader.GetInt32(12)));
			}

			Console.WriteLine($"\nRendering {spaces.Count} space{(spaces.Count == 1 ? string.Empty : "s")}. Cells at {cellRenderResolution}px, Worldspaces at {worldspaceRenderResolution}px");
			int i = 0;

			foreach (Space space in spaces)
			{
				i++;
				Console.WriteLine($"\n0x{space.formID} : {space.editorID} ({i} of {spaces.Count})");
				int resolution = space.isWorldspace ? worldspaceRenderResolution : cellRenderResolution;

				if (extraSmallCells.Contains(space.editorID))
				{
					Console.WriteLine($"Rendering space {space.editorID} at {nativeResolution} instead due to small size");
					resolution = 2048;
				}

				int range = Math.Max(space.xRange, space.yRange);

				if (range == 0)
				{
					LogError($"Space {space.editorID} has a natural range of 0. Unable to properly render.");
					continue;
				}

				double scale = ((double)resolution / range) * space.nudgeScale;

				if (scale > maxScale || scale < minScale)
				{
					LogError($"Space {space.editorID} too small or large to render at this resolution! (Scale of {scale} outside range {minScale}-{maxScale})." +
						$" Change the render resolution in order to preserve the scale. FormID: {space.formID}");
					continue;
				}

				// Default camera height unless a custom height was defined to cut into roofs
				int cameraZ = 65536;
				if (recommendedHeights.ContainsKey(space.editorID))
				{
					cameraZ = recommendedHeights[space.editorID];
				}

				string renderFile = $"{imageDirectory}{space.editorID}{(space.isWorldspace ? "_render" : string.Empty)}.dds";
				string convertedFile = $"{(space.isWorldspace ? imageDirectory : cellDirectory)}{space.editorID}{(space.isWorldspace ? "_render" : string.Empty)}.jpg";

				int renderResolution = space.isWorldspace ? worldspaceRenderResolution : cellRenderResolution;
				double cameraX = space.xCenter - (space.nudgeX * (renderResolution / 4096d) / scale);
				double cameraY = space.yCenter + (space.nudgeY * (renderResolution / 4096d) / scale);

				Console.WriteLine(GetESMPath(space.esmNumber));

				string renderCommand = $"{utilsRenderPath} {GetESMPath(space.esmNumber)} {renderFile} {resolution} {resolution} " +
					$"\"{fo4DataPath}\" -w 0x{space.formID} -l 0 -cam {scale} 180 0 0 {cameraX} {cameraY} {cameraZ} " +
					$"-light 1.8 65 180 -lcolor 1.1 0xD6CCC7 0.9 -1 -1 -hqm meshes -rq 15 -a -scol 1 -ssaa {SSAA} " +
					$"-ltxtres 512 -mip 1 -lmip 2 -mlod 0 -ndis 1 -deftxt 0x000AB07E -xm fog -xm cloud -xm effects";

				string resizeCommand = $"\"{magickPath}\" convert {renderFile} -resize {nativeResolution}x{nativeResolution} " +
						$"-quality {(space.isWorldspace ? worldspaceQuality : cellQuality)} JPEG:{convertedFile}";

				Process render = Process.Start("CMD.exe", "/C " + renderCommand);
				render.WaitForExit();

				if (File.Exists(renderFile))
				{
					Console.WriteLine($"Converting and downsampling with ImageMagick...");
					Process magickResizeConvert = Process.Start("CMD.exe", "/C " + resizeCommand);
					magickResizeConvert.WaitForExit();

					if (!keepDDSRender)
					{
						File.Delete(renderFile);
					}
				}
				else
				{
					LogError($"No file {renderFile}, maybe it was not rendered?");
				}

				// Also render the watermask dds
				// Different render params and we don't convert the DDS
				if (space.isWorldspace)
				{
					Console.WriteLine($"\n0x{space.formID} : {space.editorID} (Water Mask) ({i} of {spaces.Count})");

					string waterMaskRenderFile = $"{imageDirectory}{space.editorID}_waterMask.dds";

					string waterMaskRenderCommand = $"{utilsRenderPath} {GetESMPath(space.esmNumber)} {waterMaskRenderFile} {resolution} {resolution} " +
						$"\"{fo4DataPath}\" -w 0x{space.formID} -l 0 -cam {scale} 180 0 0 {cameraX} {cameraY} {cameraZ} " +
						$"-light 1 0 0 -ssaa {SSAA} -ltxtres 64 -wtxt \"\" -wrefl 0 -watercolor 0x7F0000FF " +
						$"-xm bog -xm swamp -xm forest -xm grass -xm plants -xm trees -xm water -xm fog -xm cloud -xm effects";

					Process waterMaskRender = Process.Start("CMD.exe", "/C " + waterMaskRenderCommand);
					waterMaskRender.WaitForExit();
				}

				// Calc and log est time remaining during batch job
				if (i != spaces.Count)
				{
					int remainingCells = spaces.Count - i;
					TimeSpan timePerCell = stopwatch.Elapsed / i;
					TimeSpan estTimeRemain = timePerCell * remainingCells;
					Console.WriteLine("\nEst. Time remaining: " + estTimeRemain);
				}
			}

			Console.WriteLine($"Finished in {stopwatch.Elapsed}");
			Console.ReadKey();
		}

		static string GetESMPath(int esmNumber)
		{
			string initialPath = $"\"{fo4DataPath}\\Fallout4.esm\"";
			switch (esmNumber)
			{
				case 0:
					return initialPath;
				case 2:
					return initialPath + $",\"{fo4DataPath}\\DLCRobot.esm\"";
				case 4:
					return initialPath + $",\"{fo4DataPath}\\DLCCoast.esm\"";
				case 6:
					return initialPath + $",\"{fo4DataPath}\\DLCworkshop03.esm\"";
				case 7:
					return initialPath + $",\"{fo4DataPath}\\DLCNukaWorld.esm\"";

				default:
					LogError("Unknown ESM number (" + esmNumber + ")");
					return string.Empty;
			}
		}

		static void LogError(string err)
		{
			Console.WriteLine(err);
			File.AppendAllText(imageDirectory + "\\errors.txt", $"{DateTime.Now} {err}\n");
		}
	}
}
