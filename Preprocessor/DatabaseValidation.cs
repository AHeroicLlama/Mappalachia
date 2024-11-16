using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;
using static Library.BuildTools;
using static Library.Hardcodings;

namespace Preprocessor
{
	internal partial class Preprocessor
	{
		static void ValidateDatabase()
		{
			StdOutWithColor("Validating database", ColorInfo);

			try
			{
				SimpleQuery("PRAGMA foreign_key_check;");
			}
			catch (SqliteException e)
			{
				FailValidation($"Foreign key check failed: {e.Message}");
			}

			if (SimpleQuery("PRAGMA integrity_check;").First() != "ok")
			{
				FailValidation($"Integrity check failed");
			}

			// Position(s) "label" and Entity "displayName" columns not validated as they have no restrictions
			ValidateColumnMatchesFormat("Position", "spaceFormID", false, ColumnType.INTEGER);
			ValidateColumnMatchesFormat("Position", "x", false, ColumnType.REAL);
			ValidateColumnMatchesFormat("Position", "y", false, ColumnType.REAL);
			ValidateColumnMatchesFormat("Position", "z", false, ColumnType.REAL);
			ValidateColumnMatchesFormat("Position", "lockLevel", true, ColumnType.TEXT, ValidateLockLevel);
			ValidateColumnMatchesFormat("Position", "primitiveShape", true, ColumnType.TEXT, ValidatePrimitiveShape);
			ValidateColumnMatchesFormat("Position", "boundX", true, ColumnType.REAL);
			ValidateColumnMatchesFormat("Position", "boundY", true, ColumnType.REAL);
			ValidateColumnMatchesFormat("Position", "boundZ", true, ColumnType.REAL);
			ValidateColumnMatchesFormat("Position", "rotZ", true, ColumnType.REAL);
			ValidateColumnMatchesFormat("Position", "referenceFormID", false, ColumnType.INTEGER);
			ValidateColumnMatchesFormat("Position", "locationFormID", true, ColumnType.INTEGER);
			ValidateColumnMatchesFormat("Position", "teleportsToFormID", true, ColumnType.INTEGER);
			ValidateColumnMatchesFormat("Position", "instanceFormID", false, ColumnType.INTEGER);

			ValidateColumnMatchesFormat("Position_PreGrouped", "spaceFormID", false, ColumnType.INTEGER);
			ValidateColumnMatchesFormat("Position_PreGrouped", "referenceFormID", false, ColumnType.INTEGER);
			ValidateColumnMatchesFormat("Position_PreGrouped", "lockLevel", true, ColumnType.TEXT, ValidateLockLevel);
			ValidateColumnMatchesFormat("Position_PreGrouped", "count", false, ColumnType.INTEGER);

			ValidateColumnMatchesFormat("Entity", "entityFormID", false, ColumnType.INTEGER);
			ValidateColumnMatchesFormat("Entity", "editorID", false, ColumnType.TEXT);
			ValidateColumnMatchesFormat("Entity", "signature", false, ColumnType.TEXT, ValidateSignature);
			ValidateColumnMatchesFormat("Entity", "percChanceNone", true, ColumnType.INTEGER);

			ValidateColumnMatchesFormat("Space", "spaceFormID", false, ColumnType.INTEGER);
			ValidateColumnMatchesFormat("Space", "spaceEditorID", false, ColumnType.TEXT);
			ValidateColumnMatchesFormat("Space", "spaceDisplayName", false, ColumnType.TEXT);

			ValidateColumnMatchesFormat("Space", "centerX", false, ColumnType.REAL);
			ValidateColumnMatchesFormat("Space", "centerY", false, ColumnType.REAL);
			ValidateColumnMatchesFormat("Space", "maxRange", false, ColumnType.REAL);

			ValidateColumnMatchesFormat("MapMarker", "x", false, ColumnType.REAL);
			ValidateColumnMatchesFormat("MapMarker", "y", false, ColumnType.REAL);
			ValidateColumnMatchesFormat("MapMarker", "label", false, ColumnType.TEXT);
			ValidateColumnMatchesFormat("MapMarker", "icon", false, ColumnType.TEXT, ValidateMapMarkerIcon);
			ValidateColumnMatchesFormat("MapMarker", "spaceFormID", false, ColumnType.INTEGER);

			ValidateColumnMatchesFormat("Location", "locationFormID", false, ColumnType.INTEGER);
			ValidateColumnMatchesFormat("Location", "npcName", false, ColumnType.TEXT);
			ValidateColumnMatchesFormat("Location", "npcClass", false, ColumnType.TEXT, ValidateNpcClass);
			ValidateColumnMatchesFormat("Location", "spawnWeight", false, ColumnType.REAL);

			ValidateColumnMatchesFormat("Region", "regionFormID", false, ColumnType.INTEGER);
			ValidateColumnMatchesFormat("Region", "regionEditorID", false, ColumnType.TEXT);
			ValidateColumnMatchesFormat("Region", "regionIndex", false, ColumnType.INTEGER);
			ValidateColumnMatchesFormat("Region", "coordIndex", false, ColumnType.INTEGER);
			ValidateColumnMatchesFormat("Region", "x", false, ColumnType.REAL);
			ValidateColumnMatchesFormat("Region", "y", false, ColumnType.REAL);
			ValidateColumnMatchesFormat("Region", "spaceFormID", false, ColumnType.INTEGER);

			ValidateColumnMatchesFormat("Scrap", "junkFormID", false, ColumnType.INTEGER);
			ValidateColumnMatchesFormat("Scrap", "component", false, ColumnType.TEXT, ValidateComponent);
			ValidateColumnMatchesFormat("Scrap", "componentQuantity", false, ColumnType.INTEGER);

			ValidateColumnMatchesFormat("Meta", "key", false, ColumnType.TEXT);
			ValidateColumnMatchesFormat("Meta", "value", false, ColumnType.TEXT);
		}

		// Validate all rows of the table column match the type and optional regex pattern, including optional blanks
		static void ValidateColumnMatchesFormat(string tableName, string columnName, bool allowBlanks, ColumnType type, Regex? validPattern = null)
		{
			Console.WriteLine($"Validating {tableName}.{columnName}");

			string readQuery = $"SELECT {columnName} FROM {tableName}";
			SqliteCommand readCommand = new SqliteCommand(readQuery, Connection);
			SqliteDataReader reader = readCommand.ExecuteReader();

			int row = 0;
			while (reader.Read())
			{
				row++;
				string value = reader.GetString(0);

				if (string.IsNullOrEmpty(value))
				{
					if (allowBlanks)
					{
						continue;
					}
					else
					{
						FailValidation($"{tableName}.{columnName} cannot contain blanks (Row {row})");
					}
				}

				if (validPattern != null && !validPattern.IsMatch(value))
				{
					FailValidation($"{tableName}.{columnName}: value {value} did not match pattern {validPattern} (Row {row})");
				}

				bool typeValidationFailed = false;
				switch (type)
				{
					case ColumnType.REAL:
						if (!double.TryParse(value, out _))
						{
							typeValidationFailed = true;
						}

						break;

					case ColumnType.INTEGER:
						if (!int.TryParse(value, out _))
						{
							typeValidationFailed = true;
						}

						break;

					case ColumnType.BOOL:
						if (!(value == "0" || value == "1"))
						{
							typeValidationFailed = true;
						}

						break;

					// Anything is valid string/text, so nothing to check
					case ColumnType.TEXT:
					default:
						break;
				}

				if (typeValidationFailed)
				{
					FailValidation($"{tableName}.{columnName}: value {value} could not be parsed as type {type} (Row {row})");
				}
			}
		}
	}
}
