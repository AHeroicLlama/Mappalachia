using Library;

namespace Mappalachia
{
	static class Settings
	{
		public static Space CurrentSpace { get; set; } = Database.CachedSpaces.First();
	}
}
