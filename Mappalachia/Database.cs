using Library;
using Microsoft.Data.Sqlite;

namespace Mappalachia
{
	static class Database
	{
		public static SqliteConnection Connection { get; } = CommonDatabase.GetNewConnection(Paths.DatabasePath);
	}
}
