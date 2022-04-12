// Gets a list of all CMPO and which component count is attributed to which quantity keyword
unit _mappalachia_componentQuantity;

	uses _mappalachia_lib;

	var	outputStrings : TStringList;

	procedure Initialize;
	begin
		// Headers are important for this script - the headers are looked up dynamically by the Preprocessor so must match the quantities by name in the ESM
		processRecordGroup('CMPO', 'Component_Quantity', 'component,Scrap Singular,Rare,Medium,Low,High,Bulk,Customer Service Bulk,Scrap Ball Level 2,Scrap Ball Level 3,Scrap Ball Level 1');
	end;

	procedure ripItem(item : IInterface);
	const
		numberOfScrapElements = 9;
	var
		i : Integer;
		output : String;
	begin
		output := sanitize(DisplayName(item))+ ',';

		for i := 0 to numberOfScrapElements do begin
			output := output + GetEditValue(ElementByName(ElementByIndex(ElementBySignature(item, 'CVPA'), i), 'Scrap Component Count'));

			// Avoid adding a comma after the last element
			if(i <> numberOfScrapElements) then begin
				output := output + ',';
			end;
		end;

		outputStrings.add(output);
	end;
end.
