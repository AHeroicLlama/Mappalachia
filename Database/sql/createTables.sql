--Create new tables to be imported into
CREATE TABLE SeventySix_FormID(entityFormID TEXT, displayName TEXT, editorID TEXT, signature TEXT);
CREATE TABLE SeventySix_Interior(referenceFormID TEXT, cellFormID TEXT, x INTEGER, y INTEGER, z INTEGER, locationFormID TEXT, lockLevel TEXT, primitiveShape TEXT, boundX INTEGER, boundY INTEGER, boundZ INTEGER, rotZ INTEGER, spawnClass TEXT);
CREATE TABLE SeventySix_NPCSpawn(npc TEXT, class TEXT, locationFormID TEXT, chance REAL);
CREATE TABLE SeventySix_Quantified_Scrap(component TEXT, componentQuantity TEXT, junkFormID TEXT);
CREATE TABLE SeventySix_Worldspace(referenceFormID TEXT, x INTEGER, y INTEGER, locationFormID TEXT, lockLevel TEXT, primitiveShape TEXT, boundX INTEGER, boundY INTEGER, rotZ INTEGER, spawnClass TEXT);
CREATE TABLE SeventySix_Cell(cellFormID TEXT, cellEditorID TEXT, cellDisplayName TEXT);
