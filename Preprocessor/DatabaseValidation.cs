using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;
using static Library.BuildTools;

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

			ValidateColumnMatchesFormat("Position", "lockLevel", true, ValidateLockLevel);
			ValidateColumnMatchesFormat("Position", "primitiveShape", true, ValidatePrimitiveShape);
			ValidateColumnMatchesFormat("Position_PreGrouped", "lockLevel", true, ValidateLockLevel);
			ValidateColumnMatchesFormat("Entity", "signature", false, ValidateSignature);
			ValidateColumnMatchesFormat("MapMarker", "icon", false, ValidateMapMarkerIcon);
			ValidateColumnMatchesFormat("Scrap", "component", false, ValidateComponent);
		}

		// Validate all rows of the table column match the regex pattern, including optional blanks
		static void ValidateColumnMatchesFormat(string tableName, string columnName, bool allowBlanks, Regex validPattern)
		{
			Console.WriteLine($"Validating {tableName}.{columnName}");

			string readQuery = $"SELECT {columnName} FROM {tableName}";
			SqliteCommand readCommand = new SqliteCommand(readQuery, Connection);
			SqliteDataReader reader = readCommand.ExecuteReader();

			int row = 0;
			while (reader.Read())
			{
				row++;

				string? value;
				if (reader.IsDBNull(0))
				{
					value = null;
				}
				else
				{
					value = reader.GetString(0);
				}

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

				if (value != null && !validPattern.IsMatch(value))
				{
					FailValidation($"{tableName}.{columnName}: value {value} did not match pattern {validPattern} (Row {row})");
				}
			}
		}
	}
}
