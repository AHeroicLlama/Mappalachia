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
		static Regex SignatureFormIDRegex { get; } = new Regex("\\[[A-Z_]{4}:([0-9A-F]{8})\\]");
		static Regex GetFormIDRegex { get; } = new Regex(".*" + SignatureFormIDRegex + ".*");
		static Regex RemoveTrailingReferenceRegex { get; } = new Regex(@"(.*) " + SignatureFormIDRegex);
		static Regex GetDisplayNameRegex { get; } = new Regex(".* \"(.*)\" " + SignatureFormIDRegex);

		static readonly SQLiteType CoordinateType = SQLiteType.REAL;

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

			Console.WriteLine($"Building Mappalachia database at {BuildPaths.GetDatabasePath()}\n");

			Cleanup();

			Connection.Open();

			// TODO do coords need to be REAL or can we get away with INTEGER?
			Tables.ForEach(table => ImportTableFromCSV(table));

			Tables.ForEach(table => UnescapeCharacters(table));

			// Pull the MapMarker data into a new table, then make some hardcoded amendments and corrections
			SimpleQuery("CREATE TABLE MapMarker AS SELECT spaceFormID, x, y, referenceFormID as label, mapMarkerName as icon FROM Position WHERE mapMarkerName != '';");
			SimpleQuery(Hardcodings.RemoveMarkersQuery);
			SimpleQuery(Hardcodings.AddMissingMarkersQuery);
			SimpleQuery(Hardcodings.CorrectDuplicateMarkersQuery);
			TransformColumn(Hardcodings.CorrectLabelsByDict, "MapMarker", "label");
			TransformColumn(Hardcodings.CorrectFissureLabels, "MapMarker", "label");
			TransformColumn(Hardcodings.CorrectCommonBadLabels, "MapMarker", "label");
			TransformColumn(Hardcodings.CorrectMarkerIcons, "MapMarker", "label", "icon");

			// Remove map marker remnants from Position table
			SimpleQuery("DELETE FROM Position WHERE mapMarkerName != '';");
			SimpleQuery("ALTER TABLE Position DROP COLUMN mapMarkerName;");

			// Remove some junk data from Position
			SimpleQuery("DELETE FROM Position WHERE shortName LIKE '%CELL:%';");

			// Capture and convert to int the referenceFormID
			TransformColumn(CaptureFormID, "Position", "referenceFormID");
			ChangeColumnType("Position", "referenceFormID", "INTEGER");

			// Capture and convert to int the spaceFormID
			TransformColumn(CaptureFormID, "Region", "spaceFormID");
			ChangeColumnType("Region", "spaceFormID", "INTEGER");

			// Transform the coordinate data to int
			if (CoordinateType == SQLiteType.INTEGER)
			{
				TransformColumn(RealToInt, "Position", "x");
				TransformColumn(RealToInt, "Position", "y");
				TransformColumn(RealToInt, "Position", "z");
				TransformColumn(RealToInt, "Position", "boundX");
				TransformColumn(RealToInt, "Position", "boundY");
				TransformColumn(RealToInt, "Position", "boundZ");
				TransformColumn(RealToInt, "Region", "x");
				TransformColumn(RealToInt, "Region", "y");
			}

			// Capture label and instanceFormID values from shortName column, splitting them into their own columns and dropping the original
			SimpleQuery("ALTER TABLE Position ADD COLUMN 'label' TEXT;");
			SimpleQuery("ALTER TABLE Position ADD COLUMN 'instanceFormID' INTEGER;");
			TransformColumn(CaptureFormID, "Position", "shortName", "instanceFormID");
			TransformColumn(RemoveTrailingReference, "Position", "shortName", "label");
			SimpleQuery("ALTER TABLE Position DROP COLUMN 'shortName';");

			// Ensure all Entities have the proper display name only, and nothing extraneous
			TransformColumn(CaptureQuotedDisplayName, "Entity", "displayName");

			// Discard spaces which are not accessible, and output a list of those
			List<string> deletedRows = SimpleQuery($"DELETE FROM Space WHERE {Hardcodings.DiscardCellsQuery} RETURNING spaceEditorID, spaceDisplayName, spaceFormID, isWorldspace;");
			deletedRows.Sort();
			deletedRows.Insert(0, "spaceEditorID,spaceDisplayName,spaceFormID,isWorldspace");
			File.WriteAllLines(BuildPaths.GetDiscardedCellsPath(), deletedRows);

			SimpleQuery("DELETE FROM Position WHERE spaceFormID NOT IN (SELECT spaceFormID FROM Space);"); // Remove coordinates located in discarded spaces
			SimpleQuery("DELETE FROM Region WHERE spaceFormID NOT IN (SELECT spaceFormID FROM Space);"); // Remove regions located in discarded spaces
			SimpleQuery("DELETE FROM MapMarker WHERE spaceFormID NOT IN (SELECT spaceFormID FROM Space);"); // Remove map markers located in discarded spaces
			SimpleQuery("DELETE FROM Entity WHERE entityFormID NOT IN (SELECT referenceFormID FROM Position);"); // Remove entities which are not placed

			// TODO NPCs
			// TODO Scrap

			// TODO Add Meta table, version, date etc

			SimpleQuery("VACUUM;");

			Console.WriteLine($"Done. {stopwatch.Elapsed.ToString("m\\m\\ s\\s")}. Press any key");
			Console.ReadKey();
		}

		// Executes any query against the open database.
		// Returns output rows as comma-separated strings in a list.
		static List<string> SimpleQuery(string query, bool silent = false)
		{
			if (!silent)
			{
				Console.WriteLine(query);
			}

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

            Console.WriteLine($"Change type: {table}.{column} -> {type}");

			SimpleQuery($"ALTER TABLE {table} ADD COLUMN {tempColumn} {type};", true); // Create a temp column with the new type
			SimpleQuery($"UPDATE {table} SET {tempColumn} = {column};", true); // Copy the source column into the temp
			SimpleQuery($"ALTER TABLE {table} DROP COLUMN {column};", true); // Drop the original
			SimpleQuery($"ALTER TABLE {table} RENAME COLUMN {tempColumn} TO {column};", true); // Rename temp column to original
		}

		// Creates the table and imports data from CSV
		static void ImportTableFromCSV(SQLiteTable table)
		{
			CreateTable(table);

			Console.WriteLine($"Import {table.Name} from CSV");

			string path = BuildPaths.GetSqlitePath();
			List<string> args = new List<string>() { BuildPaths.GetDatabasePath(), ".mode csv", $".import {BuildPaths.GetFo76EditOutputPath()}{table.Name}.csv {table.Name}" };

			Process process = Process.Start(path, args);
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

				TransformColumn(UnescapeCharacters, table.Name, field.Name);
			}
		}

		// Loops a table and amends a column according to the value of the other (or same) column, when passed to the method
		static void TransformColumn(Func<string, string?> method, string tableName, string sourceColumn, string? targetColumn = null)
		{
			// It is quite common that we transform a column based on itself.
			// So the targetColumn arg is optional, and when not passed, the sourceColumn is also the target.
			targetColumn ??= sourceColumn;

			string tempIndex = "tempIndex";

			SimpleQuery($"CREATE INDEX {tempIndex} ON {tableName} ({sourceColumn});", true);

			string readQuery = $"SELECT {sourceColumn} FROM {tableName}";
			string updateQuery = $"UPDATE {tableName} SET {targetColumn} = @new WHERE {sourceColumn} = @original";

			SqliteCommand readCommand = new SqliteCommand(readQuery, Connection);
			SqliteDataReader reader = readCommand.ExecuteReader();

			Console.WriteLine($"Transform {tableName}.{sourceColumn} -> {targetColumn}: {method.Method.Name}");

			using (SqliteTransaction transaction = Connection.BeginTransaction())
			using (SqliteCommand updateCommand = new SqliteCommand(updateQuery, Connection, transaction))
			{
				updateCommand.Parameters.AddWithValue("@new", string.Empty);
				updateCommand.Parameters.AddWithValue("@original", string.Empty);

				while (reader.Read())
				{
					string originalValue = reader.GetString(0);
					string? newValue = method(originalValue);

					// If the new value is null (method indicates value should not be changed)
					// Or if the operation would result in changing a column to the value it already is, skip.
					if (newValue == null || (sourceColumn == targetColumn && newValue == originalValue))
					{
						continue;
					}

					updateCommand.Parameters["@new"].Value = newValue;
					updateCommand.Parameters["@original"].Value = originalValue;

					updateCommand.ExecuteNonQuery();
				}

				transaction.Commit();
			}

			SimpleQuery($"DROP INDEX '{tempIndex}'", true);
		}

		// Return the given string with custom escape sequences replaced
		static string UnescapeCharacters(string input)
		{
			return input.Replace(":COMMA:", ",").Replace(":QUOT:", "\"").Replace("''", "'");
		}

		// Converts a valid 8-char hex FormID to the string value of the integer value of itself
		static string CaptureFormID(string input)
		{
			string formid = GetFormIDRegex.Match(input).Groups[1].Value;

			if (string.IsNullOrEmpty(formid))
			{
				return input;
			}

			return Convert.ToInt32(formid, 16).ToString();
		}

		// Removes the signture and formID from the end of a string
		static string RemoveTrailingReference(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}

			return RemoveTrailingReferenceRegex.Match(input).Groups[1].Value;
		}

		// Returns the true display name, from a string which is expected to contain the editorid, displayname, and sig/referenceFormID
		static string CaptureQuotedDisplayName(string displayName)
		{
			// Doesn't look like we need to do anything
			if (!GetFormIDRegex.IsMatch(displayName))
			{
				return displayName;
			}

			return GetDisplayNameRegex.Match(displayName).Groups[1].Value;
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
			File.Delete(BuildPaths.GetDatabasePath());
			File.Delete(BuildPaths.GetDatabasePath() + "-journal");
		}
	}
}
