namespace Mappalachia
{
	public static class Notify
	{
		// Displays a generic error alert
		public static void GenericError(string summary, string body, Exception? exception = null, string title = "Mappalachia: Error")
		{
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
	}
}
