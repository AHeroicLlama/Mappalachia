SELECT x, y, primitiveShape, boundX, boundY, rotZ
FROM SeventySix_Worldspace
WHERE referenceFormID = $formID AND lockLevel IN ($allowedLockTypes)
