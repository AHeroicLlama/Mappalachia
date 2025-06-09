// Rip every single entry in the ESM which is relevant for mapping. Gets each item's FormID, EditorID and displayName.
// This is later cross referenced between the location data to assign names/EditorID's to FormIDs in the location data
// CONT and LVLI are treated differently, as we export the items they contain
// Headers:
// Entity: 'entityFormID,displayName,editorID,signature'
// Container: 'containerFormID,contentsFormID,count'
// Leveled Item: 'parentFormID,childFormID,count'
unit _mappalachia_entity;

	uses _mappalachia_lib;

	var outputStrings, outputStringsContainer, outputStringsLeveledItem : TStringList;

	procedure Initialize;
	begin
		ripFormIDs();
	end;

	procedure ripFormIDs(); // Primary block for iterating down tree
	const
		outputFile = ProgramPath + 'Output\Entity.csv';
		outputFileContainer = ProgramPath + 'Output\Container.csv';
		outputFileLeveledItem = ProgramPath + 'Output\LeveledItem.csv';
	var
		i, j : Integer; // Iterators
		signatureGroup : IInterface;
		signature : String;
	begin
		outputStrings := TStringList.Create;
		outputStringsContainer := TStringList.Create;
		outputStringsLeveledItem := TStringList.Create;

		// Rip everything down to the end nodes of the hierarchy tree
		for i := 0 to ElementCount(targetESM) - 1 do begin
			signatureGroup := elementByIndex(targetESM, i);
			signature := StringReplace(BaseName(signatureGroup), 'GRUP Top ', '', [rfReplaceAll]);
			signature := StringReplace(signature, '"', '', [rfReplaceAll]); // Strip the category to its 4-char identifier

			// Don't export data for Cells or Worldspaces, as they won't contain themselves
			if (signature = 'CELL') or (signature = 'WRLD') then continue;

			AddMessage('Entity: ' + signature);

			for j := 0 to ElementCount(signatureGroup) -1 do begin
				ripItem(elementByIndex(signatureGroup, j), signature);
			end;
		end;

		createDir('Output');
		AddMessage('Writing output to file: ' + outputFile);
		AddMessage('Writing output to file: ' + outputFileContainer);
		AddMessage('Writing output to file: ' + outputFileLeveledItem);
		outputStrings.SaveToFile(outputFile);
		outputStringsContainer.SaveToFile(outputFileContainer);
		outputStringsLeveledItem.SaveToFile(outputFileLeveledItem);
	end;

	procedure ripItem(item : IInterface; signature : String);
	const
		editorId = EditorID(item);
		displayName = DisplayName(item);
	var
		i, j, k : Integer;
		containerItems, leveledList, containerItem, leveledItem, lvliBaseData, lvliReference, lvliCount : IInterface;
	begin
		if(FixedFormId(item) = 0) then begin // This is a GRUP and not an end-node, so pass each of its children back through
			for i := 0 to ElementCount(item) -1 do begin
				ripItem(elementByIndex(item, i), signature); // Recursive
			end;
		end
		else begin // This is an end-node
			if (signature = 'CONT') then begin
				containerItems := ElementByName(item, 'Items');

				for j := 0 to ElementCount(containerItems) -1 do begin
					containerItem := ElementBySignature(ElementByIndex(containerItems, j), 'CNTO');

					outputStringsContainer.Add(
						IntToStr(FixedFormId(item)) + ',' +
						GetEditValue(ElementByName(containerItem, 'Item')) + ',' +
						GetEditValue(ElementByName(containerItem, 'Count'))
					);
				end;
			end
			else if (signature = 'LVLI') then begin
				leveledList := ElementByName(item, 'Leveled List Entries');

				for k := 0 to ElementCount(leveledList) -1 do begin
					leveledItem := ElementByIndex(leveledList, k);
					lvliBaseData := ElementByName(ElementBySignature(leveledItem, 'LVLO'), 'Base Data');

					// Uses the "Base Data" structure
					if (not(Assigned(lvliBaseData))) then begin
						lvliReference := ElementByName(ElementBySignature(leveledItem, 'LVLO'), 'Reference');
						lvliCount := ElementBySignature(leveledItem, 'LVIV');
					end
					else begin
						lvliReference := ElementByName(lvliBaseData, 'Reference');
						lvliCount := ElementByName(lvliBaseData, 'Count');
					end;

					outputStringsLeveledItem.Add(
						IntToStr(FixedFormId(item)) + ',' +
						sanitize(GetEditValue(lvliReference)) + ',' +
						GetEditValue(lvliCount)
					);
				end;
			end;

			// The main entity export for all entities
			outputStrings.Add(
				IntToStr(FixedFormId(item)) + ',' +
				sanitize(displayName) + ',' +
				editorId + ',' +
				signature
			);
		end;
	end;
end.
