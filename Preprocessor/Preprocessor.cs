using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using MappalachiaLibrary;
using Microsoft.Data.Sqlite;

namespace Preprocessor
{
	internal class Preprocessor
	{
		static readonly SqliteConnection Connection = GetConnection();
		static Regex SignatureFormIDRegex { get; } = new Regex("\\[[A-Z_]{4}:([0-9A-F]{8})\\]");
		static Regex OptionalSignatureFormIDRegex { get; } = new Regex("(\\[[A-Z_]{4}:)?([0-9A-F]{8})(\\])?");
		static Regex FormIDRegex { get; } = new Regex(".*" + SignatureFormIDRegex + ".*");
		static Regex RemoveTrailingReferenceRegex { get; } = new Regex(@"(.*) " + SignatureFormIDRegex);
		static Regex QuotedTermRegex { get; } = new Regex(".* :QUOT:(.*):QUOT: " + SignatureFormIDRegex);

		// TODO do coords need to be REAL or can we get away with INTEGER?
		static string CoordinateType { get; } = "REAL";

		static void Main()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			Console.WriteLine($"Building Mappalachia database at {BuildPaths.GetDatabasePath()}\n");

			Cleanup();

			SimpleQuery("PRAGMA foreign_keys = 0");

			SimpleQuery("CREATE TABLE Meta (key TEXT PRIMARY KEY, value TEXT);");
			SimpleQuery($"INSERT INTO Meta (key, value) VALUES('GameVersion', '{GetValidatedGameVersion()}');");

			// Create new tables
			SimpleQuery($"CREATE TABLE Entity(entityFormID INTEGER PRIMARY KEY, displayName TEXT, editorID TEXT, signature TEXT, percChanceNone INTEGER);");
			SimpleQuery($"CREATE TABLE Position(spaceFormID INTEGER REFERENCES Space(spaceFormID), referenceFormID TEXT REFERENCES Entity(entityFormID), x {CoordinateType}, y {CoordinateType}, z {CoordinateType}, locationFormID TEXT REFERENCES Location(locationFormID), lockLevel TEXT, primitiveShape TEXT, boundX {CoordinateType}, boundY {CoordinateType}, boundZ {CoordinateType}, rotZ REAL, mapMarkerName TEXT, shortName TEXT);");
			SimpleQuery($"CREATE TABLE Space(spaceFormID INTEGER PRIMARY KEY, spaceEditorID TEXT, spaceDisplayName TEXT, isWorldspace INTEGER);");
			SimpleQuery($"CREATE TABLE Location(locationFormID TEXT, property TEXT, value INTEGER);");
			SimpleQuery($"CREATE TABLE Region(spaceFormID TEXT REFERENCES Space(spaceFormID), regionFormID INTEGER, regionEditorID TEXT, regionIndex INTEGER, coordIndex INTEGER, x {CoordinateType}, y {CoordinateType});");
			SimpleQuery($"CREATE TABLE Scrap(junkFormID INTEGER REFERENCES Entity(entityFormID), component TEXT, componentQuantity TEXT);");
			SimpleQuery($"CREATE TABLE Component(component TEXT PRIMARY KEY, singular INTEGER, rare INTEGER, medium INTEGER, low INTEGER, high INTEGER, bulk INTEGER);");

			ImportTableFromCSV("Entity");
			ImportTableFromCSV("Position");
			ImportTableFromCSV("Space");
			ImportTableFromCSV("Location");
			ImportTableFromCSV("Region");
			ImportTableFromCSV("Scrap");
			ImportTableFromCSV("Component");

			// Pull the MapMarker data into a new table, then make some hardcoded amendments and corrections
			SimpleQuery($"CREATE TABLE MapMarker (spaceFormID INTEGER REFERENCES Space(spaceFormID), x {CoordinateType}, y {CoordinateType}, label TEXT, icon TEXT);");
			SimpleQuery("INSERT INTO MapMarker (spaceFormID, x, y, label, icon) SELECT spaceFormID, x, y, referenceFormID, mapMarkerName FROM Position WHERE mapMarkerName != '';");
			TransformColumn(UnescapeCharacters, "MapMarker", "label");
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

