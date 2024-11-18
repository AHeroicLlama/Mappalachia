using Library;

namespace Mappalachia
{
	static class Settings
	{
		public static Space CurrentSpace { get; set; } = CommonDatabase.GetSpaces(Database.Connection).First();
	}
}
