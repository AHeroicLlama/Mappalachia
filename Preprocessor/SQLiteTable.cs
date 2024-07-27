namespace Preprocessor
{
	internal class SQLiteTable
	{
		public string Name { get; init; }
		public Dictionary<string, string> Fields { get; init; } // <Value, Type>

		public SQLiteTable(string name, Dictionary<string, string> fields)
		{
			Name = name;
			Fields = fields;
		}
	}
}
