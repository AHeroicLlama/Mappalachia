// Gets the FormID, EditorID, and DisplayName of every relevant CELL and WRLD
// Header 'spaceFormID,spaceEditorID,spaceDisplayName,isWorldspace,isInstanceable'
unit _mappalachia_space;

	uses _mappalachia_lib;

	var	outputStrings : TStringList;

	procedure Initialize;
	const
		outputFile = ProgramPath + 'Output\Space.csv';
	begin
		outputStrings := TStringList.Create;

		ripWorldSpaces();
		ripCells();

		createDir('Output');
		AddMessage('Writing output to file: ' + outputFile);
		outputStrings.SaveToFile(outputFile);
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
		entry, isInstanceable : String;
	begin
		spaceEditorID := EditorID(space);
		spaceDisplayName := sanitize(DisplayName(space));
		isInstanceable := GetEditValue(ElementByName(ElementByName(ElementByName(space, 'Record Header'), 'Record Flags'), 'Is Instancable'));

		entry := IntToStr(FixedFormId(space)) + ',' + spaceEditorID + ',' + spaceDisplayName + ',' + intToStr(isWorldspace) + ',' + isInstanceable;
		outputStrings.Add(entry);
	end;
end.
