using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace BackgroundRenderer
{
	public partial class BackgroundRenderer
	{
		const string magickPath = "C:\\Program Files\\ImageMagick-7.1.1-Q16-HDRI\\magick.exe";
		const string fo76DataPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Fallout76\\Data";
		static readonly string thisAppPath = Directory.GetCurrentDirectory();
		static readonly string mappalachiaRoot = thisAppPath + "..\\..\\..\\..\\..\\";
		static readonly string databasePath = Path.GetFullPath(mappalachiaRoot + "Mappalachia\\data\\mappalachia.db");
		static readonly string imageDirectory = Path.GetFullPath(mappalachiaRoot + "Mappalachia\\img\\");
		static readonly string cellDirectory = Path.GetFullPath(imageDirectory + "cell\\");
		static readonly string utilsRenderPath = Path.GetFullPath(mappalachiaRoot + "FO76Utils\\render.exe");

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
			{ "AMSHQ01", 3000 },
			{ "BlueRidgeOffice01", 350 },
			{ "CraterWarRoom01", 50 },
			{ "CraterWatchstation01", -700 },
			{ "DuncanDuncanRobotics01", 500 },
			{ "FortAtlas01", -1300 },
			{ "FoundationSupplyRoom01", 200 },
			{ "FraternityHouse01", 850 },
			{ "FraternityHouse02", 850 },
			{ "LewisandSonsFarmingSupply01", 500 },
			{ "OverseersHome01", 675 },
			{ "PoseidonPlant02", 3000 },
			{ "RaiderCave01", 300 },
			{ "RaiderCave03", 300 },
			{ "RaiderRaidTrailerInt", 150 },
			{ "SugarGrove02", 1000 },
			{ "TheWayward01", 400 },
			{ "TopOfTheWorld01", -1800 },
			{ "ValleyGalleria01", 700 },
			{ "Vault63Entrance", 4750 },
			{ "Vault79Entrance", -200 },
			{ "VTecAgCenter01", 400 },
			{ "WVLumberCo01", 1000 },
			{ "XPDPitt02Sanctum", 700 },
			{ "TheCraterCore01", 100 },
			{ "SheltersSoundStage", 1000 },
			{ "SheltersToxicWasteland", 2000 },
			{ "SheltersRootCellar", 300 },
		};

		// Cells which are so small, fo76utils won't render at 16k, so we force render at native 4k
		static readonly List<string> extraSmallCells = new List<string>()
		{
			"UCB02",
			"RaiderRaidTrailerInt",
		};

		public static void Main()
		{
			Console.Title = "Mappalachia Background Renderer";

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
				Console.WriteLine($"Can't find Mappalachia database at {databasePath}, please check the database has been built or copied from a release to that path.");
				Console.ReadKey();
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
			query.CommandText = "SELECT spaceFormID, spaceEditorID, isWorldspace, xCenter, yCenter, xMin, xMax, yMin, yMax, nudgeX, nudgeY, nudgeScale FROM Space_Info ORDER BY isWorldspace desc";
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

				// Account for database hack which replaces repeated Appalachia formid with empty string
				if (formId == string.Empty)
				{
					formId = "0025DA15";
				}

				// Skip CharGen02-05 as they are duplicates of 01
				// Mappalachia GUI will target CharGen01 for all of these
				if (editorId.StartsWith("CharGen") && editorId != "CharGen01")
				{
					Console.WriteLine($"Skipping {editorId} as duplicate");
					continue;
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
					reader.GetFloat(11)));
			}

			Console.WriteLine($"\nRendering {spaces.Count} space{(spaces.Count == 1 ? string.Empty : "s")}. Cells at {cellRenderResolution}px, Worldspaces at {worldspaceRenderResolution}px");
			int i = 0;

			foreach (Space space in spaces)
			{
				i++;
				Console.WriteLine($"\n0x{space.formID} : {space.editorID} ({i} of {spaces.Count})");
				int resolution = space.isWorldspace ? worldspaceRenderResolution : cellRenderResolution;

				if (resolution > nativeResolution && extraSmallCells.Contains(space.editorID))
				{
					Console.WriteLine($"Rendering space {space.editorID} at {nativeResolution} instead due to small size");
					resolution = nativeResolution;
				}

				int range = Math.Max(space.xRange, space.yRange);

				if (range == 0)
				{
					LogError($"Space {space.editorID} has a natural range of 0. Unable to properly render.");
					continue;
				}

				double scale = ((double)resolution / range) * space.nudgeScale;

				// Override Appalachia scale to match in-game map
				if (space.editorID == "Appalachia")
				{
					scale = 0.028 / (16384d / worldspaceRenderResolution);
					space.xCenter = 0;
					space.yCenter = 0;
				}

				if (scale > maxScale || scale < minScale)
				{
					LogError($"Space {space.editorID} too small or large to render at this resolution! (Scale of {scale} outside range {minScale}-{maxScale})." +
						$" Change the render resolution in order to preserve the scale. FormID: {space.formID}");
				}

				// Default camera height unless a custom height was defined to cut into roofs
				int cameraZ = 65536;
				if (recommendedHeights.ContainsKey(space.editorID))
				{
					cameraZ = recommendedHeights[space.editorID];
				}

				string renderFile = $"{imageDirectory}{space.editorID}{(space.isWorldspace ? "_render" : string.Empty)}.dds";
				string convertedFile = $"{(space.isWorldspace ? imageDirectory : cellDirectory)}{space.editorID}{(space.isWorldspace ? "_render" : string.Empty)}.jpg";

				string terrainString = space.isWorldspace ? $"-btd \"{fo76DataPath}\\Terrain\\Appalachia.btd\" " : string.Empty;

				int renderResolution = space.isWorldspace ? worldspaceRenderResolution : cellRenderResolution;
				double cameraX = space.xCenter - (space.nudgeX * (renderResolution / 4096d) / scale);
				double cameraY = space.yCenter + (space.nudgeY * (renderResolution / 4096d) / scale);

				// -rq 1 + 2 + 12 + 32 + 256
				string renderCommand = $"{utilsRenderPath} \"{fo76DataPath}\\SeventySix.esm\" {renderFile} {resolution} {resolution} " +
					$"\"{fo76DataPath}\" {terrainString} -w 0x{space.formID} -l 0 -cam {scale} 180 0 0 {cameraX} {cameraY} {cameraZ} " +
					$"-light 1.8 65 180 -lcolor 1.1 0xD6CCC7 0.9 -1 -1 -rq 303 -ssaa {SSAA} " +
					$"-ltxtres 512 -mip 1 -lmip 2 -mlod 0 -ndis 1 -xm babylon -xm fog -xm cloud";

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

					string waterMaskRenderCommand = $"{utilsRenderPath} \"{fo76DataPath}\\SeventySix.esm\" {waterMaskRenderFile} {resolution} {resolution} " +
						$"\"{fo76DataPath}\" {terrainString} -w 0x{space.formID} -l 0 -cam {scale} 180 0 0 {cameraX} {cameraY} {cameraZ} " +
						$"-light 1 0 0 -ssaa {SSAA} -ltxtres 64 -wtxt \"\" -wrefl 0 -watercolor 0x7F0000FF " +
						$"-xm babylon -xm bog -xm swamp -xm forest -xm grass -xm plants -xm trees -xm water -xm babylon -xm fog -xm cloud -xm effects";

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

		static void LogError(string err)
		{
			Console.WriteLine(err);
			File.AppendAllText(imageDirectory + "\\errors.txt", $"{DateTime.Now} {err}\n");
		}
	}
}
