using System.Text.RegularExpressions;

namespace Library
{
	// For multiple reasons, a small subset of data (or expected data) is hardcoded.
	// Naturally hardcoding things comes with risks and may require review after each patch. For that reason all hardcoded items are kept together here.
	// Hardcoding may be done for the following reasons: Datamining is not realistic, or a route is not known. Or the data is apparently server-side.
	// Notably Map Markers are the main offender of this
	public static partial class BuildTools
	{
		public static string FissureSiteLabel { get; } = "Fissure Site";

		public static Regex SpaceFormIDRegex { get; } = new Regex(@"\[(WRLD|CELL):([0-9A-F]{8})\]");

		public static Regex SignatureFormIDRegex { get; } = new Regex(@"\[[A-Z_]{4}:([0-9A-F]{8})\]");

		public static Regex FormIDRegex { get; } = new Regex(@"[0-9A-F]{8}");

		public static Regex RemoveTrailingReferenceRegex { get; } = new Regex("(.*) " + SignatureFormIDRegex);

		public static Regex QuotedTermRegex { get; } = new Regex(".* :QUOT:(.*):QUOT: " + SignatureFormIDRegex);

		public static Regex TitleCaseAddSpaceRegex { get; } = new Regex("(.*[a-z])([A-Z].*)");

		public static Regex NPCRegex { get; } = new Regex("ESSChance(Main|Sub|Critter[AB])(.*?)s?(LARGE|GIANTONLY)? " + SignatureFormIDRegex);

		public static Regex CorrectLockLevelRegex { get; } = new Regex(@"(Novice|Advanced|Expert|Master) \((Level [0-3])\)");

		public static Regex ValidateLockLevel { get; } = new Regex("^(Level [0-3]|Chained|Inaccessible|Requires Key|Requires Terminal|Unknown|Barred)$");

		public static Regex ValidatePrimitiveShape { get; } = new Regex("^(Box|Line|Plane|Sphere|Ellipsoid)$");

		public static Regex ValidateSignature { get; } = new Regex("^[A-Z_]{4}$");

		public static Regex ValidateMapMarkerIcon { get; } = new Regex("^(WhitespringResort|NukaColaQuantumPlant|TrainTrackMark|.*Marker)$");

		public static Regex ValidateNpcClass { get; } = new Regex("^(Main|Sub|Critter[AB])$");

		public static Regex ValidateComponent { get; } = new Regex("^(Acid|Adhesive|Aluminum|Antiseptic|Asbestos|Ballistic Fiber|Black Titanium|Bone|Ceramic|Circuitry|Cloth|Concrete|Copper|Cork|Crystal|Fertilizer|Fiber Optics|Fiberglass|Gear|Glass|Gold|Gunpowder|Lead|Leather|Nuclear Material|Oil|Plastic|Rubber|Screw|Silver|Spring|Steel|Ultracite|Wood)$");

		public static Regex ValidIconFolder { get; } = new Regex("DefineSprite_[0-9]{1,3}_(([A-Z].*Marker)|WhitespringResort|NukaColaQuantumPlant|TrainTrackMark)$");

		public static Dictionary<string, string> MarkerLabelCorrection { get; } = new Dictionary<string, string>()
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

		public static List<string> MapMarkersToRemove { get; } = new List<string>()
		{
			"Fissure Site Delta",
			"Fissure Site Theta",
			"Fissure Site Kappa",
			"Fissure Site Tau",
		};

		// Spaces which appear to be copy/pastes or alternates of the same thing, and therefore can and should share scaling/adjustments
		public static List<List<string>> SisterSpaces { get; } = new List<List<string>>()
		{
			new List<string>() { "ArktosPharmaLab01", "ArktosPharmaLabDungeon" },
			new List<string>() { "CharGen01", "CharGen02", "CharGen03", "CharGen04", "CharGen05", "NPEVault7601", "NVEVault7601" },
			new List<string>() { "GlassedCavern01", "GlassedCavernDungeon" },
			new List<string>() { "SugarGroveMissileSilo01", "MonongahMissileSilo01", "SpruceKnobMissileSilo01" },
			new List<string>() { "WatogaCivicCenter01", "WatogaCivicCenterDungeon" },
			new List<string>() { "WatogaHighSchool01", "WatogaHighSchoolDungeon" },
			new List<string>() { "XPDAC02Aquarium", "XPDAC02AquariumDungeon" },
			new List<string>() { "XPDAC03CommunityCenter", "XPDAC03CommunityCenterDungeon" },
			new List<string>() { "XPDPitt01Foundry", "XPDPitt01FoundryDungeon" },
		};

		// Monongah Workshop (0x003D4B48) does not have its 'Map Marker/FULL - Name' record assigned so the export scripts don't find it
		public static string AddMissingMarkersQuery { get; } = "INSERT INTO MapMarker (spaceFormID, x, y, label, icon) VALUES(2480661, 44675.304687, 73761.358125, 'Monongah Power Plant Yard', 'PublicWorkshopMarker');";

