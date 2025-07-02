using System.Drawing;
using System.Text.Json.Serialization;

namespace Library
{
	// Represents a Worldspace (WRLD) or Cell (CELL)
	public class Space(uint formID, string editorID, string displayName, bool isWorldspace, double centerX, double centerY, double maxRange)
		: Entity(formID, editorID, displayName, isWorldspace ? Signature.WRLD : Signature.CELL)
	{
		[JsonIgnore]
		public bool IsWorldspace { get; } = isWorldspace;

		[JsonIgnore]
		public double CenterX { get; } = centerX;

		[JsonIgnore]
		public double CenterY { get; } = centerY;

		[JsonIgnore]
		public double MaxRange { get; } = maxRange;

		[JsonIgnore]
		public double Radius { get; } = maxRange / 2d;

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
			return EditorID.Equals("APPALACHIA", StringComparison.OrdinalIgnoreCase);
		}

		// Return the spotlight tiles for the space
		public List<SpotlightTile> GetTiles()
		{
			return SpotlightTile.GetTilesInRect(new RectangleF((float)MinX, (float)MaxY, (float)(MaxX - MinX), (float)(MaxY - MinY) * -1), this);
		}
	}
}
