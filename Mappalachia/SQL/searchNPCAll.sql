SELECT npc, COUNT(*) AS count, MIN(chance)*100 AS minChance, 'Appalachia' AS cellDisplayName, 'Appalachia' AS cellEditorID, '0' AS isInterior
FROM SeventySix_Worldspace
INNER JOIN SeventySix_NPCSpawn ON class = spawnClass AND SeventySix_Worldspace.locationFormID = SeventySix_NPCSpawn.locationFormID
WHERE NPC = $searchTerm AND Chance >= $minChance

UNION

SELECT npc, COUNT(*) AS count, MIN(chance)*100 AS minChance, cellDisplayName, cellEditorID, '1' AS isInterior
FROM SeventySix_Interior
INNER JOIN SeventySix_NPCSpawn ON class = spawnClass AND SeventySix_Interior.locationFormID = SeventySix_NPCSpawn.locationFormID
WHERE NPC = $searchTerm AND Chance >= $minChance

GROUP BY cellDisplayName
ORDER BY isInterior, COUNT DESC