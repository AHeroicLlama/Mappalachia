using System.Text.RegularExpressions;

namespace MappalachiaLibrary
{
	// For multiple reasons, a small subset of data is hardcoded.
	// Naturally hardcoding things comes with risks and may require review after each patch. For that reason all hardcoded items are kept together here.
	// Hardcoding may be done for the following reasons: Datamining is not realistic, or a route is not known. Or the data is apparently server-side.
	// Notably Map Markers are the main offender of this
	public static class Hardcodings
	{
		static string BloodEagleMarker { get; } = "BloodEagleMarker";
		static string CultistMarker { get; } = "CultistMarker";
		static string WorkshopMarker { get; } = "PublicWorkshopMarker";
		static string FissureSiteLabel { get; } = "Fissure Site";

		static Dictionary<string, string> MapMarkerLabelCorrection { get; } = new Dictionary<string, string>()
		{
			{ "Abandoned Bog Town", WorkshopMarker },
			{ "Ammo Dump", BloodEagleMarker },
			{ "Beckley Mine Exhibit", WorkshopMarker },
			{ "Berkeley Springs West", WorkshopMarker },
			{ "Billings Homestead", WorkshopMarker },
			{ "Blakes Offering", CultistMarker },
			{ "Bloody Frank's", BloodEagleMarker },
			{ "Charleston Landfill", WorkshopMarker },
			{ "Clancy Manor", CultistMarker },
			{ "Cliffwatch", BloodEagleMarker },
			{ "Converted Munitions Factory", WorkshopMarker },
			{ "Crimson Prospect", BloodEagleMarker },
			{ "Dabney Homestead", WorkshopMarker },
			{ "Dagger's Den", BloodEagleMarker },
			{ "Dolly Sods Campground", WorkshopMarker },
			{ "Federal Disposal Field HZ-21", WorkshopMarker },
			{ "Forward Station Alpha", "BoSMarker" },
			{ "Forward Station Delta", "BoSMarker" },
			{ "Forward Station Tango", "BoSMarker" },
			{ "Foundation", "HammerWingMarker" },
			{ "Gorge Junkyard", WorkshopMarker },
			{ "Grafton Steel Yard", WorkshopMarker },
			{ "Hemlock Holes Maintenance", WorkshopMarker },
			{ "Hunter's Ridge", BloodEagleMarker },
			{ "Ingram Mansion", CultistMarker },
			{ "Johnson's Acre", CultistMarker },
			{ "Kanawha County Cemetery", CultistMarker },
			{ "Lakeside Cabins", WorkshopMarker },
			{ "Lucky Hole Mine", CultistMarker },
			{ "Monongah Power Plant Yard", WorkshopMarker },
			{ "Moth-Home", CultistMarker },
			{ "Mount Blair", WorkshopMarker },
			{ "Ohio River Adventures", "SkullRingMarker" },
			{ "Organ Cave North", CultistMarker },
			{ "Organ Cave South", CultistMarker },
			{ "Organ Cave West", CultistMarker },
			{ "Poseidon Energy Plant Yard", WorkshopMarker },
			{ "Red Rocket Mega Stop", WorkshopMarker },
			{ "Ripper Alley", BloodEagleMarker },
			{ "Rollins Labor Camp", BloodEagleMarker },
			{ "Sacrament", CultistMarker },
			{ "Seneca Gang Camp", BloodEagleMarker },
			{ "Skullbone Vantage", BloodEagleMarker },
			{ "South Cutthroat Camp", BloodEagleMarker },
			{ "Sunshine Meadows Industrial Farm", WorkshopMarker },
			{ "The Bounty", BloodEagleMarker },
			{ "The Coop", "SkullRingMarker" },
			{ "The Crater", "SkullRingMarker" },
			{ "The Crosshair", BloodEagleMarker },
			{ "The Kill Box", BloodEagleMarker },
			{ "The Pigsty", BloodEagleMarker },
			{ "The Rusty Pick", "LegendaryPurveyorMarker" },
			{ "The Sludge Works", BloodEagleMarker },
			{ "The Vantage", BloodEagleMarker },
			{ "Thunder Mt. Power Plant Yard", WorkshopMarker },
			{ "Twin Pine Cabins", BloodEagleMarker },
			{ "Tyler County Dirt Track", WorkshopMarker },
			{ "Vault 51", "Vault51Marker" },
			{ "Vault 79", "Vault79Marker" },
			{ "Wade Airport", WorkshopMarker },
			{ "Widow's Perch", BloodEagleMarker },
		};

		static List<string> MapMarkersToRemove { get; } = new List<string>()
		{
			"Fissure Site Delta",
			"Fissure Site Theta",
			"Fissure Site Kappa",
			"Fissure Site Tau",
		};

