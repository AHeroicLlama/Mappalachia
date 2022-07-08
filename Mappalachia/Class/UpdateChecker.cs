using System;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Windows.Forms;

namespace Mappalachia.Class
{
	public static class UpdateChecker
	{
		static readonly Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

		public static async void CheckForUpdate(bool userTriggered)
		{
			HttpResponseMessage response;

			// Get the GitHub API response
			try
			{
				// Get the latest release info from GitHub API
				HttpClient httpClient = new HttpClient();
				httpClient.DefaultRequestHeaders.Add("User-Agent", "AHeroicLlama/Mappalachia");
				response = await httpClient.GetAsync("https://api.github.com/repos/AHeroicLlama/Mappalachia/releases/latest");

				if (!response.IsSuccessStatusCode)
				{
					if (userTriggered)
					{
						CheckForUpdatesManual("HTTP request error: " + response.StatusCode);
					}

					return;
				}
			}
			catch (Exception)
			{
				if (userTriggered)
				{
					CheckForUpdatesManual("Networking error attempting to check for updates.");
				}

				return;
			}

			// Parse the latest version, and compare to latest
			try
			{
				JsonDocument responseDocument = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
				string latestVersionString = responseDocument.RootElement.GetProperty("tag_name").ToString();
				Version latestVersion = new Version(latestVersionString);

				if (currentVersion < latestVersion)
				{
					PromptForUpdate(latestVersion.ToString());
				}
				else if (currentVersion > latestVersion)
				{
					Console.WriteLine($"Unreleased build {currentVersion}");

					if (userTriggered)
					{
						Notify.Info("This build is ahead of the latest release.");
					}
				}
				else if (userTriggered)
				{
					Notify.Info("Mappalachia is up to date.\n" +
						"(Version " + currentVersion + ")");
				}
			}
			catch (Exception)
			{
				if (userTriggered)
				{
					CheckForUpdatesManual("Unable to parse latest release from GitHub API response.");
				}

				return;
			}
		}

		// Prompt the user for a manual update check via releases page, after an auto check failed.
		// Also displays provided reason why automatic check failed and the current Mappalachia version.
		static void CheckForUpdatesManual(string errorReason)
		{
			DialogResult question = MessageBox.Show(
				"Automatic update checking failed: \n" + errorReason + "\n\nWould you like to visit the releases page to check for updates manually?\n" +
					"Your current version is " + currentVersion,
				"Check manually?", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
			if (question == DialogResult.Yes)
			{
				GoToReleases();
			}
		}

		// Tell the user the latest version is available as an update, and prompt to download
		static void PromptForUpdate(string latestVersion)
		{
			DialogResult question = MessageBox.Show(
				"A new version of Mappalachia, " + latestVersion + " is now available! \nVisit the releases page to download it?",
				"Update available", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
			if (question == DialogResult.Yes)
			{
				GoToReleases();
			}
		}

		static void GoToReleases()
		{
			IOManager.LaunchURL("https://github.com/AHeroicLlama/Mappalachia/releases/latest");
		}
	}
}
