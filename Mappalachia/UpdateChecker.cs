using System.Reflection;
using System.Text.Json;
using Library;

namespace Mappalachia
{
	static class UpdateChecker
	{
		static string UserAgent { get; } = "AHeroicLlama/Mappalachia";

		static TaskDialogButton GetViewReleasesButton()
		{
			TaskDialogButton button = new TaskDialogButton("View releases on GitHub");
			button.Click += (sender, e) => { Common.OpenURI(URLs.Releases); };
			return button;
		}

		public static async void CheckForUpdates(bool userRequested = false)
		{
			Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version ?? throw new Exception("Assembly Version is null");

			try
			{
				// Get the response from the GitHub API
				using HttpClient httpClient = new HttpClient();
				httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);
				using HttpResponseMessage response = await httpClient.GetAsync(URLs.LatestReleaseAPI);

				if (!response.IsSuccessStatusCode)
				{
					throw new HttpRequestException(response.StatusCode.ToString());
				}

				// Parse the details out the response
				JsonElement responseRoot = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
				Version latestVersion = new Version(responseRoot.GetProperty("tag_name").ToString());
				string patchNotes = responseRoot.GetProperty("body").ToString().Replace("### ", string.Empty);
				string releaseURL = responseRoot.GetProperty("html_url").ToString();
				string downloadURL = responseRoot.GetProperty("assets")
					.EnumerateArray()
					.SingleOrDefault(asset => asset.GetProperty("name").ToString() == Common.DistributableFileName)
					.GetProperty("browser_download_url").ToString();

				// If there's no update, and the user didn't request the check, silently exit
				if (!userRequested && currentVersion >= latestVersion)
				{
					return;
				}

				ShowUpdateDialog(currentVersion, latestVersion, downloadURL, releaseURL, patchNotes);
			}
			catch (Exception e)
			{
				if (userRequested)
				{
					ShowErrorDialog(currentVersion, $"{e.GetType().Name}: {e.Message}");
				}

				return;
			}
		}

		// Displays the dialog which informs about the error while checking for updates
		static void ShowErrorDialog(Version currentVersion, string errorReason)
		{
			TaskDialogPage page = new TaskDialogPage
			{
				Caption = "Update Checker",
				Heading = $"An error prevented the update checker from identifying the latest version",
				Text = $"{errorReason}\n\nYour current version is {currentVersion}",
				Icon = TaskDialogIcon.Error,
				AllowCancel = true,
				DefaultButton = TaskDialogButton.OK,
				Buttons = new TaskDialogButtonCollection()
				{
					GetViewReleasesButton(),
					TaskDialogButton.OK,
				},
			};

			TaskDialog.ShowDialog(page);
		}

		// Displays the dialog which informs of the successful update check result
		static void ShowUpdateDialog(Version currentVersion, Version latestVersion, string downloadURL, string releaseURL, string patchNotes)
		{
			// Create a default page
			TaskDialogPage page = new TaskDialogPage()
			{
				Caption = "Update Checker",
				Icon = TaskDialogIcon.Information,
				AllowCancel = true,
				DefaultButton = TaskDialogButton.OK,
				Buttons =
				{
					TaskDialogButton.OK,
				},
			};

			// Update available
			if (currentVersion < latestVersion)
			{
				TaskDialogButton buttonDownloadNow = new TaskDialogButton("Download now");
				TaskDialogButton buttonViewMore = new TaskDialogButton("View on GitHub");
				TaskDialogButton buttonRemindLater = new TaskDialogButton("Remind me later");

				buttonDownloadNow.Click += (sender, e) => { Common.OpenURI(downloadURL ?? throw new Exception("Download URL is null")); };
				buttonViewMore.Click += (sender, e) => { Common.OpenURI(releaseURL ?? throw new Exception("Release URL is null")); };
				buttonRemindLater.Click += (sender, e) => { /* TODO - Store choice in settings */ };

				page.Heading = $"An new Mappalachia version, {latestVersion} is available.";
				page.Text = patchNotes;
				page.DefaultButton = buttonDownloadNow;
				page.Buttons =
				new TaskDialogButtonCollection
				{
					buttonDownloadNow,
					buttonViewMore,
					buttonRemindLater,
				};
			}

			// On the latest version
			else if (currentVersion == latestVersion)
			{
				page.Heading = $"Up To Date";
				page.Text = $"You are already on the latest version ({currentVersion}).";
				page.Icon = TaskDialogIcon.ShieldSuccessGreenBar;
			}

			// Ahead of the latest release (Dev build or release deleted?)
			else if (currentVersion > latestVersion)
			{
				page.Text = $"This build ({currentVersion}) is ahead of the latest release ({latestVersion}).";
				page.Buttons = new TaskDialogButtonCollection
				{
					GetViewReleasesButton(),
					TaskDialogButton.OK,
				};
			}

			TaskDialog.ShowDialog(page);
		}
	}
}
