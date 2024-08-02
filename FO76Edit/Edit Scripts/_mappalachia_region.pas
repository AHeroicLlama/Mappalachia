// Gets the region coordinates of relevant REGNs
// Header 'spaceFormID,regionFormID,regionEditorID,regionNum,coordNum,x,y'
unit _mappalachia_region;

	uses _mappalachia_lib;

	var	outputStrings : TStringList;

	procedure Initialize;
	begin
		processRecordGroup('REGN', 'Region');
	end;

	procedure ripItem(item : IInterface);
	const
		REGNEntry = ElementByName(item, 'Region Areas');
		formID = IntToStr(FixedFormId(item));
		editorID =  EditorID(item);
		spaceFormID = sanitize(GetEditValue(ElementBySignature(item, 'WNAM')));
	var
		i, p : Integer;
		currentRegionArea, regionAreaPoints, currentPoint : IInterface;
	begin
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
end.
