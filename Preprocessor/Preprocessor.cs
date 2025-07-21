using System.Diagnostics;
using System.Text.RegularExpressions;
using Library;
using Microsoft.Data.Sqlite;
using static Library.BuildTools;
using static Library.ReaderExtensions;

namespace Preprocessor
{
	internal static partial class Preprocessor
	{
		static SqliteConnection Connection { get; set; } = GetNewConnection();

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
					case '5': // Deprecated hidden option for debugging DB w/o checking image assets
						Preprocess();
						ValidateDatabase();
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

		static async void Preprocess()
		{
			Directory.CreateDirectory(DataPath);

			string gameVersion = GetValidatedGameVersion();

			StdOutWithColor($"Building Mappalachia database at {DatabasePath}\n", ColorInfo);

			Connection.Close();
			Cleanup();
			Connection.Open();

			SimpleQuery("PRAGMA foreign_keys = 0");

			// Create the Meta table, add the game version to it
			SimpleQuery("CREATE TABLE Meta (key TEXT NOT NULL UNIQUE PRIMARY KEY, value TEXT) STRICT;");
			SimpleQuery($"INSERT INTO Meta (key, value) VALUES('GameVersion', '{gameVersion}');");

			// Create new tables
			SimpleQuery($"CREATE TABLE Entity(entityFormID INTEGER PRIMARY KEY, displayName TEXT, editorID TEXT, signature TEXT);");
			SimpleQuery($"CREATE TABLE Container(containerFormID INTEGER REFERENCES Entity(entityFormID), contentFormID INTEGER REFERENCES Entity(entityFormID), quantity INTEGER);");
			SimpleQuery($"CREATE TABLE Position(spaceFormID INTEGER REFERENCES Space(spaceFormID), referenceFormID TEXT REFERENCES Entity(entityFormID), x REAL, y REAL, z REAL, locationFormID TEXT REFERENCES Location(locationFormID), lockLevel TEXT, primitiveShape TEXT, boundX REAL, boundY REAL, boundZ REAL, rotZ REAL, mapMarkerName TEXT, shortName TEXT, teleportsToFormID TEXT);");
			SimpleQuery($"CREATE TABLE Space(spaceFormID INTEGER PRIMARY KEY, spaceEditorID TEXT, spaceDisplayName TEXT, isWorldspace INTEGER, isInstanceable INTEGER);");
			SimpleQuery($"CREATE TABLE Location(locationFormID INTEGER, parentLocationFormID TEXT, minLevel INTEGER, maxLevel INTEGER, property TEXT, value INTEGER);");
			SimpleQuery($"CREATE TABLE Region(spaceFormID TEXT REFERENCES Space(spaceFormID), regionFormID INTEGER, regionEditorID TEXT, locationFormID TEXT, subRegionIndex INTEGER, coordIndex INTEGER, x REAL, y REAL);");
			SimpleQuery($"CREATE TABLE Scrap(junkFormID INTEGER REFERENCES Entity(entityFormID), component TEXT, componentQuantity TEXT);");
			SimpleQuery($"CREATE TABLE Component(component TEXT PRIMARY KEY, singular INTEGER, rare INTEGER, medium INTEGER, low INTEGER, high INTEGER, bulk INTEGER);");

			// Import to tables from xedit exports
			ImportTableFromCSV("Entity");
			ImportTableFromCSV("Container");
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

			// Capture and convert to int the contentFormID on Container
			TransformColumn(CaptureFormID, "Container", "contentFormID");
			ChangeColumnType("Container", "contentFormID", "INTEGER");
			AddForeignKey("Container", "contentFormID", "INTEGER", "Entity", "entityFormID");

			// Group Container by container and contents, for rare cases where > 1 unstackable item are in the same container, so that we increment quantity instead of having duplicate rows
			SimpleQuery("CREATE TABLE TempContainer(containerFormID INTEGER REFERENCES Entity(entityFormID), contentFormID INTEGER REFERENCES Entity(entityFormID), quantity INTEGER);");
			SimpleQuery("INSERT INTO TempContainer SELECT containerFormID, contentFormID, sum(quantity) AS quantity FROM Container GROUP BY containerFormID, contentFormID;");
			SimpleQuery("DROP TABLE Container;");
			SimpleQuery("ALTER TABLE TempContainer RENAME TO Container;");

			// Capture and convert to int the referenceFormID on Position
			TransformColumn(CaptureFormID, "Position", "referenceFormID");
			ChangeColumnType("Position", "referenceFormID", "INTEGER");
			AddForeignKey("Position", "referenceFormID", "INTEGER", "Entity", "entityFormID");

			// Capture and convert to int the locationFormID on Position
			TransformColumn(CaptureFormID, "Position", "locationFormID");

			// Capture and convert to int the Space FormID of the teleportsToFormID on Position
			TransformColumn(CaptureSpaceFormID, "Position", "teleportsToFormID");
			ChangeColumnType("Position", "teleportsToFormID", "INTEGER");
			AddForeignKey("Position", "teleportsToFormID", "INTEGER", "Space", "spaceFormID");

			// Capture and convert to int the parent location Form ID on Location
			TransformColumn(CaptureFormID, "Location", "parentLocationFormID");
			ChangeColumnType("Location", "parentLocationFormID", "INTEGER");
			AddForeignKey("Location", "parentLocationFormID", "INTEGER", "Location", "locationFormID");

			// Capture and convert to int the spaceFormID on Region
			TransformColumn(CaptureSpaceFormID, "Region", "spaceFormID");
			ChangeColumnType("Region", "spaceFormID", "INTEGER");
			AddForeignKey("Region", "spaceFormID", "INTEGER", "Space", "spaceFormID");

			// Capture and convert to int the locationFormID on Region
			TransformColumn(CaptureFormID, "Region", "locationFormID");
			ChangeColumnType("Region", "locationFormID", "INTEGER");
			AddForeignKey("Region", "locationFormID", "INTEGER", "Location", "locationFormID");

			// For the region table, use the location column to reference the Location table, to find the min and max levels of the region
			// Then, drop the location column. (Location table is dropped later)
			SimpleQuery("ALTER TABLE Region ADD COLUMN 'minLevel' INTEGER;");
			SimpleQuery("ALTER TABLE Region ADD COLUMN 'maxLevel' INTEGER;");
			TransformColumn(GetMinLocationLevel, "Region", "locationFormID", "minLevel");
			TransformColumn(GetMaxLocationLevel, "Region", "locationFormID", "maxLevel");
			SimpleQuery("ALTER TABLE Region DROP COLUMN locationFormID;");

			// Split the region table points out into another table
			SimpleQuery("CREATE TABLE RegionPoints AS SELECT regionFormID, subRegionIndex, coordIndex, x, y FROM Region;");
			SimpleQuery("ALTER TABLE Region DROP COLUMN subRegionIndex;");
			SimpleQuery("ALTER TABLE Region DROP COLUMN coordIndex;");
			SimpleQuery("ALTER TABLE Region DROP COLUMN x;");
			SimpleQuery("ALTER TABLE Region DROP COLUMN y;");
			AddForeignKey("RegionPoints", "regionFormID", "INTEGER", "Region", "regionFormID");

			// Reduce region rows to distinct rows (now that the points which made them unique are removed)
			SimpleQuery($"CREATE TABLE TempRegion(regionFormID INTEGER PRIMARY KEY, regionEditorID TEXT, spaceFormID TEXT REFERENCES Space(spaceFormID), minLevel INTEGER, maxLevel INTEGER);");
			SimpleQuery("INSERT INTO TempRegion SELECT DISTINCT regionFormID, regionEditorID, spaceFormID, minLevel, maxLevel FROM Region;");
			SimpleQuery("DROP TABLE Region;");
			SimpleQuery("ALTER TABLE TempRegion RENAME TO Region;");

			// Properly populate the isInstanceable flag - no value is false
			SimpleQuery($"UPDATE Space SET isInstanceable = 0 WHERE isInstanceable != 1;");

			// Discard spaces which are not accessible, and output a list of those
			TransformColumn(UnescapeCharacters, "Space", "spaceDisplayName");
			List<string> deletedRows = SimpleQuery($"DELETE FROM Space WHERE {DiscardCellsQuery} RETURNING spaceFormID, spaceEditorID, spaceDisplayName, isWorldspace, isInstanceable;");
			deletedRows.Sort();
			deletedRows.Insert(0, "spaceFormID,spaceDisplayName,spaceEditorID,isWorldspace,isInstanceable");
			File.WriteAllLines(DiscardedCellsPath, deletedRows);

			// Create a replacement copy of Space, adding a temporary maximum estimate for the center x/y and max 2d range
			SimpleQuery($"CREATE TABLE TempSpace(spaceFormID INTEGER PRIMARY KEY, spaceEditorID TEXT, spaceDisplayName TEXT, isWorldspace INTEGER, isInstanceable INTEGER, centerX REAL, centerY REAL, maxRange REAL);");
			SimpleQuery("INSERT INTO TempSpace (spaceFormID, spaceEditorID, spaceDisplayName, isWorldspace, isInstanceable, centerX, centerY, maxRange) SELECT Space.spaceFormID, spaceEditorID, spaceDisplayName, isWorldspace, isInstanceable, (MIN(x) + MAX(x)) / 2, (MIN(y) + MAX(y)) / 2, MAX(ABS(MIN(x) - MAX(x)), ABS(MIN(y) - MAX(y))) FROM Space JOIN Position ON Space.spaceFormID = Position.spaceFormID GROUP BY Space.spaceFormID;");
			SimpleQuery("DROP TABLE Space;");
			SimpleQuery("ALTER TABLE TempSpace RENAME TO Space;");

			// Read in scale/offset corrections for Spaces
			foreach (string file in Directory.GetFiles(CellXYScaleCorrectionPath))
			{
				string[] values = File.ReadAllLines(file);

				double centerX = double.Parse(values[0]);
				double centerY = double.Parse(values[1]);
				double maxRange = double.Parse(values[2]);

				SimpleQuery($"UPDATE Space SET centerX = {centerX}, centerY = {centerY}, maxRange = {maxRange} WHERE spaceEditorID = '{Path.GetFileNameWithoutExtension(file)}';");
			}

			// For spaces which are sister spaces, we copy the scaling of the first onto the others
			foreach (List<string> spaceCollection in SisterSpaces)
			{
				string parentEditorID = spaceCollection.First();
				Space? parent = await CommonDatabase.GetSpaceByEditorID(Connection, parentEditorID) ?? throw new Exception($"Unable to find Space {parentEditorID}");

				foreach (string childEditorID in spaceCollection.Skip(1))
				{
					SimpleQuery($"UPDATE Space SET centerX = {parent.CenterX}, centerY = {parent.CenterY}, maxRange = {parent.MaxRange} WHERE spaceEditorID = '{childEditorID}';");
				}
			}

			// Add the cell padding - excludes Worldspaces
			SimpleQuery($"UPDATE Space SET maxRange = maxRange + {CellPadding} WHERE isWorldspace = 0;");

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
			SimpleQuery("DELETE FROM RegionPoints WHERE regionFormID NOT IN (SELECT regionFormID FROM Region);");
			SimpleQuery("DELETE FROM MapMarker WHERE spaceFormID NOT IN (SELECT spaceFormID FROM Space);");
			SimpleQuery("DELETE FROM Container WHERE containerFormID NOT IN (SELECT referenceFormID FROM Position);");
			SimpleQuery("DELETE FROM Entity WHERE entityFormID NOT IN (SELECT referenceFormID FROM Position) AND entityFormID NOT IN (SELECT contentFormID FROM Container);");
			SimpleQuery("DELETE FROM Scrap WHERE junkFormID NOT IN (SELECT entityFormID FROM Entity);");

			// Clean up scrap and component names
			TransformColumn(CaptureQuotedTerm, "Scrap", "componentQuantity");
			TransformColumn(CaptureQuotedTerm, "Scrap", "component");
			SimpleQuery($"UPDATE Scrap SET componentQuantity = 'Singular' WHERE componentQuantity LIKE '%Singular%';");

			// Fix erroneous data which is exported from xEdit with values somehow misaligned from in-game
			SimpleQuery(CorrectLockLevelQuery);
			SimpleQuery(CorrectPrimitiveShapeQuery);

			TransformColumn(ReduceLockLevel, "Position", "lockLevel");

			// Transform the component quantity keywords to numeric values from Scrap table, then drop the Component table
			TransformColumn(GetComponentQuantity, "Scrap", "component", "componentQuantity", "componentQuantity");
			ChangeColumnType("Scrap", "componentQuantity", "INTEGER");
			SimpleQuery($"DROP TABLE Component;");

			// Extract the NPC types and classes on Location from the raw 'property'
			SimpleQuery("ALTER TABLE Location ADD COLUMN npcName TEXT;");
			SimpleQuery("ALTER TABLE Location ADD COLUMN npcClass TEXT;");
			TransformColumn(GetNPCName, "Location", "property", "npcName");
			TransformColumn(GetNPCClass, "Location", "property", "npcClass");
			SimpleQuery("DELETE FROM Location WHERE npcName = '' OR npcClass = '';");

			// Extract the independent NPC spawn chances, by summing the spawn pool and weights from each location
			SimpleQuery("ALTER TABLE Location ADD COLUMN sumWeight INTEGER;");
			TransformColumn(GetSumNPCSpawnWeight, "Location", "locationFormID", "npcClass", "sumWeight");
			SimpleQuery("ALTER TABLE Location ADD COLUMN spawnWeight REAL;");
			TransformColumn(DivideString, "Location", "value", "sumWeight", "spawnWeight"); // Set the value of 'spawnWeight' with value/sum.
			SimpleQuery("ALTER TABLE Location DROP COLUMN sumWeight;");
			SimpleQuery("ALTER TABLE Location DROP COLUMN value;");

			// Create the NPC table
			SimpleQuery(
				"CREATE TABLE NPC AS " +
				"SELECT Space.spaceFormID, instanceFormID, npcName, spawnWeight " +
				"FROM Position " +
				"JOIN Entity ON Entity.entityFormID = Position.referenceFormID " +
				"JOIN Location ON Position.locationFormID = Location.locationFormID " +
				"JOIN Space ON Space.spaceFormID = Position.spaceFormID " +
				"WHERE " +
				"Entity.editorID NOT LIKE '%Turret%' AND " +
				"((Entity.editorID LIKE 'LvlSub%' AND Location.npcClass = 'Sub') OR " +
				"(Entity.editorID LIKE 'LvlMain%' AND Location.npcClass = 'Main') OR " +
				"(Entity.editorID LIKE 'LvlCritterA%' AND Location.npcClass = 'CritterA') OR " +
				"(Entity.editorID LIKE 'LvlCritterB%' AND Location.npcClass = 'CritterB'));");
			ChangeColumnType("NPC", "spaceFormID", "INTEGER");
			ChangeColumnType("NPC", "instanceFormID", "INTEGER");
			SimpleQuery("DELETE FROM NPC WHERE spawnWeight = 0;");
			AddForeignKey("NPC", "instanceFormID", "INTEGER", "Position", "instanceFormID");
			SimpleQuery("DROP TABLE Location;");
			SimpleQuery("ALTER TABLE Position DROP COLUMN locationFormID;");

			// Un-escape chars from columns which we've not otherwise touched
			TransformColumn(UnescapeCharacters, "Entity", "displayName");

			// Create a reduced semi-redundant copy of the Position table, which already has all the possible search results grouped and counted
			// We can use this later to conduct the searches much faster, but the Position table remains to get non-grouped coordinates
			SimpleQuery("CREATE TABLE Position_PreGrouped (spaceFormID INTEGER REFERENCES Space(spaceFormID), referenceFormID INTEGER REFERENCES Entity(EntityFormID), lockLevel TEXT, label TEXT, count INTEGER);");
			SimpleQuery("INSERT INTO Position_PreGrouped (spaceFormID, referenceFormID, lockLevel, label, count) SELECT spaceFormID, referenceFormId, lockLevel, label, COUNT(*) as count FROM Position GROUP BY referenceFormID, label, spaceFormID, lockLevel;");

			// Modify Position to assign instanceFormID as Primary Key
			SimpleQuery("CREATE TABLE temp (spaceFormID INTEGER REFERENCES Space(spaceFormID), x REAL, y REAL, z REAL, lockLevel TEXT, primitiveShape TEXT, boundX REAL, boundY REAL, boundZ REAL, rotZ REAL, referenceFormID INTEGER REFERENCES Entity(entityFormID), teleportsToFormID INTEGER REFERENCES Space(spaceFormID), label TEXT, instanceFormID INTEGER PRIMARY KEY);");
			SimpleQuery("INSERT INTO temp SELECT * FROM Position;");
			SimpleQuery("DROP TABLE Position;");
			SimpleQuery("ALTER TABLE temp RENAME TO Position;");

			// Null teleporters which target a space we dropped
			SimpleQuery("UPDATE Position SET teleportsToFormID = NULL WHERE teleportsToFormID NOT IN (SELECT spaceFormID FROM Space);");

			// Null empty rows which are not TEXT
			SimpleQuery("UPDATE Position SET teleportsToFormID = NULL WHERE teleportsToFormID = '';");
			SimpleQuery("UPDATE Position SET boundX = NULL WHERE boundX = '';");
			SimpleQuery("UPDATE Position SET boundY = NULL WHERE boundY = '';");
			SimpleQuery("UPDATE Position SET boundZ = NULL WHERE boundZ = '';");
			SimpleQuery("UPDATE Position SET rotZ = NULL WHERE rotZ = '';");

			AssignConstraints();

			// Create indexes
			SimpleQuery("CREATE INDEX indexGeneral ON Position(referenceFormID, lockLevel, label, spaceFormID);");
			SimpleQuery("CREATE INDEX indexNPC ON NPC(spawnWeight, npcName, spaceFormID, instanceFormID);");
			SimpleQuery("CREATE INDEX indexSpaceTeleportsTo ON Position(spaceFormID, teleportsToFormID);");
			SimpleQuery("CREATE INDEX indexCount ON Position_PreGrouped(count);");
			SimpleQuery("CREATE INDEX indexReference ON Position_PreGrouped(referenceFormID);");
			SimpleQuery("CREATE INDEX indexComponent ON Scrap(component);");

			// Final steps, wrap-up, optimizations, etc
			SimpleQuery("PRAGMA foreign_keys = 1;");
			SimpleQuery("ANALYZE;");
			SimpleQuery("PRAGMA optimize;");
			SimpleQuery("VACUUM;");
			SimpleQuery("PRAGMA query_only;");

			GenerateSummary();

			StdOutWithColor($"Build and Preprocess Done.\n", ColorInfo);
		}

