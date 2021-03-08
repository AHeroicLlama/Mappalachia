UPDATE SeventySix_FormId SET displayName = replace(displayName, ':COMMA:', ','); --Comma char was replaced by :COMMA: to keep both CSV and auto-generated SQL valid
UPDATE SeventySix_FormId SET displayName = replace(displayName, '\"', '"'); --Quotation char needed escaping for the initial import to work, but can now be replaced
--There are 6 (') because each (') is escaped as ('') and each string is then wrapped in ('')
UPDATE SeventySix_FormId SET displayName = replace(displayName, '''''', ''''); --Single quotes needed escaping as 2x singles for the initial import to work, but can now be replaced

UPDATE SeventySix_Cell SET cellDisplayName = replace(cellDisplayName, ':COMMA:', ',');
UPDATE SeventySix_Cell SET cellDisplayName = replace(cellDisplayName, '\"', '"');
UPDATE SeventySix_Cell SET cellDisplayName = replace(cellDisplayName, '''''', '''');
