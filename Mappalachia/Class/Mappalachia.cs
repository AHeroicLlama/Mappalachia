using System;
using System.Windows.Forms;

namespace Mappalachia
{
	static class Mappalachia
	{
		[STAThread]
		static void Main()
		{
			try
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Console.WriteLine("Dedicated to Molly.");
				Application.Run(new FormMaster());
			}
			catch (Exception e)
			{
				Notify.Error("Mappalachia encountered an unexpected error and must close.\n" +
					IOManager.genericExceptionHelpText +
					e);

				IOManager.Cleanup();

				// Choosing to not save settings, since we just crashed it's likely safer to not keep them.
			}
		}
	}
}
