SELECT spaceFormID, spaceEditorID, spaceDisplayName
FROM Space_Info
ORDER BY CASE WHEN spaceDisplayName = 'Appalachia' THEN 1 ELSE 0 END DESC, spaceDisplayName
