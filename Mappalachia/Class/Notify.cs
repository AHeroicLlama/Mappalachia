﻿using System.Threading;
using System.Windows.Forms;

namespace Mappalachia.Class
{
	//Neatly packaged message boxes
	/*This means messageBoxes indicating errors etc don't have to be handled by classes not responsible for UI
	and allows ones called during work to not block said work if required*/
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

	//Run a Notify in a new non-blocking thread. Used to not hold up map generation
	public static class NonBlockingNotify
	{
		public static void Info(string text)
		{
			new Thread(() => Notify.Info(text)).Start();
		}

		public static void Warn(string text)
		{
			new Thread(() => Notify.Warn(text)).Start();
		}

		public static void Error(string text)
		{
			new Thread(() => Notify.Error(text)).Start();
		}
	}
}