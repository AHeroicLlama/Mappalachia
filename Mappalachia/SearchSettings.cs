using Library;

namespace Mappalachia
{
	public class SearchSettings
	{
		public bool SearchInAllSpaces { get; set; } = false;

		public bool Advanced { get; set; } = false;

		public string SearchTerm { get; set; } = GetRandomSearchHint();

		public List<Signature> SelectedSignatures { get; set; } = Enum.GetValues<Signature>().ToList();

		public List<LockLevel> SelectedLockLevels { get; set; } = Enum.GetValues<LockLevel>().ToList();

		static string GetRandomSearchHint()
		{
			List<string> searchTermHints = new List<string>
			{
				"Alcohol",
				"Alien Blaster",
				"Caps",
				"Flora",
				"Fusion Core",
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
				"Recipe",
				"SFM04_Organic_Pod",
				"Strange Encounter",
				"Tales from West Virginia",
				"Teddy Bear",
				"Thistle",
				"Treasure Map Mound",
				"Trunk Boss",
				"Vein",
				"Wind Chimes",
				"Workbench",
			};

			return searchTermHints[new Random().Next(searchTermHints.Count)];
		}
	}
}
