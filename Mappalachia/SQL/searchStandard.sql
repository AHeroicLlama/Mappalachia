SELECT * FROM

(SELECT referenceFormId, spaceFormId, lockLevel, count(*) as amount FROM Position_Data WHERE
    spaceFormID = $spaceFormID
    AND lockLevel IN ($allowedLockTypes)
    GROUP BY spaceFormId, referenceFormId)

INNER JOIN

(SELECT entityFormID, displayName, editorID FROM Entity_Info WHERE
    (EditorId LIKE $searchTerm ESCAPE '\' OR displayName LIKE $searchTerm ESCAPE '\' OR entityFormID LIKE $searchTerm ESCAPE '\')
    AND signature IN ($allowedSignatures))

ON referenceFormID = entityFormID

INNER JOIN

(SELECT spaceEditorId, spaceDisplayName, isWorldspace FROM Space_Info WHERE spaceFormID = $spaceFormID)
