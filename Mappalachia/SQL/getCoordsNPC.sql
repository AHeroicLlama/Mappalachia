SELECT x, y, z, chance as chance
FROM NPC_Search WHERE NPC = $npc and chance = $chance and spaceFormID = $spaceFormID
ORDER BY z ASC
