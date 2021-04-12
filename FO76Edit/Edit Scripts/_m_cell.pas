// Gets the FormID, EditorID, and DisplayName of every CELL
unit _m_cell;

	uses _m_lib;

	var	outputStrings : TStringList;

	procedure Initialize;
	begin
		ripCells(0); //0=SeventySix.esm
	end;

	procedure ripCells(fileNum : Integer); // Primary block for iterating down tree
	const
		targetESM = FileByIndex(fileNum);
		categoryCount = ElementCount(targetESM);
		outputFile = ProgramPath + 'Output\' + StringReplace(BaseName(targetESM), '.esm', '', [rfReplaceAll]) + '_Cell.csv';
	var
		i, j, k, l : Integer; // iterators
		category, block, subBlock, cell : IInterface;
	begin
		outputStrings := TStringList.Create;
		outputStrings.add('cellFormID,cellEditorID,cellDisplayName'); // Write CSV column headers

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

		AddMessage('Writing output to file: ' + outputFile);
		createDir('Output');
		outputStrings.SaveToFile(outputFile);
	end;

	procedure ripItem(cell : IInterface);
	var
		cellFormID, cellEditorID, cellDisplayName : IInterface;
	begin
		cellEditorID := sanitize(EditorID(cell));
		cellDisplayName := sanitize(DisplayName(cell));

		if (shouldProcessCellID(cellEditorID) and shouldProcessCellName(cellDisplayName)) then begin // Only rip this CELL if it's not some QA/Debug cell
			cellFormID := IntToHex(FixedFormId(cell), 8);

			outputStrings.Add(
				cellFormID + ',' +
				cellEditorID + ',' +
				sanitize(cellDisplayName)
			);
		end;
	end;
end.
