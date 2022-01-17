// Gets the FormID, EditorID, and DisplayName of every CELL
unit _m_cell;

	uses _m_lib;

	var	outputStrings, skippedCells : TStringList;

	procedure Initialize;
	begin
		ripCells(0); //0=SeventySix.esm
	end;

	procedure ripCells(fileNum : Integer); // Primary block for iterating down tree
	const
		targetESM = FileByIndex(fileNum);
		categoryCount = ElementCount(targetESM);
		outputFile = ProgramPath + 'Output\' + StringReplace(BaseName(targetESM), '.esm', '', [rfReplaceAll]) + '_Cell.csv';
		skippedCellsFile = StringReplace(outputFile, '_Cell', '_SkippedCells', [rfReplaceAll]);
	var
		i, j, k, l : Integer; // iterators
		category, block, subBlock, cell : IInterface;
	begin
		skippedCells := TStringList.Create;
		outputStrings := TStringList.Create;
		skippedCells.add('cellFormID,cellEditorID,cellDisplayName'); // Write CSV column headers
		outputStrings.add('cellFormID,cellEditorID,cellDisplayName');

		category := GroupBySignature(targetESM, 'CELL');
		for j := 0 to ElementCount(category) -1 do begin // Iterate over every block within the Cell category
			block := elementByIndex(category, j);

			for k := 0 to ElementCount(block) -1 do begin // Iterate over every subBlock within the block
				subBlock := elementByIndex(block, k);

				for l := 0 to ElementCount(subBlock) -1 do begin // Iterate over every cell within the subBlock
					cell := elementByIndex(subBlock, l);

					if(FixedFormId(cell) <> 0) then begin
						ripItem(cell);
					end;
				end;
			end;
		end;

		createDir('Output');
		AddMessage('Writing output to file: ' + outputFile);
		outputStrings.SaveToFile(outputFile);

		AddMessage('Writing skipped cells to file: ' + skippedCellsFile);
		skippedCells.SaveToFile(skippedCellsFile);
	end;

	procedure ripItem(cell : IInterface);
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
