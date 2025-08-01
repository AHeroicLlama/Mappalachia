using System.Drawing;
using System.Drawing.Drawing2D;
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

	static async Task Main()
	{
		Console.Title = "Mappalachia Map Icon Extractor";

		GeneratePlotIconShapes();

		List<MapMarker> mapMarkers = await CommonDatabase.GetMapMarkers(GetNewConnection(), "SELECT * FROM MapMarker GROUP BY icon ORDER BY icon ASC;");

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

	static void GeneratePlotIconShapes()
	{
		StdOutWithColor("Rendering plot icon shapes...", ColorInfo);

		Directory.CreateDirectory(IconPath);

		int size = PlotIconSize;
		Pen pen = new Pen(Color.Yellow, 20);
		int padding = (int)Math.Round(5 + pen.Width);
		Image image = new Bitmap(size, size);

		using Graphics graphics = Graphics.FromImage(image);
		graphics.SmoothingMode = SmoothingMode.AntiAlias;

		// Crosshair circle
		int circlePadding = 55;
		graphics.DrawEllipse(pen, new RectangleF(circlePadding, circlePadding, size - (circlePadding * 2), size - (circlePadding * 2)));
		graphics.DrawLine(pen, new PointF(size / 2f, 0), new PointF(size / 2f, circlePadding));
		graphics.DrawLine(pen, new PointF(size / 2f, size), new PointF(size / 2f, size - circlePadding));
		graphics.DrawLine(pen, new PointF(0, size / 2f), new PointF(circlePadding, size / 2f));
		graphics.DrawLine(pen, new PointF(size, size / 2f), new PointF(size - circlePadding, size / 2f));
		image.Save($"{IconPath}A{Common.PlotIconFileType}");

		graphics.Clear(Color.Transparent);

		// Marker triangle
		int trianglePadding = 60;
		graphics.DrawPolygon(pen, new PointF(trianglePadding, padding), new PointF(size - trianglePadding, padding), new PointF(size / 2f, (size / 2f) - (pen.Width / 2) - 2));
		image.Save($"{IconPath}B{Common.PlotIconFileType}");

		graphics.Clear(Color.Transparent);

		// Diamond
		int diamondPadding = 45;
		graphics.DrawPolygon(pen, new PointF(size / 2, diamondPadding), new PointF(size - diamondPadding, size / 2), new PointF(size / 2, size - diamondPadding), new PointF(diamondPadding, size / 2));
		image.Save($"{IconPath}C{Common.PlotIconFileType}");

		graphics.Clear(Color.Transparent);

		// X
		int xPadding = 50;
		graphics.DrawLine(pen, new PointF(xPadding, xPadding), new Point(size - xPadding, size - xPadding));
		graphics.DrawLine(pen, new PointF(xPadding, size - xPadding), new Point(size - xPadding, xPadding));
		image.Save($"{IconPath}D{Common.PlotIconFileType}");

		graphics.Clear(Color.Transparent);

		// UI-Like selector
		int selectorFract = 3;
		graphics.DrawLines(pen, new Point(size / selectorFract, padding), new PointF(padding, padding), new Point(padding, size / selectorFract));
		graphics.DrawLines(pen, new Point(size - (size / selectorFract), padding), new PointF(size - padding, padding), new Point(size - padding, size / selectorFract));
		graphics.DrawLines(pen, new Point(size / selectorFract, size - padding), new PointF(padding, size - padding), new Point(padding, size - (size / selectorFract)));
		graphics.DrawLines(pen, new Point(size - (size / selectorFract), size - padding), new PointF(size - padding, size - padding), new Point(size - padding, size - (size / selectorFract)));
		image.Save($"{IconPath}E{Common.PlotIconFileType}");

		graphics.Clear(Color.Transparent);

		// Square
		int squarePadding = 55;
		graphics.DrawPolygon(pen, new PointF(squarePadding, squarePadding), new PointF(size - squarePadding, squarePadding), new PointF(size - squarePadding, size - squarePadding), new PointF(squarePadding, size - squarePadding));
		image.Save($"{IconPath}F{Common.PlotIconFileType}");

		StdOutWithColor("Done\n", ColorInfo);
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
				string markerPath = directory + $"\\{MapMarkerIconInitialFileName}";
				string targetPath = MapMarkerPath + mapMarker.Icon + ".svg";

				XmlDocument document = new XmlDocument();
				document.Load(markerPath);

				// Run the mapmarker svg against hardcodings to apply any bespoke fixes
				document = FixMapMarkerSVG(document, mapMarker);

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

	// Recursively removes unnecessary attributes which reference FFDec, under the given node
	static void CleanXMLNode(XmlNode node)
	{
		if (node.Attributes is not null)
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
