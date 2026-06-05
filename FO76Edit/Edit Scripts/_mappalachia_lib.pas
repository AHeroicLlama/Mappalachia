// Mappalachia supporting functions - not to be run directly.
unit _mappalachia_lib;

	var
		targetESM : IInterface;
		esmNumber : Integer;
		fileName : string;

	// Remove commas and replace them with something safe for CSV
	function sanitize(input: String): String;
	begin
		input := StringReplace(input, ',', ':COMMA:', [rfReplaceAll]);
		input := StringReplace(input, '"', ':QUOT:', [rfReplaceAll]);
		input := StringReplace(input, #13, ':CR:', [rfReplaceAll]);
		input := StringReplace(input, #10, ':LF:', [rfReplaceAll]);
		Result := StringReplace(input, '''', '''''', [rfReplaceAll]);
	end;

	procedure outputToFile(esmNumber: Integer; outputFile: String; strings: TStringList);
	begin
		createDir('Output\');
		createDir('Output\' + IntToStr(esmNumber));
		AddMessage('Writing output to file: ' + outputFile);
		outputStrings.SaveToFile(outputFile);
		outputStrings.Free;
	end;

	// Handle the pulling of every record from a given signature group and write it to the output file.
	// Calling script must provide its own ripItem method for handling each actual entry.
	// See: goToRipItem()
	function processRecordGroup(signature, outputFileName: String): Integer;
	var
		i : Integer;
		outputFile : string;
		category : IInterface;
	begin
		for i := 0 to FileCount() -1 do begin
			esmNumber := i;
			targetESM := FileByIndex(esmNumber);
			fileName := GetFileName(targetESM);

			if (pos('.esm', fileName) = 0) then begin
				AddMessage('Skipping ' + fileName + ' - not an ESM');
				continue
			end;

			AddMessage('Running ' + outputFileName + ' export on ' + fileName);

			outputFile := ProgramPath + 'Output\' + IntToStr(esmNumber) + '\' + outputFileName + '.csv';
			category := GroupBySignature(targetESM, signature);

			outputStrings := TStringList.Create;

			for i := 0 to ElementCount(category) -1 do begin // Iterate over every item within the category
				goToRipItem(elementByIndex(category, i), outputFileName);
			end;

			outputToFile(esmNumber, outputFile, outputStrings);
		end;
	end;

	// The ripItem method exists for many units.
	// We need to correctly target the right version of the method.
	procedure goToRipItem(item: IInterface; filenameKey: String);
	begin
			if(filenameKey = 'Scrap') then _mappalachia_scrap.ripItem(item)
		else if(filenameKey = 'Location') then _mappalachia_location.ripItem(item)
		else if(filenameKey = 'LocationCell') then _mappalachia_locationCell.ripItem(item)
		else if(filenameKey = 'Component') then _mappalachia_component.ripItem(item)
		else if(filenameKey = 'Region') then _mappalachia_region.ripItem(item)
	end;
end.
