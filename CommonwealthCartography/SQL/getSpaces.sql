SELECT spaceFormID, spaceEditorID, spaceDisplayName, isWorldspace, xCenter, yCenter, xMin, xMax, yMin, yMax, nudgeX, nudgeY, nudgeScale
FROM Space_Info
ORDER BY CASE WHEN spaceEditorID = 'Commonwealth' THEN 1 ELSE 0 END DESC, isWorldspace DESC, spaceDisplayName
