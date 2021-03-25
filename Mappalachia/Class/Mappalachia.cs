using System;
using System.Windows.Forms;
using Mappalachia.Class;

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
				Application.Run(new FormMaster());
			}
			catch (Exception e)
			{
				Notify.Error("Mappalachia encountered an unexpected error and must close.\n" +
					IOManager.genericExceptionHelpText +
					e);

				IOManager.Cleanup();

				//Choosing to not save settings, since we just crashed it's likely safer to not keep them.
			}
		}
	}
}
