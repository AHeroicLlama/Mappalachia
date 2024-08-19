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

		static Dictionary<string, string> MarkerLabelCorrection { get; } = new Dictionary<string, string>()
		{
			{ "Animal Cave", "Hopewell Cave" },
			{ "Bleeding Kate's Grinder", "Bleeding Kate's Grindhouse" },
			{ "Building Summersville Dam", "Summersville Dam" },
			{ "Burning Mine", "The Burning Mine" },
			{ "Cranberry Bog Region", "Quarry X3" },
			{ "Cranberry Glade", "Sacramental Glade" },
			{ "Crater Outpost", "Crater Watchstation" },
			{ "Emmett Mt. Disposal Site", "Emmett Mountain Disposal Site" },
			{ "Garrahan Excavations Headquarters", "Garrahan Mining Headquarters" },
			{ "Hawke's Refuge", "Dagger's Den" },
			{ "Lumber Camp", "Sylvie & Sons Logging Camp" },
			{ "Maybell Pond", "Beckwith Farm" },
			{ "Middle Mountain Cabins", "Middle Mountain Pitstop" },
			{ "Mine Shaft No. 9", "AMS Testing Site" },
			{ "Mire", "Harpers Ferry Tunnel" },
			{ "Morgantown Regional Airfield", "Morgantown Airport" },
			{ "Mountain Region", "Colonel Kelly Monument" },
			{ "Nuked Crater", "Foundation Outpost" },
			{ "Relay Tower 2", "Relay Tower HN-B1-12" },
			{ "Relay Tower 3", "Relay Tower DP-B5-21" },
			{ "Relay Tower 4", "Relay Tower LW-B1-22" },
			{ "Relay Tower 5", "Relay Tower HG-B7-09" },
			{ "Relay Tower 6", "Relay Tower EM-B1-27" },
			{ "Schram Homestead", "Silva Homestead" },
			{ "Sundew Grove 02", "Veiled Sundew Grove" },
			{ "Sundew Grove 03", "Creekside Sundew Grove" },
			{ "The Burrows", "The Burrows South" },
			{ "The Savage Divide", "Monorail Elevator" },
			{ "World's Largest Teapot", "The Giant Teapot" },
			{ "Lightning Pylon A", "Research Site Saxony" },
			{ "Lightning Pylon B", "Research Site Bavaria" },
			{ "Lightning Pylon C", "Research Site Rhineland" },
			{ "Federal Field NG-17", "Big Meadows Gas Well" },
			{ "Shenandoah Weather Station", "Hawksbill Weather Station" },
			{ "Shining Creek Caverns", "Shining Creek Cavern" },
		};

		static Dictionary<string, string> MarkerIconCorrection { get; } = new Dictionary<string, string>()
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
		public static string RemoveMarkersQuery { get; } = $"DELETE FROM MapMarker WHERE label IN ({string.Join(",", MapMarkersToRemove.Select(m => "\'" + m + "\'"))});";

		// Monongah Workshop (0x003D4B48) does not have its 'Map Marker/FULL - Name' record assigned so the export scripts don't find it
		public static string AddMissingMarkersQuery { get; } = $"INSERT INTO MapMarker (spaceFormID, x, y, label, icon) VALUES(2480661, 44675.304687, 73761.358125, 'Monongah Power Plant Yard', '{WorkshopMarker}');";

		// Hemlock Holes Maintenance is just "Hemlock Holes" in the data, but we can't just correct it like the other misnamed map markers, because there is also a legitimate "Hemlock Holes"
		public static string CorrectDuplicateMarkersQuery { get; } = "UPDATE MapMarker set label = 'Hemlock Holes Maintenance' WHERE label = 'Hemlock Holes' AND icon = 'FactoryMarker';";

		// Returns the corrected label for the given map marker label
		public static string? CorrectLabelsByDict(string label)
		{
			if (MarkerLabelCorrection.TryGetValue(label, out string? correctedLabel))
			{
				return correctedLabel;
			}

			return label;
		}

		// Fix fissure site naming - Rename Zeta to Prime, drop latin names from all others
		public static string CorrectFissureLabels(string label)
		{
			if (label.StartsWith(FissureSiteLabel))
			{
				return (label == $"{FissureSiteLabel} Zeta") ? $"{FissureSiteLabel} Prime" : FissureSiteLabel;
			}

			return label;
		}

		// Correct map marker labels by correcting common extraneous/incorrect text in the label
		public static string CorrectCommonBadLabels(string label)
		{
			return label.Replace("Fast Travel Point: ", string.Empty).Replace("Hornwright Air Cleanser Site", "Hornwright Air Purifier Site");
		}

		// Returns the correct icon for the given map marker label
		public static string? CorrectMarkerIcons(string label)
		{
			if (MarkerIconCorrection.TryGetValue(label, out string? correctedIcon))
			{
				return correctedIcon;
			}

			// Icon does not need correcting
			return null;
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
			"spaceDisplayName = 'Vault 63 Engineering Sector' OR " +
			"spaceDisplayName = 'Diamond City' OR " +
			"spaceDisplayName = 'Goodneighbor'";
	}
}
