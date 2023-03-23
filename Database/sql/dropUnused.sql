-- Drops tables or columns which we no longer need (As their data has been fully incorporated into newly combined tables)
DROP TABLE NPC_Spawn;

DROP TABLE Quantified_Scrap;

-- SQLite does not support dropping columns so we recreate the tables as new without the dropped columns
-- Indexes must be (re)built after this occurs.

-- Dropping the columns locationFormID, spawnClass and mapMarkerName.
CREATE TABLE temp AS SELECT spaceFormID, referenceFormID, x, y, z, lockLevel, primitiveShape, boundX, boundY, boundZ, rotZ, label, instanceFormID FROM Position_Data;
DROP TABLE Position_Data;
ALTER TABLE temp RENAME TO Position_Data;

-- Dropping the column signature.
CREATE TABLE temp AS SELECT entityFormID, displayName, editorID, signature FROM Entity_Info;
DROP TABLE Entity_Info;
ALTER TABLE temp RENAME TO Entity_Info;
