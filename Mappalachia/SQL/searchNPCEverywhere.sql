SELECT NPC, MIN(chance) as chance, COUNT(*) as amount, spaceEditorId, spaceDisplayName
FROM NPC_Search
INNER JOIN Space_Info ON NPC_Search.spaceFormId = Space_Info.spaceFormID
WHERE NPC = $npc and chance >= $chance
GROUP BY spaceEditorId
ORDER BY isWorldspace DESC, amount DESC