using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Mappalachia.Class
{
	public static class UpdateChecker
	{
		static readonly string currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

		public static async void CheckForUpdate(bool userTriggered)
		{
			HttpResponseMessage response;

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

			try
			{
				// Extract the value of 'tag_name' from the json string
				string responseContent = await response.Content.ReadAsStringAsync();
				string tagName = "\"tag_name\":";
				int tagValueStart = responseContent.IndexOf(tagName) + tagName.Length;
				int tagValueLength = responseContent.Substring(tagValueStart).IndexOf(",");
				string latestVersion = responseContent.Substring(tagValueStart, tagValueLength).Replace("\"", string.Empty);

				// Verify the version string is in the format "x.y.y.y" where x is 1-999 and y is 0-999
				Regex verifyVersion = new Regex(@"^[1-9]{1,3}(\.[0-9]{1,3}){3}$");

				// We got a valid version string from GitHub
				if (verifyVersion.IsMatch(latestVersion))
				{
					if (currentVersion != latestVersion)
					{
						PromptForUpdate(latestVersion);
					}
					else if (userTriggered)
					{
						Notify.Info("Mappalachia is up to date.\n" +
							"(Version " + currentVersion + ")");
					}
				}
				else
				{
					throw new ArgumentException("Version number parsed from response was of an incorrect format.");
				}
			}
			catch (Exception)
			{
				if (userTriggered)
				{
					CheckForUpdatesManual("Unable to correctly identify latest release from API response.");
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
				Process.Start(new ProcessStartInfo { FileName = "https://github.com/AHeroicLlama/Mappalachia/releases/latest", UseShellExecute = true });
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
				Process.Start(new ProcessStartInfo { FileName = "https://github.com/AHeroicLlama/Mappalachia/releases/latest", UseShellExecute = true });
			}
		}
	}
}
