// Gets a list of all LCTN and their cells
// Header 'locationFormID,locationEditorId,locationDisplayName,space,cellX,cellY,infestation'
unit _mappalachia_locationCell;

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
		keywordsEntry = ElementBySignature(ElementByName(item, 'Keywords'), 'KWDA');
	var
		i, j, k, infestation : Integer;
		worldspaceEntry, worldspace, cellsEntry, cell, keyword : IInterface;
	begin
		for k:= 0 to elementCount(keywordsEntry) - 1 do begin
			keyword: = ElementByIndex(keywordsEntry, k);

			if (pos('LocTypeHostileTakeover', GetEditValue(keyword)) <> 0) then begin
				infestation := 1;
				break;
			end;
		end;

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
					IntToStr(GetEditValue(ElementByName(cell, 'Grid Y'))) + ',' +
					IntToStr(infestation)
				);
			end;
		end;
	end;
end.
