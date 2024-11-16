using System.Diagnostics;
using Library;
using static Library.BuildTools;

namespace BackgroundRenderer
{
	class BackgroundRenderer
	{
		static int JpegQualityCell { get; } = 85;

		static int JpegQualityWorldspace { get; } = 100;

		static int WorldspaceRenderResolution { get; } = (int)Math.Pow(2, 14); // 16k

		static int CellRenderParallelization { get; } = 8; // Max cells to render in parallel

		static void Main()
		{
			Console.Title = "Mappalachia Background Renderer";

			if (!Directory.Exists(CellPath))
			{
				Directory.CreateDirectory(CellPath);
			}

			if (!Directory.Exists(WorldPath))
			{
				Directory.CreateDirectory(WorldPath);
			}

			StdOutWithColor("Enter:\n1: Normal Render Mode\n2: Space Zoom/Offset (X/Y) correction/debug mode\n3: Space height (Z) crop correction/debug mode", ColorQuestion);

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

		// Performs the typical end-product render for the given spaces, or if null, asks the user for which
		// Provides informative logging output, est time remaining etc
		static void NormalRender(List<Space>? spaces = null)
		{
			spaces ??= GetSpaceInput();

			Stopwatch generalStopwatch = Stopwatch.StartNew();

			// Break the spaces into cells and worldspaces
			List<Space> cells = spaces.Where(space => !space.IsWorldspace).ToList();
			List<Space> worldspaces = spaces.Where(space => space.IsWorldspace).ToList();

			StdOutWithColor($"Rendering {worldspaces.Count} worldspace{Common.Pluralize(worldspaces)} and {cells.Count} cell{Common.Pluralize(cells)}...", ColorInfo);

			int spacesRendered = 0;

			// Do the serial render of worldspaces first
			foreach (Space worldspace in worldspaces)
			{
				StdOutWithColor($"\nRendering {worldspace.EditorID} (0x{worldspace.FormID})", ColorInfo);
				RenderSpace(worldspace);
				AnnounceRenderProgress(spaces, worldspace, ref spacesRendered);
			}

			Stopwatch cellStopwatch = Stopwatch.StartNew();

			// Render cells in parallel
			// We track the time and provide estimates of time remaining
			Parallel.ForEach(cells, new ParallelOptions() { MaxDegreeOfParallelism = CellRenderParallelization }, cell =>
			{
				RenderSpace(cell, true);
				AnnounceRenderProgress(spaces, cell, ref spacesRendered, cellStopwatch);
			});

			StdOutWithColor($"\nRendering Finished. {generalStopwatch.Elapsed.ToString(@"hh\:mm\:ss")}. Press Any Key.", ColorInfo);
			Console.ReadKey();
		}

		static void AnnounceRenderProgress(List<Space> spaces, Space space, ref int cellsRendered, Stopwatch? stopwatch = null)
		{
			int count = Interlocked.Increment(ref cellsRendered);

			double percentRemaining = (100d * count) / spaces.Count;
			StdOutWithColor($"{space.EditorID} (0x{space.FormID.ToHex()}) Done. ({count}/{spaces.Count} ({percentRemaining.ToString("0.00")}%))", ColorInfo);

			// If this isn't the last item, and we have a timer going
			if (count != spaces.Count && stopwatch != null)
			{
				TimeSpan avgTimePerSpace = stopwatch.Elapsed / count;
				TimeSpan estTimeRemaining = avgTimePerSpace * (spaces.Count - count);
				StdOutWithColor($"{estTimeRemaining.ToString(@"hh\:mm\:ss")} remaining", ColorInfo);
			}
		}

		// Runs through the process of rendering a cell, opening it, asking the user for corrective input, then storing that as a file
		static void SpaceZoomOffsetCorrection()
		{
			while (true)
			{
				Space space = GetSingleSpaceInput();

				double resolution = Common.MapImageResolution;
				double scale = resolution / space.MaxRange;
				double cameraX = space.CenterX;
				double cameraY = space.CenterY;
				int cameraZ = GetSpaceCameraHeight(space);
				string outputFile = TempPath + $"debug_{space.EditorID}.dds";

				string renderCommand = $"{Fo76UtilsRenderPath} \"{GameESMPath}\" {outputFile} {resolution} {resolution} " +
					$"\"{GameDataPath.WithoutTrailingSlash()}\" {(space.IsWorldspace ? $"-btd \"{GameTerrainPath}\"" : string.Empty)} " +
					$"-w 0x{space.FormID.ToHex()} -l 0 -cam {scale} 180 0 0 {cameraX} {cameraY} {cameraZ} " +
					$"-light 1.8 65 180 -rq 0 -scol 1 -ssaa 0 -ltxtres 64 -mlod 4 -xm effects";

				Console.WriteLine($"Rendering {space.EditorID} to {outputFile}");
				Process renderJob = StartProcess(renderCommand);
				renderJob.WaitForExit();
				Common.OpenURI(outputFile);

				StdOutWithColor("Using Paint.NET or similar, draw a bounding box around the correct cell contents and take a note of the Top-Left X and Y pixel coordinate, plus the width and height of the selected area.\nSee Developer help documentation for more info.", ColorInfo);

				StdOutWithColor("Enter Top-Left X:", ColorQuestion);
				int topLeftX = int.Parse(Console.ReadLine() ?? string.Empty);

				StdOutWithColor("Enter Top-Left Y:", ColorQuestion);
				int topLeftY = int.Parse(Console.ReadLine() ?? string.Empty);

				StdOutWithColor("Enter Width:", ColorQuestion);
				int width = int.Parse(Console.ReadLine() ?? string.Empty);

				StdOutWithColor("Enter height:", ColorQuestion);
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

				string correctionPath = CellXYScaleCorrectionPath + space.EditorID;

				StdOutWithColor($"Corrections:\nxCenter: {space.CenterX}->{correctedXCenter}\nyCenter:{space.CenterY}->{correctedYCenter}\nmaxRange: {space.MaxRange}->{correctedRange}", ColorInfo);
				File.WriteAllText(correctionPath, $"{correctedXCenter}\n{correctedYCenter}\n{correctedRange}");

				StdOutWithColor($"Correction file written to {correctionPath}.\nYou must rebuild the database and re-render the affected cell(s).\n\nPress any key to do another space.\n", ColorInfo);
				Console.ReadKey();
			}
		}

