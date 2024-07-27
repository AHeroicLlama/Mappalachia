namespace MappalachiaLibrary
{
	// Provides references to files around the repo structure.
	// For use by pre-production preprocessor & build projects
	public static class BuildPaths
	{
		const string SolutionFile = "Mappalachia.sln";
		const string UtilitiesPath = @"Utilities/";
		const string SigcheckPath = UtilitiesPath + "sigcheck.exe";
		const string SqlitePath = UtilitiesPath + "sqlite3.exe";
		const string Fo76UtilsRenderPath = UtilitiesPath + @"/fo76utils/render.exe";
		const string Fo76EditOutputPath = @"FO76Edit/Output/";
		const string DatabasePath = @"/mappalachia.db";

		public const string GameDataPath = @"C:\Program Files (x86)\Steam\steamapps\common\Fallout76\Data\";
		public const string GameExePath = @"C:\Program Files (x86)\Steam\steamapps\common\Fallout76\Fallout76.exe";
		public const string ImageMagickPath = @"C:\Program Files\ImageMagick-7.1.1-Q16-HDRI\magick.exe";

		static string? solutionPath = null;

		// Returns the root of the repository, where the sln file lives
		// Value is cached in rootPath and is only calculated once per launch
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
	}
}
