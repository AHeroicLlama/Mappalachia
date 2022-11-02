using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;

namespace Mappalachia
{
	static class MapIconProcessor
	{
		const string mappalachiaRoot = @"..\\..\\..\\..\\";

		const string databasePath = mappalachiaRoot + @"Mappalachia\\data\\mappalachia.db";
		const string mapIconProcessorPath = mappalachiaRoot + @"MapIconProcessor\\";

		const string extractPath = mapIconProcessorPath + @"extract\\sprites";
		const string outputPath = mappalachiaRoot + @"\\Mappalachia\\img\\mapmarker";
		const string missingMarkersFile = outputPath + @"\\MissingMarkers.error";
		const string fileExtension = ".svg";

		static readonly Regex validIconFolder = new Regex(extractPath + @"\\DefineSprite_[0-9]{2,3}_(([A-Z].*Marker)|WhitespringResort|NukaColaQuantumPlant|TrainTrackMark)$");

		const string workshopMarker = "PublicWorkshopMarker"; // This icon needs special handling

		static void Main()
		{
			// Cleanup prior run, removing all icons (Except the special case)
			foreach (string file in Directory.GetFiles(outputPath))
			{
				if (Path.GetFileName(file) != workshopMarker + fileExtension)
				{
					File.Delete(file);
				}
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

				// Don't copy this marker because we don't want to overwrite some edits manually made to it
				if (iconName == workshopMarker)
				{
					Console.WriteLine($"\n## {workshopMarker} is required but we will not copy the file!\n");
					requiredMarkerNames[workshopMarker] = true; // Mark as checked
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
					Console.WriteLine("ERROR: File for marker " + marker.Key + " was not found anywhere in any appropriately named subfolder of the extract folder.");
					File.AppendAllText(missingMarkersFile, marker.Key + "\n");
				}
			}

			Console.WriteLine("\nFinished " + (File.Exists(missingMarkersFile) ? "with errors" : "successfully") + "\nPress any key");
			Console.ReadKey();
		}
	}
}
