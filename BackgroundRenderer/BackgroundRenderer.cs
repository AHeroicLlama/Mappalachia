using System.Diagnostics;
using Library;
using Microsoft.Data.Sqlite;
using static Library.BuildTools;
using static Library.Common;

namespace BackgroundRenderer
{
	static class BackgroundRenderer
	{
		static int JpgQualityStandard { get; } = 85;

		static int JpgQualityHigh { get; } = 100;

		static int RenderParallelism { get; } = 16; // Max cells or spotlight tiles to render in parallel

		static async Task Main()
		{
			Console.Title = "Mappalachia Background Renderer";

			// Ensure the output directories exist
			foreach (string directory in new string[] { CellPath, WorldPath, SpotlightPath })
			{
				if (!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}
			}

			CleanUpSpotlight();

			StdOutWithColor("Enter:\n1: Render (Inc. spotlight) Mode\n2: Space Zoom/Offset (X/Y) correction/debug mode\n3: Space height (Z) crop correction/debug mode", ColorQuestion);

			switch (Console.ReadKey().KeyChar)
			{
				case '1':
					await NormalRender();
					break;

				case '2':
					await SpaceZoomOffsetCorrection();
					break;

				case '3':
					await SpaceHeightCorrection();
					break;

				default:
					throw new Exception("Invalid Choice");
			}
		}

		// Performs the typical end-product render for the given spaces, or if null, asks the user for which
		// Provides informative logging output, est time remaining etc
		static async Task NormalRender(List<Space>? spaces = null)
		{
			spaces ??= await GetSpaceInput();

			Stopwatch generalStopwatch = Stopwatch.StartNew();

			// Break the spaces into cells and worldspaces
			List<Space> cells = spaces.Where(space => !space.IsWorldspace).ToList();
			List<Space> worldspaces = spaces.Where(space => space.IsWorldspace).ToList();

			StdOutWithColor($"Rendering {worldspaces.Count} worldspace{Pluralize(worldspaces)} and {cells.Count} cell{Pluralize(cells)}...", ColorInfo);

			int spacesRendered = 0;

			// Do the serial render of worldspaces first
			foreach (Space worldspace in worldspaces)
			{
				StdOutWithColor($"\nRendering {worldspace.EditorID} (0x{worldspace.FormID.ToHex()})", ColorInfo);
				await SpotlightRenderSpace(worldspace);
				RenderSpace(worldspace);
				AnnounceRenderProgress(spaces, worldspace, ref spacesRendered);
			}

			Stopwatch cellStopwatch = Stopwatch.StartNew();

			// Render cells in parallel
			Parallel.ForEach(cells, new ParallelOptions() { MaxDegreeOfParallelism = RenderParallelism }, async cell =>
			{
				if (SpotlightInCells)
				{
					await SpotlightRenderSpace(cell);
				}

				RenderSpace(cell, true);
				AnnounceRenderProgress(spaces, cell, ref spacesRendered, cellStopwatch);
			});

			StdOutWithColor($"\nRendering Finished. {generalStopwatch.Elapsed.ToString(@"hh\:mm\:ss")}. Press Any Key.", ColorInfo);
			Console.ReadKey();
		}

