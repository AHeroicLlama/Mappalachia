namespace Mappalachia
{
	using Library;

	// Provides extensions and supporting functionality for mapping values between data and user speak
	public static class DataTranslator
	{
		public static string ToFriendlyName(this LockLevel lockLevel)
		{
			return lockLevel switch
			{
				LockLevel.None => "None",
				LockLevel.Level0 => "Level 0",
				LockLevel.Level1 => "Level 1",
				LockLevel.Level2 => "Level 2",
				LockLevel.Level3 => "Level 3",
				LockLevel.RequiresKey => "Requires Key",
				LockLevel.Inaccessible => "Inaccessible",
				LockLevel.RequiresTerminal => "Requires Terminal",
				LockLevel.Chained => "Chained",
				LockLevel.Unknown => "Unknown",
				_ => throw new ArgumentException($"Unexpected lock level {lockLevel}"),
			};
		}

		public static string ToFriendlyName(this Signature signature)
		{
			return signature switch
			{
				Signature.ACTI => "Activator",
				Signature.ALCH => "Aid",
				Signature.AMMO => "Ammo",
				Signature.ARMO => "Armor/Apparel",
				Signature.ASPC => "Acoustics",
				Signature.BNDS => "Spline",
				Signature.BOOK => "Note/Plan",
				Signature.CELL => "Cell",
				Signature.CNCY => "Currency",
				Signature.CONT => "Container",
				Signature.DOOR => "Door",
				Signature.FLOR => "Flora",
				Signature.FURN => "Furniture",
				Signature.HAZD => "Hazard",
				Signature.IDLM => "Idle marker",
				Signature.KEYM => "Key",
				Signature.LIGH => "Light",
				Signature.LVLI => "Loot",
				Signature.MISC => "Junk/Scrap",
				Signature.MSTT => "Moveable static",
				Signature.NOTE => "Holotape",
				Signature.NPC_ => "NPC",
				Signature.PROJ => "Projectile",
				Signature.REGN => "Region",
				Signature.SCOL => "Static collection",
				Signature.SECH => "Echo",
				Signature.SOUN => "Sound",
				Signature.STAT => "Static object",
				Signature.TACT => "Voice activator",
				Signature.TERM => "Terminal",
				Signature.TXST => "Decal",
				Signature.WEAP => "Weapon",
				Signature.WRLD => "World",
				_ => throw new ArgumentException($"Unexpected signature {signature}"),
			};
		}

		// Returns the long-form description of the Signature, or empty string if already obvious by name
		public static string GetDescription(this Signature signature)
		{
			return signature switch
			{
				Signature.ACTI => "Defined trigger volume, invisible in-game.\n" +
					"Activators can mark out designated areas, trigger events, or act as 'hit-boxes' for interaction.",
				Signature.ALCH => "Food, drink, chems etc.",
				Signature.AMMO => string.Empty,
				Signature.ARMO => string.Empty,
				Signature.ASPC => "Applies specific environmental acoustics.",
				Signature.BNDS => "A curved line shape. Usually used for power lines.",
				Signature.BOOK => "Note, Plan or Recipe.",
				Signature.CELL => "Another location separate from the main outside world.",
				Signature.CNCY => string.Empty,
				Signature.CONT => "Anything which can hold items.",
				Signature.DOOR => string.Empty,
				Signature.FLOR => "Collectable natural resource.",
				Signature.FURN => "Object or position which a character can use to enter into an animation.\n" +
					"Includes workbenches, instruments, or enemy ambush positions.",
				Signature.HAZD => "Area of space which can harm the player (fire/radiation).",
				Signature.IDLM => "Allows an NPC to enter an idle animation.",
				Signature.KEYM => string.Empty,
				Signature.LIGH => string.Empty,
				Signature.LVLI => "A lootable object or set of potential objects, occasionally spawning conditionally or with variable odds.",
				Signature.MISC => "Junk, Scrap or Mod.",
				Signature.MSTT => "Environmental scenery which animates or responds to physics.",
				Signature.NOTE => string.Empty,
				Signature.NPC_ => "Non-player character.",
				Signature.PROJ => "An 'armed' weapon such as a mine.",
				Signature.REGN => "Defined subsections of the map.",
				Signature.SCOL => "A grouped set of static objects.",
				Signature.SECH => "Trigger for echo sound effect.",
				Signature.SOUN => "Trigger for sound effect.",
				Signature.STAT => "Environmental scenery which does not move and cannot be interacted with.",
				Signature.TACT => "A trigger/activator which causes a voice line to be played.",
				Signature.TERM => string.Empty,
				Signature.TXST => "A decal applied to a surface such as paint or dirt.",
				Signature.WEAP => string.Empty,
				Signature.WRLD => "The main outside world.",
				_ => throw new ArgumentException($"Unexpected signature {signature}"),
			};
		}

		// Returns if the Signature is typically something a user would intend to search for, and thus is recommended to be selected
		public static bool IsRecommendedSelection(this Signature signature)
		{
			return signature switch
			{
				Signature.ACTI or
				Signature.ALCH or
				Signature.AMMO or
				Signature.ARMO or
				Signature.BOOK or
				Signature.CNCY or
				Signature.CONT or
				Signature.FLOR or
				Signature.FURN or
				Signature.KEYM or
				Signature.LVLI or
				Signature.MISC or
				Signature.NOTE or
				Signature.NPC_ or
				Signature.PROJ or
				Signature.REGN or
				Signature.TERM or
				Signature.WEAP => true,

				_ => false
			};
		}

		public static bool IsLockable(this Signature signature)
		{
			return signature switch
			{
				Signature.CONT or
				Signature.DOOR or
				Signature.TERM => true,

				_ => false,
			};
		}
	}
}
