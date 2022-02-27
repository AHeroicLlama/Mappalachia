-- Drops tables or columns which we no longer need (As their data has been fully incorporated into newly combined tables)
DROP TABLE NPC_Spawn;

DROP TABLE Quantified_Scrap;

-- Dropping the columns locationFormID and spawnClass. SQLite does not support dropping columns so we recreate the table as new without them
-- Remember that the indexes must be built after this occurs.
CREATE TABLE temp AS SELECT spaceFormID, referenceFormID, x, y, z, lockLevel, primitiveShape, boundX, boundY, boundZ, rotZ FROM Position_Data;
DROP TABLE Position_Data;
ALTER TABLE temp RENAME TO Position_Data;
