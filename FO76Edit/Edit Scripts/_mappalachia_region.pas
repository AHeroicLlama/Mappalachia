// Gets the region coordinates of relevant REGNs
unit _mappalachia_region;

	uses _mappalachia_lib;

	var	outputStrings : TStringList;

	procedure Initialize;
	begin
		processRecordGroup('REGN', 'Region', 'spaceFormID,regionFormID,regionEditorID,regionNum,coordNum,x,y');
	end;

	procedure ripItem(item : IInterface);
	const
		REGNEntry = ElementByName(item, 'Region Areas');
		formID = IntToHex(FixedFormId(item), 8);
		editorID =  EditorID(item);
		spaceFormID = wrldFormIdFromRef(GetEditValue(ElementBySignature(item, 'WNAM')));
	var
		i, p : Integer;
		currentRegionArea, regionAreaPoints, currentPoint : IInterface;
	begin
		if (spaceFormID <> '') then begin // Skip regions not assigned to a world
			for i:= 0 to ElementCount(REGNEntry) - 1 do begin
				currentRegionArea := ElementByName(REGNEntry, 'Region Area #' + IntToStr(i));
				regionAreaPoints := ElementBySignature(currentRegionArea, 'RPLD');
				for p:= 0 to ElementCount(regionAreaPoints) - 1 do begin
					currentPoint := ElementByName(regionAreaPoints, 'Point #' + IntToStr(p));
					outputStrings.Add(
						spaceFormID + ',' +
						formID + ',' +
						editorID + ',' +
						IntToStr(i) + ',' + // region #
						IntToStr(p) + ',' + // point #
						GetEditValue(ElementByName(currentPoint, 'X')) + ',' +
						GetEditValue(ElementByName(currentPoint, 'Y'))
					);
				end;
			end;
		end;
	end;
end.
