namespace Preprocessor
{
	internal class SQLiteTable
	{
		public string Name { get; init; }
		public List<SQLiteColumn> Fields { get; init; }

		public SQLiteTable(string name, List<SQLiteColumn> fields)
		{
			Name = name;
			Fields = fields;
		}
	}
}
