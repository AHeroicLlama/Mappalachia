﻿namespace Mappalachia
{
	class SettingsSearch
	{
		public static bool showFormID = false;
		public static bool searchInAllSpaces = false;
		public static int spawnChance = 33; // The minimum spawn chance percentage to filter for
		public static readonly int spawnChanceMin = 0; // This is a percentage so always 0-100
		public static readonly int spawnChanceMax = 100;
	}
}
