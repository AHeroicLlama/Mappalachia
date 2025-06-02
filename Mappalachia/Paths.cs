namespace Mappalachia
{
	// Local paths relative to the executable
	public static class Paths
	{
		public static string DatabasePath { get; } = @"data\mappalachia.db";

		public static string CellImgPath { get; } = @"img\cell\";

		public static string WorldspaceImgPath { get; } = @"img\wrld\";

		public static string MapMarkersPath { get; } = @"img\mapmarker\";

		public static string FontPath { get; } = @"font\futura_condensed_bold.otf";

		public static string SettingsPath { get; } = "settings.json";

		public static string SavedMapsPath { get; } = @"Saved_Maps\";
	}
}
