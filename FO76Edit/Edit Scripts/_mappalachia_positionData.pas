// Rip the location data of every placed entity inside valid cells and worldspaces. Gets FormID, coordinates, name(Inc FormID of referenced object), and information on lock levels and primitive boundaries where relevant
unit _mappalachia_positionData;

	uses _mappalachia_lib;

	var	outputStrings : TStringList;

	procedure Initialize;
	const
		outputFile = ProgramPath + 'Output\Position_Data.csv';
	begin
		outputStrings := TStringList.Create;
		outputStrings.add('spaceFormID,referenceFormID,x,y,z,locationFormID,lockLevel,primitiveShape,boundX,boundY,boundZ,rotZ,mapMarkerName,shortName'); // Write CSV column headers

		AddMessage('Beginning Mappalachia exterior position data export...');
		ripWorldspaces();
		AddMessage('Finished Mappalachia exterior position data export.');
		AddMessage('Beginning Mappalachia interior position data export...');
		ripInteriors();
		AddMessage('Finished Mappalachia interior position data export.');

		AddMessage('Writing position data output to file: ' + outputFile);
		createDir('Output');
		outputStrings.SaveToFile(outputFile);
	end;

	// Rips all interiors, uses shouldProcessSpace() to excluce debug cells
	procedure ripInteriors(); // Primary block for iterating down tree
	const
		categoryCount = ElementCount(targetESM);
	var
		i, j, k, l, m : Integer; // Iterators
		category, block, subBlock, cell, cellChild : IInterface;
		cellFormID, cellEditorID, cellDisplayName, friendlyBlockName : String;
	begin
		category := GroupBySignature(targetESM, 'CELL');
		for j := 0 to ElementCount(category) -1 do begin // Iterate over every block within the Cell category
			block := elementByIndex(category, j);
			friendlyBlockName := Trim(StringReplace(BaseName(block), 'GRUP', '', [rfReplaceAll]));

			for k := 0 to ElementCount(block) -1 do begin // Iterate over every subBlock within the block
				subBlock := elementByIndex(block, k);
				AddMessage(friendlyBlockName + ' : ' + Trim(StringReplace(BaseName(subBlock), 'GRUP', '', [rfReplaceAll])));

				for l := 0 to ElementCount(subBlock) -1 do begin // Iterate over every cell within the subBlock
					cell := elementByIndex(subBlock, l);

					if(FixedFormId(cell) <> 0) then begin // Make sure we get actual cell entries and not other stuff like headers and GRUPs
						cellFormID := IntToHex(FixedFormId(cell), 8);
						if not(shouldProcessSpace(DisplayName(cell), EditorID(cell))) then continue; // Skip this CELL if it's some QA/Debug cell

						// Rip persistent items...
						cellChild := FindChildGroup(ChildGroup(ElementByIndex(subBlock, l)), 8, ElementByIndex(subBlock, l));
						for m := 0 to ElementCount(cellChild) - 1 do begin // Iterate over every item within the cell
							ripItem(elementByIndex(cellChild, m), cellFormID);
						end;

						// Then do the same for temporary ones
						cellChild := FindChildGroup(ChildGroup(ElementByIndex(subBlock, l)), 9, ElementByIndex(subBlock, l));
						for m := 0 to ElementCount(cellChild) - 1 do begin // Iterate over every item within the cell
							ripItem(elementByIndex(cellChild, m), cellFormID);
						end;

					end;
				end;
			end;
		end;
	end;

	// Find valid worldspaces and pass them to the main worldspace rip func
	procedure ripWorldspaces();
	var
		i : Integer; // Iterator
		category, worldspace, spaceEditorID, spaceDisplayName : IInterface;
	begin
		category := GroupBySignature(targetESM, 'WRLD');
		for i := 0 to ElementCount(category) -1 do begin // Iterate over every worldspace within the worldspace category
			worldspace := elementByIndex(category, i);
			spaceEditorID := EditorID(worldspace);
			spaceDisplayName := sanitize(DisplayName(worldspace));
			if(FixedFormId(worldspace) <> 0) and (shouldProcessSpace(spaceDisplayName, spaceEditorID)) then begin
				ripWorldspace(spaceEditorID);
			end;
		end;
	end;

	procedure ripWorldspace(worldspaceEditorID: String); // Primary block for iterating down worldspace tree
	const
		worldspace = MainRecordByEditorID(GroupBySignature(FileByIndex(0), 'WRLD'), worldspaceEditorID);
		worldspaceID = IntToHex(FixedFormId(worldspace), 8);
		blocks = ChildGroup(worldspace);
		blockCount = ElementCount(blocks);
	var
		i, j, k, l : Integer; // Iterators
		block, subBlock, cell, cellItem : IInterface;
		friendlyBlockName : String;
	begin
		for i := 0 to blockCount - 1 do begin // Iterate over all blocks within the worldspace
			block := elementByIndex(blocks, i);
			friendlyBlockName := Trim(StringReplace(BaseName(block), 'GRUP', '', [rfReplaceAll]));

			for j := 0 to ElementCount(block) - 1 do begin // Iterate over all subBlocks within the block
				subBlock := elementByIndex(block, j);
				AddMessage(worldspaceEditorID + ' ' + friendlyBlockName + ' : ' + Trim(StringReplace(BaseName(subBlock), 'GRUP', '', [rfReplaceAll])));

				if(Signature(subBlock) = 'GRUP') then begin
					for k := 0 to ElementCount(subBlock) - 1 do begin // Iterate over all cells within the subBlock
						cell := FindChildGroup(ChildGroup(ElementByIndex(subBlock, k)), 9, ElementByIndex(subBlock, k));

						if(groupType(cell) = 9) then begin // Check that this isn't the persistent worldspace cell (which has a hierarchy one-less deep than normal worldspace blocks)
							for l := 0 to ElementCount(cell) - 1 do begin // Iterate over all cellElements within the cell
								cellItem := elementByIndex(cell, l);
								ripItem(cellItem, worldspaceID);
							end;
						end
						else begin // This is not a standard worldspace block (presumably the persistent worldspace cell)
							cell := elementByIndex(subBlock, k);
							ripItem(cell, worldspaceID);
						end;
					end;
				end;
			end;
		end;
	end;

	procedure ripItem(item : IInterface; spaceFormID : String);
	const
		displayName = DisplayName(item);
		shortName = ShortName(item);
		position = GetPosition(item);
	var
		primitiveEntry, boundsEntry : IInterface;
		primitiveShape, rotZ : String;
	begin
		if(Assigned(displayName) and Assigned(position) and (pos('<', displayName) <> 1)) then begin // Skip records missing key data
			primitiveEntry := ElementBySignature(item, 'XPRM');
			boundsEntry := ElementByName(primitiveEntry, 'Bounds');
			primitiveShape := GetEditValue(ElementByName(primitiveEntry, 'Type'));

			// We only need the Z Rotation for primitive shapes
			if(primitiveShape <> '') then begin
				rotZ := FloatToStr(GetRotation(item).z);
			end;

			outputStrings.Add(
				spaceFormID + ',' +
				sanitize(displayName) + ',' +
				FloatToStr(position.x) + ',' +
				FloatToStr(position.y) + ',' +
				FloatToStr(position.z) + ',' +
				sanitize(GetEditValue(ElementBySignature(item, 'XLCN'))) + ',' +
				GetEditValue(ElementByName(ElementBySignature(item, 'XLOC'), 'Level')) + ',' +
				primitiveShape + ',' +
				GetEditValue(ElementByName(boundsEntry, 'X')) + ',' +
				GetEditValue(ElementByName(boundsEntry, 'Y')) + ',' +
				GetEditValue(ElementByName(boundsEntry, 'Z')) + ',' +
				rotZ + ',' +
				GetEditValue(ElementByName(ElementByName(ElementByName(item, 'Map Marker'), 'TNAM - TNAM'), 'Type')) + ',' +
				sanitize(shortName));
		end;
	end;
end.
