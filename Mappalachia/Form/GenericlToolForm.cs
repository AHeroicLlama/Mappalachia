namespace Mappalachia
{
	// This should be abstract, but the designer requires a concrete class
	public class GenericlToolForm : Form
	{
		protected GenericlToolForm()
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
