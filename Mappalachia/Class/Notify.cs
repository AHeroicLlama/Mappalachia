using System.Windows.Forms;

namespace Mappalachia
{
	// Neatly packaged message boxes
	public static class Notify
	{
		public static void Info(string text)
		{
			MessageBox.Show(text, "Mappalachia - Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public static void Warn(string text)
		{
			MessageBox.Show(text, "Mappalachia - Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		public static void Error(string text)
		{
			MessageBox.Show(text, "Mappalachia - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
}
