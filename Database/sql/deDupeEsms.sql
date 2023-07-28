-- Filters near-duplicate rows where only the esmNumber varies, keeping the highest (most overriding) ESM

CREATE TABLE temp AS SELECT spaceFormID,
	spaceEditorID,
	spaceDisplayName,
	isWorldspace,
	nudgeX,
	nudgeY,
	nudgeScale,
	max(esmNumber) as esmNumber
FROM Space_Info
GROUP BY spaceFormID,
	spaceEditorID,
	spaceDisplayName,
	isWorldspace,
	nudgeX,
	nudgeY,
	nudgeScale;
DROP TABLE Space_Info;
ALTER TABLE temp RENAME TO Space_Info;


CREATE TABLE temp AS SELECT spaceFormID,
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
	mapMarkerName,
	label,
	max(esmNumber) as esmNumber,
	instanceFormID
  FROM Position_Data
  GROUP BY
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
	mapMarkerName,
	label,
	instanceFormID;
DROP TABLE Position_Data;
ALTER TABLE temp RENAME TO Position_Data;


CREATE TABLE temp AS SELECT spaceFormID,
	label,
	mapMarkerName,
	x,
	y,
	max(esmNumber) as esmNumber
FROM Map_Markers
GROUP BY
	spaceFormID,
	label,
	mapMarkerName,
	x,
	y;
DROP TABLE Map_Markers;
ALTER TABLE temp RENAME TO Map_Markers;

CREATE TABLE temp AS SELECT DISTINCT * FROM Entity_Info;
DROP TABLE Entity_Info;
ALTER TABLE temp RENAME TO Entity_Info;
