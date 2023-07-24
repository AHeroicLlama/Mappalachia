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
		static readonly string commonwealthCartographyRoot = Path.GetFullPath(thisAppPath + "..\\..\\..\\..\\..\\");
		static readonly string databasePath = commonwealthCartographyRoot + "CommonwealthCartography\\data\\commonwealth_cartography.db";
		static readonly string imageDirectory = commonwealthCartographyRoot + "CommonwealthCartography\\img\\";

		static SqliteConnection connection;

		static List<Space> spaces;
		static List<string> mapMarkers;

		public static void Main()
		{
			Console.Title = "Commonwealth Cartography Image Asset Checker";

			connection = new SqliteConnection("Data Source=" + databasePath + ";Mode=ReadOnly");
			connection.Open();
			PopulateSpaces();
			PopulateMapMarkers();

			ValidateBackgroundImages();
			ValidateMapMarkers();

			Console.WriteLine($"\nValidation finished. {spaces.Count} cells, {mapMarkers.Count} Map Markers.");
			Console.ReadKey();
		}

		// Populates the main List<Space> with all spaces in the database
		static void PopulateSpaces()
		{
			if (spaces == null)
			{
				spaces = new List<Space>();
			}
			else
			{
				return;
			}

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
			if (mapMarkers == null)
			{
				mapMarkers = new List<string>();
			}
			else
			{
				return;
			}

			SqliteCommand query = connection.CreateCommand();
			query.CommandText = "SELECT DISTINCT mapMarkerName FROM Map_Markers";
			query.Parameters.Clear();
			SqliteDataReader reader = query.ExecuteReader();

			while (reader.Read())
			{
				mapMarkers.Add(reader.GetString(0));
			}
		}

		static void ValidateBackgroundImages()
		{
			// Validate each Space in the database has a corresponding image jpg file
			foreach (Space space in spaces)
			{
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

				// Commonwealth only - extra bespoke checks
				if (space.GetEditorId() == "Commonwealth")
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
					throw new Exception($"File {file} doesn't appear to match to any EditorID in the database. Can it be deleted? (EditorID \"{expectedEditorId}\" not present)");
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
				throw new FileNotFoundException("Unable to find background image file " + expectedFile);
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
				throw new Exception($"File {expectedFile} is of an unusual file size ({sizeInKB}KB). Please check this.");
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
				throw new Exception($"File {expectedFile} is not of the correct dimensions. Expected: {expectedImageResolution}x{expectedImageResolution}, actual: {width}x{height}.");
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
						throw new Exception($"File {expectedFile} is of an unusual file size ({sizeInKB}KB). Please check this.");
					}
				}
				else
				{
					throw new FileNotFoundException("Unable to find map marker file " + expectedFile);
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
					throw new Exception($"File {file} doesn't appear to match to any Map Marker in the database. Can it be deleted? (MapMarker \"{expectedMapMarker}\" not present)");
				}
			}
		}
	}
}