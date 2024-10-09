using System.Diagnostics;
using System.Text.RegularExpressions;
using Library;
using Microsoft.Data.Sqlite;

namespace Preprocessor
{
	internal partial class Preprocessor
	{
		enum ColumnType
		{
			TEXT,
			REAL,
			INTEGER,
			BOOL,
		}

		static readonly SqliteConnection Connection = GetConnection();

		// TODO do coords need to be REAL or can we get away with INTEGER?
		static ColumnType CoordinateType { get; } = ColumnType.REAL;

		static void Main()
		{
			Console.Title = "Mappalachia Preprocessor";

			Stopwatch stopwatch = new Stopwatch();

			// If the database already exists, we may only want to run validations, so we give some options
			if (File.Exists(BuildPaths.DatabasePath))
			{
				Console.WriteLine("Enter:" +
					"\n1:Build and preprocess database, then run full validation suite" +
					"\n2:Run both validations without building" +
					"\n3:Run data validation only" +
					"\n4:Run image asset validation only");

				char input = Console.ReadKey().KeyChar;
				Console.WriteLine();

				stopwatch.Start();

				switch (input)
				{
					case '1':
						Preprocess();
						break;
					case '2':
						ValidateDatabase();
						ValidateImageAssets();
						break;
					case '3':
						ValidateDatabase();
						break;
					case '4':
						ValidateImageAssets();
						break;

					default:
						throw new Exception($"Not a valid selection: {input}");
				}

				ConcludeValidation();
			}

			// If the database doesn't exist yet, we do the full build and validate
			else
			{
				stopwatch.Start();
				Preprocess();
			}

			Console.WriteLine($"Finished. {stopwatch.Elapsed.ToString(@"m\m\ s\s")}. Press any key");
			Console.ReadKey();
		}

