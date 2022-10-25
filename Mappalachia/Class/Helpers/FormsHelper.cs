using System.Windows.Forms;

namespace Mappalachia
{
	static class FormsHelper
	{
		// Recursively enable/disable a tool strip menu item and all children
		public static void EnableMenuStrip(ToolStripMenuItem menuItem, bool enabled)
		{
			menuItem.Enabled = enabled;

			foreach (ToolStripMenuItem item in menuItem.DropDownItems)
			{
				EnableMenuStrip(item, enabled);
			}
		}

		// Removes the sorting from the sorted column of a DataGridView
		public static void ClearDataGridViewSort(DataGridView gridView)
		{
			DataGridViewColumn sortedColumn = gridView.SortedColumn;

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
