SELECT x, y, z, primitiveShape, boundX, boundY, boundZ, rotZ
FROM SeventySix_Worldspace
WHERE referenceFormID = $formID AND lockLevel IN ($allowedLockTypes)
