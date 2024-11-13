using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Library;
using static Library.BuildTools;

namespace MapIconProcessor;

class MapIconProcessor
{
	static string[] Directories { get; } = Directory.GetDirectories(MapIconExtractPath + "sprites");

	static List<string> Errors { get; } = new List<string>();

	static void Main()
	{
		Console.Title = "Mappalachia Map Icon Extractor";

		List<MapMarker> mapMarkers = CommonDatabase.GetMapMarkers(GetNewConnection());

		foreach (MapMarker mapMarker in mapMarkers)
		{
			if (ExtractIcon(mapMarker))
			{
				continue;
			}

			// We exhausted all the folders without finding the right icon, log error but continue
			string error = $"Failed to find suitable icon SVG for marker icon {mapMarker.Icon}";
			Errors.Add(error);
		}

		foreach (string error in Errors)
		{
			StdOutWithColor(error, ColorError);
			AppendToErrorLog(error);
		}

		StdOutWithColor("Done. Press any key.", ColorInfo);
		Console.ReadKey();
	}

	// Searches for and copies out the map marker icon SVG for the given MapMarker
	// Returns if search was successful
	static bool ExtractIcon(MapMarker mapMarker)
	{
		foreach (string directory in Directories)
		{
			Match match = ValidIconFolder.Match(directory);

			// This appears to be the directory containing this map marker icon - read it in, clean it, and write it back out
			if (match.Success && match.Groups[1].Value == mapMarker.Icon)
			{
				string markerPath = directory + @"\1.svg";
				string targetPath = MapMarkerPath + mapMarker.Icon + ".svg";

				XmlDocument document = new XmlDocument();
				document.Load(markerPath);

				foreach (XmlNode node in document)
				{
					CleanXMLNode(node);
				}

				XmlWriterSettings settings = new XmlWriterSettings()
				{
					Indent = true,
					IndentChars = "\t",
					OmitXmlDeclaration = true,
					NamespaceHandling = NamespaceHandling.OmitDuplicates,
				};

				StringWriter stringWriter = new StringWriter(new StringBuilder());
				XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings);
				document.Save(xmlWriter);

				File.WriteAllText(targetPath, stringWriter.ToString());

				Console.WriteLine($"{mapMarker.Icon}: Success");
				return true;
			}
		}

		return false;
	}

	// Recursively removes unncessary attributes which reference FFDec, under the given node
	static void CleanXMLNode(XmlNode node)
	{
		if (node.Attributes != null)
		{
			IEnumerable<XmlAttribute> attributes = node.Attributes.Cast<XmlAttribute>().Reverse();

			foreach (XmlAttribute attribute in attributes)
			{
				if (attribute.Name.Contains("ffdec", StringComparison.OrdinalIgnoreCase))
				{
					node.Attributes.RemoveNamedItem(attribute.Name);
				}
			}
		}

		foreach (XmlNode childNode in node.ChildNodes)
		{
			CleanXMLNode(childNode);
		}
	}
}
