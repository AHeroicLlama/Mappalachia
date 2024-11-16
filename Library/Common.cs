using System.Diagnostics;

namespace Library
{
	public static class Common
	{
		public static int MapImageResolution { get; } = (int)Math.Pow(2, 12); // 4096

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