		// Remove these markers which are not present in-game
		public static string RemoveMarkersQuery { get; } = $"DELETE FROM MapMarker WHERE label IN ({string.Join(",", MapMarkersToRemove.Select(m => "\'" + m + "\'"))})";

		// Monongah Workshop (0x003D4B48) does not have its 'Map Marker/FULL - Name' record assigned so the export scripts don't find it
		public static string AddMissingMarkersQuery { get; } = $"INSERT INTO MapMarker (spaceFormID, x, y, label, icon) VALUES(2480661, 44675.304687, 73761.358125, 'Monongah Power Plant Yard', '{WorkshopMarker}')";

		// Hemlock Holes Maintenance is just "Hemlock Holes" in the data, but we can't just correct it like the other misnamed map markers, because there is also a legitimate "Hemlock Holes"
		public static string CorrectDuplicateMarkersQuery { get; } = "UPDATE MapMarker set label = 'Hemlock Holes Maintenance' WHERE label = 'Hemlock Holes' AND icon = 'FactoryMarker'";

		// Fix fissure site naming - Rename Zeta to Prime, drop latin names from all others
		public static string CorrectFissureMarkerNames(string label)
		{
			if (label.StartsWith(FissureSiteLabel))
			{
				return (label == $"{FissureSiteLabel} Zeta") ? $"{FissureSiteLabel} Prime" : FissureSiteLabel;
			}

			return label;
		}

		static Dictionary<string, string> NPCNameCorrection { get; } = new Dictionary<string, string>()
		{
			//{ "CaveCricket", "Cave Cricket" },
			//{ "FogCrawler", "Fog Crawler" },
			//{ "GraftonMonster", "Grafton Monster" },
			//{ "HoneyBeast", "Honey Beast" },
			{ "Megasloth", "Mega Sloth" },
			//{ "MoleMiner", "Mole Miner" },
			{ "Molerat", "Mole Rat" },
			{ "Mutations", "Snallygaster" },
			//{ "RadAnt", "Rad Ant" },
			{ "RadFrog", "Frog" },
			{ "RadStag", "Radstag" },
			{ "RadTurkey", "Thrasher" },
			{ "Rat", "Rad Rat" },
			{ "Scorpions", "Rad Scorpion" },
			//{ "SuperMutant", "Super Mutant" },
			{ "Swamp", "Gulper" },
			{ "Toad", "Rad Toad" },
			//{ "ViciousDog", "Vicious Dog" },
			//{ "YaoGuai", "Yao Guai" },
		};

		static Regex NPCNameFilterRegex { get; } = new Regex("(ESSChance(Sub|Main|Critter(A|B)))?(.*)s?(LARGE|GIANTONLY)?");
		static Regex NPCNameSpaceRegex { get; } = new Regex("([a-z])([A-Z])");

		// Attempts to extract the "proper" NPC name from the ESSChance property of a LCTN
		static string CaptureNPCName(string data)
		{
			// Filter out the excess wording
			data = NPCNameFilterRegex.Match(data).Groups[2].Value;

			// Refer to the replacement dictionary
			if (NPCNameCorrection.TryGetValue(data, out string? correction))
			{
				data = correction;
			}

			// Add spaces between capital chars which directly follow lower-case
			data = NPCNameSpaceRegex.Replace(data, "$0 $1");

			return data;
		}

		// Provides the WHERE clause for a query which defines the rules of which cells we should discard, as they appear to be inaccessible.
		public static string DiscardCellsQuery { get; } =
			"spaceDisplayName = '' OR " +
			"spaceDisplayName LIKE '%Test%World%' OR " +
			"spaceDisplayName LIKE '%Test%Cell%' OR " +
			"spaceEditorID LIKE 'zCUT%' OR " +
			"spaceEditorID LIKE '%OLD' OR " +
			"spaceEditorID LIKE 'Warehouse%' OR " +
			"spaceEditorID LIKE 'Test%' OR " +
			"spaceEditorID LIKE '%Debug%' OR " +
			"spaceEditorID LIKE 'zz%' OR " +
			"spaceEditorID LIKE '76%' OR " +
			"spaceEditorID LIKE '%Worldspace' OR " +
			"spaceEditorID LIKE '%Nav%Test%' OR " +
			"spaceEditorID LIKE 'PackIn%' OR " +
			"spaceEditorID LIKE 'COPY%' OR " +
			"spaceDisplayName = 'Purgatory' OR " +
			"spaceDisplayName = 'Diamond City' OR " +
			"spaceDisplayName = 'Goodneighbor'";
	}
}
