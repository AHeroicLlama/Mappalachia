// Rip every single entry in the ESM which is relevant for mapping. Gets each item's FormID, EditorID and displayName.
// This is later cross referenced between the location data to assign names/EditorID's to FormIDs in the location data
unit _mappalachia_entityInfo;

	uses _mappalachia_lib;

	var outputStrings : TStringList;

	procedure Initialize;
	begin
		ripFormIDs();
	end;

	procedure ripFormIDs(); // Primary block for iterating down tree
	const
		outputFile = ProgramPath + 'Output\Entity_Info.csv';
	var
		i, j : Integer; // Iterators
		signatureGroup : IInterface;
		signature : String;
	begin
		outputStrings := TStringList.Create;
		outputStrings.add('entityFormID,displayName,editorID,signature,percChanceNone'); // Write CSV column headers

		// Rip everything down to the end 'leaves' of the hierarchy tree
		for i := 0 to ElementCount(targetESM) - 1 do begin
			signatureGroup := elementByIndex(targetESM, i);
			signature := StringReplace(BaseName(signatureGroup), 'GRUP Top ', '', [rfReplaceAll]);
			signature := StringReplace(signature, '"', '', [rfReplaceAll]); // Strip the category to its 4-char identifier

			// Only export relevant signatures
			if (not(shouldProcessSig(signature))) then continue;

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
				editorId + ',' +
				signature + ',' +
				GetEditValue(ElementBySignature(item, 'LVLD'))
			);
		end;
	end;
end.
