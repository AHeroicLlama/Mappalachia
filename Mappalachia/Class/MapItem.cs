using System;
using System.Collections.Generic;
using System.Linq;

namespace Mappalachia
{
	public enum Type
	{
		Simple,
		Scrap,
		NPC,
	}

	//Any item in the game which can show up in search results or could exist on the legend key and be mapped
	//NOT a unique reference to an instance of an item, but rather a description of all of the same type of item.
	public class MapItem
	{
		//Signatures which could be affected by a lock level
		static readonly List<string> lockableTypes = new List<string> { "CONT", "DOOR", "TERM" };

		public readonly Type type; //What type of search did this item come from and what does it represent
		public readonly string uniqueIdentifier; //The FormID, Scrap name or NPC name which this MapItem represents
		public readonly string editorID; //The editorID of the item
		public readonly string displayName; //The Display Name of the item where applicable
		public readonly string signature; //The signature eg MISC
		public readonly List<string> filteredLockTypes; //The lock types which were selected when this item was picked from the database
		public readonly double weight; //The spawn chance or weighting of this item (eg 2x scrap from junk = 2.0, 33% chance of NPC spawn = 0.33). -1 means "Varies"
		public readonly int count; //How many of this item did we find.
		public readonly string location; //Display Name of the location where was this item placed.
		public readonly string locationEditorID; //EditorID of the location
		public int legendGroup; //User-definable grouping value

		public MapItem(Type type, string uniqueIdentifier, string editorID, string displayName, string signature, List<string> filteredLockTypes, double weight, int count, string location, string locationID)
		{
			this.type = type;
			this.uniqueIdentifier = uniqueIdentifier;
			this.editorID = editorID;
			this.displayName = displayName;
			this.filteredLockTypes = filteredLockTypes;
			this.signature = signature;
			this.weight = weight;
			this.count = count;
			this.location = location;
			this.locationEditorID = locationID;
		}

		//The lock type is relevant only if it's a 'simple', lockable item with modified/filtered lock types.
		public bool GetLockRelevant()
		{
			return
				type == Type.Simple &&
				lockableTypes.Contains(signature) &&
				!filteredLockTypes.OrderBy(e => e).SequenceEqual(DataHelper.GetPermittedLockTypes());
		}

		//Get the image-scaled coordinate points for all instances of this MapItem
		public List<MapDataPoint> GetPlots()
		{
			switch (type)
			{
				case Type.Simple:
					return SettingsMap.mode == SettingsMap.Mode.Cell ?
						DataHelper.GetCellCoords(uniqueIdentifier, FormMaster.currentlySelectedCell.formID, filteredLockTypes) :
						DataHelper.GetSimpleCoords(uniqueIdentifier, filteredLockTypes);
				case Type.NPC:
					return DataHelper.GetNPCCoords(uniqueIdentifier, weight);
				case Type.Scrap:
					return DataHelper.GetScrapCoords(uniqueIdentifier);
				default:
					return null;
			}
		}

		//Get a user-friendly text representation of the MapItem to be used on the legend
		public string GetLegendText()
		{
			if (type == Type.Simple)
			{
				//Return the editorID, plus the displayName (if it exists), plus the lock levels (if they're relevant)
				return (displayName == string.Empty ?
							editorID :
							editorID + " (" + displayName + ")") +
						(GetLockRelevant() ?
							" (" + string.Join(", ", DataHelper.ConvertLockLevelCollection(filteredLockTypes, false)) + ")" :
							string.Empty);
			}
			else
			{
				return editorID;
			}
		}

		//Override equals to compare MapItem - we use the unique identifier and if they're a normal item, also the filtered lock type.
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			MapItem mapItem = (MapItem)obj;

			switch (mapItem.type)
			{
				case Type.Simple:
					return mapItem.uniqueIdentifier == uniqueIdentifier && mapItem.filteredLockTypes.SequenceEqual(filteredLockTypes);
				case Type.NPC:
					return mapItem.uniqueIdentifier == uniqueIdentifier && mapItem.weight == weight;
				case Type.Scrap:
					return mapItem.uniqueIdentifier == uniqueIdentifier;
				default:
					throw new Exception("Unsupported MapItem type " + mapItem.type);
			}
		}

		public override int GetHashCode()
		{
			switch (type)
			{
				case Type.Simple:
					return (uniqueIdentifier + string.Join(string.Empty, filteredLockTypes)).GetHashCode();
				case Type.NPC:
					return (uniqueIdentifier + weight).GetHashCode();
				case Type.Scrap:
					return uniqueIdentifier.GetHashCode();
				default:
					throw new Exception("Unsupported MapItem type " + type);
			}
		}
	}
}
