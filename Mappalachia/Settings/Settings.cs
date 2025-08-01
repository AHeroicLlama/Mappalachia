using System.Text.Json;
using System.Text.Json.Serialization;
using Library;

namespace Mappalachia
{
	// Parent class for all settings
	public class Settings
	{
		[JsonIgnore]
		public bool DoNotSave { get; set; } = false;

		Space? space;

		public Space Space
		{
			get => space ?? throw new Exception("Settings Space is null");
			set
			{
				space = value;

				// This is a workaround to detect a Space which has been initialized from the settings JSON and not the database
				if (space.MaxRange == 0)
				{
					// This re-calls this setter
					PopulateSpaceFromDatabase();
				}

				if (MapSettings is not null)
				{
					MapSettings.SpotlightEnabled = false;
				}

				MapSettings?.SetSpotlightToMapCenter();
				MapSettings?.CapSpotlightSizeToSpace();
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
					Notify.GenericError("Error loading settings from file", "Mappalachia was unable to load your last settings from the settings file.\nThe settings have been reset.", e);
					return new Settings();
				}
			}

			return new Settings();
		}

		// Re-associate a deserialized space with the same virgin space from the database
		// This ensures JSON-ignored data are restored
		// Failing this, fall back to the first space
		void PopulateSpaceFromDatabase()
		{
			Space = Database.AllSpaces.Where(s => s.Equals(Space)).FirstOrDefault() ?? Database.AllSpaces.First();
		}

		public void SaveToFile()
		{
			if (DoNotSave)
			{
				return;
			}

			File.WriteAllText(Paths.SettingsPath, JsonSerializer.Serialize(this, FileIO.JsonSerializerOptions));
		}

		public Settings()
		{
			space ??= Database.AllSpaces.First(); // Bypasses the getter
			PlotSettings ??= new PlotSettings();
			SearchSettings ??= new SearchSettings();
			MapSettings ??= new MapSettings(this);

			ResolveConflictingSettings();
			MapSettings.SetSpotlightToMapCenter();
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

			if (!Space.IsSuitableForSpotlight())
			{
				MapSettings.SpotlightEnabled = false;
			}
		}
	}
}
