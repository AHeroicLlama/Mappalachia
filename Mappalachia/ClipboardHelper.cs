namespace Mappalachia
{
	public static class ClipboardHelper
	{
		// Safely sets the clipboard text. Spawns an STA thread if necessary.
		public static void SetText(string text)
		{
			try
			{
				if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
				{
					Clipboard.SetText(text);
				}
				else
				{
					Thread thread = new Thread(() => Clipboard.SetText(text));
					thread.SetApartmentState(ApartmentState.STA);
					thread.Start();
				}
			}
			catch (Exception ex)
			{
				Notify.GenericError("Clipboard Error", "Failed to copy text to clipboard.", ex);
			}
		}
	}
}
