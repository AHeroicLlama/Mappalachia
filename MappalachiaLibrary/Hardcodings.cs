using System.Text.RegularExpressions;

namespace MappalachiaLibrary
{
	// For multiple reasons, a small subset of data is hardcoded.
	// Naturally hardcoding things comes with risks and may require review after each patch. For that reason all hardcoded items are kept together here.
	// Hardcoding may be done for the following reasons: Datamining is not realistic, or a route is not known. Or the data is apparently server-side.
	// Notably Map Markers are the main offender of this
	public static class Hardcodings
	{
		const string BloodEagleMarker = "BloodEagleMarker";
		const string CultistMarker = "CultistMarker";
		const string WorkshopMarker = "PublicWorkshopMarker";

		static readonly Dictionary<string, string> MapMarkerLabelCorrection = new Dictionary<string, string>()
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

		static readonly List<string> MapMarkersToRemove = new List<string>()
		{
			"Fissure Site Delta",
			"Fissure Site Theta",
			"Fissure Site Kappa",
			"Fissure Site Tau",
		};

		// Monongah workshop (003D4B48) does not have its 'Map Marker/FULL - Name' record assigned so the export scripts don't find it
		public const string AddMissingMarkersQuery = "INSERT INTO MapMarker (spaceFormID, x, y, label, icon) VALUES(2480661, 44675.304687, 73761.358125, 'Monongah Power Plant Yard', 'PublicWorkshopMarker')";

		static readonly Dictionary<string, string> NPCNameCorrection = new Dictionary<string, string>()
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

		static readonly Regex NPCNameFilterRegex = new Regex("(ESSChance(Sub|Main|Critter(A|B)))?(.*)s?(LARGE|GIANTONLY)?");
		static readonly Regex NPCNameSpaceRegex = new Regex("([a-z])([A-Z])");

		// Attempts to extract the "proper" NPC name from the ESSChance property of a LCTN
		static string CaptureNPCName(string data)
		{
			data = NPCNameFilterRegex.Match(data).Groups[2].Value; // Filter out the excess wording

			if (NPCNameCorrection.ContainsKey(data))
			{
				data = NPCNameCorrection[data];
			}

			data = NPCNameSpaceRegex.Replace(data, "$0 $1"); // Add spaces between capital chars which directly follow lower-case

			return data;
		}

		// Provides the WHERE clause for a query which defines the rules of which cells we should discard, as they appear to be inaccessible.
		public const string DiscardCellsQuery =
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
