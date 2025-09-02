namespace Mappalachia
{
	public static class FormsHelper
	{
		// Removes the sorting from the sorted column of a DataGridView
		public static void ClearSort(this DataGridView dataGridView)
		{
			DataGridViewColumn? sortedColumn = dataGridView.SortedColumn;

			if (sortedColumn is null)
			{
				return;
			}

			DataGridViewColumnSortMode originalSortMode = sortedColumn.SortMode;

			sortedColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
			sortedColumn.SortMode = originalSortMode;
		}

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
