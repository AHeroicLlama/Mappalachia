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

		static string BGRendererCorrectionsPath { get; } = GetRepoRoot() + @"BackgroundRenderer\Corrections\";

		public static string MapIconExtractPath { get; } = GetRepoRoot() + @"MapIconProcessor\Extract\";

		public static string SqlitePath { get; } = UtilitiesPath + "sqlite3.exe"; // https://www.sqlite.org/download.html

		public static string Fo76UtilsRenderPath { get; } = UtilitiesPath + @"fo76utils\render.exe"; // https://github.com/fo76utils/fo76utils

		public static string DiscardedCellsPath { get; } = OutputsPath + @"Discarded_Cells.csv";

		public static string DatabaseSummaryPath { get; } = OutputsPath + @"Database_Summary.txt";

		public static string ErrorsPath { get; } = OutputsPath + @"Errors.txt";

		public static string TempPath { get; } = OutputsPath + @"Temp\";

		public static string Fo76EditOutputPath { get; } = GetRepoRoot() + @"FO76Edit\Output\";

		public static string DatabasePath { get; } = AssetsPath + @"data\mappalachia.db";

		public static string ImageRootPath { get; } = AssetsPath + @"img\";

		public static string CellXYScaleCorrectionPath { get; } = BGRendererCorrectionsPath + @"XY_Scale\";

		public static string CellZCorrectionPath { get; } = BGRendererCorrectionsPath + @"Z\";

		public static string CellPath { get; } = ImageRootPath + @"cell\";

		public static string WorldPath { get; } = ImageRootPath + @"wrld\";

		public static string SuperResPath { get; } = ImageRootPath + @"super\";

		public static string MapMarkerPath { get; } = ImageRootPath + @"mapmarker\";

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

		static double SuperResImprovementThresholdPerc { get; } = 25; // Percent improvement in resolution necessary to render a super res

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
			if (solutionPath != null)
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

		// Return if the improvement in quality which super res would provide meets the threshold
		public static bool WouldBenefitFromSuperRes(this Space space)
		{
			double scale = 1d / Common.SuperResScale;
			int singleRenderResolution = space.IsWorldspace ? WorldspaceRenderResolution : Common.MapImageResolution;
			double singleScale = singleRenderResolution / space.MaxRange;
			double relativeImprovementPerc = ((scale - singleScale) / singleScale) * 100;

			return relativeImprovementPerc >= SuperResImprovementThresholdPerc;
		}

		// Return the final file path of the Super Res tile
		public static string GetFilePath(this SuperResTile tile)
		{
			return $"{SuperResPath}{tile.Space.EditorID}\\{tile.GetXID()}.{tile.GetYID()}.jpg";
		}

		// Filters through the super res folder and attempts to locate and remove redundant old tiles/folders or rogue files/folders
		public static async void CleanUpSuperRes()
		{
			StdOutWithColor("Checking for unnecessary tile files...", ColorInfo);
			List<Space> spaces = await CommonDatabase.GetSpaces(GetNewConnection());

			// The super res directory should have no file at root
			foreach (string file in Directory.GetFiles(SuperResPath))
			{
				StdOutWithColor($"Deleting rogue file {file}", ColorInfo);
				File.Delete(file);
			}

			foreach (string directory in Directory.GetDirectories(SuperResPath))
			{
				// The folder for each space should contain no folders
				foreach (string innerDirectory in Directory.GetDirectories(directory))
				{
					StdOutWithColor($"Deleting rogue directory {innerDirectory}", ColorInfo);
					Directory.Delete(innerDirectory, true);
				}

				// Find the space this folder represents
				Space? space = spaces.Where(space => space.EditorID == Path.GetFileName(directory)).FirstOrDefault();

				// If the space no longer exists, or, if the space no longer benefits from super res, delete its folder
				if (space == null || !space.WouldBenefitFromSuperRes())
				{
					Directory.Delete(directory, true);
					StdOutWithColor($"Deleting super res folder {directory}", ColorInfo);
					continue;
				}

				List<SuperResTile> tiles = space.GetTiles();

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
					if (!tiles.Where(tile => tile.GetXID() == tileX && tile.GetYID() == tileY).Any())
					{
						StdOutWithColor($"Deleting super res tile {tilePath}", ColorInfo);
						File.Delete(tilePath);
					}
				}
			}
		}
	}
}
