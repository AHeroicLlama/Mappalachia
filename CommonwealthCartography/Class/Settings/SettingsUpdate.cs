﻿using System;

namespace CommonwealthCartography
{
	// Settings related to the Update Checker
	class SettingsUpdate
	{
		static readonly TimeSpan updatePromptCooldownPeriod = TimeSpan.FromHours(48);
		public static DateTime lastDeclinedUpdate = DateTime.UnixEpoch;

		public static TimeSpan GetCooldownPeriod()
		{
			return updatePromptCooldownPeriod;
		}
	}
}
