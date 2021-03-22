//Rip every item placed within the Appalachia Worldspace. Gets coordinates, FormID, name(Inc FormID of referenced object), and information on lock levels and primitive boundaries where relevant
unit _m_worldspace;

	uses _m_lib;

	var	outputStrings : TStringList;
	const targetWorldspace = $0025DA15; //FormID of the Worldspace 'Appalachia'.

	procedure Initialize;
	begin
		ripWorldspace(0); //0=SeventySix.esm
	end;

	procedure ripWorldspace(fileNum : Integer); //Primary block for iterating down worldspace tree
	const
		targetESM = FileByIndex(fileNum);
		worldspace = RecordByFormID(targetESM, targetWorldspace, False);
		blocks = ChildGroup(worldspace);
		blockCount = ElementCount(blocks);
		outputFile = ProgramPath + 'Output\' + StringReplace(BaseName(targetESM), '.esm', '', [rfReplaceAll]) + '_Worldspace.csv';
	var
		i, j, k, l : Integer; //iterators
		block, subBlock, cell, cellItem : IInterface;
	begin
		outputStrings := TStringList.Create;
		outputStrings.add('referenceFormID,x,y,locationFormID,lockLevel,primitiveShape,boundX,boundY,rotZ'); //Write CSV column headers

		for i := 0 to blockCount - 1 do begin //Iterate over all blocks within the worldspace
			block := elementByIndex(blocks, i);
			AddMessage(BaseName(targetESM) + ' : ' + EditorID(worldspace) + ' : ' + BaseName(block));

			for j := 0 to ElementCount(block) - 1 do begin //Iterate over all subBlocks within the block
				subBlock := elementByIndex(block, j);

				if(Signature(subBlock) = 'GRUP') then begin
					for k := 0 to ElementCount(subBlock) - 1 do begin //Iterate over all cells within the subBlock
						cell := FindChildGroup(ChildGroup(ElementByIndex(subBlock, k)), 9, ElementByIndex(subBlock, k));

						if(groupType(cell) = 9) then begin //Check that this isn't the persistent worldspace cell (which has a hierarchy one-less deep than normal worldspace blocks)
							for l := 0 to ElementCount(cell) - 1 do begin //Iterate over all cellElements within the cell
								cellItem := elementByIndex(cell, l);
								ripItem(cellItem);
							end;
						end
						else begin //This is not a standard worldspace block (presumably the persistent worldspace cell)
							cell := elementByIndex(subBlock, k);
							ripItem(cell);
						end;

					end;
				end;

			end;
		end;
		AddMessage('Writing output to file: ' + outputFile);
		createDir('Output');
		outputStrings.SaveToFile(outputFile);
	end;

	procedure ripItem(item : IInterface);
	const
		position = GetPosition(item);
		displayName = DisplayName(item);
	var
		primitiveEntry, boundsEntry : IInterface;
		primitiveShape, rotZ : String;
	begin
		if(shouldProcessRecord(sigFromRef(displayName)) and Assigned(displayName) and Assigned(position)) then begin //Skip records we don't care to map or aren't useful
			primitiveEntry := ElementBySignature(item, 'XPRM');
			boundsEntry := ElementByName(primitiveEntry, 'Bounds');
			primitiveShape := GetEditValue(ElementByName(primitiveEntry, 'Type'));

			//We only need the Z Rotation for primitive shapes
			if(primitiveShape <> '') then begin
				rotZ := FloatToStr(GetRotation(item).z);
			end;

			outputStrings.Add(
				sanitize(displayName) + ',' +
				FloatToStr(position.x) + ',' +
				FloatToStr(position.y) + ',' +
				sanitize(GetEditValue(ElementBySignature(item, 'XLCN'))) + ',' +
				GetEditValue(ElementByName(ElementBySignature(item, 'XLOC'), 'Level')) + ',' +
				primitiveShape + ',' +
				GetEditValue(ElementByName(boundsEntry, 'X')) + ',' +
				GetEditValue(ElementByName(boundsEntry, 'Y')) + ',' +
				rotZ
			);
		end;
	end;
end.
