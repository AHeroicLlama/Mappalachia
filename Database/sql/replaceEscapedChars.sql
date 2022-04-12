UPDATE Entity_Info SET displayName = replace(displayName, ':COMMA:', ','); -- Comma char was replaced by :COMMA: to keep both CSV and auto-generated SQL valid
UPDATE Entity_Info SET displayName = replace(displayName, '\"', '"'); -- Quotation char needed escaping for the initial import to work, but can now be replaced
-- There are 6 (') because each (') is escaped as ('') and each string is then wrapped in ('')
UPDATE Entity_Info SET displayName = replace(displayName, '''''', ''''); -- Single quotes needed escaping as 2x singles for the initial import to work, but can now be replaced

UPDATE Space_Info SET spaceDisplayName = replace(spaceDisplayName, ':COMMA:', ',');
UPDATE Space_Info SET spaceDisplayName = replace(spaceDisplayName, '\"', '"');
UPDATE Space_Info SET spaceDisplayName = replace(spaceDisplayName, '''''', '''');

UPDATE Map_Markers SET label = replace(label, ':COMMA:', ',');
UPDATE Map_Markers SET label = replace(label, '\"', '"');
UPDATE Map_Markers SET label = replace(label, '''''', '''');
