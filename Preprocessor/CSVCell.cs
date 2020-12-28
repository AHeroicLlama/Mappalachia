using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mappalachia
{
	class CSVCell
	{
		public string data;
		public string columnName;

		static readonly Regex headersToEscape = new Regex("[Ee]ditorID|[Dd]isplayName");
		static readonly Regex propertyWithoutFormID = new Regex("(.*) \\[.{4}:.{8}\\]");
		static readonly Regex componentNameOnly = new Regex(".* \"(.*)\" .*");
		static readonly Regex formIdOnly = new Regex("\\[.{4}:(.{8})\\]");
		static readonly Regex matchFormID = new Regex("[0-9A-F]{8}");
		static readonly Regex validNpcClass = new Regex("^(Main|Sub)$");
		static readonly Regex validPrimitiveShape = new Regex("^(Box|Sphere)$");
		static readonly Regex validLockLevel = new Regex("^(Advanced \\(Level 1\\)|Chained|Expert \\(Level 2\\)|Inaccessible|Master \\(Level 3\\)|Novice \\(Level 0\\)|Opens Door|Requires Key|Requires Terminal|Unknown|Barred)$");
		static readonly Regex validSignature = new Regex("[A-Z_]{4}");
		static readonly Regex unescapedDoubleQuote = new Regex("(?<!\\\\)(\")");

		static readonly List<string> decimalHeaders = new List<string> { "x", "y", "boundX", "boundY" };

		public CSVCell(string data, string columnName)
		{
			this.data = data;
			this.columnName = columnName;
		}

		public override string ToString()
		{
			return data;
		}

		//Escape problematic SQL characters
		public void Sanitize()
		{
			if (headersToEscape.IsMatch(columnName))
			{
				//Replace (") with (\") and (') with ('')
				data = data.Replace("\"", "\\\"").Replace("'", "''");
			}
		}

		//Reduce long references and names to just the data required in them - typically just the FormID
		public void ReduceReferences()
		{
			//Grab everything before the FormID
			if (columnName == "property")
			{
				Match match = propertyWithoutFormID.Match(data);
				if (match.Success)
				{
					data = match.Groups[1].Value;
				}
			}

			//Grab whatever is in quotes surrounded by spaces
			else if (columnName.Contains("component"))
			{
				Match match = componentNameOnly.Match(data);
				if (match.Success)
				{
					data = match.Groups[1].Value;
				}
			}

			//Grab the 8-char FormID only
			else
			{
				Match match = formIdOnly.Match(data);
				if (match.Success)
				{
					data = match.Groups[1].Value;
				}
			}

			//Niche case where worldSpace Xmarkers get their reference filled with the literal Map marker text
			if (columnName == "referenceFormID" && !matchFormID.IsMatch(data))
			{
				data = "FFFFFFFF";
			}
		}

		//Reduce large decimals to integers (The precision is unnecessary due to downscaling)
		public void ReduceDecimals()
		{
			if (decimalHeaders.Contains(columnName))
			{
				if (data != string.Empty)
				{
					try
					{
						//Parse the string as a double, round it, cast it to an int, then cast it back to a string.
						data = ((int)Math.Round(double.Parse(data))).ToString();
					}
					catch (Exception)
					{
						ReportValidationError();
					}
				}
			}
		}

		//Run basic data integrity checks on expected values in columns
		//Check for any unexpected values in any cell based on expected values for its column
		public void Validate()
		{
			switch (columnName)
			{
				case "entityFormID":
				case "referenceFormID":
				case "cellFormID":
				case "junkFormID":
					//FormID Cells must only contain the FormID alone
					if (!matchFormID.IsMatch(data))
					{
						ReportValidationError();
					}
					return;

				case "locationFormID":
					//locationFormID may be empty if not a FormID
					if (data != string.Empty && !matchFormID.IsMatch(data))
					{
						ReportValidationError();
					}
					return;

				case "signature":
					//Signatures are always exactly 4 letters or underscores
					if (!validSignature.IsMatch(data))
					{
						ReportValidationError();
					}
					return;

				case "editorID":
				case "cellEditorID":
				case "displayName":
				case "cellDisplayName":
					//editorID and displayName must have had any double quotes escaped by now
					if (unescapedDoubleQuote.IsMatch(data))
					{
						ReportValidationError();
					}
					return;

				case "componentQuantity":
					//Component quantity must be an integer
					if (!int.TryParse(data, out _))
					{
						ReportValidationError();
					}
					return;

				case "chance":
					//Coordinate cells must be exactly doubles
					if (!double.TryParse(data, out _))
					{
						ReportValidationError();
					}
					return;

				case "x":
				case "y":
					//Coordinate cells must be exactly integers
					if (!int.TryParse(data, out _))
					{
						ReportValidationError();
					}
					return;

				case "boundX":
				case "boundY":
					//Boundary dimensions may be blank or otherwise integers
					if (data != string.Empty && !int.TryParse(data, out _))
					{
						ReportValidationError();
					}
					return;

				case "primitiveShape":
					//A primitive shape can only be these known types or empty
					if (data != string.Empty && !validPrimitiveShape.IsMatch(data))
					{
						ReportValidationError();
					}
					return;

				case "lockLevel":
					//Lock level must come from a set of strings or be empty
					if (data != string.Empty && !validLockLevel.IsMatch(data))
					{
						ReportValidationError();
					}
					return;

				case "spawnClass":
					//Class (monster) can only be two exact strings or empty
					if (data != string.Empty && !validNpcClass.IsMatch(data))
					{
						ReportValidationError();
					}
					return;

				case "npc":
				case "component":
					//Nothing really to validate for here since they're just in-game strings
					return;

				default:
					throw new Exception("Column " + columnName + " was not accounted for in validation");
			}
		}

		//Fail cell validation with an exception
		void ReportValidationError()
		{
			throw new Exception("Data of \"" + data + "\" is not valid or was otherwise unexpected or unsupported. At column \"" + columnName + "\"");
		}
	}
}