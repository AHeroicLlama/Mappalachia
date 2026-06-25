namespace Library
{
	// A Cell of a WorldSpace, NOT an interior cell. See Space for this.
	public class Cell(Location parentLocation, int x, int y)
		: Instance(parentLocation, parentLocation.Space, new Coord(x * Common.CellSize, (y * Common.CellSize) + Common.CellSize), 0, string.Empty, null, LockLevel.None, null)
	{
		public int X { get; } = x;

		public int Y { get; } = y;

		public CellNeighbors GetNeighbors()
		{
			CellNeighbors neighbors = new CellNeighbors();

			foreach (Cell cell in parentLocation.Cells)
			{
				if (cell.X == X && cell.Y == Y + 1)
				{
					neighbors.Up = true;
				}
				else if (cell.X == X && cell.Y == Y - 1)
				{
					neighbors.Down = true;
				}
				else if (cell.X == X - 1 && cell.Y == Y)
				{
					neighbors.Left = true;
				}
				else if (cell.X == X + 1 && cell.Y == Y)
				{
					neighbors.Right = true;
				}
			}

			return neighbors;
		}
	}
}
