SELECT * FROM
(
	SELECT
		displayName,
		editorID,
		signature,
		COUNT(*) AS amount,
		lockLevel,
		entityFormID,
		'Appalachia' AS cellDisplayName,
		'Appalachia' AS cellEditorID
	FROM SeventySix_FormId
	INNER JOIN SeventySix_Worldspace ON referenceFormID = entityFormID
	WHERE
		(
			(EditorId LIKE $searchTerm ESCAPE '\' or
			displayName LIKE $searchTerm ESCAPE '\' or
			entityFormID LIKE $searchTerm ESCAPE '\')
			AND signature IN ($allowedSignatures)
			AND lockLevel IN ($allowedLockTypes)
		)
	GROUP BY entityFormID

	UNION

	SELECT
		displayName,
		editorID,
		signature,
		count(*) AS amount,
		lockLevel,
		entityFormID,
		cellDisplayName,
		cellEditorID
	FROM SeventySix_FormId
	INNER JOIN SeventySix_Interior ON referenceFormID = entityFormID
	INNER JOIN SeventySix_Cell ON SeventySix_Cell.cellFormID = SeventySix_Interior.cellFormID
	WHERE
		(
			(EditorId LIKE $searchTerm ESCAPE '\' OR
			displayName LIKE $searchTerm ESCAPE '\' OR
			entityFormID LIKE $searchTerm ESCAPE '\')
			AND signature IN ($allowedSignatures)
			AND lockLevel IN ($allowedLockTypes)
		)
	GROUP BY entityFormID, SeventySix_Cell.cellFormID
)
ORDER BY CASE WHEN cellDisplayName = 'Appalachia' THEN 1 ELSE 0 END DESC, amount DESC
