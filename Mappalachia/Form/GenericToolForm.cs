namespace Mappalachia
{
	// This should be abstract, but the designer requires a concrete class
	public class GenericToolForm : Form
	{
		protected GenericToolForm()
		{
			StartPosition = FormStartPosition.CenterParent;
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			ShowInTaskbar = false;
			ShowIcon = false;
		}
	}
}
