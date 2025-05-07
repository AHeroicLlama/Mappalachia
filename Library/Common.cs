using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Library
{
	public static class Common
	{
		public static string DistributableFileName { get; } = "Mappalachia.zip";

		public static string BackgroundImageFileType { get; } = ".jpg";

		public static string MaskImageFileType { get; } = ".png";

		public static string WaterMaskAddendum { get; } = "_waterMask";

		public static string MenuAddendum { get; } = "_menu";

		public static string MilitaryAddendum { get; } = "_military";

		public static int MapImageResolution { get; } = (int)Math.Pow(2, 12); // 4096

		public static int SuperResTileSize { get; } = (int)Math.Pow(2, 12); // 4096

		public static int SuperResScale { get; } = 2; // The ratios of game coordinates to pixels, given in 1:x^2 (or the width in cells for each tile (x*x arrangement)) used for super resolution

		// The size in game coordinates of super res tiles
		public static int TileWidth { get; } = SuperResTileSize * SuperResScale;

		public static int TileRadius { get; } = TileWidth / 2;

		static Regex FormID { get; } = new Regex("^(0[Xx])?([0-9A-Fa-f]{1,8})$");

		// Return if this string appears to represent a FormID in hexadecimal
		public static bool IsHexFormID(this string formID)
		{
			return FormID.IsMatch(formID);
		}

		public static string ToHex(this uint formID)
		{
			return formID.ToString("X8");
		}

		public static uint HexToInt(string hex)
		{
			return Convert.ToUInt32(FormID.Matches(hex)[0].Value, 16);
		}

		public static string WithoutWhitespace(this string input)
		{
			return new string(input.Where(character => !char.IsWhiteSpace(character)).ToArray());
		}

		// Opens the web page or file path with the default application
		public static void OpenURI(string uri)
		{
			Process.Start(new ProcessStartInfo { FileName = uri, UseShellExecute = true });
		}

		public static void OpenURI(Uri uri)
		{
			OpenURI(uri.ToString());
		}

		public static string Pluralize<T>(IEnumerable<T> collection)
		{
			return collection.Count() == 1 ? string.Empty : "s";
		}

		public static bool EqualsIgnoreCase(this string value, string comparison)
		{
			return value.Equals(comparison, StringComparison.OrdinalIgnoreCase);
		}

		public static bool IsNullOrWhiteSpace(this string value)
		{
			return string.IsNullOrWhiteSpace(value);
		}
	}
}
