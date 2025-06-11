namespace Preprocessor
{
	class LeveledList(uint parentFormID, uint childFormID, uint count)
	{
		public uint ParentFormID { get; set; } = parentFormID;

		public uint ChildFormID { get; set; } = childFormID;

		public uint Count { get; set; } = count;
	}
}