			// Capture and convert to int the locationFormID
			TransformColumn(CaptureFormID, "Position", "locationFormID");
			ChangeColumnType("Position", "locationFormID", "INTEGER");

			// Capture and convert to int the spaceFormID
			TransformColumn(CaptureFormID, "Region", "spaceFormID");
			ChangeColumnType("Region", "spaceFormID", "INTEGER");
			//TODO doing this loses the foreign key

			// Transform the coordinate data to int
			if (CoordinateType == "INTEGER")
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
			TransformColumn(CaptureQuotedTerm, "Entity", "displayName");

			// Discard spaces which are not accessible, and output a list of those
			TransformColumn(UnescapeCharacters, "Space", "spaceDisplayName");
			List<string> deletedRows = SimpleQuery($"DELETE FROM Space WHERE {Hardcodings.DiscardCellsQuery} RETURNING spaceEditorID, spaceDisplayName, spaceFormID, isWorldspace;");
			deletedRows.Sort();
			deletedRows.Insert(0, "spaceEditorID,spaceDisplayName,spaceFormID,isWorldspace");
			File.WriteAllLines(BuildPaths.GetDiscardedCellsPath(), deletedRows);

			// Remove entries which are not referenced by other relevant tables
			SimpleQuery("DELETE FROM Position WHERE spaceFormID NOT IN (SELECT spaceFormID FROM Space);");
			SimpleQuery("DELETE FROM Region WHERE spaceFormID NOT IN (SELECT spaceFormID FROM Space);");
			SimpleQuery("DELETE FROM MapMarker WHERE spaceFormID NOT IN (SELECT spaceFormID FROM Space);");
			SimpleQuery("DELETE FROM Entity WHERE entityFormID NOT IN (SELECT referenceFormID FROM Position);");
			SimpleQuery("DELETE FROM Scrap WHERE junkFormID NOT IN (SELECT entityFormID FROM Entity);");
			SimpleQuery("DELETE FROM Location WHERE locationFormID NOT IN (SELECT locationFormID FROM Position);");

			// Clean up scrap component names
			TransformColumn(CaptureQuotedTerm, "Scrap", "componentQuantity");
			TransformColumn(CaptureQuotedTerm, "Scrap", "component");
			SimpleQuery($"UPDATE Scrap SET componentQuantity = 'Singular' WHERE componentQuantity LIKE '%Singular%'");

			// Transpose the component quantity keywords to numeric values against Scrap table, then drop the Component table
			TransformColumn(GetComponentQuantity, "Scrap", "component", "componentQuantity", "componentQuantity");
			SimpleQuery($"DROP TABLE Component");

			// TODO NPCs

			// Finally unescape chars from columns which we've not touched
			TransformColumn(UnescapeCharacters, "Entity", "displayName");

			// TODO Data validation - positive and negative checks for invalid or bad data

			SimpleQuery("VACUUM;");

			SimpleQuery("PRAGMA foreign_keys = 1");

			Console.WriteLine($"Done. {stopwatch.Elapsed.ToString("m\\m\\ s\\s")}. Press any key");
			Console.ReadKey();
		}

		static SqliteConnection GetConnection()
		{
			SqliteConnection connection = new SqliteConnection("Data Source=" + BuildPaths.GetDatabasePath());
			connection.Open();
			return connection;
		}

