SELECT component, SUM(magnitude) AS totalScrap, spaceDisplayName, spaceEditorID
FROM
(
	SELECT referenceFormID, component, componentQuantity*COUNT(*) AS magnitude, spaceDisplayName, spaceEditorID
	FROM Position_Data
	INNER JOIN Quantified_Scrap ON junkFormID = referenceFormID
	INNER JOIN Space_Info ON Space_Info.spaceFormID = Position_Data.spaceFormID
	WHERE component = $searchTerm
	GROUP BY referenceFormID, spaceDisplayName
)
GROUP BY spaceEditorID
ORDER BY CASE WHEN spaceEditorID = 'Appalachia' THEN 1 ELSE 0 END DESC, totalScrap DESC
