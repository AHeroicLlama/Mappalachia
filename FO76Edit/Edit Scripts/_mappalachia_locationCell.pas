// Gets a list of all LCTN and their cells
// Header 'locationFormID,locationEditorId,locationDisplayName,space,cellX,cellY'
unit _mappalachia_location;

	uses _mappalachia_lib;

	var	outputStrings : TStringList;

	procedure Initialize;
	begin
		processRecordGroup('LCTN', 'LocationCell');
	end;

	procedure ripItem(item : IInterface);
	const
		formID = IntToStr(FixedFormId(item));
		editorID = EditorID(item);
		displayName = DisplayName(item);
		worldspaceEntry = ElementBySignature(ElementByName(item, 'Master Worldspace Cells'), 'LCEC');
		worldspace = sanitize(GetEditValue(ElementByName(worldspaceEntry, 'World')));
		cellsEntry = ElementByName(worldspaceEntry, 'Cells');
	var
		i : Integer;
		cell : IInterface;
	begin
		for i:= 0 to elementCount(cellsEntry) - 1 do begin
			cell := ElementByIndex(cellsEntry, i);
			outputStrings.Add(
				formID + ',' +
				editorID + ',' +
				displayName + ',' +
				worldspace + ',' +
				IntToStr(GetEditValue(ElementByName(cell, 'Grid X'))) + ',' +
				IntToStr(GetEditValue(ElementByName(cell, 'Grid Y')))
			);
		end;
	end;
end.
