// Run every Commonwealth Cartography script consecutively
unit CommonwealthCartography;

uses
	_commonwealthCartography_entityInfo,
	_commonwealthCartography_junkScrap,
	_commonwealthCartography_location,
	_commonwealthCartography_positionData,
	_commonwealthCartography_region,
	_commonwealthCartography_spaceInfo;

	function initialize: Integer;
	var
		i : Integer;
		temp : IInterface;
	begin
		for i := 0 to FileCount() -1 do begin
			esmNumber := i;
			targetESM := FileByIndex(esmNumber);

			fileName := GetFileName(targetESM);

			if (pos('.esm', fileName) = 0) then begin
				AddMessage('Skipping ' + fileName + ' - not an ESM');
				continue
			end;

			AddMessage('Exporting file ' + fileName + ' (#' + IntToStr(esmNumber) + ')...');

			AddMessage('Now running _commonwealthCartography_junkScrap...');
			_commonwealthCartography_junkScrap.initialize();

			AddMessage('Now running _commonwealthCartography_location...');
			_commonwealthCartography_location.initialize();

			AddMessage('Now running _commonwealthCartography_region...');
			_commonwealthCartography_region.initialize();

			AddMessage('Now running _commonwealthCartography_spaceInfo...');
			_commonwealthCartography_spaceInfo.initialize();

			AddMessage('Now running _commonwealthCartography_entityInfo...');
			_commonwealthCartography_entityInfo.initialize();

			AddMessage('Now running _commonwealthCartography_positionData...');
			_commonwealthCartography_positionData.initialize();
		end;

		AddMessage('Commonwealth Cartography export finished.');
	end;
end.
