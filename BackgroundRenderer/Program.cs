using Microsoft.Data.Sqlite;
using System.Diagnostics;

namespace BackgroundRenderer
{
	public partial class Program
	{
		static string fo76DataPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Fallout76\\Data";
		static string thisAppPath = Directory.GetCurrentDirectory();
		static string mappalachiaRoot = thisAppPath + "..\\..\\..\\..\\..\\";
		static string databasePath = mappalachiaRoot + "Mappalachia\\data\\mappalachia.db";
		static string imageDirectory =  mappalachiaRoot + "Mappalachia\\img\\";
		static string utilsRenderPath = mappalachiaRoot + "FO76Utils\\render.exe";

		static double maxScale = 16;
		static double minScale = 0.02;

		static SqliteConnection connection = new SqliteConnection("Data Source=" + databasePath + ";Mode=ReadOnly");

		// 16384 by default (16k - allows us to supersample then scale down to 4k)
		/* 4096 if the cell is unusually small and causes the scale to be too high...
		(errors.txt will be written) (EG UCB02 and RaiderRaidTrailerInt) */
		static int resolution = 16384;

		static bool SSAA = true;

		public static void Main()
		{
			Console.Title = "Mappalachia Background Renderer";

			Console.WriteLine("Paste space-separated FormIDs of Cells you need rendering. Otherwise paste nothing to render all");
			string arg = Console.ReadLine();
			List<string> args = arg.Split(' ').Where(a => !string.IsNullOrWhiteSpace(a)).ToList();

			if (args.Count == 0)
			{
				Console.WriteLine("Rendering all cells in database...");
			}
			else
			{
				Console.WriteLine("\nOnly rendering Cells with the following FormIDs:");
				foreach (string formid in args)
				{
					Console.WriteLine(formid);
				}

				Console.WriteLine();
			}

			SqliteCommand query;
			SqliteDataReader reader;
			connection.Open();
			List<Space> spaces = new List<Space>();

			query = connection.CreateCommand();
			query.CommandText =
				"SELECT spaceFormID, spaceEditorID, isWorldspace, xCenter, yCenter, xMin, xMax, yMin, yMax\n" +
				"FROM Space_Info";
			query.Parameters.Clear();
			reader = query.ExecuteReader();

			while (reader.Read())
			{
				// Skip worldspaces
				if (reader.GetInt16(2) == 1)
				{
					continue;
				}

				// We specified certain cells, so skip everything not asked for
				if (args.Count > 0 && !args.Contains(reader.GetString(0)))
				{
					Console.WriteLine("Skipping " + reader.GetString(1));
					continue;
				}

				spaces.Add(new Space(reader.GetString(0), reader.GetString(1), reader.GetInt32(3), reader.GetInt32(4), Math.Abs(reader.GetInt32(6) - reader.GetInt32(5)), Math.Abs(reader.GetInt32(8) - reader.GetInt32(8))));
			}

			Console.WriteLine($"\nRendering {spaces.Count} cells at {resolution}*{resolution}px");
			int i = 0;

			foreach (Space space in spaces)
			{
				i++;
				Console.WriteLine($"\n0x{space.formID} : {space.editorID} ({i} of {spaces.Count})");

				int range = Math.Max(space.xRange, space.yRange);
				double scale = (double)resolution / range;

				if (scale > maxScale || scale < minScale)
				{
					string error = $"Space {space.editorID} too small or large to render at this resolution! (Scale of {scale} outside range {minScale}-{maxScale}). Change the render resolution in order to preserve the scale\nFormID: {space.formID}\n";
					Console.WriteLine(error);
					File.AppendAllText(imageDirectory + "\\errors.txt", error);
				}

				Process render = Process.Start("CMD.exe", "/C " + $"{utilsRenderPath} \"{fo76DataPath}\\SeventySix.esm\" {imageDirectory}{space.editorID}.dds {resolution} {resolution} \"{fo76DataPath}\" -w 0x{space.formID} -l 0 -cam {scale} 180 0 0 {space.xCenter} {space.yCenter} 65536 -light 1.25 63.435 41.8103 -ssaa {(SSAA ? 1 : 0)} -hqm meshes -env textures/shared/cubemaps/mipblur_defaultoutside1.dds -wtxt textures/water/defaultwater.dds -ltxtres 1024 -mip 0 -lmip 1 -mlod 0 -ndis 1");
				render.WaitForExit();
			}
		}
	}
}