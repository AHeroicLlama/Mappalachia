using System.Diagnostics;

namespace Library
{
	public static class Misc
	{
		public static int Kilobyte { get; } = (int)Math.Pow(2, 10);

		public static int MapImageResolution { get; } = (int)Math.Pow(2, 12);

		public static string ToHex(this uint formID)
		{
			return formID.ToString("X8");
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
