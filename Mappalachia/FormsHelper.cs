namespace Mappalachia
{
	public static class FormsHelper
	{
		// Removes the sorting from the sorted column of a DataGridView
		public static void ClearSort(this DataGridView dataGridView)
		{
			DataGridViewColumn? sortedColumn = dataGridView.SortedColumn;

			if (sortedColumn == null)
			{
				return;
			}

			DataGridViewColumnSortMode originalSortMode = sortedColumn.SortMode;

			sortedColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
			sortedColumn.SortMode = originalSortMode;
		}
	}
}
