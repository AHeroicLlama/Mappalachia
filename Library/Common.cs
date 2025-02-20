using System.Diagnostics;

namespace Library
{
	public static class Common
	{
		public static int MapImageResolution { get; } = (int)Math.Pow(2, 12); // 4096

		public static int SuperResTileSize { get; } = (int)Math.Pow(2, 12); // 4096

		public static int SuperResScale { get; } = 1; // The ratios of game coordinates to pixels, given in 1:x^2 (or the width in cells for each tile (x*x arrangement)) used for super resolution

		// The size in game coordinates of super res tiles
		public static int TileWidth { get; } = SuperResTileSize * SuperResScale;

		public static int TileRadius { get; } = TileWidth / 2;

		public static string ToHex(this uint formID)
		{
			return formID.ToString("X8");
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

		public static string Pluralize<T>(IEnumerable<T> collection)
		{
			return collection.Count() == 1 ? string.Empty : "s";
		}
	}
}
