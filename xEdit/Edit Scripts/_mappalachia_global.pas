// Gets all Globals
// Header 'EditorID,value'
unit _mappalachia_global;

	uses _mappalachia_lib;

	var	outputStrings : TStringList;

	procedure Initialize;
	begin
		processRecordGroup('GLOB', 'Global');
	end;

	procedure ripItem(item : IInterface);
	const
		floatValue = GetEditValue(ElementBySignature(item, 'FLTV'));
		editorID = EditorID(item);
	begin
		outputStrings.Add(
			editorID + ',' +
			floatValue
		);
	end;
end.
