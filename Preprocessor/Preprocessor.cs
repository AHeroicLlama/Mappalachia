using System.Diagnostics;
using MappalachiaLibrary;

namespace Preprocessor
{
	internal class Preprocessor
	{
		static readonly List<SQLiteTable> tables = new List<SQLiteTable>()
			{
				new SQLiteTable("Entity", new Dictionary<string, string> {
					{ "entityFormID", "INTEGER" },
					{ "displayName", "TEXT" },
					{ "editorID", "TEXT" },
					{ "signature", "TEXT" },
					{ "percChanceNone", "INTEGER" },
				}),

				new SQLiteTable("Position", new Dictionary<string, string> {
					{ "spaceFormID", "INTEGER" },
					{ "referenceFormID", "TEXT" },
					{ "x", "REAL" },
					{ "y", "REAL" },
					{ "z", "REAL" },
					{ "locationFormID", "TEXT" },
					{ "lockLevel", "TEXT" },
					{ "primitiveShape", "TEXT" },
					{ "boundX", "REAL" },
					{ "boundY", "REAL" },
					{ "boundZ", "REAL" },
					{ "rotZ", "REAL" },
					{ "mapMarkerName", "TEXT" },
					{ "shortName", "TEXT" },
				}),

				new SQLiteTable("Space", new Dictionary<string, string> {
					{ "spaceFormID", "INTEGER" },
					{ "spaceEditorID", "TEXT" },
					{ "spaceDisplayName", "TEXT" },
					{ "isWorldspace", "INTEGER" },
				}),

				new SQLiteTable("Location", new Dictionary<string, string> {
					{ "locationFormID", "INTEGER" },
					{ "propertu", "TEXT" },
					{ "value", "REAL" },
				}),

				new SQLiteTable("Region", new Dictionary<string, string> {
					{ "spaceFormID", "INTEGER" },
					{ "regionFormID", "INTEGER" },
					{ "regionEditorID", "TEXT" },
					{ "regionIndex", "INTEGER" },
					{ "coordIndex", "INTEGER" },
					{ "x", "REAL" },
					{ "y", "REAL" },
				}),

				new SQLiteTable("Scrap", new Dictionary<string, string> {
					{ "scrapFormID", "INTEGER" },
					{ "component", "TEXT" },
					{ "componentQuantity", "TEXT" },
				}),

				new SQLiteTable("Component", new Dictionary<string, string> {
					{ "component", "TEXT" },
					{ "singular", "TEXT" },
					{ "rare", "TEXT" },
					{ "medium", "TEXT" },
					{ "low", "TEXT" },
					{ "high", "TEXT" },
					{ "bulk", "TEXT" },
				}),
			};

		static void Main()
		{
            Console.WriteLine("Cleaning up");
            Cleanup();

			tables.ForEach(table => ImportTableFromCSV(table));

			Console.WriteLine("Done. Press any key");
			Console.ReadKey();
        }

		// Passes a command to the database via SQLite tools exe
		static void ExecuteSqlite(List<string> commands)
		{
			commands.Insert(0, BuildPaths.GetDatabasePath());

			Process process = Process.Start(BuildPaths.GetSqlitePath(), commands);
			process.WaitForExit();
		}

		// Passes a command to the database via SQLite tools exe
		static void ExecuteSqlite(string command)
		{
			ExecuteSqlite(new List<string>() { command });
		}

		static void ImportTableFromCSV(SQLiteTable table)
		{
			Console.WriteLine($"Importing raw table {table.Name}");
			ExecuteSqlite($"CREATE TABLE {table.Name}({string.Join(", ", table.Fields.Select(f => f.Key + " " + f.Value))});");
			ExecuteSqlite(new List<string>() { ".mode csv", $".import {BuildPaths.GetFo76EditOutputPath()}{table.Name}.csv {table.Name}" });
		}

		// Removes old DB files prior to building new
		static void Cleanup()
		{
			File.Delete(BuildPaths.GetDatabasePath());
			File.Delete(BuildPaths.GetDatabasePath() + "-journal");
		}
	}
}
