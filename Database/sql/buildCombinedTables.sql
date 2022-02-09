-- Create effectively a copy of position_data without coordinates, and joined to editorid and display names, to be used to easily search from
CREATE TABLE 'Standard_Search' AS
SELECT referenceFormId, editorID, displayName, signature as category, lockLevel, amount, pos_space as spaceFormId FROM

(SELECT referenceFormId, spaceFormId as pos_space, lockLevel, locationFormId, count(*) as amount FROM Position_Data GROUP BY pos_space, referenceFormId, lockLevel)

INNER JOIN
(SELECT entityFormID, displayName, editorID, signature FROM Entity_Info)
ON referenceFormID = entityFormID

INNER JOIN
(SELECT spaceFormId as space_space, spaceEditorId, spaceDisplayName, isWorldspace FROM Space_Info)
ON pos_space = space_space