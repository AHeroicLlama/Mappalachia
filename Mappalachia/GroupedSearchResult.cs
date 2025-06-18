using Library;

namespace Mappalachia
{
	// Represents an amount of BaseInstances which share the same basic properties
	// This is used to represent a row on the search results
	public class GroupedSearchResult(Entity entity, Space space, int count, int legendGroup, string label, LockLevel lockLevel, double spawnWeight = 1, bool inContainer = false)
		: BaseInstance(entity, space, label, lockLevel, spawnWeight, inContainer)
	{
		public int Count { get; } = count;

		public int LegendGroup { get; } = legendGroup;

		public PlotIcon? PlotIcon { get; set; } = null;

		// Properties referenced by UI DGVs to map data to cells
		// they are referenced via a string value of their name
		public virtual string DataValueFormID => Entity.GetFriendlyFormID();

		public string DataValueEditorID => Entity.EditorID;

		public string DataValueDisplayName => Entity.DisplayName;

		public string DataValueSignature => Mappalachia.GetIsAdvanced() ? Entity.Signature.ToString() : Entity.Signature.ToFriendlyName();

		public string DataValueLabel => Label;

		public string DataValueInContainer => InContainer ? "Yes" : "No";

		public string DataValueLockLevel => !Entity.Signature.IsLockable() && !InContainer && LockLevel == LockLevel.None ?
						"N/A" : LockLevel.ToFriendlyName();

		public string DataValueSpawnWeight => Math.Round(SpawnWeight * 100, 2).ToString();

		public string DataValueCount => Count.ToString();

		public string DataValueLocation => Mappalachia.GetIsAdvanced() ? Space.EditorID : Space.DisplayName;
	}
}
