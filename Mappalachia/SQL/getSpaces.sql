SELECT spaceFormID, spaceEditorID, spaceDisplayName, isWorldspace, xCenter, yCenter, xMin, xMax, yMin, yMax
FROM Space_Info
ORDER BY isWorldspace DESC, spaceDisplayName
