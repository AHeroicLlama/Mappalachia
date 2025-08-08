using System.Text.Json.Serialization;
using Library;

namespace Mappalachia
{
	// Represents an amount of BaseInstances which share the same basic properties
	// This is used to represent a row on the search results
	public class GroupedSearchResult(Entity entity, Space space, int count = 1, double spawnWeight = 1, string label = "", LockLevel lockLevel = LockLevel.None, bool inContainer = false)
		: BaseInstance(entity, space, label, lockLevel, spawnWeight, inContainer)
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

		public string LegendText { get; set; } = string.Empty;

		public void GenerateLegendText()
		{
			string text;

			List<string> additionalInfo = new List<string>();

			if (Entity is DerivedNPC || Entity is DerivedScrap || Entity is DerivedRawFlux)
			{
				text = Entity.DisplayName;
			}
			else
			{
				text = $"{Entity.EditorID}";

				if (!Entity.DisplayName.IsNullOrWhiteSpace())
				{
					additionalInfo.Add($"\"{Entity.DisplayName}\"");
				}
			}

			if (!Label.IsNullOrWhiteSpace())
			{
				additionalInfo.Add(Label);
			}

			if (LockLevelRelevant())
			{
				additionalInfo.Add(LockLevel.ToFriendlyName());
			}

			if (additionalInfo.Count > 0)
			{
				text += $" ({string.Join(", ", additionalInfo)})";
			}

			if (Entity is DerivedNPC)
			{
				text += $" [{Math.Round(SpawnWeight * 100, 2)}%]";
			}

			if (Entity is DerivedScrap)
			{
				text += $" [x{SpawnWeight}]";
			}

			LegendText = text;
		}

		bool LockLevelRelevant()
		{
			return Entity.Signature.IsLockable() || InContainer || LockLevel != LockLevel.None;
		}

		// Properties referenced by UI DGVs to map data to cells
		// they are referenced via a string value of their name
		[JsonIgnore]
		public virtual string DataValueFormID => Entity.GetFriendlyFormID();

		[JsonIgnore]
		public string DataValueEditorID => Entity.EditorID;

		[JsonIgnore]
		public string DataValueDisplayName => Entity.DisplayName;

		[JsonIgnore]
		public string DataValueSignature => Mappalachia.GetIsAdvanced() ? Entity.Signature.ToString() : Entity.Signature.ToFriendlyName();

		[JsonIgnore]
		public string DataValueLabel => Label;

		[JsonIgnore]
		public string DataValueInContainer => InContainer ? "Yes" : "No";

		[JsonIgnore]
		public string DataValueLockLevel => LockLevelRelevant() ? LockLevel.ToFriendlyName() : "N/A";

		[JsonIgnore]
		public double DataValueSpawnWeight => SpawnWeight * 100;

		[JsonIgnore]
		public int DataValueCount => Count;

		[JsonIgnore]
		public string DataValueLocation => Mappalachia.GetIsAdvanced() ? Space.EditorID : Space.DisplayName;
	}
}
