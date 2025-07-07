using System.Text.Json;
using Library;

namespace Mappalachia
{
	public class Settings
	{
		Space space;

		public Space Space
		{
			get => space;
			set
			{
				space = value;
				MapSettings?.SetSpotlightToMapCenter();
			}
		}

		volatile Version? lastDeclinedUpdateVersion;

		// The UpdateChecker runs async and will read/write this
		public Version? LastDeclinedUpdateVersion
		{
			get => lastDeclinedUpdateVersion;
			set => Interlocked.Exchange(ref lastDeclinedUpdateVersion, value);
		}

		public MapSettings MapSettings { get; set; }

		public PlotSettings PlotSettings { get; set; }

		public SearchSettings SearchSettings { get; set; }

		JsonSerializerOptions JsonSerializerOptions { get; set; } = new JsonSerializerOptions() { WriteIndented = true };

		public static Settings LoadFromFile()
		{
			if (File.Exists(Paths.SettingsPath))
			{
				try
				{
					Settings settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(Paths.SettingsPath)) ?? throw new Exception("Settings JSON Deserialized to null");
					settings.MapSettings.RootSettings = settings;

					// Re-associate the space from the settings file with the same virgin space from the database
					// This ensures JSON-ignored data (those not compared in overrode .Equals()) are not lost
					// Failing this, fall back to the first space
					settings.Space = Database.AllSpaces.Where(s => s.Equals(settings.Space)).FirstOrDefault() ?? Database.AllSpaces.First();

					return settings;
				}
				catch (Exception e)
				{
					File.Delete(Paths.SettingsPath);
					Notify.GenericError("Error loading settings from file", "Mappalachia was unable to load your last settings from the settings file.\nThe settings have been reset.", e);
					return new Settings();
				}
			}

			return new Settings();
		}

		public void SaveToFile()
		{
			File.WriteAllText(Paths.SettingsPath, JsonSerializer.Serialize(this, JsonSerializerOptions));
		}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
		public Settings()
		{
			Space ??= Database.AllSpaces.First();
			PlotSettings ??= new PlotSettings();
			SearchSettings ??= new SearchSettings();
			MapSettings ??= new MapSettings(this);

			ResolveConflictingSettings();
			MapSettings.SetSpotlightToMapCenter();
		}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

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