		// Handles the process of manually finding the correct height crop for a space render
		// Overarching function which simply tracks the corrected spaces, calls the heavy-lifting function, and finally renders the corrected spaces.
		static void SpaceHeightCorrection()
		{
			List<Space> correctedSpaces = new List<Space>();

			while (true)
			{
				Space? correctedSpace = DoSpaceHeightCorrection();

				// If the user aborted this space, don't add it to the list.
				// Additionally, if we haven't corrected any spaces yet, don't offer to render - just do another space
				if (correctedSpace != null)
				{
					correctedSpaces.Add(correctedSpace);
				}
				else if (correctedSpaces.Count == 0)
				{
					continue;
				}

				StdOutWithColor($"Press \"y\" to finish height correction and render the corrected cells.\nPress any other key to correct another cell.", ColorInfo);

				char key = Console.ReadKey().KeyChar;
				if (key.ToString().Equals("y", StringComparison.OrdinalIgnoreCase))
				{
					Console.WriteLine("\n");
					break;
				}
			}

			NormalRender(correctedSpaces);
		}

		// Does the space height correction
		static Space? DoSpaceHeightCorrection()
		{
			Space space = GetSingleSpaceInput();
			int height = GetSpaceCameraHeight(space);

			// Loop of user estimates new height, render, ask again.
			while (true)
			{
				string outputFile = TempPath + $"debug_{space.EditorID}_Z{height}.dds";

				string renderCommand = $"{Fo76UtilsRenderPath} \"{GameESMPath}\" {outputFile} {Common.MapImageResolution} {Common.MapImageResolution} " +
					$"\"{GameDataPath.WithoutTrailingSlash()}\" {(space.IsWorldspace ? $"-btd \"{GameTerrainPath}\"" : string.Empty)} " +
					$"-w 0x{space.FormID.ToHex()} -l 0 -cam {Common.MapImageResolution / space.MaxRange} 180 0 0 {space.CenterX} {space.CenterY} {height} " +
					$"-light 1.8 65 180 -rq 0 -scol 1 -ssaa 0 -ltxtres 64 -mlod 4 -xm effects";

				Console.WriteLine($"Rendering {space.EditorID} to {outputFile}");
				Process renderJob = StartProcess(renderCommand);
				renderJob.WaitForExit();
				Common.OpenURI(outputFile);

				StdOutWithColor($"Was {height} the correct height? Enter \"y\" to save, \"exit\" to exit, or otherwise enter a new height to try again:\n", ColorQuestion);
				string input = Console.ReadLine() ?? "1000";

				// The correct crop has been found - save it to a file, do the proper render, and finish.
				if (input.Equals("y", StringComparison.OrdinalIgnoreCase))
				{
					string correctionPath = CellZCorrectionPath + space.EditorID;

					File.WriteAllText(correctionPath, height.ToString());
					StdOutWithColor($"Correction file written to {correctionPath}.\n", ColorInfo);
					return space;
				}
				else if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
				{
					return null;
				}

