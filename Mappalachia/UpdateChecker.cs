using System.Reflection;
using System.Text.Json;
using Library;

namespace Mappalachia
{
	static class UpdateChecker
	{
		static string UserAgent { get; } = "AHeroicLlama/Mappalachia";

		static Uri LatestReleaseAPI { get; } = new Uri("https://api.github.com/repos/AHeroicLlama/Mappalachia/releases/latest");

		static Uri ReleasesURL { get; } = new Uri("https://github.com/AHeroicLlama/Mappalachia/releases");

		public static async void CheckForUpdates(bool userRequested = false)
		{
			Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version ?? throw new Exception("Assembly Version is null");

			try
			{
				// Get the response from the GitHub API
				using HttpClient httpClient = new HttpClient();
				httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);
				using HttpResponseMessage response = await httpClient.GetAsync(LatestReleaseAPI);

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
					ShowUpdateDialog(currentVersion, errorReason: $"{e.GetType().Name}: {e.Message}");
				}

				return;
			}
		}

		// Displays the dialog which informs of the update check result
		static void ShowUpdateDialog(Version currentVersion, Version? latestVersion = null, string? downloadURL = null, string? releaseURL = null, string? patchNotes = null, string? errorReason = null)
		{
			// Set up the dialog with default common settings
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

			TaskDialogButton viewReleases = new TaskDialogButton("View releases on GitHub");
			viewReleases.Click += (sender, e) => { Common.OpenURI(ReleasesURL); };

			// Error
			if (errorReason != null)
			{
				page.Heading = $"An error prevented the update checker from identifying the latest version";
				page.Text = $"{errorReason}\n\nYour current version is {currentVersion}";
				page.Icon = TaskDialogIcon.Error;
				page.Buttons = new TaskDialogButtonCollection()
				{
					viewReleases,
					TaskDialogButton.OK,
				};
			}

			// Update available
			else if (currentVersion < latestVersion)
			{
				TaskDialogButton downloadNow = new TaskDialogButton("Download now");
				TaskDialogButton viewMore = new TaskDialogButton("View on GitHub");
				TaskDialogButton notNow = new TaskDialogButton("Remind me later");

				downloadNow.Click += (sender, e) => { Common.OpenURI(downloadURL ?? throw new Exception("Download URL is null")); };
				viewMore.Click += (sender, e) => { Common.OpenURI(releaseURL ?? throw new Exception("Release URL is null")); };
				notNow.Click += (sender, e) => { /* TODO - Store choice in settings */ };

				page.Heading = $"An new Mappalachia version, {latestVersion} is available.";
				page.Text = patchNotes;
				page.DefaultButton = downloadNow;
				page.Buttons =
				new TaskDialogButtonCollection
				{
					downloadNow,
					viewMore,
					notNow,
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
					viewReleases,
					TaskDialogButton.OK,
				};
			}

			TaskDialog.ShowDialog(page);
		}
	}
}
