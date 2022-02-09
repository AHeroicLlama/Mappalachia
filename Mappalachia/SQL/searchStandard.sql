SELECT referenceFormId,
    editorID,
    displayName,
    category,
    lockLevel,
    amount
FROM Standard_Search
JOIN Space_Info ON Standard_Search.spaceFormId = Space_Info.spaceFormID
WHERE
(category IN ($allowedSignatures) AND
lockLevel IN ($allowedLockTypes) AND
(EditorId LIKE $searchTerm ESCAPE '\' OR displayName LIKE $searchTerm ESCAPE '\' OR referenceFormId LIKE $searchTerm ESCAPE '\')
AND Standard_Search.spaceFormId = $spaceFormID)

ORDER BY isWorldspace DESC, amount DESC