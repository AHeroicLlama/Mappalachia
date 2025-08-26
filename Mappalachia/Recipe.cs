using System.Text.Json;
using KGySoft.ComponentModel;
using Library;

namespace Mappalachia
{
	public class Recipe(Space space, MapSettings mapSettings, PlotSettings plotSettings, SortableBindingList<GroupedSearchResult> itemsToPlot)
	{
		public Space Space { get; private set; } = space;

		public MapSettings MapSettings { get; private set; } = mapSettings;

		public PlotSettings PlotSettings { get; private set; } = plotSettings;

		public SortableBindingList<GroupedSearchResult> ItemsToPlot { get; private set; } = itemsToPlot;

		public static Recipe? LoadFromFile(string filePath)
		{
			try
			{
				return JsonSerializer.Deserialize<Recipe>(File.ReadAllText(filePath));
			}
			catch (Exception e)
			{
				Notify.GenericError("Recipe load failed", "Mappalachia was unable to load the provided recipe file.\nError details below.", e);
				return null;
			}
		}

		public Settings AsSettings()
		{
			Settings settings = new Settings
			{
				Space = Space,
				MapSettings = MapSettings,
				PlotSettings = PlotSettings,
			};

			settings.MapSettings.RootSettings = settings;

			foreach (GroupedSearchResult item in ItemsToPlot)
			{
				item.Space = item.Space.PopulateFromData();
			}

			return settings;
		}

		public void SaveToFile(string filePath)
		{
			FileIO.CreateRecipesFolder();

			try
			{
				File.WriteAllText(filePath, JsonSerializer.Serialize(this, FileIO.JsonSerializerOptions));
			}
			catch (Exception e)
			{
				Notify.GenericError("Recipe save failed", "Mappalachia was unable to save the recipe file.\nError details below.", e);
			}
		}
	}
}
