-- Delete entities which are not placed in the game world
DELETE FROM Entity_Info WHERE (
	entityFormID NOT IN (SELECT referenceFormID FROM Position_Data)
);

DELETE FROM Space_Info WHERE (
	spaceFormID NOT IN (SELECT spaceFormID FROM Position_Data)
);

DELETE FROM Position_Data WHERE (
	referenceFormID NOT IN (SELECT entityFormID FROM Entity_Info)
);