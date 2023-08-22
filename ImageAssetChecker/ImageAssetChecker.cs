using Microsoft.Data.Sqlite;

namespace ImageAssetChecker
{
	internal class ImageAssetChecker
	{
		const int minMapSizeKB = 100;
		const int maxMapSizeKBCell = 10000;
		const int maxMapSizeKBWorldspace = 50000;
		const int maxMarkerSizeKB = 50;
		const int expectedImageResolution = 4096;

		const string backgroundImageFileType = ".jpg";
		const string maskImageFileType = ".png";
		const string mapMarkerImageFileType = ".svg";

		const string cellDirectoryPath = "cell\\";
		const string mapMarkerDirectoryPath = "mapmarker\\";

		static readonly string thisAppPath = Directory.GetCurrentDirectory();
		static readonly string mappalachiaRoot = Path.GetFullPath(thisAppPath + "..\\..\\..\\..\\..\\");
		static readonly string databasePath = mappalachiaRoot + "Mappalachia\\data\\mappalachia.db";
		static readonly string imageDirectory = mappalachiaRoot + "Mappalachia\\img\\";
		static readonly ConsoleColor defaultConsoleColor = Console.ForegroundColor;

		static SqliteConnection connection;

		static List<Space> spaces;
		static List<string> mapMarkers;
		static List<string> errors = new List<string>();

		public static void Main()
		{
			Console.Title = "Mappalachia Image Asset Checker";

			connection = new SqliteConnection("Data Source=" + databasePath + ";Mode=ReadOnly");
			connection.Open();
			PopulateSpaces();
			PopulateMapMarkers();

			ValidateBackgroundImages();
			ValidateMapMarkers();

			if (!ValidationSuccessful())
			{
				Console.ForegroundColor = ConsoleColor.Red;

				Console.WriteLine("\nValidation Failed.\nErrors Reported:");

				foreach (string error in errors)
				{
					Console.WriteLine("* " + error);
				}

				File.AppendAllLines(imageDirectory + "errors.txt", errors);
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine($"\nValidation Passed. {spaces.Count} cells, {mapMarkers.Count} Map Markers.");
			}

			Console.ForegroundColor = defaultConsoleColor;
			Console.ReadKey();
		}

		// Populates the main List<Space> with all spaces in the database
		static void PopulateSpaces()
		{
			spaces = new List<Space>();

			SqliteCommand query = connection.CreateCommand();
			query.CommandText = "SELECT spaceEditorID, isWorldspace FROM Space_Info";
			query.Parameters.Clear();
			SqliteDataReader reader = query.ExecuteReader();

			while (reader.Read())
			{
				spaces.Add(new Space(reader.GetString(0), reader.GetInt32(1) == 1));
			}
		}

		// Populates the main List<string> with all distinct Map Markers in the database
		static void PopulateMapMarkers()
		{
			mapMarkers = new List<string>();

			SqliteCommand query = connection.CreateCommand();
			query.CommandText = "SELECT DISTINCT mapMarkerName FROM Map_Markers";
			query.Parameters.Clear();
			SqliteDataReader reader = query.ExecuteReader();

			while (reader.Read())
			{
				mapMarkers.Add(reader.GetString(0));
			}
		}

		// Return if errors were reported
		static bool ValidationSuccessful()
		{
			return errors.Count == 0;
		}

		// Log an error to the error queue
		static void ReportError(string error)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(error);
			Console.ForegroundColor = defaultConsoleColor;

			errors.Add(error);
		}

