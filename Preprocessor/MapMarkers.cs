using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CommonwealthCartography
{
	internal static class MapMarkers
	{
		const string fastTravelBadString = "Fast Travel Point: ";
		static readonly Regex validMapMarkerName = new Regex("^(.*)$");
		static readonly Regex badMapMarkerNames = new Regex("^(Door|Quest|PowerArmorLoc|PlayerLoc)Marker$");

		static readonly Dictionary<string, string> locationMarkerCorrection = new Dictionary<string, string>()
		{

		};

		static readonly Dictionary<string, string> markerNameCorrection = new Dictionary<string, string>()
		{
			{ "Gov'tBuildingMonumentMarker", "MonumentMarker" },
			{ "MetroStationMarker", "MetroMarker" },
			{ "OfficeCivicBuildingMarker", "OfficeMarker" },
			{ "NaturalLandmarkMarker", "LandmarkMarker" },
			{ "RuinsUrbanMarker", "UrbanRuinsMarker" },
			{ "RuinsTownMarker", "TownRuinsMarker" },
			{ "FactoryIndustrialSiteMarker", "FactoryMarker" },
			{ "SewerUtilityTunnelsMarker", "SewerMarker" },
			{ "SanctuaryMarker", "SancHillsMarker" },
			{ "SwanPondMarker", "PondLakeMarker" },
		};

		static readonly Dictionary<string, string> wrongLabelNames = new Dictionary<string, string>()
		{

		};

		static readonly List<string> removedLabels = new List<string>()
		{

		};

		// Pull the MapMarker display text from position data and store it in a new file
		public static CSVFile ProcessMapMarkers(CSVFile positionData)
		{
			List<string> newFileHeader = new List<string> { "spaceFormID", "label", "mapMarkerName", "x", "y", "esmNumber" };
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
				string iconName = row.GetCellFromColumn("mapMarkerName") + "Marker";

				/* Fix incorrect Map Marker Icons.
				There must be something basic data mining misses, or an XEdit bug
				or the game changes them on the fly, but some labels and icons are different in-game.
				This appears to largely but not exclusively affect content changed or added with Wastelanders */

				// Swap or amend some entirely wrong names
				if (wrongLabelNames.ContainsKey(label))
				{
					label = wrongLabelNames[label];
				}

				// This marker is in the data but not in-game so we drop it
				if (removedLabels.Contains(label))
				{
					continue;
				}

				// Removes "Fast Travel Point: " from some (typically station) names
				if (label.StartsWith(fastTravelBadString))
				{
					label = label.Replace(fastTravelBadString, string.Empty);
				}

				iconName = iconName.Replace(" ", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);

				// Correct the actual marker name
				if (markerNameCorrection.ContainsKey(iconName))
				{
					iconName = markerNameCorrection[iconName];
				}

				// Large collection of incorrect icons - Correct markers after labels have bene corrected
				if (locationMarkerCorrection.ContainsKey(label))
				{
					iconName = locationMarkerCorrection[label];
				}

				// Perform our own specialized validation
				if (badMapMarkerNames.IsMatch(iconName) ||
					!validMapMarkerName.IsMatch(iconName) ||
					wrongLabelNames.ContainsKey(label))
				{
					throw new System.Exception("Map Marker failed internal validation: " + label + ", " + iconName);
				}

				// ... Finally, we assume this is a (corrected) map marker
				string newRow =
					spaceFormID + "," +
					label + "," +
					iconName + "," +
					row.GetCellFromColumn("x") + "," +
					row.GetCellFromColumn("y") + "," +
					row.GetCellFromColumn("esmNumber");

				newFileRows.Add(new CSVRow(newRow, newFileHeader));
			}

			return new CSVFile("Map_Markers.csv", newFileHeader, newFileRows);
		}
	}
}
