namespace Mappalachia
{
	public static class SettingsFileExport
	{
		public enum ExtensionType
		{
			PNG,
			JPEG,
		}

		public const int jpegQualityMin = 20;
		public const int jpegQualityMax = 100;
		public const int jpegQualityDefault = 86;
		public const bool openExplorerDefault = false;

		public static bool useRecommended = true;

		public static ExtensionType fileType = ExtensionType.JPEG;
		public static int jpegQuality = jpegQualityDefault;
		public static bool openExplorer = openExplorerDefault;

		public static bool IsPNG()
		{
			return fileType == ExtensionType.PNG;
		}

		public static bool IsJPEG()
		{
			return fileType == ExtensionType.JPEG;
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

		public static ExtensionType GetFileTypeRecommendation()
		{
			return SettingsMap.background == SettingsMap.Background.None ? ExtensionType.PNG : ExtensionType.JPEG;
		}

		public static void SetUseRecommended(bool newValue)
		{
			useRecommended = newValue;
			UpdateRecommendation();
		}
	}
}
