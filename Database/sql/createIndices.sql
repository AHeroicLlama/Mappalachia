CREATE INDEX Index_ScrapSearch_CompMagSpace ON Scrap_Search (
	component,
	magnitude,
	spaceFormID
);

CREATE INDEX Index_StandardSearch_RefSpace ON Standard_Search (
	referenceFormId,
	spaceFormId
);

CREATE INDEX Index_PositionData_SpaceXYZ ON Position_Data (
	spaceFormID,
	x,
	y,
	z
);

CREATE INDEX Index_PositionData_CoverAll ON Position_Data (
	spaceFormID,
	referenceFormID,
	x,
	y,
	z,
	lockLevel,
	primitiveShape,
	boundX,
	boundY,
	boundZ,
	rotZ,
	label
);
