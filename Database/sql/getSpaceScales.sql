-- Calculate and add 6 new columns (X/Y/Z Center and X/Y/Z Range) to the Space_Info table
CREATE TABLE temp AS

SELECT A.spaceFormID, spaceEditorID, spaceDisplayName, isWorldspace, xCenter, YCenter, xMin, xMax, yMin, yMax, nudgeX, nudgeY, nudgeScale FROM Space_Info as A
INNER JOIN
(SELECT spaceFormID, ((max(x) + min(x)) / 2) as xCenter, ((max(y) + min(y)) / 2) as yCenter, min(x) as xMin, max(x) as xMax, min(y) as yMin, max(y) as yMax FROM Position_Data GROUP BY spaceFormID) as B
ON A.spaceFormID = B.spaceFormID;

DROP TABLE Space_Info;
ALTER TABLE temp RENAME TO Space_Info;