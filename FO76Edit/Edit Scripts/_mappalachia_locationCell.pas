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
		worldspacesEntry = ElementByName(item, 'Master Worldspace Cells');
	var
		i, j : Integer;
		worldspaceEntry, worldspace, cellsEntry, cell : IInterface;
	begin
		for i:= 0 to elementCount(worldspacesEntry) - 1 do begin
			worldspaceEntry: = ElementByIndex(worldspacesEntry, i);
			worldspace: = sanitize(GetEditValue(ElementByName(worldspaceEntry, 'World')));
			cellsEntry: = ElementByName(worldspaceEntry, 'Cells');

			for j:= 0 to elementCount(cellsEntry) - 1 do begin
				cell := ElementByIndex(cellsEntry, j);
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
	end;
end.
