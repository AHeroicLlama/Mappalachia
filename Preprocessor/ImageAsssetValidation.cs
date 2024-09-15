using MappalachiaLibrary;
using Microsoft.Data.Sqlite;
using System.Drawing;

namespace Preprocessor
{
	internal partial class Preprocessor
	{
		const string BackgroundImageFileType = ".jpg";
		const string MaskImageFileType = ".png";
		const string MapMarkerImageFileType = ".svg";

		static void ValidateImageAssets()
		{
			Console.WriteLine("Validating image assets");

			ValidationFailures.Clear();
			SqliteCommand query = Connection.CreateCommand();
			SqliteDataReader reader;

			// Collect all spaces
			List<Space> spaces = new List<Space>();
			query.CommandText = "SELECT spaceEditorID, isWorldspace FROM Space";
			reader = query.ExecuteReader();
			while (reader.Read())
			{
				spaces.Add(new Space(reader.GetString(0), reader.GetInt32(1) == 1));
			}

			// Collect all map markers
			List<MapMarker> mapMarkers = new List<MapMarker>();
			query.CommandText = "SELECT DISTINCT icon FROM MapMarker";
			reader = query.ExecuteReader();
			while (reader.Read())
			{
				mapMarkers.Add(new MapMarker(reader.GetString(0), string.Empty));
			}

			// Validate the image files for the database objects exist, and have expected parameters
			spaces.ForEach(ValidateSpaceImage);
			mapMarkers.ForEach(ValidateMapMarkerImage);

			// Inverse of above, check there are no extraneous files

		}

		static void ValidateSpaceImage(Space space)
		{
			string filePath = (space.IsWorldspace ? BuildPaths.GetImageWorldPath() : BuildPaths.GetImageCellPath()) + space.EditorID + BackgroundImageFileType;

			using (Image image = Image.FromFile(filePath))
			{

			}
		}

		static void ValidateMapMarkerImage(MapMarker mapMarker)
		{
			string filePath = BuildPaths.GetImageMapMarkerPath() + mapMarker.Icon + MapMarkerImageFileType;

		}
	}
}
