SELECT component, SUM(magnitude) AS total_scrap, cellDisplayName, cellEditorID
FROM
(
	SELECT referenceFormID, component, componentQuantity*COUNT(*) AS magnitude, 'Appalachia' AS cellDisplayName, 'Appalachia' AS cellEditorID
	FROM SeventySix_Worldspace
	INNER JOIN SeventySix_Quantified_Scrap ON junkFormID = referenceFormID
	WHERE component = $searchTerm
	GROUP BY referenceFormID
)
GROUP BY cellDisplayName
ORDER BY total_scrap DESC