		// Executes any query against the open database.
		// Returns output rows as comma-separated strings in a list.
		static List<string> SimpleQuery(string query, bool silent = false, SqliteConnection? connection = null)
		{
			if (!silent)
			{
				Console.WriteLine(query);
			}

			// Calling code can supply their own connection for parallel access, otherwise use the global connection
			connection ??= Connection;

			SqliteDataReader reader = new SqliteCommand(query, connection).ExecuteReader();

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

		static void ImportTableFromCSV(string tableName)
		{
			Console.WriteLine($"Import {tableName} from CSV");

			string path = BuildPaths.GetSqlitePath();
			List<string> args = new List<string>() { BuildPaths.GetDatabasePath(), ".mode csv", $".import {BuildPaths.GetFo76EditOutputPath()}{tableName}.csv {tableName}" };

			Process process = Process.Start(path, args);
			process.WaitForExit();
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

		// Loops a table and amends a column according to the value of 2 columns, when passed to the method
		static void TransformColumn(Func<string, string, string?> method, string tableName, string sourceColumnA, string sourceColumnB, string targetColumn)
		{
			string tempIndex = "tempIndex";

			SimpleQuery($"CREATE INDEX {tempIndex} ON {tableName} ({sourceColumnA}, {sourceColumnB});", true);

			string readQuery = $"SELECT {sourceColumnA}, {sourceColumnB} FROM {tableName}";
			string updateQuery = $"UPDATE {tableName} SET {targetColumn} = @new WHERE {sourceColumnA} = @originalA AND {sourceColumnB} = @originalB";

			SqliteCommand readCommand = new SqliteCommand(readQuery, Connection);
			SqliteDataReader reader = readCommand.ExecuteReader();

			Console.WriteLine($"Transform {tableName}.{sourceColumnA},{sourceColumnB} -> {targetColumn}: {method.Method.Name}");

			using (SqliteTransaction transaction = Connection.BeginTransaction())
			using (SqliteCommand updateCommand = new SqliteCommand(updateQuery, Connection, transaction))
			{
				updateCommand.Parameters.AddWithValue("@new", string.Empty);
				updateCommand.Parameters.AddWithValue("@originalA", string.Empty);
				updateCommand.Parameters.AddWithValue("@originalB", string.Empty);

				while (reader.Read())
				{
					string originalValueA = reader.GetString(0);
					string originalValueB = reader.GetString(1);
					string? newValue = method(originalValueA, originalValueB);

					// If the new value is null (method indicates value should not be changed), skip.
					if (newValue == null)
					{
						continue;
					}

					updateCommand.Parameters["@new"].Value = newValue;
					updateCommand.Parameters["@originalA"].Value = originalValueA;
					updateCommand.Parameters["@originalB"].Value = originalValueB;

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
			string formid = OptionalSignatureFormIDRegex.Match(input).Groups[2].Value;

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
		static string CaptureQuotedTerm(string displayName)
		{
			// Doesn't look like we need to do anything
			if (!FormIDRegex.IsMatch(displayName))
			{
				return displayName;
			}

			return QuotedTermRegex.Match(displayName).Groups[1].Value;
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

		// Returns the numeric quantity of the component for the given quantity name
		static string GetComponentQuantity(string component, string quantity)
		{
			return SimpleQuery($"SELECT \"{quantity}\" FROM Component where component = '{component}'", true, GetConnection()).First();
		}

		// Properly fetches the game version - tries the exe and asks if it was correct, otherwise asks for direct input
		static string GetValidatedGameVersion()
		{
			string? gameVersion = GetGameVersionFromExe();

			if (gameVersion == null)
			{
				Console.Write($"Unable to determine game version from exe at {BuildPaths.GameExePath}.");
				return GetGameVersionFromUser();
			}

			Console.WriteLine($"Is \"{gameVersion}\" the correct game version?\n(Enter y/n)");

			while (true)
			{
				char key = char.ToLower(Console.ReadKey().KeyChar);

				if (key == 'y')
				{
                    Console.WriteLine();
					return gameVersion;
				}
				else if (key == 'n')
				{
					return GetGameVersionFromUser();
				}

				Console.Write("\r \r");
			}
		}

		// Return the game version string on the FO76 exe, assuming it is present else null
		static string? GetGameVersionFromExe()
		{
			return File.Exists(BuildPaths.GameExePath) ? FileVersionInfo.GetVersionInfo(BuildPaths.GameExePath).FileVersion : null;
		}

		// Asks the user to enter game version, and returns the entered line
		static string GetGameVersionFromUser()
		{
			Console.Write("\nPlease enter the correct version string:");
			return Console.ReadLine() ?? string.Empty;
		}

		// Removes old DB files
		static void Cleanup()
		{
			File.Delete(BuildPaths.GetDatabasePath());
			File.Delete(BuildPaths.GetDatabasePath() + "-journal");
		}
	}
}
