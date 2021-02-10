SELECT lockLevel
FROM SeventySix_Worldspace
UNION
SELECT lockLevel
FROM SeventySix_Interior
GROUP BY lockLevel
