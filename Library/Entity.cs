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

		public string EditorID { get; } = editorID;

		public string DisplayName { get; } = displayName;

		public Signature Signature { get; } = signature;

		public override bool Equals(object? obj)
		{
			if (obj is not Entity other || obj is null)
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			return FormID.Equals(other.FormID) && EditorID.Equals(other.EditorID);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(FormID);
		}
	}
}
