using System.Drawing.Imaging;
using Library;

namespace Mappalachia
{
	static class Mappalachia
	{
		static FormMain? FormMain { get; set; } = null;

		public static bool GUILaunched => FormMain == null;

		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				// To customize application configuration such as set high DPI settings or default font,
				// see https://aka.ms/applicationconfiguration.
				ApplicationConfiguration.Initialize();

				Directory.SetCurrentDirectory(AppContext.BaseDirectory);
				bool passedRecipesAsArgs = false;

				foreach (string arg in args)
				{
					if (arg.EndsWith(Common.RecipeFileType))
					{
						if (!File.Exists(arg))
						{
							continue;
						}

						passedRecipesAsArgs = true;

						Recipe? recipe = Recipe.LoadFromFile(arg);

						if (recipe == null)
						{
							continue;
						}

						Settings settings = new Settings
						{
							MapSettings = recipe.MapSettings,
							PlotSettings = recipe.PlotSettings,
							Space = recipe.Space,
						};

						Image? image = Map.Draw(recipe.ItemsToPlot.ToList(), settings, null, new CancellationTokenSource().Token);

						if (image == null)
						{
							continue;
						}

						ImageFormat outputFormat = FileIO.GetImageFileTypeRecommendation(settings);
						string outputPath = Path.GetDirectoryName(arg) + "\\" + Path.GetFileNameWithoutExtension(arg) + "." + outputFormat;
						FileIO.Save(image, outputFormat, outputPath);

						Console.WriteLine($"Recipe {arg} drawn as image file {outputPath}.");
					}
					else if (arg.EqualsIgnoreCase("dedication"))
					{
						Console.WriteLine("Dedicated to Molly.");
					}
				}

				// The user had passed recipes as arguments, so we don't launch the GUI
				if (passedRecipesAsArgs)
				{
					return;
				}

				FormMain = new FormMain();
				Application.Run(FormMain);
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.ToString());
				Notify.FatalException(exception);
				Application.Exit();
			}
		}

		// Hack so the 'advanced' setting can be fetched globally where references to Settings are not available
		// This is used where the DataGridView DataProperty properties are set via their name, and therefore cannot have arguments passed to them
		public static bool GetIsAdvanced()
		{
			return FormMain?.Settings.SearchSettings.Advanced ?? throw new NullReferenceException("Main Form was null");
		}
	}
}