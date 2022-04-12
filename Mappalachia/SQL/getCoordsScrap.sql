SELECT x, y, z, magnitude FROM Scrap_Search
WHERE component = $scrap AND Scrap_Search.spaceFormID = $spaceFormID
ORDER BY z ASC
