using Microsoft.Win32;

namespace Mappalachia
{
	// URLs the application reaches out to or opens in the browser
	public static class URLs
	{
		static string DiscordInviteCode { get; } = "Z2GMpm6rad";

		static string DiscordProtocolName { get; } = "discord";

		// Query the registry to find if the URL protocol is registered
		static bool IsProtocolRegistered(string protocol)
		{
			return Registry.ClassesRoot.OpenSubKey(protocol)?.GetValue("URL Protocol") is not null;
		}

		public static Uri DiscordInvite
		{
			get
			{
				return IsProtocolRegistered(DiscordProtocolName) ? new Uri($"{DiscordProtocolName}://-/invite/{DiscordInviteCode}") : new Uri($"https://discord.gg/invite/{DiscordInviteCode}");
			}
		}

		public static Uri LatestReleaseAPI { get; } = new Uri("https://api.github.com/repos/AHeroicLlama/Mappalachia/releases/latest");

		public static Uri GitHub { get; } = new Uri("https://github.com/AHeroicLlama/Mappalachia");

		public static Uri Releases { get; } = new Uri("https://github.com/AHeroicLlama/Mappalachia/releases");

		public static Uri HelpDocs { get; } = new Uri("https://github.com/AHeroicLlama/Mappalachia#getting-started---user-guides");

		public static Uri SpotlightInstallationGuide { get; } = new Uri("https://github.com/AHeroicLlama/Mappalachia/blob/master/Docs/User/Installation.md#installing-spotlight");

		public static Uri DonatePaypal { get; } = new Uri("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=TDVKFJ97TFFVC&source=url");
	}
}
