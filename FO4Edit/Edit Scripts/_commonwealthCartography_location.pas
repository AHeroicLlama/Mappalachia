// Gets a list of all LCTN and their Actor Value properties (used to identify variable spawns)
unit _commonwealthCartography_location;

	uses _commonwealthCartography_lib;

	var	outputStrings : TStringList;

	procedure Initialize;
	begin
		processRecordGroup('LCTN', 'Location', 'property,value,locationFormID');
	end;

	procedure ripItem(item : IInterface);
	const
		KeywordEntry = ElementBySignature(item, 'KWDA');
		formID = IntToHex(FixedFormId(item), 8);
	var
		i, weight : Integer;
		value : String;
	begin
		for i:= 0 to elementCount(KeywordEntry) - 1 do begin
			value := GetEditValue(ElementByIndex(KeywordEntry, i));

			if (pos('LocEnc', value) = 1) then begin
				// Different from 76, 4 seems to have statically assigned spawn types with no weight given where fo76 has multiple weighted variable spawns
				// Lexington LCTN 00024FA8 seems to be the only place with 2 types (ghouls and raiders), although still no weight is given
				weight := 1;
				outputStrings.Add(value + ',' + IntToStr(weight) + ',' + formID);
			end;
		end;
	end;
end.
