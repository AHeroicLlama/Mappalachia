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
		static SqliteConnection Connection = new SqliteConnection("Data Source=" + BuildPaths.GetDatabasePath());

		static readonly Regex FormIDRegex = new Regex(@".*\[[A-Z_]{4}:([0-9A-F]{8})\].*");
		static readonly Regex RemoveReferenceRegex = new Regex(@"(.*) ?\[[A-Z_]{4}:[0-9A-F]{8}\]");

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
				new SQLiteColumn( "x", SQLiteType.REAL ),
				new SQLiteColumn( "y", SQLiteType.REAL ),
				new SQLiteColumn( "z", SQLiteType.REAL ),
				new SQLiteColumn( "locationFormID", SQLiteType.TEXT ),
				new SQLiteColumn( "lockLevel", SQLiteType.TEXT ),
				new SQLiteColumn( "primitiveShape", SQLiteType.TEXT ),
				new SQLiteColumn( "boundX", SQLiteType.REAL ),
				new SQLiteColumn( "boundY", SQLiteType.REAL ),
				new SQLiteColumn( "boundZ", SQLiteType.REAL ),
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
				new SQLiteColumn( "x", SQLiteType.REAL ),
				new SQLiteColumn( "y", SQLiteType.REAL ),
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
			Cleanup();

			Connection.Open();

			Tables.ForEach(table => ImportTableFromCSV(table));

			Console.WriteLine("Unescaping characters");
			Tables.ForEach(table => UnescapeCharacters(table));

			// Pull the MapMarker data into a new table, separate from Position
			SimpleQuery("CREATE TABLE MapMarker AS SELECT spaceFormID, referenceFormID as label, mapMarkerName as icon FROM Position WHERE mapMarkerName != '';");
			SimpleQuery("ALTER TABLE Position DROP COLUMN mapMarkerName;");

			Console.WriteLine("Reducing data");
			TransformColumn("Position", "referenceFormID", ConvertToFormID);
			ChangeColumnType("Position", "ReferenceFormID", "INTEGER");

			TransformColumn("Region", "spaceFormID", ConvertToFormID);
			ChangeColumnType("Region", "spaceFormID", "INTEGER");

			// TODO replace Position.ShortName with both label and instanceFormID

			SimpleQuery("DELETE FROM Space WHERE spaceEditorID = '' OR spaceDisplayName = '';"); // Remove spaces without names
			SimpleQuery("DELETE FROM Entity WHERE entityFormID NOT IN (SELECT referenceFormID FROM Position);"); // Remove entities which are not placed
			SimpleQuery("DELETE FROM Region WHERE spaceFormID = '';"); // Remove regions which are not placed

			Console.WriteLine("Vacuuming");
			SimpleQuery("VACUUM;");

			Console.WriteLine("Done. Press any key");
			Console.ReadKey();
		}

		// Executes a query against the open database
		static void SimpleQuery(string query)
		{
			using (SqliteCommand command = new SqliteCommand(query, Connection))
			{
				command.ExecuteNonQuery();
			}
		}

		// Changes the type of the given column on the given table
		static void ChangeColumnType(string table, string column, string type)
		{
			SimpleQuery($"ALTER TABLE {table} ADD COLUMN 'temp' {type};"); // Create a temp column with the new type
			SimpleQuery($"UPDATE {table} SET 'temp' = (SELECT {column} FROM {table});"); // Copy the source column into the temp
			SimpleQuery($"ALTER TABLE {table} DROP COLUMN {column};"); // Drop the original
			SimpleQuery($"ALTER TABLE {table} RENAME COLUMN 'temp' TO {column};"); // Rename temp column to original
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
			SimpleQuery($"CREATE INDEX 'temp' ON {tableName} ({columnName});");

			string readQuery = $"SELECT {columnName} FROM {tableName}";
			string updateQuery = $"UPDATE {tableName} SET {columnName} = @new WHERE {columnName} = @original"; //TODO bug, this updates all rows despite the WHERE clause

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

			SimpleQuery($"DROP INDEX temp");
		}

		// Return the given string with custom escape sequences replaced
		static string UnescapeCharacters(string input)
		{
			return input.Replace(":COMMA:", ",").Replace(":QUOT:", "\"").Replace("''''", "\'\'");
		}

		// Converts the input string to the string value of the integer value of the valid 8-char hex FormID which it contains
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

		// Removes old DB files prior to building new
		static void Cleanup()
		{
			Console.WriteLine("Cleaning up");

			File.Delete(BuildPaths.GetDatabasePath());
			File.Delete(BuildPaths.GetDatabasePath() + "-journal");
		}
	}
}
