using System.Diagnostics;
using Library;

namespace BackgroundRenderer
{
	class BackgroundRenderer
	{
		static double MaxZoomScale { get; } = 16;

		static double MinZoomScale { get; } = 0.002;

		static void Main()
		{
			Console.Title = "Mappalachia Background Renderer";
			BuildTools.StdOutWithColor("Enter:\n1:Normal Render Mode\n2:Space Zoom/Offset correction/debug mode", BuildTools.ColorQuestion);

			switch (Console.ReadKey().KeyChar)
			{
				case '1':
					NormalRender();
					break;

				case '2':
					DebugRender();
					break;

				default:
					throw new Exception("Invalid Choice");
			}
		}

		static void NormalRender()
		{
			GetSpaceInput().ForEach(RenderSpace);
			BuildTools.StdOutWithColor($"Rendering Finished.", BuildTools.ColorInfo);
			Console.ReadKey();
		}

		static void DebugRender()
		{
			BuildTools.StdOutWithColor("\nEnter a the EditorIDs of the space for correction:", BuildTools.ColorQuestion);
			string input = Console.ReadLine() ?? string.Empty;

			// Select spaces unconditionally if none provided, otherwise select only the list of provided spaces
			string query = $"SELECT * FROM Space WHERE spaceEditorID = '{input}';";
			Space space = CommonDatabase.GetSpaces(BuildTools.GetNewConnection(), query).First();

			double resolution = Misc.MapImageResolution;
			double scale = resolution / space.MaxRange;
			double cameraX = space.CenterX;
			double cameraY = space.CenterY;
			double cameraZ = BuildTools.GetSpaceCameraHeight(space);
			string outputFile = BuildTools.TempPath + $"debug_{space.EditorID}.dds";

			string renderCommand = $"{BuildTools.Fo76UtilsRenderPath} \"{BuildTools.GameESMPath}\" {outputFile} {resolution} {resolution} " +
				$"\"{BuildTools.GameDataPath.WithoutTrailingSlash()}\" {(space.IsWorldspace ? $"-btd \"{BuildTools.GameTerrainPath}\"" : string.Empty)} " +
				$"-w 0x{space.FormID.ToHex()} -l 0 -cam {scale} 180 0 0 {cameraX} {cameraY} {cameraZ} " +
				$"-light 1.8 65 180 -rq 0 -scol 1 -ssaa 0 -ltxtres 64 -mlod 4 -xm effects";

			Console.WriteLine($"Rendering {space.EditorID} to {outputFile}");
			Process renderJob = Process.Start("CMD.exe", $"/C {renderCommand}");
			renderJob.WaitForExit();
			Misc.OpenURI(outputFile);

			BuildTools.StdOutWithColor("Using Paint.NET or similar, draw a bounding box around the correct cell contents and take a note of the Top-Left X and Y px coordinate, plus the width and height of the selected area.\nSee Developer help documentation for more info.", BuildTools.ColorInfo);

			BuildTools.StdOutWithColor("Enter Top-Left X:", BuildTools.ColorQuestion);
			int topLeftX = int.Parse(Console.ReadLine() ?? string.Empty);

			BuildTools.StdOutWithColor("Enter Top-Left Y:", BuildTools.ColorQuestion);
			int topLeftY = int.Parse(Console.ReadLine() ?? string.Empty);

			BuildTools.StdOutWithColor("Enter Width:", BuildTools.ColorQuestion);
			int width = int.Parse(Console.ReadLine() ?? string.Empty);

			BuildTools.StdOutWithColor("Enter height:", BuildTools.ColorQuestion);
			int height = int.Parse(Console.ReadLine() ?? string.Empty);

			double newRange = Math.Max(width, height);
			cameraX = cameraX - (((resolution / 2d) - (topLeftX + (width / 2d))) / scale);
			cameraY = cameraY + (((resolution / 2d) - (topLeftY + (height / 2d))) / scale);
			scale = scale * resolution / newRange; // Modified *after* it is used in X/Y offsets

			double correctRange = resolution / scale;

			string correctionPath = BuildTools.CellCorrectionPath + space.EditorID;

			BuildTools.StdOutWithColor($"Corrections:\nxCenter: {space.CenterX}->{cameraX}\nyCenter:{space.CenterY}->{cameraY}\nmaxRange: {space.MaxRange}->{correctRange}", BuildTools.ColorInfo);
			File.WriteAllText(correctionPath, $"{cameraX}\n{cameraY}\n{correctRange}");

			BuildTools.StdOutWithColor($"Correction file written to {correctionPath}.\nYou must now rebuild the database and re-render the affected cell(s).\n\nPress any key.", BuildTools.ColorInfo);
			Console.ReadKey();
		}

		static void RenderSpace(Space space)
		{

		}

		static List<Space> GetSpaceInput()
		{
			BuildTools.StdOutWithColor("Enter a space-separated list of space EditorIDs to render. Or enter nothing to render all", BuildTools.ColorQuestion);
			string input = Console.ReadLine() ?? string.Empty;

			List<string> requestedIDs = input.Trim().Split(" ").Where(space => !string.IsNullOrWhiteSpace(space)).ToList();

			// Select spaces unconditionally if none provided, otherwise select only the list of provided spaces
			string query = $"SELECT * FROM Space{(requestedIDs.Count == 0 ? string.Empty : $" WHERE spaceEditorID IN {requestedIDs.ToSqliteCollection()}")};";
			List<Space> spaces = CommonDatabase.GetSpaces(BuildTools.GetNewConnection(), query);

			// If we specified cells, ensure we found them all in the DB before proceeding
			if (requestedIDs.Count != 0 && spaces.Count != requestedIDs.Count)
			{
				throw new Exception("Inputted cells were not all found. Check for typos in EditorIDs");
			}

			return spaces;
		}
	}
}
