using Library;

namespace Mappalachia
{
	static class Mappalachia
	{
		static FormMain? FormMain { get; set; } = null;

		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				// To customize application configuration such as set high DPI settings or default font,
				// see https://aka.ms/applicationconfiguration.
				ApplicationConfiguration.Initialize();

				if (args.Length > 0 && args[0].EqualsIgnoreCase("dedication"))
				{
					Console.WriteLine("Dedicated to Molly.");
				}

				FormMain = new FormMain();
				Application.Run(FormMain);
			}
			catch (Exception exception)
			{
				Notify.FatalException(exception);
				Application.Exit();
			}
		}

		// Hack so the 'advanced' setting can be fetched globally where references to Settings are not available
		// This is used where the DataGridView DataProperty properties are set via their name, and therefore cannot have arguments passed to them
		public static bool GetIsAdvanced()
		{
			return FormMain?.Settings.SearchSettings.Advanced ?? throw new NullReferenceException("Main Form was null");
		}
	}
}