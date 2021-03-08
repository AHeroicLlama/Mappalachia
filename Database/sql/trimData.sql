--Delete entities which are not placed in the game world
DELETE FROM SeventySix_FormId WHERE (
	entityFormID NOT IN (SELECT referenceFormID FROM SeventySix_Worldspace) AND
	entityFormID NOT IN (SELECT referenceFormID FROM SeventySix_Interior)
);

DELETE FROM SeventySix_Quantified_Scrap WHERE (
	junkFormID NOT IN (SELECT referenceFormID FROM SeventySix_Worldspace) AND
	junkFormID NOT IN (SELECT referenceFormID FROM SeventySix_Interior)
);

DELETE FROM SeventySix_NPCSpawn WHERE (
	locationFormID NOT IN (SELECT locationFormID FROM SeventySix_Worldspace) AND
	locationFormID NOT IN (SELECT locationFormID FROM SeventySix_Interior)
);

DELETE FROM SeventySix_Cell WHERE (
	cellFormID NOT IN (SELECT cellFormID FROM SeventySix_Interior)
);
