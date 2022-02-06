SELECT min(x), max(x), min(y), max(y), min(z), max(z)
FROM Position_Data
WHERE spaceFormID = $spaceFormID
