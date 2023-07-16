using System.Windows.Forms;

namespace CommonwealthCartography
{
	// Neatly packaged message boxes
	public static class Notify
	{
		public static void Info(string text)
		{
			MessageBox.Show(text, "Commonwealth Cartography - Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public static void Warn(string text)
		{
			MessageBox.Show(text, "Commonwealth Cartography - Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		public static void Error(string text)
		{
			MessageBox.Show(text, "Commonwealth Cartography - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
}
