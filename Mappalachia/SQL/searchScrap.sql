SELECT * FROM
(
	SELECT component, SUM(magnitude) AS totalScrap, spaceDisplayName, spaceEditorID, spaceFormID
	FROM
	(
		SELECT referenceFormID, component, componentQuantity*COUNT(*) AS magnitude, spaceDisplayName, spaceEditorID, Space_Info.spaceFormID, isWorldspace
		FROM Position_Data
		INNER JOIN Quantified_Scrap ON junkFormID = referenceFormID
		INNER JOIN Space_Info ON Space_Info.spaceFormID = Position_Data.spaceFormID
		WHERE component = $searchTerm
		GROUP BY referenceFormID, spaceDisplayName
	)
	GROUP BY spaceEditorID
	ORDER BY isWorldspace DESC, totalScrap DESC
)
WHERE spaceFormID = $spaceFormID