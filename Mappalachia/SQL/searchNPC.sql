SELECT NPC, spaceFormID, chance, COUNT(*), spaceDisplayName, spaceEditorId, isWorldspace FROM NPC_Search 
INNER JOIN Space_Info ON NPC_Search.spaceFormId = Space_Info.spaceFormID
WHERE NPC = $npc and chance >= $chance and NPC_Search.spaceFormId = $spaceFormID
