-- Delete entities which are not placed in the game world
DELETE FROM Entity_Info WHERE (
	entityFormID NOT IN (SELECT referenceFormID FROM Position_Data)
);

DELETE FROM Quantified_Scrap WHERE (
	junkFormID NOT IN (SELECT referenceFormID FROM Position_Data)
);

DELETE FROM NPC_Spawn WHERE (
	locationFormID NOT IN (SELECT locationFormID FROM Position_Data)
);

DELETE FROM Space_Info WHERE (
	spaceFormID NOT IN (SELECT spaceFormID FROM Position_Data)
);
