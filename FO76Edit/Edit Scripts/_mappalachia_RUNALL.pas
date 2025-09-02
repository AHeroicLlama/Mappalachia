// Run every Mappalachia script consecutively
unit Mappalachia;

uses
	_mappalachia_component,
	_mappalachia_entity,
	_mappalachia_scrap,
	_mappalachia_location,
	_mappalachia_position,
	_mappalachia_region,
	_mappalachia_space;

	function initialize: Integer;
	begin
		AddMessage('Now running _mappalachia_space...');
		_mappalachia_space.initialize();

		AddMessage('Now running _mappalachia_location...');
		_mappalachia_location.initialize();

		AddMessage('Now running _mappalachia_region...');
		_mappalachia_region.initialize();

		AddMessage('Now running _mappalachia_scrap...');
		_mappalachia_scrap.initialize();

		AddMessage('Now running _mappalachia_component...');
		_mappalachia_component.initialize();

		AddMessage('Now running _mappalachia_entity...');
		_mappalachia_entity.initialize();

		AddMessage('Now running _mappalachia_position...');
		_mappalachia_position.initialize();

		AddMessage('Mappalachia export finished.');
	end;
end.
