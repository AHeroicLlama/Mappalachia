namespace ImageAssetChecker
{
	internal class Space
	{
		readonly string editorId;
		readonly bool isWorldspace;

		public Space(string editorId, bool isWorldspace)
		{
			this.editorId = editorId;
			this.isWorldspace = isWorldspace;
		}

		public string GetEditorId()
		{
			return editorId;
		}

		public bool IsWorldspace()
		{
			return isWorldspace;
		}
	}
}
