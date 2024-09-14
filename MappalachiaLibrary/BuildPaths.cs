namespace MappalachiaLibrary
{
	// Provides references to files around the repo structure.
	// For use by pre-production preprocessor & build projects
	public static class BuildPaths
	{
		const string SolutionFile = "Mappalachia.sln";
		const string UtilitiesPath = @"Utilities/";
		const string OutputsPath = @"BuildOutputs/";
		const string ImageAssetPath = @"img/";
		const string SigcheckPath = UtilitiesPath + "sigcheck.exe";
		const string SqlitePath = UtilitiesPath + "sqlite3.exe";
		const string Fo76UtilsRenderPath = UtilitiesPath + @"/fo76utils/render.exe";
		const string DiscardedCellsPath = OutputsPath + @"Discarded_Cells.csv";
		const string DatabaseSummaryPath = OutputsPath + @"Database_Summary.txt";
		const string ErrorsPath = OutputsPath + @"Errors.txt";
		const string Fo76EditOutputPath = @"FO76Edit/Output/";
		const string DatabasePath = @"/data/mappalachia.db";
		const string CellPath = @"cell/";
		const string MapMarkerPath = @"mapmarker/";

		static string? solutionPath = null;

		public static string GameDataPath { get; } = @"C:\Program Files (x86)\Steam\steamapps\common\Fallout76\Data\";
		public static string GameExePath { get; } = @"C:\Program Files (x86)\Steam\steamapps\common\Fallout76\Fallout76.exe";
		public static string ImageMagickPath { get; } = @"C:\Program Files\ImageMagick-7.1.1-Q16-HDRI\magick.exe";

		// Returns the root of the repository, where the sln file lives
		// Value is cached in rootPath so is only calculated once per launch
		static string GetSolutionPath()
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

			solutionPath = path + "/";
			return solutionPath;
		}

		public static string GetSigcheckPath()
		{
			return GetSolutionPath() + SigcheckPath;
		}

		public static string GetSqlitePath()
		{
			return GetSolutionPath() + SqlitePath;
		}

		public static string GetRenderPath()
		{
			return GetSolutionPath() + Fo76UtilsRenderPath;
		}

		public static string GetFo76EditOutputPath()
		{
			return GetSolutionPath() + Fo76EditOutputPath;
		}

		public static string GetDatabasePath()
		{
			return GetSolutionPath() + DatabasePath;
		}

		public static string GetDiscardedCellsPath()
		{
			return GetSolutionPath() + DiscardedCellsPath;
		}

		public static string GetErrorsPath()
		{
			return GetSolutionPath() + ErrorsPath;
		}

		public static string GetDBSummaryPath()
		{
			return GetSolutionPath() + DatabaseSummaryPath;
		}

		public static string GetImageRootPath()
		{
			return GetSolutionPath() + ImageAssetPath;
		}

		public static string GetImageCellPath()
		{
			return GetImageRootPath() + CellPath;
		}

		public static string GetImageMapMarkerPath()
		{
			return GetImageRootPath() + MapMarkerPath;
		}
	}
}
