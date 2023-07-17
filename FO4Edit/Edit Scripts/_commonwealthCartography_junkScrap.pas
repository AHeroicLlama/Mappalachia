// Gets a list of all MISC and which items in which quantities(KYWD) they give when scrapped
unit _commonwealthCartography_junkScrap;

	uses _commonwealthCartography_lib;

	var	outputStrings : TStringList;

	procedure Initialize;
	begin
		processRecordGroup('MISC', 'Junk_Scrap', 'component,componentQuantity,junkFormID');
	end;

	procedure ripItem(item : IInterface);
	const
		MCQPEntry = ElementBySignature(item, 'CVPA');
		formID = IntToHex(FixedFormId(item), 8);
	var
		i : Integer;
		currentComponent : IInterface;
	begin
		for i:= 0 to ElementCount(MCQPEntry) - 1 do begin
			currentComponent := ElementByName(MCQPEntry, 'Component #' + IntToStr(i));
			outputStrings.Add(
				GetEditValue(ElementByName(currentComponent, 'Component')) + ',' +
				GetEditValue(ElementByName(currentComponent, 'Count')) + ',' +
				formID
			);
		end;
	end;
end.
