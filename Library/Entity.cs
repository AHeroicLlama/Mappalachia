namespace Library
{
	public enum Signature
	{
		ACTI,
		ALCH,
		AMMO,
		ARMO,
		ASPC,
		BNDS,
		BOOK,
		CNCY,
		CONT,
		DOOR,
		FLOR,
		FURN,
		HAZD,
		IDLM,
		KEYM,
		LIGH,
		LVLI,
		MISC,
		MSTT,
		NOTE,
		NPC_,
		PROJ,
		SCOL,
		SECH,
		SOUN,
		STAT,
		TACT,
		TERM,
		TXST,
		WEAP,
	}

	// Represents any entity in the ESM
	public class Entity(uint formID, string editorID, string displayName, string signature)
	{
		public uint FormID { get; } = formID;

		public string EditorID { get; } = editorID;

		public string DisplayName { get; } = displayName;

		public Signature Signature { get; } = Enum.Parse<Signature>(signature);
	}
}
