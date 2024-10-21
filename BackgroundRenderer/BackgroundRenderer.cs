using System.Diagnostics;
using Library;
using Microsoft.Data.Sqlite;

namespace BackgroundRenderer
{
	class BackgroundRenderer
	{
		static double MaxZoomScale { get; } = 16;

		static double MinZoomScale { get; } = 0.002;

		static void Main()
		{
			Console.Title = "Mappalachia Background Renderer";
			SqliteConnection connection = BuildTools.GetNewConnection();

			BuildTools.StdOutWithColor("Enter a space-separated list of space EditorIDs to render. Or enter nothing to render all", BuildTools.QuestionColor);
			string input = "TheWayward01";// Console.ReadLine() ?? string.Empty;

			Stopwatch stopwatch = Stopwatch.StartNew();

			List<string> requestedIDs = input.Trim().Split(" ").Where(space => !string.IsNullOrWhiteSpace(space)).ToList();

			// Select spaces unconditionally if none provided, otherwise select only the list of provided spaces
			string query = $"SELECT * FROM Space{(requestedIDs.Count == 0 ? string.Empty : $" WHERE spaceEditorID IN {requestedIDs.ToSqliteCollection()}")};";
			List<Space> spaces = CommonDatabase.GetSpaces(connection, query);

			// If we specified cells, ensure we found them all in the DB before proceeding
			if (requestedIDs.Count != 0 && spaces.Count != requestedIDs.Count)
			{
				throw new Exception("Inputted cells were not all found. Check for typos in EditorIDs");
			}

			spaces.ForEach(RenderSpace);

			BuildTools.StdOutWithColor($"Rendering Finished. {stopwatch.Elapsed}", BuildTools.InfoColor);
			Console.ReadKey();
		}

		static void RenderSpace(Space space)
		{
			double scale = MinZoomScale;
			double cameraX = 0;
			double cameraY = 0;
			double cameraZ = Misc.MaxRenderCameraZ;
			string outputFileName = "temp_out.dds";

			string renderCommand = $"{BuildTools.Fo76UtilsRenderPath} \"{BuildTools.GameESMPath}\" {outputFileName} {Misc.MapImageResolution} {Misc.MapImageResolution} " +
				$"\"{BuildTools.GameDataPath.WithoutTrailingSlash()}\" {(space.IsWorldspace ? $"-btd \"{BuildTools.GameTerrainPath}\"" : string.Empty)} -w 0x{space.FormID.ToHex()} -l 0 -cam {scale} 180 0 0 {cameraX} {cameraY} {cameraZ} " +
				$"-light 1.8 65 180 -rq 0 -scol 1 -ssaa 0 -ltxtres 64 -mlod 4 -xm effects";

			Console.WriteLine($"Rendering {space.EditorID}");
			Process renderJob = Process.Start("CMD.exe", $"/C {renderCommand}");
			renderJob.WaitForExit();

			Misc.OpenURI(outputFileName);
		}
	}
}
