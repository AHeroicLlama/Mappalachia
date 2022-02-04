CREATE INDEX indexPositionData ON Position_Data (
	referenceFormID,
	locationFormID,
	lockLevel,
	spawnClass
);

CREATE INDEX indexEntityInfo ON Entity_Info (
	entityFormID,
	displayName,
	editorID,
	signature
);

CREATE INDEX indexNPCSpawn ON NPC_Spawn (
	npc,
	class,
	locationFormID,
	chance
);

CREATE INDEX indexQuantifiedScrap ON Quantified_Scrap (
	component,
	componentQuantity,
	junkFormID
);

CREATE INDEX indexSpaceInfo ON Space_Info (
	spaceFormID,
	spaceEditorID,
	spaceDisplayName
);
