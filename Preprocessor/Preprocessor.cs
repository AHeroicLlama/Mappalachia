using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using MappalachiaLibrary;
using Microsoft.Data.Sqlite;
using static Preprocessor.SQLiteColumn;

namespace Preprocessor
{
	internal class Preprocessor
	{
		static readonly SqliteConnection Connection = new SqliteConnection("Data Source=" + BuildPaths.GetDatabasePath());

		static readonly Regex FormIDRegex = new Regex(@".*\[[A-Z_]{4}:([0-9A-F]{8})\].*");
		static readonly Regex RemoveReferenceRegex = new Regex(@"(.*) ?\[[A-Z_]{4}:[0-9A-F]{8}\]");

		static readonly SQLiteType CoordinateType = SQLiteType.REAL;

		// Provides the WHERE clause for a query which defines the rules of which cells we should discard, as they appear to be inaccessible.
		static readonly string DiscardCellsQuery =
			"spaceDisplayName = '' OR " +
			"spaceDisplayName LIKE '%Test%World%' OR " + 
			"spaceDisplayName LIKE '%Test%Cell%' OR " +
			"spaceEditorID LIKE 'zCUT%' OR " +
			"spaceEditorID LIKE '%OLD' OR " +
			"spaceEditorID LIKE 'Warehouse%' OR " +
			"spaceEditorID LIKE 'Test%' OR " +
			"spaceEditorID LIKE '%Debug%' OR " +
			"spaceEditorID LIKE 'zz%' OR " +
			"spaceEditorID LIKE '76%' OR " +
			"spaceEditorID LIKE '%Worldspace' OR " +
			"spaceEditorID LIKE '%Nav%Test%' OR " +
			"spaceEditorID LIKE 'PackIn%' OR " +
			"spaceEditorID LIKE 'COPY%' OR " +
			"spaceDisplayName = 'Purgatory' OR " +
			"spaceDisplayName = 'Diamond City' OR " +
			"spaceDisplayName = 'Goodneighbor'";

		static readonly List<SQLiteTable> Tables = new List<SQLiteTable>()
		{
			new SQLiteTable("Entity", new List<SQLiteColumn> {
				new SQLiteColumn( "entityFormID", SQLiteType.INTEGER ),
				new SQLiteColumn( "displayName", SQLiteType.TEXT ),
				new SQLiteColumn( "editorID", SQLiteType.TEXT ),
				new SQLiteColumn( "signature", SQLiteType.TEXT ),
				new SQLiteColumn( "percChanceNone", SQLiteType.INTEGER ),
			}),

			new SQLiteTable("Position", new List<SQLiteColumn> {
				new SQLiteColumn( "spaceFormID", SQLiteType.INTEGER ),
				new SQLiteColumn( "referenceFormID", SQLiteType.TEXT ),
				new SQLiteColumn( "x", CoordinateType ),
				new SQLiteColumn( "y", CoordinateType ),
				new SQLiteColumn( "z", CoordinateType ),
				new SQLiteColumn( "locationFormID", SQLiteType.TEXT ),
				new SQLiteColumn( "lockLevel", SQLiteType.TEXT ),
				new SQLiteColumn( "primitiveShape", SQLiteType.TEXT ),
				new SQLiteColumn( "boundX", CoordinateType ),
				new SQLiteColumn( "boundY", CoordinateType ),
				new SQLiteColumn( "boundZ", CoordinateType ),
				new SQLiteColumn( "rotZ", SQLiteType.REAL ),
				new SQLiteColumn( "mapMarkerName", SQLiteType.TEXT ),
				new SQLiteColumn( "shortName", SQLiteType.TEXT ),
			}),

			new SQLiteTable("Space", new List<SQLiteColumn> {
				new SQLiteColumn( "spaceFormID", SQLiteType.INTEGER ),
				new SQLiteColumn( "spaceEditorID", SQLiteType.TEXT ),
				new SQLiteColumn( "spaceDisplayName", SQLiteType.TEXT ),
				new SQLiteColumn( "isWorldspace", SQLiteType.INTEGER ),
			}),

			new SQLiteTable("Location", new List<SQLiteColumn> {
				new SQLiteColumn( "locationFormID", SQLiteType.INTEGER ),
				new SQLiteColumn( "property", SQLiteType.TEXT ),
				new SQLiteColumn( "value", SQLiteType.REAL ),
			}),

			new SQLiteTable("Region", new List<SQLiteColumn> {
				new SQLiteColumn( "spaceFormID", SQLiteType.TEXT ),
				new SQLiteColumn( "regionFormID", SQLiteType.INTEGER ),
				new SQLiteColumn( "regionEditorID", SQLiteType.TEXT ),
				new SQLiteColumn( "regionIndex", SQLiteType.INTEGER ),
				new SQLiteColumn( "coordIndex", SQLiteType.INTEGER ),
				new SQLiteColumn( "x", CoordinateType ),
				new SQLiteColumn( "y", CoordinateType ),
			}),

			new SQLiteTable("Scrap", new List<SQLiteColumn> {
				new SQLiteColumn( "scrapFormID", SQLiteType.INTEGER ),
				new SQLiteColumn( "component", SQLiteType.TEXT ),
				new SQLiteColumn( "componentQuantity", SQLiteType.TEXT ),
			}),

			new SQLiteTable("Component", new List<SQLiteColumn> {
				new SQLiteColumn( "component", SQLiteType.TEXT ),
				new SQLiteColumn( "singular", SQLiteType.TEXT ),
				new SQLiteColumn( "rare", SQLiteType.TEXT ),
				new SQLiteColumn( "medium", SQLiteType.TEXT ),
				new SQLiteColumn( "low", SQLiteType.TEXT ),
				new SQLiteColumn( "high", SQLiteType.TEXT ),
				new SQLiteColumn( "bulk", SQLiteType.TEXT ),
			}),
		};

