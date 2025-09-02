// Gets a list of all CMPO and which component count is attributed to which quantity keyword
// Header 'component,Scrap Singular,Rare,Medium,Low,High,Bulk'
unit _mappalachia_component;

	uses _mappalachia_lib;

	var	outputStrings : TStringList;

	procedure Initialize;
	begin
		processRecordGroup('CMPO', 'Component');
	end;

	procedure ripItem(item : IInterface);
	const
		numberOfScrapElements = 5;
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
