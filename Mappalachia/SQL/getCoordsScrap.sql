SELECT x, y, z, componentQuantity FROM Position_Data
INNER JOIN Quantified_Scrap ON Position_Data.referenceFormID = Quantified_Scrap.junkFormID
WHERE spaceFormID = $spaceFormID AND component = $scrap
ORDER BY z ASC
