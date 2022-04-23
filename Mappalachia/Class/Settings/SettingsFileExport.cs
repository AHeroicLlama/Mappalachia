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
		public static readonly int jpegQualityDefault = 85;

		public static bool useRecommended { get; private set; } = true;

		public static FileType fileType = FileType.JPEG;
		public static int jpegQuality = jpegQualityDefault;

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
				// Worldspace - prefer JPEG allowing for compression
				if (SettingsSpace.CurrentSpaceIsWorld())
				{
					fileType = FileType.JPEG;
					jpegQuality = jpegQualityDefault;
				}

				// Cell - perfer PNG allowing for transparency
				else
				{
					fileType = FileType.PNG;
				}
			}
		}

		public static void SetUseRecommended(bool newValue)
		{
			useRecommended = newValue;
			UpdateRecommendation();
		}
	}
}
