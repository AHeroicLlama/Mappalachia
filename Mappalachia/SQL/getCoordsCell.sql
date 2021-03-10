SELECT x, y, z, primitiveShape, boundX, boundY, rotZ
FROM SeventySix_Interior
WHERE cellFormID = $cellFormID AND referenceFormID = $formID AND lockLevel IN ($allowedLockTypes)
