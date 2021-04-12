SELECT component, SUM(magnitude) AS total_scrap, cellDisplayName, cellEditorID, '0' AS isInterior
FROM
(
	SELECT referenceFormID, component, componentQuantity*COUNT(*) AS magnitude, 'Appalachia' AS cellDisplayName, 'Appalachia' AS cellEditorID
	FROM SeventySix_Worldspace
	INNER JOIN SeventySix_Quantified_Scrap ON junkFormID = referenceFormID
	WHERE component = $searchTerm
	GROUP BY referenceFormID
)

UNION

SELECT component, SUM(magnitude) AS total_scrap, cellDisplayName, cellEditorID, '1' AS isInterior
FROM
(
	SELECT referenceFormID, component, componentQuantity*COUNT(*) AS magnitude, cellDisplayName, cellEditorID
	FROM SeventySix_Interior
	INNER JOIN SeventySix_Quantified_Scrap ON junkFormID = referenceFormID
	INNER JOIN SeventySix_Cell ON SeventySix_Cell.cellFormID = SeventySix_Interior.cellFormID
	WHERE component = $searchTerm
	GROUP BY referenceFormID, cellDisplayName
)
GROUP BY cellEditorID
ORDER BY isInterior, total_scrap DESC
