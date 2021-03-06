CREATE INDEX indexWorldspace ON SeventySix_Worldspace (
	referenceFormID,
	locationFormID,
	lockLevel,
	spawnClass
);

CREATE INDEX indexInterior ON SeventySix_Interior (
	referenceFormID,
	cellFormID,
	locationFormID,
	lockLevel,
	spawnClass
);

CREATE INDEX indexFormId ON SeventySix_FormId (
	entityFormID,
	displayName,
	editorID,
	signature
);

CREATE INDEX indexNPCSpawn ON SeventySix_NPCSpawn (
	npc,
	class,
	locationFormID,
	chance
);

CREATE INDEX indexQuantifiedScrap ON SeventySix_Quantified_Scrap (
	component,
	componentQuantity,
	junkFormID
);

CREATE INDEX indexCell ON SeventySix_Cell (
	cellFormID,
	cellEditorID,
	cellDisplayName
);
