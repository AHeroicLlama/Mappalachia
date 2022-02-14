SELECT component, SUM(magnitude), spaceFormId, spaceEditorId, spaceDisplayName, isWorldspace FROM Scrap_Search
INNER JOIN Space_Info ON Scrap_Search.spaceFormId = Space_Info.spaceFormId
WHERE component = $scrap AND Scrap_Search.spaceFormId = $spaceFormId