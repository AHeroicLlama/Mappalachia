using Library;

namespace Mappalachia
{
	// Static local paths relative to the executable
	public static class Paths
	{
		static string ImgPath { get; } = @"img\";

		public static string DatabasePath { get; } = @"data\mappalachia.db";

		public static string SpotlightTilePath { get; } = @$"{ImgPath}spotlight\";

		public static string CellImgPath { get; } = @$"{ImgPath}cell\";

		public static string WorldspaceImgPath { get; } = @$"{ImgPath}wrld\";

		public static string MapMarkersPath { get; } = @$"{ImgPath}mapmarker\";

		public static string DoorMarkerPath { get; } = @$"{ImgPath}DoorMarker{Common.MapMarkerImageFileType}";

		public static string FontPath { get; } = @"font\futura_condensed_bold.otf";

		public static string SettingsPath { get; } = "settings.json";

		public static string SavedMapsPath { get; } = @"Saved_Maps\";

		public static string TempPath { get; } = @"temp\";
	}
}
