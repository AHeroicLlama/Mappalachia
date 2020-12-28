--Provide a report on key data statistics. The outputted report is source controlled and allows us to verify at a glance how game updates affect the Mappalachia database
SELECT '==Worldspace total placement count==';
SELECT COUNT(referenceFormID) FROM SeventySix_WorldSpace;

SELECT '==Worldspace unique placement count==';
SELECT COUNT(DISTINCT referenceFormID) FROM SeventySix_Worldspace;

SELECT '==Interior total placement count==';
SELECT COUNT(referenceFormID) FROM SeventySix_Interior;

SELECT '==Interior unique placement count==';
SELECT COUNT(DISTINCT referenceFormID) FROM SeventySix_Interior;

SELECT '==Average interior placement count==';
SELECT AVG(count) FROM
(
    SELECT COUNT(referenceFormID) as count FROM SeventySix_Interior
    GROUP BY cellFormID
);

SELECT '==Average X Coord==';
SELECT AVG(x) FROM SeventySix_Worldspace;

SELECT '==Average Y Coord==';
SELECT AVG(y) FROM SeventySix_Worldspace;

SELECT '==Average X Bounds Width==';
SELECT AVG(boundX) FROM SeventySix_Worldspace;

SELECT '==Average Y Bounds Width==';
SELECT AVG(boundY) FROM SeventySix_Worldspace;

SELECT '==Total unique items by signature==';
SELECT signature, COUNT(signature) as count
FROM SeventySix_FormID
GROUP BY signature;

SELECT '==Unique scrap-containing junk count==';
SELECT component, COUNT(*)
FROM SeventySix_Quantified_Scrap
GROUP BY component;

SELECT '==NPC per-class count==';
SELECT npc, class, COUNT(*)
FROM SeventySix_NPCSpawn
GROUP BY npc, class;

SELECT '==Internal cells list==';
SELECT cellEditorID
FROM SeventySix_Interior
GROUP BY cellEditorID;

SELECT '==Internal cells count==';
SELECT COUNT(DISTINCT cellFormID) FROM  SeventySix_Interior;