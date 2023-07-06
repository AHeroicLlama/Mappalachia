using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Mappalachia
{
	// Any item in the game which can show up in search results or could exist on the legend key and be mapped
	// NOT a unique reference to an instance of an item, but rather a description of all of the same type of item.
	public class MapItem
	{
		public enum Type
		{
			Standard,
			Scrap,
			NPC,
			Region,
		}

		public readonly Type type; // What type of search did this item come from and what does it represent
		public readonly string uniqueIdentifier; // The FormID, Scrap name or NPC name which this MapItem represents
		public readonly string editorID; // The editorID of the item
		public readonly string displayName; // The Display Name of the item where applicable
		public readonly string signature; // The signature eg MISC
		public readonly List<string> filteredLockTypes; // The lock types which were selected when this item was picked from the database
		public readonly float weight; // The spawn chance or weighting of this item (eg 2x scrap from junk = 2.0, 33% chance of NPC spawn = 0.33). -1 means "Varies"
		public readonly int count; // How many of this item did we find.
		public readonly string spaceName; // Display Name of the location where this item was placed.
		public readonly string spaceEditorID; // EditorID of the location
		public int legendGroup; // User-definable grouping value
		public string overridingLegendText = string.Empty; // The user-provided legend text, if given
		public string label; // The "label" (XEdit 'ShortName') attached to the reference

		List<MapDataPoint> plots;

		public MapItem(Type type, string uniqueIdentifier, string editorID, string displayName, string signature, List<string> filteredLockTypes, float weight, int count, string spaceEditorID, string spaceName, string label)
		{
			this.type = type;
			this.uniqueIdentifier = uniqueIdentifier;
			this.editorID = editorID;
			this.displayName = displayName;
			this.filteredLockTypes = filteredLockTypes;
			this.signature = signature;
			this.weight = weight;
			this.count = count;
			this.spaceName = spaceName;
			this.spaceEditorID = spaceEditorID;
			this.label = label;
		}

		// The lock type is relevant only if it's a 'standard', lockable item with modified/filtered lock types.
		public bool GetLockRelevant()
		{
			return
				type == Type.Standard &&
				DataHelper.lockableTypes.Contains(signature) &&
				!filteredLockTypes.OrderBy(e => e).SequenceEqual(Database.GetLockTypes().OrderBy(e => e));
		}

		public bool IsRegion()
		{
			return type == Type.Region;
		}

		// Get the image-scaled coordinate points for all instances of this MapItem
		public List<MapDataPoint> GetPlots()
		{
			switch (type)
			{
				case Type.Standard:
					plots = Database.GetStandardCoords(uniqueIdentifier, SettingsSpace.GetCurrentFormID(), filteredLockTypes, label, weight == -1 ? 1 : weight / 100f);
					break;
				case Type.NPC:
					plots = Database.GetNPCCoords(uniqueIdentifier, SettingsSpace.GetCurrentFormID(), weight);
					break;
				case Type.Scrap:
					plots = Database.GetScrapCoords(uniqueIdentifier, SettingsSpace.GetCurrentFormID());
					break;
				case Type.Region:
					plots = Database.GetRegionCoords(uniqueIdentifier, SettingsSpace.GetCurrentFormID());
					break;
			}

			Space currentSpace = SettingsSpace.GetSpace();
			foreach (MapDataPoint point in plots)
			{
				point.x = Map.ScaleCoordinateToSpace(point.x, false);
				point.y = Map.ScaleCoordinateToSpace(point.y, true);

				point.boundX = Math.Max(point.boundX * currentSpace.scale, Map.minVolumeDimension);
				point.boundY = Math.Max(point.boundY * currentSpace.scale, Map.minVolumeDimension);
			}

			return plots;
		}

		// Return the entire Region object represented here
		public Region GetRegion()
		{
			if (!IsRegion())
			{
				throw new InvalidOperationException("Not a Region type");
			}

			return new Region(Database.GetRegionPoints(uniqueIdentifier, SettingsSpace.GetCurrentFormID()));
		}

		// Get a user-friendly or user-defined text representation of the MapItem to be used on the legend
		// forceDefault to ignore user override and return to auto-generated
		public string GetLegendText(bool forceDefault)
		{
			if (!forceDefault && !string.IsNullOrEmpty(overridingLegendText))
			{
				return overridingLegendText;
			}

			if (type == Type.Standard)
			{
				string lockRemark = string.Empty;

				if (GetLockRelevant())
				{
					// If the filtered types are all but "Not locked"
					if (filteredLockTypes.OrderBy(e => e).SequenceEqual(Database.GetLockTypes().Where(l => !string.IsNullOrEmpty(l)).OrderBy(e => e)))
					{
						lockRemark = " (Locked)";
					}
					else
					{
						lockRemark = " (" + string.Join(", ", DataHelper.ConvertLockLevel(filteredLockTypes, false)) + ")";
					}
				}

				string labelRemark = !string.IsNullOrEmpty(label) ? $" ({label})" : string.Empty;
				string nameRemark = !string.IsNullOrEmpty(displayName) ? $" ({displayName})" : string.Empty;

				return editorID + nameRemark + labelRemark + lockRemark;
			}
			else
			{
				return editorID;
			}
		}

		// Find the appropriate legend color for this item
		// Varies on the plotting mode, and further on the heatmap color mode
		public Color GetLegendColor()
		{
			if (IsRegion())
			{
				return GetIcon().color;
			}

			switch (SettingsPlot.mode)
			{
				case SettingsPlot.Mode.Icon:
					return GetIcon().color;

				case SettingsPlot.Mode.Topography:
					return SettingsPlotTopograph.legendColor;

				case SettingsPlot.Mode.Heatmap:
					return SettingsPlotHeatmap.IsMono() ?
					SettingsPlotStyle.GetFirstColor() :
					(legendGroup % 2 == 0 ? SettingsPlotStyle.GetFirstColor() : SettingsPlotStyle.GetSecondColor());

				case SettingsPlot.Mode.Cluster:
					return SettingsPlotStyle.GetFirstColor();

				default:
					return Color.Gray;
			}
		}

		public PlotIcon GetIcon()
		{
			return PlotIcon.GetIconForGroup(legendGroup, this);
		}

		// Override equals to compare MapItem - we use the unique identifier and if they're a normal item, also the filtered lock type.
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			MapItem mapItem = (MapItem)obj;

			switch (mapItem.type)
			{
				case Type.Standard:
					return mapItem.uniqueIdentifier == uniqueIdentifier && mapItem.filteredLockTypes.SequenceEqual(filteredLockTypes) && mapItem.label == label;
				case Type.NPC:
					return mapItem.uniqueIdentifier == uniqueIdentifier && mapItem.weight == weight;
				case Type.Scrap:
				case Type.Region:
					return mapItem.uniqueIdentifier == uniqueIdentifier;
				default:
					return false;
			}
		}

		public override int GetHashCode()
		{
			switch (type)
			{
				case Type.Standard:
					return (uniqueIdentifier + string.Join(string.Empty, filteredLockTypes) + label).GetHashCode();
				case Type.NPC:
					return (uniqueIdentifier + weight).GetHashCode();
				case Type.Scrap:
				case Type.Region:
					return uniqueIdentifier.GetHashCode();
				default:
					return -1;
			}
		}
	}
}
