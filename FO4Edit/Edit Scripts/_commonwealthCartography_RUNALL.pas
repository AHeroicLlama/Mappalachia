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
	begin
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

		AddMessage('Commonwealth Cartography export finished.');
	end;
end.
