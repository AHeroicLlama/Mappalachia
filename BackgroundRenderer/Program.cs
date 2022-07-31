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

		static SqliteConnection connection = new SqliteConnection("Data Source=" + databasePath + ";Mode=ReadOnly");

		static int resolution = 16384; //1024, 4096, 16384
		static bool SSAA = true;

		public static void Main()
		{
			Console.Title = "Mappalachia Background Renderer";

			SqliteCommand query;
			SqliteDataReader reader;
			connection.Open();
			List<Space> spaces = new List<Space>();

			int xOffset = 0;
			int yOffset = 0;
			int xRange = 0;
			int yRange = 0;

			query = connection.CreateCommand();
			query.CommandText =
				"SELECT spaceFormID, spaceEditorID, isWorldspace\n" +
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

				spaces.Add(new Space(reader.GetString(0), reader.GetString(1)));
			}

			Console.WriteLine($"Rendering {spaces.Count} cells at {resolution}*{resolution}px");
			int i = 0;

			foreach (Space space in spaces)
			{
				i++;
				Console.WriteLine($"\n0x{space.formID} : {space.editorID} ({i} of {spaces.Count})");

				query = connection.CreateCommand();
				query.CommandText =
					"SELECT ((max(x) + min(x)) / 2) as xCenter, ((max(y) + min(y)) / 2) as yCenter, abs(max(x) - min(x)) as xRange, abs(max(y) - min(y)) as yRange\n" +
					"FROM Position_Data\n" +
					"WHERE spaceFormID = $spaceFormID";
				query.Parameters.Clear();
				query.Parameters.AddWithValue("$spaceFormID", space.formID);
				reader = query.ExecuteReader();

				while (reader.Read())
				{
					xOffset = reader.GetInt32(0);
					yOffset = reader.GetInt32(1);
					xRange = reader.GetInt32(2);
					yRange = reader.GetInt32(3);
				}

				int range = Math.Max(xRange, yRange);
				double scale = (double)resolution / range;

				Process render = Process.Start("CMD.exe", "/C " + $"{utilsRenderPath} \"{fo76DataPath}\\SeventySix.esm\" {imageDirectory}{space.editorID}.dds {resolution} {resolution} \"{fo76DataPath}\" -w 0x{space.formID} -l 0 -cam {scale} 180 0 0 {xOffset} {yOffset} 65536 -light 1.25 63.435 41.8103 -ssaa {(SSAA ? 1 : 0)} -hqm meshes -env textures/shared/cubemaps/mipblur_defaultoutside1.dds -wtxt textures/water/defaultwater.dds -ltxtres 2048 -mip 0 -lmip 1 -mlod 0 -ndis 1");
				render.WaitForExit();
			}
		}
	}
}