		// Runs through the process of rendering a cell, opening it, asking the user for corrective input, then storing that as a file
		static async Task SpaceZoomOffsetCorrection()
		{
			while (true)
			{
				Space space = await GetSingleSpaceInput();

				double resolution = MapImageResolution;
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
				OpenURI(outputFile);

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
		static async Task SpaceHeightCorrection()
		{
			List<Space> correctedSpaces = new List<Space>();

			while (true)
			{
				Space? correctedSpace = await DoSpaceHeightCorrection();

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

			await NormalRender(correctedSpaces);
		}

		// Does the space height correction
		static async Task<Space?> DoSpaceHeightCorrection()
		{
			Space space = await GetSingleSpaceInput();
			int height = GetSpaceCameraHeight(space);

			// Loop of user estimates new height, render, ask again.
			while (true)
			{
				string outputFile = TempPath + $"debug_{space.EditorID}_Z{height}.dds";

				string renderCommand = $"{Fo76UtilsRenderPath} \"{GameESMPath}\" {outputFile} {MapImageResolution} {MapImageResolution} " +
					$"\"{GameDataPath.WithoutTrailingSlash()}\" {(space.IsWorldspace ? $"-btd \"{GameTerrainPath}\"" : string.Empty)} " +
					$"-w 0x{space.FormID.ToHex()} -l 0 -cam {MapImageResolution / space.MaxRange} 180 0 0 {space.CenterX} {space.CenterY} {height} " +
					$"-light 1.8 65 180 -rq 0 -scol 1 -ssaa 0 -ltxtres 64 -mlod 4 -xm effects";

				Console.WriteLine($"Rendering {space.EditorID} to {outputFile}");
				Process renderJob = StartProcess(renderCommand);
				renderJob.WaitForExit();
				OpenURI(outputFile);

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
			int renderResolution = space.IsWorldspace ? WorldspaceRenderResolution : MapImageResolution;
			string ddsFile = TempPath + $"{space.EditorID}.dds";
			string finalFile = (space.IsWorldspace ? WorldPath : CellPath) + space.EditorID + BackgroundImageFileType;
			string terrainString = space.IsWorldspace ? $"-btd \"{GameTerrainPath}\" " : string.Empty;
			double scale = renderResolution / space.MaxRange;

			string renderCommand = $"{Fo76UtilsRenderPath} \"{GameESMPath}\" {ddsFile} {renderResolution} {renderResolution} " +
				$"\"{GameDataPath.WithoutTrailingSlash()}\" {terrainString}" +
				$"-w 0x{space.FormID.ToHex()} -l 0 -cam {scale} 180 0 0 {space.CenterX} {space.CenterY} {GetSpaceCameraHeight(space)} " +
				$"-light 1.8 65 180 -lcolor 1.1 0xD6CCC7 0.9 -1 -1 -rq {1 + 2 + 12 + 256 + (space.IsWorldspace ? 0 : 32)} -ssaa 2 " +
				$"-ltxtres 512 -tc 4096 -mc 64 -mip 1 -lmip 2 -mlod 0 -ndis 1 " +
				$"-xm " + string.Join(" -xm ", BuildTools.RenderExcludeModels);

			string resizeCommand = $"magick {ddsFile} -resize {MapImageResolution}x{MapImageResolution} " +
						$"-quality {(space.IsWorldspace ? JpgQualityHigh : JpgQualityStandard)} JPEG:{finalFile}";

			Process render = StartProcess(renderCommand, silent);
			render.WaitForExit();

			Process magickResizeConvert = StartProcess(resizeCommand, silent);
			magickResizeConvert.WaitForExit();

			// Do the watermask
			if (space.IsWorldspace)
			{
				string waterMaskDDS = TempPath + space.EditorID + WaterMaskAddendum + ".dds";
				string waterMaskFinalFile = WorldPath + space.EditorID + WaterMaskAddendum + MaskImageFileType;

				string waterMaskRenderCommand = $"{Fo76UtilsRenderPath} \"{GameESMPath}\" {waterMaskDDS} {renderResolution} {renderResolution} " +
					$"\"{GameDataPath.WithoutTrailingSlash()}\" {terrainString} -w 0x{space.FormID.ToHex()} -l 0 -cam {scale} 180 0 0 {space.CenterX} {space.CenterY} {GetSpaceCameraHeight(space)} " +
					$"-light 1 0 0 -ssaa 2 -watermask 1 -xm water " +
					$"-xm " + string.Join(" -xm ", BuildTools.RenderExcludeModels);

				string waterMaskResizeCommand = $"magick {waterMaskDDS} -fill #0000FF -fuzz 25% +opaque #000000 -transparent #000000 -resize {MapImageResolution}x{MapImageResolution} PNG:{waterMaskFinalFile}";

				Process waterMaskRender = StartProcess(waterMaskRenderCommand, silent);
				waterMaskRender.WaitForExit();

				Process magickWaterMaskResizeConvert = StartProcess(waterMaskResizeCommand, silent);
				magickWaterMaskResizeConvert.WaitForExit();
			}
		}

		static async Task SpotlightRenderSpace(Space space)
		{
			double scale = 1d / SpotlightScale;

			string outputPath = TempPath + $"{space.EditorID}\\";
			string finalPath = SpotlightPath + $"{space.EditorID}\\";

			// If there is little to no improvement from spotlight, skip
			if (!space.WouldBenefitFromSpotlight())
			{
				return;
			}

			Directory.CreateDirectory(outputPath);
			Directory.CreateDirectory(finalPath);

			List<SpotlightTile> tiles = space.GetTiles();
			List<Region> worldBorderRegions = await space.GetWorldBorders();

			if (worldBorderRegions.Count > 0)
			{
				tiles = tiles.Where(t => t.IntersectsRegions(worldBorderRegions)).ToList();
			}

			if (!space.IsWorldspace)
			{
				tiles = tiles.Where(t => t.HasEntities().Result).ToList();
			}

			if (space.IsWorldspace)
			{
				StdOutWithColor($"Depending on your hardware, Spotlight rendering an entire worldspace ({space.EditorID}) may take a few hours. Would you like to:\n1:Render the whole space\n2:Render only missing tiles\n3:Define a target area to render", ColorQuestion);

				switch (Console.ReadKey().KeyChar)
				{
					case '1':
						break;

					case '2':
						tiles = tiles.Where(t => !File.Exists(t.GetFilePath())).ToList();
						break;

					case '3':
						StdOutWithColor("\nEnter 3 values: X and Y coordinate of the center tile, and the radius of tiles around it.", ColorQuestion);

						StdOutWithColor("Center X:", ColorQuestion);
						int xCenter = int.Parse(Console.ReadLine() ?? "0");

						StdOutWithColor("Center Y:", ColorQuestion);
						int yCenter = int.Parse(Console.ReadLine() ?? "0");

						StdOutWithColor("Radius (tiles):", ColorQuestion);
						int radius = int.Parse(Console.ReadLine() ?? "0");

						tiles = tiles.Where(t =>
							Math.Abs(t.XId - xCenter) <= radius &&
							Math.Abs(t.YId - yCenter) <= radius)
							.ToList();

						break;

					default:
						StdOutWithColor("Unrecognized input. Skipping this operation. Please try again.", ColorError);
						return;
				}

				Console.WriteLine();
			}

			StdOutWithColor($"Spotlight Rendering {tiles.Count} tile{Pluralize(tiles)} of {space.EditorID}", ColorInfo);

			Stopwatch stopwatch = Stopwatch.StartNew();
			int i = 0;

			// Loop over every tile for the space
			Parallel.ForEach(tiles, new ParallelOptions() { MaxDegreeOfParallelism = RenderParallelism }, tile =>
			{
				string outputFile = $"{outputPath}{tile.XId}.{tile.YId}.dds";
				string finalFile = tile.GetFilePath();

				string renderCommand = $"{Fo76UtilsRenderPath} \"{GameESMPath}\" {outputFile} {SpotlightTileSize} {SpotlightTileSize} " +
					$"\"{GameDataPath.WithoutTrailingSlash()}\" {(space.IsWorldspace ? $"-btd \"{GameTerrainPath}\"" : string.Empty)} " +
					$"-r {tile.XId * SpotlightScale} {tile.YId * SpotlightScale} {(tile.XId + 1) * SpotlightScale} {(tile.YId + 1) * SpotlightScale} " +
					$"-w 0x{space.FormID.ToHex()} -l 0 -cam {scale} 180 0 0 {tile.XCenter} {tile.YCenter} {GetSpaceCameraHeight(space)} " +
					$"-light 1.8 65 180 -lcolor 1.1 0xD6CCC7 0.9 -1 -1 -rq {1 + 2 + 12 + 256 + 32} -ssaa 1 " +
					$"-ltxtres 4096 -tc 4096 -mc 64 -mip 0 -lmip 1 -mlod 0 -ndis 1 " +
					$"-xm " + string.Join(" -xm ", RenderExcludeModels);

				string convertCommand = $"magick {outputFile} -quality {JpgQualityStandard} JPEG:{finalFile}";

				Process render = StartProcess(renderCommand, true);
				render.WaitForExit();

				Process magickResizeConvert = StartProcess(convertCommand);
				magickResizeConvert.WaitForExit();

				File.Delete(outputFile);

				// If the file appears to be the minimum size given its resolution, (plus 512 bytes)
				// This suggests there are no visible entities here, so delete it.
				// Not necessary for Worldspaces
				if (!space.IsWorldspace)
				{
					long size = new FileInfo(finalFile).Length;
					if (size <= (Math.Pow(SpotlightTileSize, 2) / (8 * 8 * 4)) + 512)
					{
						File.Delete(finalFile);
					}
				}

				if (space.IsWorldspace)
				{
					Interlocked.Increment(ref i);
					TimeSpan timePerTile = stopwatch.Elapsed / i;
					TimeSpan timeRemaining = timePerTile * (tiles.Count - i);
					StdOutWithColor(
						$"{space.EditorID} Spotlight: {tile.XId},{tile.YId} (X:{tile.XCenter}, Y:{tile.YCenter}) (Tile {i} of {tiles.Count} ({Math.Round(((double)i / tiles.Count) * 100, 2)}%)) " +
						$"Est {timeRemaining.Days}D {timeRemaining.Hours:D2}H {timeRemaining.Minutes:D2}M {timeRemaining.Seconds:D2}S remaining", ColorInfo);
				}
			});
		}

