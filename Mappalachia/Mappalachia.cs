namespace Mappalachia
{
	static class Mappalachia
	{
		public static MapSettings Settings { get; } = new MapSettings();

		[STAThread]
		static void Main()
		{
			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();
			Console.WriteLine("Dedicated to Molly.");
			UpdateChecker.CheckForUpdates();

			Application.Run(new FormMain());
		}
	}
}