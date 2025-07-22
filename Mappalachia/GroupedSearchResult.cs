using Library;

namespace Mappalachia
{
	// Represents an amount of BaseInstances which share the same basic properties
	// This is used to represent a row on the search results
	public class GroupedSearchResult(Entity entity, Space space, int count = 1, double spawnWeight = 1, string label = "", LockLevel lockLevel = LockLevel.None, bool inContainer = false)
		: BaseInstance(entity, space, label, lockLevel, spawnWeight, inContainer), IDisposable
	{
		PlotIcon? plotIcon;

		public PlotIcon PlotIcon
		{
			get
			{
				return plotIcon ?? throw new Exception("Search result Plot Icon not assigned");
			}

			set
			{
				plotIcon = value;
			}
		}

		public int Count { get; } = count;

		public int LegendGroup { get; set; } = 0;

		public string OverridingLegendText { private get; set; } = string.Empty;

		public string GetLegendText()
		{
			if (!OverridingLegendText.IsNullOrWhiteSpace())
			{
				return OverridingLegendText;
			}

			string text;

			if (Entity is DerivedNPC || Entity is DerivedScrap)
			{
				text = Entity.DisplayName;
			}
			else
			{
				text = $"{Entity.EditorID}";

				if (!Entity.DisplayName.IsNullOrWhiteSpace())
				{
					text += $" ({Entity.DisplayName})";
				}
			}

			if (Entity is DerivedNPC)
			{
				text += $" [{Math.Round(SpawnWeight * 100, 2)}%]";
			}

			if (Entity is DerivedScrap)
			{
				text += $" [x{SpawnWeight}]";
			}

			return text;
		}

		// Properties referenced by UI DGVs to map data to cells
		// they are referenced via a string value of their name
		public virtual string DataValueFormID => Entity.GetFriendlyFormID();

		public string DataValueEditorID => Entity.EditorID;

		public string DataValueDisplayName => Entity.DisplayName;

		public string DataValueSignature => Mappalachia.GetIsAdvanced() ? Entity.Signature.ToString() : Entity.Signature.ToFriendlyName();

		public string DataValueLabel => Label;

		public string DataValueInContainer => InContainer ? "Yes" : "No";

		public string DataValueLockLevel => !Entity.Signature.IsLockable() && !InContainer && LockLevel == LockLevel.None ? "N/A" : LockLevel.ToFriendlyName();

		public double DataValueSpawnWeight => SpawnWeight * 100;

		public int DataValueCount => Count;

		public string DataValueLocation => Mappalachia.GetIsAdvanced() ? Space.EditorID : Space.DisplayName;

		public void Dispose()
		{
			plotIcon?.Dispose();
			plotIcon = null;
			GC.SuppressFinalize(this);
		}
	}
}
