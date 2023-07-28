// Gets the FormID, EditorID, and DisplayName of every relevant CELL and WRLD
unit _commonwealthCartography_spaceInfo;

	uses _commonwealthCartography_lib;

	var	outputStrings, skippedspaces : TStringList;

	procedure Initialize;
	const
		outputFile = ProgramPath + 'Output\Space_Info_' + IntToStr(esmNumber) + '.csv';
		skippedspacesFile = ProgramPath + 'Output\Skipped_spaces_' + IntToStr(esmNumber) + '.csv';
	begin
		skippedSpaces := TStringList.Create;
		outputStrings := TStringList.Create;

		// Write CSV column headers
		skippedSpaces.add('spaceFormID,spaceEditorID,spaceDisplayName,isWorldspace,esmNumber');
		outputStrings.add('spaceFormID,spaceEditorID,spaceDisplayName,isWorldspace,esmNumber');

		ripWorldSpaces();
		ripCells();

		if (outputStrings.Count > 1) then begin
			createDir('Output');
			AddMessage('Writing output to file: ' + outputFile);
			outputStrings.SaveToFile(outputFile);
		end;

		if (skippedspaces.Count > 1) then begin
			createDir('Output');
			AddMessage('Writing skipped spaces to file: ' + skippedSpacesFile);
			skippedSpaces.SaveToFile(skippedSpacesFile);
		end;
	end;

	procedure ripWorldspaces();
	var
		i : Integer; // Iterator
		category, worldspace : IInterface;
	begin
		category := GroupBySignature(targetESM, 'WRLD');
		for i := 0 to ElementCount(category) -1 do begin // Iterate over every worldspace within the worldspace category
			worldspace := elementByIndex(category, i);
			if(FixedFormId(worldspace) <> 0) then begin
				ripSpace(worldspace, 1);
			end;

		end;
	end;

	procedure ripCells(); // Primary block for iterating down tree
	var
		i, j, k, l : Integer; // Iterators
		category, block, subBlock, cell : IInterface;
	begin
		category := GroupBySignature(targetESM, 'CELL');
		for j := 0 to ElementCount(category) -1 do begin // Iterate over every block within the Cell category
			block := elementByIndex(category, j);

			for k := 0 to ElementCount(block) -1 do begin // Iterate over every subBlock within the block
				subBlock := elementByIndex(block, k);

				for l := 0 to ElementCount(subBlock) -1 do begin // Iterate over every cell within the subBlock
					cell := elementByIndex(subBlock, l);

					if(FixedFormId(cell) <> 0) then begin
						ripSpace(cell, 0);
					end;
				end;
			end;
		end;
	end;

	procedure ripSpace(space : IInterface; isWorldspace : Integer);
	var
		spaceEditorID, spaceDisplayName : IInterface;
		entry : String;
	begin
		spaceEditorID := EditorID(space);
		spaceDisplayName := sanitize(DisplayName(space));

		entry := IntToHex(FixedFormId(space), 8) + ',' + spaceEditorID + ',' + spaceDisplayName + ',' + intToStr(isWorldspace) + ',' + intToStr(esmNumber);

		if (shouldProcessSpace(spaceDisplayName, spaceEditorID)) then begin // Put valid in-game spacess in the right file, otherwise storing debug spaces elsewhere for the record
			outputStrings.Add(entry);
		end
		else begin
			skippedspaces.Add(entry);
		end;
	end;
end.
