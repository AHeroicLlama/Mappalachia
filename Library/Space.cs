using System.Drawing;
using System.Text.Json.Serialization;

namespace Library
{
	// Represents a Worldspace (WRLD) or Cell (CELL)
	public class Space(uint formID, string editorID, string displayName, bool isWorldspace, bool isInstanceable, double centerX, double centerY, double maxRange, double northAngle)
		: Entity(formID, editorID, displayName, isWorldspace ? Signature.WRLD : Signature.CELL)
	{
		[JsonIgnore]
		public bool IsWorldspace { get; } = isWorldspace;

		[JsonIgnore]
		public bool IsInstanceable { get; } = isInstanceable;

		[JsonIgnore]
		public double CenterX { get; } = centerX;

		[JsonIgnore]
		public double CenterY { get; } = centerY;

		[JsonIgnore]
		public double MaxRange { get; } = maxRange;

		[JsonIgnore]
		public double NorthAngle { get; } = northAngle;

		[JsonIgnore]
		public double MinX { get; } = centerX - (maxRange / 2d);

		[JsonIgnore]
		public double MaxX { get; } = centerX + (maxRange / 2d);

		[JsonIgnore]
		public double MinY { get; } = centerY - (maxRange / 2d);

		[JsonIgnore]
		public double MaxY { get; } = centerY + (maxRange / 2d);

		// Accessed via variable name in the UI - provides the name for the Space select dropdown
		[JsonIgnore]
		public string FriendlyName => $"{DisplayName} ({EditorID})";

		public bool IsAppalachia()
		{
			return EditorID.EqualsIgnoreCase("APPALACHIA");
		}

		// Return if this Space can be targeted (or otherwise somehow affected) by a nuke
		public bool IsNukable()
		{
			return IsAppalachia();
		}

		// Return the EditorIDs of known regions in this space which are "non-nukable"
		public List<string> GetNonNukableZoneEditorIds()
		{
			return IsAppalachia() ? new List<string> { "NonNukableZone" } : new List<string>();
		}

		// Return the spotlight tiles for the space
		public List<SpotlightTile> GetTiles()
		{
			return SpotlightTile.GetTilesInRect(new RectangleF((float)MinX, (float)MaxY, (float)Math.Abs(MaxX - MinX), (float)Math.Abs(MaxY - MinY)), this);
		}

		public Coord GetCenter()
		{
			return new Coord(CenterX, CenterY);
		}

		public RectangleF GetRectangle()
		{
			return new RectangleF((float)MinX, (float)MaxY, (float)Math.Abs(MaxX - MinX), (float)Math.Abs(MaxY - MinY));
		}

		// Return the maximum spotlight size which the space could utilise, before it actually makes the view smaller than the non-spotlight perspective
		public double GetMaxSpotlightBenefit()
		{
			return MaxRange / Common.TileWidth;
		}

		// Return if the Space is large enough to meet the minimum threshold to suitably benefit from spotlight
		public bool IsSuitableForSpotlight()
		{
			return GetMaxSpotlightBenefit() >= Common.SpotlightMinTileResolution;
		}
	}
}
