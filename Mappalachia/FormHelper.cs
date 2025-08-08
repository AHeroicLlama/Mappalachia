namespace Mappalachia
{
	public static class FormHelper
	{
		public static void UpdateProgress(IProgress<ProgressInfo>? progressInfo, int percent, string status)
		{
			if (progressInfo is null)
			{
				return;
			}

			progressInfo.Report(new ProgressInfo(percent, status));
		}

		// Overload to automatically calculate percentage from x out of y
		public static void UpdateProgress(IProgress<ProgressInfo>? progressInfo, int current, int total, string status)
		{
			if (progressInfo is null)
			{
				return;
			}

			if (current == 0)
			{
				progressInfo.Report(new ProgressInfo(0, status));
				return;
			}

			progressInfo.Report(new ProgressInfo((int)Math.Round((current / (double)total) * 100), status));
		}
	}
}
