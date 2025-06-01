using System.Text.Json;
using Library;

namespace Mappalachia
{
	public class Settings
	{
		Space space;

		public MapSettings MapSettings { get; set; }

		public PlotSettings PlotSettings { get; set; }

		public SearchSettings SearchSettings { get; set; }

		public static Settings LoadFromFile(string path)
		{
			if (File.Exists(path))
			{
				try
				{
					Settings settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(path)) ?? new Settings();
					settings.MapSettings.RootSettings = settings;
					return settings;
				}
				catch (Exception)
				{
					return new Settings();
				}
			}

			return new Settings();
		}

		public void SaveToFile(string path)
		{
			File.WriteAllText(path, JsonSerializer.Serialize(this));
		}

		public Settings()
		{
			space ??= Database.AllSpaces.First();
			MapSettings ??= new MapSettings(this);
			PlotSettings ??= new PlotSettings();
			SearchSettings ??= new SearchSettings();
		}

		public Space Space
		{
			get => space;

			set
			{
				if (Space == value)
				{
					return;
				}

				space = value;
				ResolveConflictingSettings();
			}
		}

		// Check for and amend settings which shouldn't be used together
		public void ResolveConflictingSettings()
		{
			if (MapSettings.BackgroundImage == BackgroundImageType.Military && !Space.IsAppalachia())
			{
				MapSettings.BackgroundImage = BackgroundImageType.Menu;
			}

			if (MapSettings.BackgroundImage == BackgroundImageType.Menu && !Space.IsWorldspace)
			{
				MapSettings.BackgroundImage = BackgroundImageType.Render;
			}
		}
	}
}
