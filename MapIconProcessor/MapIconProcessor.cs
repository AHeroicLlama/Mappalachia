using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;

namespace CommonwealthCartography
{
	static class MapIconProcessor
	{
		const string commonwealthCartographyRoot = @"..\\..\\..\\..\\";

		const string databasePath = commonwealthCartographyRoot + @"CommonwealthCartography\\data\\commonwealth_cartography.db";
		const string mapIconProcessorPath = commonwealthCartographyRoot + @"MapIconProcessor\\";

		const string extractPath = mapIconProcessorPath + @"extract\\sprites";
		const string outputPath = commonwealthCartographyRoot + @"\\CommonwealthCartography\\img\\mapmarker";
		const string missingMarkersFile = outputPath + @"\\MissingMarkers.error";
		const string fileExtension = ".svg";

		static readonly Regex validIconFolder = new Regex(extractPath + @"\\DefineSprite_[0-9]{1,3}_([A-Z].*Marker)$");

		static void Main()
		{
			// Cleanup prior run, removing all icons (Except the special case)
			foreach (string file in Directory.GetFiles(outputPath))
			{
				File.Delete(file);
			}

			List<string> mapMarkers = new List<string>();

			SqliteConnection connection = new SqliteConnection("Data Source=" + databasePath + ";Mode=ReadOnly");
			connection.Open();

			SqliteCommand query = connection.CreateCommand();
			query.CommandText = "SELECT DISTINCT mapMarkerName FROM Map_Markers";
			query.Parameters.Clear();
			SqliteDataReader reader = query.ExecuteReader();

			while (reader.Read())
			{
				mapMarkers.Add(reader.GetString(0));
				Console.WriteLine(reader.GetString(0));
			}

			// Map marker names from the database, along with a bool indicating if they're accounted for
			Dictionary<string, bool> requiredMarkerNames = new Dictionary<string, bool>(mapMarkers.ToDictionary(line => line, value => false));

			if (!Directory.Exists(outputPath))
			{
				Directory.CreateDirectory(outputPath);
			}

			Console.WriteLine("Reading raw extracts from " + Path.GetFullPath(extractPath));
			Console.WriteLine("Placing outputs at " + Path.GetFullPath(outputPath));

			foreach (string iconFolder in Directory.GetDirectories(extractPath.Replace("\\\\", "\\")))
			{
				Match match = validIconFolder.Match(iconFolder);

				// A folder which doesn't even look like an icon at all - skip entirely
				if (!match.Success)
				{
					continue;
				}

				string iconName = match.Groups[1].Captures[0].ToString();

				// This is a marker but we don't need it, so also skip
				if (!requiredMarkerNames.ContainsKey(iconName))
				{
					Console.WriteLine("Skipping " + iconName);
					continue;
				}

				// Looks like we want this icon - copy and rename appropriately
				try
				{
					Console.WriteLine("Copying " + iconName);
					File.Copy(iconFolder + "\\1" + fileExtension, outputPath + "\\" + iconName + fileExtension, true);
					requiredMarkerNames[iconName] = true;
				}
				catch (FileNotFoundException)
				{
					// Mark it as missing so it is handled later
					requiredMarkerNames[iconName] = false;
				}
			}

			// Verify each marker in the database was accounted for
			foreach (KeyValuePair<string, bool> marker in requiredMarkerNames)
			{
				if (marker.Value == false)
				{
					Console.WriteLine("ERROR: File for marker " + marker.Key + " was not found anywhere in any appropriately named subfolder of the extract folder.\n");
					File.AppendAllText(missingMarkersFile, marker.Key + "\n");
				}
			}

			Console.WriteLine("\nFinished " + (File.Exists(missingMarkersFile) ? "with errors" : "successfully") + "\nPress any key");
			Console.ReadKey();
		}
	}
}
