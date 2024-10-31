using Library;

namespace MapIconProcessor;

class MapIconProcessor
{
	static void Main()
	{
		Console.Title = "Mappalachia Map Icon Extractor";

		List<MapMarker> mapMarkers = CommonDatabase.GetMapMarkers(BuildTools.GetNewConnection(), "SELECT * FROM MapMarker GROUP BY icon ORDER BY icon ASC;");

		foreach (string directory in Directory.GetDirectories(BuildTools.MapIconExtractPath + "sprites"))
		{
			string expectedIconName = BuildTools.ValidIconFolder.Match(directory).Groups[1].Value;

			Console.WriteLine($"Matched {directory} as {expectedIconName}");

			// If the icon name implied by the folder, matches an icon in the database
			if (mapMarkers.Where(mapMarker => mapMarker.Icon == expectedIconName).Any())
			{
				string markerPath = directory + @"\1.svg";
				string targetPath = BuildTools.MapMarkerPath + expectedIconName + ".svg";

				File.Copy(markerPath, targetPath, true);

				Console.WriteLine($"Copied {markerPath} to {targetPath}");
			}
		}

		BuildTools.StdOutWithColor("Done. Press any key.", BuildTools.ColorInfo);
		Console.ReadKey();
	}
}
