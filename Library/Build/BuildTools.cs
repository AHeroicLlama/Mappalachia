using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;

namespace Library
{
	// Provides references to files around the repo structure and other build-only helpers.
	// Intended for use only by pre-production preprocessor & build projects.
	public static partial class BuildTools
	{
		static string? solutionPath = null;

		static string SolutionFile { get; } = "Mappalachia.sln";

		static string AssetsPath { get; } = GetRepoRoot() + @"Assets\";

		static string UtilitiesPath { get; } = GetRepoRoot() + @"Utilities\";

		static string OutputsPath { get; } = GetRepoRoot() + @"BuildOutputs\";

		static string BGRendererPath { get; } = GetRepoRoot() + @"BackgroundRenderer\";

		static string BGRendererCorrectionsPath { get; } = @$"{BGRendererPath}Corrections\";

		public static string PreviousAssetsSpotlightPath { get; } = @$"{BGRendererPath}PreviousVersion\spotlight\";

		public static string SpotlightPatchDiffPath { get; } = OutputsPath + @"SpotlightPatchDiff\spotlight\";

		public static string MapIconExtractPath { get; } = GetRepoRoot() + @"MapIconProcessor\Extract\";

		public static string SqlitePath { get; } = UtilitiesPath + "sqlite3.exe"; // https://www.sqlite.org/download.html

		public static string Fo76UtilsRenderPath { get; } = UtilitiesPath + @"fo76utils\render.exe"; // https://github.com/fo76utils/fo76utils

		public static string DiscardedCellsPath { get; } = OutputsPath + @"Discarded_Cells.csv";

		public static string DatabaseSummaryPath { get; } = OutputsPath + @"Database_Summary.txt";

		public static string ErrorsPath { get; } = OutputsPath + @"Errors.txt";

		public static string TempPath { get; } = OutputsPath + @"Temp\";

		public static string Fo76EditOutputPath { get; } = GetRepoRoot() + @"FO76Edit\Output\";

		public static string CellXYScaleCorrectionPath { get; } = BGRendererCorrectionsPath + @"XY_Scale\";

		public static string CellZCorrectionPath { get; } = BGRendererCorrectionsPath + @"Z\";

		public static string DataPath { get; } = AssetsPath + @"data\";

		public static string DatabasePath { get; } = DataPath + @"mappalachia.db";

		public static string ImagePath { get; } = AssetsPath + @"img\";

		public static string CellPath { get; } = ImagePath + @"cell\";

		public static string IconPath { get; } = ImagePath + @"icon\";

		public static string WorldPath { get; } = ImagePath + @"wrld\";

		public static string SpotlightPath { get; } = ImagePath + @"spotlight\";

		public static string MapMarkerPath { get; } = ImagePath + @"mapmarker\";

		public static string DoorMarkerPath { get; } = ImagePath + "DoorMarker" + Common.MapMarkerImageFileType;

		public static string CompassRosePath { get; } = ImagePath + "CompassRose.png";

		static string GamePath { get; } = @"C:\Program Files (x86)\Steam\steamapps\common\Fallout76\";

		public static string GameDataPath { get; } = @$"{GamePath}Data\";

		public static string GameESMPath { get; } = @$"{GameDataPath}SeventySix.esm";

		public static string GameTerrainPath { get; } = @$"{GameDataPath}Terrain\Appalachia.btd";

		public static string GameExePath { get; } = $"{GamePath}Fallout76.exe";

		public static ConsoleColor ColorInfo { get; } = ConsoleColor.DarkYellow;

		public static ConsoleColor ColorQuestion { get; } = ConsoleColor.Blue;

		public static ConsoleColor ColorError { get; } = ConsoleColor.Red;

		public static int Kilobyte { get; } = (int)Math.Pow(2, 10);

		public static int HashPrime { get; } = 31;

		public static int WorldspaceRenderResolution { get; } = (int)Math.Pow(2, 14); // 16k

		public static int PlotIconSize { get; } = 256;

		static Regex TileFileNameValidation { get; } = new Regex(@"(-?[0-9]{1,2}\.){2}jpg");

		static ReaderWriterLock ErrorLogLock { get; } = new ReaderWriterLock();

		static BuildTools()
		{
			if (Directory.Exists(TempPath))
			{
				Directory.Delete(TempPath, true);
			}

			Directory.CreateDirectory(TempPath);
		}

		public static SqliteConnection GetNewConnection()
		{
			return CommonDatabase.GetNewConnection(DatabasePath, false);
		}

