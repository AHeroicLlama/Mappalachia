using System.Text.RegularExpressions;

namespace Mappalachia
{
	static class MapIconProcessor
	{
		static readonly string extractPathRelative = @"..\\..\\..\\extract\\sprites";
		static readonly string outputPathRelative = @"..\\..\\..\\..\\Mappalachia\\img\\mapmarker";
		static readonly string markerListFilePath = @"..\\..\\..\\..\\Database\\requiredMarkers.txt";
		static readonly string missingMarkersFile = @outputPathRelative + "\\" + "MissingMarkers.error";
		static readonly Regex validIconFolder = new Regex(extractPathRelative + @"\\DefineSprite_[0-9]{2,3}_(([A-Z].*Marker)|WhitespringResort|NukaColaQuantumPlant|TrainTrackMark)$");
		static readonly string fileExtension = ".svg";

		static void Main()
		{
			// Cleanup prior run, removing any potentially unneeded icons
			File.Delete(missingMarkersFile);
			Directory.Delete(outputPathRelative, true);

			if (!File.Exists(markerListFilePath))
			{
				Console.WriteLine("requiredMarkers.txt not found at " + Path.GetFullPath(markerListFilePath) + ". Please build the database first to generate this file.");
				Console.WriteLine("Press any key");
				Console.ReadKey();
				return;
			}

			// Map marker names from the database, along with a bool indicating if they're accounted for
			Dictionary<string, bool> requiredMarkerNames = new Dictionary<string, bool>(File.ReadAllLines(markerListFilePath).ToDictionary(line => line, value => false));

			if (!Directory.Exists(outputPathRelative))
			{
				Directory.CreateDirectory(outputPathRelative);
			}

			Console.WriteLine("Reading raw extracts from " + Path.GetFullPath(extractPathRelative));
			Console.WriteLine("Placing outputs at " + Path.GetFullPath(outputPathRelative));

			foreach (string iconFolder in Directory.GetDirectories(extractPathRelative.Replace("\\\\", "\\")))
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
					File.Copy(iconFolder + "\\1" + fileExtension, outputPathRelative + "\\" + iconName + fileExtension, true);
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
