SELECT regionFormID, regionEditorId, spaceEditorId, spaceDisplayName
FROM Region
INNER JOIN Space_Info ON Region.spaceFormID = Space_Info.spaceFormID
WHERE regionEditorId LIKE $searchTerm ESCAPE '\' OR regionFormID LIKE $searchTerm ESCAPE '\'
GROUP BY regionFormID, spaceEditorID
ORDER BY Region.spaceFormID = $spaceFormID DESC, regionEditorId ASC
