using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CommonwealthCartography
{
	public static class Validation
	{
		public static readonly Regex headersToEscape = new Regex("[Ee]ditorID|[Dd]isplayName");
		public static readonly Regex propertyWithoutFormID = new Regex("(.*) \\[[A-Z_]{4}:[0-9A-F]{8}\\]");
		public static readonly Regex componentNameOnly = new Regex(".* \"(.*)\" .*");
		public static readonly Regex formIdOnly = new Regex("\\[[A-Z_]{4}:([0-9A-F]{8})\\]");
		public static readonly Regex matchFormID = new Regex("[0-9A-F]{8}");
		public static readonly Regex validPrimitiveShape = new Regex("^(Box|Line|Plane|Sphere|Ellipsoid)$");
		public static readonly Regex validLockLevel = new Regex("^(Advanced|Chained|Expert|Inaccessible|Master|Novice (1|25)|Requires Key|Requires Terminal|Unknown|Barred)$");
		public static readonly Regex validSignature = new Regex("[A-Z_]{4}");
		public static readonly Regex validSQLiteBoolean = new Regex("^(0|1)$");
		public static readonly Regex unescapedDoubleQuote = new Regex("(?<!\\\\)(\")");
		public static readonly Regex shortNameIsolateRef = new Regex("^(.*) ?\\[[A-Z_]{4}:[0-9A-F]{8}\\]$");
		public static readonly Regex shortNameGetRef = new Regex("^.* ?\\[[A-Z_]{4}:([0-9A-F]{8})\\]$");

		public static readonly List<string> decimalHeaders = new List<string> { "x", "y", "z", "boundX", "boundY", "boundZ", "rotZ" };
	}
}
