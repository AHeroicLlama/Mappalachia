//Gets a list of all LCTN and their Actor Value properties (used to identify variable spawns)
{Expected runtime: <1 sec}
unit _m_location;

	uses _m_lib;

	var	outputStrings : TStringList;

	procedure Initialize;
	begin
		processRecordGroup(0, 'LCTN', 'Location', 'property,value,locationFormID');
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