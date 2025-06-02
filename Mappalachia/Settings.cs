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

		public static Settings LoadFromFile()
		{
			if (File.Exists(Paths.SettingsPath))
			{
				try
				{
					Settings settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(Paths.SettingsPath)) ?? throw new Exception("Settings JSON Deserialized to null");
					settings.MapSettings.RootSettings = settings;
					return settings;
				}
				catch (Exception e)
				{
					File.Delete(Paths.SettingsPath);
					Notify.GenericError("Exception loading settings from file", "Mappalachia was unable to load your last settings from the settings file.\nThe settings have been reset.", e);
					return new Settings();
				}
			}

			return new Settings();
		}

		public void SaveToFile()
		{
			File.WriteAllText(Paths.SettingsPath, JsonSerializer.Serialize(this));
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
