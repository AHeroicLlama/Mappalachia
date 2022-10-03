using Microsoft.Data.Sqlite;
using System.Diagnostics;

namespace BackgroundRenderer
{
	public partial class BackgroundRenderer
	{
		static string fo76DataPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Fallout76\\Data";
		static string thisAppPath = Directory.GetCurrentDirectory();
		static string mappalachiaRoot = thisAppPath + "..\\..\\..\\..\\..\\";
		static string databasePath = mappalachiaRoot + "Mappalachia\\data\\mappalachia.db";
		static string imageDirectory =  mappalachiaRoot + "Mappalachia\\img\\";
		static string utilsRenderPath = mappalachiaRoot + "FO76Utils\\render.exe";

		static double maxScale = 16;
		static double minScale = 0.02;

		static bool openFileAfterRender = false;

		// Manually-adjusted camera heights for cells which would otherwise be predominantly obscured by a roof or ceiling
		static Dictionary<string, int> recommendedHeights = new Dictionary<string, int>()
		{
			{"AMSHQ01", 3000},
			{"BlueRidgeOffice01", 350},
			{"CraterWarRoom01", 50},
			{"CraterWatchstation01", -700},
			{"DuncanDuncanRobotics01", 500},
			{"FortAtlas01", -1300},
			{"FoundationSupplyRoom01", 200},
			{"FraternityHouse01", 850},
			{"FraternityHouse02", 850},
			{"LewisandSonsFarmingSupply01", 500},
			{"OverseersHome01", 675},
			{"PoseidonPlant02", 3000},
			{"RaiderCave01", 300},
			{"RaiderCave03", 300},
			{"RaiderRaidTrailerInt", 150},
			{"SugarGrove02", 1000},
			{"TheWayward01", 400},
			{"TopOfTheWorld01", -1800},
			{"ValleyGalleria01", 700},
			{"Vault63Entrance", 4750},
			{"Vault79Entrance", -200},
			{"VTecAgCenter01", 400},
			{"WVLumberCo01", 1000},
			{"XPDPitt02Sanctum", 700},
		};

		// 16384 by default (16k - allows us to supersample then scale down to 4k)
		/* 4096 if the cell is unusually small and causes the scale to be too high...
		(errors.txt will be written) (EG UCB02 and RaiderRaidTrailerInt) */
		static int resolution = 16384;

		static bool SSAA = true;

		public static void Main()
		{
			Console.Title = "Mappalachia Background Renderer";

			Console.WriteLine("Paste space-separated EditorIDs of Cells you need rendering. Otherwise paste nothing to render all");
			string arg = Console.ReadLine();
			List<string> args = arg.Split(' ').Where(a => !string.IsNullOrWhiteSpace(a)).ToList();

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

			Console.WriteLine($"\nRendering {spaces.Count} cells at {resolution}*{resolution}px");
			int i = 0;

			foreach (Space space in spaces)
			{
				i++;
				Console.WriteLine($"\n0x{space.formID} : {space.editorID} ({i} of {spaces.Count})");

				int range = Math.Max(space.xRange, space.yRange);
				double scale = ((double)resolution / range) * space.nudgeScale;

				if (scale > maxScale || scale < minScale)
				{
					string error = $"Space {space.editorID} too small or large to render at this resolution! (Scale of {scale} outside range {minScale}-{maxScale}). Change the render resolution in order to preserve the scale\nFormID: {space.formID}\n";
					Console.WriteLine(error);
					File.AppendAllText(imageDirectory + "\\errors.txt", error);
				}

				// Default camera height unless a custom height was defined to cut into roofs
				int cameraY = 65536;
				if (recommendedHeights.ContainsKey(space.editorID))
				{
					cameraY = recommendedHeights[space.editorID];
				}

				string file = $"{imageDirectory}{space.editorID}.dds";
				Process render = Process.Start("CMD.exe", "/C " + $"{utilsRenderPath} \"{fo76DataPath}\\SeventySix.esm\" {file} {resolution} {resolution} \"{fo76DataPath}\" -w 0x{space.formID} -l 0 -cam {scale} 180 0 0 {space.xCenter + (nudgeX * (resolution / 4096d) / scale)} {space.yCenter + (nudgeY * (resolution / 4096d) / scale)} {cameraY} -light 1.25 63.435 41.8103 -ssaa {(SSAA ? 1 : 0)} -hqm meshes -env textures/shared/cubemaps/mipblur_defaultoutside1.dds -wtxt textures/water/defaultwater.dds -ltxtres 1024 -mip 0 -lmip 1 -mlod 0 -ndis 1");
				render.WaitForExit();

				if (openFileAfterRender)
				{
					Process.Start(new ProcessStartInfo { FileName = file, UseShellExecute = true });
				}
			}
		}
	}
}