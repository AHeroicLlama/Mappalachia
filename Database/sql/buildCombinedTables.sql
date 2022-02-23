-- Create effectively a copy of position_data without coordinates, and joined to editorid and display names, to be used to easily search from
CREATE TABLE 'Standard_Search' AS
SELECT referenceFormId, signature as category, lockLevel, amount, pos_space as spaceFormId FROM

(SELECT referenceFormId, spaceFormId as pos_space, lockLevel, locationFormId, count(*) as amount FROM Position_Data GROUP BY pos_space, referenceFormId, lockLevel)

INNER JOIN
(SELECT entityFormID, signature FROM Entity_Info)
ON referenceFormID = entityFormID

INNER JOIN
(SELECT spaceFormId as space_space FROM Space_Info)
ON pos_space = space_space;

-- Create a single fast table entirely for all NPC search AND plot usage.
CREATE TABLE 'NPC_Search' AS
SELECT npc, chance, x, y, z, spaceFormId
FROM Position_Data
INNER JOIN NPC_Spawn ON class = spawnClass AND Position_Data.locationFormId = NPC_Spawn.locationFormId;

-- TODO drop NPC_Spawn, drop locationFormID, spawnclass from position_data

CREATE TABLE 'Scrap_Search' AS
SELECT component, componentQuantity AS magnitude, x, y, z, spaceFormId
FROM Position_Data
INNER JOIN Quantified_Scrap ON junkFormID = referenceFormID;

-- TODO drop component_quantity
