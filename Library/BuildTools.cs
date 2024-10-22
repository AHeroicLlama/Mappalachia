﻿using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.Data.Sqlite;

namespace Library
{
	// Provides references to files around the repo structure and other build-only helpers.
	// Intended for use only by pre-production preprocessor & build projects.
	public static partial class BuildTools
	{
		const string SolutionFile = "Mappalachia.sln";
		const string AssetsPath = @"Assets\";
		const string UtilitiesPath = @"Utilities\";
		const string OutputsPath = @"BuildOutputs\";
		const string ImageAssetPath = AssetsPath + @"img\";

		public static string SqlitePath { get; } = GetSolutionPath() + UtilitiesPath + "sqlite3.exe"; // https://www.sqlite.org/download.html

		public static string Fo76UtilsRenderPath { get; } = GetSolutionPath() + UtilitiesPath + @"fo76utils\render.exe"; // https://github.com/fo76utils/fo76utils

		public static string DiscardedCellsPath { get; } = GetSolutionPath() + OutputsPath + @"Discarded_Cells.csv";

		public static string DatabaseSummaryPath { get; } = GetSolutionPath() + OutputsPath + @"Database_Summary.txt";

		public static string ErrorsPath { get; } = GetSolutionPath() + OutputsPath + @"Errors.txt";

		public static string Fo76EditOutputPath { get; } = GetSolutionPath() + @"FO76Edit\Output\";

		public static string DatabasePath { get; } = GetSolutionPath() + AssetsPath + @"data\mappalachia.db";

		public static string ImageRootPath { get; } = GetSolutionPath() + ImageAssetPath;

		public static string CellPath { get; } = ImageRootPath + @"cell\";

		public static string WorldPath { get; } = ImageRootPath + @"wrld\";

		public static string MapMarkerPath { get; } = ImageRootPath + @"mapmarker\";

		static string GamePath { get; } = @"C:\Program Files (x86)\Steam\steamapps\common\Fallout76\";

		public static string GameDataPath { get; } = @$"{GamePath}Data\";

		public static string GameESMPath { get; } = @$"{GameDataPath}SeventySix.esm";

		public static string GameTerrainPath { get; } = @$"{GameDataPath}Terrain\Appalachia.btd";

		public static string GameExePath { get; } = $"{GamePath}Fallout76.exe";

		public static string ImageMagickPath { get; } = @"C:\Program Files\ImageMagick-7.1.1-Q16-HDRI\magick.exe";

		public static ConsoleColor InfoColor { get; } = ConsoleColor.DarkYellow;

		public static ConsoleColor QuestionColor { get; } = ConsoleColor.Blue;

		static string? solutionPath = null;

		public static SqliteConnection GetNewConnection()
		{
			SqliteConnection connection = new SqliteConnection($"Data Source={DatabasePath}; Pooling=false");
			connection.Open();
			return connection;
		}

		// Returns the root of the repository, where the sln file lives
		// Value is cached in solutionPath so is only calculated once per launch
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

			solutionPath = path + @"\";
			return solutionPath;
		}

		// Returns the MD5 Hash of the given file
		public static string GetMD5Hash(string filePath)
		{
			using FileStream fileStream = File.OpenRead(filePath);

			byte[] hash = MD5.Create().ComputeHash(fileStream);
			return BitConverter.ToString(hash).Replace("-", string.Empty);
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
	}
}