		static void Main()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			Cleanup();

			Connection.Open();

			// TODO do coords need to be REAL or can we get away with INTEGER?
			Tables.ForEach(table => ImportTableFromCSV(table));

			Console.WriteLine("Unescaping characters");
			Tables.ForEach(table => UnescapeCharacters(table));

            // Pull the MapMarker data into a new table, separate from Position
            SimpleQuery("CREATE TABLE MapMarker AS SELECT spaceFormID, referenceFormID as label, mapMarkerName as icon FROM Position WHERE mapMarkerName != '';");
			SimpleQuery("ALTER TABLE Position DROP COLUMN mapMarkerName;");

			// TODO rigourous map marker correction

			Console.WriteLine("Reducing data");
			TransformColumn("Position", "referenceFormID", ConvertToFormID);
			ChangeColumnType("Position", "referenceFormID", "INTEGER");

			TransformColumn("Region", "spaceFormID", ConvertToFormID);
			ChangeColumnType("Region", "spaceFormID", "INTEGER");

			if (CoordinateType == SQLiteType.INTEGER)
			{
				TransformColumn("Position", "x", RealToInt);
				TransformColumn("Position", "y", RealToInt);
				TransformColumn("Position", "z", RealToInt);
				TransformColumn("Position", "boundX", RealToInt);
				TransformColumn("Position", "boundY", RealToInt);
				TransformColumn("Position", "boundZ", RealToInt);
				TransformColumn("Region", "x", RealToInt);
				TransformColumn("Region", "y", RealToInt);
			}

			// TODO replace Position.ShortName with both label and instanceFormID

			// TODO Correct displayName of LVLI in Entity

			// Discard spaces which are not accessible, and output a list of those
			List<string> deletedRows = SimpleQuery($"DELETE FROM Space WHERE {DiscardCellsQuery} RETURNING spaceEditorID, spaceDisplayName, spaceFormID, isWorldspace;");
			deletedRows.Sort();
			File.WriteAllLines(BuildPaths.GetDiscardedCellsPath(), deletedRows);

			SimpleQuery("DELETE FROM Position WHERE spaceFormID NOT IN (SELECT spaceFormID FROM Space);"); // Remove coordinates located in discarded spaces
			SimpleQuery("DELETE FROM Region WHERE spaceFormID NOT IN (SELECT spaceFormID FROM Space);"); // Remove regions located in discarded spaces
			SimpleQuery("DELETE FROM Entity WHERE entityFormID NOT IN (SELECT referenceFormID FROM Position);"); // Remove entities which are not placed

			// TODO NPCs
			// TODO Scrap

			// TODO Add Meta table, version, date etc
			
			Console.WriteLine("Vacuuming");
			SimpleQuery("VACUUM;");

			Console.WriteLine($"Done. {stopwatch.Elapsed}. Press any key");
			Console.ReadKey();
		}

