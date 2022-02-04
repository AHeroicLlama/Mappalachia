SELECT npc, COUNT(*) AS count, MIN(chance)*100 AS minChance, spaceDisplayName, spaceEditorID
FROM Position_Data
INNER JOIN NPC_Spawn ON class = spawnClass AND Position_Data.locationFormID = NPC_Spawn.locationFormID
INNER JOIN Space_Info ON Space_Info.spaceFormID = Position_Data.spaceFormID
WHERE NPC = $searchTerm AND Chance >= $minChance
GROUP BY spaceEditorID
ORDER BY CASE WHEN spaceEditorID = 'Appalachia' THEN 1 ELSE 0 END DESC, count DESC
