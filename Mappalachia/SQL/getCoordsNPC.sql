SELECT x, y, z, chance*100 AS chance FROM SeventySix_Worldspace
INNER JOIN SeventySix_NPCSpawn ON class = spawnClass AND SeventySix_Worldspace.locationFormID = SeventySix_NPCSpawn.locationFormID
WHERE npc = $npc AND chance >= $minChance
