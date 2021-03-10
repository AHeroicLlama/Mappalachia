namespace Mappalachia.Class
{
	//Represents an internal cell. Used to select and differentiate cells in Cell mode
	class Cell
	{
		public string formID;
		public string editorID;
		public string displayName;

		public Cell(string formID, string editorID, string displayName)
		{
			this.formID = formID;
			this.editorID = editorID;
			this.displayName = displayName;
		}
	}
}
