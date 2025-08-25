using Library;

namespace Mappalachia
{
	public class SearchSettings
	{
		public bool SearchInAllSpaces { get; set; } = false;

		public bool Advanced { get; set; } = false;

		public string SearchTerm { get; set; } = GetRandomSearchHint();

		bool searchInstancesOnly = false;

		public bool SearchInInstancesOnly
		{
			get
			{
				return searchInstancesOnly;
			}

			set
			{
				searchInstancesOnly = value;

				// Search in instances only implies searching in all spaces
				if (value)
				{
					SearchInAllSpaces = true;
				}
			}
		}

		public List<Signature> SelectedSignatures { get; set; } = Enum.GetValues<Signature>().ToList().Where(s => s.IsRecommendedSelection()).ToList();

		public List<LockLevel> SelectedLockLevels { get; set; } = Enum.GetValues<LockLevel>().ToList();

		static string GetRandomSearchHint()
		{
			List<string> searchTermHints = new List<string>
			{
				"Alcohol",
				"Alien Blaster",
				"Ballistic Fiber",
				"Caps",
				"Flora",
				"Fusion Core",
				"Ghoul",
				"Ginseng",
				"Hardpoint",
				"Instrument",
				"Chem",
				"LPI_Food",
				"LvlCritter",
				"NoCampAllowed",
				"Nuka Cola",
				"Overseer's Cache",
				"P01C_Bucket_Loot",
				"PowerArmorFurniture_",
				"Pre War Money",
				"Protest Sign",
				"Pumpkin",
				"RETrigger",
				"Rare",
				"Raw Flux",
				"Recipe",
				"SFM04_Organic_Pod",
				"Starlight Creeper",
				"Strange Encounter",
				"Tales from West Virginia",
				"Teddy Bear",
				"Treasure Map Mound",
				"Trunk Boss",
				"Vein",
				"Wind Chimes",
				"Workbench",
			};

			return searchTermHints[new Random().Next(searchTermHints.Count)];
		}

		public bool ShouldSearchForNPC()
		{
			return SelectedSignatures.Contains(Signature.NPC_) && SelectedLockLevels.Contains(LockLevel.None);
		}

		public bool ShouldSearchForScrap()
		{
			return SelectedSignatures.Contains(Signature.MISC) && SelectedLockLevels.Contains(LockLevel.None);
		}

		public bool ShouldSearchForRawFlux()
		{
			return SelectedSignatures.Contains(Signature.ALCH) && SelectedLockLevels.Contains(LockLevel.None);
		}

		public bool ShouldSearchForRegion()
		{
			return SelectedSignatures.Contains(Signature.REGN) && SelectedLockLevels.Contains(LockLevel.None);
		}
	}
}
