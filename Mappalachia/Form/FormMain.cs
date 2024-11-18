namespace Mappalachia
{
	public partial class FormMain : Form
	{
		public FormMain()
		{
			InitializeComponent();
			pictureBoxMapDisplay.Image = Map.Draw();
		}
	}
}
