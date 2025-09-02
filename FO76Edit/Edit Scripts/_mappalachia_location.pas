// Gets a list of all LCTN and their Actor Value properties (used to identify variable spawns)
// Header 'locationFormID,parent,minLevel,maxLevel,property,value'
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
		parentLocation = sanitize(GetEditValue(ElementBySignature(item, 'PNAM')));
		minLevel =	GetEditValue(ElementByName(ElementBySignature(item, 'DATA'), 'Min Location Level'));
		maxLevel = GetEditValue(ElementByName(ElementBySignature(item, 'DATA'), 'Max Location Level'));
	var
		i : Integer;
		currentProperty : IInterface;
	begin

		// Add one row with blank properties, so the min and max level are present for LCTN with no properties
		outputStrings.Add(
			formID + ',' +
			parentLocation + ',' +
			minLevel + ',' +
			maxLevel + ','
			+ ','
		);

		for i:= 0 to elementCount(PropertyEntry) - 1 do begin
			currentProperty := ElementByIndex(PropertyEntry, i);
			outputStrings.Add(
				formID + ',' +
				parentLocation + ',' +
				minLevel + ',' +
				maxLevel + ',' +
				sanitize(GetEditValue(ElementByName(currentProperty, 'Actor Value'))) + ',' +
				sanitize(GetEditValue(ElementByName(currentProperty, 'Value')))
			);
		end;
	end;
end.
