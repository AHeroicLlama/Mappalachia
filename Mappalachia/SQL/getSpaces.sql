SELECT spaceFormID, spaceEditorID, spaceDisplayName, isWorldspace, xCenter, yCenter, xMin, xMax, yMin, yMax, nudgeX, nudgeY, nudgeScale
FROM Space_Info
ORDER BY isWorldspace DESC, spaceDisplayName
