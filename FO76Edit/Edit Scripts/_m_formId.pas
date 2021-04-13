// Rip every single entry in the ESM which is relevant for mapping as defined by _m_lib.shouldProcessRecord(). Gets each item's FormID, EdID and displayName.
// This is cross referenced by the Preprocessor between the location data to assign names/EditorID's to FormIDs in the location data
unit _m_formID;

	uses _m_lib;

	var outputStrings : TStringList;

	procedure Initialize;
	begin
		ripFormIDs(0); //0=SeventySix.esm
	end;

	procedure ripFormIDs(fileNum : Integer); // Primary block for iterating down tree
	const
		targetESM = FileByIndex(fileNum);
		outputFile = ProgramPath + 'Output\' + StringReplace(BaseName(targetESM), '.esm', '', [rfReplaceAll]) + '_FormID.csv';
	var
		i, j : Integer; // iterators
		signatureGroup : IInterface;
		signature : String;
	begin
		outputStrings := TStringList.Create;
		outputStrings.add('entityFormID,displayName,editorID,signature'); // Write CSV column headers

		// Rip everything down to the end 'leaves' of the hierarchy tree
		for i := 0 to ElementCount(targetESM) - 1 do begin
			signatureGroup := elementByIndex(targetESM, i);
			signature := StringReplace(BaseName(signatureGroup), 'GRUP Top ', '', [rfReplaceAll]);
			signature := StringReplace(signature, '"', '', [rfReplaceAll]); // Strip the category to its 4-char identifier

			for j := 0 to ElementCount(signatureGroup) -1 do begin
				ripItem(elementByIndex(signatureGroup, j), signature);
			end;
		end;

		AddMessage('Writing output to file: ' + outputFile);
		createDir('Output');
		outputStrings.SaveToFile(outputFile);
	end;

	procedure ripItem(item : IInterface; signature : String);
	const
		editorId = EditorID(item);
		displayName = DisplayName(item);
	var
		i : Integer;
		bestDisplayName : String;
	begin
		if(FixedFormId(item) = 0) then begin // This is a GRUP and not an end-node, so pass each of its children back through
			for i := 0 to ElementCount(item) -1 do begin
				ripItem(elementByIndex(item, i), signature); // Recursive
			end;
		end
		else begin // This is an end-node
			if (signature = 'LVLI') then begin
				bestDisplayName := getNameforLvli(item);
			end
			else begin
				bestDisplayName := displayName;
			end;

			outputStrings.Add(
				IntToHex(FixedFormId(item), 8) + ',' +
				sanitize(bestDisplayName) + ',' +
				sanitize(editorID) + ',' +
				signature
			);
		end;
	end;
end.
