CREATE INDEX indexPositionData ON Position_Data (
	z,
	spaceFormID,
	referenceFormID,
	locationFormID,
	lockLevel,
	spawnClass
);

CREATE INDEX indexPositionData_referenceFormID ON Position_Data (
	referenceFormID
);

CREATE INDEX indexPositionData_lockLevel ON Position_Data (
	lockLevel
);

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
	spaceDisplayName
);
