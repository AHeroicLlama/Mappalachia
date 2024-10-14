namespace Library
{
	public static class Misc
	{
		public static int MapImageResolution { get; } = (int)Math.Pow(2, 12);

		public static int Kilobyte { get; } = (int)Math.Pow(2, 10);

		public static string ToHex(this int formID)
		{
			return formID.ToString("X8");
		}
	}
}
