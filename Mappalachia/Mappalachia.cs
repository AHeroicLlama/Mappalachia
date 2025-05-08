namespace Mappalachia
{
	static class Mappalachia
	{
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