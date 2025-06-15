using Library;

namespace Mappalachia
{
	static class Mappalachia
	{
		static FormMain? FormMain { get; set; } = null;

		[STAThread]
		static void Main()
		{
			try
			{
				// To customize application configuration such as set high DPI settings or default font,
				// see https://aka.ms/applicationconfiguration.
				ApplicationConfiguration.Initialize();
				Console.WriteLine("Dedicated to Molly.");

				FormMain = new FormMain();
				Application.Run(FormMain);
			}
			catch (Exception exception)
			{
				TaskDialogButton discordButton = new TaskDialogButton("Join Discord")
				{
					AllowCloseDialog = false,
				};

				TaskDialogButton copyDetailsButton = new TaskDialogButton("Copy details to clipboard")
				{
					AllowCloseDialog = false,
				};

				discordButton.Click += (sender, e) => { Common.OpenURI(URLs.DiscordInvite); };
				copyDetailsButton.Click += (sender, e) => { Clipboard.SetText(exception.ToString()); };

				TaskDialogPage page = new TaskDialogPage
				{
					Caption = "Mappalachia",
					Heading = "Unexpected error",
					Text = $"An unexpected error has occurred and Mappalachia must close.\n" +
					$"If this keeps happening, please ensure you have properly unzipped all files from the latest release.\n" +
					$"If you need support, join our Discord with the button below.\n\n" +
					$"Error details:\n{exception.Message}",
					Icon = TaskDialogIcon.Error,
					AllowCancel = true,
					DefaultButton = TaskDialogButton.OK,
					Buttons = new TaskDialogButtonCollection()
					{
						discordButton,
						copyDetailsButton,
						TaskDialogButton.OK,
					},
				};

				TaskDialog.ShowDialog(page);

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