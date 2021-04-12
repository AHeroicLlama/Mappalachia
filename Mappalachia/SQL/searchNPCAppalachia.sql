SELECT npc, COUNT(*) AS count, MIN(chance)*100 AS minChance, 'Appalachia' AS cellDisplayName, 'Appalachia' AS cellEditorID
FROM SeventySix_Worldspace
INNER JOIN SeventySix_NPCSpawn ON class = spawnClass AND SeventySix_Worldspace.locationFormID = SeventySix_NPCSpawn.locationFormID
WHERE NPC = $searchTerm AND Chance >= $minChance
GROUP BY cellEditorID
ORDER BY COUNT DESC
