SELECT x, y, z, chance*100 as chance
FROM NPC_Search WHERE NPC = $npc and chance >= $chance and spaceFormId = $spaceFormID
ORDER BY z ASC
