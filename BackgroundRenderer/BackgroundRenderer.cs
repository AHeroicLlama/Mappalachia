using System.Diagnostics;
using Library;

namespace BackgroundRenderer
{
	class BackgroundRenderer
	{
		static int JpegQualityCell { get; } = 85;

		static int JpegQualityWorldspace { get; } = 100;

		static int WorldspaceResolution { get; } = (int)Math.Pow(2, 14); // 16k

		static void Main()
		{
			Console.Title = "Mappalachia Background Renderer";

			if (!Directory.Exists(BuildTools.CellPath))
			{
				Directory.CreateDirectory(BuildTools.CellPath);
			}

			if (!Directory.Exists(BuildTools.WorldPath))
			{
				Directory.CreateDirectory(BuildTools.WorldPath);
			}

			BuildTools.StdOutWithColor("Enter:\n1: Normal Render Mode\n2: Space Zoom/Offset (X/Y) correction/debug mode\n3: Space height (Z) crop correction/debug mode", BuildTools.ColorQuestion);

			switch (Console.ReadKey().KeyChar)
			{
				case '1':
					NormalRender();
					break;

				case '2':
					SpaceZoomOffsetCorrection();
					break;

				case '3':
					SpaceHeightCorrection();
					break;

				default:
					throw new Exception("Invalid Choice");
			}
		}

		static void NormalRender()
		{
			List<Space> spaces = GetSpaceInput();

			BuildTools.StdOutWithColor($"Rendering {spaces.Count} spaces...", BuildTools.ColorInfo);

			Stopwatch stopwatch = Stopwatch.StartNew();
			int i = 0;

			foreach (Space space in spaces)
			{
				i++;
				BuildTools.StdOutWithColor($"\nRendering {space.EditorID} (0x{space.FormID.ToHex()}). {i} of {spaces.Count}", BuildTools.ColorInfo);

				RenderSpace(space);

				if (i != spaces.Count)
				{
					TimeSpan avgTimePerSpace = stopwatch.Elapsed / i;
					TimeSpan estTimeRemaining = avgTimePerSpace * (spaces.Count - i);
					Console.WriteLine($"Est. time remaining: {estTimeRemaining} ({((100d * i) / spaces.Count).ToString("0.00")}% complete)");
				}
			}

			BuildTools.StdOutWithColor($"\nRendering Finished. Press Any Key.", BuildTools.ColorInfo);
			Console.ReadKey();
		}

		// Runs through the process of rendering a cell, opening it, asking the user for corrective input, then storing that as a file
		static void SpaceZoomOffsetCorrection()
		{
			Space space = GetSingleSpaceInput();

			double resolution = Misc.MapImageResolution;
			double scale = resolution / space.MaxRange;
			double cameraX = space.CenterX;
			double cameraY = space.CenterY;
			double cameraZ = GetSpaceCameraHeight(space);
			string outputFile = BuildTools.TempPath + $"debug_{space.EditorID}.dds";

			string renderCommand = $"{BuildTools.Fo76UtilsRenderPath} \"{BuildTools.GameESMPath}\" {outputFile} {resolution} {resolution} " +
				$"\"{BuildTools.GameDataPath.WithoutTrailingSlash()}\" {(space.IsWorldspace ? $"-btd \"{BuildTools.GameTerrainPath}\"" : string.Empty)} " +
				$"-w 0x{space.FormID.ToHex()} -l 0 -cam {scale} 180 0 0 {cameraX} {cameraY} {cameraZ} " +
				$"-light 1.8 65 180 -rq 0 -scol 1 -ssaa 0 -ltxtres 64 -mlod 4 -xm effects";

			Console.WriteLine($"Rendering {space.EditorID} to {outputFile}");
			Process renderJob = Process.Start("CMD.exe", $"/C {renderCommand}");
			renderJob.WaitForExit();
			Misc.OpenURI(outputFile);

			BuildTools.StdOutWithColor("Using Paint.NET or similar, draw a bounding box around the correct cell contents and take a note of the Top-Left X and Y pixel coordinate, plus the width and height of the selected area.\nSee Developer help documentation for more info.", BuildTools.ColorInfo);

			BuildTools.StdOutWithColor("Enter Top-Left X:", BuildTools.ColorQuestion);
			int topLeftX = int.Parse(Console.ReadLine() ?? string.Empty);

			BuildTools.StdOutWithColor("Enter Top-Left Y:", BuildTools.ColorQuestion);
			int topLeftY = int.Parse(Console.ReadLine() ?? string.Empty);

			BuildTools.StdOutWithColor("Enter Width:", BuildTools.ColorQuestion);
			int width = int.Parse(Console.ReadLine() ?? string.Empty);

			BuildTools.StdOutWithColor("Enter height:", BuildTools.ColorQuestion);
			int height = int.Parse(Console.ReadLine() ?? string.Empty);

			// Get the corrected range as a proportion of the selected space against the ratio used initially
			double correctedRange = Math.Max(width, height) / scale;

