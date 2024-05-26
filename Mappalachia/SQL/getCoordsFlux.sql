SELECT x, y, z FROM Position_Data
INNER JOIN Entity_Info ON Position_Data.referenceFormID = Entity_Info.entityFormID
WHERE fluxColor = $fluxColor and spaceFormID = $spaceFormID
