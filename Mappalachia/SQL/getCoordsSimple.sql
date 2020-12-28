SELECT x, y, primitiveShape, boundX, boundY
FROM SeventySix_Worldspace
WHERE referenceFormID = $formID AND lockLevel IN ($allowedLockTypes)