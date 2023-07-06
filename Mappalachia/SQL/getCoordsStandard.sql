SELECT x, y, z, primitiveShape, boundX, boundY, boundZ, rotZ, instanceFormID
FROM Position_Data
WHERE spaceFormID = $spaceFormID AND referenceFormID = $formID AND lockLevel IN ($allowedLockTypes) AND label = $label
ORDER BY z ASC
