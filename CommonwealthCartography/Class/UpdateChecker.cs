﻿using System;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Windows.Forms;

namespace CommonwealthCartography
{
	public static class UpdateChecker
	{
		static readonly Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

		public static async void CheckForUpdate(bool userTriggered)
		{
			// If this is an auto-check and the cooldown window from a previous decline has not expired yet
			if (!userTriggered && DateTime.Now - SettingsUpdate.lastDeclinedUpdate < SettingsUpdate.GetCooldownPeriod())
			{
				return;
			}

			HttpResponseMessage response;

			// Get the GitHub API response
			try
			{
				// Get the latest release info from GitHub API
				HttpClient httpClient = new HttpClient();
				httpClient.DefaultRequestHeaders.Add("User-Agent", "Mappalachia/Commonwealth_Cartography");
				response = await httpClient.GetAsync(new Uri("https://api.github.com/repos/Mappalachia/Commonwealth_Cartography/releases/latest"));

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
				bool isDraft = responseDocument.RootElement.GetProperty("draft").GetBoolean();
				bool isPreRelease = responseDocument.RootElement.GetProperty("prerelease").GetBoolean();
				Version latestVersion = new Version(responseDocument.RootElement.GetProperty("tag_name").ToString());

				if (latestVersion > currentVersion)
				{
					if (isDraft || isPreRelease)
					{
						// If it's a draft/pre - warn manual checkers, abort auto-checkers
						if (userTriggered)
						{
							Notify.Warn("The latest release is marked as a pre-release or draft release.\nAlthough available now, it is not recommended to download it yet.");
						}
						else
						{
							return;
						}
					}

					PromptForUpdate(latestVersion.ToString(), userTriggered);
				}
				else if (currentVersion > latestVersion)
				{
					if (userTriggered)
					{
						Notify.Info("This build is ahead of the latest release.");
					}
				}
				else if (userTriggered)
				{
					Notify.Info("Commonwealth Cartography is up to date.\n" +
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
		// Also displays provided reason why automatic check failed and the current Commonwealth Cartography version.
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
		static void PromptForUpdate(string latestVersion, bool userTriggered)
		{
			DialogResult question = MessageBox.Show(
				"A new version of Commonwealth Cartography, " + latestVersion + " is now available! \nVisit the releases page to download it?",
				"Update available", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
			if (question == DialogResult.Yes)
			{
				GoToReleases();
			}

			// If they decline the auto-update prompt
			else if (question == DialogResult.No && !userTriggered)
			{
				SettingsUpdate.lastDeclinedUpdate = DateTime.Now;
			}
		}

		static void GoToReleases()
		{
			IOManager.LaunchURL("https://github.com/Mappalachia/Commonwealth_Cartography/releases/latest");
		}
	}
}
