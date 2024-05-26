// Mappalachia supporting functions - not to be run directly.
unit _mappalachia_lib;

	const targetESM = FileByIndex(0);

	// Remove commas and replace them with something safe for CSV
	function sanitize(input: String): String;
	begin
		Result := StringReplace(input, ',', ':COMMA:', [rfReplaceAll])
	end;

	// Handle the pulling of every recrod from a given signature group and write it to the output file.
	// Calling script must provide its own ripItem method for handling each actual entry.
	// See: goToRipItem()
	function processRecordGroup(signature, fileName, header: String): Integer;
	const
		outputFile = ProgramPath + 'Output\' + fileName + '.csv';
		category = GroupBySignature(targetESM, signature);
	var
		i : Integer;
	begin
		outputStrings := TStringList.Create;
		outputStrings.add(header); // Write CSV column headers

		for i := 0 to ElementCount(category) -1 do begin // Iterate over every item within the category
			goToRipItem(elementByIndex(category, i), signature);
		end;

		AddMessage('Writing output to file: ' + outputFile);
		createDir('Output');
		outputStrings.SaveToFile(outputFile);
	end;

	// The ripItem method exists for many units.
	// We need to correctly target the right version of the method.
	procedure goToRipItem(item: IInterface; signature: String);
	begin
			if(signature = 'MISC') then _mappalachia_junkScrap.ripItem(item)
		else if(signature = 'LCTN') then _mappalachia_location.ripItem(item)
		else if(signature = 'CMPO') then _mappalachia_componentQuantity.ripItem(item)
		else if(signature = 'REGN') then _mappalachia_region.ripItem(item)
	end;

	// Do we need to process this interior space, given its in-game name or editorID?
	// Something like 1/3 space data are just leftover test/debug spaces or otherwise inaccessible, so skipping them helps performance and data size
	function shouldProcessSpace(spaceName, spaceEditorID: String): Boolean;
	begin
		if 	(spaceName = '') or
			(spaceEditorID = '') or
			(spaceName = 'Quick Test Cell') or

			(pos('PackIn', spaceEditorID) <> 0) or
			(pos('COPY', spaceEditorID) <> 0) or
			(pos('zCUT', spaceEditorID) <> 0) or
			((pos('Cell', spaceEditorID) <> 0) and (pos('Cellar', spaceEditorID) = 0)) or // 'Cell' but not 'Cellar'
			(pos('Test', spaceEditorID) <> 0) or
			(pos('Holding', spaceEditorID) <> 0) or
			(pos('Debug', spaceEditorID) <> 0) or
			(pos('OLD', spaceEditorID) <> 0) or
			(pos('Proto', spaceEditorID) <> 0) or
			(pos('Unused', spaceEditorID) <> 0) or
			(pos('QA', spaceEditorID) <> 0) or
			(pos('Smoke', spaceEditorID) <> 0) or

			(pos('Test', spaceName) <> 0) or
			((pos('Cell', spaceName) <> 0) and (pos('Cellar', spaceName) = 0)) or // 'Cell' but not 'Cellar'
			(pos('Debug', spaceName) <> 0) or

			(pos('Goodneighbor', spaceEditorID) <> 0) or
			(pos('DiamondCity', spaceEditorID) <> 0) or

			(pos('Warehouse', spaceEditorID) = 1)
		then begin
			result := false
		end
		else result := true;
	end;

	// Find the display name of a referenced entity by parsing the reference
	// EG "PrewarMoney "Pre-War Money" [MISC:00059B02]" becomes "Pre-War Money"
	function nameFromRef(reference: String): String;
	const
		len = Length(reference);
		firstPos = pos('"', reference) + 1;
		firstSubStr = copy(reference, firstPos, len - firstPos);
		secondPos = pos('"', firstSubStr) - 1;
	begin
		result := copy(reference, firstPos, secondPos);
	end;

	// Find the FormID of a referenced WRLD by parsing the edit value
	// EG "Appalachia "Appalachia" [WRLD:0025DA15]" becomes "0025DA15"
	function wrldFormIdFromRef(reference: String): String;
	const
		len = Length(reference);
		firstPos = pos('[WRLD:', reference) + 6;
		firstSubStr = copy(reference, firstPos, len - firstPos);
		secondPos = 8; //FormID length
	begin
		result := copy(reference, firstPos, secondPos);
	end;

	// Is this signature one we expect to see in the world, and therefore worth processing?
	function shouldProcessSig(sig: String): Boolean;
	begin
		if (sig = 'ACTI') or
			(sig = 'ALCH') or
			(sig = 'AMMO') or
			(sig = 'ARMO') or
			(sig = 'ASPC') or
			(sig = 'BNDS') or
			(sig = 'BOOK') or
			(sig = 'CNCY') or
			(sig = 'CONT') or
			(sig = 'DOOR') or
			(sig = 'FLOR') or
			(sig = 'FURN') or
			(sig = 'HAZD') or
			(sig = 'IDLM') or
			(sig = 'KEYM') or
			(sig = 'LIGH') or
			(sig = 'LVLI') or
			(sig = 'MISC') or
			(sig = 'MSTT') or
			(sig = 'NOTE') or
			(sig = 'NPC_') or
			(sig = 'PROJ') or
			(sig = 'SCOL') or
			(sig = 'SECH') or
			(sig = 'SOUN') or
			(sig = 'STAT') or
			(sig = 'TACT') or
			(sig = 'TERM') or
			(sig = 'TXST') or
			(sig = 'WEAP')
		then begin
			result := true
		end
		else result := false;
	end;

	// Finds a representative name for LVLIs without a displayName, by referring to their leveled lists
	function getNameforLvli(item: IInterface): String;
	const
		leveledListEntries = ElementByName(item, 'Leveled List Entries');
	var
		i : integer;
	begin
		result := '';

		// If this already has a display name, just use that
		if (DisplayName(item) <> '') then begin
			exit (DisplayName(item));
		end;

		// This item has just one entry in the leveled item list - we can only look here for a name.
		if(DisplayName(item) = '') and (ElementCount(leveledListEntries) = 1) then begin
			// Find the first and only entry and look under LVLO/Reference
			result := nameFromRef(GetEditValue(ElementByName(ElementByName(ElementByIndex(leveledListEntries, 0), 'LVLO - LVLO'), 'Reference')));

			// If no item was found, try looking under LVLO/Base Data/Reference
			if(result = '') then begin
				result := nameFromRef(GetEditValue(ElementByName(ElementByName(ElementByName(ElementByIndex(leveledListEntries, 0), 'LVLO - LVLO'), 'Base Data'), 'Reference')));
			end;

			// We've looked everywhere - return
			exit(result);
		end;

		// This item must have multiple leveled item entries - while this normally means the list has multiple different items...
		// It would seem that Flora or Vein items *can* provide a representative name since they only give themselves or a nuked version...
		// So - starting with the bottom item, work upwards until we find a name
		if(DisplayName(item) = '') and ((pos('LPI_Flora', EditorID(item)) <> 0) or (pos('LPI_Vein', EditorID(item)) <> 0)) then begin
			for i := ElementCount(leveledListEntries) - 1 downto 0 do begin
				// Find the end of the leveled item list, directly under LVLO/Reference
				result := nameFromRef(GetEditValue(ElementByName(ElementByName(ElementByIndex(leveledListEntries, i), 'LVLO - LVLO'), 'Reference')));

				// If no item was found, try looking under LVLO/Base Data/Reference
				if(result = '') then begin
					result := nameFromRef(GetEditValue(ElementByName(ElementByName(ElementByName(ElementByIndex(leveledListEntries, i), 'LVLO - LVLO'), 'Base Data'), 'Reference')));
				end;

				// If we found an item then return, otherwise continue looping
				if(result <> '') then begin
					exit(result);
				end;
			end;
		end;
	end;

	// Hardcoded and not datamined
	function getFluxColor(editorID: String): String;
	begin
		if	(editorID = 'LPI_FloraAster01') or
			(editorID = 'LPI_FloraBlight01') or
			(editorID = 'LPI_FloraFungusBrain01') or
			(editorID = 'LPI_FloraFungusBrain02') or
			(editorID = 'LPI_FloraFungusBrain03') or
			(editorID = 'LPI_FloraCranberry01') or
			(editorID = 'LPI_FloraCranberry02') or
			(editorID = 'LPI_FloraCranberry03') or
			(editorID = 'LPI_FloraCranberry01Diseased') or
			(editorID = 'LPI_FloraCranberry02Diseased') or
			(editorID = 'LPI_FloraCranberry03Diseased') or
			(editorID = 'LPI_FloraFeverBlossom01') or
			(editorID = 'LPI_FloraFirecap01') or
			(editorID = 'LPI_FloraWildGourdVine01') or
			(editorID = 'LPI_FloraMothmanEggs01') or
			(editorID = 'LPI_FloraMothmanEggs02') or
			(editorID = 'LPI_FloraMothmanEggs03')
		then begin
			result := 'Crimson';
		end

		else if
			(editorID = 'LPI_FloraBloodLeaf01') or
			(editorID = 'LPI_FloraBloodLeaf02') or
			(editorID = 'LPI_FloraWildCornStalk01') or
			(editorID = 'LPI_FloraFirecracker01') or
			(editorID = 'LPI_FloraFirecracker02') or
			(editorID = 'LPI_FloraSap01') or
			(editorID = 'LPI_FloraSap02') or
			(editorID = 'LPI_FloraWildMutFruit') or
			(editorID = 'LPI_FloraSiltBean01') or
			(editorID = 'LPI_FloraSiltBean02') or
			(editorID = 'LPI_FloraStarlightCreeper01') or
			(editorID = 'LPI_FloraStarlightCreeper02')
		then begin
			result := 'Cobalt';
		end

		else if
			(editorID = 'LPI_FloraFungusGlowing01') or
			(editorID = 'LPI_FloraFungusGlowing02') or
			(editorID = 'LPI_FloraFungusGlowing03') or
			(editorID = 'LPI_FloraFungusGlowing04') or
			(editorID = 'LPI_FloraFungusGlowing05') or
			(editorID = 'LPI_FloraFungusGlowing06') or
			(editorID = 'LPI_FloraRhododendron01') or
			(editorID = 'LPI_TrapFloraThistle')
		then begin
			result := 'Fluorescent';
		end

		else if
			(editorID = 'LPI_FloraPitcherPlant') or
			(editorID = 'LPI_FloraGinseng01') or
			(editorID = 'LPI_FloraFern01') or
			(editorID = 'LPI_FloraFern02') or
			(editorID = 'LPI_FloraFern01_Charred01') or
			(editorID = 'LPI_FloraFern01_Charred02') or
			(editorID = 'LPI_FloraFern01_Charred03') or
			(editorID = 'LPI_FloraFern01_Charred04') or
			(editorID = 'LPI_FloraFern02_Charred01') or
			(editorID = 'LPI_FloraFern02_Charred02') or
			(editorID = 'LPI_FloraFern02_Charred03') or
			(editorID = 'LPI_FloraFern02_Charred04') or
			(editorID = 'LPI_FloraWildRazorgrain01') or
			(editorID = 'LPI_FloraSnapTail01') or
			(editorID = 'LPI_FloraSnapTail02') or
			(editorID = 'LPI_FloraSwampPod01') or
			(editorID = 'LPI_FloraWildTarberryFloat01') or
			(editorID = 'LPI_FloraWildTarberryFloat02') or
			(editorID = 'LPI_FloraWildTatoPlant01') or
			(editorID = 'LPI_FloraWildTatoPlant02')
		then begin
			result := 'Violet';
		end

		else if
			(editorID = 'LPI_FloraAshRose01') or
			(editorID = 'LPI_FloraAshRose02') or
			(editorID = 'LPI_FloraBleachDogwoodBark01') or
			(editorID = 'LPI_FloraBleachDogwoodBark02') or
			(editorID = 'LPI_FloraBleachDogwoodBark03') or
			(editorID = 'LPI_FloraWildCarrotFlower') or
			(editorID = 'LPI_FloraWildMelonVine01') or
			(editorID = 'LPI_FloraSootFlower01') or
			(editorID = 'LPI_FloraSootFlower02') or
			(editorID = 'LPI_FloraToxicSootFlower01') or
			(editorID = 'LPI_FloraToxicSootFlower02') or
			(editorID = 'LPI_FloraBlackberry01') or
			(editorID = 'LPI_FloraBlackberry02')
		then begin
			result := 'Yellowcake';
		end

		else result := '';
	end;
end.
