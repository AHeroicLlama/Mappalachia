-- Provide a report on key data statistics. The outputted report is source controlled and allows us to verify at a glance how game updates affect the Mappalachia database
SELECT '==Game Version==';
SELECT version FROM Game_Version;

SELECT '==Mean Average X/Y/Z Coord==';
SELECT AVG(x), AVG(y), AVG(z) FROM Position_Data;

SELECT '==Min/Max X/Y/Z Coord==';
SELECT Min(x), Max(x), Min(y), Max(y), Min(z), Max(z) FROM Position_Data;

SELECT '==100 most common xyz coordinates + count==';
SELECT x, y, z, COUNT(*) AS count FROM Position_Data
GROUP BY x, y, z
ORDER BY count DESC, x ASC, y ASC, z ASC
LIMIT 100;

-- Assumes thresholds are -8000 and +45000. Verify these in SettingsPlotTopograph.zThreshUpper/Lower
SELECT '==Z outliers==';
SELECT COUNT(*) FROM Position_Data WHERE z < -8000 OR z > 45000;

SELECT '==Average X Bounds Width==';
SELECT AVG(boundX) FROM Position_Data;

SELECT '==Average Y Bounds Width==';
SELECT AVG(boundY) FROM Position_Data;

SELECT '==Average Z Bounds Width==';
SELECT AVG(boundZ) FROM Position_Data;

SELECT '==Average Z Rotation==';
SELECT AVG(rotZ) FROM Position_Data;

SELECT '==Average display name string length==';
SELECT AVG(length) FROM
(
	SELECT LENGTH(displayName) AS length FROM Entity_Info
);

SELECT '==Average editorID string length==';
SELECT AVG(length) FROM
(
	SELECT LENGTH(editorID) AS length FROM Entity_Info
);

SELECT '==Map Markers==';
SELECT spaceEditorID, mapMarkerName, label, x, y FROM Map_Markers
INNER JOIN Space_Info ON Map_Markers.spaceFormID = Space_Info.spaceFormID
ORDER BY spaceEditorID ASC, mapMarkerName ASC, label ASC;

SELECT '==Total Unique entities==';
SELECT COUNT(DISTINCT referenceFormID) FROM Position_Data;

SELECT '==Regions==';
SELECT 'Space|Region|subRegions|maxVerts';
SELECT spaceEditorID, regionEditorID, max(regionNum) + 1 AS subRegions, max(coordNum) + 1 AS maxVerts FROM Region
INNER JOIN Space_Info ON Region.spaceFormID = Space_Info.spaceFormID
GROUP BY regionEditorID;

SELECT '==Total entities by PrimitiveShape by Space==';
SELECT spaceEditorId, primitiveShape, COUNT(*) FROM Position_Data
INNER JOIN Space_Info ON Position_Data.spaceFormID = Space_Info.spaceFormID
GROUP BY spaceEditorId, primitiveShape
ORDER BY spaceEditorId, primitiveShape;

SELECT '==Total entities by LockLevel by Space==';
SELECT spaceEditorId, lockLevel, SUM(amount) FROM Standard_Search
INNER JOIN Space_Info ON Standard_Search.spaceFormID = Space_Info.spaceFormID
GROUP BY spaceEditorId, lockLevel
ORDER BY spaceEditorId, lockLevel;

SELECT '==Total entities by Category by Space==';
SELECT spaceEditorId, category, SUM(amount) FROM Standard_Search
INNER JOIN Space_Info ON Standard_Search.spaceFormID = Space_Info.spaceFormID
GROUP BY spaceEditorId, category
ORDER BY spaceEditorId, category;

SELECT '==Total entities by Label by Space==';
SELECT spaceEditorId, label, SUM(amount) FROM Standard_Search
INNER JOIN Space_Info ON Standard_Search.spaceFormID = Space_Info.spaceFormID
GROUP BY spaceEditorId, label
ORDER BY spaceEditorId, label;

SELECT '==Total standard entities by spawnChance by Space==';
SELECT spaceEditorId, 100 - percChanceNone as chance, SUM(amount) FROM Standard_Search
INNER JOIN Space_Info ON Standard_Search.spaceFormID = Space_Info.spaceFormID
GROUP BY spaceEditorId, chance
ORDER BY spaceEditorId, chance;

SELECT '==Total Scrap and Junk per Component per Space==';
SELECT spaceEditorID, component, SUM(magnitude), COUNT(*) FROM Scrap_Search
INNER JOIN Space_Info ON Scrap_Search.spaceFormID = Space_Info.spaceFormID
GROUP BY Space_Info.spaceFormID, component
ORDER BY spaceEditorID, component;

SELECT '==Average (variable only) Spawn chance and Count per NPC per Space==';
SELECT Space_Info.spaceEditorID, NPC, ROUND(AVG(chance), 2), COUNT(*) FROM NPC_Search
INNER JOIN Space_Info ON Space_Info.spaceFormID = NPC_Search.spaceFormID
GROUP BY NPC_Search.spaceFormID, NPC
ORDER BY spaceEditorID;

SELECT '==Space Info==';
SELECT * FROM Space_Info;

SELECT '==Instance IDs Count Vs Distinct==';
SELECT COUNT(*), COUNT(DISTINCT(instanceFormID)) FROM Position_Data;