		static async void GenerateSummary()
		{
			StdOutWithColor($"\nGenerating Summary Report at {DatabaseSummaryPath}\n", ColorInfo);

			Connection.Close();
			Connection.Dispose();
			GC.Collect();
			GC.WaitForPendingFinalizers();

			AddToSummaryReport("MD5 Checksum", GetMD5Hash(DatabasePath));

			Connection = GetNewConnection();
			Connection.Open();

			AddToSummaryReport("Size", (new FileInfo(DatabasePath).Length / BuildTools.Kilobyte).ToString() + " KB");
			AddToSummaryReport("Built At UTC", DateTime.UtcNow.ToString());
			AddToSummaryReport("CSV Imported with SQLite Version", SqliteTools("--version"));
			AddToSummaryReport("Tables", SqliteTools(DatabasePath + " .tables"));
			AddToSummaryReport("Indices", SqliteTools(DatabasePath + " .indices"));
			AddToSummaryReport("Game Version", await CommonDatabase.GetGameVersion(Connection));
			AddToSummaryReport("Spaces", SimpleQuery("SELECT spaceEditorID, spaceDisplayName, spaceFormID, isWorldspace, isInstanceable, centerX, centerY, maxRange FROM Space ORDER BY isWorldspace DESC, spaceEditorID ASC;"));
			AddToSummaryReport("Avg X/Y/Z", SimpleQuery("SELECT AVG(x), AVG(y), AVG(z) FROM Position;"));
			AddToSummaryReport("Avg Bounds X/Y/Z", SimpleQuery("SELECT AVG(boundX), AVG(boundY), AVG(boundZ) FROM Position;"));
			AddToSummaryReport("Avg CenterX, CenterY", SimpleQuery("SELECT AVG(centerX), AVG(centerY) FROM Space;"));
			AddToSummaryReport("Avg Rotation", SimpleQuery("SELECT AVG(rotZ) FROM Position;"));
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
			AddToSummaryReport("Avg Count Regions, Coords from RegionPoints", SimpleQuery("SELECT AVG(regionMax), AVG(coordMax) FROM (SELECT MAX(subRegionIndex) as regionMax, MAX(coordIndex) as coordMax FROM RegionPoints GROUP BY regionFormID);"));
			AddToSummaryReport("Scrap with Avg component qty", SimpleQuery("SELECT component, AVG(componentQuantity) FROM Scrap GROUP BY component;"));
			AddToSummaryReport("Avg Map Marker X/Y", SimpleQuery("SELECT AVG(x), AVG(y) FROM MapMarker;"));
			AddToSummaryReport("Map Markers", SimpleQuery("SELECT spaceEditorID, icon, label FROM MapMarker INNER JOIN Space ON MapMarker.spaceFormID = Space.spaceFormID ORDER BY spaceEditorID ASC, icon ASC, label ASC;"));
			AddToSummaryReport("NPCs by Space and Weight", SimpleQuery(
				"SELECT Space.spaceDisplayName, npcName, AVG(spawnWeight) " +
				"FROM NPC " +
				"JOIN Space on NPC.spaceFormID = Space.spaceFormID " +
				"GROUP BY npcName, spaceDisplayName " +
				"ORDER BY isWorldspace DESC, spaceDisplayName, npcName;"));
			AddToSummaryReport("Worldspaces", SimpleQuery("SELECT spaceFormID, spaceEditorID, spaceDisplayName, isInstanceable FROM Space WHERE isWorldspace = 1;"));
			AddToSummaryReport("Avg spaceFormID as Dec", SimpleQuery("SELECT AVG(spaceFormID) FROM Space;"));
			AddToSummaryReport("Avg entityFormID as Dec", SimpleQuery("SELECT AVG(entityFormID) FROM Entity;"));
			AddToSummaryReport("Avg containerFormID as Dec", SimpleQuery("SELECT AVG(containerFormID) FROM Container;"));
			AddToSummaryReport("Avg contentFormID as Dec", SimpleQuery("SELECT AVG(contentFormID) FROM Container;"));
			AddToSummaryReport("Avg MapMarker spaceFormID as Dec", SimpleQuery("SELECT AVG(spaceFormID) FROM MapMarker;"));
			AddToSummaryReport("Avg Psn spaceFormID as Dec", SimpleQuery("SELECT AVG(spaceFormID) FROM Position;"));
			AddToSummaryReport("Avg Psn referenceFormID as Dec", SimpleQuery("SELECT AVG(referenceFormID) FROM Position;"));
			AddToSummaryReport("Avg Psn instanceFormID as Dec", SimpleQuery("SELECT AVG(instanceFormID) FROM Position;"));
			AddToSummaryReport("Avg Psn teleportsToFormID as Dec", SimpleQuery("SELECT AVG(teleportsToFormID) FROM Position;"));
			AddToSummaryReport("Avg regionFormID as Dec", SimpleQuery("SELECT AVG(regionFormID) FROM Region;"));
			AddToSummaryReport("Avg region spaceFormID as Dec", SimpleQuery("SELECT AVG(spaceFormID) FROM Region;"));
			AddToSummaryReport("Avg region minLevel", SimpleQuery("SELECT AVG(minLevel) FROM Region;"));
			AddToSummaryReport("Avg region maxLevel", SimpleQuery("SELECT AVG(maxLevel) FROM Region;"));
			AddToSummaryReport("Avg junkFormID as Dec", SimpleQuery("SELECT AVG(junkFormID) FROM Scrap;"));
			AddToSummaryReport("Avg X, Y per Space", SimpleQuery("SELECT spaceEditorID, AVG(x), AVG(y) FROM Space JOIN Position ON Position.spaceFormID = Space.spaceFormID GROUP BY Space.spaceFormID ORDER BY isWorldspace DESC, space.spaceEditorID ASC;"));
			AddToSummaryReport("Avg Container items per container", SimpleQuery("SELECT avg(contentsCount) FROM (SELECT count(contentFormID) as contentsCount FROM Container GROUP BY ContainerFormID);"));
			AddToSummaryReport("Avg Container quantity per item", SimpleQuery("SELECT avg(quantity) FROM Container;"));
			AddToSummaryReport("Unique Container count", SimpleQuery("SELECT count(DISTINCT containerFormID) FROM Container;"));

			List<string> spaceExterns = new List<string>();
			List<string> spaceChecksums = new List<string>();
			foreach (Space space in await CommonDatabase.GetAllSpaces(Connection))
			{
				List<Coord> coordinates = await CommonDatabase.GetAllCoords(Connection, space);
				double maxRadius = space.MaxRange / 2d;

				// Find the count of coordinates in the space which are further from the center than the max range
				int outlierSum = coordinates.Where(c =>
					Math.Abs(space.CenterX - c.X) > maxRadius ||
					Math.Abs(space.CenterY - c.Y) > maxRadius).Count();

				spaceExterns.Add($"{space.EditorID}:{outlierSum}");
				spaceChecksums.Add($"{space.EditorID}:{coordinates.ApproximateChecksum()}");
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

			List<string> data = new List<string>();

			using SqliteCommand command = new SqliteCommand(query, connection);
			using SqliteDataReader reader = command.ExecuteReader();

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

			Console.WriteLine($"Transform {tableName}.{sourceColumn} -> {targetColumn}: {method.Method.Name}");

			using SqliteCommand readCommand = new SqliteCommand(readQuery, Connection);
			using SqliteDataReader reader = readCommand.ExecuteReader();
			using SqliteTransaction transaction = Connection.BeginTransaction();
			using SqliteCommand updateCommand = new SqliteCommand(updateQuery, Connection, transaction);

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

			SimpleQuery($"DROP INDEX '{tempIndex}'", true);
		}

		// Loops a table and amends a column according to the value of 2 columns, when passed to the method
		static void TransformColumn(Func<string, string, string?> method, string tableName, string sourceColumnA, string sourceColumnB, string targetColumn)
		{
			string tempIndex = "tempIndex";

			SimpleQuery($"CREATE INDEX {tempIndex} ON {tableName} ({sourceColumnA}, {sourceColumnB});", true);

			string readQuery = $"SELECT {sourceColumnA}, {sourceColumnB}, ROWID FROM {tableName}";
			string updateQuery = $"UPDATE {tableName} SET {targetColumn} = @new WHERE ROWID = @rowID";

			Console.WriteLine($"Transform {tableName}.{sourceColumnA},{sourceColumnB} -> {targetColumn}: {method.Method.Name}");

			using SqliteCommand readCommand = new SqliteCommand(readQuery, Connection);
			using SqliteDataReader reader = readCommand.ExecuteReader();
			using SqliteTransaction transaction = Connection.BeginTransaction();
			using SqliteCommand updateCommand = new SqliteCommand(updateQuery, Connection, transaction);

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

			SimpleQuery($"DROP INDEX '{tempIndex}'", true);
		}

		// Assigns NOT NULL and UNIQUE constraints on columns, also sets tables to STRICT (except Meta)
		// Unfortately SQLite requires that we fully re-create the table in order to achieve this
		static void AssignConstraints()
		{
			SimpleQuery("CREATE TABLE temp AS SELECT * FROM Container;");
			SimpleQuery("DROP TABLE Container;");
			SimpleQuery("CREATE TABLE Container (containerFormID INTEGER NOT NULL REFERENCES Entity (entityFormID), contentFormID INTEGER NOT NULL REFERENCES Entity (entityFormID), quantity INTEGER NOT NULL) STRICT;");
			SimpleQuery("INSERT INTO Container (containerFormID, contentFormID, quantity) SELECT containerFormID, contentFormID, quantity FROM temp;");
			SimpleQuery("DROP TABLE temp;");

			SimpleQuery("CREATE TABLE temp AS SELECT * FROM Entity;");
			SimpleQuery("DROP TABLE Entity;");
			SimpleQuery("CREATE TABLE Entity (entityFormID INTEGER NOT NULL UNIQUE PRIMARY KEY, displayName TEXT, editorID TEXT NOT NULL UNIQUE, signature TEXT NOT NULL) STRICT;");
			SimpleQuery("INSERT INTO Entity (entityFormID, displayName, editorID, signature) SELECT entityFormID, displayName, editorID, signature FROM temp;");
			SimpleQuery("DROP TABLE temp;");

			SimpleQuery("CREATE TABLE temp AS SELECT * FROM MapMarker;");
			SimpleQuery("DROP TABLE MapMarker;");
			SimpleQuery("CREATE TABLE MapMarker (x REAL NOT NULL, y REAL NOT NULL, label TEXT NOT NULL, icon TEXT NOT NULL, spaceFormID INTEGER NOT NULL REFERENCES Space (spaceFormID)) STRICT;");
			SimpleQuery("INSERT INTO MapMarker (x, y, label, icon, spaceFormID) SELECT x, y, label, icon, spaceFormID FROM temp;");
			SimpleQuery("DROP TABLE temp;");

			SimpleQuery("CREATE TABLE temp AS SELECT * FROM NPC;");
			SimpleQuery("DROP TABLE NPC;");
			SimpleQuery("CREATE TABLE NPC (npcName TEXT NOT NULL, spawnWeight REAL NOT NULL, spaceFormID INTEGER NOT NULL, instanceFormID INTEGER NOT NULL REFERENCES Position (instanceFormID)) STRICT;");
			SimpleQuery("INSERT INTO NPC (npcName, spawnWeight, spaceFormID, instanceFormID) SELECT npcName, spawnWeight, spaceFormID, instanceFormID FROM temp;");
			SimpleQuery("DROP TABLE temp;");

			SimpleQuery("CREATE TABLE temp AS SELECT * FROM Position;");
			SimpleQuery("DROP TABLE Position;");
			SimpleQuery("CREATE TABLE Position (spaceFormID INTEGER NOT NULL REFERENCES Space (spaceFormID), x REAL NOT NULL, y REAL NOT NULL, z REAL NOT NULL, lockLevel TEXT, primitiveShape TEXT, boundX REAL, boundY REAL, boundZ REAL, rotZ REAL, referenceFormID INTEGER NOT NULL REFERENCES Entity (entityFormID), teleportsToFormID INTEGER REFERENCES Space (spaceFormID), label TEXT NOT NULL, instanceFormID INTEGER NOT NULL UNIQUE PRIMARY KEY) STRICT;");
			SimpleQuery("INSERT INTO Position (spaceFormID, x, y, z, lockLevel, primitiveShape, boundX, boundY, boundZ, rotZ, referenceFormID, teleportsToFormID, label, instanceFormID) SELECT spaceFormID, x, y, z, lockLevel, primitiveShape, boundX, boundY, boundZ, rotZ, referenceFormID, teleportsToFormID, label, instanceFormID FROM temp;");
			SimpleQuery("DROP TABLE temp;");

			SimpleQuery("CREATE TABLE temp AS SELECT * FROM Position_PreGrouped;");
			SimpleQuery("DROP TABLE Position_PreGrouped;");
			SimpleQuery("CREATE TABLE Position_PreGrouped (spaceFormID INTEGER NOT NULL REFERENCES Space (spaceFormID), referenceFormID INTEGER NOT NULL REFERENCES Entity (EntityFormID), lockLevel TEXT, label TEXT, count INTEGER NOT NULL) STRICT;");
			SimpleQuery("INSERT INTO Position_PreGrouped (spaceFormID, referenceFormID, lockLevel, label, count) SELECT spaceFormID, referenceFormID, lockLevel, label, count FROM temp;");
			SimpleQuery("DROP TABLE temp;");

			SimpleQuery("CREATE TABLE temp AS SELECT * FROM Region;");
			SimpleQuery("DROP TABLE Region;");
			SimpleQuery("CREATE TABLE Region (regionFormID INTEGER NOT NULL UNIQUE PRIMARY KEY, regionEditorID TEXT NOT NULL UNIQUE, spaceFormID TEXT NOT NULL REFERENCES Space (spaceFormID), minLevel INTEGER NOT NULL, maxLevel INTEGER NOT NULL) STRICT;");
			SimpleQuery("INSERT INTO Region (regionFormID, regionEditorID, spaceFormID, minLevel, maxLevel) SELECT regionFormID, regionEditorID, spaceFormID, minLevel, maxLevel FROM temp;");
			SimpleQuery("DROP TABLE temp;");

			SimpleQuery("CREATE TABLE temp AS SELECT * FROM RegionPoints;");
			SimpleQuery("DROP TABLE RegionPoints;");
			SimpleQuery("CREATE TABLE RegionPoints (subRegionIndex INTEGER NOT NULL, coordIndex INTEGER NOT NULL, x REAL NOT NULL, y REAL NOT NULL, regionFormID INTEGER NOT NULL REFERENCES Region (regionFormID)) STRICT;");
			SimpleQuery("INSERT INTO RegionPoints (subRegionIndex, coordIndex, x, y, regionFormID) SELECT subRegionIndex, coordIndex, x, y, regionFormID FROM temp;");
			SimpleQuery("DROP TABLE temp;");

			SimpleQuery("CREATE TABLE temp AS SELECT * FROM Scrap;");
			SimpleQuery("DROP TABLE Scrap;");
			SimpleQuery("CREATE TABLE Scrap (junkFormID INTEGER NOT NULL REFERENCES Entity (entityFormID), component TEXT NOT NULL, componentQuantity INTEGER NOT NULL) STRICT;");
			SimpleQuery("INSERT INTO Scrap (junkFormID, component, componentQuantity) SELECT junkFormID, component, componentQuantity FROM temp;");
			SimpleQuery("DROP TABLE temp;");

			SimpleQuery("CREATE TABLE temp AS SELECT * FROM Space;");
			SimpleQuery("DROP TABLE Space;");
			SimpleQuery("CREATE TABLE Space (spaceFormID INTEGER NOT NULL UNIQUE PRIMARY KEY, spaceEditorID TEXT NOT NULL UNIQUE, spaceDisplayName TEXT NOT NULL, isWorldspace INTEGER NOT NULL, isInstanceable INTEGER NOT NULL, centerX REAL NOT NULL, centerY REAL NOT NULL, maxRange REAL NOT NULL) STRICT;");
			SimpleQuery("INSERT INTO Space (spaceFormID, spaceEditorID, spaceDisplayName, isWorldspace, isInstanceable, centerX, centerY, maxRange) SELECT spaceFormID, spaceEditorID, spaceDisplayName, isWorldspace, isInstanceable, centerX, centerY, maxRange FROM temp;");
			SimpleQuery("DROP TABLE temp;");
		}

		// Return the given string with custom escape sequences replaced
		static string UnescapeCharacters(string input)
		{
			return input.Replace(":COMMA:", ",").Replace(":QUOT:", "\"").Replace("''", "'").Replace(":CRLF:", "\r\n");
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

		// Removes the signature and formID from the end of a string
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

		static string GetMinLocationLevel(string locationFormID)
		{
			if (locationFormID.IsNullOrWhiteSpace())
			{
				return "0";
			}

			using SqliteCommand command = new SqliteCommand($"SELECT minLevel, parentLocationFormID FROM Location WHERE locationFormID = {locationFormID}", GetNewConnection());
			using SqliteDataReader reader = command.ExecuteReader();

			if (!reader.Read())
			{
				return "0";
			}

			string min = reader.GetString("minLevel");

			if (min.IsNullOrWhiteSpace() || min == "0")
			{
				return GetMinLocationLevel(reader.GetString("parentLocationFormID"));
			}

			return min;
		}

		static string GetMaxLocationLevel(string locationFormID)
		{
			if (locationFormID.IsNullOrWhiteSpace())
			{
				return "0";
			}

			using SqliteCommand command = new SqliteCommand($"SELECT maxLevel, parentLocationFormID FROM Location WHERE locationFormID = {locationFormID}", GetNewConnection());
			using SqliteDataReader reader = command.ExecuteReader();

			if (!reader.Read())
			{
				return "0";
			}

			string max = reader.GetString("maxLevel");

			if (max.IsNullOrWhiteSpace() || max == "0")
			{
				return GetMaxLocationLevel(reader.GetString("parentLocationFormID"));
			}

			return max;
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
			if (string.IsNullOrEmpty(lockLevel))
			{
				return string.Empty;
			}

			Match match = LockLevelRegex.Match(lockLevel);

			// Remove the novice/advanced etc from the levelled locks
			if (match.Success)
			{
				lockLevel = match.Groups[2].Value;
			}

			return lockLevel.WithoutWhitespace();
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

		// Returns a very basic approx checksum of a collection of Coord
		static long ApproximateChecksum(this List<Coord> coords)
		{
			long sum = HashPrime;
			long precision = 10000000000;

			foreach (Coord coord in coords)
			{
				sum += (((long)coord.X * precision) + HashPrime) * (((long)coord.Y * precision) + HashPrime) * (((long)coord.Z * precision) + HashPrime);
			}

			return sum;
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
