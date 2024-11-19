using Library;
using Microsoft.Data.Sqlite;
using static Library.CommonDatabase;

namespace Mappalachia
{
	static class Database
	{
		public static SqliteConnection Connection { get; } = GetNewConnection(Paths.DatabasePath);

		public static List<Space> Spaces { get; } = GetSpaces(Connection);

		public static List<MapMarker> MapMarkers { get; } = GetMapMarkers(Connection);
	}
}
