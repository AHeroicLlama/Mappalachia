// Gets a list of all LCTN and their Actor Value properties (used to identify variable spawns)
// Header 'locationFormID,property,value'
unit _mappalachia_location;

	uses _mappalachia_lib;

	var	outputStrings : TStringList;

	procedure Initialize;
	begin
		processRecordGroup('LCTN', 'Location');
	end;

	procedure ripItem(item : IInterface);
	const
		PropertyEntry = ElementBySignature(item, 'PRPS');
		formID = IntToStr(FixedFormId(item));
	var
		i : Integer;
		currentProperty : IInterface;
	begin
		for i:= 0 to elementCount(PropertyEntry) - 1 do begin
			currentProperty := ElementByIndex(PropertyEntry, i);
			outputStrings.Add(
				formID + ',' +
				sanitize(GetEditValue(ElementByName(currentProperty, 'Actor Value'))) + ',' +
				sanitize(GetEditValue(ElementByName(currentProperty, 'Value')))
			);
		end;
	end;
end.