		static void Preprocess()
		{
			string gameVersion = GetValidatedGameVersion();

			Console.WriteLine($"Building Mappalachia database at {BuildPaths.DatabasePath}\n");

			Cleanup();
			SimpleQuery("PRAGMA foreign_keys = 0");

			// Create the Meta table, add the game version to it
			SimpleQuery("CREATE TABLE Meta (key TEXT PRIMARY KEY, value TEXT);");
			SimpleQuery($"INSERT INTO Meta (key, value) VALUES('GameVersion', '{gameVersion}');");

			// Create new tables
			SimpleQuery($"CREATE TABLE Entity(entityFormID INTEGER PRIMARY KEY, displayName TEXT, editorID TEXT, signature TEXT, percChanceNone INTEGER);");
			SimpleQuery($"CREATE TABLE Position(spaceFormID INTEGER REFERENCES Space(spaceFormID), referenceFormID TEXT REFERENCES Entity(entityFormID), x {CoordinateType}, y {CoordinateType}, z {CoordinateType}, locationFormID TEXT REFERENCES Location(locationFormID), lockLevel TEXT, primitiveShape TEXT, boundX {CoordinateType}, boundY {CoordinateType}, boundZ {CoordinateType}, rotZ REAL, mapMarkerName TEXT, shortName TEXT, teleportsToFormID TEXT);");
			SimpleQuery($"CREATE TABLE Space(spaceFormID INTEGER PRIMARY KEY, spaceEditorID TEXT, spaceDisplayName TEXT, isWorldspace INTEGER);");
			SimpleQuery($"CREATE TABLE Location(locationFormID INTEGER, property TEXT, value INTEGER);");
			SimpleQuery($"CREATE TABLE Region(spaceFormID TEXT REFERENCES Space(spaceFormID), regionFormID INTEGER, regionEditorID TEXT, regionIndex INTEGER, coordIndex INTEGER, x {CoordinateType}, y {CoordinateType});");
			SimpleQuery($"CREATE TABLE Scrap(junkFormID INTEGER REFERENCES Entity(entityFormID), component TEXT, componentQuantity TEXT);");
			SimpleQuery($"CREATE TABLE Component(component TEXT PRIMARY KEY, singular INTEGER, rare INTEGER, medium INTEGER, low INTEGER, high INTEGER, bulk INTEGER);");

			// Import to tables from xedit exports
			ImportTableFromCSV("Entity");
			ImportTableFromCSV("Position");
			ImportTableFromCSV("Space");
			ImportTableFromCSV("Location");
			ImportTableFromCSV("Region");
			ImportTableFromCSV("Scrap");
			ImportTableFromCSV("Component");

			// Pull the MapMarker data into a new table, then make some hardcoded amendments and corrections
			SimpleQuery("CREATE TABLE MapMarker AS SELECT spaceFormID, x, y, referenceFormID as label, mapMarkerName as icon FROM Position WHERE mapMarkerName != '';");
			TransformColumn(UnescapeCharacters, "MapMarker", "label");
			SimpleQuery($"DELETE FROM MapMarker WHERE label IN ({string.Join(",", MapMarkersToRemove.Select(m => "\'" + m + "\'"))});");
			SimpleQuery(AddMissingMarkersQuery);
			SimpleQuery(CorrectDuplicateMarkersQuery);
			TransformColumn(CorrectLabelsByDict, "MapMarker", "label");
			TransformColumn(CorrectFissureLabels, "MapMarker", "label");
			TransformColumn(CorrectCommonBadLabels, "MapMarker", "label");
			TransformColumn(GetCorrectedMarkerIcon, "MapMarker", "label", "icon");
			AddForeignKey("MapMarker", "spaceFormID", "INTEGER", "Space", "spaceFormID");

			// Remove map marker remnants from Position table
			SimpleQuery("DELETE FROM Position WHERE mapMarkerName != '';");
			SimpleQuery("ALTER TABLE Position DROP COLUMN mapMarkerName;");

			// Remove some junk data from Position
			SimpleQuery("DELETE FROM Position WHERE shortName LIKE '%CELL:%';");

			// Capture and convert to int the referenceFormID on Position
			TransformColumn(CaptureFormID, "Position", "referenceFormID");
			ChangeColumnType("Position", "referenceFormID", "INTEGER");
			AddForeignKey("Position", "referenceFormID", "INTEGER", "Entity", "entityFormID");

			// Capture and convert to int the locationFormID on Position
			TransformColumn(CaptureFormID, "Position", "locationFormID");
			ChangeColumnType("Position", "locationFormID", "INTEGER");

			// Capture and convert to int the Space FormID of the teleportsToFormID on Position
			TransformColumn(CaptureSpaceFormID, "Position", "teleportsToFormID");
			ChangeColumnType("Position", "teleportsToFormID", "INTEGER");
			AddForeignKey("Position", "teleportsToFormID", "INTEGER", "Space", "spaceFormID");

			// Capture and convert to int the spaceFormID on Region
			TransformColumn(CaptureFormID, "Region", "spaceFormID");
			ChangeColumnType("Region", "spaceFormID", "INTEGER");
			AddForeignKey("Region", "spaceFormID", "INTEGER", "Space", "spaceFormID");

			// Transform the coordinate data to int
			if (CoordinateType == ColumnType.INTEGER)
			{
				TransformColumn(RealToInt, "Position", "x");
				TransformColumn(RealToInt, "Position", "y");
				TransformColumn(RealToInt, "Position", "z");
				TransformColumn(RealToInt, "Position", "boundX");
				TransformColumn(RealToInt, "Position", "boundY");
				TransformColumn(RealToInt, "Position", "boundZ");
				TransformColumn(RealToInt, "Region", "x");
				TransformColumn(RealToInt, "Region", "y");
				TransformColumn(RealToInt, "MapMarker", "x");
				TransformColumn(RealToInt, "MapMarker", "y");
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
			List<string> deletedRows = SimpleQuery($"DELETE FROM Space WHERE {DiscardCellsQuery} RETURNING spaceFormID, spaceEditorID, spaceDisplayName, isWorldspace;");
			deletedRows.Sort();
			deletedRows.Insert(0, "spaceFormID,spaceDisplayName,spaceEditorID,isWorldspace");
			File.WriteAllLines(BuildPaths.DiscardedCellsPath, deletedRows);

			// Create a replacement copy of Space, adding the min/max/mid of x/y
			SimpleQuery($"CREATE TABLE TempSpace(spaceFormID INTEGER PRIMARY KEY, spaceEditorID TEXT, spaceDisplayName TEXT, isWorldspace INTEGER, minX {CoordinateType}, maxX {CoordinateType}, midX {CoordinateType}, minY {CoordinateType}, maxY {CoordinateType}, midY {CoordinateType});");
			SimpleQuery("INSERT INTO TempSpace (spaceFormID, spaceEditorID, spaceDisplayName, isWorldspace, minX, maxX, midX, minY, maxY, midY) SELECT Space.spaceFormID, spaceEditorID, spaceDisplayName, isWorldspace, min(x), max(x), ((min(x) + max(x)) / 2), min(y), max(y), ((min(y) + max(y)) / 2) FROM Space JOIN Position ON Space.spaceFormID = Position.spaceFormID GROUP BY Space.spaceFormID;");
			SimpleQuery("DROP TABLE Space;");
			SimpleQuery("ALTER TABLE TempSpace RENAME TO Space;");

			if (CoordinateType == ColumnType.INTEGER)
			{
				TransformColumn(RealToInt, "Space", "midX");
				TransformColumn(RealToInt, "Space", "midY");
			}

			// Remove entries which are not referenced by other relevant tables (orphaned records)
			SimpleQuery("DELETE FROM Position WHERE spaceFormID NOT IN (SELECT spaceFormID FROM Space);");
			SimpleQuery("DELETE FROM Region WHERE spaceFormID NOT IN (SELECT spaceFormID FROM Space);");
			SimpleQuery("DELETE FROM MapMarker WHERE spaceFormID NOT IN (SELECT spaceFormID FROM Space);");
			SimpleQuery("DELETE FROM Entity WHERE entityFormID NOT IN (SELECT referenceFormID FROM Position);");
			SimpleQuery("DELETE FROM Location WHERE locationFormID NOT IN (SELECT locationFormID FROM Position);");
			SimpleQuery("DELETE FROM Scrap WHERE junkFormID NOT IN (SELECT entityFormID FROM Entity);");

			// Clean up scrap and component names
			TransformColumn(CaptureQuotedTerm, "Scrap", "componentQuantity");
			TransformColumn(CaptureQuotedTerm, "Scrap", "component");
			SimpleQuery($"UPDATE Scrap SET componentQuantity = 'Singular' WHERE componentQuantity LIKE '%Singular%'");

			// Fix erroneous data which is exported from xEdit with values somehow misaligned from in-game
			SimpleQuery(CorrectLockLevelQuery);
			SimpleQuery(CorrectPrimitiveShapeQuery);

			TransformColumn(ReduceLockLevel, "Position", "lockLevel");

			// Transform the component quantity keywords to numeric values from Scrap table, then drop the Component table
			TransformColumn(GetComponentQuantity, "Scrap", "component", "componentQuantity", "componentQuantity");
			SimpleQuery($"DROP TABLE Component;");

			// Extract the NPC types and classes on Location from the raw 'property'
			SimpleQuery("ALTER TABLE Location ADD COLUMN npcName TEXT;");
			SimpleQuery("ALTER TABLE Location ADD COLUMN npcClass TEXT;");
			TransformColumn(GetNPCName, "Location", "property", "npcName");
			TransformColumn(GetNPCClass, "Location", "property", "npcClass");
			SimpleQuery("ALTER TABLE Location DROP COLUMN property;");
			SimpleQuery("DELETE FROM Location WHERE npcName = '' OR npcClass = '';");

			// Extract the independent NPC spawn chances, by summing the spawn pool and weights from each location
			SimpleQuery("ALTER TABLE Location ADD COLUMN sumWeight INTEGER;");
			TransformColumn(GetSumNPCSpawnWeight, "Location", "locationFormID", "npcClass", "sumWeight");
			SimpleQuery("ALTER TABLE Location ADD COLUMN spawnWeight REAL;");
			TransformColumn(DivideString, "Location", "value", "sumWeight", "spawnWeight"); // Set the value of 'spawnWeight' with value/sum.
			SimpleQuery("ALTER TABLE Location DROP COLUMN sumWeight;");
			SimpleQuery("ALTER TABLE Location DROP COLUMN value;");
			SimpleQuery("DELETE FROM Location WHERE spawnWeight = 0;");

			// Unescape chars from columns which we've not otherwise touched
			TransformColumn(UnescapeCharacters, "Entity", "displayName");

			// Create a reduced semi-redundant copy of the Position table, which already has all the possible search results grouped and counted
			// We can use this later to conduct the searches much faster, but the Position table remains to get non-grouped coordinates
			SimpleQuery("CREATE TABLE Position_PreGrouped (spaceFormID INTEGER REFERENCES Space(spaceFormID), referenceFormID INTEGER REFERENCES Entity(EntityFormID), lockLevel TEXT, label TEXT, count INTEGER);");
			SimpleQuery("INSERT INTO Position_PreGrouped (spaceFormID, referenceFormID, lockLevel, label, count) SELECT spaceFormID, referenceFormId, lockLevel, label, COUNT(*) as count FROM Position GROUP BY referenceFormID, label, spaceFormID, lockLevel;");

			// Create indexes
			SimpleQuery("CREATE INDEX indexStandard ON Position(referenceFormID, lockLevel, label, spaceFormID);");
			SimpleQuery("CREATE INDEX indexInstance ON Position(instanceFormID);");
			SimpleQuery("CREATE INDEX indexLocation ON Position(locationFormID);");
			SimpleQuery("CREATE INDEX indexSpace ON Position_PreGrouped(spaceFormID);");

			// Final steps, wrap-up, optimizations, etc
			SimpleQuery("PRAGMA foreign_keys = 1;");
			SimpleQuery("ANALYZE;");
			SimpleQuery("PRAGMA optimize;");
			SimpleQuery("VACUUM;");
			SimpleQuery("PRAGMA query_only;");

			Console.WriteLine($"Build and Preprocess Done.\n");

			ValidateDatabase();
			ValidateImageAssets();
		}

		static SqliteConnection GetConnection()
		{
			SqliteConnection connection = new SqliteConnection("Data Source=" + BuildPaths.DatabasePath);
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

		// Adds a foreign key to a column which already exists
		static void AddForeignKey(string table, string column, string columnType, string foreignTable, string foreignColumn)
		{
			string tempColumn = "temp";

			Console.WriteLine($"Re-add foreign key: {table}.{column}:{foreignTable}.{foreignColumn}");

			SimpleQuery($"ALTER TABLE {table} ADD COLUMN {tempColumn} {columnType} REFERENCES {foreignTable}({foreignColumn});", true); // Create a temp column with the foreign key
			SimpleQuery($"UPDATE {table} SET {tempColumn} = {column};", true); // Copy the source column into the temp
			SimpleQuery($"ALTER TABLE {table} DROP COLUMN {column};", true); // Drop the original
			SimpleQuery($"ALTER TABLE {table} RENAME COLUMN {tempColumn} TO {column};", true); // Rename temp column to original
		}

		static void ImportTableFromCSV(string tableName)
		{
			Console.WriteLine($"Import {tableName} from CSV");

			string path = BuildPaths.SqlitePath;
			List<string> args = new List<string>() { BuildPaths.DatabasePath, ".mode csv", $".import {BuildPaths.Fo76EditOutputPath}{tableName}.csv {tableName}" };

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

		// Captures the FormID specifically from the present [CELL/WRLD:x] phrase, to the string value of the integer value of itself
		static string CaptureSpaceFormID(string input)
		{
			string formid = SpaceFormIDRegex.Match(input).Groups[2].Value;

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

		// Returns the total spawn weight of spawn pool for the class at the location
		static string GetSumNPCSpawnWeight(string locationFormID, string npcClass)
		{
			return SimpleQuery($"SELECT sum(value) FROM Location WHERE locationFormID = {locationFormID} and npcClass = '{npcClass}'", true, GetConnection()).First();
		}

		static string GetNPCName(string value)
		{
			// Doesn't look like we need to do anything
			if (!NPCRegex.IsMatch(value))
			{
				return string.Empty;
			}

			// Extract only the part containing the NPC name
			string name = NPCRegex.Match(value).Groups[2].Value;

			// Add a space if it looks like it needs it
			if (TitleCaseAddSpaceRegex.IsMatch(name))
			{
				GroupCollection matchesForSpace = TitleCaseAddSpaceRegex.Match(name).Groups;
				name = matchesForSpace[1].Value + " " + matchesForSpace[2].Value;
			}

			// Refer to the hardcodings replacement dictionary
			if (NPCNameCorrection.TryGetValue(name, out string? correction))
			{
				name = correction;
			}

			return name;
		}

		static string GetNPCClass(string value)
		{
			// Doesn't look like we need to do anything
			if (!NPCRegex.IsMatch(value))
			{
				return string.Empty;
			}

			return NPCRegex.Match(value).Groups[1].Value;
		}

		// Returns the corrected label for the given map marker label
		static string? CorrectLabelsByDict(string label)
		{
			if (MarkerLabelCorrection.TryGetValue(label, out string? correctedLabel))
			{
				return correctedLabel;
			}

			return label;
		}

		// Returns num/denom
		static string DivideString(string num, string denom)
		{
			return (double.Parse(num) / double.Parse(denom)).ToString();
		}

		static string ReduceLockLevel(string lockLevel)
		{
			// If the lock level does not need changing
			if (!CorrectLockLevelRegex.IsMatch(lockLevel))
			{
				return lockLevel;
			}

			return CorrectLockLevelRegex.Match(lockLevel).Groups[2].Value;
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
			File.Delete(BuildPaths.DatabasePath);
			File.Delete(BuildPaths.DatabasePath + "-journal");
		}
	}
}
