using MappalachiaLibrary;
using Microsoft.Data.Sqlite;
using System.Text.RegularExpressions;

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

		static Regex lockLevel { get; } = new Regex("^(Advanced \\(Level 1\\)|Chained|Expert \\(Level 2\\)|Inaccessible|Master \\(Level 3\\)|Novice \\(Level 0\\)|Requires Key|Requires Terminal|Unknown|Barred)$");
		static Regex primitiveShape { get; } = new Regex("^(Box|Line|Plane|Sphere|Ellipsoid)$");

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

			ValidateColumnMatchesFormat("Position", "spaceFormID", false, AcceptableType.Int);
			ValidateColumnMatchesFormat("Position", "x", false, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("Position", "y", false, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("Position", "z", false, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("Position", "lockLevel", true, AcceptableType.String, lockLevel);
			ValidateColumnMatchesFormat("Position", "primitiveShape", true, AcceptableType.String, primitiveShape);
			ValidateColumnMatchesFormat("Position", "boundX", true, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("Position", "boundY", true, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("Position", "boundZ", true, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("Position", "rotZ", true, AcceptableType.Decimal);
			ValidateColumnMatchesFormat("Position", "referenceFormID", false, AcceptableType.Int);
			ValidateColumnMatchesFormat("Position", "locationFormID", true, AcceptableType.Int);
			ValidateColumnMatchesFormat("Position", "teleportsToFormID", true, AcceptableType.Int);
			//ValidateColumnMatchesFormat("Position", "label", true, AcceptableType.String);
			ValidateColumnMatchesFormat("Position", "instanceFormID", false, AcceptableType.Int);

			ConcludeValidation();
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

		static void ConcludeValidation()
		{
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
	}
}
