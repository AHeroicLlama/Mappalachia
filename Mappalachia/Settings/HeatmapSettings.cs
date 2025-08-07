namespace Mappalachia
{
	using System.Text.Json.Serialization;
	using Library;

	public class HeatmapSettings
	{
		public HeatmapSettings(int range, int intensity, bool liveUpdate)
		{
			Range = range;
			Intensity = intensity;
			LiveUpdate = liveUpdate;
		}

		public HeatmapSettings()
		{
		}

		[JsonIgnore]
		public static int MinRange { get; } = 2;

		[JsonIgnore]
		public static int MaxRange { get; } = Common.MapImageResolution / 16;

		[JsonIgnore]
		public static int MinIntensity { get; } = 2;

		[JsonIgnore]
		public static int MaxIntensity { get; } = 128;

		// Range in pixels
		public int Range { get; set; } = 128;

		// The amount of alpha added per 1 'spawnWeight' per item
		// Naturally this caps at 255 on the final image
		public int Intensity { get; set; } = 16;

		public bool LiveUpdate { get; set; } = true;
	}
}
