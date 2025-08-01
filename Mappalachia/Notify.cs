using Library;

namespace Mappalachia
{
	public static class Notify
	{
		public static void GenericError(string summary, string body, Exception? exception = null, string title = "Mappalachia: Error")
		{
			if (!Mappalachia.GUILaunched)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(summary);
				Console.WriteLine(body);
				Console.WriteLine(exception);
				Console.ResetColor();
				return;
			}

			if (exception != null)
			{
				body += $"\n\nError details:\n{exception.Message}";
			}

			TaskDialogPage page = new TaskDialogPage
			{
				Caption = title,
				Heading = summary,
				Text = body,
				Icon = TaskDialogIcon.Error,
				AllowCancel = true,
				DefaultButton = TaskDialogButton.OK,
				Buttons = new TaskDialogButtonCollection()
				{
					TaskDialogButton.OK,
				},
			};

			TaskDialog.ShowDialog(page);
		}

		public static void Info(string title, string summary, string body)
		{
			if (!Mappalachia.GUILaunched)
			{
				Console.WriteLine(summary);
				Console.WriteLine(body);
				Console.ResetColor();
				return;
			}

			TaskDialogPage page = new TaskDialogPage
			{
				Caption = title,
				Heading = summary,
				Text = body,
				Icon = TaskDialogIcon.Information,
				AllowCancel = true,
				DefaultButton = TaskDialogButton.OK,
				Buttons = new TaskDialogButtonCollection()
				{
					TaskDialogButton.OK,
				},
			};

			TaskDialog.ShowDialog(page);
		}

		public static void SpotlightInstallationPrompt()
		{
			if (!Mappalachia.GUILaunched)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Spotlight is not installed! Unable to be used. Please install it and try again.");
				Console.ResetColor();
				return;
			}

			TaskDialogButton openInstallGuide = new TaskDialogButton("Open installation guide");
			openInstallGuide.Click += (sender, e) => { Common.OpenURI(URLs.SpotlightInstallationGuide); };

			TaskDialogPage page = new TaskDialogPage
			{
				Caption = "Spotlight",
				Heading = "Spotlight is not installed",
				Text = "Spotlight mode is a free feature available through an optional download. It appears you have not installed it yet, however.\n\nIf you'd like to use spotlight, please use the button below to follow the quick installation guide, before trying again.",
				Icon = TaskDialogIcon.Warning,
				AllowCancel = true,
				DefaultButton = openInstallGuide,
				Buttons = new TaskDialogButtonCollection()
				{
					openInstallGuide,
					TaskDialogButton.Cancel,
				},
			};

			TaskDialog.ShowDialog(page);
		}

		public static void FatalException(Exception exception)
		{
			if (!Mappalachia.GUILaunched)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"Fatal!\n{exception}");
				Console.ResetColor();
				return;
			}

			TaskDialogButton discordButton = new TaskDialogButton("Join Discord")
			{
				AllowCloseDialog = false,
			};

			TaskDialogButton copyDetailsButton = new TaskDialogButton("Copy details to clipboard")
			{
				AllowCloseDialog = false,
			};

			TaskDialogButton resetSettingsButton = new TaskDialogButton("Try again with reset settings");

			discordButton.Click += (sender, e) => { Common.OpenURI(URLs.DiscordInvite); };
			copyDetailsButton.Click += (sender, e) => { Clipboard.SetText(exception.ToString()); };
			resetSettingsButton.Click += (sender, e) =>
			{
				if (File.Exists(Paths.SettingsPath))
				{
					try
					{
						File.Delete(Paths.SettingsPath);
						Application.Restart();
					}

					// No action
					catch
					{
					}
				}
			};

			resetSettingsButton.Enabled = File.Exists(Paths.SettingsPath);

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
						resetSettingsButton,
						copyDetailsButton,
						TaskDialogButton.OK,
					},
			};

			TaskDialog.ShowDialog(page);
		}
	}
}
