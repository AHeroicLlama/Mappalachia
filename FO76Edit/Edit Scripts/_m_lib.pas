//Mappalachia supporting functions - not to be run directly.
unit _m_lib;

	//Remove commas and replace them with something safe for CSV
	function sanitize(input: String): String;
	begin
		Result := StringReplace(input, ',', ':COMMA:', [rfReplaceAll])
	end;

	//Handle the pulling of every recrod from a given signature group and write it to the output file.
	//Calling script must provide its own ripItem method for handling each actual entry.
	//See: goToRipItem()
	function processRecordGroup(fileNum: Integer; signature, fileName, header: String): Integer;
	const
		targetESM = FileByIndex(fileNum);
		outputFile = ProgramPath + 'Output\' + StringReplace(BaseName(targetESM), '.esm', '', [rfReplaceAll]) + '_' + fileName + '.csv';
		category = GroupBySignature(targetESM, signature);
	var
		i : Integer;
	begin
		outputStrings := TStringList.Create;
		outputStrings.add(header); //Write CSV column headers

		for i := 0 to ElementCount(category) -1 do begin //Iterate over every item within the category
			goToRipItem(elementByIndex(category, i), signature);
		end;

		AddMessage('Writing output to file: ' + outputFile);
		createDir('Output');
		outputStrings.SaveToFile(outputFile);
	end;

	//The ripItem method exists for many units.
	//We need to correctly target the right version of the method.
	procedure goToRipItem(item: IInterface; signature: String);
	begin
			if(signature = 'MISC') then _m_junkScrap.ripItem(item)
		else if(signature = 'LCTN') then _m_location.ripItem(item)
		else if(signature = 'CMPO') then _m_componentQuantity.ripItem(item)
	end;

	//Do we need to process this interior cell, given its in-game name?
	//Something like 1/3 cell data are just leftover test/debug cells or otherwise inaccessible, so skipping them helps performance and data size
	function shouldProcessCellName(cellName: String): Boolean;
	begin
		if(cellName = '') then result := false
		else if(cellName = 'Quick Test Cell') then result := false

		else if(pos('Test', cellName) <> 0) then result := false
		else if(pos('Cell', cellName) <> 0) then result := false
		else if(pos('76', cellName) <> 0) then result := false
		else if(pos('Babylon', cellName) <> 0) then result := false

		else result := true;
	end;

	//Do we need to process this interior cell, given its editorID?
	function shouldProcessCellID(cellEditorID: String): Boolean;
	begin
		//Contains
		if(pos('Test', cellEditorID) <> 0) then result := false
		else if(pos('CUT', cellEditorID) <> 0) then result := false
		else if(pos('Delete', cellEditorID) <> 0) then result := false
		else if(pos('Debug', cellEditorID) <> 0) then result := false
		else if(pos('OLD', cellEditorID) <> 0) then result := false
		else if(pos('Unused', cellEditorID) <> 0) then result := false
		else if(pos('Proto', cellEditorID) <> 0) then result := false
		else if(pos('QA', cellEditorID) <> 0) then result := false

		//BEGINS with
		else if(pos('Warehouse', cellEditorID) = 1) then result := false

		else result := true;
	end;

	//Do we need to process this record, given its signature?
	//Something like 8/10 worldspace records aren't helpful to be mapped, so skipping them helps performance and data size massively
	function shouldProcessRecord(signature: String): Boolean;
	begin
			if(signature = 'STAT') then begin Exit(false) end //Explicitly return immediately if this is a STAT, as they're easily 90% of all records and we don't need them
		else if(signature = 'SCOL') then begin Exit(false) end

		else if(signature = 'LVLI') then result := true
		else if(signature = 'FLOR') then result := true
		else if(signature = 'MISC') then result := true
		else if(signature = 'ACTI') then result := true
		else if(signature = 'FURN') then result := true
		else if(signature = 'CONT') then result := true
		else if(signature = 'NPC_') then result := true
		else if(signature = 'DOOR') then result := true
		else if(signature = 'HAZD') then result := true
		else if(signature = 'BOOK') then result := true
		else if(signature = 'ALCH') then result := true
		else if(signature = 'TERM') then result := true
		else if(signature = 'NOTE') then result := true
		else if(signature = 'WEAP') then result := true
		else if(signature = 'ARMO') then result := true
		else if(signature = 'AMMO') then result := true
		else if(signature = 'TACT') then result := true
		else if(signature = 'KEYM') then result := true

		else result := false;
	end;

	//Find the signature of a referenced entity by parsing the reference
	//EG "PrewarMoney "Pre-War Money" [MISC:00059B02]" becomes "MISC"
	function sigFromRef(reference: String): String;
	begin
		result := copy(reference, pos('[', reference) + 1, 4);
	end;

	//Find the display name of a referenced entity by parsing the reference
	//EG "PrewarMoney "Pre-War Money" [MISC:00059B02]" becomes "Pre-War Money"
	function nameFromRef(reference: String): String;
	const
		len = Length(reference);
		firstPos = pos('"', reference) + 1;
		firstSubStr = copy(reference, firstPos, len - firstPos);
		secondPos = pos('"', firstSubStr) - 1;
	begin
		result := copy(reference, firstPos, secondPos);
	end;

	//Finds a representative name for LVLIs without a displayName, by referring to their leveled lists
	function getNameforLvli(item: IInterface): String;
	const
		leveledListEntries = ElementByName(item, 'Leveled List Entries');
	var
		i : integer;
	begin
		result := '';

		//If this already has a display name, just use that
		if (DisplayName(item) <> '') then begin
			exit (DisplayName(item));
		end;

		//This item has just one entry in the leveled item list - we can only look here for a name.
		if(DisplayName(item) = '') and (ElementCount(leveledListEntries) = 1) then begin
			//Find the first and only entry and look under LVLO/Reference
			result := nameFromRef(GetEditValue(ElementByName(ElementByName(ElementByIndex(leveledListEntries, 0), 'LVLO - LVLO'), 'Reference')));

			//If no item was found, try looking under LVLO/Base Data/Reference
			if(result = '') then begin
				result := nameFromRef(GetEditValue(ElementByName(ElementByName(ElementByName(ElementByIndex(leveledListEntries, 0), 'LVLO - LVLO'), 'Base Data'), 'Reference')));
			end;

			//We've looked everywhere - return
			exit(result);
		end;

		//This item must have multiple leveled item entries - while this normally means the list has multiple different items...
		//It would seem that Flora or Vein items *can* provide a representative name since they only give themselves or a nuked version...
		//So - starting with the bottom item, work upwards until we find a name
		if(DisplayName(item) = '') and ((pos('LPI_Flora', EditorID(item)) <> 0) or (pos('LPI_Vein', EditorID(item)) <> 0)) then begin
			for i := ElementCount(leveledListEntries) downto 0 do begin
				//Find the end of the leveled item list, directly under LVLO/Reference
				result := nameFromRef(GetEditValue(ElementByName(ElementByName(ElementByIndex(leveledListEntries, i), 'LVLO - LVLO'), 'Reference')));

				//If no item was found, try looking under LVLO/Base Data/Reference
				if(result = '') then begin
					result := nameFromRef(GetEditValue(ElementByName(ElementByName(ElementByName(ElementByIndex(leveledListEntries, i), 'LVLO - LVLO'), 'Base Data'), 'Reference')));
				end;

				//If we found an item then return, otherwise continue looping
				if(result <> '') then begin
					exit(result);
				end;
			end;
		end;
	end;

end.