SELECT NPC, MIN(chance) as chance, COUNT(*), spaceEditorId, spaceDisplayName
FROM NPC_Search
INNER JOIN Space_Info ON NPC_Search.spaceFormId = Space_Info.spaceFormID
WHERE NPC = $npc and NPC_Search.spaceFormId = $spaceFormID
GROUP BY chance
ORDER BY chance DESC
