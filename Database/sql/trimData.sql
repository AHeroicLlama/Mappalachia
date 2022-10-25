-- Delete entities which are not placed in the game world
DELETE FROM Entity_Info WHERE (
	entityFormID NOT IN (SELECT referenceFormID FROM Position_Data)
);

DELETE FROM Space_Info WHERE (
	spaceFormID NOT IN (SELECT spaceFormID FROM Position_Data)
);

DELETE FROM Region WHERE (
	spaceFormID NOT IN (SELECT spaceFormID FROM Position_Data)
);

DELETE FROM NPC_Search WHERE chance = 0;
