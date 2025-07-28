using System.Text.Json.Serialization;

namespace Library
{
	public enum Signature
	{
		// Order is used to define percieved relevance and thus sort in the UI
		LVLI,
		FLOR,
		NPC_,
		MISC,
		BOOK,
		ALCH,
		CONT,
		STAT,
		SCOL,
		MSTT,
		FURN,
		NOTE,
		TERM,
		ARMO,
		WEAP,
		AMMO,
		PROJ,
		CNCY,
		KEYM,
		REGN,
		ACTI,
		TACT,
		DOOR,
		HAZD,
		IDLM,
		LIGH,
		SOUN,
		SECH,
		ASPC,
		TXST,
		BNDS,
		WRLD,
		CELL,
	}

	// Represents any entity in the ESM
	public class Entity(uint formID, string editorID, string displayName, Signature signature)
	{
		public uint FormID { get; } = formID;

		[JsonIgnore]
		public string EditorID { get; } = editorID;

		[JsonIgnore]
		public string DisplayName { get; } = displayName;

		[JsonIgnore]
		public Signature Signature { get; } = signature;

		// Return a representation of the FormID suitable for display in the UI
		public virtual string GetFriendlyFormID()
		{
			return FormID.ToHex();
		}

		public override bool Equals(object? obj)
		{
			if (obj is null)
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (obj.GetType() != GetType())
			{
				return false;
			}

			if (obj is not Entity other)
			{
				return false;
			}

			return FormID.Equals(other.FormID) && EditorID.Equals(other.EditorID);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(FormID);
		}
	}
}