		// Return if this tile contains any entities
		public static async Task<bool> HasEntities(this SpotlightTile tile)
		{
			string countQuery = $"SELECT count(*) as count FROM Position WHERE SpaceFormID = {tile.Space.FormID} AND " +
				$"x >= {tile.XCenter - TileRadius} AND x <= {tile.XCenter + TileRadius} AND y >= {tile.YCenter - TileRadius} AND y <= {tile.YCenter + TileRadius}";

			SqliteDataReader reader = await CommonDatabase.GetReader(GetNewConnection(), countQuery);
			reader.Read();

			return Convert.ToInt32(reader["count"]) > 0;
		}

		// Returns if the tile intersects any of the regions
		public static bool IntersectsRegions(this SpotlightTile tile, List<Region> regions)
		{
			foreach (Region region in regions)
			{
				if (region.Points.Count == 0)
				{
					throw new ArgumentException($"Region {region.EditorID} has no points");
				}

				// First case - The tile is at least partly within the region because the center of it is
				// It may not be wholly contained
				if (region.ContainsPoint(new Coord(tile.XCenter, tile.YCenter)))
				{
					return true;
				}

				// Edge cases
				// Test if any of the 4 edges of the tile intersect any of the region's edges
				for (int i = 0; i < region.Points.Count; i++)
				{
					int j = i + 1;

					if (i == region.Points.Count - 1)
					{
						j = 0;
					}

					Coord regionPointA = region.Points[i].Coord;
					Coord regionPointB = region.Points[j].Coord;

					Coord topLeft = new Coord(tile.XCenter - TileRadius, tile.YCenter + TileRadius);
					Coord topRight = new Coord(tile.XCenter + TileRadius, tile.YCenter + TileWidth);
					Coord bottomLeft = new Coord(tile.XCenter - TileRadius, tile.YCenter - TileRadius);
					Coord bottomRight = new Coord(tile.XCenter + TileRadius, tile.YCenter - TileRadius);

					if (GeometryHelper.LinesIntersect(regionPointA, regionPointB, topLeft, topRight) ||
						GeometryHelper.LinesIntersect(regionPointA, regionPointB, bottomLeft, bottomRight) ||
						GeometryHelper.LinesIntersect(regionPointA, regionPointB, topLeft, bottomLeft) ||
						GeometryHelper.LinesIntersect(regionPointA, regionPointB, topRight, bottomRight))
					{
						return true;
					}
				}
			}

			return false;
		}

