--Provide a report on key data statistics. The outputted report is source controlled and allows us to verify at a glance how game updates affect the Mappalachia database
SELECT '==Total unique items by signature==';
SELECT signature, COUNT(signature) AS count FROM SeventySix_FormID
GROUP BY signature;

SELECT '==Average display name string length==';
SELECT AVG(length) FROM
(
    SELECT LENGTH(displayName) AS length from SeventySix_FormID
);

SELECT '==Average editorID string length==';
SELECT AVG(length) FROM
(
    SELECT LENGTH(editorID) AS length from SeventySix_FormID
);

SELECT '==Worldspace average X Coord==';
SELECT AVG(x) FROM SeventySix_Worldspace;

SELECT '==Worldspace average Y Coord==';
SELECT AVG(y) FROM SeventySix_Worldspace;

SELECT '==Worldspace average X Bounds Width==';
SELECT AVG(boundX) FROM SeventySix_Worldspace;

SELECT '==Worldspace average Y Bounds Width==';
SELECT AVG(boundY) FROM SeventySix_Worldspace;

SELECT '==Worldspace average Z Rotation==';
SELECT AVG(rotZ) FROM SeventySix_Worldspace;

SELECT '==Worldspace primitive shape types + count==';
SELECT primitiveShape, COUNT(*) FROM SeventySix_Worldspace GROUP BY primitiveShape;

SELECT '==Worldspace total placement count==';
SELECT COUNT(referenceFormID) FROM SeventySix_Worldspace;

SELECT '==Interior average X Coord==';
SELECT AVG(x) FROM SeventySix_Interior;

SELECT '==Interior average Y Coord==';
SELECT AVG(y) FROM SeventySix_Interior;

SELECT '==Interior average Z Coord==';
SELECT AVG(z) FROM SeventySix_Interior;

SELECT '==Interior average X Bounds Width==';
SELECT AVG(boundX) FROM SeventySix_Interior;

SELECT '==Interior average Y Bounds Width==';
SELECT AVG(boundY) FROM SeventySix_Interior;

SELECT '==Interior average Z Rotation==';
SELECT AVG(rotZ) FROM SeventySix_Interior;

SELECT '==Interior total placement count==';
SELECT COUNT(referenceFormID) FROM SeventySix_Interior;

SELECT '==Worldspace unique placement count==';
SELECT COUNT(DISTINCT referenceFormID) FROM SeventySix_Worldspace;

SELECT '==Interior unique placement count==';
SELECT COUNT(DISTINCT referenceFormID) FROM SeventySix_Interior;

SELECT '==Average interior placement count==';
SELECT AVG(count) FROM
(
    SELECT COUNT(referenceFormID) as count FROM SeventySix_Interior
    GROUP BY cellFormID
);

SELECT '==Worldspace lock levels + count==';
SELECT  lockLevel, COUNT(*) FROM SeventySix_Worldspace GROUP BY lockLevel;

SELECT '==Interior lock levels + count==';
SELECT  lockLevel, COUNT(*) FROM SeventySix_Interior GROUP BY lockLevel;

SELECT '==Worldspace variable spawn entities + count==';
SELECT  spawnClass, COUNT(*) FROM SeventySix_Worldspace GROUP BY spawnClass;

SELECT '==Interior variable spawn entities + count==';
SELECT  spawnClass, COUNT(*) FROM SeventySix_Interior GROUP BY spawnClass;

SELECT '==Worldspace total entity-with-locationFormID count==';
SELECT  COUNT(*) FROM SeventySix_Worldspace WHERE locationFormID <> '';

SELECT '==Interior total entity-with-locationFormID count==';
SELECT  COUNT(*) FROM SeventySix_Interior WHERE locationFormID <> '';

SELECT '==Worldspace unique locationFormID count==';
SELECT  COUNT(*) FROM
(
    SELECT * FROM SeventySix_Worldspace GROUP BY locationFormID
);

SELECT '==Interior unique locationFormID count==';
SELECT  COUNT(*) FROM
(
    SELECT * FROM SeventySix_Interior GROUP BY locationFormID
);

SELECT '==Junk-by-scrap count==';
SELECT component, COUNT(*) FROM SeventySix_Quantified_Scrap
GROUP BY component;

SELECT '==Count of unique junk items==';
SELECT COUNT(*) FROM
(
    SELECT * FROM SeventySix_Quantified_Scrap GROUP BY junkFormID
);

SELECT '==Average NPC Spawns per location FormID==';
SELECT AVG(count) FROM
(
    SELECT COUNT(*) as count FROM SeventySix_NPCSpawn
    GROUP BY locationFormID
);

SELECT '==NPC per-class avg spawn chance + count==';
SELECT npc, class, AVG(chance), COUNT(*) FROM SeventySix_NPCSpawn
GROUP BY npc, class;

SELECT '==Internal cells list + entity count==';
SELECT cellEditorID, cellDisplayName, count(*) FROM SeventySix_Interior
INNER JOIN SeventySix_Cell ON SeventySix_Cell.cellFormID = SeventySix_Interior.cellFormID
GROUP BY SeventySix_Interior.cellFormID
ORDER BY cellEditorID;
