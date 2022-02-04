// Gets the FormID, EditorID, and DisplayName of every relevant CELL and WRLD
unit _mappalachia_spaceInfo;

	uses _mappalachia_lib;

	var	outputStrings, skippedCells : TStringList;
	const
		appalachiaWorldspaceID = '0025DA15';

	procedure Initialize;
	const
		outputFile = ProgramPath + 'Output\Space_Info.csv';
		skippedCellsFile = ProgramPath + 'Output\Skipped_Cells.csv';
	begin
		skippedCells := TStringList.Create;
		outputStrings := TStringList.Create;

		// Write CSV column headers
		skippedCells.add('cellFormID,cellEditorID,cellDisplayName');
		outputStrings.add('spaceFormID,spaceEditorID,spaceDisplayName');

		ripWorld(appalachiaWorldspaceID);
		ripCells();

		createDir('Output');
		AddMessage('Writing output to file: ' + outputFile);
		outputStrings.SaveToFile(outputFile);

		AddMessage('Writing skipped cells to file: ' + skippedCellsFile);
		skippedCells.SaveToFile(skippedCellsFile);
	end;

	procedure ripCells(); // Primary block for iterating down tree
	const
		targetESM = FileByIndex(0);
		categoryCount = ElementCount(targetESM);

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
						ripCell(cell);
					end;
				end;
			end;
		end;
	end;

	procedure ripWorld(worldspaceID : Integer);
	const
		worldspace = RecordByFormID(FileByIndex(0), worldspaceID, False);
	begin
		outputStrings.Add(
			IntToHex(FixedFormId(worldspace), 8) + ',' +
			sanitize(EditorID(worldspace)) + ',' +
			sanitize(DisplayName(worldspace)));
	end;

	procedure ripCell(cell : IInterface);
	var
		cellFormID, cellEditorID, cellDisplayName : IInterface;
	begin
		cellFormID := IntToHex(FixedFormId(cell), 8);
		cellEditorID := sanitize(EditorID(cell));
		cellDisplayName := sanitize(DisplayName(cell));

		if (shouldProcessCell(cellDisplayName, cellEditorID)) then begin // Put valid in-game CELLs in the right file, otherwise storing debug cells elsewhere for the record
			outputStrings.Add(
				cellFormID + ',' +
				cellEditorID + ',' +
				sanitize(cellDisplayName)
			);
		end
		else begin
			skippedCells.Add(
				cellFormID + ',' +
				cellEditorID + ',' +
				sanitize(cellDisplayName)
			);
		end;
	end;
end.
