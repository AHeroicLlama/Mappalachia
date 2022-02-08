SELECT displayName, editorId, signature, internalCount, lockLevel, entityFormID, spaceDisplayName, spaceEditorID, spaceFormID, isWorldspace FROM
(
	SELECT spaceFormId as internalSpaceFormId, referenceFormID, lockLevel, count(*) as internalCount FROM Position_Data WHERE
	(
		spaceFormID = '0025DA15'
		AND lockLevel IN ('','Novice (Level 0)','Advanced (Level 1)','Expert (Level 2)','Master (Level 3)','Requires Terminal','Requires Key','Chained','Opens Door','Inaccessible','Unknown')
	)
	GROUP BY referenceFormID, spaceFormID
)

INNER JOIN

(
	SELECT * FROM Entity_Info WHERE
	(
		(EditorId LIKE '%a%' ESCAPE '\' OR
		displayName LIKE '%a%' ESCAPE '\' OR
		entityFormID LIKE '%a%' ESCAPE '\') AND
		signature IN ('STAT','LVLI','FLOR','NPC_','MISC','BOOK','ALCH','CONT','FURN','NOTE','TERM','ARMO','WEAP','AMMO','PROJ','CNCY','KEYM','ACTI')
	)
)
ON referenceFormID = entityFormID

INNER JOIN Space_Info ON spaceFormID = internalSpaceFormId

ORDER BY isWorldspace DESC, internalCount DESC