		// Returns a list of Spaces gathered from the user, for rendering
		static async Task<List<Space>> GetSpaceInput()
		{
			StdOutWithColor("\nEnter a space-separated list of space EditorIDs to render.\nEnter nothing to render all, 'cell' for all Cells, or 'world' for all WorldSpaces", ColorQuestion);
			string input = (Console.ReadLine() ?? string.Empty).Trim();

			if (input.EqualsIgnoreCase("cell") || input.EqualsIgnoreCase("world"))
			{
				int worldspaceFlag = input.EqualsIgnoreCase("world") ? 1 : 0;
				return await CommonDatabase.GetSpaces(GetNewConnection(), $"SELECT * FROM Space WHERE isWorldSpace == {worldspaceFlag} ORDER BY spaceEditorID ASC;");
			}

			if (input.IsNullOrWhiteSpace())
			{
				return await CommonDatabase.GetAllSpaces(GetNewConnection());
			}

			List<string> requestedIDs = input.Split(" ").Where(space => !space.IsNullOrWhiteSpace()).ToList();

			// Select spaces unconditionally if none provided, otherwise select only the list of provided spaces
			string query = $"SELECT * FROM Space WHERE spaceEditorID IN {requestedIDs.ToSqliteCollection()} ORDER BY isWorldspace DESC, spaceEditorID ASC;";
			List<Space> spaces = await CommonDatabase.GetSpaces(GetNewConnection(), query);

			// If we specified cells, ensure we found them all in the DB before proceeding
			if (spaces.Count != requestedIDs.Count)
			{
				throw new Exception("Inputted cells were not all found. Check for typos in EditorIDs");
			}

			return spaces;
		}

		// Asks for a single editorID and returns the corresponding space
		static async Task<Space> GetSingleSpaceInput()
		{
			while (true)
			{
				StdOutWithColor("\nEnter the EditorID of the space:", ColorQuestion);
				string input = Console.ReadLine() ?? string.Empty;

				Space? space = await CommonDatabase.GetSpaceByEditorID(GetNewConnection(), input);

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
				StdOutWithColor($"Est {estTimeRemaining.ToString(@"hh\:mm\:ss")} remaining", ColorInfo);
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
