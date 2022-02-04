-- Provide a report on key data statistics. The outputted report is source controlled and allows us to verify at a glance how game updates affect the Mappalachia database
SELECT '==Game Version==';
SELECT version FROM Game_Version;

SELECT '==Total unique items by signature==';
SELECT signature, COUNT(signature) AS count FROM Entity_Info
GROUP BY signature;

SELECT '==Average display name string length==';
SELECT AVG(length) FROM
(
    SELECT LENGTH(displayName) AS length from Entity_Info
);

SELECT '==Average editorID string length==';
SELECT AVG(length) FROM
(
    SELECT LENGTH(editorID) AS length from Entity_Info
);

SELECT '==Average X Coord==';
SELECT AVG(x) FROM Position_Data;

SELECT '==Average Y Coord==';
SELECT AVG(y) FROM Position_Data;

SELECT '==Average Z Coord==';
SELECT AVG(z) FROM Position_Data;

-- Assumes thresholds are -8000 and +45000. Verify these in SettingsPlotTopograph.zThreshUpper/Lower
SELECT '==Z outliers==';
SELECT COUNT(*) FROM Position_Data WHERE z < -8000 or z > 45000;

SELECT '==Average X Bounds Width==';
SELECT AVG(boundX) FROM Position_Data;

SELECT '==Average Y Bounds Width==';
SELECT AVG(boundY) FROM Position_Data;

SELECT '==Average Z Bounds Width==';
SELECT AVG(boundZ) FROM Position_Data;

SELECT '==Average Z Rotation==';
SELECT AVG(rotZ) FROM Position_Data;

SELECT '==Primitive shape types + count==';
SELECT primitiveShape, COUNT(*) FROM Position_Data GROUP BY primitiveShape;

SELECT '==Total entity placement count==';
SELECT COUNT(referenceFormID) FROM Position_Data;

SELECT '==Unique entity placement count==';
SELECT COUNT(DISTINCT referenceFormID) FROM Position_Data;

SELECT '==Lock levels + count==';
SELECT  lockLevel, COUNT(*) FROM Position_Data GROUP BY lockLevel;

SELECT '==Variable spawn entities + count==';
SELECT  spawnClass, COUNT(*) FROM Position_Data GROUP BY spawnClass;

SELECT '==Total entity-with-locationFormID count==';
SELECT  COUNT(*) FROM Position_Data WHERE locationFormID <> '';

SELECT '==Unique locationFormID count==';
SELECT  COUNT(*) FROM
(
    SELECT * FROM Position_Data GROUP BY locationFormID
);

SELECT '==Junk-by-scrap count==';
SELECT component, COUNT(*) FROM Quantified_Scrap
GROUP BY component;

SELECT '==Count of unique junk items==';
SELECT COUNT(*) FROM
(
    SELECT * FROM Quantified_Scrap GROUP BY junkFormID
);

SELECT '==Average NPC Spawns per location FormID==';
SELECT AVG(count) FROM
(
    SELECT COUNT(*) as count FROM NPC_Spawn
    GROUP BY locationFormID
);

SELECT '==NPC per-class avg spawn chance + count==';
SELECT npc, class, AVG(chance), COUNT(*) FROM NPC_Spawn
GROUP BY npc, class;

SELECT '==Spaces list + entity count==';
SELECT spaceEditorID, spaceDisplayName, count(*) FROM Position_Data
INNER JOIN Space_Info ON Space_Info.spaceFormID = Position_Data.spaceFormID
GROUP BY Position_Data.spaceFormID
ORDER BY spaceEditorID;
