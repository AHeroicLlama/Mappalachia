using System.Text.RegularExpressions;

namespace Mappalachia
{
	static class MapIconProcessor
	{
		static readonly string extractPathRelative = @"..\\..\\..\\extract\\sprites";
		static readonly string outputPathRelative = @"..\\..\\..\\..\\Mappalachia\\img\\mapmarker";
		static readonly string markerListFilePath = @"..\\..\\..\\..\\Database\\requiredMarkers.txt";
		static readonly Regex validIconFolder = new Regex(extractPathRelative + @"\\DefineSprite_[0-9]{2,3}_(([A-Z].*Marker)|WhitespringResort|NukaColaQuantumPlant|TrainTrackMark)$");
		static readonly string fileExtension = ".svg";

		static void Main()
		{
			if (!File.Exists(markerListFilePath))
            {
				Console.WriteLine("requiredMarkers.txt not found at " + Path.GetFullPath(markerListFilePath) + ". Please build the database first to generate this file.");
				Console.WriteLine("Press any key");
				Console.ReadKey();
				return;
			}

			List<string> requiredMarkerNames = new List<string>(File.ReadAllLines(markerListFilePath));

			if (!Directory.Exists(outputPathRelative))
            {
				Directory.CreateDirectory(outputPathRelative);
            }

			Console.WriteLine("Reading raw extracts from " + Path.GetFullPath(extractPathRelative));
			Console.WriteLine("Placing outputs at " + Path.GetFullPath(outputPathRelative));

			foreach (string iconFolder in Directory.GetDirectories(extractPathRelative.Replace("\\\\", "\\")))
            {
				Match match = validIconFolder.Match(iconFolder);

				if (!match.Success)
                {
					continue;
				}

				string iconName = match.Groups[1].Captures[0].ToString();

				if (!requiredMarkerNames.Contains(iconName))
                {
					Console.WriteLine("Skipping " + iconName);
					continue;
                }

				Console.WriteLine("Copying " + iconName);
				File.Copy(iconFolder + "\\1" + fileExtension, outputPathRelative + "\\" + iconName + fileExtension, true);
			}

			Console.WriteLine("Press any key");
			Console.ReadKey();
		}
	}
}
