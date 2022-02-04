SELECT x, y, z, chance*100 AS chance FROM Position_Data
INNER JOIN NPC_Spawn ON class = spawnClass AND Position_Data.locationFormID = NPC_Spawn.locationFormID
WHERE spaceFormID = $spaceFormID AND npc = $npc AND chance >= $minChance
ORDER BY z ASC
