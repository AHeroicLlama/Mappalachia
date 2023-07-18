using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace BackgroundRenderer
{
	public partial class BackgroundRenderer
	{
		const string magickPath = "C:\\Program Files\\ImageMagick-7.1.0-Q16-HDRI\\magick.exe";
		const string fo76DataPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Fallout76\\Data";
		static readonly string thisAppPath = Directory.GetCurrentDirectory();
		static readonly string mappalachiaRoot = thisAppPath + "..\\..\\..\\..\\..\\";
		static readonly string databasePath = Path.GetFullPath(mappalachiaRoot + "Mappalachia\\data\\mappalachia.db");
		static readonly string imageDirectory = Path.GetFullPath(mappalachiaRoot + "Mappalachia\\img\\");
		static readonly string outputDirectory = Path.GetFullPath(imageDirectory + "cell\\");
		static readonly string utilsRenderPath = Path.GetFullPath(mappalachiaRoot + "FO76Utils\\render.exe");

		const double maxScale = 16;
		const double minScale = 0.02;

		const int targetRenderResolution = 4096; // (Recommend 4096) use 16384 for minor increase in quality only if you have 12h to wait and a high-end PC
		const int nativeResolution = 4096;
		const int SSAA = 2; // 0,1,2
		const bool keepDDSRender = false; // Whether or not to keep the raw DDS outputs after they're converted to JPEG
		const int jpegQuality = 85; // The % quality of the final JPEGs

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

			Console.WriteLine("Paste space-separated EditorIDs of Cells you need rendering. Otherwise paste nothing to render all");
			string arg = Console.ReadLine() ?? string.Empty;
			List<string> args = arg.Split(' ').Where(a => !string.IsNullOrWhiteSpace(a)).ToList();

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			if (args.Count == 0)
			{
				Console.WriteLine("Rendering all cells in database...");
			}
			else
			{
				Console.WriteLine("\nOnly rendering Cells with the following EditorIDs:");
				foreach (string editorId in args)
				{
					Console.WriteLine(editorId);
				}

				Console.WriteLine();
			}

			Console.WriteLine($"Final outputs will be placed at {outputDirectory}");
			if (!Directory.Exists(outputDirectory))
			{
				Directory.CreateDirectory(outputDirectory);
			}

			List<Space> spaces = new List<Space>();

			SqliteConnection connection = new SqliteConnection("Data Source=" + databasePath + ";Mode=ReadOnly");
			connection.Open();

			SqliteCommand query = connection.CreateCommand();
			query.CommandText = "SELECT spaceFormID, spaceEditorID, isWorldspace, xCenter, yCenter, xMin, xMax, yMin, yMax, nudgeX, nudgeY, nudgeScale FROM Space_Info";
			query.Parameters.Clear();
			SqliteDataReader reader = query.ExecuteReader();

			while (reader.Read())
			{
				// Skip worldspaces
				if (reader.GetInt32(2) == 1)
				{
					continue;
				}

				string editorId = reader.GetString(1);

				// We specified certain cells, so skip everything not asked for
				if (args.Count > 0 && !args.Contains(editorId))
				{
					Console.WriteLine("Skipping " + editorId);
					continue;
				}

				// Skip CharGen02-05 as they are duplicates of 01
				// Mappalachia GUI will target CharGen01 for all of these
				if (editorId.StartsWith("CharGen") && editorId != "CharGen01")
				{
					Console.WriteLine($"Skipping {editorId} as duplicate");
					continue;
				}

				spaces.Add(new Space(reader.GetString(0), editorId, reader.GetInt32(3), reader.GetInt32(4), Math.Abs(reader.GetInt32(6) - reader.GetInt32(5)), Math.Abs(reader.GetInt32(8) - reader.GetInt32(7)), reader.GetInt32(9), reader.GetInt32(10), reader.GetFloat(11)));
			}

			Console.WriteLine($"\nRendering {spaces.Count} cells at {targetRenderResolution}*{targetRenderResolution}px");
			int i = 0;

			foreach (Space space in spaces)
			{
				i++;
				Console.WriteLine($"\n0x{space.formID} : {space.editorID} ({i} of {spaces.Count})");
				int resolution = targetRenderResolution;

				if (resolution > nativeResolution && extraSmallCells.Contains(space.editorID))
				{
					Console.WriteLine($"Rendering space {space.editorID} at {nativeResolution} instead due to small size");
					resolution = nativeResolution;
				}

				int range = Math.Max(space.xRange, space.yRange);
				double scale = ((double)resolution / range) * space.nudgeScale;

				if (scale > maxScale || scale < minScale)
				{
					LogError($"Space {space.editorID} too small or large to render at this resolution! (Scale of {scale} outside range {minScale}-{maxScale}). Change the render resolution in order to preserve the scale\nFormID: {space.formID}\n");
				}

				// Default camera height unless a custom height was defined to cut into roofs
				int cameraY = 65536;
				if (recommendedHeights.ContainsKey(space.editorID))
				{
					cameraY = recommendedHeights[space.editorID];
				}

				string renderFile = $"{imageDirectory}{space.editorID}.dds";
				string convertedFile = $"{outputDirectory}{space.editorID}.jpg";

				Process render = Process.Start("CMD.exe", "/C " + $"{utilsRenderPath} \"{fo76DataPath}\\SeventySix.esm\" {renderFile} {resolution} {resolution} \"{fo76DataPath}\" -w 0x{space.formID} -l 0 -cam {scale} 180 0 0 {space.xCenter - (space.nudgeX * (targetRenderResolution / 4096d) / scale)} {space.yCenter + (space.nudgeY * (targetRenderResolution / 4096d) / scale)} {cameraY} -light 1.8 65 180 -lcolor 1.1 0xD6CCC7 0.9 -1 -1 -hqm meshes -rq 15 -a -scol 1 -ssaa {SSAA} -ltxtres 512 -mip 1 -lmip 2 -mlod 0 -ndis 1 -xm babylon -xm fog -xm cloud -xm effects");
				render.WaitForExit();

				if (File.Exists(renderFile))
				{
					Console.WriteLine($"Converting and downsampling with ImageMagick...");
					Process magickResizeConvert = Process.Start("CMD.exe", "/C " + $"\"{magickPath}\" convert {renderFile} -resize {nativeResolution}x{nativeResolution} -quality {jpegQuality} JPEG:{convertedFile}");
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
			File.AppendAllText(imageDirectory + "\\errors.txt", err);
		}
	}
}
