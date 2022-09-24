namespace ImageAssetChecker
{
	internal class Space
	{
		string editorId;
		bool isWorldspace;

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
