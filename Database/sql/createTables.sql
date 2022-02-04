-- Create new tables to be imported into
CREATE TABLE NPC_Spawn(npc TEXT, class TEXT, locationFormID TEXT, chance REAL);
CREATE TABLE Quantified_Scrap(component TEXT, componentQuantity INTEGER, junkFormID TEXT);
CREATE TABLE Entity_Info(entityFormID TEXT, displayName TEXT, editorID TEXT, signature TEXT);
CREATE TABLE Space_Info(spaceFormID TEXT, spaceEditorID TEXT, spaceDisplayName TEXT);
CREATE TABLE Position_Data(spaceFormID TEXT, referenceFormID TEXT, x INTEGER, y INTEGER, z INTEGER, locationFormID TEXT, lockLevel TEXT, primitiveShape TEXT, boundX INTEGER, boundY INTEGER, boundZ INTEGER, rotZ INTEGER, spawnClass TEXT);

