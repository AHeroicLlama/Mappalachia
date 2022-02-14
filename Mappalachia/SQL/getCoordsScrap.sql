SELECT x, y, z, magnitude FROM Scrap_Search
WHERE component = $scrap AND Scrap_Search.spaceFormId = $spaceFormId
ORDER BY z ASC