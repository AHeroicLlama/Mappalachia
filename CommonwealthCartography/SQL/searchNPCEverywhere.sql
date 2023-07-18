SELECT NPC, MIN(chance) as chance, COUNT(*) as amount, spaceEditorId, spaceDisplayName
FROM NPC_Search
INNER JOIN Space_Info ON NPC_Search.spaceFormId = Space_Info.spaceFormID
WHERE NPC = $npc
GROUP BY spaceEditorId, chance
ORDER BY NPC_Search.spaceFormID = $spaceFormID DESC, chance DESC, amount DESC
