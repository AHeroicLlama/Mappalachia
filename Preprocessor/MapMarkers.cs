using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mappalachia
{
	internal static class MapMarkers
	{
		static readonly string bloodEagleMarker = "BloodEagleMarker";
		static readonly string cultistMarker = "CultistMarker";
		static readonly string fastTravelBadString = "Fast Travel Point: ";
		static readonly string fissureSite = "Fissure Site";
		static readonly Regex validMapMarkerName = new Regex("^(([A-Z].*Marker)|WhitespringResort|NukaColaQuantumPlant|TrainTrackMark)$");
		static readonly Regex badMapMarkerNames = new Regex("^(Door|Quest|PowerArmorLoc|PlayerLoc)Marker$");
		static readonly Regex biomeNames = new Regex("^(The )?(Mire|Cranberry Bog|Forest|Toxic Valley|Savage Divide|Mountain)( Region)?$");
		static readonly Dictionary<string, string> locationMarkerCorrection = new Dictionary<string, string>()
		{
			{ "Ammo Dump", bloodEagleMarker },
			{ "Bloody Frank's", bloodEagleMarker },
			{ "Cliffwatch", bloodEagleMarker },
			{ "Crimson Prospect", bloodEagleMarker },
			{ "Hunter's Ridge", bloodEagleMarker },
			{ "Ripper Alley", bloodEagleMarker },
			{ "Rollins Labor Camp", bloodEagleMarker },
			{ "Seneca Gang Camp", bloodEagleMarker },
			{ "Skullbone Vantage", bloodEagleMarker },
			{ "South Cutthroat Camp", bloodEagleMarker },
			{ "The Bounty", bloodEagleMarker },
			{ "The Crosshair", bloodEagleMarker },
			{ "The Kill Box", bloodEagleMarker },
			{ "The Pigsty", bloodEagleMarker },
			{ "The Sludge Works", bloodEagleMarker },
			{ "The Vantage", bloodEagleMarker },
			{ "Twin Pine Cabins", bloodEagleMarker },
			{ "Widow's Perch", bloodEagleMarker },
			{ "Blakes Offering", cultistMarker },
			{ "Clancy Manor", cultistMarker },
			{ "Ingram Mansion", cultistMarker },
			{ "Johnson's Acre", cultistMarker },
			{ "Kanawha County Cemetery", cultistMarker },
			{ "Lucky Hole Mine", cultistMarker },
			{ "Moth-Home", cultistMarker },
			{ "Sacrament", cultistMarker },
			{ "Foundation", "HammerWingMarker" },
			{ "Hawke's Refuge", "CaveMarker" },
			{ "Ohio River Adventures", "SkullRingMarker" },
			{ "The Crater", "SpaceStationMarker" },
			{ "The Rusty Pick", "LegendaryPurveyorMarker" },
			{ "Vault 51", "Vault51Marker" },
			{ "Vault 79", "Vault79Marker" },
		};

		static readonly Dictionary<string, string> inaccurateBiomeNames = new Dictionary<string, string>()
		{
			{ "Mountain Region", "Colonel Kelly Monument" },
			{ "The Savage Divide", "Monorail Elevator" },
			{ "Cranberry Bog Region", "Quarry X3" },
		};

		// Pull the MapMarker display text from position data and store it in a new file
		public static CSVFile ProcessMapMarkers(CSVFile positionData)
		{
			List<string> newFileHeader = new List<string> { "spaceFormID", "label", "mapMarkerName", "x", "y" };
			List<CSVRow> newFileRows = new List<CSVRow>();

			foreach (CSVRow row in positionData.rows)
			{
				// Uniquely for map markers, xEdit scripts return the map marker label under what is normally the reference column
				string label = row.GetCellFromColumn("referenceFormID");

				if (Validation.matchFormID.IsMatch(label))
				{
					continue; // This is a normal entity which came without an actual display name so we assume it's not a map marker
				}

				string spaceFormID = row.GetCellFromColumn("spaceFormID");
				string iconName = row.GetCellFromColumn("mapMarkerName");

				/* Fix incorrect Map Marker Icons.
				There must be something basic data mining misses, or an XEdit bug
				or the game changes them on the fly, but some labels and icons are different in-game.
				This appears to largely but not exclusively affect content changed or added with Wastelanders */

				// Somehow a few locations are named as their parent region
				// There is also a Mire marker which marks nothing particular so we drop it
				if (inaccurateBiomeNames.ContainsKey(label))
                {
					label = inaccurateBiomeNames[label];
                }

				if (label == "Mire")
                {
					continue;
				}

				// Large collection of incorrect Wastelanders icons - suspect xedit bug?
				if (locationMarkerCorrection.ContainsKey(label))
				{
					iconName = locationMarkerCorrection[label];
                }

				// Misnamed workshop
				if (label == "Hemlock Holes" && iconName == "FactoryMarker")
				{
					label = "Hemlock Holes Maintenance";
				}

				// Removes "Fast Travel Point: " from some (typically station) names
				if (label.StartsWith(fastTravelBadString))
                {
					label = label.Replace(fastTravelBadString, string.Empty);
                }

				// Fix fissure site naming - Rename Zeta to Alpha, drop names from all others
				if (label.StartsWith(fissureSite))
				{
					if (label == "Fissure Site Zeta")
					{
						label = "Fissure Site Alpha";
					}
					else
					{
						label = fissureSite;
					}
				}

				// Perform our own specialized validation
				if (badMapMarkerNames.IsMatch(iconName) ||
					!validMapMarkerName.IsMatch(iconName) ||
					biomeNames.IsMatch(label))
				{
					throw new System.Exception("Map Marker failed internal validation: " + label + ", " + iconName);
				}

				// ... Finally, we assume this is a (corrected) map marker
				string newRow =
					spaceFormID + "," +
					label + "," +
					iconName + "," +
					row.GetCellFromColumn("x") + "," +
					row.GetCellFromColumn("y");

				newFileRows.Add(new CSVRow(newRow, newFileHeader));
			}

			return new CSVFile("Map_Markers.csv", newFileHeader, newFileRows);
		}
	}
}
