SELECT spaceFormID, spaceEditorID, spaceDisplayName, isWorldspace, xCenter, yCenter, xMin, xMax, yMin, yMax, nudgeX, nudgeY, nudgeScale
FROM Space_Info
ORDER BY
CASE WHEN spaceEditorID = 'Commonwealth' THEN 1 ELSE 0 END DESC,
CASE WHEN spaceEditorID = 'DLC03FarHarbor' THEN 1 ELSE 0 END DESC,
CASE WHEN spaceEditorID = 'NukaWorld' THEN 1 ELSE 0 END DESC,
spaceDisplayName
