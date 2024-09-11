using System.Text.RegularExpressions;
using MappalachiaLibrary;
using Microsoft.Data.Sqlite;

namespace Preprocessor
{
	internal partial class Preprocessor
	{
		enum AcceptableType
		{
			String,
			Decimal,
			Int,
			Bool,
		}

		static List<string> ValidationFailures { get; } = new List<string>();

		static void Validate()
		{
			Console.WriteLine("Validating database");

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
			ValidateColumnMatchesFormat("Position", "spaceFormID", false, AcceptableType.Int);
			ValidateColumnMatchesFormat("Position", "x", false, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("Position", "y", false, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("Position", "z", false, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("Position", "lockLevel", true, AcceptableType.String, ValidateLockLevel);
			ValidateColumnMatchesFormat("Position", "primitiveShape", true, AcceptableType.String, ValidatePrimitiveShape);
			ValidateColumnMatchesFormat("Position", "boundX", true, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("Position", "boundY", true, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("Position", "boundZ", true, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("Position", "rotZ", true, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("Position", "referenceFormID", false, AcceptableType.Int);
			ValidateColumnMatchesFormat("Position", "locationFormID", true, AcceptableType.Int);
			ValidateColumnMatchesFormat("Position", "teleportsToFormID", true, AcceptableType.Int);
			ValidateColumnMatchesFormat("Position", "instanceFormID", false, AcceptableType.Int);

			ValidateColumnMatchesFormat("Position_PreGrouped", "spaceFormID", false, AcceptableType.Int);
			ValidateColumnMatchesFormat("Position_PreGrouped", "referenceFormID", false, AcceptableType.Int);
			ValidateColumnMatchesFormat("Position_PreGrouped", "lockLevel", true, AcceptableType.String, ValidateLockLevel);
			ValidateColumnMatchesFormat("Position_PreGrouped", "count", false, AcceptableType.Int);

			ValidateColumnMatchesFormat("Entity", "entityFormID", false, AcceptableType.Int);
			ValidateColumnMatchesFormat("Entity", "editorID", false, AcceptableType.String);
			ValidateColumnMatchesFormat("Entity", "signature", false, AcceptableType.String, ValidateSignature);
			ValidateColumnMatchesFormat("Entity", "percChanceNone", true, AcceptableType.Int);

			ValidateColumnMatchesFormat("Space", "spaceFormID", false, AcceptableType.Int);
			ValidateColumnMatchesFormat("Space", "spaceEditorID", false, AcceptableType.String);
			ValidateColumnMatchesFormat("Space", "spaceDisplayName", false, AcceptableType.String);

			ValidateColumnMatchesFormat("Space", "minX", false, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("Space", "maxX", false, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("Space", "midX", false, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("Space", "minY", false, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("Space", "maxY", false, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("Space", "midY", false, AcceptableType.Decimal);

			ValidateColumnMatchesFormat("MapMarker", "x", false, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("MapMarker", "y", false, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("MapMarker", "label", false, AcceptableType.String);
			ValidateColumnMatchesFormat("MapMarker", "icon", false, AcceptableType.String, ValidateMapMarkerIcon);
			ValidateColumnMatchesFormat("MapMarker", "spaceFormID", false, AcceptableType.Int);

			ValidateColumnMatchesFormat("Location", "locationFormID", false, AcceptableType.Int);
			ValidateColumnMatchesFormat("Location", "npcName", false, AcceptableType.String);
			ValidateColumnMatchesFormat("Location", "npcClass", false, AcceptableType.String, ValidateNpcClass);
			ValidateColumnMatchesFormat("Location", "spawnWeight", false, AcceptableType.Decimal);

			ValidateColumnMatchesFormat("Region", "regionFormID", false, AcceptableType.Int);
			ValidateColumnMatchesFormat("Region", "regionEditorID", false, AcceptableType.String);
			ValidateColumnMatchesFormat("Region", "regionIndex", false, AcceptableType.Int);
			ValidateColumnMatchesFormat("Region", "coordIndex", false, AcceptableType.Int);
			ValidateColumnMatchesFormat("Region", "x", false, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("Region", "y", false, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("Region", "spaceFormID", false, AcceptableType.Int);

			ValidateColumnMatchesFormat("Scrap", "junkFormID", false, AcceptableType.Int);
			ValidateColumnMatchesFormat("Scrap", "component", false, AcceptableType.String, ValidateComponent);
			ValidateColumnMatchesFormat("Scrap", "componentQuantity", false, AcceptableType.Int);

			ValidateColumnMatchesFormat("Meta", "key", false, AcceptableType.String);
			ValidateColumnMatchesFormat("Meta", "value", false, AcceptableType.String);

			ConsoleColor originalColor = Console.ForegroundColor;
			if (ValidationFailures.Count > 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;

				Console.WriteLine("Data validation failed. The following errors were reported:");
				ValidationFailures.ForEach(Console.WriteLine);
				File.WriteAllLines(BuildPaths.GetErrorsPath(), ValidationFailures);
				Console.WriteLine($"Error details stored to {BuildPaths.GetErrorsPath()}");
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Validation passed!");
			}

			Console.ForegroundColor = originalColor;
		}

		// Validate all rows of the table column match the type and optional regex pattern, including optional blanks
		static void ValidateColumnMatchesFormat(string tableName, string columnName, bool allowBlanks, AcceptableType type, Regex? validPattern = null)
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
					case AcceptableType.Decimal:
						if (!double.TryParse(value, out _))
						{
							typeValidationFailed = true;
						}

						break;

					case AcceptableType.Int:
						if (!int.TryParse(value, out _))
						{
							typeValidationFailed = true;
						}

						break;

					case AcceptableType.Bool:
						if (!(value == "0" || value == "1"))
						{
							typeValidationFailed = true;
						}

						break;

					// Everything will map to string so nothing to check
					case AcceptableType.String:
					default:
						break;
				}

				if (typeValidationFailed)
				{
					FailValidation($"{tableName}.{columnName}: value {value} could not be parsed as type {type} (Row {row})");
				}
			}
		}

		static void FailValidation(string reason)
		{
			ConsoleColor originalColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;

			Console.WriteLine($"Validation failure: {reason}");
			ValidationFailures.Add(reason);

			Console.ForegroundColor = originalColor;
		}
	}
}
