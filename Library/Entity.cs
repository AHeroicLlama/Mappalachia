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
		CELL,
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
		REGN,
		SCOL,
		SECH,
		SOUN,
		STAT,
		TACT,
		TERM,
		TXST,
		WEAP,
		WRLD,
	}

	// Represents any entity in the ESM
	public class Entity(uint formID, string editorID, string displayName, Signature signature)
	{
		public uint FormID { get; } = formID;

		public string EditorID { get; } = editorID;

		public string DisplayName { get; } = displayName;

		public Signature Signature { get; } = signature;

		// Overload for passing Signature as string
		public Entity(uint formID, string editorID, string displayName, string signature)
			: this(formID, editorID, displayName, Enum.Parse<Signature>(signature))
		{
		}
	}
}
