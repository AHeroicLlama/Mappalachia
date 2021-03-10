//Rip every item placed within a cell. Gets parent cell FormID, EditorID and DisplayName, and FormID and name(Inc FormID of referenced object) of placed object reference
//This does not rip spatial data since we don't map interior cells anyway. It is used simply to show what a cell may contain.
unit _m_interior;

	uses _m_lib;

	var	outputStrings : TStringList;

	procedure Initialize;
	begin
		ripInteriors(0); //0=SeventySix.esm
	end;

	procedure ripInteriors(fileNum : Integer); //Primary block for iterating down tree
	const
		targetESM = FileByIndex(fileNum);
		categoryCount = ElementCount(targetESM);
		outputFile = ProgramPath + 'Output\' + StringReplace(BaseName(targetESM), '.esm', '', [rfReplaceAll]) + '_Interior.csv';
	var
		i, j, k, l, m : Integer; //iterators
		category, block, subBlock, cell, cellChild : IInterface;
		cellFormID, cellEditorID, cellDisplayName : String;
	begin
		outputStrings := TStringList.Create;
		outputStrings.add('referenceFormID,cellFormID,x,y,z,locationFormID,lockLevel,primitiveShape,boundX,boundY,rotZ');//Write CSV column headers

		category := GroupBySignature(targetESM, 'CELL');
		for j := 0 to ElementCount(category) -1 do begin //Iterate over every block within the Cell category
			block := elementByIndex(category, j);
			AddMessage(BaseName(targetESM) + ' : ' + BaseName(block));

			for k := 0 to ElementCount(block) -1 do begin //Iterate over every subBlock within the block
				subBlock := elementByIndex(block, k);

				for l := 0 to ElementCount(subBlock) -1 do begin //Iterate over every cell within the subBlock
					cell := elementByIndex(subBlock, l);

					if(FixedFormId(cell) <> 0) then begin //Make sure we get actual cell entries and not other stuff like headers and GRUPs
						cellFormID := IntToHex(FixedFormId(cell), 8);
						cellEditorID := sanitize(EditorID(cell));
						cellDisplayName := sanitize(DisplayName(cell));
						if not(shouldProcessCellName(cellDisplayName)) or not(shouldProcessCellID(cellEditorID)) then continue; //Skip this CELL if it's some QA/Debug cell

						//Rip persistent items...
						cellChild := FindChildGroup(ChildGroup(ElementByIndex(subBlock, l)), 8, ElementByIndex(subBlock, l));
						for m := 0 to ElementCount(cellChild) - 1 do begin //Iterate over every item within the cell
							ripItem(elementByIndex(cellChild, m), cellFormID, cellEditorID, cellDisplayName);
						end;

						//Then do the same for temporary ones
						cellChild := FindChildGroup(ChildGroup(ElementByIndex(subBlock, l)), 9, ElementByIndex(subBlock, l));
						for m := 0 to ElementCount(cellChild) - 1 do begin //Iterate over every item within the cell
							ripItem(elementByIndex(cellChild, m), cellFormID, cellEditorID, cellDisplayName);
						end;

					end;
				end;
			end;
		end;

		AddMessage('Writing output to file: ' + outputFile);
		createDir('Output');
		outputStrings.SaveToFile(outputFile);
	end;

	procedure ripItem(item : IInterface; cellFormID, cellEditorID, cellDisplayName : String);
	const
		displayName = DisplayName(item);
		position = GetPosition(item);
	var
		primitiveEntry, boundsEntry : IInterface;
		primitiveShape, rotZ : String;
	begin
		if(Assigned(displayName) and shouldProcessRecord(sigFromRef(displayName))) then begin //Skip records we don't care to map or can't refer to
			primitiveEntry := ElementBySignature(item, 'XPRM');
			boundsEntry := ElementByName(primitiveEntry, 'Bounds');
			primitiveShape := GetEditValue(ElementByName(primitiveEntry, 'Type'));

			//We only need the Z Rotation for primitive shapes
			if(primitiveShape <> '') then begin
				rotZ := FloatToStr(GetRotation(item).z);
			end;

			outputStrings.Add(
				sanitize(displayName) + ',' +
				cellFormID + ',' +
				FloatToStr(position.x) + ',' +
				FloatToStr(position.y) + ',' +
				FloatToStr(position.z) + ',' +
				sanitize(GetEditValue(ElementBySignature(item, 'XLCN'))) + ',' +
				GetEditValue(ElementByName(ElementBySignature(item, 'XLOC'), 'Level')) + ',' +
				primitiveShape + ',' +
				GetEditValue(ElementByName(boundsEntry, 'X')) + ',' +
				GetEditValue(ElementByName(boundsEntry, 'Y')) + ',' +
				rotZ);
		end;
	end;
end.
