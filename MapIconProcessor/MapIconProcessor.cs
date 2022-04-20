using System.Text.RegularExpressions;

namespace Mappalachia
{
	static class MapIconProcessor
	{
		static readonly string extractPathRelative = @"..\\..\\..\\extract\\sprites";
		static readonly string outputPathRelative = @"..\\..\\..\\..\\Mappalachia\\img\\mapmarker";
		static readonly Regex validIconFolder = new Regex(extractPathRelative + @"\\DefineSprite_[0-9]{2,3}_(([A-Z].*Marker)|WhitespringResort|NukaColaQuantumPlant|TrainTrackMark)$");

		static void Main()
		{
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
				File.Copy(iconFolder + "\\1.png", outputPathRelative + "\\" + iconName + ".png", true);
			}

			Console.WriteLine("\nDone! Press any key");
			Console.ReadKey();
		}
	}
}