		// Hemlock Holes Maintenance is just "Hemlock Holes" in the data, but we can't just correct it like the other misnamed map markers, because there is also a legitimate "Hemlock Holes"
		public static string CorrectDuplicateMarkersQuery { get; } = "UPDATE MapMarker set label = 'Hemlock Holes Maintenance' WHERE label = 'Hemlock Holes' AND icon = 'FactoryMarker';";

		// For an unknown reason, some enitities in xEdit have this invalid lock level
		public static string CorrectLockLevelQuery { get; } = "UPDATE Position SET lockLevel = 'Novice (Level 0)' WHERE lockLevel = 'Opens Door';";

		// For an unknown reason, some enitities in xEdit have this invalid primitive shape
		public static string CorrectPrimitiveShapeQuery { get; } = "UPDATE Position SET primitiveShape = 'Box' WHERE primitiveShape = '7';";

		// Assumes PascalCased names will already have had spaces added
		public static Dictionary<string, string> NPCNameCorrection { get; } = new Dictionary<string, string>()
		{
			{ "Megasloth", "Mega Sloth" },
			{ "Molerat", "Mole Rat" },
			{ "Mutation", "Snallygaster" },
			{ "Rad Frog", "Frog" },
			{ "Rad Stag", "Radstag" },
			{ "Rad Turkey", "Thrasher" },
			{ "Rat", "Rad Rat" },
			{ "Scorpion", "Rad Scorpion" },
			{ "Swamp", "Gulper" },
			{ "Toad", "Rad Toad" },
		};

		// Provides the WHERE clause for a query which defines the rules of which cells we should discard, as they are understood to be cut or otherwise inaccessible.
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

		public static string? GetCorrectedMarkerIcon(string markerName)
		{
			switch (markerName)
			{
				case "Ammo Dump":
				case "Bloody Frank's":
				case "Cliffwatch":
				case "Crimson Prospect":
				case "Dagger's Den":
				case "Hunter's Ridge":
				case "Ripper Alley":
				case "Rollins Labor Camp":
				case "Seneca Gang Camp":
				case "Skullbone Vantage":
				case "South Cutthroat Camp":
				case "The Bounty":
				case "The Crosshair":
				case "The Kill Box":
				case "The Pigsty":
				case "The Sludge Works":
				case "The Vantage":
				case "Twin Pine Cabins":
				case "Widow's Perch":
					return "BloodEagleMarker";

				case "Forward Station Alpha":
				case "Forward Station Delta":
				case "Forward Station Tango":
					return "BoSMarker";

				case "Blakes Offering":
				case "Clancy Manor":
				case "Ingram Mansion":
				case "Johnson's Acre":
				case "Kanawha County Cemetery":
				case "Lucky Hole Mine":
				case "Moth-Home":
				case "Organ Cave North":
				case "Organ Cave South":
				case "Organ Cave West":
				case "Sacrament":
					return "CultistMarker";

				case "Foundation":
					return "HammerWingMarker";

				case "The Rusty Pick":
					return "LegendaryPurveyorMarker";

				case "Ohio River Adventures":
				case "The Coop":
				case "The Crater":
					return "SkullRingMarker";

				case "Vault 51":
					return "Vault51Marker";

				case "Vault 79":
					return "Vault79Marker";

				case "Abandoned Bog Town":
				case "Beckley Mine Exhibit":
				case "Berkeley Springs West":
				case "Billings Homestead":
				case "Charleston Landfill":
				case "Converted Munitions Factory":
				case "Dabney Homestead":
				case "Dolly Sods Campground":
				case "Federal Disposal Field HZ-21":
				case "Gorge Junkyard":
				case "Grafton Steel Yard":
				case "Hemlock Holes Maintenance":
				case "Lakeside Cabins":
				case "Monongah Power Plant Yard":
				case "Mount Blair":
				case "Poseidon Energy Plant Yard":
				case "Red Rocket Mega Stop":
				case "Sunshine Meadows Industrial Farm":
				case "Thunder Mt. Power Plant Yard":
				case "Tyler County Dirt Track":
				case "Wade Airport":
					return "PublicWorkshopMarker";

				default:
					return null;
			}
		}

		// Values passed with the -xm argument to the render command
		public static List<string> RenderExcludeModels { get; } = new List<string>()
		{
			"babylon",
			"cranberrybog",
			"cranbog",
			"bog",
			"grass",
			"trees",
			"tree",
			"plants",
			"shrub",
			"vine",
			"root",
			"stump",
			"fog",
			"cloud",
			"effects",
		};

		// Fix fissure site naming - Rename Zeta to Prime, drop Greek alphabet names from all others
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
	}
}
