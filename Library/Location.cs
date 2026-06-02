namespace Library
{
	public class Location(uint formID, string editorID, string displayName, Space space)
		: Entity(formID, editorID, displayName, Signature.LCTN)
	{
		public Space Space { get; } = space;

		public List<Cell> Cells { get; } = new List<Cell>();

		public void AddCell(Cell cell)
		{
			Cells.Add(cell);
		}
	}
}