				height = int.Parse(input);
			}
		}

		// Renders a space in the normal way, and converts and moves it appropriately.
		// Worldspaces also see the watermask generated.
		static void RenderSpace(Space space, bool silent = false)
		{
			int renderResolution = space.IsWorldspace ? WorldspaceRenderResolution : Common.MapImageResolution;
			string ddsFile = TempPath + $"{space.EditorID}_render.dds";
			string finalFile = (space.IsWorldspace ? WorldPath : CellPath) + space.EditorID + ".jpg";
			string terrainString = space.IsWorldspace ? $"-btd \"{GameTerrainPath}\" " : string.Empty;
			double scale = renderResolution / space.MaxRange;

			// -rq 1 + 2 + 12 + 256 (+32 for cells)
			string renderCommand = $"{Fo76UtilsRenderPath} \"{GameESMPath}\" {ddsFile} {renderResolution} {renderResolution} " +
				$"\"{GameDataPath.WithoutTrailingSlash()}\" {terrainString}" +
				$"-w 0x{space.FormID.ToHex()} -l 0 -cam {scale} 180 0 0 {space.CenterX} {space.CenterY} {GetSpaceCameraHeight(space)} " +
				$"-light 1.8 65 180 -lcolor 1.1 0xD6CCC7 0.9 -1 -1 -rq {(space.IsWorldspace ? "271" : "303")} -ssaa 2 " +
				$"-ltxtres 512 -mip 1 -lmip 2 -mlod 0 -ndis 1 " +
				$"-xm " + string.Join(" -xm ", Hardcodings.RenderExcludeModels);

			string resizeCommand = $"magick {ddsFile} -resize {Common.MapImageResolution}x{Common.MapImageResolution} " +
						$"-quality {(space.IsWorldspace ? JpegQualityWorldspace : JpegQualityCell)} JPEG:{finalFile}";

			Process render = StartProcess(renderCommand, silent);
			render.WaitForExit();

			Process magickResizeConvert = StartProcess(resizeCommand, silent);
			magickResizeConvert.WaitForExit();

			// Do the watermask
			if (space.IsWorldspace)
			{
				string watermaskDDS = TempPath + space.EditorID + "_waterMask.dds";
				string watermaskFinalFile = WorldPath + space.EditorID + "_waterMask.png";

				string waterMaskRenderCommand = $"{Fo76UtilsRenderPath} \"{GameESMPath}\" {watermaskDDS} {renderResolution} {renderResolution} " +
					$"\"{GameDataPath.WithoutTrailingSlash()}\" {terrainString} -w 0x{space.FormID.ToHex()} -l 0 -cam {scale} 180 0 0 {space.CenterX} {space.CenterY} {GetSpaceCameraHeight(space)} " +
					$"-light 1 0 0 -ssaa 2 -watermask 1 -xm water " +
					$"-xm " + string.Join(" -xm ", Hardcodings.RenderExcludeModels);

				string watermaskResizeCommand = $"magick {watermaskDDS} -fill #0000FF -fuzz 25% +opaque #000000 -transparent #000000 -resize {Common.MapImageResolution}x{Common.MapImageResolution} PNG:{watermaskFinalFile}";

				Process watermaskRender = StartProcess(waterMaskRenderCommand, silent);
				watermaskRender.WaitForExit();

				Process magickWatermaskResizeConvert = StartProcess(watermaskResizeCommand, silent);
				magickWatermaskResizeConvert.WaitForExit();
			}
		}

		// Returns a list of Spaces gathered from the user, for rendering
		static List<Space> GetSpaceInput()
		{
			StdOutWithColor("\nEnter a space-separated list of space EditorIDs to render. Or enter nothing to render all", ColorQuestion);
			string input = Console.ReadLine() ?? string.Empty;

			List<string> requestedIDs = input.Trim().Split(" ").Where(space => !string.IsNullOrWhiteSpace(space)).ToList();

			// Select spaces unconditionally if none provided, otherwise select only the list of provided spaces
			string query = $"SELECT * FROM Space{(requestedIDs.Count == 0 ? string.Empty : $" WHERE spaceEditorID IN {requestedIDs.ToSqliteCollection()}")} ORDER BY isWorldspace DESC, spaceEditorID ASC;";
			List<Space> spaces = CommonDatabase.GetSpaces(GetNewConnection(), query);

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
			while (true)
			{
				StdOutWithColor("\nEnter the EditorID of the space:", ColorQuestion);
				string input = Console.ReadLine() ?? string.Empty;

				Space? space = CommonDatabase.GetSpaceByEditorID(GetNewConnection(), input);

				if (space == null)
				{
					StdOutWithColor($"Space with editorID \"{input}\" was not found in the database.", ColorError);
				}
				else
				{
					return space;
				}
			}
		}

		// Returns the predetermined cropped height for a cell, otherwise max height
		public static int GetSpaceCameraHeight(Space space)
		{
			string cropFile = CellZCorrectionPath + space.EditorID;

			if (File.Exists(cropFile))
			{
				return int.Parse(File.ReadAllText(cropFile));
			}
			else
			{
				return (int)Math.Pow(2, 16);
			}
		}

		// Starts the given process from CMD. Returns the Process reference. Discards the std out if silent is true.
		public static Process StartProcess(string command, bool silent = false)
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo()
			{
				FileName = "CMD.exe",
				Arguments = "/C " + command,
				RedirectStandardOutput = silent,
				RedirectStandardError = silent,
			};

			Process? process = Process.Start(processStartInfo);

			return process ?? throw new Exception("Failed to start process with command " + command);
		}
	}
}
