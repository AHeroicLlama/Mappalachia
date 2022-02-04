// Gets a list of all LCTN and their Actor Value properties (used to identify variable spawns)
unit _mappalachia_location;

	uses _mappalachia_lib;

	var	outputStrings : TStringList;

	procedure Initialize;
	begin
		processRecordGroup('LCTN', 'Location', 'property,value,locationFormID');
	end;

	procedure ripItem(item : IInterface);
	const
		PropertyEntry = ElementBySignature(item, 'PRPS');
		formID = IntToHex(FixedFormId(item), 8);
	var
		i : Integer;
		currentProperty : IInterface;
	begin
		for i:= 0 to elementCount(PropertyEntry) - 1 do begin
			currentProperty := ElementByIndex(PropertyEntry, i);
			outputStrings.Add(
				GetEditValue(ElementByName(currentProperty, 'Actor Value')) + ',' +
				GetEditValue(ElementByName(currentProperty, 'Value')) + ',' +
				formID
			);
		end;
	end;
end.