		static void ValidateBackgroundImages()
		{
			// Validate each Space in the database has a corresponding image jpg file
			foreach (Space space in spaces)
			{
				// Skip checking CharGen02-05 as they're duplicates of 01. Background renderer and main GUI account for this
				if (space.GetEditorId().StartsWith("CharGen") && space.GetEditorId() != "CharGen01")
				{
					continue;
				}

				string editorId = space.GetEditorId();
				bool isWorldSpace = space.IsWorldspace();
				string subDirectory = isWorldSpace ? string.Empty : cellDirectoryPath;
				string expectedFile = imageDirectory + subDirectory + editorId + backgroundImageFileType;

				ValidateImageFile(expectedFile, isWorldSpace);

				// Run a second validation for the rendered version of worldspace maps
				if (isWorldSpace)
				{
					expectedFile = imageDirectory + subDirectory + editorId + "_render" + backgroundImageFileType;
					ValidateImageFile(expectedFile, isWorldSpace);
				}

				// Appalachia only - extra bespoke checks
				if (space.GetEditorId() == "Appalachia")
				{
					expectedFile = imageDirectory + subDirectory + editorId + "_military" + backgroundImageFileType;
					ValidateImageFile(expectedFile, isWorldSpace);

					expectedFile = imageDirectory + subDirectory + editorId + "_waterMask" + maskImageFileType;
					ValidateImageFile(expectedFile, isWorldSpace);
				}
			}

			// Reverse of the above - validate that each file matches to an editorId (Checks we have no pointless files)
			// NB: This only checks cells
			foreach (string file in Directory.GetFiles(imageDirectory + cellDirectoryPath))
			{
				string expectedEditorId = Path.GetFileNameWithoutExtension(file);

				if (spaces.Where(s => s.GetEditorId() == expectedEditorId).Any())
				{
					Console.WriteLine($"{Path.GetFileName(file)} matches {expectedEditorId}: OK");
				}
				else
				{
					ReportError($"File {file} doesn't appear to match to any EditorID in the database. Can it be deleted? (EditorID \"{expectedEditorId}\" not present)");
				}
			}
		}

		static void ValidateImageFile(string expectedFile, bool isWorldSpace)
		{
			if (File.Exists(expectedFile))
			{
				Console.WriteLine("Image file exists " + expectedFile);
			}
			else
			{
				ReportError("Unable to find background image file " + expectedFile);
				return;
			}

			// Validate file size
			long sizeInBytes = new FileInfo(expectedFile).Length;
			long sizeInKB = sizeInBytes / 1024;
			int maxFileSizeKB = isWorldSpace ? maxMapSizeKBWorldspace : maxMapSizeKBCell;

			if (sizeInKB > minMapSizeKB && sizeInKB < maxFileSizeKB)
			{
				Console.WriteLine($"File size OK ({sizeInKB}KB)");
			}
			else
			{
				ReportError($"File {expectedFile} is of an unusual file size ({sizeInKB}KB). Please check this.");
			}

			// Validate image dimensions
			using Image image = Image.FromFile(expectedFile);
			int width = image.Width;
			int height = image.Height;

			if (width == expectedImageResolution && height == expectedImageResolution)
			{
				Console.WriteLine($"Image dimensions OK ({width}x{height})");
			}
			else
			{
				ReportError($"File {expectedFile} is not of the correct dimensions. Expected: {expectedImageResolution}x{expectedImageResolution}, actual: {width}x{height}.");
			}
		}

		static void ValidateMapMarkers()
		{
			string mapMarkerDirectory = imageDirectory + mapMarkerDirectoryPath;

			// Validate each Map Marker in the database has a corresponding marker svg file
			foreach (string mapMarker in mapMarkers)
			{
				string expectedFile = mapMarkerDirectory + mapMarker + mapMarkerImageFileType;

				// Validate file exists at all
				if (File.Exists(expectedFile))
				{
					Console.WriteLine("MapMarker file exists " + expectedFile);

					// Validate file size
					long sizeInBytes = new FileInfo(expectedFile).Length;
					long sizeInKB = sizeInBytes / 1024;

					if (sizeInKB < maxMarkerSizeKB)
					{
						Console.WriteLine($"File size OK ({sizeInKB}KB)");
					}
					else
					{
						ReportError($"File {expectedFile} is of an unusual file size ({sizeInKB}KB). Please check this.");
					}
				}
				else
				{
					ReportError("Unable to find map marker file " + expectedFile);
				}
			}

			// Reverse of the above - validate that each file matches to a requested map marker in the DB
			foreach (string file in Directory.GetFiles(mapMarkerDirectory))
			{
				string expectedMapMarker = Path.GetFileNameWithoutExtension(file);

				if (mapMarkers.Contains(expectedMapMarker))
				{
					Console.WriteLine($"{Path.GetFileName(file)} matches {expectedMapMarker}: OK");
				}
				else
				{
					ReportError($"File {file} doesn't appear to match to any Map Marker in the database. Can it be deleted? (MapMarker \"{expectedMapMarker}\" not present)");
				}
			}
		}
	}
}