SELECT component, SUM(magnitude), COUNT(*), spaceEditorId, spaceDisplayName
FROM Scrap_Search
INNER JOIN Space_Info ON Scrap_Search.spaceFormID = Space_Info.spaceFormID
WHERE component = $scrap AND Scrap_Search.spaceFormID = $spaceFormID
