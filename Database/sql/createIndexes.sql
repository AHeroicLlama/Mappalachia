CREATE INDEX indexPositionData_CoverAll ON Position_Data (
    spaceFormID,
    referenceFormID,
    x,
    y,
    z,
    locationFormID,
    lockLevel,
    primitiveShape,
    boundX,
    boundY,
    boundZ,
    rotZ,
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
	spaceDisplayName,
	isWorldspace
);
