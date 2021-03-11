using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mappalachia
{
	public static class Validation
	{
		public static readonly Regex headersToEscape = new Regex("[Ee]ditorID|[Dd]isplayName");
		public static readonly Regex propertyWithoutFormID = new Regex("(.*) \\[.{4}:.{8}\\]");
		public static readonly Regex componentNameOnly = new Regex(".* \"(.*)\" .*");
		public static readonly Regex formIdOnly = new Regex("\\[.{4}:(.{8})\\]");
		public static readonly Regex matchFormID = new Regex("[0-9A-F]{8}");
		public static readonly Regex validNpcClass = new Regex("^(Main|Sub)$");
		public static readonly Regex validPrimitiveShape = new Regex("^(Box|Sphere|Line|Plane|Ellipsoid)$");
		public static readonly Regex validLockLevel = new Regex("^(Advanced \\(Level 1\\)|Chained|Expert \\(Level 2\\)|Inaccessible|Master \\(Level 3\\)|Novice \\(Level 0\\)|Opens Door|Requires Key|Requires Terminal|Unknown|Barred)$");
		public static readonly Regex validSignature = new Regex("[A-Z_]{4}");
		public static readonly Regex unescapedDoubleQuote = new Regex("(?<!\\\\)(\")");

		public static readonly List<string> decimalHeaders = new List<string> { "x", "y", "z", "boundX", "boundY", "boundZ", "rotZ" };
	}
}