		// Returns the root of the repository, where the sln file lives
		// Value is cached in solutionPath so is only calculated once per launch
		static string GetRepoRoot()
		{
			if (solutionPath is not null)
			{
				return solutionPath;
			}

			string path = AppDomain.CurrentDomain.BaseDirectory;

			while (!File.Exists(path + @"\" + SolutionFile))
			{
				DirectoryInfo parent = Directory.GetParent(path) ?? throw new FileNotFoundException($"Unable to find {SolutionFile} in this or any parent path");

				path = parent.ToString();
			}

			solutionPath = path + @"\";
			return solutionPath;
		}

		// Returns the MD5 Hash of the given file
		public static string GetMD5Hash(string filePath)
		{
			using FileStream fileStream = File.OpenRead(filePath);

			byte[] hash = MD5.Create().ComputeHash(fileStream);
			return Convert.ToHexString(hash).Replace("-", string.Empty);
		}

		// Calls sqlitetools (sqlite3.exe) with arguments and returns the output
		public static string SqliteTools(string args)
		{
			Process process = Process.Start(new ProcessStartInfo
			{
				FileName = SqlitePath,
				Arguments = args,
				RedirectStandardOutput = true,
			}) ?? throw new Exception("Failed to start Sqlite3 process");

			string output = process.StandardOutput.ReadToEnd().Trim();

			process.WaitForExit();
			return output;
		}

		// Appends the text to the error log file
		public static void AppendToErrorLog(string error)
		{
			try
			{
				ErrorLogLock.AcquireWriterLock(5000);
				File.AppendAllLines(ErrorsPath, new List<string>() { error });
			}
			finally
			{
				ErrorLogLock.ReleaseWriterLock();
			}
		}

		public static void StdOutWithColor(string text, ConsoleColor color)
		{
			Console.ForegroundColor = color;
			Console.WriteLine(text);
			Console.ResetColor();
		}

		public static string WithoutTrailingSlash(this string path)
		{
			return path.TrimEnd('\\', '/');
		}

		// Return the final file path of the spotlight tile
		public static string GetFilePath(this SpotlightTile tile)
		{
			return $"{SpotlightPath}{tile.Space.EditorID}\\{tile.XId}.{tile.YId}{Common.SpotlightTileImageFileType}";
		}

		// Filters through the spotlight folder and attempts to locate and remove redundant old tiles/folders or rogue files/folders
		public static async Task CleanUpSpotlight()
		{
			StdOutWithColor("Checking for unnecessary tile files...", ColorInfo);
			List<Space> spaces = await CommonDatabase.GetAllSpaces(GetNewConnection());

			if (!Directory.Exists(SpotlightPath))
			{
				StdOutWithColor("Spotlight path not found - unable to cleanup", ColorError);
				return;
			}

			// The spotlight directory should have no file at root
			foreach (string file in Directory.GetFiles(SpotlightPath))
			{
				StdOutWithColor($"Deleting rogue file {file}", ColorInfo);
				File.Delete(file);
			}

			foreach (string directory in Directory.GetDirectories(SpotlightPath))
			{
				// The folder for each space should contain no folders
				foreach (string innerDirectory in Directory.GetDirectories(directory))
				{
					StdOutWithColor($"Deleting rogue directory {innerDirectory}", ColorInfo);
					Directory.Delete(innerDirectory, true);
				}

				// Find the space this folder represents
				Space? space = spaces.Where(space => space.EditorID == Path.GetFileName(directory)).FirstOrDefault();

				// If the space no longer exists, or, if the space no longer benefits from spotlight, delete its folder
				if (space is null || !space.IsSuitableForSpotlight())
				{
					Directory.Delete(directory, true);
					StdOutWithColor($"Deleting spotlight folder {directory}", ColorInfo);
					continue;
				}

				List<SpotlightTile> tiles = space.GetTiles();

				// Loop every candidate tile file in the space folder
				foreach (string tilePath in Directory.GetFiles(directory))
				{
					string tileFileName = Path.GetFileName(tilePath);

					// Delete files not matching the expected name
					if (!TileFileNameValidation.IsMatch(tileFileName))
					{
						StdOutWithColor($"Deleting rogue tile file {tilePath}", ColorInfo);
						File.Delete(tilePath);
						continue;
					}

					int tileX = int.Parse(tileFileName.Split(".")[0]);
					int tileY = int.Parse(tileFileName.Split(".")[1]);

					// Find the X and Y of the tile this file represents. If it is no longer needed by the space, delete it.
					if (!tiles.Where(tile => tile.XId == tileX && tile.YId == tileY).Any())
					{
						StdOutWithColor($"Deleting spotlight tile {tilePath}", ColorInfo);
						File.Delete(tilePath);
					}
				}
			}
		}
	}
}
