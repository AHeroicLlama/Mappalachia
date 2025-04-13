namespace Mappalachia
{
	internal static class Mappalachia
	{
		[STAThread]
		static void Main()
		{
			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();
			Console.WriteLine("Dedicated to Molly.");
			UpdateChecker.CheckForUpdates();

			Task mainFormTask = Task.Run(() => Application.Run(new FormMain()));
			Task mapViewFormTask = Task.Run(() => Application.Run(new FormMapView()));

			Task.WaitAny(mainFormTask, mapViewFormTask);
		}
	}
}