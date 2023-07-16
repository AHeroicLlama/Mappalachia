SELECT regionFormID, regionEditorId, spaceEditorId, spaceDisplayName
FROM Region
INNER JOIN Space_Info ON Region.spaceFormID = Space_Info.spaceFormID
WHERE (regionEditorId LIKE $searchTerm ESCAPE '\' OR regionFormID LIKE $searchTerm ESCAPE '\') AND Region.spaceFormID = $spaceFormID
GROUP BY regionFormID, spaceEditorID
ORDER BY regionEditorId ASC