		// Executes any query against the open database.
		// Returns output rows as comma-separated strings in a list.
		static List<string> SimpleQuery(string query)
		{
			SqliteDataReader reader = new SqliteCommand(query, Connection).ExecuteReader();

			List<string> data = new List<string>();

			while (reader.Read())
			{
				object[] row = new object[reader.FieldCount];
				reader.GetValues(row);
				data.Add(string.Join(",", row));
			}

			return data;
		}

		// Changes the type of the given column
		static void ChangeColumnType(string table, string column, string type)
		{
			string tempColumn = "temp";

			SimpleQuery($"ALTER TABLE {table} ADD COLUMN {tempColumn} {type};"); // Create a temp column with the new type
			SimpleQuery($"UPDATE {table} SET {tempColumn} = {column};"); // Copy the source column into the temp
			SimpleQuery($"ALTER TABLE {table} DROP COLUMN {column};"); // Drop the original
			SimpleQuery($"ALTER TABLE {table} RENAME COLUMN {tempColumn} TO {column};"); // Rename temp column to original
		}

		// Creates the table and imports data from CSV
		static void ImportTableFromCSV(SQLiteTable table)
		{
			Console.WriteLine($"Importing raw table '{table.Name}'");

			CreateTable(table);

			Process process = Process.Start(BuildPaths.GetSqlitePath(), new List<string>() { BuildPaths.GetDatabasePath(), ".mode csv", $".import {BuildPaths.GetFo76EditOutputPath()}{table.Name}.csv {table.Name}" });
			process.WaitForExit();
		}

		static void CreateTable(SQLiteTable table)
		{
			SimpleQuery($"CREATE TABLE {table.Name}({string.Join(", ", table.Fields.Select(f => f.Name + " " + f.Type))});");
		}

		static void UnescapeCharacters(SQLiteTable table)
		{
			// Loop over the fields and replace escaped characters
			foreach (SQLiteColumn field in table.Fields)
			{
				// We should only find escaped chars in text fields
				if (field.Type != SQLiteType.TEXT)
				{
					continue;
				}

				TransformColumn(table.Name, field.Name, UnescapeCharacters);
			}
		}

		// Loops a column and transforms the data according to the passed method
		static void TransformColumn(string tableName, string columnName, Func<string, string> method)
		{
			string tempIndex = "tempIndex";

			SimpleQuery($"CREATE INDEX {tempIndex} ON {tableName} ({columnName});");

			string readQuery = $"SELECT {columnName} FROM {tableName}";
			string updateQuery = $"UPDATE {tableName} SET {columnName} = @new WHERE {columnName} = @original";

			SqliteCommand readCommand = new SqliteCommand(readQuery, Connection);
			SqliteDataReader reader = readCommand.ExecuteReader();

			using (SqliteTransaction transaction = Connection.BeginTransaction())
			using (SqliteCommand updateCommand = new SqliteCommand(updateQuery, Connection, transaction))
			{
				updateCommand.Parameters.AddWithValue("@new", string.Empty);
				updateCommand.Parameters.AddWithValue("@original", string.Empty);

				while (reader.Read())
				{
					string originalValue = reader.GetString(0);
					string newValue = method(originalValue);

					if (newValue == originalValue)
					{
						continue;
					}

					updateCommand.Parameters["@new"].Value = newValue;
					updateCommand.Parameters["@original"].Value = originalValue;

					updateCommand.ExecuteNonQuery();
				}

				transaction.Commit();
			}

			SimpleQuery($"DROP INDEX '{tempIndex}'");
		}

		// Return the given string with custom escape sequences replaced
		static string UnescapeCharacters(string input)
		{
			return input.Replace(":COMMA:", ",").Replace(":QUOT:", "\"").Replace("''", "'");
		}

		// Converts a valid 8-char hex FormID to the string value of the integer value of itself
		static string ConvertToFormID(string input)
		{
			string formid = FormIDRegex.Match(input).Groups[1].Value;

			if (string.IsNullOrEmpty(formid))
			{
				return string.Empty;
			}

			return Convert.ToInt32(formid, 16).ToString();
		}

		static string RemoveReference(string input)
		{
			return RemoveReferenceRegex.Match(input).Groups[1].Value;
		}

		// Converts a database REAL as a string, to a string suitable to be a database INTEGER
		static string RealToInt(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return string.Empty;
			}

			return ((int)Math.Round(double.Parse(input))).ToString();
		}

		// Removes old DB files prior to building new
		static void Cleanup()
		{
			Console.WriteLine("Cleaning up");

			File.Delete(BuildPaths.GetDatabasePath());
			File.Delete(BuildPaths.GetDatabasePath() + "-journal");
		}
	}
}