			// Find the px coord of the center of the user-drawn bounding box
			double boundingBoxCenterX = topLeftX + (width / 2d);
			double boundingBoxCenterY = topLeftY + (height / 2d);

			// Find the necessary correction in pixels
			double correctionXPx = (resolution / 2d) - boundingBoxCenterX;
			double correctionYPx = (resolution / 2d) - boundingBoxCenterY;

			// Translate the pixel correction to game coordinates
			double actualCorrectionX = correctionXPx / scale;
			double actualCorrectionY = correctionYPx / scale;

			// Get the new space centers once correction is applied
			double correctedXCenter = cameraX - actualCorrectionX;
			double correctedYCenter = cameraY + actualCorrectionY;

			string correctionPath = BuildTools.CellXYScaleCorrectionPath + space.EditorID;

			BuildTools.StdOutWithColor($"Corrections:\nxCenter: {space.CenterX}->{correctedXCenter}\nyCenter:{space.CenterY}->{correctedYCenter}\nmaxRange: {space.MaxRange}->{correctedRange}", BuildTools.ColorInfo);
			File.WriteAllText(correctionPath, $"{cameraX}\n{cameraY}\n{correctedRange}");

			BuildTools.StdOutWithColor($"Correction file written to {correctionPath}.\nYou must now rebuild the database and re-render the affected cell(s).\n\nPress any key.", BuildTools.ColorInfo);
			Console.ReadKey();
		}

		// Handles the process of manually finding the correct height crop for a space render
		static void SpaceHeightCorrection()
		{
			Space space = GetSingleSpaceInput();
			string outputFile = BuildTools.TempPath + $"debug_{space.EditorID}.dds";

			BuildTools.StdOutWithColor("Enter an estimate of the correct height for the cell, eg 1000:", BuildTools.ColorQuestion);
			int height = int.Parse(Console.ReadLine() ?? "1000");

			// Loop of user estimates new height, render, ask again.
			while (true)
			{
				string renderCommand = $"{BuildTools.Fo76UtilsRenderPath} \"{BuildTools.GameESMPath}\" {outputFile} {Misc.MapImageResolution} {Misc.MapImageResolution} " +
					$"\"{BuildTools.GameDataPath.WithoutTrailingSlash()}\" {(space.IsWorldspace ? $"-btd \"{BuildTools.GameTerrainPath}\"" : string.Empty)} " +
					$"-w 0x{space.FormID.ToHex()} -l 0 -cam {Misc.MapImageResolution / (double)space.MaxRange} 180 0 0 {space.CenterX} {space.CenterY} {height} " +
					$"-light 1.8 65 180 -rq 0 -scol 1 -ssaa 0 -ltxtres 64 -mlod 4 -xm effects";

				Console.WriteLine($"Rendering {space.EditorID} to {outputFile}");
				Process renderJob = Process.Start("CMD.exe", $"/C {renderCommand}");
				renderJob.WaitForExit();
				Misc.OpenURI(outputFile);

				BuildTools.StdOutWithColor("Was the height correct? Enter \"y\" to save, otherwise enter a new height to try again:", BuildTools.ColorQuestion);
				string input = Console.ReadLine() ?? "1000";

				// The correct crop has been found - save it to a file, do the proper render, and finish.
				if (input.Equals("y", StringComparison.OrdinalIgnoreCase))
				{
					string correctionPath = BuildTools.CellZCorrectionPath + space.EditorID;

					File.WriteAllText(correctionPath, height.ToString());
					BuildTools.StdOutWithColor($"Correction file written to {correctionPath}.\nPress any key to re-render the corrected cell properly.", BuildTools.ColorInfo);
					Console.ReadKey();

					RenderSpace(space);
					BuildTools.StdOutWithColor($"Done. Press Any Key.", BuildTools.ColorInfo);
					Console.ReadKey();

					return;
				}

				height = int.Parse(input);
			}
		}

		// Renders a space in the normal way, and converts and moves it appropriately.
		// Worldspaces also see the watermask generated.
		static void RenderSpace(Space space)
		{
			int renderResolution = space.IsWorldspace ? WorldspaceResolution : Misc.MapImageResolution;
			string ddsFile = BuildTools.TempPath + $"{space.EditorID}_render.dds";
			string finalFile = (space.IsWorldspace ? BuildTools.WorldPath : BuildTools.CellPath) + space.EditorID + ".jpg";
			string terrainString = space.IsWorldspace ? $"-btd \"{BuildTools.GameTerrainPath}\" " : string.Empty;
			double scale = renderResolution / (double)space.MaxRange;

			// -rq 1 + 2 + 12 + 256 (+32 for cells)
			string renderCommand = $"{BuildTools.Fo76UtilsRenderPath} \"{BuildTools.GameESMPath}\" {ddsFile} {renderResolution} {renderResolution} " +
				$"\"{BuildTools.GameDataPath.WithoutTrailingSlash()}\" {terrainString}" +
				$"-w 0x{space.FormID.ToHex()} -l 0 -cam {scale} 180 0 0 {space.CenterX} {space.CenterY} {GetSpaceCameraHeight(space)} " +
				$"-light 1.8 65 180 -lcolor 1.1 0xD6CCC7 0.9 -1 -1 -rq {(space.IsWorldspace ? "271" : "303")} -ssaa 2 " +
				$"-ltxtres 512 -mip 1 -lmip 2 -mlod 0 -ndis 1 " +
				$"-xm " + string.Join(" -xm ", BuildTools.RenderExcludeModels);

			string resizeCommand = $"\"{BuildTools.ImageMagickPath}\" {ddsFile} -resize {Misc.MapImageResolution}x{Misc.MapImageResolution} " +
						$"-quality {(space.IsWorldspace ? JpegQualityWorldspace : JpegQualityCell)} JPEG:{finalFile}";

			Process render = Process.Start("CMD.exe", "/C " + renderCommand);
			render.WaitForExit();

			BuildTools.StdOutWithColor($"Converting and downsampling with ImageMagick...", BuildTools.ColorInfo);
			Process magickResizeConvert = Process.Start("CMD.exe", "/C " + resizeCommand);
			magickResizeConvert.WaitForExit();

			// Do the watermask
			if (space.IsWorldspace)
			{
				BuildTools.StdOutWithColor("Rendering Watermask...", BuildTools.ColorInfo);

				string watermaskDDS = BuildTools.TempPath + space.EditorID + "_waterMask.dds";
				string watermaskFinalFile = BuildTools.WorldPath + space.EditorID + "_waterMask.png";

				string waterMaskRenderCommand = $"{BuildTools.Fo76UtilsRenderPath} \"{BuildTools.GameESMPath}\" {watermaskDDS} {renderResolution} {renderResolution} " +
					$"\"{BuildTools.GameDataPath.WithoutTrailingSlash()}\" {terrainString} -w 0x{space.FormID.ToHex()} -l 0 -cam {scale} 180 0 0 {space.CenterX} {space.CenterY} {GetSpaceCameraHeight(space)} " +
					$"-light 1 0 0 -ssaa 2 -watermask 1 -xm water " +
					$"-xm " + string.Join(" -xm ", BuildTools.RenderExcludeModels);

				string watermaskResizeCommand = $"\"{BuildTools.ImageMagickPath}\" {watermaskDDS} -fill #0000FF -fuzz 25% +opaque #000000 -transparent #000000 -resize {Misc.MapImageResolution}x{Misc.MapImageResolution} PNG:{watermaskFinalFile}";

				Process watermaskRender = Process.Start("CMD.exe", "/C " + waterMaskRenderCommand);
				watermaskRender.WaitForExit();

				BuildTools.StdOutWithColor($"Converting and downsampling with ImageMagick...", BuildTools.ColorInfo);
				Process magickWatermaskResizeConvert = Process.Start("CMD.exe", "/C " + watermaskResizeCommand);
				magickWatermaskResizeConvert.WaitForExit();
			}
		}

		// Returns a list of Spaces gathered from the user, for rendering
		static List<Space> GetSpaceInput()
		{
			BuildTools.StdOutWithColor("\nEnter a space-separated list of space EditorIDs to render. Or enter nothing to render all", BuildTools.ColorQuestion);
			string input = Console.ReadLine() ?? string.Empty;

			List<string> requestedIDs = input.Trim().Split(" ").Where(space => !string.IsNullOrWhiteSpace(space)).ToList();

			// Select spaces unconditionally if none provided, otherwise select only the list of provided spaces
			string query = $"SELECT * FROM Space{(requestedIDs.Count == 0 ? string.Empty : $" WHERE spaceEditorID IN {requestedIDs.ToSqliteCollection()}")} ORDER BY isWorldspace DESC, spaceEditorID ASC;";
			List<Space> spaces = CommonDatabase.GetSpaces(BuildTools.GetNewConnection(), query);

			// If we specified cells, ensure we found them all in the DB before proceeding
			if (requestedIDs.Count != 0 && spaces.Count != requestedIDs.Count)
			{
				throw new Exception("Inputted cells were not all found. Check for typos in EditorIDs");
			}

			return spaces;
		}

		// Asks for a single editorID and returns the corresponding space
		static Space GetSingleSpaceInput()
		{
			BuildTools.StdOutWithColor("\nEnter the EditorID of the space:", BuildTools.ColorQuestion);
			string input = Console.ReadLine() ?? string.Empty;

			string query = $"SELECT * FROM Space WHERE spaceEditorID = '{input}';";
			return CommonDatabase.GetSpaces(BuildTools.GetNewConnection(), query).First();
		}

		// Returns the predetermined cropped height for a cell, otherwise max height
		public static double GetSpaceCameraHeight(Space space)
		{
			string cropFile = BuildTools.CellZCorrectionPath + space.EditorID;

			if (File.Exists(cropFile))
			{
				return int.Parse(File.ReadAllText(cropFile));
			}
			else
			{
				return (int)Math.Pow(2, 16);
			}
		}
	}
}
