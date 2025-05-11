using Library;

namespace Mappalachia
{
	public class SearchSettings
	{
		public bool SearchInAllSpaces { get; set; } = false;

		public string SearchTerm { get; set; } = string.Empty;

		public List<Signature> SelectedSignatures { get; set; } = Enum.GetValues<Signature>().ToList();

		public List<LockLevel> SelectedLockLevels { get; set; } = Enum.GetValues<LockLevel>().ToList();
	}
}
