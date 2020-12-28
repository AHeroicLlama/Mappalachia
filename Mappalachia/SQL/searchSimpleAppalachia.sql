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
			(EditorId LIKE $searchTerm ESCAPE '\' OR
			displayName LIKE $searchTerm ESCAPE '\' OR
			entityFormID LIKE $searchTerm ESCAPE '\')
			AND signature IN ($allowedSignatures)
			AND lockLevel IN ($allowedLockTypes)
		)
	GROUP BY entityFormID
)
ORDER BY amount DESC