using System.Diagnostics;
using System.Text.RegularExpressions;
using Library;
using Microsoft.Data.Sqlite;
using static Library.BuildTools;
using static Library.Hardcodings;

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

		static SqliteConnection Connection { get; } = GetNewConnection();

		static List<string> SummaryReport { get; } = new List<string>();

		static int CellPadding { get; } = 300; // Number of game units which we expand the Cell's maxRange value, above the initial crop

		static void Main()
		{
			Console.Title = "Mappalachia Preprocessor";

			Stopwatch stopwatch = new Stopwatch();

			// If the database already exists, we may only want to run validations, so we give some options
			if (File.Exists(DatabasePath))
			{
				StdOutWithColor(
					"Enter:" +
					"\n1:Build and preprocess database, then run full validation suite" +
					"\n2:Run both validations without building" +
					"\n3:Run data validation only" +
					"\n4:Run image asset validation only",
					ColorQuestion);

				char input = Console.ReadKey().KeyChar;
				Console.WriteLine();

				stopwatch.Start();

				switch (input)
				{
					case '1':
						Preprocess();
						ValidateDatabase();
						ValidateImageAssets();
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

			StdOutWithColor($"Finished. {stopwatch.Elapsed.ToString(@"m\m\ s\s")}. Press any key", ColorInfo);
			Console.ReadKey();
		}

		static void Preprocess()
		{
			string gameVersion = GetValidatedGameVersion();

			StdOutWithColor($"Building Mappalachia database at {DatabasePath}\n", ColorInfo);

			Connection.Close();
			Cleanup();
			Connection.Open();

			SimpleQuery("PRAGMA foreign_keys = 0");

			// Create the Meta table, add the game version to it
			SimpleQuery("CREATE TABLE Meta (key TEXT PRIMARY KEY, value TEXT);");
			SimpleQuery($"INSERT INTO Meta (key, value) VALUES('GameVersion', '{gameVersion}');");

			// Create new tables
			SimpleQuery($"CREATE TABLE Entity(entityFormID INTEGER PRIMARY KEY, displayName TEXT, editorID TEXT, signature TEXT, percChanceNone INTEGER);");
			SimpleQuery($"CREATE TABLE Position(spaceFormID INTEGER REFERENCES Space(spaceFormID), referenceFormID TEXT REFERENCES Entity(entityFormID), x REAL, y REAL, z REAL, locationFormID TEXT REFERENCES Location(locationFormID), lockLevel TEXT, primitiveShape TEXT, boundX REAL, boundY REAL, boundZ REAL, rotZ REAL, mapMarkerName TEXT, shortName TEXT, teleportsToFormID TEXT);");
			SimpleQuery($"CREATE TABLE Space(spaceFormID INTEGER PRIMARY KEY, spaceEditorID TEXT, spaceDisplayName TEXT, isWorldspace INTEGER);");
			SimpleQuery($"CREATE TABLE Location(locationFormID INTEGER, property TEXT, value INTEGER);");
			SimpleQuery($"CREATE TABLE Region(spaceFormID TEXT REFERENCES Space(spaceFormID), regionFormID INTEGER, regionEditorID TEXT, regionIndex INTEGER, coordIndex INTEGER, x REAL, y REAL);");
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
			SimpleQuery($"DELETE FROM MapMarker WHERE label IN {MapMarkersToRemove.ToSqliteCollection()};");
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
			TransformColumn(CaptureSpaceFormID, "Region", "spaceFormID");
			ChangeColumnType("Region", "spaceFormID", "INTEGER");
			AddForeignKey("Region", "spaceFormID", "INTEGER", "Space", "spaceFormID");

			// Discard spaces which are not accessible, and output a list of those
			TransformColumn(UnescapeCharacters, "Space", "spaceDisplayName");
			List<string> deletedRows = SimpleQuery($"DELETE FROM Space WHERE {DiscardCellsQuery} RETURNING spaceFormID, spaceEditorID, spaceDisplayName, isWorldspace;");
			deletedRows.Sort();
			deletedRows.Insert(0, "spaceFormID,spaceDisplayName,spaceEditorID,isWorldspace");
			File.WriteAllLines(DiscardedCellsPath, deletedRows);

			// Create a replacement copy of Space, adding a temporary maximum estimate for the center x/y and max 2d range
			SimpleQuery($"CREATE TABLE TempSpace(spaceFormID INTEGER PRIMARY KEY, spaceEditorID TEXT, spaceDisplayName TEXT, isWorldspace INTEGER, centerX REAL, centerY REAL, maxRange REAL);");
			SimpleQuery("INSERT INTO TempSpace (spaceFormID, spaceEditorID, spaceDisplayName, isWorldspace, centerX, centerY, maxRange) SELECT Space.spaceFormID, spaceEditorID, spaceDisplayName, isWorldspace, (MIN(x) + MAX(x)) / 2, (MIN(y) + MAX(y)) / 2, MAX(ABS(MIN(x) - MAX(x)), ABS(MIN(y) - MAX(y))) FROM Space JOIN Position ON Space.spaceFormID = Position.spaceFormID GROUP BY Space.spaceFormID;");
			SimpleQuery("DROP TABLE Space;");
			SimpleQuery("ALTER TABLE TempSpace RENAME TO Space;");

			// Read in scale/offset corrections for Spaces
			foreach (string file in Directory.GetFiles(CellXYScaleCorrectionPath))
			{
				string[] values = File.ReadAllLines(file);

				double centerX = double.Parse(values[0]);
				double centerY = double.Parse(values[1]);
				double maxRange = double.Parse(values[2]);

				SimpleQuery($"UPDATE Space SET centerX = {centerX}, centerY = {centerY}, maxRange = {maxRange} WHERE spaceEditorID = '{Path.GetFileNameWithoutExtension(file)}'");
			}

			// For spaces which are sister spaces, we copy the scaling of the first onto the others
			foreach (List<string> spaceCollection in SisterSpaces)
			{
				string parentEditorID = spaceCollection.First();
				Space? parent = CommonDatabase.GetSpaceByEditorID(Connection, parentEditorID) ?? throw new Exception($"Unable to find Space {parentEditorID}");

				foreach (string childEditorID in spaceCollection.Skip(1))
				{
					SimpleQuery($"UPDATE Space SET centerX = {parent.CenterX}, centerY = {parent.CenterY}, maxRange = {parent.MaxRange} WHERE spaceEditorID = '{childEditorID}'");
				}
			}

			// Add the cell padding
			SimpleQuery($"UPDATE Space SET maxRange = maxRange + {CellPadding} WHERE isWorldspace = 0");

			// Capture label and instanceFormID values from shortName column, splitting them into their own columns and dropping the original
			SimpleQuery("ALTER TABLE Position ADD COLUMN 'label' TEXT;");
			SimpleQuery("ALTER TABLE Position ADD COLUMN 'instanceFormID' INTEGER;");
			TransformColumn(CaptureFormID, "Position", "shortName", "instanceFormID");
			TransformColumn(RemoveTrailingReference, "Position", "shortName", "label");
			SimpleQuery("ALTER TABLE Position DROP COLUMN 'shortName';");

			// Ensure all Entities have the proper display name only, and nothing extraneous
			TransformColumn(CaptureQuotedTerm, "Entity", "displayName");

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

			GenerateSummary();

			StdOutWithColor($"Build and Preprocess Done.\n", ColorInfo);
		}

		static void GenerateSummary()
		{
			StdOutWithColor($"\nGenerating Summary Report at {DatabaseSummaryPath}\n", ColorInfo);

			Connection.Close();
			AddToSummaryReport("MD5 Checksum", GetMD5Hash(DatabasePath));
			Connection.Open();

			AddToSummaryReport("Size", (new FileInfo(DatabasePath).Length / BuildTools.Kilobyte).ToString() + " KB");
			AddToSummaryReport("Built At UTC", DateTime.UtcNow.ToString());
			AddToSummaryReport("CSV Imported with SQLite Version", SqliteTools("--version"));
			AddToSummaryReport("Tables", SqliteTools(DatabasePath + " .tables"));
			AddToSummaryReport("Indices", SqliteTools(DatabasePath + " .indices"));
			AddToSummaryReport("Game Version", CommonDatabase.GetGameVersion(Connection));
			AddToSummaryReport("Spaces", SimpleQuery("SELECT spaceEditorID, spaceDisplayName, spaceFormID, isWorldspace, centerX, centerY, maxRange FROM Space ORDER BY isWorldspace DESC, spaceEditorID ASC"));
			AddToSummaryReport("Avg X/Y/Z", SimpleQuery("SELECT AVG(x), AVG(y), AVG(z) FROM Position;"));
			AddToSummaryReport("Avg Bounds X/Y/Z", SimpleQuery("SELECT AVG(boundX), AVG(boundY), AVG(boundZ) FROM Position;"));
			AddToSummaryReport("Avg CenterX, CenterY", SimpleQuery("SELECT AVG(centerX), AVG(centerY) FROM Space;"));
			AddToSummaryReport("Avg Rotation", SimpleQuery("SELECT AVG(rotZ) FROM Position;"));
			AddToSummaryReport("Avg PercChanceNone", SimpleQuery("SELECT AVG(percChanceNone) FROM Entity;"));
			AddToSummaryReport("Lock Levels", SimpleQuery("SELECT lockLevel, COUNT(lockLevel) FROM Position GROUP BY lockLevel;"));
			AddToSummaryReport("Primitive Shapes", SimpleQuery("SELECT primitiveShape, COUNT(primitiveShape) FROM Position GROUP BY primitiveShape;"));
			AddToSummaryReport("Entity Category Count", SimpleQuery("SELECT signature, COUNT(signature) FROM Entity GROUP BY signature;"));
			AddToSummaryReport("Position PreGrouped Count", SimpleQuery("SELECT COUNT(*) FROM Position_PreGrouped;"));
			AddToSummaryReport("Avg Num Instances per Reference", SimpleQuery("SELECT AVG(count) FROM Position_PreGrouped;"));
			AddToSummaryReport("X-Table Entity Sum", $"{SimpleQuery("SELECT COUNT(DISTINCT referenceFormID) FROM Position;").First()} = {SimpleQuery("SELECT count(DISTINCT referenceFormID) FROM Position_PreGrouped;").First()} = {SimpleQuery("SELECT count(DISTINCT entityFormID) FROM Entity;").First()}");
			AddToSummaryReport("Avg Length Entity DisplayName", SimpleQuery("SELECT AVG(length) FROM (SELECT LENGTH(displayName) AS length FROM Entity);"));
			AddToSummaryReport("Avg Length Entity EditorID", SimpleQuery("SELECT AVG(length) FROM (SELECT LENGTH(editorID) AS length FROM Entity);"));
			AddToSummaryReport("Avg Length Space DisplayName", SimpleQuery("SELECT AVG(length) FROM (SELECT LENGTH(spaceDisplayName) AS length FROM Space);"));
			AddToSummaryReport("Avg Length Space EditorID", SimpleQuery("SELECT AVG(length) FROM (SELECT LENGTH(spaceEditorID) AS length FROM Space);"));
			AddToSummaryReport("Avg Length Region EditorID", SimpleQuery("SELECT AVG(length) FROM (SELECT LENGTH(regionEditorID) AS length FROM Region);"));
			AddToSummaryReport("Avg Length Label", SimpleQuery("SELECT AVG(length) FROM (SELECT LENGTH(label) AS length FROM Position);"));
			AddToSummaryReport("Avg Count Regions, Coords from Region", SimpleQuery("SELECT AVG(regionMax), AVG(coordMax) FROM (SELECT MAX(regionIndex) as regionMax, MAX(coordIndex) as coordMax FROM Region GROUP BY regionEditorID);"));
			AddToSummaryReport("Scrap with Avg component qty", SimpleQuery("SELECT component, AVG(componentQuantity) FROM Scrap GROUP BY component;"));
			AddToSummaryReport("Avg Map Marker X/Y", SimpleQuery("SELECT AVG(x), AVG(y) FROM MapMarker;"));
			AddToSummaryReport("Map Markers", SimpleQuery("SELECT spaceEditorID, icon, label FROM MapMarker INNER JOIN Space ON MapMarker.spaceFormID = Space.spaceFormID ORDER BY spaceEditorID ASC, icon ASC, label ASC;"));
			AddToSummaryReport("Unique LCTN", SimpleQuery("SELECT COUNT(DISTINCT locationFormID) FROM Location;"));
			AddToSummaryReport("NPCs by Space, Class, and Weight", SimpleQuery(
				"SELECT Space.spaceDisplayName, npcClass, npcName, AVG(spawnWeight) FROM Position " +
				"JOIN Entity ON Entity.entityFormID = Position.referenceFormID " +
				"JOIN Location ON Position.locationFormID = Location.LocationFormID " +
				"JOIN Space ON Space.spaceFormID = Position.spaceFormID " +
				"WHERE ((Entity.editorID LIKE 'LvlSub%' AND Location.npcClass = 'Sub') OR " +
				"(Entity.editorID LIKE 'LvlMain%' AND Location.npcClass = 'Main') OR " +
				"(Entity.editorID LIKE 'LvlCritterA%' AND Location.npcClass = 'CritterA') OR " +
				"(Entity.editorID LIKE 'LvlCritterB%' AND Location.npcClass = 'CritterB')) AND Entity.editorID NOT LIKE '%Turret%' " +
				"GROUP BY npcName, npcClass, spaceDisplayName " +
				"ORDER BY isWorldspace DESC, spaceDisplayName, npcClass, npcName;"));
			AddToSummaryReport("Worldspaces", SimpleQuery("SELECT spaceFormID, spaceEditorID, spaceDisplayName FROM Space WHERE isWorldspace = 1;"));
			AddToSummaryReport("AVG spaceFormID as Dec", SimpleQuery("SELECT AVG(spaceFormID) FROM Space;"));
			AddToSummaryReport("AVG entityFormID as Dec", SimpleQuery("SELECT AVG(entityFormID) FROM Entity;"));
			AddToSummaryReport("AVG locationFormID as Dec", SimpleQuery("SELECT AVG(locationFormID) FROM Location;"));
			AddToSummaryReport("AVG MapMarker spaceFormID as Dec", SimpleQuery("SELECT AVG(spaceFormID) FROM MapMarker;"));
			AddToSummaryReport("AVG Psn spaceFormID as Dec", SimpleQuery("SELECT AVG(spaceFormID) FROM Position;"));
			AddToSummaryReport("AVG Psn referenceFormID as Dec", SimpleQuery("SELECT AVG(referenceFormID) FROM Position;"));
			AddToSummaryReport("AVG Psn instanceFormID as Dec", SimpleQuery("SELECT AVG(instanceFormID) FROM Position;"));
			AddToSummaryReport("AVG Psn teleportsToFormID as Dec", SimpleQuery("SELECT AVG(teleportsToFormID) FROM Position;"));
			AddToSummaryReport("AVG Psn locationFormID as Dec", SimpleQuery("SELECT AVG(locationFormID) FROM Position;"));
			AddToSummaryReport("AVG regionFormID as Dec", SimpleQuery("SELECT AVG(regionFormID) FROM Region;"));
			AddToSummaryReport("AVG region spaceFormID as Dec", SimpleQuery("SELECT AVG(spaceFormID) FROM Region;"));
			AddToSummaryReport("AVG junkFormID as Dec", SimpleQuery("SELECT AVG(junkFormID) FROM Scrap;"));
			AddToSummaryReport("AVG X, Y per Space", SimpleQuery("SELECT spaceEditorID, AVG(x), AVG(y) FROM Space JOIN Position ON Position.spaceFormID = Space.spaceFormID GROUP BY Space.spaceFormID ORDER BY isWorldspace DESC, space.spaceEditorID ASC;"));

			List<string> spaceExterns = new List<string>();
			List<string> spaceChecksums = new List<string>();
			foreach (Space space in CommonDatabase.GetSpaces(Connection))
			{
				List<Instance> coordinates = CommonDatabase.GetInstancesFromSpace(Connection, space);
				double maxRadius = space.MaxRange / 2d;

				// Find the count of coordinates in the space which are further from the center than the max range
				int outlierSum = coordinates.Where(c =>
					Math.Abs(space.CenterX - c.X) > maxRadius ||
					Math.Abs(space.CenterY - c.Y) > maxRadius).Count();

				spaceExterns.Add($"{space.EditorID}:{outlierSum}");
				spaceChecksums.Add($"{space.EditorID}:{coordinates.GetHashCode()}");
			}

			AddToSummaryReport("Entities outside of space range", string.Join("\n", spaceExterns));
			AddToSummaryReport("Space Coordinate Checksums", string.Join("\n", spaceChecksums));

			File.WriteAllLines(DatabaseSummaryPath, SummaryReport);
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

			string path = SqlitePath;
			List<string> args = new List<string>() { DatabasePath, ".mode csv", $".import {Fo76EditOutputPath}{tableName}.csv {tableName}" };

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

			string readQuery = $"SELECT {sourceColumn}, ROWID FROM {tableName}";
			string updateQuery = $"UPDATE {tableName} SET {targetColumn} = @new WHERE ROWID = @rowID";

			SqliteCommand readCommand = new SqliteCommand(readQuery, Connection);
			SqliteDataReader reader = readCommand.ExecuteReader();

			Console.WriteLine($"Transform {tableName}.{sourceColumn} -> {targetColumn}: {method.Method.Name}");

			using (SqliteTransaction transaction = Connection.BeginTransaction())
			using (SqliteCommand updateCommand = new SqliteCommand(updateQuery, Connection, transaction))
			{
				updateCommand.Parameters.AddWithValue("@new", string.Empty);
				updateCommand.Parameters.AddWithValue("@rowID", string.Empty);

				while (reader.Read())
				{
					string originalValue = reader.GetString(0);
					int rowID = reader.GetInt32(1);
					string? newValue = method(originalValue);

					// If the new value is null (method indicates value should not be changed), skip
					if (newValue == null)
					{
						continue;
					}

					updateCommand.Parameters["@new"].Value = newValue;
					updateCommand.Parameters["@rowID"].Value = rowID;

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

			string readQuery = $"SELECT {sourceColumnA}, {sourceColumnB}, ROWID FROM {tableName}";
			string updateQuery = $"UPDATE {tableName} SET {targetColumn} = @new WHERE ROWID = @rowID";

			SqliteCommand readCommand = new SqliteCommand(readQuery, Connection);
			SqliteDataReader reader = readCommand.ExecuteReader();

			Console.WriteLine($"Transform {tableName}.{sourceColumnA},{sourceColumnB} -> {targetColumn}: {method.Method.Name}");

			using (SqliteTransaction transaction = Connection.BeginTransaction())
			using (SqliteCommand updateCommand = new SqliteCommand(updateQuery, Connection, transaction))
			{
				updateCommand.Parameters.AddWithValue("@new", string.Empty);
				updateCommand.Parameters.AddWithValue("@rowID", string.Empty);

				while (reader.Read())
				{
					string originalValueA = reader.GetString(0);
					string originalValueB = reader.GetString(1);
					int rowID = reader.GetInt32(2);
					string? newValue = method(originalValueA, originalValueB);

					// If the new value is null (method indicates value should not be changed), skip.
					if (newValue == null)
					{
						continue;
					}

					updateCommand.Parameters["@new"].Value = newValue;
					updateCommand.Parameters["@rowID"].Value = rowID;

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
			if (string.IsNullOrWhiteSpace(input))
			{
				return input;
			}

			string formid;

			// Try and extract the formid from its normal/proper presentation
			if (SignatureFormIDRegex.IsMatch(input))
			{
				formid = SignatureFormIDRegex.Match(input).Groups[1].Value;
			}

			// Less ideally, we find an 8-char hex value without the signature
			else if (FormIDRegex.IsMatch(input))
			{
				formid = FormIDRegex.Match(input).Groups[0].Value;
			}

			// We found no matches. We can only hope it was already converted
			else
			{
				return input;
			}

			return Convert.ToInt32(formid, 16).ToString();
		}

		// Captures the FormID specifically from the present [CELL/WRLD:x] phrase, and returns the string value of the integer value of itself
		// Prefers WRLD over CELL
		static string CaptureSpaceFormID(string input)
		{
			MatchCollection matches = SpaceFormIDRegex.Matches(input);

			if (matches.Count == 0)
			{
				return input;
			}

			Match bestMatch = matches.Last();

			// Prefer the match for "WRLD" in the potential event that we find a match for both WRLD and CELL (and WRLD wasn't the last match anyway)
			// Finds the last match of WRLD if there are multiple
			foreach (Match match in matches.Reverse())
			{
				if (match.Groups[1].Value == "WRLD")
				{
					bestMatch = match;
					break;
				}
			}

			return Convert.ToInt32(bestMatch.Groups[2].Value, 16).ToString();
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
			if (!SignatureFormIDRegex.IsMatch(displayName))
			{
				return displayName;
			}

			return QuotedTermRegex.Match(displayName).Groups[1].Value;
		}

		// Returns the numeric quantity of the component for the given quantity name
		static string GetComponentQuantity(string component, string quantity)
		{
			return SimpleQuery($"SELECT \"{quantity}\" FROM Component where component = '{component}'", true, GetNewConnection()).First();
		}

		// Returns the total spawn weight of spawn pool for the class at the location
		static string GetSumNPCSpawnWeight(string locationFormID, string npcClass)
		{
			return SimpleQuery($"SELECT sum(value) FROM Location WHERE locationFormID = {locationFormID} and npcClass = '{npcClass}'", true, GetNewConnection()).First();
		}

		// Returns and corrects the NPC Name present in the raw input string
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

		// Returns and corrects the NPC 'class' present in the raw input string
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

		// Simplifies/corrects the given lock level string
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
				StdOutWithColor($"Unable to determine game version from exe at {GameExePath}.", ColorInfo);
				return GetGameVersionFromUser();
			}

			StdOutWithColor($"Is \"{gameVersion}\" the correct game version?\n(Enter y/n)", ColorQuestion);

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
			return File.Exists(GameExePath) ? FileVersionInfo.GetVersionInfo(GameExePath).FileVersion : null;
		}

		// Asks the user to enter game version, and returns the entered line
		static string GetGameVersionFromUser()
		{
			StdOutWithColor("\nPlease enter the correct game version string:", ColorQuestion);
			return Console.ReadLine() ?? string.Empty;
		}

		static void AddToSummaryReport(string desc, string row)
		{
			AddToSummaryReport(desc, new List<string> { row });
		}

		static void AddToSummaryReport(string desc, List<string> rows)
		{
			SummaryReport.Add(desc);
			SummaryReport.AddRange(rows);
			SummaryReport.Add(string.Empty);
		}

		// Removes old DB files and outputs
		static void Cleanup()
		{
			File.Delete(DatabasePath);
			File.Delete(DatabasePath + "-journal");
			File.Delete(DiscardedCellsPath);
			File.Delete(DatabaseSummaryPath);
		}
	}
}
