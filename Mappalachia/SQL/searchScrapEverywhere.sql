SELECT component, SUM(magnitude) as amount, COUNT(*), spaceEditorId, spaceDisplayName
FROM Scrap_Search
INNER JOIN Space_Info ON Scrap_Search.spaceFormID = Space_Info.spaceFormID
WHERE component = $scrap
GROUP BY spaceEditorId
ORDER BY Scrap_Search.spaceFormID = $spaceFormID DESC, amount DESC
