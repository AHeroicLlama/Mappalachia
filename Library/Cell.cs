namespace Library
{
	// A Cell of a WorldSpace, NOT an interior cell. See Space for this.
	public class Cell(Location parentLocation, int x, int y)
		: Instance(parentLocation, parentLocation.Space, new Coord((x * Common.CellSize) + (Common.CellSize / 2), (y * Common.CellSize) + (Common.CellSize / 2)), 0, string.Empty, null, LockLevel.None, null)
	{
	}
}
