using Library;

namespace Mappalachia
{
	public class Settings()
	{
		Space space = Database.AllSpaces.First();
		BackgroundImageType backgroundImage = BackgroundImageType.Menu;

		public Space Space
		{
			get => space;

			set
			{
				if (space == value)
				{
					return;
				}

				space = value;
				ResolveConflictingSettings();
			}
		}

		public BackgroundImageType BackgroundImage
		{
			get => backgroundImage;

			set
			{
				if (backgroundImage == value)
				{
					return;
				}

				backgroundImage = value;
				ResolveConflictingSettings();
			}
		}

		public float Brightness { get; set; } = 1.0f;

		public bool GrayscaleBackground { get; set; } = false;

		public bool HighlightWater { get; set; } = false;

		public bool MapMarkerIcons { get; set; } = false;

		public bool MapMarkerLabels { get; set; } = false;

		public LegendStyle LegendStyle { get; set; } = LegendStyle.Normal;

		public bool SearchInAllSpaces { get; set; } = true;

		public string SearchTerm { get; set; } = string.Empty;

		public List<Signature> SelectedSignatures { get; set; } = Enum.GetValues<Signature>().ToList();

		public List<LockLevel> SelectedLockLevels { get; set; } = Enum.GetValues<LockLevel>().ToList();

		// Check for and amend settings which shouldn't be used together
		void ResolveConflictingSettings()
		{
			if (BackgroundImage == BackgroundImageType.Military && !Space.IsAppalachia())
			{
				BackgroundImage = BackgroundImageType.Menu;
			}

			if (BackgroundImage == BackgroundImageType.Menu && !Space.IsWorldspace)
			{
				BackgroundImage = BackgroundImageType.Render;
			}
		}
	}
}
