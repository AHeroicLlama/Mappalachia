// Run every Mappalachia script consecutively
unit Mappalachia;

uses
	_mappalachia_componentQuantity,
	_mappalachia_entityInfo,
	_mappalachia_junkScrap,
	_mappalachia_location,
	_mappalachia_positionData,
	_mappalachia_region,
	_mappalachia_spaceInfo;

	function initialize: Integer;
	begin
		AddMessage('Now running _mappalachia_componentQuantity...');
		_mappalachia_componentQuantity.initialize();

		AddMessage('Now running _mappalachia_junkScrap...');
		_mappalachia_junkScrap.initialize();

		AddMessage('Now running _mappalachia_location...');
		_mappalachia_location.initialize();

		AddMessage('Now running _mappalachia_region...');
		_mappalachia_region.initialize();

		AddMessage('Now running _mappalachia_spaceInfo...');
		_mappalachia_spaceInfo.initialize();

		AddMessage('Now running _mappalachia_entityInfo...');
		_mappalachia_entityInfo.initialize();

		AddMessage('Now running _mappalachia_positionData...');
		_mappalachia_positionData.initialize();

		AddMessage('Mappalachia export finished.');
	end;
end.
