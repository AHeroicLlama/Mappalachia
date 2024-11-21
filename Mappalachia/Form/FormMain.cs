using System.Diagnostics;
using Library;

namespace Mappalachia
{
	public partial class FormMain : Form
	{
		public FormMain()
		{
			InitializeComponent();
			pictureBoxMapDisplay.Image = Map.Draw();

			Entity entity = new Entity(5408329, "LPI_Loot_CapsStash_Tin", "Caps Stash", Signature.LVLI); // dummy
			Stopwatch stopwatch = Stopwatch.StartNew();
			List<Instance> instances = Database.GetInstances(entity, Settings.CurrentSpace);
			MessageBox.Show($"In {stopwatch.Elapsed} found {instances.Count} instances of {entity.DisplayName} in Space {Settings.CurrentSpace.DisplayName}.");
		}
	}
}
