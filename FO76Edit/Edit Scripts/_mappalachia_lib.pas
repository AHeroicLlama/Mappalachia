// Mappalachia supporting functions - not to be run directly.
unit _mappalachia_lib;

	const targetESM = FileByIndex(0);

	// Remove commas and replace them with something safe for CSV
	function sanitize(input: String): String;
	begin
		input := StringReplace(input, ',', ':COMMA:', [rfReplaceAll]);
		input := StringReplace(input, '"', ':QUOT:', [rfReplaceAll]);
		input := StringReplace(input, #13#10, ':CRLF:', [rfReplaceAll]);
		Result := StringReplace(input, '''', '''''', [rfReplaceAll]);
	end;

	// Handle the pulling of every record from a given signature group and write it to the output file.
	// Calling script must provide its own ripItem method for handling each actual entry.
	// See: goToRipItem()
	function processRecordGroup(signature, fileName: String): Integer;
	const
		outputFile = ProgramPath + 'Output\' + fileName + '.csv';
		category = GroupBySignature(targetESM, signature);
	var
		i : Integer;
	begin
		outputStrings := TStringList.Create;

		for i := 0 to ElementCount(category) -1 do begin // Iterate over every item within the category
			goToRipItem(elementByIndex(category, i), signature);
		end;

		AddMessage('Writing output to file: ' + outputFile);
		createDir('Output');
		outputStrings.SaveToFile(outputFile);
		outputStrings.Free;
	end;

	// The ripItem method exists for many units.
	// We need to correctly target the right version of the method.
	procedure goToRipItem(item: IInterface; signature: String);
	begin
			if(signature = 'MISC') then _mappalachia_scrap.ripItem(item)
		else if(signature = 'LCTN') then _mappalachia_location.ripItem(item)
		else if(signature = 'CMPO') then _mappalachia_component.ripItem(item)
		else if(signature = 'REGN') then _mappalachia_region.ripItem(item)
	end;
end.
