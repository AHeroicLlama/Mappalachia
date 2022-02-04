SELECT
	displayName,
	editorID,
	signature,
	COUNT(*) AS amount,
	lockLevel,
	entityFormID,
	spaceDisplayName,
	spaceEditorID
FROM Entity_Info
INNER JOIN Position_Data ON referenceFormID = entityFormID
INNER JOIN Space_Info ON Space_Info.spaceFormID = Position_Data.spaceFormID
WHERE
	(
		(EditorId LIKE $searchTerm ESCAPE '\' OR
		displayName LIKE $searchTerm ESCAPE '\' OR
		entityFormID LIKE $searchTerm ESCAPE '\')
		AND signature IN ($allowedSignatures)
		AND lockLevel IN ($allowedLockTypes)
	)
GROUP BY entityFormID, Space_Info.spaceFormID

ORDER BY CASE WHEN spaceDisplayName = 'Appalachia' THEN 1 ELSE 0 END DESC, amount DESC
