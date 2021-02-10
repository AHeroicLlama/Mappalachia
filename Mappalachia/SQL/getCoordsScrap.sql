SELECT x, y, componentQuantity FROM SeventySix_Worldspace
INNER JOIN SeventySix_Quantified_Scrap ON SeventySix_Worldspace.referenceFormID = SeventySix_Quantified_Scrap.junkFormID
WHERE component = $scrap
