﻿using System.Collections.Generic;

namespace CommonwealthCartography
{
	internal static class MapMarkers
	{
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
			{ "SwanPondMarker", "SwanPondMarker" },
			{ "BrownstoneTownhouseMarker", "BrownstoneMarker" },
			{ "LowRiseBuildingMarker", "LowRiseMarker" },
			{ "MechanistLairRaidersettlementVassalsettlementPotentialVassalsettlementMarker", "MechanistMarker" },
			{ "Custom66Marker", "RaiderSettlementMarker" },
			{ "Custom70Marker", "GalacticMarker" },
			{ "Custom73Marker", "MonorailMarker" },
			{ "Custom74Marker", "RidesMarker" },
			{ "Custom75Marker", "SafariMarker" },
			{ "Custom78Marker", "DisciplesMarker" },
			{ "Custom79Marker", "OperatorsMarker" },
			{ "Custom80Marker", "PackMarker" },
		};

		static readonly Dictionary<string, string> wrongLabelNames = new Dictionary<string, string>()
		{
			{ "ERROR: <<NO REF LOCATION>>", "Nuka-World Junkyard" },
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
