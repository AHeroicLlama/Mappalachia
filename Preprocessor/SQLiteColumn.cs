namespace Preprocessor
{
	internal partial class SQLiteColumn
	{
		public enum SQLiteType
		{
			TEXT,
			INTEGER,
			REAL,
		}

		public string Name { get; init; }
		public SQLiteType Type { get; init; }

		public SQLiteColumn(string name, SQLiteType type)
		{
			Name = name;
			Type = type;
		}
	}
}
