namespace Mappalachia.Class
{
	public static class SettingsFileExport
	{
		public enum FileType
		{
			PNG,
			JPEG,
		}

		public static readonly int jpegQualityMin = 20;
		public static readonly int jpegQualityMax = 100;
		public static readonly int jpegQualityDefault = 86;
		public static readonly bool openExplorerDefault = false;

		public static bool useRecommended = true;

		public static FileType fileType = FileType.JPEG;
		public static int jpegQuality = jpegQualityDefault;
		public static bool openExplorer = openExplorerDefault;

		public static bool IsPNG()
		{
			return fileType == FileType.PNG;
		}

		public static bool IsJPEG()
		{
			return fileType == FileType.JPEG;
		}

		// Set the appropriate settings if recommended settings are chosen
		public static void UpdateRecommendation()
		{
			if (useRecommended)
			{
				fileType = GetFileTypeRecommendation();

				// Don't adjust the jpeg quality unless it's the recommended type
				if (SettingsSpace.CurrentSpaceIsWorld())
				{
					jpegQuality = jpegQualityDefault;
				}
			}
		}

		public static FileType GetFileTypeRecommendation()
		{
			return FileType.JPEG;
		}

		public static void SetUseRecommended(bool newValue)
		{
			useRecommended = newValue;
			UpdateRecommendation();
		}
	}
}
