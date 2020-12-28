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
		'Appalachia' AS cellEditorID,
		'0025DA15' AS cellFormID
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
		cellEditorID,
		cellFormID
	FROM SeventySix_FormId
	INNER JOIN SeventySix_Interior ON referenceFormID = entityFormID
	WHERE
		(
			(EditorId LIKE $searchTerm ESCAPE '\' OR
			displayName LIKE $searchTerm ESCAPE '\' OR
			entityFormID LIKE $searchTerm ESCAPE '\')
			AND signature IN ($allowedSignatures)
			AND lockLevel IN ($allowedLockTypes)
		)
	GROUP BY entityFormID, cellFormID
)
ORDER BY CASE WHEN cellDisplayName = 'Appalachia' THEN 1 ELSE 0 END DESC, amount DESC